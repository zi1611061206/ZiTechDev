using System.ComponentModel.DataAnnotations;

namespace ZiTechDev.CommonModel.Requests.Auth
{
    public class LoginRequest
    {
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Ghi nhớ tôi")]
        public bool RememberMe { get; set; }
    }
}
