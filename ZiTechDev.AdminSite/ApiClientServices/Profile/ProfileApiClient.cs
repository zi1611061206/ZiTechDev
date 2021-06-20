using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ZiTechDev.CommonModel.Engines.CustomResult;
using ZiTechDev.CommonModel.Requests.Profile;

namespace ZiTechDev.AdminSite.ApiClientServices.Profile
{
    public class ProfileApiClient : IProfileApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProfileApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        private HttpClient GetHttpClient()
        {
            var client = _httpClientFactory.CreateClient("zitechdev");
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            return client;
        }

        #region GetProfile (Call API)
        public async Task<ApiResult<ProfileViewModel>> GetProfile(Guid userId)
        {
            var client = GetHttpClient();

            var response = await client.GetAsync("api/profiles/get?userId=" + userId);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<Successed<ProfileViewModel>>(body);
            }
            return JsonConvert.DeserializeObject<Failed<ProfileViewModel>>(body);
        }
        #endregion

        #region EditProfile (Call API)
        public async Task<ApiResult<bool>> EditProfile(ProfileEditRequest request)
        {
            var client = GetHttpClient();

            var content = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("api/profiles/edit?userId=" + request.Id, httpContent);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new Successed<bool>(true);
            }
            return new Failed<bool>(body);
        }
        #endregion

        #region ChangePassword (Call API)
        public async Task<ApiResult<bool>> ChangePassword(ChangePasswordRequest request)
        {
            var client = GetHttpClient();

            var content = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("api/profiles/change-password?userId=" + request.Id, httpContent);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new Successed<bool>(true);
            }
            return new Failed<bool>(body);
        }
        #endregion

        #region ChageEmail (Call API)
        public async Task<ApiResult<bool>> ChangeEmail(string changeEmailBaseUrl, ChangeEmailRequest request)
        {
            var client = GetHttpClient();

            var content = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("api/profiles/change-email?changeEmailBaseUrl=" + WebUtility.UrlEncode(changeEmailBaseUrl), httpContent);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new Successed<bool>(true);
            }
            return new Failed<bool>(body);
        }
        #endregion

        #region Setup2FA (Call API)
        public async Task<ApiResult<string>> Setup2FA(Guid userId)
        {
            var client = GetHttpClient();

            var response = await client.GetAsync($"api/profiles/setup-2fa?userId={userId}");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new Successed<string>(body);
            }
            return new Failed<string>(body);
        }
        #endregion

        #region Change2FA (Call API)
        public async Task<ApiResult<bool>> Change2FA(Guid userId, AuthenticateCodeRequest request)
        {
            var client = GetHttpClient();

            var content = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"api/profiles/change-2fa?userId={userId}", httpContent);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new Successed<bool>(true);
            }
            return new Failed<bool>(body);
        }
        #endregion
    }
}
