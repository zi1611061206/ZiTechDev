using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ZiTechDev.Business.Engines.CustomResult;
using ZiTechDev.Business.Engines.Paginition;
using ZiTechDev.Business.Requests.Role;

namespace ZiTechDev.AdminSite.ApiClientServices.Role
{
    public class RoleApiClient : IRoleApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RoleApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResult<PaginitionEngines<RoleViewModel>>> Get(RoleFilter filter)
        {
            var client = _httpClientFactory.CreateClient("zitechdev");
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var content = JsonConvert.SerializeObject(filter);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/roles", httpContent);
            var body = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<Successed<PaginitionEngines<RoleViewModel>>>(body);
            return roles;
        }

        public async Task<ApiResult<RoleViewModel>> GetById(Guid roleId)
        {
            var client = _httpClientFactory.CreateClient("zitechdev");
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.GetAsync("api/roles/" + roleId);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<Successed<RoleViewModel>>(body);
            }
            return JsonConvert.DeserializeObject<Failed<RoleViewModel>>(body);
        }

        public async Task<ApiResult<string>> Create(RoleCreateRequest request)
        {
            var client = _httpClientFactory.CreateClient("zitechdev");
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var content = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/roles/create", httpContent);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new Successed<string>(body);
            }
            return new Failed<string>(body);
        }

        public async Task<ApiResult<string>> Update(RoleUpdateRequest request)
        {
            var client = _httpClientFactory.CreateClient("zitechdev");
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var content = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("api/roles/update/" + request.Id, httpContent);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new Successed<string>(body);
            }
            return new Failed<string>(body);
        }

        public async Task<ApiResult<bool>> Delete(Guid roleId)
        {
            var client = _httpClientFactory.CreateClient("zitechdev");
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.DeleteAsync("api/roles/delete/" + roleId);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new Successed<bool>();
            }
            return new Failed<bool>(body);
        }

        public async Task<ApiResult<List<RoleViewModel>>> GetAll()
        {
            var client = _httpClientFactory.CreateClient("zitechdev");
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.GetAsync("/api/roles");
            var body = await response.Content.ReadAsStringAsync(); 
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<Successed<List<RoleViewModel>>>(body);
            }
            return JsonConvert.DeserializeObject<Failed<List<RoleViewModel>>>(body);
        }
    }
}
