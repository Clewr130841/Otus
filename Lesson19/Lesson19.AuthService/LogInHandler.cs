using Lesson16.Code;
using Lesson16.Code.Handlers;
using Lesson19.AuthService.Users;
using Lesson19.CrossLib;
using System;
using System.Collections.Generic;

namespace Lesson19.AuthService
{
    public class LogInHandler : MessageHandlerBase<LogInData, IJwtData>
    {
        IUserRepository _userRepository;
        IJwtService _jwtService;

        public LogInHandler(IUserRepository userRepository, IJwtService jwtService)
        {
            if (userRepository == null)
            {
                throw new ArgumentNullException(nameof(userRepository));
            }

            if (jwtService == null)
            {
                throw new ArgumentNullException(nameof(jwtService));
            }

            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public override IJwtData HandleMessage(LogInData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var user = _userRepository.GetUserByLoginAndPassword(data.Login, data.Password);

            if (user == null)
            {
                throw new ExceptionWithCode(400, "Bad login and password");
            }

            var token = _jwtService.GetToken(user);

            return token;
        }
    }
}
