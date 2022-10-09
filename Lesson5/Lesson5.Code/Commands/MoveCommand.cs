using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson5.Code.Commands
{
    public class MoveCommand : ICommand
    {
        IMovable _movable;
        public MoveCommand(IMovable movable)
        {
            _movable = movable;
        }

        public void Execute()
        {
            _movable.Position += _movable.Velocity;
        }
    }
}
