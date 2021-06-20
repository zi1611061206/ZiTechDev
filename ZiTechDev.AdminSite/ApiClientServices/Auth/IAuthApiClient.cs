using System;
using System.Threading.Tasks;
using ZiTechDev.CommonModel.Engines.CustomResult;
using ZiTechDev.CommonModel.Requests.Auth;

namespace ZiTechDev.AdminSite.ApiClientServices.Auth
{
    public interface IAuthApiClient
    {
        /// <summary>
        /// Gọi tới API đăng ký (gửi email kích hoạt email)
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <param name="activatedEmailBaseUrl"></param>
        /// <typeparam name="RegisterRequest"></typeparam>
        /// <param name="request"></param>
        /// <returns>
        /// Trả về thông báo lỗi (Message) nếu không thành công,
        /// Trả về true (ReturnedObject) nếu thành công
        /// </returns>
        Task<ApiResult<bool>> Register(string activatedEmailBaseUrl, RegisterRequest request);

        /// <summary>
        /// Gọi tới API kiểm tra thông tin đăng nhập (gửi email cảnh báo đăng nhập nếu bị khóa)
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <param name="resetPasswordBaseUrl"></param>
        /// <typeparam name="LoginRequest"></typeparam>
        /// <param name="request"></param>
        /// <returns>
        /// Trả về thông báo lỗi (Message) nếu không thành công,
        /// Trả về TwoFactorsEnabled (ReturnedObject) nếu thành công
        /// </returns>
        Task<ApiResult<bool>> ValidateLogin(string resetPasswordBaseUrl, LoginRequest request);

        /// <summary>
        /// Gọi tới API đăng nhập cho tài khoản không bật xác thực 2 bước
        /// </summary>
        /// <typeparam name="LoginRequest"></typeparam>
        /// <param name="request"></param>
        /// <returns>
        /// Trả về thông báo lỗi (Message) nếu không thành công,
        /// Trả về chuỗi token người dùng đã mã hóa (ReturnedObject) nếu thành công
        /// </returns>
        Task<ApiResult<string>> Login(LoginRequest request);

        /// <summary>
        /// Gọi tới API đăng nhập cho tài khoản đã bật xác thực 2 bước
        /// </summary>
        /// <typeparam name="Authenticate2FARequest"></typeparam>
        /// <param name="request"></param>
        /// <returns>
        /// Trả về thông báo lỗi (Message) nếu không thành công,
        /// Trả về chuỗi token người dùng đã mã hóa (ReturnedObject) nếu thành công
        /// </returns>
        Task<ApiResult<string>> Authenticate2FA(Authenticate2FARequest request);

        /// <summary>
        /// Gọi tới API quên mật khẩu (gửi email xác nhận đặt lại mật khẩu)
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <param name="resetPasswordBaseUrl"></param>
        /// <typeparam name="ForgotPasswordRequest"></typeparam>
        /// <param name="request"></param>
        /// <returns>
        /// Trả về thông báo lỗi (Message) nếu không thành công,
        /// Trả về true (ReturnedObject) nếu thành công
        /// </returns>
        Task<ApiResult<bool>> ForgotPassword(string resetPasswordBaseUrl, ForgotPasswordRequest request);

        /// <summary>
        /// Gọi tới API mở khóa tài khoản
        /// </summary>
        /// <typeparam name="Guid"></typeparam>
        /// <param name="userId">Mở khóa cho tài khoản nào</param>
        /// <returns>
        /// Trả về thông báo lỗi (Message) nếu không thành công,
        /// Trả về true (ReturnedObject) nếu thành công
        /// </returns>
        Task<ApiResult<bool>> UnlockOut(Guid userId);

        /// <summary>
        /// Gọi tới API đặt lại mật khẩu (Đồng nghĩa email được xác thực)
        /// </summary>
        /// <typeparam name="ResetPasswordRequest"></typeparam>
        /// <param name="request"></param>
        /// <returns>
        /// Trả về thông báo lỗi (Message) nếu không thành công,
        /// Trả về true (ReturnedObject) nếu thành công
        /// </returns>
        Task<ApiResult<bool>> ResetPassword(ResetPasswordRequest request);

        /// <summary>
        /// Gọi tới API xử lý xác thực email (Không bắt buộc đổi mật khẩu)
        /// </summary>
        /// <typeparam name="Guid"></typeparam>
        /// <param name="userId"></param>
        /// <typeparam name="string"></typeparam>
        /// <param name="token"></param>
        /// <returns>
        /// Trả về thông báo lỗi (Message) nếu không thành công,
        /// Trả về true (ReturnedObject) nếu thành công
        /// </returns>
        Task<ApiResult<bool>> VertifiedEmail(Guid userId, string token);

        /// <summary>
        /// Gọi tới API xử lý thay đổi email
        /// </summary>
        /// <typeparam name="Guid"></typeparam>
        /// <param name="userId"></param>
        /// <typeparam name="string"></typeparam>
        /// <param name="token"></param>
        /// <typeparam name="string"></typeparam>
        /// <param name="newEmail"></param>
        /// <returns>
        /// Trả về thông báo lỗi (Message) nếu không thành công,
        /// Trả về true (ReturnedObject) nếu thành công
        /// </returns>
        Task<ApiResult<bool>> VertifiedChangeEmail(Guid userId, string token, string newEmail);

        /// <summary>
        /// Gọi tới API xử lý kích hoạt email (Bắt buộc đổi mật khẩu)
        /// </summary>
        /// <typeparam name="Guid"></typeparam>
        /// <param name="userId"></param>
        /// <typeparam name="string"></typeparam>
        /// <param name="token"></param>
        /// <returns>
        /// Trả về thông báo lỗi (Message) nếu không thành công,
        /// Trả về token đặt lại mật khẩu (ReturnedObject) nếu thành công
        /// </returns>
        Task<ApiResult<string>> ActivatedEmail(Guid userId, string token);
    }
}
