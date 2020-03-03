using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain.Entities
{
    public class Tag : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public ICollection<PostTag> PostTags { get; set; }
    }
}
