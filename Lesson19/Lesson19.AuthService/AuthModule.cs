using Lesson16.Code;
using Lesson9.Code;
using Lesson19.AuthService.Users;
using Lesson19.CrossLib.Keys;

namespace Lesson19.AuthService
{
    public class AuthModule : IContainerModule
    {
        public void RegisterModule(IRegistration reg)
        {
            reg.Register<IMessageEndpoint>().As<MessageEndpoint>().AsSingleton().Complete();
            reg.Register<IMessageHandler>().As<LogInHandler>().WithName("LOGIN").AsSingleton().Complete();
            reg.Register<IJwtOptions>().As<JwtOptions>().AsSingleton().Complete();
            reg.Register<IJwtService>().As<JwtService>().AsSingleton().Complete();
            reg.Register<IUserRepository>().As<HardcodeUserRepository>().AsSingleton().Complete();
        }
    }
}
