using Lesson12.Code;
using Lesson5.Code.Commands;
using Lesson9.Code;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson16.Code.Commands
{
    public abstract class AdaptCommandBase : ICommand
    {
        protected IGame _game;
        protected IUObject _uObject;
        protected object[] _args;
        protected IContainer _container;

        public AdaptCommandBase(IGame game, IUObject uObject, object[] args, IContainer container)
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

            if (uObject == null)
            {
                throw new ArgumentNullException(nameof(uObject));
            }

            _game = game;
            _uObject = uObject;
            _container = container;
            _args = args;
        }

        public abstract void Execute();
    }
}
