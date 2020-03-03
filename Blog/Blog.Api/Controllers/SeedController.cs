using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Domain.Entities;
using Blog.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{
    [Route("api/seed")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly BlogContext _blogContext;
        public SeedController(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }

        [Route("{number}")]
        [HttpGet]
        public async Task<IActionResult> Seed(int number)
        {
            for (int i = 1; i < number; i++)
            {
                var post = new Post
                {
                    Title = $"test {i}",
                    Content = "{\"blocks\":[{\"key\":\"9rsu9\",\"text\":\"test\",\"type\":\"unstyled\",\"depth\":0,\"inlineStyleRanges\":[],\"entityRanges\":[],\"data\":{}}],\"entityMap\":{}}",
                };
                await _blogContext.AddAsync(post);
            }
            await _blogContext.SaveChangesAsync();

            return Ok();
        }
    }
}