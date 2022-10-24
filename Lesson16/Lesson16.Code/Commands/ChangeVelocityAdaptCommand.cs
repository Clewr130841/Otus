using Lesson12.Code;
using Lesson16.Code.Handlers;
using Lesson5.Code.Commands;
using Lesson7.Code;
using Lesson7.Code.Commands;
using Lesson9.Code;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Xml.Linq;

namespace Lesson16.Code.Commands
{
    public class ChangeVelocityAdaptCommand : ICommand
    {
        IGame _game;
        Guid _gameObjectGuid;
        object[] _args;
        IContainer _container;
        public ChangeVelocityAdaptCommand(IGame game, Guid gameObjectGuid, object[] args, IContainer container)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            _game = game;
            _gameObjectGuid = gameObjectGuid;
            _args = args;
            _container = container;
        }

        public void Execute()
        {
            var gameObject = _game.FindObject(_gameObjectGuid);
            var adapterFactory = _container.Resolve<IUObjectAdapterFactory>();
            var adapter = adapterFactory.Adapt<IChangeVelocityTarget>(gameObject, _container);
            adapter.NewVelocity = new Vector2(Convert.ToSingle(_args[0]), Convert.ToSingle(_args[1]));
            _container.Resolve<ICommand>("CHANGE_VELOCITY_COMMAND", new object[] { adapter }).Execute();
        }
    }
}
