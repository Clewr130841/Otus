using Lesson16.Code;
using Lesson16.Code.Handlers;
using Lesson19.AuthService.Users;
using Lesson19.CrossLib;
using System;
using System.Collections.Generic;

namespace Lesson19.AuthService
{
    public class RefreshTokenHandler : MessageHandlerBase<string, IJwtData>
    {
        IJwtService _jwtService;

        public RefreshTokenHandler(IJwtService jwtService)
        {
            if (jwtService == null)
            {
                throw new ArgumentNullException(nameof(jwtService));
            }

            _jwtService = jwtService;
        }

        public override IJwtData HandleMessage(string refreshToken)
        {
            if (refreshToken == null)
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }

            var token = _jwtService.RefreshToken(refreshToken);

            if (token == null)
            {
                throw new ExceptionWithCode(400, "Refresh token is not valid");
            }

            return token;
        }
    }
}
