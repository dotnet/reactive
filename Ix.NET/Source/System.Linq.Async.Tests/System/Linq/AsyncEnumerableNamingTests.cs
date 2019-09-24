// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class AsyncEnumerableNamingTests
    {
        [Fact]
        public static void AsyncEnumerable_MethodNames()
        {
            var methods = typeof(AsyncEnumerable).GetMethods(BindingFlags.Public | BindingFlags.Static);

            //
            // Async suffix
            //

            var asyncMethodsNoAsyncSuffix = (from m in methods
                                             where IsTaskLike(m.ReturnType)
                                             where !m.Name.EndsWith("Async")
                                             select m.Name)
                                            .ToArray();

            Assert.Empty(asyncMethodsNoAsyncSuffix);

            //
            // Consistency of delegate types and Await[WithCancellation] naming convention
            //

            var methodsWithDelegateParameter = (from m in methods
                                                where m.GetParameters().Any(p => IsAsyncDelegate(p.ParameterType))
                                                select m)
                                               .ToArray();

            foreach (var m in methodsWithDelegateParameter)
            {
                var kinds = (from p in m.GetParameters()
                             where IsDelegate(p.ParameterType)
                             select GetDelegateKind(p.ParameterType))
                            .Distinct();

                Assert.Single(kinds);

                var suffix = IsTaskLike(m.ReturnType) ? "Async" : "";

                switch (kinds.Single())
                {
                    case DelegateKind.Async:
                        suffix = "Await" + suffix;
                        break;
                    case DelegateKind.AsyncCancel:
                        suffix = "AwaitWithCancellation" + suffix;
                        break;
                }

                Assert.EndsWith(suffix, m.Name);
            }

            static bool IsValueTask(Type t) => t == typeof(ValueTask) || (t.IsConstructedGenericType && t.GetGenericTypeDefinition() == typeof(ValueTask<>));
            static bool IsTask(Type t) => typeof(Task).IsAssignableFrom(t);
            static bool IsTaskLike(Type t) => IsTask(t) || IsValueTask(t);

            static bool IsDelegate(Type t) => typeof(Delegate).IsAssignableFrom(t);
            static bool TryGetInvoke(Type t, out MethodInfo? m) => (m = t.GetMethod("Invoke")) != null;
            static bool IsAsyncDelegate(Type t) => IsDelegate(t) && TryGetInvoke(t, out var i) && IsTaskLike(i!.ReturnType);
            static bool IsCancelableDelegate(Type t) => IsDelegate(t) && TryGetInvoke(t, out var i) && i!.GetParameters().LastOrDefault()?.ParameterType == typeof(CancellationToken);
            static DelegateKind GetDelegateKind(Type t) => IsAsyncDelegate(t) ? (IsCancelableDelegate(t) ? DelegateKind.AsyncCancel : DelegateKind.Async) : DelegateKind.Sync;
        }

        private enum DelegateKind
        {
            Sync,
            Async,
            AsyncCancel,
        }
    }
}
