using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Blog.Domain.Entities;
using Blog.Persistence;
using Microsoft.AspNetCore.Authorization;
using Blog.MVCFrontEnd.API;
using Blog.MVCFrontEnd.Models;
using Blog.Domain.DTO;

namespace Blog.MVCFrontEnd.Controllers
{
    // [Authorize(Roles = "Administrator")] uncomment to authorize with roles
    [Authorize]
    public class PostsController : Controller
    {
        private readonly IApiService _apiService;
        public PostsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var user = await _apiService.GetAsync<UserProfile>("/api/userprofile");
            if (user == null)
            {
                TempData["Message"] = "You need to create your profile to continue";
                return RedirectToAction("Create", "UserProfiles");
            }

            var blogContext = await _apiService.GetAsync<List<PostSummaryDto>>("/post/userposts");
            return View(blogContext ?? new List<PostSummaryDto>());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _apiService.GetAsyncWithoutHeader<PostDto>($"/post/{id}");

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public async Task<IActionResult> Create()
        {
            var categories = await _apiService.GetAsync<IEnumerable<Category>>("/api/category");
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,CategoryId,Tags,File,ShortDescription,Content,IsPublished,Id")] SavePostViewModel post)
        {
            var categories = await _apiService.GetAsync<IEnumerable<Category>>("/api/category");

            if (ModelState.IsValid)
            {
                var postDto = new PostDto
                {
                    Title = post.Title,
                    CategoryId = post.CategoryId,
                    Content = post.Content,
                    ShortDescription = post.ShortDescription,
                    IsPublished = post.IsPublished,
                    Tags = !string.IsNullOrEmpty(post.Tags) ? post.Tags.Split(";").ToList() : null
                };
                var result = await _apiService.PostFileAsync("/file", post.File);

                if (result == "Unauthorized request! Please log out then log in again!" || string.IsNullOrEmpty(result))
                {
                    ViewData["Error"] = result;
                    ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", post.CategoryId);
                    return View(post);
                }

                postDto.FileUploadId = Guid.Parse(result);
                var response = await _apiService.PostAsync("/post", postDto);
                if (!response.IsSuccessStatusCode)
                {
                    ViewData["Error"] = "An Error has occurred";
                    ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", post.CategoryId);
                    return View(post);
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", post.CategoryId);
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _apiService.GetAsync<PostDto>($"/post/{id}");
            if (post == null)
            {
                return NotFound();
            }

            var savePostViewModel = new SavePostViewModel
            {
                Id = post.Id,
                IsPublished = post.IsPublished,
                Title = post.Title,
                ShortDescription = post.ShortDescription,
                Content = post.Content,
                CategoryId = post.CategoryId,
                Tags = string.Join(";", post.Tags),
                FileUploadId = post.FileUploadId,
            };

            var categories = await _apiService.GetAsync<IEnumerable<Category>>("/api/category");
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name");

            return View(savePostViewModel);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,CategoryId,Tags,File,ShortDescription,Content,IsPublished,FileUploadId,Id")] SavePostViewModel post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            var categories = await _apiService.GetAsync<IEnumerable<Category>>("/api/category");

            if (ModelState.IsValid)
            {
                var postDto = new PostDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    CategoryId = post.CategoryId,
                    Content = post.Content,
                    ShortDescription = post.ShortDescription,
                    IsPublished = post.IsPublished,
                    Tags = !string.IsNullOrEmpty(post.Tags) ? post.Tags.Split(";").ToList() : null
                };

                if (post.File != null)
                {
                    var fileUploadId = await _apiService.PostFileAsync("/file", post.File);
                    if (string.IsNullOrEmpty(fileUploadId))
                    {
                        ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", post.CategoryId);
                        return View(post);
                    }

                    postDto.FileUploadId = Guid.Parse(fileUploadId);
                }

                var response = await _apiService.PostAsync("/post", postDto);

                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", post.CategoryId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _apiService.GetAsyncWithoutHeader<PostDto>($"/post/{id}");
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _apiService.PostAsync($"/deletepost", id);
            return RedirectToAction(nameof(Index));
        }
    }
}
