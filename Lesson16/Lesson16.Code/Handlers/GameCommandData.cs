using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson16.Code.Handlers
{
    public class GameCommandData : IGameCommandData
    {
        public Guid GameGuid { get; set; }
        public Guid GameObjectGuid { get; set; }
        public string Operation { get; set; }
        public object[] Args { get; set; }
    }
}
