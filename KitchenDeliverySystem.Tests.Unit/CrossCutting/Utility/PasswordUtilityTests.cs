using FluentAssertions;
using KitchenDeliverySystem.CrossCutting.Utility;
using KitchenDeliverySystem.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KitchenDeliverySystem.Test.Unit.CrossCutting.Utility
{
    public class PasswordUtilityTests
    {
        [Fact]
        public void CreatePasswordHash_ShouldReturnValidHashAndSalt()
        {
            // Arrange
            var password = "securePassword";

            // Act
            var result = PasswordUtility.CreatePasswordHash(password);

            // Assert
            result.PasswordHash.Should().NotBeNullOrEmpty();
            result.PasswordSalt.Should().NotBeNullOrEmpty();
            result.PasswordHash.Length.Should().Be(64);
            result.PasswordSalt.Length.Should().Be(128);
        }

        [Fact]
        public void CheckHash_ShouldReturnTrueForValidPassword()
        {
            // Arrange
            var password = "securePassword";
            var passwordHashDto = PasswordUtility.CreatePasswordHash(password);

            // Act
            var isValid = PasswordUtility.CheckHash(password, passwordHashDto.PasswordHash, passwordHashDto.PasswordSalt);

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void CheckHash_ShouldReturnFalseForInvalidPassword()
        {
            // Arrange
            var correctPassword = "correctPassword";
            var incorrectPassword = "incorrectPassword";
            var passwordHashDto = PasswordUtility.CreatePasswordHash(correctPassword);

            // Act
            var isValid = PasswordUtility.CheckHash(incorrectPassword, passwordHashDto.PasswordHash, passwordHashDto.PasswordSalt);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void CreateToken_ShouldReturnValidJwtToken()
        {
            // Arrange
            var user = new User(
                true,
                "firstName",
                "lastName",
                "userName",
                new byte[64],
                new byte[64]);

            var key = "this_is_a_very_secure_and_long_test_token_key_1234";

            // Act
            var token = PasswordUtility.CreateToken(user, key);

            // Assert
            token.Should().NotBeNullOrEmpty();

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            jwtToken.Should().NotBeNull();

           var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            handler.ValidateToken(token, validationParameters, out _);
        }
    }
}
