using Lesson5.Code;
using Lesson5.Code.Commands;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Lesson7.Code.Commands
{
    public class BurnFuelCommand : ICommand
    {
        IFuelBurnTarget _target;


        public BurnFuelCommand(IFuelBurnTarget target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            _target = target;
        }

        public void Execute()
        {
            //Наверное не проверяем уровень горючего т.к. есть специальная комманда, так что если в минус, так в минус, но можно дописать
            _target.FuelLevel -= _target.FuelNeedToBurn;
        }
    }
}
