using Lesson19.CrossLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson19.AuthService.Users
{
    public class HardcodeUserRepository : IUserRepository
    {
        private class User : IUser
        {
            public User(int id, string login)
            {
                Id = id;
                Login = login;
            }

            public int Id { get; private set; }

            public string Login { get; private set; }
        }

        //Some hardcoded users;
        Dictionary<string, IUser> _users = new Dictionary<string, IUser>()
        {
            { "maxim:fsdojwewd123!", new User(1, "Maxim") },
            { "vlad:ds!@3g5g!", new User(1, "Vlad") },
            { "strashnyhren:fsdfF24v@", new User(1, "StrahnyHren") },
            { "gameover:fsdf12dasdF24v@", new User(1, "GameOver") },
        };

        public IUser? GetUserByLoginAndPassword(string login, string password)
        {
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            var key = login.ToLower().Trim() + ":" + password;

            if (_users.TryGetValue(key, out IUser user))
            {
                return user;
            }

            return null;
        }
    }
}
