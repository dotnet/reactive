// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading;

namespace System.Reactive.Concurrency
{
    internal static class SynchronizationContextExtensions
    {
        public static void PostWithStartComplete<T>(this SynchronizationContext context, Action<T> action, T state)
        {
            context.OperationStarted();

            context.Post(
                o =>
                {
                    try
                    {
                        action((T)o);
                    }
                    finally
                    {
                        context.OperationCompleted();
                    }
                },
                state
            );
        }

        public static void PostWithStartComplete(this SynchronizationContext context, Action action)
        {
            context.OperationStarted();

            context.Post(
                _ =>
                {
                    try
                    {
                        action();
                    }
                    finally
                    {
                        context.OperationCompleted();
                    }
                },
                null
            );
        }
    }
}
