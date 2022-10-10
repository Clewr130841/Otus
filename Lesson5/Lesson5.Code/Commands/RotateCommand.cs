using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson5.Code.Commands
{
    public class RotateCommand : ICommand
    {
        IRotatable _target;

        public RotateCommand(IRotatable target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            _target = target;
        }

        public void Execute()
        {
            var result = (_target.Direction + _target.AngularVelocity) % _target.DirectionsNumber;

            if (result < 0)
            {
                result = _target.DirectionsNumber + result;
            }

            _target.Direction = (uint)result;
        }
    }
}
