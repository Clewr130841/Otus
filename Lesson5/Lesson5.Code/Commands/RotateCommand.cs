using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson5.Code.Commands
{
    public class RotateCommand : ICommand
    {
        IRotatable _rotatable;
        IGameProperties _gameProperties;

        public RotateCommand(IRotatable rotatable, IGameProperties gameProperties)
        {
            _rotatable = rotatable;
            _gameProperties = gameProperties;
        }

        public void Execute()
        {
            var result = (_rotatable.Direction + _rotatable.AngularVelocity) % _gameProperties.DirectionsNumber;

            if (result < 0)
            {
                result = _gameProperties.DirectionsNumber + result;
            }

            _rotatable.Direction = (uint)result;
        }
    }
}
