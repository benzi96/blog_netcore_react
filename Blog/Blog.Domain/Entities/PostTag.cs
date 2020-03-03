﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain.Entities
{
    public class PostTag
    {
        public int PostId { get; set; }
        public Post Post { get; set; }
        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
