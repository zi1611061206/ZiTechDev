using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ZiTechDev.Business.Engines.CustomResult;
using ZiTechDev.Business.Requests.User;

namespace ZiTechDev.AdminSite.ApiClientServices.Profile
{
    public class ProfileApiClient : IProfileApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProfileApiClient(
            IHttpClientFactory httpClientFactory, 
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResult<UserViewModel>> GetProfile(Guid userId)
        {
            var client = _httpClientFactory.CreateClient("zitechdev");
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.GetAsync("api/profiles/" + userId);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<Successed<UserViewModel>>(body);
            }
            return JsonConvert.DeserializeObject<Failed<UserViewModel>>(body);
        }
    }
}
