using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ZiTechDev.CommonModel.Requests.CommonItems;

namespace ZiTechDev.CommonModel.Requests.Auth
{
    public class Authenticate2FARequest : LoginRequest
    {
        [Display(Name = "Mã xác thực")]
        public string PinCode { get; set; }
    }
}
