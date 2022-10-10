using Lesson5.Code.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson7.Code.Commands
{
    public class MoveWithFuelBurnCommand : MacroCommand
    {
        IMoveWithFuelBurnTarget _target;
        public MoveWithFuelBurnCommand(IMoveWithFuelBurnTarget target)
            : base(
                  new CheckFuelCommand(target),
                  new MoveCommand(target),
                  new BurnFuelCommand(target))
        {
        }
    }
}
