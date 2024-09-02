using FluentValidation.TestHelper;
using KitchenDeliverySystem.Api.Validation;
using KitchenDeliverySystem.Domain.Validation;
using KitchenDeliverySystem.Dto.User;

namespace KitchenDeliverySystem.Test.Unit.Presentation.Validation
{
    public class CreateUserDtoValidatorTests
    {
        private readonly CreateUserDtoValidator _validator;

        public CreateUserDtoValidatorTests()
        {
            _validator = new CreateUserDtoValidator();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Have_Error_When_FirstName_Is_Null_Or_Empty(string firstName)
        {
            // Arrange
            var userDto = new CreateUserDto { FirstName = firstName };

            // Act
            var result = _validator.TestValidate(userDto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                  .WithErrorMessage(ValidationConstants.FirstNameIsInvalid);
        }

        [Fact]
        public void Should_Have_Error_When_FirstName_Exceeds_MaxLength()
        {
            // Arrange
            var userDto = new CreateUserDto { FirstName = new string('A', 51) };

            // Act
            var result = _validator.TestValidate(userDto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                  .WithErrorMessage(ValidationConstants.FirstNameIsInvalid);
        }

        [Fact]
        public void Should_Not_Have_Error_When_FirstName_Is_Valid()
        {
            // Arrange
            var userDto = new CreateUserDto { FirstName = "ValidFirstName" };

            // Act
            var result = _validator.TestValidate(userDto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Have_Error_When_LastName_Is_Null_Or_Empty(string lastName)
        {
            // Arrange
            var userDto = new CreateUserDto { LastName = lastName };

            // Act
            var result = _validator.TestValidate(userDto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.LastName)
                  .WithErrorMessage(ValidationConstants.LastNameIsInvalid);
        }

        [Fact]
        public void Should_Have_Error_When_LastName_Exceeds_MaxLength()
        {
            // Arrange
            var userDto = new CreateUserDto { LastName = new string('B', 51) };

            // Act
            var result = _validator.TestValidate(userDto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.LastName)
                  .WithErrorMessage(ValidationConstants.LastNameIsInvalid);
        }

        [Fact]
        public void Should_Not_Have_Error_When_LastName_Is_Valid()
        {
            // Arrange
            var userDto = new CreateUserDto { LastName = "ValidLastName" };

            // Act
            var result = _validator.TestValidate(userDto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.LastName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Have_Error_When_UserName_Is_Null_Or_Empty(string userName)
        {
            // Arrange
            var userDto = new CreateUserDto { UserName = userName };

            // Act
            var result = _validator.TestValidate(userDto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserName)
                  .WithErrorMessage(ValidationConstants.UserNameIsInvalid);
        }

        [Fact]
        public void Should_Have_Error_When_UserName_Exceeds_MaxLength()
        {
            // Arrange
            var userDto = new CreateUserDto { UserName = new string('C', 31) };

            // Act
            var result = _validator.TestValidate(userDto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserName)
                  .WithErrorMessage(ValidationConstants.UserNameIsInvalid);
        }

        [Fact]
        public void Should_Not_Have_Error_When_UserName_Is_Valid()
        {
            // Arrange
            var userDto = new CreateUserDto { UserName = "ValidUserName" };

            // Act
            var result = _validator.TestValidate(userDto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.UserName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("short")]
        public void Should_Have_Error_When_Password_Is_Invalid(string password)
        {
            // Arrange
            var userDto = new CreateUserDto { Password = password };

            // Act
            var result = _validator.TestValidate(userDto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                  .WithErrorMessage(ValidationConstants.PasswordIsInvalid);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Password_Is_Valid()
        {
            // Arrange
            var userDto = new CreateUserDto { Password = "ValidPassword123" };

            // Act
            var result = _validator.TestValidate(userDto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }
    }
}
