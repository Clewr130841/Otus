using Lesson19.CrossLib;
using Lesson19.CrossLib.Keys;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Lesson19.AuthService
{
    public class JwtService : IJwtService
    {
        private class JwtToken : IJwtData
        {
            public string Token { get; set; }

            public string RefreshToken { get; set; }
        }

        const int TOKEN_LIFE_TIME_MINS = 10;

        SigningCredentials _signingCredentials;
        Dictionary<string, IUser> _refreshData;
        IJwtOptions _options;
        public JwtService(IJwtOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var key = new SymmetricSecurityKey(options.Key);
            _signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            _refreshData = new Dictionary<string, IUser>();
            _options = options;
        }

        public IJwtData GetToken(IUser userData)
        {
            if (userData == null)
            {
                throw new ArgumentNullException(nameof(userData));
            }

            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.Sid, userData.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, userData.Login),
            };

            var now = DateTime.Now;
            var jwt = new JwtSecurityToken(
                _options.Issuer,
                _options.Audience,
                claims,
                now,
                now.Add(TimeSpan.FromMinutes(TOKEN_LIFE_TIME_MINS)),
                _signingCredentials);

            var refreshToken = Guid.NewGuid().ToString();

            _refreshData[refreshToken] = userData;

            return new JwtToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                RefreshToken = refreshToken,
            };
        }

        public IJwtData? RefreshToken(string refreshToken)
        {
            if (refreshToken == null)
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }

            if (_refreshData.TryGetValue(refreshToken, out IUser result))
            {
                return GetToken(result);
            }

            return null;
        }
    }
}
