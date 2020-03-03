using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain.Entities
{
    public class Post : BaseEntity<int>
    {
        public string Title { get; set; }
        public Guid? FileUploadId { get; set; }
        public Guid? AuthorId { get; set; }
        public Guid? CategoryId { get; set; }
        public string UrlSlug { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsPublished { get; set; }

        public FileUpload FileUpload { get; set; }
        public UserProfile Author { get; set; }
        public Category Category { get; set; }
        public ICollection<PostTag> PostTags { get; set; }
    }
}
