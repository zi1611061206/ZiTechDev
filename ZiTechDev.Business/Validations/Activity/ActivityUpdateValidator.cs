using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.Business.Requests.Activity;

namespace ZiTechDev.Business.Validations.Activity
{
    public class ActivityUpdateValidator : AbstractValidator<ActivityUpdateRequest>
    {
        public ActivityUpdateValidator()
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Trường thông tin này là bắt buộc")
                .MaximumLength(50).WithMessage("Không thể vượt quá 50 ký tự");
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Không thể vượt quá 500 ký tự");
        }
    }
}
