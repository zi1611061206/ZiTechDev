using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ZiTechDev.Business.Requests.Auth;

namespace ZiTechDev.AdminSite.ApiClientServices.Auth
{
    public class AuthApiClient : IAuthApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> Login(LoginRequest request)
        {
            var client = _httpClientFactory.CreateClient("zitechdev"); 
            var content = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/auths/login", httpContent);
            var token = await response.Content.ReadAsStringAsync();
            return token;
        }
    }
}
