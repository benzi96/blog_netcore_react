using Blog.Domain.DTO;
using Blog.Domain.Entities;
using Blog.Persistence;
using Blog.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Blog.Services.Tests.Tests
{
    public class UserProfileServiceTests : IDisposable
    {
        public BlogContext _blogContext;
        public Mock<IConfiguration> _configuration;
        public UserProfileService _userProfileService;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;

        public UserProfileServiceTests()
        {
            var myDatabaseName = "mydatabase_" + DateTime.Now.ToFileTimeUtc();
            var options = new DbContextOptionsBuilder<BlogContext>()
                .UseInMemoryDatabase(databaseName: myDatabaseName)
                .Options;

            _blogContext = new BlogContext(options);
            _configuration = new Mock<IConfiguration>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _userProfileService = new UserProfileService(_blogContext, _httpContextAccessor.Object);
        }

        public void Dispose()
        {
            _blogContext.Dispose();
        }

        [Fact]
        public async Task SaveUserProfile_add_new_user_success()
        {
            //arrange
            var userProfileDto = new UserProfileDto
            {
                PenName = "test",
            };

            _httpContextAccessor.Setup(x => x.HttpContext.User.Identity.Name).Returns("test@mail.com");

            //act
            await _userProfileService.SaveUserProfile(userProfileDto);

            //assert
            var newUser = await _blogContext.UserProfiles.FirstOrDefaultAsync(u => u.Email == "test@mail.com");
            Assert.Equal("test", newUser.PenName);
        }

        [Fact]
        public async Task SaveUserProfile_edit_user_success()
        {
            //arrange
            var userProfileDto = new UserProfileDto
            {
                PenName = "test1",
            };

            _httpContextAccessor.Setup(x => x.HttpContext.User.Identity.Name).Returns("test@mail.com");

            var user = new UserProfile
            {
                Email = "test@mail.com",
                PenName = "test",
            };

            await _blogContext.AddAsync(user);
            await _blogContext.SaveChangesAsync();

            //act
            await _userProfileService.SaveUserProfile(userProfileDto);

            //assert
            var newUser = await _blogContext.UserProfiles.FirstOrDefaultAsync(u => u.Email == "test@mail.com");
            Assert.Equal("test1", newUser.PenName);
        }

        [Fact]
        public async Task GetUserProfile_return_success()
        {
            //arrange
            _httpContextAccessor.Setup(x => x.HttpContext.User.Identity.Name).Returns("test@mail.com");

            var user = new UserProfile
            {
                Email = "test@mail.com",
                PenName = "test",
            };

            await _blogContext.AddAsync(user);
            await _blogContext.SaveChangesAsync();

            //act
            var result = await _userProfileService.GetUserProfile();

            //assert
            Assert.Equal("test", result.PenName);
        }
    }
}
