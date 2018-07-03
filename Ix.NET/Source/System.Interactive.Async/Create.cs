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
            {
                throw new ArgumentNullException(nameof(getEnumerator));
            }

            return new AnonymousAsyncEnumerable<T>(getEnumerator);
        }

        public static IAsyncEnumerator<T> CreateEnumerator<T>(Func<CancellationToken, Task<bool>> moveNext, Func<T> current, Action dispose)
        {
            if (moveNext == null)
            {
                throw new ArgumentNullException(nameof(moveNext));
            }

            // Note: Many methods pass null in for the second two parameters. We're assuming
            // That the caller is responsible and knows what they're doing
            return new AnonymousAsyncIterator<T>(moveNext, current, dispose);
        }

        private static IAsyncEnumerator<T> CreateEnumerator<T>(Func<CancellationToken, TaskCompletionSource<bool>, Task<bool>> moveNext, Func<T> current, Action dispose)
        {
            var self = new AnonymousAsyncIterator<T>(
                async ct =>
                {
                    var tcs = new TaskCompletionSource<bool>();

                    var stop = new Action(
                        () =>
                        {
                            tcs.TrySetCanceled();
                        });

                    using (ct.Register(stop))
                    {
                        return await moveNext(ct, tcs)
                                   .ConfigureAwait(false);
                    }
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

            public IAsyncEnumerator<T> GetEnumerator()
            {
                return getEnumerator();
            }
        }

        private sealed class AnonymousAsyncIterator<T> : AsyncIterator<T>
        {
            private readonly Func<T> currentFunc;
            private readonly Func<CancellationToken, Task<bool>> moveNext;
            private Action dispose;


            public AnonymousAsyncIterator(Func<CancellationToken, Task<bool>> moveNext, Func<T> currentFunc, Action dispose)
            {
                Debug.Assert(moveNext != null);

                this.moveNext = moveNext;
                this.currentFunc = currentFunc;
                Volatile.Write(ref this.dispose, dispose);

                // Explicit call to initialize enumerator mode
                GetEnumerator();
            }

            public override AsyncIterator<T> Clone()
            {
                throw new NotSupportedException("AnonymousAsyncIterator cannot be cloned. It is only intended for use as an iterator.");
            }

            public override void Dispose()
            {
                Interlocked.Exchange(ref this.dispose, null)?.Invoke();
                base.Dispose();
            }

            protected override async Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (await moveNext(cancellationToken).ConfigureAwait(false))
                        {
                            current = currentFunc();
                            return true;
                        }

                        Dispose();
                        break;
                }

                return false;
            }
        }
    }
}
