// See https://aka.ms/new-console-template for more information
using Lesson14.Code.Loops;
using Lesson9.Code;

var container = new Container();

var loop1Args = new[] { "Some loop 1" };

//Регаем модуль лупа
container.Resolve<IRegistration>().RegisterModule(new AsyncLoopModule());

container.Resolve<IAsyncLoop>(loop1Args).Enqueue((t) =>
{
    var i = 0;
    while (!t.IsCancellationRequested)
    {
        Console.WriteLine("Enter for stop - " + +i++);
        Task.Delay(1000, t).Wait();
    }
});


container.Resolve<IAsyncLoop>(loop1Args).Start();

Console.ReadLine();
container.Resolve<IAsyncLoop>(loop1Args).StopHard();


container.Resolve<IAsyncLoop>(loop1Args).Wait();



Console.WriteLine("End");

Console.ReadLine();