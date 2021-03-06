using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ZiTechDev.CommonModel.Engines.CustomResult;
using ZiTechDev.CommonModel.Engines.Paginition;
using ZiTechDev.CommonModel.Requests.User;

namespace ZiTechDev.AdminSite.ApiClientServices.User
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        private string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst(i => i.Type == "UserId").Value;
        }

        private HttpClient GetHttpClient()
        {
            var client = _httpClientFactory.CreateClient("zitechdev");
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            return client;
        }

        public async Task<ApiResult<PaginitionEngines<UserViewModel>>> Get(UserFilter filter)
        {
            var client = GetHttpClient();

            if (GetCurrentUserId() != null)
            {
                filter.CurrentUserId = GetCurrentUserId();
            }

            var content = JsonConvert.SerializeObject(filter);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/users", httpContent);
            var body = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<Successed<PaginitionEngines<UserViewModel>>>(body);
            return users;
        }
        
        public async Task<ApiResult<UserViewModel>> GetById(Guid userId)
        {
            var client = GetHttpClient();

            var response = await client.GetAsync("api/users/" + userId);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<Successed<UserViewModel>>(body);
            }
            return JsonConvert.DeserializeObject<Failed<UserViewModel>>(body);
        }

        public async Task<ApiResult<UserViewModel>> GetByUserName(string userName)
        {
            var client = GetHttpClient();

            var response = await client.GetAsync("api/users?userName=" + userName);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<Successed<UserViewModel>>(body);
            }
            return JsonConvert.DeserializeObject<Failed<UserViewModel>>(body);
        }

        public async Task<ApiResult<string>> Create(UserCreateRequest request)
        {
            var client = GetHttpClient();

            var content = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/users/create", httpContent);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new Successed<string>(body);
            }
            return new Failed<string>(body);
        }

        public async Task<ApiResult<string>> Update(UserUpdateRequest request)
        {
            var client = GetHttpClient();

            var content = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("api/users/update/" + request.Id, httpContent);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new Successed<string>(body);
            }
            return new Failed<string>(body);
        }

        public async Task<ApiResult<bool>> Delete(Guid userId)
        {
            var client = GetHttpClient();

            var response = await client.DeleteAsync("api/users/delete/" + userId);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new Successed<bool>();
            }
            return new Failed<bool> (body);
        }

        public async Task<ApiResult<string>> ConfirmEmail(Guid userId)
        {
            var client = GetHttpClient();

            var response = await client.GetAsync("api/users/confirm-email/" + userId);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new Successed<string>(body);
            }
            return new Failed<string>(body);
        }

        public async Task<ApiResult<bool>> SendActiveEmail(string email, string token, string activeBaseUrl)
        {
            var client = GetHttpClient();

            var response = await client.GetAsync($"api/users/send-active?email={email}&token={token}&activeBaseUrl=" + WebUtility.UrlEncode(activeBaseUrl));
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new Successed<bool>(true);
            }
            return new Failed<bool>(body);
        }
    }
}
