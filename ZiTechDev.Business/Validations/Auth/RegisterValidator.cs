using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.Business.Interfaces;
using ZiTechDev.Business.Requests.Auth;

namespace ZiTechDev.Business.Validations.Auth
{
    public class RegisterValidator : AbstractValidator<RegisterRequest>
    {
        private readonly IAuthService _userService;

        public RegisterValidator(IAuthService userService)
        {
            _userService = userService;
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
            RuleFor(x => x.DateOfBirth)
                .GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("Birthday can not greater than 100 years");
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone Number is required")
                .MaximumLength(11).WithMessage("Phone Number is up to 11 characters long");
            RuleFor(x => x.Password)
                .CusTomPasswordRule();
            RuleFor(x => x.PasswordConfirmation)
                .NotEmpty().WithMessage("Confirm Password is required")
                .Equal(x => x.Password).WithMessage("Confirmation Password is not match");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is Invalid");
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required")
                .MaximumLength(50).WithMessage("Username is up to 50 characters long")
                .Must((request, userName) => !_userService.IsExistedUserName(userName).Result).WithMessage("Username must be unique");
        }
    }
}
