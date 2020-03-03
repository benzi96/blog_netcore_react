using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain.Entities
{
    public class UserProfile : BaseEntity<Guid>
    {
        public string Email { get; set; }
        public string PenName { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
