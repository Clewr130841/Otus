using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Lesson9.Code
{
    public class MultipleInstanceResolver : ResolverBase
    {
        Type _type;
        Func<IContainer, object[], object> _constr;

        public MultipleInstanceResolver(Type type, Func<IContainer, object[], object> constr = null) : base(type)
        {
            _type = type;
            _constr = constr;
        }

        public override object Resolve(object[] args, IContainer container)
        {
            if (_constr != null)
            {
                return _constr.Invoke(container, args);
            }
            return base.Resolve(args, container);
        }
    }
}
