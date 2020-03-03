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
    public class UserProfileService : IUserProfileService
    {
        private readonly BlogContext _blogContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserProfileService(BlogContext blogContext, IHttpContextAccessor httpContextAccessor)
        {
            _blogContext = blogContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SaveUserProfile(UserProfileDto userProfileDto)
        {
            var authenticatedUserName = _httpContextAccessor.HttpContext.User.Identity.Name;

            var user = await _blogContext.UserProfiles.FirstOrDefaultAsync(u => u.Email == authenticatedUserName);
            if(user == null)
            {
                var newUser = new UserProfile
                {
                    Email = authenticatedUserName,
                    PenName = userProfileDto.PenName,
                };
                await _blogContext.AddAsync(newUser);
            }
            else
            {
                user.Email = authenticatedUserName;
                user.PenName = userProfileDto.PenName;
            }

            await _blogContext.SaveChangesAsync();

        }

        public async Task<UserProfile> GetUserProfile()
        {
            var authenticatedUser = _httpContextAccessor.HttpContext.User.Identity.Name;

            return await _blogContext.UserProfiles.FirstOrDefaultAsync(u => u.Email == authenticatedUser);
        }
    }
}
