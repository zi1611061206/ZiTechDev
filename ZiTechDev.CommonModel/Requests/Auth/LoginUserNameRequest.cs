using System.ComponentModel.DataAnnotations;

namespace ZiTechDev.CommonModel.Requests.Auth
{
    public class LoginUserNameRequest
    {
        [Display(Name = "Tên đăng nhập hoặc email")]
        public string UserNameOrEmail { get; set; }
    }
}
