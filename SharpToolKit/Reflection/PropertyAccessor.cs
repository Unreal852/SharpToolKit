using System.Reflection;

namespace SharpToolKit.Reflection;

public readonly struct PropertyAccessor<TInstance, TPropertyType>
{
    public readonly Func<TInstance, TPropertyType>   Get;
    public readonly Action<TInstance, TPropertyType> Set;

    public PropertyAccessor(PropertyInfo propertyInfo)
    {
        Get = DelegateHelper.CreatePropertyGetterDelegate<TInstance, TPropertyType>(propertyInfo);
        Set = DelegateHelper.CreatePropertySetterDelegate<TInstance, TPropertyType>(propertyInfo);
    }
}

public readonly struct PropertyAccessor<TPropertyType>
{
    public readonly Func<TPropertyType>   Get;
    public readonly Action<TPropertyType> Set;
    public readonly object?               Instance;

    public PropertyAccessor(object? instance, PropertyInfo propertyInfo)
    {
        Instance = instance;
        Get = DelegateHelper.CreatePropertyGetterDelegate<TPropertyType>(instance, propertyInfo);
        Set = DelegateHelper.CreatePropertySetterDelegate<TPropertyType>(instance, propertyInfo);
    }
}