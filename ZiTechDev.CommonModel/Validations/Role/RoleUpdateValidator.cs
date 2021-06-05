using FluentValidation;
using ZiTechDev.CommonModel.Requests.Role;

namespace ZiTechDev.CommonModel.Validations.Role
{
    public class RoleUpdateValidator : AbstractValidator<RoleUpdateRequest>
    {
        public RoleUpdateValidator()
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Trường thông tin này là bắt buộc")
                .MaximumLength(20).WithMessage("Không thể vượt quá 20 ký tự");
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Không thể vượt quá 500 ký tự");
        }
    }
}
