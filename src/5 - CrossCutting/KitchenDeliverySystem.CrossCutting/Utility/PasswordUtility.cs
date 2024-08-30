using KitchenDeliverySystem.Dto.User;
using System.Security.Cryptography;
using System.Text;

namespace KitchenDeliverySystem.CrossCutting.Utility
{
    public static class PasswordUtility
    {
        public static PasswordHashDto CreatePasswordHash(string password)
        {
            var passwordHashDto = new PasswordHashDto();
            using (var hmac = new HMACSHA512())
            {
                passwordHashDto.PasswordSalt = hmac.Key;
                passwordHashDto.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }

            return passwordHashDto;
        }

        public static bool CheckHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmacsha = new HMACSHA512(passwordSalt))
            {
                var hashCalculado = hmacsha.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < hashCalculado.Length; i++)
                {
                    if (hashCalculado[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
