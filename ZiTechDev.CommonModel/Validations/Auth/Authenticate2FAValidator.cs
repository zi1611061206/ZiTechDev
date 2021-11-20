using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.CommonModel.Requests.Auth;

namespace ZiTechDev.CommonModel.Validations.Auth
{
    public class Authenticate2FAValidator : AbstractValidator<Authenticate2FARequest>
    {
        public Authenticate2FAValidator()
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.PinCode)
                .NotEmpty().WithMessage("Trường thông tin này là bắt buộc");
        }
    }
}
