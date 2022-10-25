using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson19.CrossLib
{
    public interface IUser
    {
        public int Id { get; }
        public string Login { get; }
    }
}
