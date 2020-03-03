using Blog.Domain.Entities;
using Blog.MVCFrontEnd.API;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.MVCFrontEnd.ViewComponents
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly IApiService _apiService;

        public CategoryViewComponent(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _apiService.GetAsyncWithoutHeader<List<Category>>("/api/category");

            return View(categories ?? new List<Category>());
        }
    }
}
