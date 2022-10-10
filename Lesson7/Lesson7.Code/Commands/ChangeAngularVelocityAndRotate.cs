using Lesson5.Code.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson7.Code.Commands
{
    public class ChangeAngularVelocityAndRotate : MacroCommand
    {
        public ChangeAngularVelocityAndRotate(IRotateWithFuelBurnTarget target)
            : base(
                  new ChangeVelocityCommand(target),
                  new RotateCommand(target))
        {
        }
    }
}
