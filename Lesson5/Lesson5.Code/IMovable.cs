using System;
using System.Numerics;

namespace Lesson5.Code
{
    public interface IMovable
    {
        Vector2 Position { get; set; }
        Vector2 Velocity { get; }
    }
}
