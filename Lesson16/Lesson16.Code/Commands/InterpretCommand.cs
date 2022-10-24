using Lesson12.Code;
using Lesson14.Code.Loops;
using Lesson16.Code.Handlers;
using Lesson5.Code.Commands;
using Lesson9.Code;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Lesson16.Code.Commands
{
    public class InterpretCommand : IInterpretCommand
    {
        IGameCommandData _gameCommandData;
        IContainer _container;
        IUObject _uObject;
        public InterpretCommand(IGame game, IUObject uObject, IGameCommandData gameCommandData, IContainer container)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            if (uObject == null)
            {
                throw new ArgumentNullException(nameof(uObject));
            }

            if (gameCommandData == null)
            {
                throw new ArgumentNullException(nameof(gameCommandData));
            }

            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            _uObject = uObject;
            _gameCommandData = gameCommandData;
            _container = container;
        }

        public void Execute()
        {
            if (_container.CanResolve<ICommand>(_gameCommandData.Operation))
            {
                var command = _container.Resolve<ICommand>(_gameCommandData.Operation, new object[] { _uObject, _gameCommandData.GameObjectGuid, _gameCommandData.Args });
                var loop = _container.Resolve<IGameLoop>();
                loop.Enqueue(command);
            }
            else
            {
                throw new ExceptionWithCode(400, $"Can't find operation {_gameCommandData.Operation}");
            }
        }
    }
}
