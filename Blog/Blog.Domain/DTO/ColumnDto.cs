using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain.DTO
{
    public class ColumnDto
    {
        public int Id { get; set; }
        public string LabelName { get; set; }
        public string ColumnName { get; set; }
        public string Align { get; set; }
    }
}
