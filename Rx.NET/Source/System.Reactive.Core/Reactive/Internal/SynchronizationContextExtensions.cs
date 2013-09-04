// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_SYNCCTX
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
#endif