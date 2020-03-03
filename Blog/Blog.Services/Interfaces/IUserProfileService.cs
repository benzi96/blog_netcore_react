using Blog.Domain.DTO;
using Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Interfaces
{
    public interface IUserProfileService
    {
        Task SaveUserProfile(UserProfileDto userProfileDto);
        Task<UserProfile> GetUserProfile();
    }
}
