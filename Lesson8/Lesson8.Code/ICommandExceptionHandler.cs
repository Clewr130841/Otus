using Lesson5.Code.Commands;
using System;

namespace Lesson8.Code
{
    public interface ICommandExceptionHandler
    {
        public void Handle(Exception ex, ICommand command);
    }
}
