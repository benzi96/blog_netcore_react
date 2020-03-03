using Blog.Domain.Entities;
using Blog.Persistence;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Services
{
    public class FileService : IFileService
    {
        private readonly BlogContext _blogContext;
        private readonly IConfiguration _config;

        public FileService(BlogContext blogContext, IConfiguration config)
        {
            _blogContext = blogContext;
            _config = config;
        }

        public async Task<string> GetFilePathById(Guid id)
        {
            var fileUpload = await _blogContext.FileUploads.FindAsync(id);
            return Path.Combine(_config["StoredFilesPath"], fileUpload.FileName);
        }

        public async Task<Guid?> SaveFile(IFormFile formFile)
        {
            if (formFile.Length <= 0)
            {
                return null;
            }

            var newFileUpload = new FileUpload();

            await _blogContext.AddAsync(newFileUpload);
            await _blogContext.SaveChangesAsync();

            var fileExt = Path.GetExtension(formFile.FileName);
            var fileName = $"{newFileUpload.Id.ToString()}{fileExt}";
            var filePath = Path.Combine(_config["StoredFilesPath"], fileName);

            Directory.CreateDirectory(_config["StoredFilesPath"]);
            using (var stream = File.Create(filePath))
            {
                await formFile.CopyToAsync(stream);
            }

            newFileUpload.FilePath = filePath;
            newFileUpload.FileName = fileName;
            newFileUpload.Extension = fileExt;

            await _blogContext.SaveChangesAsync();

            return newFileUpload.Id;
        }
    }
}
