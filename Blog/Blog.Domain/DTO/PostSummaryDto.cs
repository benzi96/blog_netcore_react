using Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blog.Domain.DTO
{
    public class PostSummaryDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public Guid? FileUploadId { get; set; }
        public string CategoryName { get; set; }
        public string UrlSlug { get; set; }
        public string CreatedDate { get; set; }
        public List<string> Tags { get; set; }
        public string AuthorName { get; set; }
        public bool IsPublished { get; set; }
    }
}
