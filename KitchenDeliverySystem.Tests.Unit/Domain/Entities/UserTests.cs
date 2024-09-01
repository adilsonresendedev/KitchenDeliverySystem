using KitchenDeliverySystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenDeliverySystem.Test.Unit.Domain.Entities
{
    public class UserTests
    {
        [Fact]
        public void Constructor_Should_Set_Properties()
        {
            // Arrange
            bool isActive = true;
            string firstName = "John";
            string lastName = "Doe";
            string userName = "johndoe";
            byte[] passwordHash = new byte[] { 1, 2, 3 };
            byte[] passwordSalt = new byte[] { 4, 5, 6 };

            // Act
            var user = new User(isActive, firstName, lastName, userName, passwordHash, passwordSalt);

            // Assert
            Assert.True(user.IsActive);
            Assert.Equal(firstName, user.FirstName);
            Assert.Equal(lastName, user.LastName);
            Assert.Equal(userName, user.Username);
            Assert.Equal(passwordHash, user.PasswordHash);
            Assert.Equal(passwordSalt, user.PasswordSalt);
        }

        [Fact]
        public void Update_Should_Modify_Properties()
        {
            // Arrange
            var user = new User(true, "John", "Doe", "johndoe", new byte[] { 1, 2, 3 }, new byte[] { 4, 5, 6 });

            bool newIsActive = false;
            string newFirstName = "Jane";
            string newLastName = "Smith";
            string newUserName = "janesmith";
            byte[] newPasswordHash = new byte[] { 7, 8, 9 };
            byte[] newPasswordSalt = new byte[] { 10, 11, 12 };

            // Act
            user.Update(newIsActive, newFirstName, newLastName, newUserName, newPasswordHash, newPasswordSalt);

            // Assert
            Assert.False(user.IsActive);
            Assert.Equal(newFirstName, user.FirstName);
            Assert.Equal(newLastName, user.LastName);
            Assert.Equal(newUserName, user.Username);
            Assert.Equal(newPasswordHash, user.PasswordHash);
            Assert.Equal(newPasswordSalt, user.PasswordSalt);
        }
    }
}
