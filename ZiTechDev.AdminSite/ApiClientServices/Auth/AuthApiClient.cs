using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ZiTechDev.CommonModel.Engines.CustomResult;
using ZiTechDev.CommonModel.Requests.Auth;
using ZiTechDev.CommonModel.Requests.User;

namespace ZiTechDev.AdminSite.ApiClientServices.Auth
{
    public class AuthApiClient : IAuthApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        #region Login (Call API)
        public async Task<ApiResult<string>> Login(LoginRequest request)
        {
            var client = _httpClientFactory.CreateClient("zitechdev"); 

            var content = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/auths/login", httpContent);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new Successed<string>(body);
            }
            return new Failed<string>(body);
        }
        #endregion

        #region LoginWarning (Call API)
        public async Task<ApiResult<int>> LoginWarning(string userName, string forgotPasswordBaseUrl)
        {
            var client = _httpClientFactory.CreateClient("zitechdev");

            var response = await client.GetAsync($"api/auths/login-warning?userName={userName}&forgotPasswordBaseUrl=" + WebUtility.UrlEncode(forgotPasswordBaseUrl));
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new Successed<int>(int.Parse(body));
            }
            return new Failed<int>(body);
        }
        #endregion

        #region ForgotPassword (Call API)
        public async Task<ApiResult<string>> ForgotPassword(ForgotPasswordRequest request)
        {
            var client = _httpClientFactory.CreateClient("zitechdev");

            var content = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"api/auths/forgot-password", httpContent);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new Successed<string>(body);
            }
            return new Failed<string>(body);
        }
        #endregion

        #region SendForgotPasswordEmail (Call API)
        public async Task<ApiResult<bool>> SendForgotPasswordEmail(string email, string token, string resetPasswordBaseUrl)
        {
            var client = _httpClientFactory.CreateClient("zitechdev");

            var response = await client.GetAsync($"api/auths/send-forgot?email={email}&token={token}&resetPasswordBaseUrl=" + WebUtility.UrlEncode(resetPasswordBaseUrl));
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new Successed<bool>(true);
            }
            return new Failed<bool>(body);
        }
        #endregion

        #region UnlockOut (Call API)
        public async Task<ApiResult<bool>> UnlockOut(Guid userId)
        {
            var client = _httpClientFactory.CreateClient("zitechdev");

            var response = await client.GetAsync($"api/auths/unlock-out?userId={userId}");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new Successed<bool>(true);
            }
            return new Failed<bool>(body);
        }
        #endregion

        #region ResetPassword (Call API)
        public async Task<ApiResult<bool>> ResetPassword(ResetPasswordRequest request)
        {
            var client = _httpClientFactory.CreateClient("zitechdev");

            var content = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"api/auths/reset-password", httpContent);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new Successed<bool>(true);
            }
            return new Failed<bool>(body);
        }
        #endregion

        #region VertifiedEmail (Call API)
        public async Task<ApiResult<bool>> VertifiedEmail(Guid userId, string token)
        {
            var client = _httpClientFactory.CreateClient("zitechdev");

            var response = await client.GetAsync($"api/auths/vertified-email?userId={userId}&token={token}");
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
