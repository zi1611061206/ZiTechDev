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
                .NotEmpty().WithMessage("Trường thông tin này là bắt buộc")
                .MinimumLength(8).WithMessage("Mật khẩu phải chứa tối thiểu 8 ký tự")
                .Matches("[A-Z]").WithMessage("Mật khẩu phải chứa tối thiểu 1 ký tự viết hoa")
                .Matches("[a-z]").WithMessage("Mật khẩu phải chứa tối thiểu 1 ký tự viết thường")
                .Matches("[0-9]").WithMessage("Mật khẩu phải chứa tối thiểu 1 ký tự số")
                .Matches("[^a-zA-Z0-9]").WithMessage("Mật khẩu phải chứa tối thiểu 1 ký tự đặc biệt");
            return options;
        }
    }
}
