using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Domain.DTO;
using Blog.Persistence;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{
    [Route("post")]
    [ApiController]
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> SavePost(PostDto postDto)
        {
            await _postService.SavePost(postDto);
            return Ok();
        }

        [Route("deletepost/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            await _postService.DeletePost(id);
            return Ok();
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await _postService.GetPost(id);
            return Ok(post);
        }

        [Route("page/{page}")]
        [HttpGet]
        public async Task<IActionResult> GetPosts(int page)
        {
            var posts = await _postService.GetPosts(page);
            return Ok(posts);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("userposts")]
        [HttpGet]
        public async Task<IActionResult> GetPostsByUser()
        {
            var posts = await _postService.GetPostsByUser();
            return Ok(posts);
        }

        [Route("userposts/{userName}")]
        [HttpGet]
        public async Task<IActionResult> GetPostsByUser(string userName)
        {
            var posts = await _postService.GetPostsByUser(userName);
            return Ok(posts);
        }

        [Route("hasmore/{page}")]
        [HttpGet]
        public async Task<IActionResult> HasMorePosts(int page)
        {
            var hasMore = await _postService.HasMorePosts(page);
            return Ok(hasMore);
        }

        [Route("category/{categoryName}")]
        [HttpGet]
        public async Task<IActionResult> GetPostsByCategory(string categoryName)
        {
            var posts = await _postService.GetPostsByCategory(categoryName);
            return Ok(posts);
        }

        [Route("tag/{tagName}")]
        [HttpGet]
        public async Task<IActionResult> GetPostsByTag(string tagName)
        {
            var posts = await _postService.GetPostsByTag(tagName);
            return Ok(posts);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("managepost/{page}")]
        [HttpGet]
        public async Task<IActionResult> GetManagePostView(int page)
        {
            var managePostView = await _postService.GetManagePostView(page);
            return Ok(managePostView);
        }

        [Route("viewpost/{page}")]
        [HttpGet]
        public async Task<IActionResult> GetListPostView(int page)
        {
            var listPostView = await _postService.GetListPostView(page);
            return Ok(listPostView);
        }
    }
}