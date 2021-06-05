using FluentValidation;
using ZiTechDev.CommonModel.Requests.User;

namespace ZiTechDev.Business.Validations.User
{
    public class UserUpdateValidator : AbstractValidator<UserUpdateRequest>
    {
        public UserUpdateValidator()
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
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Trường thông tin này là bắt buộc")
                .MaximumLength(11).WithMessage("Không thể vượt quá 11 ký tự");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Trường thông tin này là bắt buộc")
                .EmailAddress().WithMessage("Định dạng không hợp lệ (VD: MailName@MailServer)");
        }
    }
}
