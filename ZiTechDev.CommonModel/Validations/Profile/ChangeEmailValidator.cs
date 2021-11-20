using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.CommonModel.Requests.Profile;

namespace ZiTechDev.CommonModel.Validations.Profile
{
    public class ChangeEmailValidator : AbstractValidator<ChangeEmailRequest>
    {
        public ChangeEmailValidator()
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.NewEmail.ToLower())
                .NotEmpty().WithMessage("Trường thông tin này là bắt buộc")
                .EmailAddress().WithMessage("Định dạng không hợp lệ (VD: MailName@MailServer)")
                .NotEqual(x=>x.OldEmail.ToLower()).WithMessage("Email không có gì thay đổi");
        }
    }
}
