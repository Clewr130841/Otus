using Lesson5.Code.Commands;
using Lesson9.Code;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Lesson14.Code
{
    public class ActionCommand : ICommand
    {
        Action _action;

        public ActionCommand(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            _action = action;
        }

        public ActionCommand(Action<CancellationToken> action, CancellationTokenSource cancellationTokenSource)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            _action = () => action(cancellationTokenSource.Token);
        }

        public void Execute()
        {
            _action();
        }
    }
}
