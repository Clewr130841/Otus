using Lesson5.Code;
using Lesson5.Code.Commands;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Lesson7.Code.Commands
{
    public class ChangeVelocityCommand : ICommand
    {
        IChangeVelocityTarget _target;
        public ChangeVelocityCommand(IChangeVelocityTarget target)
        {
            if(target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            _target = target;
        }

        public void Execute()
        {
            _target.Velocity = _target.NewVelocity;
        }
    }
}
