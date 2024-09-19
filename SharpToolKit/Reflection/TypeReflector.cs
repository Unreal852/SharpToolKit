using System.Reflection;

namespace SharpToolKit.Reflection;

public static class TypeReflector
{
    private const BindingFlags DefaultLookup
        = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

    public static FieldAccessor<TInstance, TFieldType>? GetFieldAccessor<TInstance, TFieldType>(this Type type, string fieldName,
        BindingFlags bindingFlags = DefaultLookup)
    {
        var fieldInfo = type.GetField(fieldName, bindingFlags);
        return fieldInfo == null
            ? default
            : new FieldAccessor<TInstance, TFieldType>(fieldInfo);
    }

    public static FieldAccessor<TFieldType>? GetFieldAccessor<TFieldType>(
        this Type type, object? instance, string propertyName, BindingFlags bindingFlags = DefaultLookup)
    {
        var fieldInfo = type.GetField(propertyName, bindingFlags);
        return fieldInfo == null
            ? default
            : new FieldAccessor<TFieldType>(instance, fieldInfo);
    }

    public static PropertyAccessor<TInstance, TPropertyType>? GetPropertyAccessor<TInstance, TPropertyType>(
        this Type type, string propertyName, BindingFlags bindingFlags = DefaultLookup)
    {
        var propertyInfo = type.GetProperty(propertyName, bindingFlags);
        return propertyInfo == null
            ? default
            : new PropertyAccessor<TInstance, TPropertyType>(propertyInfo);
    }

    public static PropertyAccessor<TPropertyType>? GetPropertyAccessor<TPropertyType>(
        this Type type, object? instance, string propertyName, BindingFlags bindingFlags = DefaultLookup)
    {
        var propertyInfo = type.GetProperty(propertyName, bindingFlags);
        return propertyInfo == null
            ? default
            : new PropertyAccessor<TPropertyType>(instance, propertyInfo);
    }

    public static TDelegate? GetMethodDelegate<TDelegate>(
        this Type type, object? instance, string methodName, BindingFlags bindingFlags = DefaultLookup)
    {
        var methodInfo = type.GetMethod(methodName, bindingFlags);
        return methodInfo == null ? default : DelegateHelper.CreateDelegate<TDelegate>(instance, methodInfo);
    }
}