using FluentValidation;
using KitchenDeliverySystem.Domain.Validation;
using KitchenDeliverySystem.Dto.Order;

namespace KitchenDeliverySystem.Api.Validation
{
    public class CreateOrderItemDtoValidator : AbstractValidator<CreateOrderItemDto>
    {
        public CreateOrderItemDtoValidator()
        {
            RuleFor(item => item.Name)
                .NotEmpty().WithMessage(ValidationConstants.OrderItemNameIsInvalid)
                .MaximumLength(30).WithMessage(ValidationConstants.OrderItemNameTooLong);

            RuleFor(item => item.Quantity)
                .GreaterThan(0).WithMessage(ValidationConstants.OrderItemQuantityIsInvalid);

            RuleFor(item => item.Notes)
                .MaximumLength(200).WithMessage(ValidationConstants.OrderItemNotesIsInvalid);
        }
    }
}
