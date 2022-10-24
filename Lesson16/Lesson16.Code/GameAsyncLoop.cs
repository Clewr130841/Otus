using Lesson14.Code.Loops;
using Lesson9.Code;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson16.Code
{
    public class GameAsyncLoop : AsyncLoop, IGameLoop
    {
        const string GAME_ASYNC_LOOP = "GAME_ASYNC_LOOP";

        public GameAsyncLoop(IContainer container) : base(GAME_ASYNC_LOOP, container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }
        }
    }
}
