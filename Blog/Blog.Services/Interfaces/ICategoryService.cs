using Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategories();
    }
}
