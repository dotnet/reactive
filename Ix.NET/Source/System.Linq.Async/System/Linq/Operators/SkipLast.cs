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
        public static IAsyncEnumerable<TSource> SkipLast<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            if (count <= 0)
            {
                // Return source if not actually skipping, but only if it's a type from here, to avoid
                // issues if collections are used as keys or otherwise must not be aliased.
                if (source is AsyncIterator<TSource>)
                {
                    return source;
                }

                count = 0;
            }

            return new SkipLastAsyncIterator<TSource>(source, count);
        }

        private sealed class SkipLastAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly int _count;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource> _enumerator;
            private Queue<TSource> _queue;

            public SkipLastAsyncIterator(IAsyncEnumerable<TSource> source, int count)
            {
                Debug.Assert(source != null);

                _source = source;
                _count = count;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new SkipLastAsyncIterator<TSource>(_source, _count);
            }

            public override async ValueTask DisposeAsync()
            {
                try
                {
                    if (_enumerator != null)
                    {
                        await _enumerator.DisposeAsync().ConfigureAwait(false);
                    }
                }
                finally
                {
                    _enumerator = null;
                    _queue = null; // release the memory

                    await base.DisposeAsync().ConfigureAwait(false);
                }
            }


            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(cancellationToken);
                        _queue = new Queue<TSource>();

                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;


                    case AsyncIteratorState.Iterating:
                        while (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var item = _enumerator.Current;
                            _queue.Enqueue(item);

                            if (_queue.Count > _count)
                            {
                                current = _queue.Dequeue();
                                return true;
                            }
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }
    }
}
