using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZiTechDev.CommonModel.Requests.Profile
{
    public class AuthenticateCodeRequest
    {
        public string ManualEntryKey { get; set; } = null;

        public string QrCodeSetupImageUrl { get; set; } = null;

        [Display(Name = "Mã PIN")]
        public string PinCode { get; set; }
    }
}
