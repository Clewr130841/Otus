// See https://aka.ms/new-console-template for more information
using Lesson12.Code;
using Lesson12.Code.Compilation;
using Lesson5.Code;
using Lesson9.Code;
using System.Numerics;

Console.WriteLine("Hello, World!");

var compiller = new RuntimeCompiler();
var container = new Container();
var buildFactory = new UObjectAdapterFactory(compiller);

var uObject = new UObject();

var adapter = buildFactory.Adapt<IMovable>(uObject, container);

buildFactory.AdaptStrategyForPropery(
    container,
    nameof(IMovable.Position),
    (c, u) => u.GetProperty<Vector2>(nameof(IMovable.Position)),
    (c, u, v) => u.SetProperty(nameof(IMovable.Position), v + Vector2.One)
);

adapter.Position = new System.Numerics.Vector2(100, 100);

var pos1 = adapter.Position;

buildFactory.RemoveAdaptedStrategyForProperty(container, nameof(IMovable.Position), PropertyTypeEnum.Get | PropertyTypeEnum.Set);

adapter.Position = new System.Numerics.Vector2(100, 100);

var pos2 = adapter.Position;