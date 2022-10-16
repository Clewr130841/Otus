using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson9.Code
{
    public class SingleInstanceResolver : ResolverBase
    {
        object _instance;
        object _locker;
        Func<IContainer, object[], object> _constr;

        public SingleInstanceResolver(Type type, Func<IContainer, object[], object> constr = null) : base(type)
        {
            _locker = new object();
            _constr = constr;
        }

        public override object Resolve(object[] args, IContainer container)
        {
            if (_instance == null) //Двойная проверка для многопоточности и быстрого резолва, чтобы не лочить каждый раз
            {
                lock (_locker)
                {
                    if (_instance == null)
                    {
                        //Если нам не передали конструктор, пробуем сделать его сами и передаем параметры,
                        //если их передали при первом вызове, так же, если инстанс создан, далее игнорируем все параметры
                        if (_constr == null)
                        {
                            _instance = base.Resolve(args, container);
                        }
                        else
                        {
                            _instance = _constr(container, args);
                        }
                    }
                }
            }

            return _instance;
        }
    }
}
