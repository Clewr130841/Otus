using Lesson9.Code;

IContainer container = new Container();

//Так регаем зависимости
//Синглтон
container.Resolve<IRegistration>().Register<int>().AsSingleton(_ => 123).Complete(); //Вызов Complete - обязателен, иначе не зарегается
container.Resolve<IRegistration>().Register<IList<int>>().AsSingleton(_ => new List<int>()).Complete();

//Если не нужен синглтон, то просто не вызываем метод AsSingleton
container.Resolve<IRegistration>().Register<List<int>>().WithName("TestList").Complete();


//Все дочерние скоупы наследуют регистрации родительского, при этом если это был синглтон, он пересоздается для дочернего скоупа

//Так получаем лайфтайм скоуп
using (var lifetimeScope = container.Resolve<ILifetimeScope>())
{
    //Все изменения произошедшие с lifetimeScope не коснутся родительского
}

//Так получаем тред скоуп
var threadScope = container.Resolve<IThreadScope>(); //Скоуп создается автоматом для потока

//Так получаем именованный скоуп, при чем неважно из какого скуопа мы получаем его, он всегда будет ссылкой на один и тот же экземпляр
//который зареган по имени в любом из скоупов
var namedScope = container.Resolve<INamedScope>().WithName("Test"); //Скоуп создается автоматом для имени


//Контейнер умеет подставлять зарегистрированные типы аргументов в конструктор, если при резолве не было передано других
container.Resolve<IRegistration>().Register<TestClass>().Complete();
var testClass = container.Resolve<TestClass>(); //IList<int> сам подставится в конструктор


public class TestClass
{
    public IList<string> List { get; set; }
    public TestClass(IList<int> list) => List = List;
}
