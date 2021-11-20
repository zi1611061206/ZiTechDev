using System.ComponentModel.DataAnnotations;

namespace ZiTechDev.CommonModel.Requests.Auth
{
    public class AuthenticateForgotPasswordRequest
    {
        public string UserNameOrEmail { get; set; }
        public string Provider { get; set; }

        [Display(Name = "Mã xác thực")]
        public string PinCode { get; set; }
    }
}
