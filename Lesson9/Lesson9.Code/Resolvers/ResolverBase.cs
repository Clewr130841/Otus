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
            var constructors = _type.GetConstructors().Where(x => x.IsPublic && x.GetParameters().Length >= args.Length).ToArray();

            if (constructors.Length == 0)
            {
                return Activator.CreateInstance(_type);
            }

            if (args.Length == 0)
            {
                //Если есть конструктор без параметров ищем его и инвокаем
                var defaultConstructor = constructors.Where(x => x.GetParameters().Length == 0).FirstOrDefault();

                if (defaultConstructor != null)
                {
                    return defaultConstructor.Invoke(new object[0]);
                }
            }

            //Если такого нет, тогда ищем первый подходящий
            foreach (var constr in constructors)
            {
                var constrParams = constr.GetParameters();
                var paramList = new List<object>();

                var argsCopy = new object[args.Length];
                Array.Copy(args, argsCopy, args.Length);

                foreach (var param in constrParams)
                {
                    var foundArgIndex = Array.FindIndex(argsCopy, (o) =>
                    {
                        var paramType = o?.GetType();

                        if (paramType != null)
                        {
                            return param.ParameterType == paramType || param.ParameterType.IsAssignableFrom(paramType);
                        }

                        return false;
                    });

                    if (foundArgIndex != -1) //Если юзер передал значение
                    {
                        paramList.Add(argsCopy[foundArgIndex]);
                        argsCopy[foundArgIndex] = null;
                    }
                    else if (container.CanResolve(param.ParameterType)) //Если контейнер может зарезолвить тип
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

            throw new Exception("Can't resolve type");
        }
    }
}