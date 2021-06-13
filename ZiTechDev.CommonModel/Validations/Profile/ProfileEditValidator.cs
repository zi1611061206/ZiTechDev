using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.CommonModel.Requests.Profile;

namespace ZiTechDev.CommonModel.Validations.Profile
{
    public class ProfileEditValidator : AbstractValidator<ProfileEditRequest>
    {
        public ProfileEditValidator()
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
