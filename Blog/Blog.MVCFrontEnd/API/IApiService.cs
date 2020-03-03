using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blog.MVCFrontEnd.API
{
    public interface IApiService
    {
        Task<T> GetAsync<T>(string requestUrl);
        Task<T> GetAsyncWithoutHeader<T>(string requestUrl);
        Task<HttpResponseMessage> PostAsync<T>(string requestUrl, T content);
        Task<string> PostFileAsync(string requestUrl, IFormFile file);
    }
}
