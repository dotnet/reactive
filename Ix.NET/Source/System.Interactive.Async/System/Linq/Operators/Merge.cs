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
        public static IAsyncEnumerable<TSource> Merge<TSource>(params IAsyncEnumerable<TSource>[] sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return new MergeAsyncIterator<TSource>(sources);
        }

        public static IAsyncEnumerable<TSource> Merge<TSource>(this IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return sources.ToAsyncEnumerable().SelectMany(source => source);
        }

        public static IAsyncEnumerable<TSource> Merge<TSource>(this IAsyncEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return sources.SelectMany(source => source);
        }

        private sealed class MergeAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly IAsyncEnumerable<TSource>[] _sources;

            private IAsyncEnumerator<TSource>[] _enumerators;
            private ValueTask<bool>[] _moveNexts;
            private int _active;

            public MergeAsyncIterator(IAsyncEnumerable<TSource>[] sources)
            {
                Debug.Assert(sources != null);

                _sources = sources;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new MergeAsyncIterator<TSource>(_sources);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerators != null)
                {
                    var n = _enumerators.Length;

                    var disposes = new ValueTask[n];

                    for (var i = 0; i < n; i++)
                    {
                        var dispose = _enumerators[i].DisposeAsync();
                        disposes[i] = dispose;
                    }

                    await Task.WhenAll(disposes.Select(t => t.AsTask())).ConfigureAwait(false);
                    _enumerators = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        var n = _sources.Length;

                        _enumerators = new IAsyncEnumerator<TSource>[n];
                        _moveNexts = new ValueTask<bool>[n];
                        _active = n;

                        for (var i = 0; i < n; i++)
                        {
                            var enumerator = _sources[i].GetAsyncEnumerator(cancellationToken);
                            _enumerators[i] = enumerator;
                            _moveNexts[i] = enumerator.MoveNextAsync();
                        }

                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (_active > 0)
                        {
                            //
                            // REVIEW: This approach does have a bias towards giving sources on the left
                            //         priority over sources on the right when yielding values. We may
                            //         want to consider a "prefer fairness" option.
                            //

                            var moveNext = await Task.WhenAny(_moveNexts.Select(t => t.AsTask())).ConfigureAwait(false);

                            var index = Array.IndexOf(_moveNexts, moveNext);

                            if (!await moveNext.ConfigureAwait(false))
                            {
                                _moveNexts[index] = TaskExt.Never;
                                _active--;
                            }
                            else
                            {
                                var enumerator = _enumerators[index];
                                current = enumerator.Current;
                                _moveNexts[index] = enumerator.MoveNextAsync();
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
