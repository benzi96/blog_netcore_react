using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain.Entities
{
    public class BaseEntity<T>
    {
        public T Id { get; set; }
    }
}
