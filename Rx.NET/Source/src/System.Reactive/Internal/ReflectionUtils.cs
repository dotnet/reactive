// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;

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

#if HAS_TRIMMABILITY_ATTRIBUTES
    [RequiresUnreferencedCode(Constants_Core.EventReflectionTrimIncompatibilityMessage)]
#endif
        public static void GetEventMethods<TSender, TEventArgs>(Type targetType, object? target, string eventName, out MethodInfo addMethod, out MethodInfo removeMethod, out Type delegateType, out bool isWinRT)
        {
            EventInfo? e;

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

            addMethod = e.GetAddMethod() ?? throw new InvalidOperationException(Strings_Linq.EVENT_MISSING_ADD_METHOD);
            removeMethod = e.GetRemoveMethod() ?? throw new InvalidOperationException(Strings_Linq.EVENT_MISSING_REMOVE_METHOD);

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

            if (IsWinRTEventRegistrationTokenType(addMethod.ReturnType))
            {
                isWinRT = true;

                var pet = psr[0];
                if (IsWinRTEventRegistrationTokenType(pet.ParameterType))
                {
                    throw new InvalidOperationException(Strings_Linq.EVENT_WINRT_REMOVE_METHOD_SHOULD_TAKE_ERT);
                }
            }

            delegateType = psa[0].ParameterType;

            var invokeMethod = delegateType.GetMethod("Invoke")!; // NB: Delegates always have an Invoke method.

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

        /// <summary>
        /// Determine whether a type represents a WinRT event registration token
        /// (https://learn.microsoft.com/en-us/uwp/api/windows.foundation.eventregistrationtoken).
        /// </summary>
        /// <param name="t">The type to check.</param>
        /// <returns>True if this represents a WinRT event registration token</returns>
        /// <remarks>
        /// <para>
        /// We used to perform a simple comparison with typeof(EventRegistrationToken), but the
        /// introduction of C#/WinRT has made this problematic. Before C#/WinRT, the .NET
        /// projection of WinRT's Windows.Foundation.EventRegistrationToken type was
        /// System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken. But that type is
        /// associated with the old WinRT interop mechanisms in which the CLR works directly with
        /// WinMD. That was how it worked up as far as .NET Core 3.1, and it's still how .NET
        /// Framework works, but this direct WinMD support was removed in .NET 5.0.
        /// </para>
        /// <para>
        /// If you're on .NET 5.0 or later, the System.Runtime.InteropServices.WindowsRuntime types
        /// are no longer supported. While you can still get access to them through the NuGet
        /// package of the same name (that's how .NET Standard 2.0 libraries are able to use these
        /// types) they are best avoided, because the types in that library are no longer the types
        /// you see when any of the WinRT types they are meant to represent are projected into the
        /// CLR's world.
        /// </para>
        /// <para>
        /// It was therefore necessary for Rx to stop using these types, and to drop its reference
        /// to the System.Runtime.InteropServices.WindowsRuntime package. We can replicate the
        /// same logic by looking for the type name. By checking for either the old or new
        /// namespaces, we can support both the old projection (still used on .NET Framework) and
        /// also the new C#/WinRT projection.
        /// </para>
        /// </remarks>
        private static bool IsWinRTEventRegistrationTokenType(Type t) =>
            t.Name == "EventRegistrationToken" &&
            (t.Namespace == "System.Runtime.InteropServices.WindowsRuntime" ||
             t.Namespace == "WinRT");
    }
}
