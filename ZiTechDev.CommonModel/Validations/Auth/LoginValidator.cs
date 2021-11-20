using FluentValidation;
using ZiTechDev.CommonModel.Requests.Auth;

namespace ZiTechDev.CommonModel.Validations.Auth
{
    public class LoginValidator : AbstractValidator<LoginRequest>
    {
        public LoginValidator()
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.UserNameOrEmail)
                .NotEmpty().WithMessage("Tên đăng nhập là bắt buộc");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Mật khẩu là bắt buộc");
        }
    }
}
