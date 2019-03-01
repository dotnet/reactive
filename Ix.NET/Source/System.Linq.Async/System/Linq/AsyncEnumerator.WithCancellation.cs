// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    public static partial class AsyncEnumerator
    {
        /// <summary>
        /// Wraps the specified enumerator with an enumerator that checks for cancellation upon every invocation
        /// of the <see cref="IAsyncEnumerator{T}.MoveNextAsync"/> method.
        /// </summary>
        /// <typeparam name="T">The type of the elements returned by the enumerator.</typeparam>
        /// <param name="source">The enumerator to augment with cancellation support.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>An enumerator that honors cancellation requests.</returns>
        public static IAsyncEnumerator<T> WithCancellation<T>(this IAsyncEnumerator<T> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            if (cancellationToken == default)
                return source;

            return new WithCancellationAsyncEnumerator<T>(source, cancellationToken);
        }

        private sealed class WithCancellationAsyncEnumerator<T> : IAsyncEnumerator<T>
        {
            private readonly IAsyncEnumerator<T> _source;
            private readonly CancellationToken _cancellationToken;

            public WithCancellationAsyncEnumerator(IAsyncEnumerator<T> source, CancellationToken cancellationToken)
            {
                _source = source;
                _cancellationToken = cancellationToken;
            }

            public T Current => _source.Current;

            public ValueTask DisposeAsync() => _source.DisposeAsync();

            public ValueTask<bool> MoveNextAsync()
            {
                _cancellationToken.ThrowIfCancellationRequested();

                return _source.MoveNextAsync(); // REVIEW: Signal cancellation through task or synchronously?
            }
        }
    }
}
