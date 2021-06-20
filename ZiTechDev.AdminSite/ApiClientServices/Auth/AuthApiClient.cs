﻿using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ZiTechDev.CommonModel.Engines.CustomResult;
using ZiTechDev.CommonModel.Requests.Auth;

namespace ZiTechDev.AdminSite.ApiClientServices.Auth
{
    public class AuthApiClient : IAuthApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        #region Register (Call API)
        public async Task<ApiResult<bool>> Register(string activatedEmailBaseUrl, RegisterRequest request)
        {
            var client = _httpClientFactory.CreateClient("zitechdev");

            var content = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/auths/register?activatedEmailBaseUrl=" + WebUtility.UrlEncode(activatedEmailBaseUrl), httpContent);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new Successed<bool>(true);
            }
            return new Failed<bool>(body);
        }
        #endregion

        #region ValidateLogin (Call API)
        public async Task<ApiResult<bool>> ValidateLogin(string resetPasswordBaseUrl, LoginRequest request)
        {
            var client = _httpClientFactory.CreateClient("zitechdev"); 

            var content = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/auths/validate-login?resetPasswordBaseUrl=" + WebUtility.UrlEncode(resetPasswordBaseUrl), httpContent);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                bool result = bool.Parse(body);
                return new Successed<bool>(result);
            }
            return new Failed<bool>(body);
        }
        #endregion

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

        #region Authenticate2FA (Call API)
        public async Task<ApiResult<string>> Authenticate2FA(Authenticate2FARequest request)
        {
            var client = _httpClientFactory.CreateClient("zitechdev");

            var content = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/auths/authenticate-2fa", httpContent);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new Successed<string>(body);
            }
            return new Failed<string>(body);
        }
        #endregion

        #region ForgotPassword (Call API)
        public async Task<ApiResult<bool>> ForgotPassword(string resetPasswordBaseUrl, ForgotPasswordRequest request)
        {
            var client = _httpClientFactory.CreateClient("zitechdev");

            var content = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"api/auths/forgot-password?resetPasswordBaseUrl=" + WebUtility.UrlEncode(resetPasswordBaseUrl), httpContent);
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

        #region VertifiedChangeEmail (Call API)
        public async Task<ApiResult<bool>> VertifiedChangeEmail(Guid userId, string token, string newEmail)
        {
            var client = _httpClientFactory.CreateClient("zitechdev");

            var response = await client.GetAsync($"api/auths/vertified-change-email?userId={userId}&token={token}&newEmail={newEmail}");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new Successed<bool>(true);
            }
            return new Failed<bool>(body);
        }
        #endregion

        #region ActivatedEmail (Call API)
        public async Task<ApiResult<string>> ActivatedEmail(Guid userId, string token)
        {
            var client = _httpClientFactory.CreateClient("zitechdev");

            var response = await client.GetAsync($"api/auths/activated-email?userId={userId}&token={token}");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new Successed<string>(body);
            }
            return new Failed<string>(body);
        }
        #endregion
    }
}
