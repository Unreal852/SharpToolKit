using System.Reflection;
using SharpToolKit.Reflection;
using SharpToolKit.Timing;

namespace SharpToolKit.Sample;

public static class ReflectionSample
{
    private static readonly ReflectMe                           DefaultInstance;
    private static readonly ReflectMe                           FastInstance;
    private static readonly FieldInfo                           FieldInfo;
    private static readonly PropertyInfo                        PropertyInfo;
    private static readonly MethodInfo                          MethodInfo;
    private static readonly FieldAccessor<ReflectMe, string>    FieldAccessor;
    private static readonly PropertyAccessor<ReflectMe, string> PropertyAccessor;
    private static readonly Func<int, int, int>                 MethodAccessor;

    static ReflectionSample()
    {
        // Cache everything needed
        
        var instanceType = typeof(ReflectMe);

        // Initialize default reflection 
        DefaultInstance = new ReflectMe();
        FieldInfo = instanceType.GetField("_myPrivateField", BindingFlags.Instance | BindingFlags.NonPublic) ??
                     throw new Exception("Field not found");
        PropertyInfo = instanceType.GetProperty("MyPublicProperty", BindingFlags.Instance | BindingFlags.Public) ??
                        throw new Exception("Property not found");
        MethodInfo = instanceType.GetMethod("Sum", BindingFlags.Instance | BindingFlags.Public) ??
                      throw new Exception("Method not found");

        // Initialize fast accessors
        FastInstance = new ReflectMe();
        FieldAccessor = instanceType.GetFieldAccessor<ReflectMe, string>("_myPrivateField")!.Value;
        PropertyAccessor = instanceType.GetPropertyAccessor<ReflectMe, string>("MyPublicProperty")!.Value;
        MethodAccessor = instanceType.GetMethodDelegate<Func<int, int, int>>(FastInstance, "Sum")!;
    }

    public static void CompareSpeed(Action<OperationResult> endCallback, int iterations = 100)
    {
        var fieldValue = string.Empty;
        var propertyValue = string.Empty;
        var methodResult = 0;

        using (Profiler.RunNew("Default Reflection", endCallback))
        {
            for (var i = 0; i < iterations; i++)
            {
                fieldValue = (string)FieldInfo.GetValue(DefaultInstance)!;
                propertyValue = (string)PropertyInfo.GetValue(DefaultInstance)!;

                FieldInfo.SetValue(DefaultInstance, "I have been renamed by a field accessor");
                PropertyInfo.SetValue(DefaultInstance, "I have been renamed by a property accessor");

                fieldValue = (string)FieldInfo.GetValue(DefaultInstance)!;
                propertyValue = (string)PropertyInfo.GetValue(DefaultInstance)!;
            }
        }

        fieldValue = string.Empty;
        propertyValue = string.Empty;
        methodResult = 0;

        using (Profiler.RunNew("Custom Reflection", endCallback))
        {
            for (var i = 0; i < iterations; i++)
            {
                fieldValue = FieldAccessor.Get(FastInstance);
                propertyValue = PropertyAccessor.Get(FastInstance);

                FieldAccessor.Set(FastInstance, "I have been renamed by a field accessor");
                PropertyAccessor.Set(FastInstance, "I have been renamed by a property accessor");

                fieldValue = FieldAccessor.Get(FastInstance);
                propertyValue = PropertyAccessor.Get(FastInstance);
            }
        }
    }

    public static void ExecuteSample()
    {
        var instance = new ReflectMe();
        var instanceType = typeof(ReflectMe);
        var fieldAccess = instanceType.GetFieldAccessor<string>(instance, "_myPrivateField")!.Value;
        var propertyAccess = instanceType.GetPropertyAccessor<string>(instance, "MyPublicProperty")!.Value;

        Console.WriteLine($"Original Field: {fieldAccess.Get()}");
        Console.WriteLine($"Original Property: {propertyAccess.Get()}");

        fieldAccess.Set("I have been renamed by a field accessor");
        propertyAccess.Set("I have been renamed by a property accessor");

        Console.WriteLine($"New Field: {fieldAccess.Get()}");
        Console.WriteLine($"New Property: {propertyAccess.Get()}");

        var methodDelegate = instanceType.GetMethodDelegate<Func<int, int, int>>(instance, "Sum")!;

        Console.WriteLine($"4 + 5 = {methodDelegate(4, 5)}");
    }
}

public class ReflectMe
{
    private string _myPrivateField = "Hello field.";

    public string MyPublicProperty { get; set; } = "Hello property.";

    public int Sum(int a, int b)
    {
        return a + b;
    }
}