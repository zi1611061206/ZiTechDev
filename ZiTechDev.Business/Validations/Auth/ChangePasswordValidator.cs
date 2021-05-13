using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.Business.Requests.Auth;

namespace ZiTechDev.Business.Validations.Auth
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordValidator()
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("Trường thông tin này là bắt buộc");
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Trường thông tin này là bắt buộc")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$").WithMessage("Password must be at least 8 characters, at least 1 uppercase, 1 lowercase, 1 number and 1 special character");
            RuleFor(x => x.PasswordConfirmation)
                .NotEmpty().WithMessage("Trường thông tin này là bắt buộc")
                .Equal(x => x.NewPassword).WithMessage("Mật khẩu xác nhận không khớp");
        }
    }
}
