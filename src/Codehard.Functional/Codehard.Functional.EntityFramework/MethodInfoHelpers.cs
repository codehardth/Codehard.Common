using System.Reflection;

namespace Codehard.Functional.EntityFramework;

public class MethodInfoHelpers
{
    private record GenericParameterInfo(Type Type, int Position);

    public static bool AreMethodsEqual(MethodInfo method1, MethodInfo method2)
    {
        // Check if the methods have the same name
        if (method1.Name != method2.Name)
        {
            return false;
        }

        // Check if the methods have the same number of parameters
        if (method1.GetParameters().Length != method2.GetParameters().Length)
        {
            return false;
        }

        // Check if the methods have the same parameter types
        var parameters1 = method1.GetParameters().Select(GetParameterType).ToArray();
        var parameters2 = method2.GetParameters().Select(GetParameterType).ToArray();
        if (!parameters1.SequenceEqual(parameters2))
        {
            return false;
        }

        // Check if the methods have the same return type
        var returnType1 = GetReturnType(method1);
        var returnType2 = GetReturnType(method2);
        if (returnType1 != returnType2)
        {
            return false;
        }

        // If the methods are generic, check if they have the same generic type parameters
        if (method1.IsGenericMethod != method2.IsGenericMethod)
        {
            return false;
        }

        if (method1.IsGenericMethod && method2.IsGenericMethod)
        {
            var genericParameters1 = method1.GetGenericArguments();
            var genericParameters2 = method2.GetGenericArguments();
            if (genericParameters1.Length != genericParameters2.Length)
            {
                return false;
            }

            for (int i = 0; i < genericParameters1.Length; i++)
            {
                if (genericParameters1[i] != genericParameters2[i])
                {
                    return false;
                }
            }
        }

        // The methods are equal
        return true;
    }

    private static Type GetParameterType(ParameterInfo parameterInfo)
    {
        var type = parameterInfo.ParameterType;

        if (type.IsGenericParameter && parameterInfo.Member is MethodInfo method)
        {
            // If the parameter type is a generic parameter and belongs to a generic method,
            // return the corresponding type parameter of the method
            return method.GetGenericArguments()[type.GenericParameterPosition];
        }

        // if (type.IsGenericType)
        // {
        //     // If the parameter type is a generic type, recursively call GetParameterType on its generic type arguments
        //     var genericArguments = type.GetGenericArguments();
        //     var newArguments = new Type[genericArguments.Length];
        //
        //     for (var i = 0; i < genericArguments.Length; i++)
        //     {
        //         newArguments[i] = GetParameterType(new GenericParameterInfo(genericArguments[i], parameterInfo));
        //     }
        //
        //     return type.GetGenericTypeDefinition().MakeGenericType(newArguments);
        // }

        return type;
    }

    private static Type GetReturnType(MethodInfo methodInfo)
    {
        var type = methodInfo.ReturnType;

        if (type.IsGenericParameter && methodInfo.IsGenericMethod)
        {
            // If the return type is a generic parameter and the method is generic,
            // return the corresponding type parameter of the method
            return methodInfo.GetGenericArguments()[type.GenericParameterPosition];
        }

        return type;
    }
}