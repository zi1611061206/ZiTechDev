using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ZiTechDev.Business.Engines.Paginition;
using ZiTechDev.Business.Requests.User;

namespace ZiTechDev.AdminSite.ApiClientServices.User
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public UserApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<PaginitionEngines<UserViewModel>> Get(UserFilter filter)
        {
            var client = _httpClientFactory.CreateClient("zitechdev");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", filter.BearerToken);

            var content = JsonConvert.SerializeObject(filter);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/users", httpContent);

            //var response = await client.GetAsync($"/api/users?" +
            //    $"currentPageIndex={filter.CurrentPageIndex}&" +
            //    $"pageSize={filter.PageSize}&" +
            //    $"fullName={filter.FullName}&" +
            //    $"userName={filter.UserName}&" +
            //    $"displayName={filter.DisplayName}&" +
            //    $"phoneNumber={filter.PhoneNumber}&" +
            //    $"email={filter.Email}&" +
            //    $"gender={filter.Gender}&" +
            //    $"fromDOB={filter.FromDOB}&" +
            //    $"toDOB={filter.ToDOB}");

            var body = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<PaginitionEngines<UserViewModel>>(body);
            return user;
        }
    }
}
