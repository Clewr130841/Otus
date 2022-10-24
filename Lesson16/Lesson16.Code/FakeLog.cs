using Lesson8.Code;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson16.Code
{
    public class FakeLog : ILog
    {
        public void Log(Exception ex)
        {
            //Заглушка
        }
    }
}
