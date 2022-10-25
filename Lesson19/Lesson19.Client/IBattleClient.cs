using System;

namespace Lesson19.Client
{
    public interface IBattleClient
    {
        bool Login(string username, string password);
    }
}
