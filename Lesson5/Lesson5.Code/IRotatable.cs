using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson5.Code
{
    public interface IRotatable
    {
        uint Direction { get; set; }
        int AngularVelocity { get; }
        int DirectionsNumber { get; }
    }
}
