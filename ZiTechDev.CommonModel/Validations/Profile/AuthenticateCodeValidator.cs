using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.CommonModel.Requests.Profile;

namespace ZiTechDev.CommonModel.Validations.Profile
{
    public class AuthenticateCodeValidator : AbstractValidator<AuthenticateCodeRequest>
    {
        public AuthenticateCodeValidator()
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.PinCode)
                .NotEmpty().WithMessage("Trường thông tin này là bắt buộc");
        }
    }
}
