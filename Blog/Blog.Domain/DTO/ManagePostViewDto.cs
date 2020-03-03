using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain.DTO
{
    public class ManagePostViewDto
    {
        public List<ColumnDto> Columns { get; set; }
        public List<PostSummaryDto> Rows { get; set; }
        public int Total { get; set; }
    }
}
