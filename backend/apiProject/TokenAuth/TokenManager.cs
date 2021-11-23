using apiProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;
using System.Text;


namespace apiProject.TokenAuth
{
    public class TokenManager:ITokenManager
    {
        private List<Token> listTokens;
        public TokenManager()
        {
            listTokens = new List<Token>();
        }

        public string GenerateJwtToken(IdentityUser user, ProjectPSConfig psConfig)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(psConfig.JwtSecretToken);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    //when previous token expiry, jti will create new token
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(6),
                //define algorithsm to create token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            string jwtToken = jwtTokenHandler.WriteToken(token);

            Token tokenItem = new Token { Value = jwtToken, ExpiryDate = DateTime.Now.AddHours(6) };

            listTokens.Add(tokenItem);

            return jwtToken;

        }

    }
}
