using System.Linq.Expressions;
using System.Reflection;

// ReSharper disable MemberCanBePrivate.Global

namespace SharpToolKit.Reflection;

public static class DelegateHelper
{
    /// <summary>
    ///   The name of the Invoke method of a Delegate.
    /// </summary>
    private const string InvokeMethod = "Invoke";

    /// <summary>
    ///   Get method info for a specified delegate type.
    /// </summary>
    /// <param name = "delegateType">The delegate type to get info for.</param>
    /// <returns>The method info for the given delegate type.</returns>
    public static MethodInfo MethodInfoFromDelegateType(Type delegateType)
    {
        if (!delegateType.IsSubclassOf(typeof(MulticastDelegate)))
            throw new ArgumentException("Given type should be a delegate.");

        return delegateType.GetMethod(InvokeMethod) ??
               throw new NullReferenceException("Failed to find the invoke method on the delegate");
    }

    /// <summary>
    ///   Creates a delegate of a specified type that represents the specified
    ///   static or instance method, with the specified first argument.
    ///   Conversions are done when possible.
    /// </summary>
    /// <typeparam name = "T">The type for the delegate.</typeparam>
    /// <param name = "instance">
    ///   The object to which the delegate is bound,
    ///   or null to treat method as static
    /// </param>
    /// <param name = "method">
    ///   The MethodInfo describing the static or
    ///   instance method the delegate is to represent.
    /// </param>
    public static T CreateDelegate<T>(object? instance, MethodInfo method)
    {
        var delegateInfo = MethodInfoFromDelegateType(typeof(T));
        var methodParameters = method.GetParameters();
        var delegateParameters = delegateInfo.GetParameters();

        // Convert the arguments from the delegate argument type to the method argument type when necessary.
        var arguments = new ParameterExpression[delegateParameters.Length];

        for (var i = 0; i < arguments.Length; i++)
            arguments[i] = Expression.Parameter(delegateParameters[i].ParameterType);

        var convertedArguments = new Expression[methodParameters.Length];

        for (var i = 0; i < methodParameters.Length; i++)
        {
            var methodType = methodParameters[i].ParameterType;
            var delegateType = delegateParameters[i].ParameterType;
            if (methodType != delegateType)
                convertedArguments[i] = Expression.Convert(arguments[i], methodType);
            else
                convertedArguments[i] = arguments[i];
        }

        // Create method call.
        var instanceObj = instance == null ? null : Expression.Constant(instance);
        var methodCall = Expression.Call(instanceObj, method, convertedArguments);

        // Convert return type when necessary.
        var convertedMethodCall = delegateInfo.ReturnType == method.ReturnType
            ? (Expression)methodCall
            : Expression.Convert(methodCall, delegateInfo.ReturnType);

        return Expression.Lambda<T>(convertedMethodCall, arguments).Compile();
    }

    public static Func<TFieldType> CreateFieldGetterDelegate<TFieldType>(object? instance, FieldInfo fieldInfo)
    {
        // TODO: Check provided generic type against the provided fieldInfo type and try to convert if they not match
        var instanceObject = instance == null ? null : Expression.Constant(instance);
        var field = Expression.Field(instanceObject, fieldInfo);
        return Expression.Lambda<Func<TFieldType>>(field).Compile();
    }

    public static Func<TInstance, TFieldType> CreateFieldGetterDelegate<TInstance, TFieldType>(FieldInfo fieldInfo)
    {
        // TODO: Check provided generic type against the provided fieldInfo type and try to convert if they don't match
        var instanceParameter = Expression.Parameter(typeof(TInstance), "instance");
        var field = Expression.Field(instanceParameter, fieldInfo);
        return Expression.Lambda<Func<TInstance, TFieldType>>(field, instanceParameter).Compile();
    }

    public static Action<TFieldType> CreateFieldSetterDelegate<TFieldType>(object? instance, FieldInfo fieldInfo)
    {
        // TODO: Check provided generic type against the provided fieldInfo type and try to convert if they don't match
        var genericType = typeof(TFieldType);
        var instanceObject = instance == null ? null : Expression.Constant(instance);
        var field = Expression.Field(instanceObject, fieldInfo);
        var fieldValueParam = Expression.Parameter(genericType, fieldInfo.Name);
        var assignExpression = Expression.Assign(field, fieldValueParam);
        return Expression.Lambda<Action<TFieldType>>(assignExpression, fieldValueParam).Compile();
    }

    public static Action<TInstance, TFieldType> CreateFieldSetterDelegate<TInstance, TFieldType>(FieldInfo fieldInfo)
    {
        // TODO: Check provided generic type against the provided fieldInfo type and try to convert if they don't match
        var instanceType = typeof(TInstance);
        var fieldType = typeof(TFieldType);
        var instanceParameter = Expression.Parameter(instanceType);
        var field = Expression.Field(instanceParameter, fieldInfo);
        var fieldValueParam = Expression.Parameter(fieldType, fieldInfo.Name);
        var assignExpression = Expression.Assign(field, fieldValueParam);
        return Expression.Lambda<Action<TInstance, TFieldType>>(assignExpression, instanceParameter, fieldValueParam)
            .Compile();
    }

    public static Func<TPropertyType> CreatePropertyGetterDelegate<TPropertyType>(object? instance, PropertyInfo propertyInfo)
    {
        // TODO: Check provided generic type against the provided fieldInfo type and try to convert if they not match
        var instanceObject = instance == null ? null : Expression.Constant(instance);
        var propertyExp = Expression.Property(instanceObject, propertyInfo);
        return Expression.Lambda<Func<TPropertyType>>(propertyExp).Compile();
    }

    public static Func<TInstance, TPropertyType> CreatePropertyGetterDelegate<TInstance, TPropertyType>(PropertyInfo propertyInfo)
    {
        // TODO: Check provided generic type against the provided fieldInfo type and try to convert if they don't match
        var instanceParameter = Expression.Parameter(typeof(TInstance), "instance");
        var propertyExp = Expression.Property(instanceParameter, propertyInfo);
        return Expression.Lambda<Func<TInstance, TPropertyType>>(propertyExp, instanceParameter).Compile();
    }

    public static Action<TPropertyType> CreatePropertySetterDelegate<TPropertyType>(object? instance, PropertyInfo propertyInfo)
    {
        // TODO: Check provided generic type against the provided fieldInfo type and try to convert if they don't match
        // TODO: Check if this property is writable.
        var genericType = typeof(TPropertyType);
        var instanceObject = instance == null ? null : Expression.Constant(instance);
        var propertyExp = Expression.Property(instanceObject, propertyInfo);
        var propertyValueParam = Expression.Parameter(genericType, propertyInfo.Name);
        var assignExpression = Expression.Assign(propertyExp, propertyValueParam);
        return Expression.Lambda<Action<TPropertyType>>(assignExpression, propertyValueParam).Compile();
    }

    public static Action<TInstance, TPropertyType> CreatePropertySetterDelegate<TInstance, TPropertyType>(PropertyInfo propertyInfo)
    {
        // TODO: Check provided generic type against the provided fieldInfo type and try to convert if they don't match
        // TODO: Check if this property is writable.
        var instanceType = typeof(TInstance);
        var fieldType = typeof(TPropertyType);
        var instanceParameter = Expression.Parameter(instanceType);
        var propertyExp = Expression.Property(instanceParameter, propertyInfo);
        var propertyValueParam = Expression.Parameter(fieldType, propertyInfo.Name);
        var assignExpression = Expression.Assign(propertyExp, propertyValueParam);
        return Expression
            .Lambda<Action<TInstance, TPropertyType>>(assignExpression, instanceParameter, propertyValueParam)
            .Compile();
    }
}