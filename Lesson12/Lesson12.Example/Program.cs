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


adapter.Position = new Vector2(100, 100);
