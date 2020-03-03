using Blog.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Blog.Identity.Persistence
{
    public class BlogIdentityContext : IdentityDbContext<ApplicationUser>
    {
        public BlogIdentityContext(DbContextOptions<BlogIdentityContext> options) : base(options)
        {
        }
    }
}
