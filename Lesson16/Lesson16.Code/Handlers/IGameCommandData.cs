using System;

namespace Lesson16.Code.Handlers
{
    public interface IGameCommandData
    {
        public Guid GameGuid { get; }
        public Guid GameObjectGuid { get; }
        public string Operation { get; }
        public object[] Args { get; }
    }
}