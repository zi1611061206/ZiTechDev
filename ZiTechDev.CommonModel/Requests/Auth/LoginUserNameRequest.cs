using System.ComponentModel.DataAnnotations;

namespace ZiTechDev.CommonModel.Requests.Auth
{
    public class LoginUserNameRequest
    {
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }
    }
}
