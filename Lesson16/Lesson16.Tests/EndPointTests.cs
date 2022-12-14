using Lesson12.Code;
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

            //?reate a game object without any specifics and get its guid
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

            IMessageHandlerResponse response = null;

            Assert.DoesNotThrow(() =>
            {
                response = messageEp.HandleMessage(jsonMessage); //this message will be added and executed in the game loop
            });

            Assert.That(response.Success, response.Error);

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

            //Test bad type

            var jsonMessage = @$"{{
                type:""GAMEOBJECT_COMMAND_TEST"",
                data:{{
                    gameGuid: ""{_newGameGuid}"",
                    gameObjectGuid: ""{_newGameObjectGuid}"",
                    operation: ""CHANGE_VELOCITY"",
                    args: [105, 10]
                }}
            }}";

            IMessageHandlerResponse response = null;

            Assert.DoesNotThrow(() =>
            {
                response = messageEp.HandleMessage(jsonMessage);
            });

            Assert.That(!response.Success, response.Error);

            //Test bad operation

            jsonMessage = @$"{{
                type:""GAMEOBJECT_COMMAND"",
                data:{{
                    gameGuid: ""{_newGameGuid}"",
                    gameObjectGuid: ""{_newGameObjectGuid}"",
                    operation: ""CHANGE_VELOCITY!!!!!!"",
                    args: [105, 10]
                }}
            }}";

            Assert.DoesNotThrow(() =>
            {
                response = messageEp.HandleMessage(jsonMessage);
            });

            Assert.That(!response.Success, response.Error);


            //Test bad gameObjectGuid

            jsonMessage = @$"{{
                type:""GAMEOBJECT_COMMAND"",
                data:{{
                    gameGuid: ""{_newGameGuid}"",
                    gameObjectGuid: ""fsdfsdfsdf"",
                    operation: ""CHANGE_VELOCITY"",
                    args: [105, 10]
                }}
            }}";

            Assert.DoesNotThrow(() =>
            {
                response = messageEp.HandleMessage(jsonMessage);
            });

            Assert.That(!response.Success, response.Error);

            //Test bad gameGuid

            jsonMessage = @$"{{
                type:""GAMEOBJECT_COMMAND"",
                data:{{
                    gameGuid: ""fgdsgfdg"",
                    gameObjectGuid: ""{_newGameObjectGuid}"",
                    operation: ""CHANGE_VELOCITY"",
                    args: [105, 10]
                }}
            }}";

            Assert.DoesNotThrow(() =>
            {
                response = messageEp.HandleMessage(jsonMessage);
            });

            Assert.That(!response.Success, response.Error);

            //Test error JSON
            jsonMessage = @$"dasdasd";

            Assert.DoesNotThrow(() =>
            {
                response = messageEp.HandleMessage(jsonMessage);
            });

            Assert.That(!response.Success, response.Error);
        }

        [Test]
        public void TestNullArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => new MessageEndpoint(null));
            Assert.Throws<ArgumentNullException>(() => new GameAsyncLoop(null));
            Assert.Throws<ArgumentNullException>(() => new Game(null));
            Assert.Throws<ArgumentNullException>(() => new GameCommandHandler(null));
            Assert.Throws<ArgumentNullException>(() => new InterpretCommand(null, _container.Resolve<IUObject>(), new GameCommandData(), _container));
            Assert.Throws<ArgumentNullException>(() => new InterpretCommand(_game, null, new GameCommandData(), _container));
            Assert.Throws<ArgumentNullException>(() => new InterpretCommand(_game, _container.Resolve<IUObject>(), null, _container));
            Assert.Throws<ArgumentNullException>(() => new InterpretCommand(_game, _container.Resolve<IUObject>(), new GameCommandData(), null));

            Assert.Throws<ArgumentNullException>(() => new ChangeVelocityAdaptCommand(null, _container.Resolve<IUObject>(), new object[0], _container));
            Assert.Throws<ArgumentNullException>(() => new ChangeVelocityAdaptCommand(_game, null, new object[0], _container));
            Assert.Throws<ArgumentNullException>(() => new ChangeVelocityAdaptCommand(_game, _container.Resolve<IUObject>(), null, _container));
            Assert.Throws<ArgumentNullException>(() => new ChangeVelocityAdaptCommand(_game, _container.Resolve<IUObject>(), new object[0], null));
        }
    }
}