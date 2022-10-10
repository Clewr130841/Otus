using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson5.Code.Commands
{
    public class MoveCommand : ICommand
    {
        IMovable _target;
        public MoveCommand(IMovable target)
        {
            if(target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            _target = target;
        }

        public void Execute()
        {
            _target.Position += _target.Velocity;
        }
    }
}
