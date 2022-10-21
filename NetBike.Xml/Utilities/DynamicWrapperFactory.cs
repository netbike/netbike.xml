namespace NetBike.Xml.Utilities
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    internal static class DynamicWrapperFactory
    {
        private static readonly Type CreatorType = typeof(Func<object>);
        private static readonly Type GetterType = typeof(Func<object, object>);
        private static readonly Type SetterType = typeof(Action<object, object>);
        private static readonly Module Module = typeof(DynamicWrapperFactory).Module;

        public static Func<object> CreateConstructor(Type valueType)
        {
            var dynamicMethod = new DynamicMethod(
                "Create" + valueType.FullName,
                Types.Object,
                Type.EmptyTypes,
                Module)
            {
                InitLocals = true
            };

            var generator = dynamicMethod.GetILGenerator();

            if (valueType.IsValueType)
            {
                generator.DeclareLocal(valueType);
                generator.Emit(OpCodes.Ldloc_0);
                generator.Emit(OpCodes.Box, valueType);
            }
            else
            {
                var constructor = valueType.GetDefaultConstructor();

                if (constructor == null)
                {
                    throw new ArgumentException("Type \"{0}\" hasn't default constructor.", nameof(valueType));
                }

                generator.Emit(OpCodes.Newobj, constructor);
            }

            generator.Emit(OpCodes.Ret);

            return (Func<object>)dynamicMethod.CreateDelegate(CreatorType);
        }

        public static Func<object, object> CreateGetter(PropertyInfo propertyInfo)
        {
            var ownerType = propertyInfo.DeclaringType;
            var returnType = propertyInfo.PropertyType;

            var dynamicMethod = new DynamicMethod(
                "Get" + propertyInfo.Name,
                Types.Object,
                new Type[] { Types.Object },
                Module,
                false);

            var generator = dynamicMethod.GetILGenerator();
            var getMethod = propertyInfo.GetGetMethod();

            PushOwner(ownerType, generator, getMethod);
            EmitMethodCall(generator, getMethod);

            if (returnType.IsValueType)
            {
                generator.Emit(OpCodes.Box, returnType);
            }

            generator.Emit(OpCodes.Ret);

            return (Func<object, object>)dynamicMethod.CreateDelegate(GetterType);
        }

        public static Action<object, object> CreateSetter(PropertyInfo propertyInfo)
        {
            var ownerType = propertyInfo.DeclaringType;
            var returnType = propertyInfo.PropertyType;

            var dynamicMethod = new DynamicMethod(
                "Set" + propertyInfo.Name,
                Types.Void,
                new Type[] { Types.Object, Types.Object },
                Module,
                false);

            var generator = dynamicMethod.GetILGenerator();
            var setMethod = propertyInfo.GetSetMethod(true);

            PushOwner(ownerType, generator, setMethod);
            generator.Emit(OpCodes.Ldarg_1);
            EmitUnboxOrCast(generator, propertyInfo.PropertyType);
            EmitMethodCall(generator, setMethod);
            generator.Emit(OpCodes.Ret);

            return (Action<object, object>)dynamicMethod.CreateDelegate(SetterType);
        }

        private static void EmitUnboxOrCast(ILGenerator il, Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType != null)
            {
                il.Emit(OpCodes.Unbox_Any, underlyingType);
                
                var ci = typeof(Nullable<>).MakeGenericType(underlyingType).GetConstructor(new[] { underlyingType });
                il.Emit(OpCodes.Newobj, ci);
            }
            else
            {
                var opCode = type.IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass;
                il.Emit(opCode, type);
            }
        }

        private static void EmitMethodCall(ILGenerator il, MethodInfo method)
        {
            var opCode = method.IsFinal ? OpCodes.Call : OpCodes.Callvirt;
            il.Emit(opCode, method);
        }

        private static void PushOwner(Type ownerType, ILGenerator generator, MethodInfo getMethod)
        {
            if (!getMethod.IsStatic)
            {
                generator.Emit(OpCodes.Ldarg_0);
                EmitUnboxOrCast(generator, ownerType);
            }
        }
    }
}