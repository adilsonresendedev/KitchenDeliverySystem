using FluentValidation.TestHelper;
using KitchenDeliverySystem.Api.Validation;
using KitchenDeliverySystem.Domain.Validation;
using KitchenDeliverySystem.Dto.Order;

namespace KitchenDeliverySystem.Test.Unit.Presentation.Validation
{
    public class CreateOrderItemDtoValidatorTests
    {
        private readonly CreateOrderItemDtoValidator _validator;

        public CreateOrderItemDtoValidatorTests()
        {
            _validator = new CreateOrderItemDtoValidator();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Have_Error_When_Name_Is_Null_Or_Empty(string name)
        {
            // Arrange
            var orderItemDto = new CreateOrderItemDto { Name = name };

            // Act
            var result = _validator.TestValidate(orderItemDto);

            // Assert
            result.ShouldHaveValidationErrorFor(item => item.Name)
                  .WithErrorMessage(ValidationConstants.OrderItemNameIsInvalid);
        }

        [Fact]
        public void Should_Have_Error_When_Name_Exceeds_MaxLength()
        {
            // Arrange
            var orderItemDto = new CreateOrderItemDto { Name = new string('A', 31) };

            // Act
            var result = _validator.TestValidate(orderItemDto);

            // Assert
            result.ShouldHaveValidationErrorFor(item => item.Name)
                  .WithErrorMessage(ValidationConstants.OrderItemNameTooLong);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Name_Is_Valid()
        {
            // Arrange
            var orderItemDto = new CreateOrderItemDto { Name = "Valid Item Name" };

            // Act
            var result = _validator.TestValidate(orderItemDto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(item => item.Name);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_Have_Error_When_Quantity_Is_Less_Than_Or_Equal_To_Zero(int quantity)
        {
            // Arrange
            var orderItemDto = new CreateOrderItemDto { Quantity = quantity };

            // Act
            var result = _validator.TestValidate(orderItemDto);

            // Assert
            result.ShouldHaveValidationErrorFor(item => item.Quantity)
                  .WithErrorMessage(ValidationConstants.OrderItemQuantityIsInvalid);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Quantity_Is_Valid()
        {
            // Arrange
            var orderItemDto = new CreateOrderItemDto { Quantity = 5 };

            // Act
            var result = _validator.TestValidate(orderItemDto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(item => item.Quantity);
        }

        [Fact]
        public void Should_Have_Error_When_Notes_Exceed_MaxLength()
        {
            // Arrange
            var orderItemDto = new CreateOrderItemDto { Notes = new string('B', 201) };

            // Act
            var result = _validator.TestValidate(orderItemDto);

            // Assert
            result.ShouldHaveValidationErrorFor(item => item.Notes)
                  .WithErrorMessage(ValidationConstants.OrderItemNotesIsInvalid);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Notes_Are_Valid()
        {
            // Arrange
            var orderItemDto = new CreateOrderItemDto { Notes = "This is a valid note." };

            // Act
            var result = _validator.TestValidate(orderItemDto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(item => item.Notes);
        }
    }
}
