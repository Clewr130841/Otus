using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Lesson9.Code.Resolvers
{
    public abstract class ResolverBase
    {
        Type _type;
        public ResolverBase(Type type)
        {
            _type = type;
        }

        public virtual object Resolve(object[] args, IContainer container)
        {
            if (args == null || args.Length == 0) // Если юзверь не передал аргументов для конструктора, ищем их сами
            {
                var constructors = _type.GetConstructors().Where(x => x.IsPublic);

                //Если есть конструктор без параметров ищем его и инвокаем
                var defaultConstructor = constructors.Where(x => x.GetParameters().Length == 0).FirstOrDefault();

                if (defaultConstructor != null)
                {
                    return defaultConstructor.Invoke(new object[0]);
                }

                //Если такого нет, тогда ищем первый подходящий
                foreach (var constr in constructors)
                {
                    var constrParams = constr.GetParameters();
                    var paramList = new List<object>();

                    foreach (var param in constrParams)
                    {
                        //Если контейнер может зарезолвить тип
                        if (container.CanResolve(param.ParameterType))
                        {
                            paramList.Add(container.Resolve(param.ParameterType));
                        }
                        else if (param.HasDefaultValue) //Если есть дефолтное значение
                        {
                            paramList.Add(param.DefaultValue);
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (paramList.Count == constrParams.Length) //Если нашли нужный конструктор и создали все параметры
                    {
                        return constr.Invoke(paramList.ToArray());
                    }
                }
            }

            //Пробуем создать инстанс, если все выше не прошло или передали аргументы
            //Можно конечно было все решить кодом выше, но уже слишком много для ДЗ
            return Activator.CreateInstance(_type, args);
        }
    }
}