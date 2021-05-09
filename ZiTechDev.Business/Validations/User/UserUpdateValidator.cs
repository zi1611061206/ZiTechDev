using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.Business.Requests.User;

namespace ZiTechDev.Business.Validations.User
{
    public class UserUpdateValidator : AbstractValidator<UserUpdateRequest>
    {
        public UserUpdateValidator()
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required")
                .MaximumLength(20).WithMessage("First Name is up to 20 characters long");
            RuleFor(x => x.MiddleName)
                .MaximumLength(20).WithMessage("Middle Name is up to 20 characters long");
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required")
                .MaximumLength(20).WithMessage("Last Name is up to 20 characters long");
            RuleFor(x => x.DisplayName)
                .NotEmpty().WithMessage("Display Name is required")
                .MaximumLength(50).WithMessage("Display Name is up to 50 characters long");
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone Number is required")
                .MaximumLength(11).WithMessage("Phone Number is up to 11 characters long");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is Invalid");
        }
    }
}
