using Lesson16.Code;
using Lesson9.Code;
using System;

namespace Lesson19.BattleService
{
    public class NewGameHandler : MessageHandlerBase<GameSettings, Guid>
    {
        IContainer _container;

        public NewGameHandler(IContainer container)
        {
            _container = container;
        }

        public override Guid HandleMessage(GameSettings settings)
        {
            var game = _container.Resolve<IMultiplayerGame>(settings);

            _container.Resolve<IRegistration>()
                .Register<IMultiplayerGame>()
                .WithName(game.Guid.ToString())
                .AsSingleton(x => game)
                .Complete();

            return game.Guid;
        }
    }
}
