namespace NetBike.Xml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal static class TypeExtensions
    {
        private static readonly Dictionary<Type, string> ShortNames = new Dictionary<Type, string>
        {
            { Types.String, "String" },
            { Types.Byte, "Byte" },
            { Types.Int16, "Short" },
            { Types.Int32, "Int" },
            { Types.Int64, "Long" },
            { Types.Char, "Char" },
            { Types.Float, "Float" },
            { Types.Double, "Double" },
            { Types.Bool, "Bool" },
            { Types.Decimal, "Decimal" }
        };

        public static bool IsBasicType(this Type type)
        {
            return type.IsPrimitive || type == Types.String;
        }

        public static bool IsEnumerable(this Type type)
        {
            return type.GetInterfaces().Any(x => x == Types.Enumerable);
        }

        public static bool IsFinalType(this Type type)
        {
            return type.IsValueType || type.IsSealed;
        }

        public static bool IsActivable(this Type type)
        {
            return !type.IsAbstract && !type.IsInterface && type.HasDefaultConstructor();
        }

        public static ConstructorInfo GetDefaultConstructor(this Type type)
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            return type.GetConstructor(bindingFlags, null, Type.EmptyTypes, null);
        }

        public static bool HasDefaultConstructor(this Type type)
        {
            return GetDefaultConstructor(type) != null;
        }

        public static Type GetEnumerableItemType(this Type type)
        {
            if (type.IsArray)
            {
                return type.GetElementType();
            }

            Type elementType = null;

            if (type == Types.Enumerable)
            {
                elementType = Types.Object;
            }
            else if (type.IsGenericType && 
                type.GetGenericTypeDefinition() == Types.EnumerableDefinition)
            {
                elementType = type.GetGenericArguments()[0];
            }
            else
            {
                foreach (var interfaceType in type.GetInterfaces())
                {
                    if (interfaceType == Types.Enumerable)
                    {
                        elementType = Types.Object;
                    }
                    else if (interfaceType.IsGenericType &&
                        interfaceType.GetGenericTypeDefinition() == Types.EnumerableDefinition)
                    {
                        elementType = interfaceType.GetGenericArguments()[0];
                        break;
                    }
                }
            }

            return elementType;
        }

        public static bool IsNullable(this Type type)
        {
            return type.IsGenericTypeOf(Types.NullableDefinition);
        }

        public static Type GetUnderlyingNullableType(this Type type)
        {
            if (type.IsNullable())
            {
                return type.GetGenericArguments()[0];
            }

            return null;
        }

        public static bool IsGenericTypeOf(this Type type, Type definitionType)
        {
            return type.IsGenericType
                && !type.IsGenericTypeDefinition
                && type.GetGenericTypeDefinition() == definitionType;
        }

        public static bool IsGenericTypeOf(this Type type, params Type[] definitionTypes)
        {
            if (type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                var typeDefinition = type.GetGenericTypeDefinition();

                foreach (var expectedType in definitionTypes)
                {
                    if (typeDefinition == expectedType)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static string GetShortName(this Type type)
        {
            if (!ShortNames.TryGetValue(type, out var shortName))
            {
                if (type.IsArray)
                {
                    shortName = "ArrayOf" + GetShortName(type.GetElementType());
                }
                else
                {
                    shortName = type.Name;

                    if (type.IsGenericType)
                    {
                        var typeDefIndex = shortName.LastIndexOf('`');

                        if (typeDefIndex != -1)
                        {
                            shortName = shortName.Substring(0, typeDefIndex);
                        }
                    }
                }
            }

            return shortName;
        }

        public static MethodInfo GetStaticMethod(this Type type, string methodName, params Type[] parameters)
        {
            var bindingFlags = BindingFlags.Static | BindingFlags.Public;
            return type.GetMethod(methodName, bindingFlags, null, parameters ?? Type.EmptyTypes, null);
        }

        public static MethodInfo GetInstanceMethod(this Type type, string methodName, params Type[] parameters)
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            return type.GetMethod(methodName, bindingFlags, null, parameters ?? Type.EmptyTypes, null);
        }
    }
}