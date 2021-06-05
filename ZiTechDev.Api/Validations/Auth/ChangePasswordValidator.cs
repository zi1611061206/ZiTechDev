using FluentValidation;
using ZiTechDev.CommonModel.Requests.Auth;

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
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
                .WithMessage("Mật khẩu dài tối thiểu 8 ký tự chứa ít nhất: 1 ký tự viết in, 1 ký tự viết thường, 1 ký tự số và 1 ký tự đặc biệt");
            RuleFor(x => x.PasswordConfirmation)
                .NotEmpty().WithMessage("Trường thông tin này là bắt buộc")
                .Equal(x => x.NewPassword).WithMessage("Mật khẩu xác nhận không khớp");
        }
    }
}
