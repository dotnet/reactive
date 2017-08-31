// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<T> CreateEnumerable<T>(Func<IAsyncEnumerator<T>> getEnumerator)
        {
            if (getEnumerator == null)
                throw new ArgumentNullException(nameof(getEnumerator));

            return new AnonymousAsyncEnumerable<T>(getEnumerator);
        }

        public static IAsyncEnumerator<T> CreateEnumerator<T>(Func<Task<bool>> moveNext, Func<T> current, Func<Task> dispose)
        {
            if (moveNext == null)
                throw new ArgumentNullException(nameof(moveNext));

            // Note: Many methods pass null in for the second two params. We're assuming
            // That the caller is responsible and knows what they're doing
            return new AnonymousAsyncIterator<T>(moveNext, current, dispose);
        }

        private static IAsyncEnumerator<T> CreateEnumerator<T>(Func<TaskCompletionSource<bool>, Task<bool>> moveNext, Func<T> current, Func<Task> dispose)
        {
            var self = new AnonymousAsyncIterator<T>(
                async () =>
                {
                    var tcs = new TaskCompletionSource<bool>();

                    var stop = new Action(
                        () =>
                        {
                            tcs.TrySetCanceled();
                        });

                    return await moveNext(tcs)
                                .ConfigureAwait(false);
                },
                current,
                dispose
            );
            return self;
        }

        private class AnonymousAsyncEnumerable<T> : IAsyncEnumerable<T>
        {
            private readonly Func<IAsyncEnumerator<T>> getEnumerator;

            public AnonymousAsyncEnumerable(Func<IAsyncEnumerator<T>> getEnumerator)
            {
                Debug.Assert(getEnumerator != null);

                this.getEnumerator = getEnumerator;
            }

            public IAsyncEnumerator<T> GetAsyncEnumerator()
            {
                return getEnumerator();
            }
        }

        private sealed class AnonymousAsyncIterator<T> : AsyncIterator<T>
        {
            private readonly Func<T> currentFunc;
            private readonly Func<Task> dispose;
            private readonly Func<Task<bool>> moveNext;


            public AnonymousAsyncIterator(Func<Task<bool>> moveNext, Func<T> currentFunc, Func<Task> dispose)
            {
                Debug.Assert(moveNext != null);

                this.moveNext = moveNext;
                this.currentFunc = currentFunc;
                this.dispose = dispose;

                // Explicit call to initialize enumerator mode
                GetAsyncEnumerator();
            }

            public override AsyncIterator<T> Clone()
            {
                throw new NotSupportedException("AnonymousAsyncIterator cannot be cloned. It is only intended for use as an iterator.");
            }

            public override async Task DisposeAsync()
            {
                if (dispose != null)
                {
                    await dispose().ConfigureAwait(false);
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async Task<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (await moveNext().ConfigureAwait(false))
                        {
                            current = currentFunc();
                            return true;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
        }
    }
}