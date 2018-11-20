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
        public static IAsyncEnumerable<T> CreateEnumerable<T>(Func<CancellationToken, IAsyncEnumerator<T>> getEnumerator)
        {
            if (getEnumerator == null)
                throw Error.ArgumentNull(nameof(getEnumerator));

            return new AnonymousAsyncEnumerable<T>(getEnumerator);
        }

        public static WithCancellationAsyncEnumerable<T> WithCancellation<T>(this IAsyncEnumerable<T> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return new WithCancellationAsyncEnumerable<T>(source, cancellationToken);
        }

        private sealed class AnonymousAsyncEnumerable<T> : IAsyncEnumerable<T>
        {
            private readonly Func<CancellationToken, IAsyncEnumerator<T>> _getEnumerator;

            public AnonymousAsyncEnumerable(Func<CancellationToken, IAsyncEnumerator<T>> getEnumerator)
            {
                Debug.Assert(getEnumerator != null);

                _getEnumerator = getEnumerator;
            }

            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken) => _getEnumerator(cancellationToken);
        }

        // REVIEW: Explicit implementation of the interfaces allows for composition with other "modifier operators" such as ConfigureAwait.
        //         We expect that the "await foreach" statement will bind to the public struct methods, thus avoiding boxing.

        public readonly struct WithCancellationAsyncEnumerable<T> : IAsyncEnumerable<T>
        {
            private readonly IAsyncEnumerable<T> _source;
            private readonly CancellationToken _cancellationToken;

            public WithCancellationAsyncEnumerable(IAsyncEnumerable<T> source, CancellationToken cancellationToken)
            {
                _source = source;
                _cancellationToken = cancellationToken;
            }

            // REVIEW: Should we simply ignore the second cancellation token or should we link the two?

            public WithCancellationAsyncEnumerator GetAsyncEnumerator(CancellationToken cancellationToken)
                => new WithCancellationAsyncEnumerator(_source.GetAsyncEnumerator(_cancellationToken));

            IAsyncEnumerator<T> IAsyncEnumerable<T>.GetAsyncEnumerator(CancellationToken cancellationToken)
                => GetAsyncEnumerator(cancellationToken);

            public readonly struct WithCancellationAsyncEnumerator : IAsyncEnumerator<T>
            {
                private readonly IAsyncEnumerator<T> _enumerator;

                public WithCancellationAsyncEnumerator(IAsyncEnumerator<T> enumerator)
                {
                    _enumerator = enumerator;
                }

                public T Current => _enumerator.Current;

                public ValueTask DisposeAsync() => _enumerator.DisposeAsync();

                public ValueTask<bool> MoveNextAsync() => _enumerator.MoveNextAsync();
            }
        }
    }
}
