using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Blog.MVCFrontEnd.API
{
    public class ApiService : IApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private Uri BaseEndpoint { get; set; }

        public ApiService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            BaseEndpoint = new Uri(_configuration.GetValue<string>("ApiUrl"));
        }

        public async Task<T> GetAsync<T>(string requestUrl)
        {
            var client = _httpClientFactory.CreateClient();
            await AddHeaders(client);

            try
            {
                var response = await client.GetAsync(CreateRequestUri(requestUrl));

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<T>();
                }

                return default;
            }
            catch
            {
                return default;
            }
        }

        public async Task<T> GetAsyncWithoutHeader<T>(string requestUrl)
        {
            var client = _httpClientFactory.CreateClient();

            try
            {
                var response = await client.GetAsync(CreateRequestUri(requestUrl));
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<T>();
                }

                return default;
            }
            catch
            {
                return default;
            }
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string requestUrl, T content)
        {
            var client = _httpClientFactory.CreateClient();
            await AddHeaders(client);

            try
            {
                var response = await client.PostAsJsonAsync(CreateRequestUri(requestUrl), content);

                return response;
            }
            catch
            {
                return null;
            }
            
        }

        public async Task<string> PostFileAsync(string requestUrl, IFormFile file)
        {
            var client = _httpClientFactory.CreateClient();
            await AddHeaders(client);

            var response = await client.PostAsync(CreateRequestUri(requestUrl), CreateHttpMultiPartContent(file));
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<string>();
                return data;
            }

            return (int)response.StatusCode == StatusCodes.Status401Unauthorized ? "Unauthorized request! Please log out then log in again!" : string.Empty;
            
        }

        private async Task<T1> PostAsync<T1, T2>(string requestUrl, T2 content)
        {
            var client = _httpClientFactory.CreateClient();
            await AddHeaders(client);
            var response = await client.PostAsJsonAsync(requestUrl, CreateHttpContent<T2>(content));
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T1>(data);
        }

        private Uri CreateRequestUri(string relativePath, string queryString = "")
        {
            var endpoint = new Uri(BaseEndpoint, relativePath);
            var uriBuilder = new UriBuilder(endpoint);
            uriBuilder.Query = queryString;
            return uriBuilder.Uri;
        }

        private HttpContent CreateHttpContent<T>(T content)
        {
            var json = JsonConvert.SerializeObject(content, MicrosoftDateFormatSettings);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private MultipartFormDataContent CreateHttpMultiPartContent(IFormFile file)
        {
            byte[] data;
            using (var br = new BinaryReader(file.OpenReadStream()))
            {
                data = br.ReadBytes((int)file.OpenReadStream().Length);
            }

            ByteArrayContent bytes = new ByteArrayContent(data);
            MultipartFormDataContent multiContent = new MultipartFormDataContent();

            multiContent.Add(bytes, "file", file.FileName);

            return multiContent;
        }

        private static JsonSerializerSettings MicrosoftDateFormatSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
            }
        }

        private async Task AddHeaders(HttpClient client)
        {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }
}
