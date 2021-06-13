using System;
using System.Threading.Tasks;
using ZiTechDev.CommonModel.Engines.CustomResult;
using ZiTechDev.CommonModel.Requests.Auth;
using ZiTechDev.CommonModel.Requests.User;

namespace ZiTechDev.AdminSite.ApiClientServices.Auth
{
    public interface IAuthApiClient
    {
        /// <summary>
        /// Gọi tới API đăng nhập
        /// </summary>
        /// <typeparam name="LoginRequest"></typeparam>
        /// <param name="request"></param>
        /// <returns>
        /// Trả về thông báo lỗi (Message) nếu không thành công,
        /// Trả về chuỗi token người dùng đã mã hóa (ReturnedObject) nếu thành công
        /// </returns>
        Task<ApiResult<string>> Login(LoginRequest request);

        /// <summary>
        /// Gọi tới API kiểm tra tình trạng khóa tài khoản
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <param name="userName"></param>
        /// <typeparam name="string"></typeparam>
        /// <param name="forgotPasswordBaseUrl"></param>
        /// <returns>
        /// Trả về số lần đăng nhập thất bại (Message) nếu tài khoản không bị khóa,
        /// Trả về tổng thời gian khóa còn lại tính bằng giây (ReturnedObject) đồng thời gửi cảnh báo đăng nhập đến email người dùng nếu tài khoản bị khóa mà vẫn cố đăng nhập
        /// </returns>
        Task<ApiResult<int>> LoginWarning(string userName, string forgotPasswordBaseUrl);

        /// <summary>
        /// Gọi tới API quên mật khẩu
        /// </summary>
        /// <typeparam name="ForgotPasswordRequest"></typeparam>
        /// <param name="request"></param>
        /// <returns>
        /// Trả về thông báo lỗi (Message) nếu không thành công,
        /// Trả về chuỗi token ResetPassword đã mã hóa (ReturnedObject) nếu thành công
        /// </returns>
        Task<ApiResult<string>> ForgotPassword(ForgotPasswordRequest request);

        /// <summary>
        /// Gọi tới API gửi thông báo xác nhận quên mật khẩu đến email người dùng cho phép chuyến hướng đặt lại mật khẩu
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <param name="email">Gửi cho ai?</param>
        /// <typeparam name="string"></typeparam>
        /// <param name="token">Mã thông báo là gì?</param>
        /// <typeparam name="string"></typeparam>
        /// <param name="resetPasswordBaseUrl">Đường dẫn chuyển hướng cơ sở</param>
        /// <returns>
        /// Trả về thông báo lỗi (Message) nếu không thành công,
        /// Trả về true (ReturnedObject) nếu thành công
        /// </returns>
        Task<ApiResult<bool>> SendForgotPasswordEmail(string email, string token, string resetPasswordBaseUrl);

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
        /// Gọi tới API đặt lại mật khẩu
        /// </summary>
        /// <typeparam name="ResetPasswordRequest"></typeparam>
        /// <param name="request"></param>
        /// <returns>
        /// Trả về thông báo lỗi (Message) nếu không thành công,
        /// Trả về true (ReturnedObject) nếu thành công
        /// </returns>
        Task<ApiResult<bool>> ResetPassword(ResetPasswordRequest request);

        /// <summary>
        /// Gọi tới API xử lý email xác thực
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
    }
}
