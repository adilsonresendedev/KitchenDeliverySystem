using FluentValidation;
using KitchenDeliverySystem.Domain.Validation;
using KitchenDeliverySystem.Dto.Order;

namespace KitchenDeliverySystem.Api.Validation
{
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator()
        {
            RuleFor(order => order.CustomerName)
                .NotEmpty().WithMessage(ValidationConstants.CustomerNameIsInvalid)
                .MaximumLength(30).WithMessage(ValidationConstants.CustomerNameTooLong);
        }
    }
}
