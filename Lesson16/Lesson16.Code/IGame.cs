using Lesson12.Code;
using Lesson9.Code;
using System;

namespace Lesson16.Code
{
    public interface IGame
    {
        Guid Guid { get; }
        IContainer GameScope { get; }
        IUObject FindObject(Guid guid);
        Guid CreateObject();
    }
}