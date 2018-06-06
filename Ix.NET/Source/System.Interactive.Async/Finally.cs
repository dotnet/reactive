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
                throw new ArgumentNullException(nameof(source));
            if (finallyAction == null)
                throw new ArgumentNullException(nameof(finallyAction));

            return new FinallyAsyncIterator<TSource>(source, finallyAction);
        }

        private sealed class FinallyAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly Action finallyAction;
            private readonly IAsyncEnumerable<TSource> source;

            private IAsyncEnumerator<TSource> enumerator;

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
                if (enumerator != null)
                {
                    enumerator.Dispose();
                    enumerator = null;

                    finallyAction();
                }

                base.Dispose();
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
                        if (await enumerator.MoveNext(cancellationToken)
                                            .ConfigureAwait(false))
                        {
                            current = enumerator.Current;
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