using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson9.Code
{
    public class SingleInstanceConstructor : ConstructorBase
    {
        object _instance;
        object _locker;
        IRegistrationContainer _container;
        Func<IContainer, object[], object> _constr;

        public SingleInstanceConstructor(Type type, IRegistrationContainer container, Func<IContainer, object[], object> constr = null) : base(type, container)
        {
            _locker = new object();
            _constr = constr;
            _container = container;
        }

        public override object Construct(object[] args)
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
                            _instance = base.Construct(args);
                        }
                        else
                        {
                            _instance = _constr(_container, args);
                        }
                    }
                }
            }

            return _instance;
        }
    }
}
