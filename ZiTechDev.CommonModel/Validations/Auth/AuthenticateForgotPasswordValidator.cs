using FluentValidation;
using ZiTechDev.CommonModel.Requests.Auth;

namespace ZiTechDev.CommonModel.Validations.Auth
{
    public class AuthenticateForgotPasswordValidator : AbstractValidator<AuthenticateForgotPasswordRequest>
    {
        public AuthenticateForgotPasswordValidator()
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.PinCode)
                .NotEmpty().WithMessage("Trường thông tin này là bắt buộc");
        }
    }
}
