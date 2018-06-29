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
        public static IAsyncEnumerable<TSource> Finally<TSource>(this IAsyncEnumerable<TSource> source, Action finallyAction)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (finallyAction == null)
            {
                throw new ArgumentNullException(nameof(finallyAction));
            }

            return new FinallyAsyncIterator<TSource>(source, finallyAction);
        }

        private sealed class FinallyAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly Action finallyAction;
            private readonly IAsyncEnumerable<TSource> source;

            private IAsyncEnumerator<TSource> enumerator;
            private CancellationTokenRegistration _tokenRegistration;
            private int _once;

            public FinallyAsyncIterator(IAsyncEnumerable<TSource> source, Action finallyAction)
            {
                Debug.Assert(source != null);
                Debug.Assert(finallyAction != null);

                this.source = source;
                this.finallyAction = finallyAction;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new FinallyAsyncIterator<TSource>(source, finallyAction);
            }

            public override void Dispose()
            {
                // This could now be executed by either MoveNextCore
                // or the trigger from a CancellationToken
                // make sure this happens at most once.
                if (Interlocked.CompareExchange(ref _once, 1, 0) == 0)
                {
                    if (enumerator != null)
                    {
                        enumerator.Dispose();
                        // make sure the clearing of the enumerator
                        // becomes visible to MoveNextCore
                        Volatile.Write(ref enumerator, null);

                        finallyAction();
                    }

                    base.Dispose();
                    _tokenRegistration.Dispose();
                }
            }

            protected override async Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetEnumerator();
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        // clear any previous registration
                        _tokenRegistration.Dispose();
                        // and setup a new registration
                        // we can't know if the token is the same as last time
                        // note that the registration extends the lifetime of "this"
                        // so the current AsyncIterator better not be just abandoned
                        _tokenRegistration = cancellationToken.Register(
                            state => ((FinallyAsyncIterator<TSource>)state).Dispose(), this);

                        // Now that the CancellationToken may call Dispose
                        // from any thread while the current thread is in
                        // MoveNextCore, we must make sure the enumerator
                        // hasn't been cleared out in the meantime
                        var en = Volatile.Read(ref enumerator);
                        if (en != null)
                        {
                            if (await en.MoveNext(cancellationToken)
                                                .ConfigureAwait(false))
                            {
                                current = enumerator.Current;
                                return true;
                            }

                            Dispose();
                        }
                        break;
                }

                return false;
            }
        }
    }
}
