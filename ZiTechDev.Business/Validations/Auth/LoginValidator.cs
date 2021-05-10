using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.Business.Interfaces;
using ZiTechDev.Business.Requests.Auth;

namespace ZiTechDev.Business.Validations.Auth
{
    public class LoginValidator : AbstractValidator<LoginRequest>
    {
        private readonly IAuthService _userService;

        public LoginValidator(IAuthService userService)
        {
            _userService = userService;
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .Must((request, password) => _userService.IsMatchedUser(request.UserName, password, request.RememberMe).Result).WithMessage("Username or Password is incorrect");
        }
    }
}
