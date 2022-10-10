using Lesson5.Code;
using Lesson5.Code.Commands;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Lesson7.Code.Commands
{
    public class CheckFuelCommand : ICommand
    {
        IFuelCheckTarget _target;

        public CheckFuelCommand(IFuelCheckTarget target)
        {
            if(target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            _target = target;
        }

        public void Execute()
        {
            if (_target.FuelLevel < _target.FuelNeedToBurn)
            {
                throw new CommandException("Not enough fuel");
            }
        }
    }
}
