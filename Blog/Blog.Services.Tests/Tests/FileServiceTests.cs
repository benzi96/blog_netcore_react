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
    public class FileServiceTests : IDisposable
    {
        public BlogContext _blogContext;
        public Mock<IConfiguration> _configuration;
        public FileService _fileService;
        public FileServiceTests()
        {
            var myDatabaseName = "mydatabase_" + DateTime.Now.ToFileTimeUtc();
            var options = new DbContextOptionsBuilder<BlogContext>()
                .UseInMemoryDatabase(databaseName: myDatabaseName)
                .Options;

            _blogContext = new BlogContext(options);
            _configuration = new Mock<IConfiguration>();
            _fileService = new FileService(_blogContext, _configuration.Object);
        }

        public void Dispose()
        {
            _blogContext.Dispose();
        }

        [Fact]
        public async Task GetFilePath_return_success()
        {
            //arrange
            var fileName = $"{Guid.NewGuid()}.jpg";
            var fileUpload = new FileUpload
            {
                FileName = fileName
            };

            _blogContext.Add(fileUpload);
            _blogContext.SaveChanges();

            var storedFilesPath = "D://test";
            _configuration.Setup(c => c["StoredFilesPath"]).Returns(storedFilesPath);

            //act
            var result = await _fileService.GetFilePathById(fileUpload.Id);

            //assert
            Assert.Equal(Path.Combine(storedFilesPath, fileName), result);
        }
    }
}
