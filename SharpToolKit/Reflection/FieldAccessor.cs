using System.Reflection;

namespace SharpToolKit.Reflection;

public struct FieldAccessor<TInstance, TFieldType>
{
    public readonly Func<TInstance, TFieldType>   Get;
    public readonly Action<TInstance, TFieldType> Set;

    public FieldAccessor(FieldInfo fieldInfo)
    {
        Get = DelegateHelper.CreateFieldGetterDelegate<TInstance, TFieldType>(fieldInfo);
        Set = DelegateHelper.CreateFieldSetterDelegate<TInstance, TFieldType>(fieldInfo);
    }
}

public struct FieldAccessor<TFieldType>
{
    public readonly Func<TFieldType>   Get;
    public readonly Action<TFieldType> Set;
    public readonly object?            Instance;

    public FieldAccessor(object? instance, FieldInfo fieldInfo)
    {
        Instance = instance;
        Get = DelegateHelper.CreateFieldGetterDelegate<TFieldType>(instance, fieldInfo);
        Set = DelegateHelper.CreateFieldSetterDelegate<TFieldType>(instance, fieldInfo);
    }
}