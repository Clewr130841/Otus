using Lesson19.CrossLib;

namespace Lesson19.AuthService
{
    public interface IJwtService
    {
        IJwtData GetToken(IUser userData);
        IJwtData? RefreshToken(string refreshToken);
    }
}