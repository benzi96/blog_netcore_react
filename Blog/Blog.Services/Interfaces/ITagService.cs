using Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Services.Interfaces
{
    public interface ITagService
    {
        List<Tag> GetSuggestions();
        void SaveTags(List<Tag> tags);
    }
}
