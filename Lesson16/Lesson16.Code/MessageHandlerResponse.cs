using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson16.Code
{
    public class MessageHandlerResponse : IMessageHandlerResponse
    {
        public bool Success { get; set; }
        public object Data { get; set; }
        public int Status { get; set; }
        public string Error { get; set; }
    }
}
