// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Globalization;
using System.Reflection;

#if HAS_WINRT
using System.Runtime.InteropServices.WindowsRuntime;
#endif

namespace System.Reactive
{
    internal static class ReflectionUtils
    {
        public static TDelegate CreateDelegate<TDelegate>(object o, MethodInfo method)
        {
            return (TDelegate)(object)method.CreateDelegate(typeof(TDelegate), o);
        }

        public static Delegate CreateDelegate(Type delegateType, object o, MethodInfo method)
        {
            return method.CreateDelegate(delegateType, o);
        }

        public static void GetEventMethods<TSender, TEventArgs>(Type targetType, object target, string eventName, out MethodInfo addMethod, out MethodInfo removeMethod, out Type delegateType, out bool isWinRT)
        {
            var e = default(EventInfo);

            if (target == null)
            {
                e = targetType.GetEvent(eventName, BindingFlags.Public | BindingFlags.Static);
                if (e == null)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings_Linq.COULD_NOT_FIND_STATIC_EVENT, eventName, targetType.FullName));
                }
            }
            else
            {
                e = targetType.GetEvent(eventName, BindingFlags.Public | BindingFlags.Instance);
                if (e == null)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings_Linq.COULD_NOT_FIND_INSTANCE_EVENT, eventName, targetType.FullName));
                }
            }

            addMethod = e.GetAddMethod();
            removeMethod = e.GetRemoveMethod();

            if (addMethod == null)
            {
                throw new InvalidOperationException(Strings_Linq.EVENT_MISSING_ADD_METHOD);
            }

            if (removeMethod == null)
            {
                throw new InvalidOperationException(Strings_Linq.EVENT_MISSING_REMOVE_METHOD);
            }

            var psa = addMethod.GetParameters();
            if (psa.Length != 1)
            {
                throw new InvalidOperationException(Strings_Linq.EVENT_ADD_METHOD_SHOULD_TAKE_ONE_PARAMETER);
            }

            var psr = removeMethod.GetParameters();
            if (psr.Length != 1)
            {
                throw new InvalidOperationException(Strings_Linq.EVENT_REMOVE_METHOD_SHOULD_TAKE_ONE_PARAMETER);
            }

            isWinRT = false;

#if HAS_WINRT && !CSWINRT
            if (addMethod.ReturnType == typeof(EventRegistrationToken))
            {
                isWinRT = true;

                var pet = psr[0];
                if (pet.ParameterType != typeof(EventRegistrationToken))
                {
                    throw new InvalidOperationException(Strings_Linq.EVENT_WINRT_REMOVE_METHOD_SHOULD_TAKE_ERT);
                }
            }
#endif

            delegateType = psa[0].ParameterType;

            var invokeMethod = delegateType.GetMethod("Invoke");

            var parameters = invokeMethod.GetParameters();

            if (parameters.Length != 2)
            {
                throw new InvalidOperationException(Strings_Linq.EVENT_PATTERN_REQUIRES_TWO_PARAMETERS);
            }

            if (!typeof(TSender).IsAssignableFrom(parameters[0].ParameterType))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings_Linq.EVENT_SENDER_NOT_ASSIGNABLE, typeof(TSender).FullName));
            }

            if (!typeof(TEventArgs).IsAssignableFrom(parameters[1].ParameterType))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings_Linq.EVENT_ARGS_NOT_ASSIGNABLE, typeof(TEventArgs).FullName));
            }

            if (invokeMethod.ReturnType != typeof(void))
            {
                throw new InvalidOperationException(Strings_Linq.EVENT_MUST_RETURN_VOID);
            }
        }

#if (CRIPPLED_REFLECTION && HAS_WINRT)
        public static MethodInfo GetMethod(this Type type, string name) => type.GetTypeInfo().GetDeclaredMethod(name);
#endif
    }
}
