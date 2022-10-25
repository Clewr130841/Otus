// See https://aka.ms/new-console-template for more information
using Lesson19.AuthService;
using Lesson19.BattleService;
using Lesson9.Code;

Console.WriteLine("Hello, World!");

//Start auth service
var authServiceContainer = new Container();
authServiceContainer.Resolve<IRegistration>().RegisterModule(new AuthModule());


//Start battle service
var battleServiceContainer = new Container();
battleServiceContainer.Resolve<IRegistration>().RegisterModule(new BattleModule());