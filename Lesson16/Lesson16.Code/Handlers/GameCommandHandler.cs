using Lesson16.Code.Commands;
using Lesson9.Code;
using System;

namespace Lesson16.Code.Handlers
{


    public class GameCommandHandler : MessageHandlerBase<GameCommandData>
    {
        IContainer _container;

        public GameCommandHandler(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            _container = container;
        }

        public override void HandleMessage(GameCommandData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var game = _container.Resolve<IGame>(data.GameGuid.ToString());
            //Starts command in the game scope, cause it can be, for example paid or free scope
            game.GameScope.Resolve<IInterpretCommand>(game, data).Execute();
        }
    }
}
