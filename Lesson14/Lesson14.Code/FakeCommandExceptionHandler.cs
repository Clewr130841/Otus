using Lesson5.Code.Commands;
using Lesson8.Code;
using Lesson8.Code.Commands;
using Lesson8.Code.ExceptionHandlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson14.Code
{
    public class FakeCommandExceptionHandler : ICommandExceptionHandler
    {
        public void Handle(Exception ex, ICommand command)
        {
            //Просто заглушка
        }
    }
}
