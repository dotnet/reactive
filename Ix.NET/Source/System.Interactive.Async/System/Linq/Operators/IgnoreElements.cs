// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerableEx
    {
        public static IAsyncEnumerable<TSource> IgnoreElements<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new IgnoreElementsAsyncIterator<TSource>(source);
        }

        private sealed class IgnoreElementsAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly IAsyncEnumerable<TSource> _source;
            private IAsyncEnumerator<TSource> _enumerator;

            public IgnoreElementsAsyncIterator(IAsyncEnumerable<TSource> source)
            {
                Debug.Assert(source != null);

                _source = source;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new IgnoreElementsAsyncIterator<TSource>(_source);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(cancellationToken);
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                        }

                        break; // case
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }
    }
}
