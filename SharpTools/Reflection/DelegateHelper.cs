using System.Linq.Expressions;
using System.Reflection;

namespace SharpTools.Reflection;

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
}