using Lesson12.Code.Compilation;
using Lesson5.Code.Commands;
using Lesson9.Code;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace Lesson12.Code
{

    public class UObjectAdapterFactory : IUObjectAdapterFactory
    {
        IRuntimeCompiler _runtimeCompiler;
        ConcurrentDictionary<Type, ConstructorInfo> _constructors;
        public UObjectAdapterFactory(IRuntimeCompiler runtimeCompiler)
        {
            if (runtimeCompiler == null)
            {
                throw new ArgumentNullException(nameof(runtimeCompiler));
            }

            _runtimeCompiler = runtimeCompiler;
            _constructors = new ConcurrentDictionary<Type, ConstructorInfo>();
        }

        public T Adapt<T>(IUObject uObject, IContainer container)
        {
            if (uObject == null)
            {
                throw new ArgumentNullException(nameof(uObject));
            }

            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            var constr = _constructors.GetOrAdd(typeof(T), (t) =>
            {
                var type = BuildAdapterType(t);
                return type.GetConstructors().First();
            });

            var result = constr.Invoke(new object[] { uObject, container });

            return (T)result;
        }

        private Type BuildAdapterType(Type interfaceType)
        {
            var newTypeName = $"AdapterFrom{typeof(IUObject).Name}To{interfaceType.Name}";
            var sourceCode = GenerateSourceCode(newTypeName, interfaceType, out var usedTypes);
            var asm = _runtimeCompiler.CompileToAssembly(sourceCode, usedTypes);
            var type = asm.DefinedTypes.First();
            return type;
        }


        public void AdaptStrategyForPropery<T>(IContainer container, string propertyName, Func<IContainer, IUObject, T> get, Action<IContainer, IUObject, T> set)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (get == null && set == null)
            {
                throw new AggregateException(new ArgumentNullException(nameof(get)), new ArgumentNullException(nameof(set)));
            }

            if (set != null)
            {
                var key = GetStrategyStoreKeyForPropery(propertyName, PropertyTypeEnum.Set);
                container.Resolve<IRegistration>().Register<T>().WithName(key).As((c, args) =>
                {
                    set(c, (IUObject)args[0], (T)args[1]);
                    return default(T);
                }).Complete();
            }

            if (get != null)
            {
                var key = GetStrategyStoreKeyForPropery(propertyName, PropertyTypeEnum.Get);
                container.Resolve<IRegistration>().Register<T>().WithName(key).As((c, args) =>
                {
                    return get(c, (IUObject)args[0]);
                }).Complete();
            }
        }

        public void RemoveAdaptedStrategyForProperty<T>(IContainer container, string propertyName, PropertyTypeEnum propTypeEnum)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (!propTypeEnum.HasFlag(PropertyTypeEnum.Set) && !propTypeEnum.HasFlag(PropertyTypeEnum.Get))
            {
                throw new ArgumentException(nameof(propTypeEnum));
            }

            if (propTypeEnum.HasFlag(PropertyTypeEnum.Set))
            {
                var key = GetStrategyStoreKeyForPropery(propertyName, PropertyTypeEnum.Set);
                container.Resolve<IRegistration>().Unregister<T>(key);
            }

            if (propTypeEnum.HasFlag(PropertyTypeEnum.Get))
            {
                var key = GetStrategyStoreKeyForPropery(propertyName, PropertyTypeEnum.Get);
                container.Resolve<IRegistration>().Unregister<T>(key);
            }
        }

        private string GetStrategyStoreKeyForPropery(string propertyName, PropertyTypeEnum propTypeEnum)
        {
            return $"{nameof(UObject)}:Strategy:{propertyName}:{propTypeEnum}";
        }

        private string GenerateSourceCode(string newTypeName, Type interfaceType, out Type[] usedTypes)
        {
            var sb = new StringBuilder();

            var uObjectType = typeof(IUObject);
            var containerType = typeof(IContainer);

            var props = interfaceType.GetProperties();

            var usedTypesList = new List<Type>();
            var thisType = GetType();

            usedTypesList.AddRange(props.Select(x => x.PropertyType).Distinct());
            usedTypesList.Add(uObjectType);
            usedTypesList.Add(containerType);
            usedTypesList.Add(interfaceType);

            usedTypes = usedTypesList.Distinct().ToArray();



            foreach (var ns in usedTypes.Select(x => x.Namespace).Distinct())
            {
                sb.AppendLine($"using {ns};");
            }



            sb.AppendLine($"namespace {thisType.Namespace}.Generated {{");
            sb.AppendLine($"public class {newTypeName}: {interfaceType.FullName} {{");
            sb.AppendLine($"\t{containerType.Name} _c;");
            sb.AppendLine($"\t{uObjectType.Name} _o;");
            sb.AppendLine($"\tpublic {newTypeName}({uObjectType.Name} o, {containerType.Name} c){{ _o = o; _c = c; }}");

            foreach (var prop in props)
            {
                sb.AppendLine($"\tpublic {prop.PropertyType.Name} {prop.Name} {{");

                if (prop.CanRead)
                {
                    var key = GetStrategyStoreKeyForPropery(prop.Name, PropertyTypeEnum.Get);
                    var canResolve = $"_c.CanResolve<{prop.PropertyType.Name}>(\"{key}\")";
                    var getProp = $"_o.{nameof(IUObject.GetProperty)}<{prop.PropertyType.Name}>(\"{prop.Name}\")";
                    var resolveProp = $"_c.Resolve<{prop.PropertyType.Name}>(\"{key}\", _o)";

                    sb.AppendLine("\t\tget{");
                    sb.AppendLine($"\t\t\treturn {canResolve}?{resolveProp}:{getProp};");
                    sb.AppendLine("\t\t}");
                }

                if (prop.CanWrite)
                {
                    var key = GetStrategyStoreKeyForPropery(prop.Name, PropertyTypeEnum.Set);
                    var canResolve = $"_c.CanResolve<{prop.PropertyType.Name}>(\"{key}\")";
                    var resolveProp = $"_c.Resolve<{prop.PropertyType.Name}>(\"{key}\", _o, value)";
                    var setProp = $"_o.{nameof(IUObject.SetProperty)}<{prop.PropertyType.Name}>(\"{prop.Name}\", value)";

                    sb.AppendLine($"\t\tset{{");
                    sb.AppendLine($"\t\t\tif({canResolve}){{");
                    sb.AppendLine($"\t\t\t\t{resolveProp};");
                    sb.AppendLine("\t\t\t} else {");
                    sb.AppendLine($"\t\t\t\t{setProp};");
                    sb.AppendLine("\t\t\t}");
                    sb.AppendLine("\t\t}");
                }


                sb.AppendLine("\t}");
            }

            sb.AppendLine("}}");

            return sb.ToString();
        }
    }
}
