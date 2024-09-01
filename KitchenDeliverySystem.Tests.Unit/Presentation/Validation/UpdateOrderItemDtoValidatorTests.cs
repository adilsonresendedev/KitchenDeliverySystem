using FluentValidation.TestHelper;
using KitchenDeliverySystem.Api.Validation;
using KitchenDeliverySystem.Domain.Validation;
using KitchenDeliverySystem.Dto.Order;

namespace KitchenDeliverySystem.Test.Unit.Presentation.Validation
{
    public class UpdateOrderItemDtoValidatorTests
    {
        private readonly UpdateOrderItemDtoValidator _validator;

        public UpdateOrderItemDtoValidatorTests()
        {
            _validator = new UpdateOrderItemDtoValidator();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Have_Error_When_Name_Is_Null_Or_Empty(string name)
        {
            // Arrange
            var orderItemDto = new UpdateOrderItemDto { Name = name };

            // Act
            var result = _validator.TestValidate(orderItemDto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage(ValidationConstants.OrderItemNameIsInvalid);
        }

        [Fact]
        public void Should_Have_Error_When_Name_Exceeds_MaxLength()
        {
            // Arrange
            var orderItemDto = new UpdateOrderItemDto { Name = new string('A', 31) };

            // Act
            var result = _validator.TestValidate(orderItemDto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage(ValidationConstants.OrderItemNameTooLong);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_Have_Error_When_Quantity_Is_Less_Than_Or_Equal_To_Zero(int quantity)
        {
            // Arrange
            var orderItemDto = new UpdateOrderItemDto { Quantity = quantity };

            // Act
            var result = _validator.TestValidate(orderItemDto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Quantity)
                  .WithErrorMessage(ValidationConstants.OrderItemQuantityIsInvalid);
        }

        [Fact]
        public void Should_Have_Error_When_Notes_Exceed_MaxLength()
        {
            // Arrange
            var orderItemDto = new UpdateOrderItemDto { Notes = new string('A', 201) };

            // Act
            var result = _validator.TestValidate(orderItemDto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Notes)
                  .WithErrorMessage(ValidationConstants.OrderItemNotesIsInvalid);
        }

        [Fact]
        public void Should_Not_Have_Error_When_OrderItem_Is_Valid()
        {
            // Arrange
            var orderItemDto = new UpdateOrderItemDto
            {
                Name = "ValidItemName",
                Quantity = 5,
                Notes = "Valid notes"
            };

            // Act
            var result = _validator.TestValidate(orderItemDto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
            result.ShouldNotHaveValidationErrorFor(x => x.Quantity);
            result.ShouldNotHaveValidationErrorFor(x => x.Notes);
        }
    }
}
