using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using Microsoft.OpenApi.Models;
using Blog.Domain.Entities;
using Blog.Services.Interfaces;
using Blog.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Blog.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<BlogContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("Blog")));
            //options.UseSqlServer(Configuration.GetConnectionString("Blog")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<BlogContext>();

            services.AddAuthentication("Bearer")
                    .AddIdentityServerAuthentication(options =>
                    {
                        options.Authority = Configuration.GetValue<string>("AuthorityUrl");
                        options.RequireHttpsMetadata = false;
                        options.ApiName = "api1";
                        options.RoleClaimType = "role";
                    });

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Blog Api",
                    Version = "v1",
                });
            });

            ConfigureServiceLayer(services);

        }

        public void ConfigureServiceLayer(IServiceCollection services)
        {
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IUserProfileService, UserProfileService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCors(x => x.AllowAnyOrigin()
                .WithMethods("POST", "GET", "PUT", "DELETE")
                .AllowAnyHeader()
            );

            app.UseRouting();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog API V1");
            });

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
