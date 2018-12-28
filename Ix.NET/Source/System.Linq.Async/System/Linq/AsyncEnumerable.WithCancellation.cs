// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        // REVIEW: [LDM-2018-11-28] Should return type be a struct or just the interface type? Should this live in the System.Linq namespace or in System.Collections.Generic?

        public static WithCancellationTokenAsyncEnumerable<T> WithCancellation<T>(this IAsyncEnumerable<T> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return new WithCancellationTokenAsyncEnumerable<T>(source, cancellationToken);
        }

        // REVIEW: Explicit implementation of the interfaces allows for composition with other "modifier operators" such as ConfigureAwait.
        //         We expect that the "await foreach" statement will bind to the public struct methods, thus avoiding boxing.

        public readonly struct WithCancellationTokenAsyncEnumerable<T> : IAsyncEnumerable<T>
        {
            private readonly IAsyncEnumerable<T> _source;
            private readonly CancellationToken _cancellationToken;

            public WithCancellationTokenAsyncEnumerable(IAsyncEnumerable<T> source, CancellationToken cancellationToken)
            {
                _source = source;
                _cancellationToken = cancellationToken;
            }

            // REVIEW: Should we simply ignore the second cancellation token or should we link the two?
            // REVIEW: [LDM-2018-11-28] Should we have eager cancellation here too?

            public WithCancellationAsyncEnumerator GetAsyncEnumerator(CancellationToken cancellationToken)
            {
                CancellationToken token;

                if (cancellationToken == default)
                {
                    token = _cancellationToken;
                }
                else if (_cancellationToken == default)
                {
                    token = cancellationToken;
                }
                else
                {
                    token = CancellationTokenSource.CreateLinkedTokenSource(_cancellationToken, cancellationToken).Token;
                }

                return new WithCancellationAsyncEnumerator(_source.GetAsyncEnumerator(token));
            }

            IAsyncEnumerator<T> IAsyncEnumerable<T>.GetAsyncEnumerator(CancellationToken cancellationToken)
                => GetAsyncEnumerator(cancellationToken);

            public readonly struct WithCancellationAsyncEnumerator : IAsyncEnumerator<T>
            {
                private readonly IAsyncEnumerator<T> _enumerator;

                public WithCancellationAsyncEnumerator(IAsyncEnumerator<T> enumerator) => _enumerator = enumerator;

                public T Current => _enumerator.Current;

                public ValueTask DisposeAsync() => _enumerator.DisposeAsync();

                public ValueTask<bool> MoveNextAsync() => _enumerator.MoveNextAsync();
            }
        }
    }
}
