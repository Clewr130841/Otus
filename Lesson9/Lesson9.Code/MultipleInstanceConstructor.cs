using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Lesson9.Code
{
    public class MultipleInstanceConstructor : ConstructorBase
    {
        Type _type;
        Func<IContainer, object[], object> _constr;
        IRegistrationContainer _container;

        public MultipleInstanceConstructor(Type type, IRegistrationContainer container, Func<IContainer, object[], object> constr = null) : base(type, container)
        {
            _type = type;
            _constr = constr;
            _container = container;
        }

        public override object Construct(object[] args)
        {
            if (_constr != null)
            {
                return _constr.Invoke(_container, args);
            }
            return base.Construct(args);
        }
    }
}
