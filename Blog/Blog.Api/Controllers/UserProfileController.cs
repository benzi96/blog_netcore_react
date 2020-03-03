using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blog.Domain.DTO;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;

namespace Blog.Api.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : Controller
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserProfile()
        {
            return Ok(await _userProfileService.GetUserProfile());
        }

        [HttpPost]
        public async Task<IActionResult> SaveUserProfile(UserProfileDto userProfileDto)
        {
            await _userProfileService.SaveUserProfile(userProfileDto);

            return Ok();
        }
    }
}