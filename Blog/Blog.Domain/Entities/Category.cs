using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain.Entities
{
    public class Category : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
