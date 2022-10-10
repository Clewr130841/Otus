using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Lesson7.Code
{
    public interface IFuelBurnTarget
    {
        float FuelLevel { get; set; }
        float FuelNeedToBurn { get; }
    }
}
