using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Interfaces
{
    public interface IFileService
    {
        Task<string> GetFilePathById(Guid id);
        Task<Guid?> SaveFile(IFormFile formFile);
    }
}
