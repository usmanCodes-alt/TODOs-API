using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TODOs_API.Data
{
    public class JsonWebTokenAuthenticationManager : IJsonWebTokenAuthenticationManager
    {
        private readonly string _key;

        public JsonWebTokenAuthenticationManager(string key)
        {
            _key = key;
        }
        public string Authenticate(string username)
        {
            JwtSecurityTokenHandler tokenHander = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_key);
            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHander.CreateToken(securityTokenDescriptor);
            return tokenHander.WriteToken(token);
        }
    }
}
