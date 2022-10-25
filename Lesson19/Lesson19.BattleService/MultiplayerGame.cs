using Lesson12.Code;
using Lesson16.Code;
using Lesson9.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;

namespace Lesson19.BattleService
{
    public class MultiplayerGame : IMultiplayerGame
    {
        IGame _gameInner;
        IGameSettings _settings;
        Dictionary<Guid, string> _gameObjectToUser;

        public MultiplayerGame(IGameSettings settings, IContainer container)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            _settings = settings;
            _gameInner = container.Resolve<IGame>();
        }

        public Guid Guid => _gameInner.Guid;

        public IContainer GameScope => _gameInner.GameScope;

        public bool IsUserOfGame(string userLogin)
        {
            return _settings.Users.Any(x => x.Equals(userLogin, StringComparison.InvariantCultureIgnoreCase));
        }

        public Guid CreateUserObject(string userLogin)
        {
            var guid = CreateObject();
            _gameObjectToUser[guid] = userLogin;
            return guid;
        }

        public bool IsBelongsTo(Guid guid, string userLogin)
        {
            if (_gameObjectToUser.TryGetValue(guid, out var belongsTo))
            {
                return belongsTo.Equals(userLogin, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }

        public Guid CreateObject()
        {
            return _gameInner.CreateObject();
        }

        public IUObject FindObject(Guid guid)
        {
            return _gameInner.FindObject(guid);
        }
    }
}
