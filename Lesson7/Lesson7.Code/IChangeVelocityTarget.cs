using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Lesson7.Code
{
    public interface IChangeVelocityTarget
    {
        Vector2 Velocity { set; }
        Vector2 NewVelocity { get; set; }
    }
}