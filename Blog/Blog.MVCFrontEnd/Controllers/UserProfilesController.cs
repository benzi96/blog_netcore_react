using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Blog.Domain.Entities;
using Blog.Persistence;
using Blog.MVCFrontEnd.API;
using Blog.Domain.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Blog.MVCFrontEnd.Controllers
{
    [Authorize]
    public class UserProfilesController : Controller
    {
        private readonly IApiService _apiService;

        public UserProfilesController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: UserProfiles/Details/5
        public async Task<IActionResult> Details()
        {
            var userProfile = await _apiService.GetAsync<UserProfile>("/api/userprofile");
            if (userProfile == null)
            {
                return NotFound();
            }

            return View(userProfile);
        }

        // GET: UserProfiles/Create
        public async Task<IActionResult> CreateAsync()
        {
            var userProfile = await _apiService.GetAsync<UserProfile>("/api/userprofile");
            return View(userProfile);
        }

        // POST: UserProfiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,PenName")] UserProfile userProfile)
        {
            if (ModelState.IsValid)
            {
                var userProfileDto = new UserProfileDto
                {
                    PenName = userProfile.PenName
                };

                var response = await _apiService.PostAsync("/api/userprofile", userProfileDto);
                return RedirectToAction(nameof(Details));
            }

            return View(userProfile);
        }
    }
}
