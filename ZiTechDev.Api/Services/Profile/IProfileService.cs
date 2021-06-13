using System;
using System.Threading.Tasks;
using ZiTechDev.CommonModel.Engines.CustomResult;
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
    }
}
