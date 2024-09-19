using System.Reflection;

namespace SharpToolKit.Reflection;

public readonly struct PropertyAccessor<TInstance, TPropertyType>(PropertyInfo propertyInfo)
{
    public readonly Func<TInstance, TPropertyType> Get = DelegateHelper.CreatePropertyGetterDelegate<TInstance, TPropertyType>(propertyInfo);
    public readonly Action<TInstance, TPropertyType> Set = DelegateHelper.CreatePropertySetterDelegate<TInstance, TPropertyType>(propertyInfo);
}

public readonly struct PropertyAccessor<TPropertyType>(object? instance, PropertyInfo propertyInfo)
{
    public readonly Func<TPropertyType> Get = DelegateHelper.CreatePropertyGetterDelegate<TPropertyType>(instance, propertyInfo);
    public readonly Action<TPropertyType> Set = DelegateHelper.CreatePropertySetterDelegate<TPropertyType>(instance, propertyInfo);
    public readonly object? Instance = instance;
}