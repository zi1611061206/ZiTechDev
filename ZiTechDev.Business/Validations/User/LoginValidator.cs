using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.Business.Interfaces;
using ZiTechDev.Business.Requests.User;
using ZiTechDev.Business.Services;
using ZiTechDev.Data.Context;

namespace ZiTechDev.Business.Validations.User
{
    public class LoginValidator : AbstractValidator<LoginRequest>
    {
        private readonly IUserService _userService;

        public LoginValidator(IUserService userService)
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
