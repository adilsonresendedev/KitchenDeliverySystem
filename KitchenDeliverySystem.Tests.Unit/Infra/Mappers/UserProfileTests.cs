using AutoMapper;
using KitchenDeliverySystem.Domain.Entities;
using KitchenDeliverySystem.Domain.Enums;
using KitchenDeliverySystem.Dto.Order;
using KitchenDeliverySystem.Dto.User;
using KitchenDeliverySystem.Infra.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenDeliverySystem.Test.Unit.Infra.Mappers
{
    public class UserProfileTests
    {
        private readonly IMapper _mapper;

        public UserProfileTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserProfile>();
            });

            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void Should_Have_Valid_Configuration()
        {
            // Act & Assert
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Fact]
        public void Should_Map_User_To_UserDto()
        {
            // Arrange
            var user = new User(
                isActive: true,
                firstName: "John",
                lastName: "Doe",
                userName: "johndoe",
                passwordHash: new byte[] { 1, 2, 3 },
                passwordSalt: new byte[] { 4, 5, 6 }
            );

            // Act
            var result = _mapper.Map<UserDto>(user);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.FirstName, result.FirstName);
            Assert.Equal(user.LastName, result.LastName);
            Assert.Equal(user.Username, result.Username);
            Assert.Equal(user.IsActive, result.IsActive);
        }

        [Fact]
        public void Should_Map_UserDto_To_User()
        {
            // Arrange
            var userDto = new UserDto
            {
                IsActive = true,
                FirstName = "John",
                LastName = "Doe",
                Username = "johndoe"
            };

            // Act
            var result = _mapper.Map<User>(userDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userDto.FirstName, result.FirstName);
            Assert.Equal(userDto.LastName, result.LastName);
            Assert.Equal(userDto.Username, result.Username);
            Assert.Equal(userDto.IsActive, result.IsActive);

            Assert.Null(result.PasswordHash);
            Assert.Null(result.PasswordSalt);
        }
    }
}
