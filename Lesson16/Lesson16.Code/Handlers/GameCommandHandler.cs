using Lesson12.Code;
using Lesson16.Code.Commands;
using Lesson9.Code;
using System;

namespace Lesson16.Code.Handlers
{


    public class GameCommandHandler : MessageHandlerBase<GameCommandData, bool>
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

        public override bool HandleMessage(GameCommandData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var gameKey = data.GameGuid.ToString();

            if (_container.CanResolve<IGame>(gameKey))
            {
                var game = _container.Resolve<IGame>(gameKey);

                var gameObject = game.FindObject(data.GameObjectGuid);

                if (gameObject != null)
                {
                    //Starts command in the game scope, cause it can be, for example paid or free scope
                    game.GameScope.Resolve<IInterpretCommand>(game, gameObject, data).Execute();
                    return true;
                }
                else
                {
                    throw new ExceptionWithCode(400, $"Can't find the game object with Guid {data.GameObjectGuid}");
                }
            }
            else
            {
                throw new ExceptionWithCode(400, $"Can't find the game with Guid {data.GameGuid}");
            }
        }
    }
}
