using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain.DTO
{
    public class PostDto
    {
        public int? Id { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public List<string> Tags { get; set; }
        public string Title { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Guid? FileUploadId { get; set; }
        public string UrlSlug { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsPublished { get; set; }
        public string AuthorName { get; set; }
    }
}
