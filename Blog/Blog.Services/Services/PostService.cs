using Blog.Domain.DTO;
using Blog.Domain.Entities;
using Blog.Persistence;
using Blog.Services.Extensions;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Services
{
    public class PostService : IPostService
    {
        private readonly BlogContext _blogContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostService(BlogContext blogContext, IHttpContextAccessor httpContextAccessor)
        {
            _blogContext = blogContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SavePost(PostDto postDto)
        {
            var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
            var author = await _blogContext.UserProfiles.FirstOrDefaultAsync(u => u.Email == userName);
            var insertedTags = await _blogContext.Tags.Where(c => postDto.Tags.Contains(c.Name)).ToListAsync();
            var newTags = postDto.Tags.Except(insertedTags.Select(c => c.Name)).Select(x => new Tag { Name = x });
            if (!postDto.Id.HasValue)
            {
                var newPost = new Post
                {
                    Content = postDto.Content,
                    Title = postDto.Title,
                    ShortDescription = postDto.ShortDescription,
                    UrlSlug = postDto.Title.GenerateSlug(),
                    CategoryId = Guid.Parse(postDto.CategoryId),
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    IsPublished = postDto.IsPublished,
                    FileUploadId = postDto.FileUploadId,
                    AuthorId = author.Id
                };

                await _blogContext.AddAsync(newPost);
                await _blogContext.AddRangeAsync(newTags);
                await _blogContext.SaveChangesAsync();

                var postTags = new List<PostTag>();
                foreach (var newTag in newTags)
                {
                    var dbNewTag = await _blogContext.Tags.FirstOrDefaultAsync(t => t.Name == newTag.Name);
                    postTags.Add(new PostTag { PostId = newPost.Id, TagId = dbNewTag.Id });
                }

                foreach (var insertedTag in insertedTags)
                {
                    postTags.Add(new PostTag { PostId = newPost.Id, TagId = insertedTag.Id });
                }

                
                await _blogContext.AddRangeAsync(postTags);
            }
            else
            {
                var post = await _blogContext.Posts.Where(p => p.Id == postDto.Id.Value).FirstOrDefaultAsync();
                post.Content = postDto.Content;
                post.Title = postDto.Title;
                post.ShortDescription = postDto.ShortDescription;
                post.UrlSlug = postDto.Title.GenerateSlug();
                post.UpdatedDate = DateTime.Now;
                post.IsPublished = postDto.IsPublished;
                if(postDto.FileUploadId.HasValue)
                {
                    post.FileUploadId = postDto.FileUploadId;
                }

                post.CategoryId = Guid.Parse(postDto.CategoryId);
                post.AuthorId = author.Id;

                var oldPostTags = await _blogContext.PostTags.Where(p => p.PostId == post.Id).ToListAsync();
                 _blogContext.PostTags.RemoveRange(oldPostTags);

                await _blogContext.AddRangeAsync(newTags);
                await _blogContext.SaveChangesAsync();

                var postTags = new List<PostTag>();
                foreach (var newTag in newTags)
                {
                    var dbNewTag = await _blogContext.Tags.FirstOrDefaultAsync(t => t.Name == newTag.Name);
                    postTags.Add(new PostTag { PostId = post.Id, TagId = dbNewTag.Id });
                }

                foreach (var insertedTag in insertedTags)
                {
                    postTags.Add(new PostTag { PostId = post.Id, TagId = insertedTag.Id });
                }

                await _blogContext.AddRangeAsync(postTags);
            }

            await _blogContext.SaveChangesAsync();
        }

        public async Task<PostDto> GetPost(int id)
        {
            var post = await _blogContext.Posts.Where(p => p.Id == id).Select(p => new PostDto
            {
                Title = p.Title,
                ShortDescription = p.ShortDescription,
                Content = p.Content,
                CategoryId = p.CategoryId.ToString(),
                CategoryName = p.Category.Name,
                CreatedDate = p.CreatedDate,
                FileUploadId = p.FileUploadId,
                IsPublished = p.IsPublished,
                Id = p.Id,
                Tags = p.PostTags.Select(pt => pt.Tag.Name).ToList(),
                AuthorName = p.Author.PenName,
            }).FirstOrDefaultAsync();

            return post;
        }

        public async Task<List<PostSummaryDto>> GetPosts(int page)
        {
            return await _blogContext.Posts.OrderByDescending(c => c.CreatedDate).Where(p => p.IsPublished).Skip(10 * page).Take(10)
                                     .Select(x => new PostSummaryDto
                                     {
                                         Id = x.Id,
                                         Title = x.Title,
                                         ShortDescription = x.ShortDescription,
                                         FileUploadId = x.FileUploadId,
                                         Tags = x.PostTags.Select(x => x.Tag).Select(x => x.Name).ToList(),
                                         UrlSlug = x.UrlSlug,
                                         CategoryName = x.Category.Name,
                                         CreatedDate = x.CreatedDate.ToLongDateString(),
                                         AuthorName = x.Author.PenName,
                                         IsPublished = x.IsPublished,
                                     })
                                     .ToListAsync();
        }

        public async Task<List<PostSummaryDto>> GetPostsByCategory(string categoryName)
        {
            return await _blogContext.Posts.OrderByDescending(c => c.CreatedDate).Where(p => p.IsPublished && p.Category.Name == categoryName)
                                     .Select(x => new PostSummaryDto
                                     {
                                         Id = x.Id,
                                         Title = x.Title,
                                         ShortDescription = x.ShortDescription,
                                         FileUploadId = x.FileUploadId,
                                         Tags = x.PostTags.Select(x => x.Tag).Select(x => x.Name).ToList(),
                                         UrlSlug = x.UrlSlug,
                                         CategoryName = x.Category.Name,
                                         CreatedDate = x.CreatedDate.ToLongDateString(),
                                         AuthorName = x.Author.PenName,
                                         IsPublished = x.IsPublished,
                                     })
                                     .ToListAsync();
        }

        public async Task<List<PostSummaryDto>> GetPostsByTag(string tagName)
        {
            return await _blogContext.PostTags.Where(t => t.Tag.Name == tagName)
                                     .Select(t => t.Post)
                                     .OrderByDescending(c => c.CreatedDate)
                                     .Where(p => p.IsPublished)
                                     .Select(x => new PostSummaryDto
                                     {
                                         Id = x.Id,
                                         Title = x.Title,
                                         ShortDescription = x.ShortDescription,
                                         FileUploadId = x.FileUploadId,
                                         Tags = x.PostTags.Select(x => x.Tag).Select(x => x.Name).ToList(),
                                         UrlSlug = x.UrlSlug,
                                         CategoryName = x.Category.Name,
                                         CreatedDate = x.CreatedDate.ToLongDateString(),
                                         AuthorName = x.Author.PenName,
                                         IsPublished = x.IsPublished,
                                     })
                                     .ToListAsync();
        }

        public async Task<List<PostSummaryDto>> GetPostsByUser(string userName = "")
        {
            var authorEmail = string.IsNullOrEmpty(userName) ? _httpContextAccessor.HttpContext.User.Identity.Name : userName;
            return await _blogContext.Posts.OrderByDescending(c => c.CreatedDate).Where(p => p.IsPublished && p.Author.Email == authorEmail)
                                     .Select(x => new PostSummaryDto
                                     {
                                         Id = x.Id,
                                         Title = x.Title,
                                         ShortDescription = x.ShortDescription,
                                         FileUploadId = x.FileUploadId,
                                         Tags = x.PostTags.Select(x => x.Tag).Select(x => x.Name).ToList(),
                                         UrlSlug = x.UrlSlug,
                                         CategoryName = x.Category.Name,
                                         CreatedDate = x.CreatedDate.ToLongDateString(),
                                         AuthorName = x.Author.PenName,
                                         IsPublished = x.IsPublished,
                                     })
                                     .ToListAsync();
        }

        public async Task<ManagePostViewDto> GetManagePostView(int page)
        {
            var managePostViewDto = new ManagePostViewDto();
            var columns = new List<ColumnDto>
            {
                new ColumnDto
                {
                    Id = 1,
                    ColumnName = "id",
                    LabelName = "Id",
                    Align = "left",
                },
                new ColumnDto
                {
                    Id = 2,
                    ColumnName = "title",
                    LabelName = "Title",
                    Align = "left",
                },
                new ColumnDto
                {
                    Id = 3,
                    ColumnName = "shortDescription",
                    LabelName = "Short Description",
                    Align = "left",
                }
            };
            var postSummaryDtoes = await _blogContext.Posts.OrderByDescending(c => c.CreatedDate).Where(p => p.IsPublished).Skip(10 * page).Take(10)
                                                     .Select(x => new PostSummaryDto
                                                     {
                                                         Id = x.Id,
                                                         Title = x.Title,
                                                         ShortDescription = x.ShortDescription,
                                                         FileUploadId = x.FileUploadId,
                                                         Tags = x.PostTags.Select(x => x.Tag).Select(x => x.Name).ToList(),
                                                         UrlSlug = x.UrlSlug,
                                                         CategoryName = x.Category.Name,
                                                         CreatedDate = x.CreatedDate.ToLongDateString(),
                                                         AuthorName = x.Author.PenName,
                                                         IsPublished = x.IsPublished,
                                                     })
                                                     .ToListAsync();

            var totalPost = await _blogContext.Posts.CountAsync();
            managePostViewDto.Columns = columns;
            managePostViewDto.Rows = postSummaryDtoes;
            managePostViewDto.Total = totalPost;

            return managePostViewDto;
        }

        public async Task<ListPostViewDto> GetListPostView(int page)
        {
            var listPostViewDto = new ListPostViewDto();

            var postSummaryDtoes = await _blogContext.Posts.OrderByDescending(c => c.CreatedDate).Where(p => p.IsPublished).Skip(10 * (page - 1)).Take(10)
                                                     .Select(x => new PostSummaryDto
                                                     {
                                                         Id = x.Id,
                                                         Title = x.Title,
                                                         ShortDescription = x.ShortDescription,
                                                         FileUploadId = x.FileUploadId,
                                                         Tags = x.PostTags.Select(x => x.Tag).Select(x => x.Name).ToList(),
                                                         UrlSlug = x.UrlSlug,
                                                         CategoryName = x.Category.Name,
                                                         CreatedDate = x.CreatedDate.ToLongDateString(),
                                                         AuthorName = x.Author.PenName,
                                                         IsPublished = x.IsPublished,
                                                     })
                                                     .ToListAsync();

            listPostViewDto.HasMore = await _blogContext.Posts.Where(p => p.IsPublished).Skip(10 * page).Take(10).AnyAsync();
            listPostViewDto.Posts = postSummaryDtoes;

            return listPostViewDto;
        }

        public async Task<bool> HasMorePosts(int page)
        {
            return await _blogContext.Posts.Where(p => p.IsPublished).Skip(10 * (page + 1)).Take(10).AnyAsync();
        }

        public async Task DeletePost(int id)
        {
            var post = await _blogContext.Posts.FindAsync(id);
            _blogContext.Remove(post);

            await _blogContext.SaveChangesAsync();
        }
    }
}
