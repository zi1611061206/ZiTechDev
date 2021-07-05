using System;
using System.Threading.Tasks;
using ZiTechDev.CommonModel.Engines.CustomResult;
using ZiTechDev.CommonModel.Requests.Auth;

namespace ZiTechDev.Api.Services.Auth
{
    public interface IAuthService
    {
        /// <summary>
        /// Hàm xử lý đăng ký (gửi email kích hoạt email)
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
        /// Hàm kiểm tra tài khoản đăng nhập
        /// </summary>
        /// <typeparam name="LoginUserNameRequest"></typeparam>
        /// <param name="request"></param>
        /// <returns>
        /// Trả về thông báo lỗi (Message) nếu không thành công,
        /// Trả về EmailConfirmed (ReturnedObject) nếu thành công
        /// </returns>
        Task<ApiResult<bool>> ValidateUserName(LoginUserNameRequest request);

        /// <summary>
        /// Hàm kiểm tra thông tin đăng nhập (gửi email cảnh báo đăng nhập nếu bị khóa)
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
        /// Hàm xử lý đăng nhập cho tài khoản không bật xác thực 2 bước
        /// </summary>
        /// <typeparam name="LoginRequest"></typeparam>
        /// <param name="request"></param>
        /// <returns>
        /// Trả về thông báo lỗi (Message) nếu không thành công,
        /// Trả về chuỗi token người dùng đã mã hóa (ReturnedObject) nếu thành công
        /// </returns>
        Task<ApiResult<string>> Login(LoginRequest request);

        /// <summary>
        /// Hàm xử lý gửi email kích hoạt tài khoản
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <param name="activatedEmailBaseUrl"></param>
        /// <typeparam name="string"></typeparam>
        /// <param name="userNameOrEmail"></param>
        /// <returns>
        /// Trả về thông báo lỗi (Message) nếu không thành công,
        /// Trả về true (ReturnedObject) nếu thành công
        /// </returns>
        Task<ApiResult<bool>> ActiveAccount(string activatedEmailBaseUrl, string userNameOrEmail);

        /// <summary>
        /// Hàm xử lý gửi mã xác thực danh tính theo phương thức xác thực được chọn
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <param name="userNameOrEmail"></param>
        /// <typeparam name="string"></typeparam>
        /// <param name="provider"></param>
        /// <returns>
        /// Trả về thông báo lỗi (Message) nếu không thành công,
        /// Trả về true (ReturnedObject) nếu thành công
        /// </returns>
        Task<ApiResult<bool>> GetAuthenticationMethod(string userNameOrEmail, string provider);

        /// <summary>
        /// Hàm xử lý đối chiếu mã xác thực bước 2 trường hợp đăng nhập cho tài khoản bật 2FA
        /// </summary>
        /// <typeparam name="Authenticate2FARequest"></typeparam>
        /// <param name="request"></param>
        /// <returns>
        /// Trả về thông báo lỗi (Message) nếu không thành công,
        /// Trả về chuỗi token người dùng đã mã hóa (ReturnedObject) nếu thành công
        /// </returns>
        Task<ApiResult<string>> Authenticate2FA(Authenticate2FARequest request);

        /// <summary>
        /// Hàm xử lý đối chiếu mã xác thực danh tính trường hợp quên mật khẩu
        /// </summary>
        /// <typeparam name="AuthenticateForgotPasswordRequest"></typeparam>
        /// <param name="request"></param>
        /// <returns>
        /// Trả về thông báo lỗi (Message) nếu không thành công,
        /// Trả về chuỗi token ResetPassword đã mã hóa (ReturnedObject) nếu thành công
        /// </returns>
        Task<ApiResult<string>> AuthenticateForgotPassword(AuthenticateForgotPasswordRequest request);

        /// <summary>
        /// Hàm xử lý mở khóa tài khoản
        /// </summary>
        /// <typeparam name="Guid"></typeparam>
        /// <param name="userId"></param>
        /// <returns>
        /// Trả về thông báo lỗi (Message) nếu không thành công,
        /// Trả về true (ReturnedObject) nếu thành công
        /// </returns>
        Task<ApiResult<bool>> UnlockOut(Guid userId);

        /// <summary>
        /// Hàm xử lý đặt lại mật khẩu (Đồng nghĩa email được xác thực)
        /// </summary>
        /// <typeparam name="ResetPasswordRequest"></typeparam>
        /// <param name="request"></param>
        /// <returns>
        /// Trả về thông báo lỗi (Message) nếu không thành công,
        /// Trả về true (ReturnedObject) nếu thành công
        /// </returns>
        Task<ApiResult<bool>> ResetPassword(ResetPasswordRequest request);

        /// <summary>
        /// Hàm xử lý email xác thực (Không bắt buộc đổi mật khẩu)
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
        /// Hàm xử lý thay đổi email
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
        /// Hàm xử lý kích hoạt email (Bắt buộc thay đổi email)
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <param name="userNameOrEmail"></param>
        /// <typeparam name="string"></typeparam>
        /// <param name="token"></param>
        /// <returns>
        /// Trả về thông báo lỗi (Message) nếu không thành công,
        /// Trả về token đặt lại mật khẩu (ReturnedObject) nếu thành công
        /// </returns>
        Task<ApiResult<string>> ActivatedEmail(string userNameOrEmail, string token);
    }
}
