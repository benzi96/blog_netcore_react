using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.MVCFrontEnd.Models
{
    public class SavePostViewModel
    {
        public int? Id { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public string Tags { get; set; }
        public string Title { get; set; }
        public string CategoryId { get; set; }
        public Guid? FileUploadId { get; set; }
        public IFormFile File { get; set; }
        public string UrlSlug { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsPublished { get; set; }
    }
}
