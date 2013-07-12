// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

#if HAS_WINRT
using System.Runtime.InteropServices.WindowsRuntime;
#endif

namespace System.Reactive
{
    static class ReflectionUtils
    {
        public static TDelegate CreateDelegate<TDelegate>(object o, MethodInfo method)
        {
#if (CRIPPLED_REFLECTION && HAS_WINRT)
            return (TDelegate)(object)method.CreateDelegate(typeof(TDelegate), o);
#else
            return (TDelegate)(object)Delegate.CreateDelegate(typeof(TDelegate), o, method);
#endif
        }

        public static Delegate CreateDelegate(Type delegateType, object o, MethodInfo method)
        {
#if (CRIPPLED_REFLECTION && HAS_WINRT)
            return method.CreateDelegate(delegateType, o);
#else
            return Delegate.CreateDelegate(delegateType, o, method);
#endif
        }

        public static void GetEventMethods<TSender, TEventArgs>(Type targetType, object target, string eventName, out MethodInfo addMethod, out MethodInfo removeMethod, out Type delegateType, out bool isWinRT)
#if !NO_EVENTARGS_CONSTRAINT
            where TEventArgs : EventArgs
#endif
        {
            var e = default(EventInfo);

            if (target == null)
            {
                e = targetType.GetEventEx(eventName, true);
                if (e == null)
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings_Linq.COULD_NOT_FIND_STATIC_EVENT, eventName, targetType.FullName));
            }
            else
            {
                e = targetType.GetEventEx(eventName, false);
                if (e == null)
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings_Linq.COULD_NOT_FIND_INSTANCE_EVENT, eventName, targetType.FullName));
            }

            addMethod = e.GetAddMethod();
            removeMethod = e.GetRemoveMethod();

            if (addMethod == null)
                throw new InvalidOperationException(Strings_Linq.EVENT_MISSING_ADD_METHOD);
            if (removeMethod == null)
                throw new InvalidOperationException(Strings_Linq.EVENT_MISSING_REMOVE_METHOD);

            var psa = addMethod.GetParameters();
            if (psa.Length != 1)
                throw new InvalidOperationException(Strings_Linq.EVENT_ADD_METHOD_SHOULD_TAKE_ONE_PARAMETER);

            var psr = removeMethod.GetParameters();
            if (psr.Length != 1)
                throw new InvalidOperationException(Strings_Linq.EVENT_REMOVE_METHOD_SHOULD_TAKE_ONE_PARAMETER);

            isWinRT = false;

#if HAS_WINRT
            if (addMethod.ReturnType == typeof(EventRegistrationToken))
            {
                isWinRT = true;

                var pet = psr[0];
                if (pet.ParameterType != typeof(EventRegistrationToken))
                    throw new InvalidOperationException(Strings_Linq.EVENT_WINRT_REMOVE_METHOD_SHOULD_TAKE_ERT);
            }
#endif

            delegateType = psa[0].ParameterType;

            var invokeMethod = delegateType.GetMethod("Invoke");

            var parameters = invokeMethod.GetParameters();

            if (parameters.Length != 2)
                throw new InvalidOperationException(Strings_Linq.EVENT_PATTERN_REQUIRES_TWO_PARAMETERS);

            if (!typeof(TSender).IsAssignableFrom(parameters[0].ParameterType))
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings_Linq.EVENT_SENDER_NOT_ASSIGNABLE, typeof(TSender).FullName));

            if (!typeof(TEventArgs).IsAssignableFrom(parameters[1].ParameterType))
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings_Linq.EVENT_ARGS_NOT_ASSIGNABLE, typeof(TEventArgs).FullName));

            if (invokeMethod.ReturnType != typeof(void))
                throw new InvalidOperationException(Strings_Linq.EVENT_MUST_RETURN_VOID);
        }

        public static EventInfo GetEventEx(this Type type, string name, bool isStatic)
        {
#if (CRIPPLED_REFLECTION && HAS_WINRT)
            // TODO: replace in the future by System.Reflection.RuntimeExtensions extension methods
            var q = new Queue<TypeInfo>();
            q.Enqueue(type.GetTypeInfo());

            while (q.Count > 0)
            {
                var t = q.Dequeue();

                var e = t.GetDeclaredEvent(name);
                if (e != null)
                    return e;

                foreach (var i in t.ImplementedInterfaces)
                    q.Enqueue(i.GetTypeInfo());

                if (t.BaseType != null)
                    q.Enqueue(t.BaseType.GetTypeInfo());
            }

            return null;
#else
            return type.GetEvent(name, isStatic ? BindingFlags.Public | BindingFlags.Static : BindingFlags.Public | BindingFlags.Instance);
#endif
        }

#if (CRIPPLED_REFLECTION && HAS_WINRT)
        public static MethodInfo GetMethod(this Type type, string name)
        {
            return type.GetTypeInfo().GetDeclaredMethod(name);
        }

        public static MethodInfo GetAddMethod(this EventInfo eventInfo)
        {
            return eventInfo.AddMethod;
        }

        public static MethodInfo GetRemoveMethod(this EventInfo eventInfo)
        {
            return eventInfo.RemoveMethod;
        }

        public static bool IsAssignableFrom(this Type type1, Type type2)
        {
            return type1.GetTypeInfo().IsAssignableFrom(type2.GetTypeInfo());
        }
#endif
    }
}
