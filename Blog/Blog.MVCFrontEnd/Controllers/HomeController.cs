using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Blog.MVCFrontEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using Blog.MVCFrontEnd.API;
using Blog.Domain.DTO;
using Blog.Domain.Entities;

namespace Blog.MVCFrontEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IApiService _apiService;

        public HomeController(ILogger<HomeController> logger, IApiService apiService, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _apiService = apiService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index(int? page)
        {
            if (page != null)
            {
                ViewData["page"] = page;
                var pageResult = await _apiService.GetAsyncWithoutHeader<List<PostSummaryDto>>($"/post/page/{page}");
                ViewData["hasMore"] = await _apiService.GetAsyncWithoutHeader<bool>($"/post/hasmore/{page}");

                return View(pageResult);
            }

            ViewData["page"] = 0;
            var result = await _apiService.GetAsyncWithoutHeader<List<PostSummaryDto>>("/post/page/0");
            ViewData["hasMore"] = await _apiService.GetAsyncWithoutHeader<bool>("/post/hasmore/0");
            //var accessToken = _httpContextAccessor.HttpContext.GetTokenAsync("access_token").Result;
            //var handler = new JwtSecurityTokenHandler();
            //var token = handler.ReadJwtToken(accessToken);

            return View(result ?? new List<PostSummaryDto>());
        }

        public async Task<IActionResult> ViewPost(int id)
        {
            var result = await _apiService.GetAsync<PostDto>($"/post/{id}");
            return View(result);
        }

        [Route("category/{categoryName}")]
        public async Task<IActionResult> ViewPostByCategory(string categoryName)
        {
            var result = await _apiService.GetAsync<List<PostSummaryDto>>($"/post/category/{categoryName}");
            ViewData["CategoryName"] = categoryName;
            return View(result ?? new List<PostSummaryDto>());
        }

        [Route("tag/{tagName}")]
        public async Task<IActionResult> ViewPostByTag(string tagName)
        {
            var result = await _apiService.GetAsync<List<PostSummaryDto>>($"/post/tag/{Uri.EscapeDataString(tagName)}");
            ViewData["TagName"] = tagName;
            return View(result ?? new List<PostSummaryDto>());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
