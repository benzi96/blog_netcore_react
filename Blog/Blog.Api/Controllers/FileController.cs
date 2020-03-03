using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;

namespace Blog.Api.Controllers
{
    [Route("file")]
    [ApiController]
    public class FileController : Controller
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<FileResult> GetFile(Guid id)
        {
            var filePath = await _fileService.GetFilePathById(id);
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(Path.GetFileName(filePath), out contentType))
            {
                contentType = "application/octet-stream";
            }

            return PhysicalFile(filePath, contentType);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> OnPostUploadAsync(IFormFile file)
        {
            var result = await _fileService.SaveFile(file);

            return Ok(result);
        }
    }
}