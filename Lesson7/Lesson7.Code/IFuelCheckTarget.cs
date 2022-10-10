using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Lesson7.Code
{
    public interface IFuelCheckTarget
    {
        float FuelLevel { get; }
        float FuelNeedToBurn { get; }
    }
}
