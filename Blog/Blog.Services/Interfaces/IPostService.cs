using Blog.Domain.DTO;
using Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Interfaces
{
    public interface IPostService
    {
        Task SavePost(PostDto postDto);
        Task<PostDto> GetPost(int id);
        Task<List<PostSummaryDto>> GetPosts(int page);
        Task<bool> HasMorePosts(int page);
        Task<List<PostSummaryDto>> GetPostsByCategory(string categoryName);
        Task<List<PostSummaryDto>> GetPostsByTag(string tagName);
        Task<ManagePostViewDto> GetManagePostView(int page);
        Task<ListPostViewDto> GetListPostView(int page);
        Task DeletePost(int id);
        Task<List<PostSummaryDto>> GetPostsByUser(string userName = "");
    }
}
