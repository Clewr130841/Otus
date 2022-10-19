using Lesson9.Code;
using System;

namespace Lesson12.Code
{
    public interface IUObjectAdapterFactory
    {
        T Adapt<T>(IUObject uObject, IContainer container);
        void AdaptStrategyForPropery<T>(IContainer container, string propertyName, Func<IContainer, IUObject, T> get, Action<IContainer, IUObject, T> set);
        void RemoveAdaptedStrategyForProperty<T>(IContainer container, string propertyName, PropertyTypeEnum propTypeEnum);
    }
}