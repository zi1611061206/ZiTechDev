using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.Business.Requests.User;

namespace ZiTechDev.Business.Validations.User
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordValidator()
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("Old Password is required");
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New Password is required")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$").WithMessage("Password must be at least 8 characters, at least 1 uppercase, 1 lowercase, 1 number and 1 special character");
            RuleFor(x => x.PasswordConfirmation)
                .NotEmpty().WithMessage("Confirm Password is required")
                .Equal(x=>x.NewPassword).WithMessage("Confirmation Password is not match");
        }
    }
}
