using Blog.Domain.DTO;
using Blog.Domain.Entities;
using Blog.Persistence;
using Blog.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Blog.Services.Tests.Tests
{
    public class PostServiceTests : IDisposable
    {
        public BlogContext _blogContext;
        public Mock<IConfiguration> _configuration;
        public PostService _postService;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;

        public PostServiceTests()
        {
            var myDatabaseName = "mydatabase_" + DateTime.Now.ToFileTimeUtc();
            var options = new DbContextOptionsBuilder<BlogContext>()
                .UseInMemoryDatabase(databaseName: myDatabaseName)
                .Options;

            _blogContext = new BlogContext(options);
            _configuration = new Mock<IConfiguration>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _postService = new PostService(_blogContext, _httpContextAccessor.Object);
        }

        public void Dispose()
        {
            _blogContext.Dispose();
        }

        [Fact]
        public async Task SavePost_add_new_post_success()
        {
            //arrange
            var fileUploadId = Guid.NewGuid();
            var user = new UserProfile
            {
                Email = "test@mail.com",
                PenName = "test",
            };

            var category = new Category
            {
                Name = "category test",
            };

            await _blogContext.AddAsync(user);
            await _blogContext.AddAsync(category);
            await _blogContext.SaveChangesAsync();

            var categoryFromDb = await _blogContext.Categories.FirstOrDefaultAsync(c => c.Name == "category test");

            var postDto = new PostDto
            {
                Id = null,
                Content = "test",
                Title = "test",
                ShortDescription = "test",
                Tags = new List<string> { "tag1" },
                CategoryId = categoryFromDb.Id.ToString(),
                IsPublished = true,
                FileUploadId = fileUploadId,
            };

            _httpContextAccessor.Setup(x => x.HttpContext.User.Identity.Name).Returns("test@mail.com");

            //act
            await _postService.SavePost(postDto);

            //assert
            var newPost = await _blogContext.Posts.FirstOrDefaultAsync(u => u.Title == "test");
            var newTags = await _blogContext.Tags.ToListAsync();
            Assert.Equal("test", newPost.Title);
            Assert.Equal("test", newPost.Content);
            Assert.Equal("test", newPost.ShortDescription);
            Assert.Equal("tag1", newTags[0].Name);
            Assert.Equal(categoryFromDb.Id, newPost.CategoryId);
            Assert.True(newPost.IsPublished);
            Assert.Equal(fileUploadId, newPost.FileUploadId);
        }

        [Fact]
        public async Task SavePost_edit_post_success()
        {
            //arrange
            var fileUploadId = Guid.NewGuid();
            var user = new UserProfile
            {
                Email = "test@mail.com",
                PenName = "test",
            };

            var category = new Category
            {
                Name = "category test",
            };

            var post = new Post();

            await _blogContext.AddAsync(user);
            await _blogContext.AddAsync(category);
            await _blogContext.AddAsync(post);
            await _blogContext.SaveChangesAsync();

            var categoryFromDb = await _blogContext.Categories.FirstOrDefaultAsync(c => c.Name == "category test");
            var postFromDb = await _blogContext.Posts.FirstOrDefaultAsync();

            var postDto = new PostDto
            {
                Id = postFromDb.Id,
                Content = "test",
                Title = "test",
                ShortDescription = "test",
                Tags = new List<string> { "tag1" },
                CategoryId = categoryFromDb.Id.ToString(),
                IsPublished = true,
                FileUploadId = fileUploadId,
            };

            _httpContextAccessor.Setup(x => x.HttpContext.User.Identity.Name).Returns("test@mail.com");

            //act
            await _postService.SavePost(postDto);

            //assert
            var newPost = await _blogContext.Posts.FirstOrDefaultAsync(u => u.Title == "test");
            var newTags = await _blogContext.Tags.ToListAsync();
            Assert.Equal("test", newPost.Title);
            Assert.Equal("test", newPost.Content);
            Assert.Equal("test", newPost.ShortDescription);
            Assert.Equal("tag1", newTags[0].Name);
            Assert.Equal(categoryFromDb.Id, newPost.CategoryId);
            Assert.True(newPost.IsPublished);
            Assert.Equal(fileUploadId, newPost.FileUploadId);
        }

        [Fact]
        public async Task GetPost_return_success()
        {
            //arrange
            var post = new Post();

            await _blogContext.AddAsync(post);
            await _blogContext.SaveChangesAsync();

            var postFromDb = await _blogContext.Posts.FirstOrDefaultAsync();

            //act
            var result = await _postService.GetPost(postFromDb.Id);

            //assert
            Assert.Equal(postFromDb.Id, result.Id);
        }

        [Fact]
        public async Task GetPosts_return_success()
        {
            //arrange
            var user = new UserProfile
            {
                Email = "test@mail.com",
                PenName = "test",
            };
            await _blogContext.AddAsync(user);
            await _blogContext.SaveChangesAsync();

            var userFromDb = await _blogContext.UserProfiles.FirstOrDefaultAsync();

            var notPublishedPost = new Post 
            { 
                Title = "Not Published",
                IsPublished = false,
                AuthorId = userFromDb.Id,
                CreatedDate = DateTime.Now,
            };

            var publishedPost = new Post
            {
                Title = "Published",
                IsPublished = true,
                AuthorId = userFromDb.Id,
                CreatedDate = DateTime.Now.AddDays(-1),
            };

            var firstPublishedPost = new Post
            {
                Title = "Published Now",
                IsPublished = true,
                AuthorId = userFromDb.Id,
                CreatedDate = DateTime.Now,
            };

            await _blogContext.AddAsync(notPublishedPost);
            await _blogContext.AddAsync(publishedPost);
            await _blogContext.AddAsync(firstPublishedPost);
            await _blogContext.SaveChangesAsync();

            var notPublishedPostFromDb = await _blogContext.Posts.FirstOrDefaultAsync(p => !p.IsPublished);
            var firstPublishedPostFromDb = await _blogContext.Posts.FirstOrDefaultAsync(p => p.Title == "Published Now" && p.IsPublished);

            //act
            var result = await _postService.GetPosts(0);

            //assert
            Assert.Equal("Published Now", result[0].Title);
        }

        [Fact]
        public async Task HasMorePosts_return_success()
        {
            //arrange
            var publishedPost = new Post
            {
                Title = "Published",
                IsPublished = true,
            };

            await _blogContext.AddAsync(publishedPost);
            await _blogContext.SaveChangesAsync();

            //act
            var result = await _postService.HasMorePosts(0);

            //assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeletePost_success()
        {
            //arrange
            var publishedPost = new Post
            {
                Title = "Published",
                IsPublished = true,
            };

            await _blogContext.AddAsync(publishedPost);
            await _blogContext.SaveChangesAsync();

            var deletePost = await _blogContext.Posts.FirstOrDefaultAsync();
            var deletePostId = deletePost.Id;
            //act
            await _postService.DeletePost(deletePostId);

            //assert
            var postFromDb = await _blogContext.Posts.FirstOrDefaultAsync(p => p.Id == deletePostId);

            Assert.Null(postFromDb);
        }
    }
}
