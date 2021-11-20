using FluentValidation;
using ZiTechDev.CommonModel.Requests.Auth;

namespace ZiTechDev.CommonModel.Validations.Auth
{
    public class LoginUserNameValidator : AbstractValidator<LoginUserNameRequest>
    {
        public LoginUserNameValidator()
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.UserNameOrEmail)
                .NotEmpty().WithMessage("Tên đăng nhập là bắt buộc");
        }
    }
}
