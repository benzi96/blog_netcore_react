using Blog.Domain.Entities;
using Blog.Persistence;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly BlogContext _blogContext;
        private readonly IConfiguration _config;

        public CategoryService(BlogContext blogContext, IConfiguration config)
        {
            _blogContext = blogContext;
            _config = config;
        }

        public async Task<List<Category>> GetCategories()
        {
            return await _blogContext.Categories.ToListAsync();
        }
    }
}
