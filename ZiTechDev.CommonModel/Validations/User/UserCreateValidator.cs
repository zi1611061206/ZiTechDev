using FluentValidation;
using System;
using ZiTechDev.CommonModel.Requests.User;
using ZiTechDev.CommonModel.Validations.Auth;

namespace ZiTechDev.CommonModel.Validations.User
{
    public class UserCreateValidator : AbstractValidator<UserCreateRequest>
    {
        public UserCreateValidator()
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Trường thông tin này là bắt buộc")
                .MaximumLength(20).WithMessage("Không thể vượt quá 20 ký tự");
            RuleFor(x => x.MiddleName)
                .MaximumLength(20).WithMessage("Không thể vượt quá 20 ký tự");
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Trường thông tin này là bắt buộc")
                .MaximumLength(20).WithMessage("Không thể vượt quá 20 ký tự");
            RuleFor(x => x.DisplayName)
                .MaximumLength(50).WithMessage("Không thể vượt quá 50 ký tự");
            RuleFor(x => x.DateOfBirth)
                .GreaterThan(DateTime.Now.AddYears(-150)).WithMessage("Bạn không thể vượt quá 150 tuổi")
                .LessThan(DateTime.Now.AddYears(-1)).WithMessage("Bạn không thể nào mới ra đời được! :D");
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Trường thông tin này là bắt buộc")
                .MaximumLength(11).WithMessage("Không thể vượt quá 11 ký tự");
            RuleFor(x => x.Password)
                .CusTomPasswordRule();
            RuleFor(x => x.PasswordConfirmation)
                .NotEmpty().WithMessage("Trường thông tin này là bắt buộc")
                .Equal(x => x.Password).WithMessage("Mật khẩu xác nhận không khớp");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Trường thông tin này là bắt buộc")
                .EmailAddress().WithMessage("Định dạng không hợp lệ (VD: MailName@MailServer)");
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Trường thông tin này là bắt buộc")
                .MaximumLength(50).WithMessage("Không thể vượt quá 50 ký tự");
        }
    }
}
