using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.CommonModel.Requests.Auth;

namespace ZiTechDev.CommonModel.Validations.Auth
{
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordRequest>
    {
        public ForgotPasswordValidator()
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Trường thông tin này là bắt buộc")
                .EmailAddress().WithMessage("Định dạng không hợp lệ (VD: MailName@MailServer)");
        }
    }
}
