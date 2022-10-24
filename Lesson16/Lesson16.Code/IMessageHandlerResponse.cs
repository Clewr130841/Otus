using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson16.Code
{
    public interface IMessageHandlerResponse
    {
        public bool Success { get; }
        public object Data { get; }
        public int Status { get; set; }
        public string Error { get; }
    }
}
