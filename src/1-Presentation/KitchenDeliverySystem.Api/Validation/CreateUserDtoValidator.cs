using FluentValidation;
using KitchenDeliverySystem.Domain.Validation;
using KitchenDeliverySystem.Dto.User;

namespace KitchenDeliverySystem.Api.Validation
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage(ValidationConstants.FirstNameIsInvalid)
                .MaximumLength(50).WithMessage(ValidationConstants.FirstNameIsInvalid);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage(ValidationConstants.LastNameIsInvalid)
                .MaximumLength(50).WithMessage(ValidationConstants.LastNameIsInvalid);

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage(ValidationConstants.UserNameIsInvalid)
                .MaximumLength(30).WithMessage(ValidationConstants.UserNameIsInvalid);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(ValidationConstants.PasswordIsInvalid)
                .MinimumLength(8).WithMessage(ValidationConstants.PasswordIsInvalid);
        }
    }
}
