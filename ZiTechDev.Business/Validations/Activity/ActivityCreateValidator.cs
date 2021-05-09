using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.Business.Requests.Activity;

namespace ZiTechDev.Business.Validations.Activity
{
    public class ActivityCreateValidator : AbstractValidator<ActivityCreateRequest>
    {
        public ActivityCreateValidator()
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(50).WithMessage("Name is up to 50 characters long");
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description is up to 500 characters long");
        }
    }
}
