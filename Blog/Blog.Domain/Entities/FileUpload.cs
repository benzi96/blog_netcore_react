using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain.Entities
{
    public class FileUpload : BaseEntity<Guid>
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Extension { get; set; }
    }
}
