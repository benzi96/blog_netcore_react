using Blog.Domain.Entities;
using Blog.Persistence;
using Blog.Services.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Blog.Services.Tests.Tests
{
    public class CategoryServiceTests : IDisposable
    {
        public BlogContext _blogContext;
        public Mock<IConfiguration> _configuration;
        public CategoryService _categoryService;
        public CategoryServiceTests()
        {
            var myDatabaseName = "mydatabase_" + DateTime.Now.ToFileTimeUtc();
            var options = new DbContextOptionsBuilder<BlogContext>()
                .UseInMemoryDatabase(databaseName: myDatabaseName)
                .Options;

            _blogContext = new BlogContext(options);
            _configuration = new Mock<IConfiguration>();
            _categoryService = new CategoryService(_blogContext, _configuration.Object);
        }

        public void Dispose()
        {
            _blogContext.Dispose();
        }

        [Fact]
        public async Task GetCategories_return_success()
        {
            //arrange
            var category = new Category
            {
                Name = "test",
            };

            _blogContext.Add(category);
            _blogContext.SaveChanges();

            //act
            var result = await _categoryService.GetCategories();

            //assert
            Assert.Equal("test", result[0].Name);
        }
    }
}
