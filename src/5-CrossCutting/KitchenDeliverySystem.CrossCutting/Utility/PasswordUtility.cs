using KitchenDeliverySystem.Domain.Entities;
using KitchenDeliverySystem.Dto.User;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
                var calculatedHash = hmacsha.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < calculatedHash.Length; i++)
                {
                    if (calculatedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static string CreateToken(User user, string key)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = signingCredentials
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            return jwtSecurityTokenHandler.WriteToken(securityToken);
        }
    }
}
