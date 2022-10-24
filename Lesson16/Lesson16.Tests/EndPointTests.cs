using Lesson16.Code;
using Lesson16.Code.Commands;
using Lesson16.Code.Handlers;
using Lesson9.Code;
using Newtonsoft.Json;
using System.Numerics;
using static System.Net.Mime.MediaTypeNames;

namespace Lesson16.Tests
{
    public class Tests
    {
        IContainer _container;
        Guid _newGameGuid;
        Guid _newGameObjectGuid;
        IGame _game;

        [SetUp]
        public void Setup()
        {
            _container = new Container();

            Assert.DoesNotThrow(() =>
            {
                //Test GameModule registration
                _container.Resolve<IRegistration>().RegisterModule(new GameModule());
            });

            //Starting common game loop
            _container.Resolve<IGameLoop>().Start();

            //Create and register a new game
            _game = _container.Resolve<IGame>();
            _container.Resolve<IRegistration>().Register<IGame>().AsSingleton(() => _game).WithName(_game.Guid.ToString()).Complete();
            _newGameGuid = _game.Guid;

            //Ñreate a game object without any specifics and get its guid
            _newGameObjectGuid = _game.CreateObject();
        }

        [Test]
        public void TestMessage()
        {
            var messageEp = _container.Resolve<IMessageEndpoint>();

            //this is json of command
            //field "type" is a type of a handler wich can process "data" section
            //the content of the "data" field depends on the handler
            //this message will be expanded further, for authorization and authentication
            var jsonMessage = @$"{{
                type:""GAMEOBJECT_COMMAND"",
                data:{{
                    gameGuid: ""{_newGameGuid}"",
                    gameObjectGuid: ""{_newGameObjectGuid}"",
                    operation: ""CHANGE_VELOCITY"",
                    args: [105, 10]
                }}
            }}";

            Assert.DoesNotThrow(() =>
            {
                messageEp.HandleMessage(jsonMessage); //this message will be added and executed in the game loop
            });

            _container.Resolve<IGameLoop>().Stop(); //soft stop the game loop
            _container.Resolve<IGameLoop>().Wait(); //wait for all commands to complete

            var uObject = _container.Resolve<IGame>(_newGameGuid.ToString()).FindObject(_newGameObjectGuid);
            var resultVelocity = uObject.GetProperty<Vector2>("Velocity");

            Assert.That(resultVelocity.X == 105 && resultVelocity.Y == 10); //Check that our message was executed
        }

        [Test]
        public void TestBadMessage()
        {
            var messageEp = _container.Resolve<IMessageEndpoint>();

            var json = "fsdfsdf";
            Assert.Throws<JsonReaderException>(() =>
            {
                messageEp.HandleMessage(json);
            });
        }

        [Test]
        public void TestCreation()
        {
            Assert.Throws<ArgumentNullException>(() => new MessageEndpoint(null));
            Assert.Throws<ArgumentNullException>(() => new GameAsyncLoop(null));
            Assert.Throws<ArgumentNullException>(() => new Game(null));
            Assert.Throws<ArgumentNullException>(() => new GameCommandHandler(null));
            Assert.Throws<ArgumentNullException>(() => new InterpretCommand(null, new GameCommandData(), _container));
            Assert.Throws<ArgumentNullException>(() => new InterpretCommand(_game, null, _container));
            Assert.Throws<ArgumentNullException>(() => new InterpretCommand(_game, new GameCommandData(), null));

            Assert.Throws<ArgumentNullException>(() => new ChangeVelocityAdaptCommand(null, Guid.Empty, new object[0], _container));
            Assert.Throws<ArgumentNullException>(() => new ChangeVelocityAdaptCommand(_game, Guid.Empty, null, _container));
            Assert.Throws<ArgumentNullException>(() => new ChangeVelocityAdaptCommand(_game, Guid.Empty, new object[0], null));
        }
    }
}