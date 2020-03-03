using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain.DTO
{
    public class ListPostViewDto
    {
        public List<PostSummaryDto> Posts { get; set; }
        public bool HasMore { get; set; }
    }
}
