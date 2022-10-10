using Lesson5.Code;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson7.Code
{
    public interface IRotateWithFuelBurnTarget : IRotatable, IFuelBurnTarget, IFuelCheckTarget, IChangeVelocityTarget
    {
    }
}
