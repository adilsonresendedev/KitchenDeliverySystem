﻿namespace KitchenDeliverySystem.Domain.Entities
{
    public class User : BaseEntity
    {
        protected User() { }

        public User(bool isActive, string firstName, string lastName, string userName, byte[] passwordHash, byte[] passwordSalt)
        {
            IsActive = isActive;
            FirstName = firstName;
            LastName = lastName;
            Username = userName;
            IsActive = isActive;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }

        public void Update(bool isActive, string firstName, string lastName, string userName, byte[] passwordHash, byte[] passwordSalt)
        {
            IsActive = isActive;
            FirstName = firstName;
            LastName = lastName;
            Username = userName;
            IsActive = isActive;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }

        public bool IsActive { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Username { get; private set; }
        public byte[] PasswordHash { get; private set; }
        public byte[] PasswordSalt { get; private set; }
    }
}
