using Lesson12.Code;
using Lesson16.Code.Handlers;
using Lesson5.Code.Commands;
using Lesson7.Code;
using Lesson7.Code.Commands;
using Lesson9.Code;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Xml.Linq;

namespace Lesson16.Code.Commands
{
    public class ChangeVelocityAdaptCommand : AdaptCommandBase
    {
        public ChangeVelocityAdaptCommand(IGame game, IUObject uObject, object[] args, IContainer container) : base(game, uObject, args, container)
        {
        }

        public override void Execute()
        {
            var adapterFactory = _container.Resolve<IUObjectAdapterFactory>();
            var adapter = adapterFactory.Adapt<IChangeVelocityTarget>(_uObject, _container);
            adapter.NewVelocity = new Vector2(Convert.ToSingle(_args[0]), Convert.ToSingle(_args[1]));
            _container.Resolve<ICommand>("CHANGE_VELOCITY_COMMAND", new object[] { adapter }).Execute();
        }
    }
}
