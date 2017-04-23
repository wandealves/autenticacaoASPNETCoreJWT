using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace AuthAPI.Domain.Auth
{
    public class JwtProvider
    {
        private JwtSecurityTokenHandler _tokenHandler;
        private JwtSettings _settings;

        public JwtProvider(JwtSettings settings, JwtSecurityTokenHandler tokenHandler)
        {
            _settings = settings;
            _tokenHandler = tokenHandler;
        }

        public string CreateEncoded(string json)
        {
            return _tokenHandler.CreateEncodedJwt(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new GenericIdentity(json)),
                Expires = DateTime.UtcNow.AddDays(_settings.TokenExpiration),
                SigningCredentials = new SigningCredentials(_settings.SecurityKey, SecurityAlgorithms.HmacSha256Signature),
                Audience = _settings.Audience,
                Issuer = _settings.Issuer
            });

        }
    }
}
