using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ZiTechDev.CommonModel.Requests.CommonItems;

namespace ZiTechDev.CommonModel.Requests.Auth
{
    public class Authenticate2FARequest
    {
        public string UserNameOrEmail { get; set; }
        public bool RememberMe { get; set; }
        public string Provider { get; set; }

        [Display(Name = "Mã xác thực")]
        public string PinCode { get; set; }

        [Display(Name = "Ghi nhớ thiết bị này")]
        public bool IsRememberClient { get; set; } = false;
    }
}
