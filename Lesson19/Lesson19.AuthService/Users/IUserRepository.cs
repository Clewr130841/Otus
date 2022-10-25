using Lesson19.CrossLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson19.AuthService.Users
{
    public interface IUserRepository
    {
        public bool UserExists(string login, string password);
        public IUser? GetUserByLoginAndPassword(string login, string password);
    }
}
