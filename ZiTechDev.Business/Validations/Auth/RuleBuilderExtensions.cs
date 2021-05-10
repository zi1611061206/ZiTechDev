using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Business.Validations.Auth
{
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilder<T, string> CusTomPasswordRule<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var options = ruleBuilder
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password is at least 8 characters")
                .Matches("[A-Z]").WithMessage("Password is at least 1 uppercase characters")
                .Matches("[a-z]").WithMessage("Password is at least 1 lowercase characters")
                .Matches("[0-9]").WithMessage("Password is at least 1 number characters")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password is at least 1 special characters");
            return options;
        }
    }
}
