using FluentValidation.TestHelper;
using KitchenDeliverySystem.Api.Validation;
using KitchenDeliverySystem.Domain.Validation;
using KitchenDeliverySystem.Dto.Order;

namespace KitchenDeliverySystem.Test.Unit.Presentation.Validation
{
    public class UpdateOrderDtoValidatorTests
    {
        private readonly CreateOrderDtoValidator _validator;

        public UpdateOrderDtoValidatorTests()
        {
            _validator = new CreateOrderDtoValidator();
        }

        [Theory]
        [InlineData(null)] // Test case: null CustomerName
        [InlineData("")]   // Test case: empty CustomerName
        [InlineData(" ")]  // Test case: whitespace CustomerName
        public void Should_Have_Error_When_CustomerName_Is_Null_Or_Empty(string customerName)
        {
            // Arrange
            var orderDto = new CreateOrderDto { CustomerName = customerName };

            // Act
            var result = _validator.TestValidate(orderDto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CustomerName)
                  .WithErrorMessage(ValidationConstants.CustomerNameIsInvalid);
        }

        [Fact]
        public void Should_Have_Error_When_CustomerName_Exceeds_MaxLength()
        {
            // Arrange
            var orderDto = new CreateOrderDto { CustomerName = new string('A', 31) };

            // Act
            var result = _validator.TestValidate(orderDto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CustomerName)
                  .WithErrorMessage(ValidationConstants.CustomerNameTooLong);
        }

        [Fact]
        public void Should_Not_Have_Error_When_CustomerName_Is_Valid()
        {
            // Arrange
            var orderDto = new CreateOrderDto { CustomerName = "ValidCustomerName" };

            // Act
            var result = _validator.TestValidate(orderDto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.CustomerName);
        }
    }
}
