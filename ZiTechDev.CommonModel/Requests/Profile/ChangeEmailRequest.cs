using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZiTechDev.CommonModel.Requests.Profile
{
    public class ChangeEmailRequest
    {
        [Display(Name = "Email cũ")]
        [DataType(DataType.EmailAddress)]
        public string OldEmail { get; set; }

        [Display(Name = "Email mới")]
        [DataType(DataType.EmailAddress)]
        public string NewEmail { get; set; }
    }
}
