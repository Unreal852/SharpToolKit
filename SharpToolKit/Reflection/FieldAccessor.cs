using System.Reflection;

namespace SharpToolKit.Reflection;

public readonly struct FieldAccessor<TInstance, TFieldType>(FieldInfo fieldInfo)
{
    public readonly Func<TInstance, TFieldType> Get = DelegateHelper.CreateFieldGetterDelegate<TInstance, TFieldType>(fieldInfo);
    public readonly Action<TInstance, TFieldType> Set = DelegateHelper.CreateFieldSetterDelegate<TInstance, TFieldType>(fieldInfo);
}

public readonly struct FieldAccessor<TFieldType>(object? instance, FieldInfo fieldInfo)
{
    public readonly Func<TFieldType> Get = DelegateHelper.CreateFieldGetterDelegate<TFieldType>(instance, fieldInfo);
    public readonly Action<TFieldType> Set = DelegateHelper.CreateFieldSetterDelegate<TFieldType>(instance, fieldInfo);
    public readonly object? Instance = instance;
}