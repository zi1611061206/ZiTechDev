using Google.Authenticator;
using System;
using System.Threading.Tasks;
using ZiTechDev.CommonModel.Engines.CustomResult;
using ZiTechDev.CommonModel.Requests.CommonItems;
using ZiTechDev.CommonModel.Requests.Profile;

namespace ZiTechDev.Api.Services.Profile
{
    public interface IProfileService
    {
        /// <summary>
        /// Hàm xử lý lấy hồ sơ người dùng
        /// </summary>
        /// <typeparam name="Guid"></typeparam>
        /// <param name="userId"></param>
        /// <returns>
        /// Trả về thông báo lỗi (Message) nếu không thành công,
        /// Trả về ViewModel của hồ sơ người dùng (ReturnedObject) nếu thành công
        /// </returns>
        Task<ApiResult<ProfileViewModel>> GetProfile(Guid userId);

        /// <summary>
        /// Hàm xử lý chỉnh sửa hồ sơ người dùng
        /// </summary>
        /// <typeparam name="ProfileEditRequest"></typeparam>
        /// <param name="request"></param>
        /// <returns>
        /// Trả về thông báo lỗi (Message) nếu không thành công,
        /// Trả về true (ReturnedObject) nếu thành công
        /// </returns>
        Task<ApiResult<bool>> EditProfile(ProfileEditRequest request);

        /// <summary>
        /// Hàm xử lý đổi mật khẩu
        /// </summary>
        /// <typeparam name="ChangePasswordRequest"></typeparam>
        /// <param name="request"></param>
        /// <returns>
        /// Trả về thông báo lỗi (Message) nếu không thành công,
        /// Trả về true (ReturnedObject) nếu thành công
        /// </returns>
        Task<ApiResult<bool>> ChangePassword(ChangePasswordRequest request);

        /// <summary>
        /// Hàm gửi xác nhận thay đổi email
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <param name="changeEmailBaseUrl"></param>
        /// <typeparam name="ChangeEmailRequest"></typeparam>
        /// <param name="request"></param>
        /// <returns>
        /// Trả về thông báo lỗi (Message) nếu không thành công,
        /// Trả về true (ReturnedObject) nếu thành công
        /// </returns>
        Task<ApiResult<bool>> ChangeEmail(string changeEmailBaseUrl, ChangeEmailRequest request);

        /// <summary>
        /// Hàm tạo mã cài đặt cho thiết lập xác thực 2 bước với GoogleAuthenticator
        /// </summary>
        /// <typeparam name="Guid"></typeparam>
        /// <param name="userId"></param>
        /// <returns>
        /// Trả về thông báo lỗi (Message) nếu không thành công,
        /// Trả về SetupCode chứa mã cài đặt (ReturnedObject) nếu thành công
        /// </returns>
        Task<ApiResult<SetupCode>> Setup2FA(Guid userId);

        /// <summary>
        /// Hàm kiểm tra PIN và bật/tắt xác thực 2 bước với GoogleAuthenticator
        /// </summary>
        /// <typeparam name="Guid"></typeparam>
        /// <param name="userId"></param>
        /// <typeparam name="AuthenticateCodeRequest"></typeparam>
        /// <param name="request"></param>
        /// <returns>
        /// Trả về thông báo lỗi (Message) nếu không thành công,
        /// Trả về true (ReturnedObject) nếu thành công
        /// </returns>
        Task<ApiResult<bool>> Change2FA(Guid userId, AuthenticateCodeRequest request);
    }
}
