using Lesson14.Code.Loops;
using Lesson16.Code.Handlers;
using Lesson5.Code.Commands;
using Lesson9.Code;
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
        IGame _game;
        public InterpretCommand(IGame game, IGameCommandData gameCommandData, IContainer container)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            if (gameCommandData == null)
            {
                throw new ArgumentNullException(nameof(gameCommandData));
            }

            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            _game = game;
            _gameCommandData = gameCommandData;
            _container = container;
        }

        public void Execute()
        {
            var command = _container.Resolve<ICommand>(_gameCommandData.Operation, new object[] { _game, _gameCommandData.GameObjectGuid, _gameCommandData.Args });
            var loop = _container.Resolve<IGameLoop>();
            loop.Enqueue(command);
        }
    }
}
