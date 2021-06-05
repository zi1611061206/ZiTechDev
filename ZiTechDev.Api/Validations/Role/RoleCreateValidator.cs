using FluentValidation;
using ZiTechDev.CommonModel.Requests.Role;

namespace ZiTechDev.Business.Validations.Role
{
    public class RoleCreateValidator : AbstractValidator<RoleCreateRequest>
    {
        public RoleCreateValidator()
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
