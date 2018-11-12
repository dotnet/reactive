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
        public static IAsyncEnumerable<TSource> OnErrorResumeNext<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw Error.ArgumentNull(nameof(first));
            if (second == null)
                throw Error.ArgumentNull(nameof(second));

            return OnErrorResumeNextCore(new[] { first, second });
        }

        public static IAsyncEnumerable<TSource> OnErrorResumeNext<TSource>(params IAsyncEnumerable<TSource>[] sources)
        {
            if (sources == null)
                throw Error.ArgumentNull(nameof(sources));

            return OnErrorResumeNextCore(sources);
        }

        public static IAsyncEnumerable<TSource> OnErrorResumeNext<TSource>(this IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw Error.ArgumentNull(nameof(sources));

            return OnErrorResumeNextCore(sources);
        }

        private static IAsyncEnumerable<TSource> OnErrorResumeNextCore<TSource>(IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            return new OnErrorResumeNextAsyncIterator<TSource>(sources);
        }

        private sealed class OnErrorResumeNextAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly IEnumerable<IAsyncEnumerable<TSource>> _sources;

            private IAsyncEnumerator<TSource> _enumerator;
            private IEnumerator<IAsyncEnumerable<TSource>> _sourcesEnumerator;

            public OnErrorResumeNextAsyncIterator(IEnumerable<IAsyncEnumerable<TSource>> sources)
            {
                Debug.Assert(sources != null);

                _sources = sources;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new OnErrorResumeNextAsyncIterator<TSource>(_sources);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_sourcesEnumerator != null)
                {
                    _sourcesEnumerator.Dispose();
                    _sourcesEnumerator = null;
                }

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
                        _sourcesEnumerator = _sources.GetEnumerator();

                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (true)
                        {
                            if (_enumerator == null)
                            {
                                if (!_sourcesEnumerator.MoveNext())
                                {
                                    break; // while -- done, nothing else to do
                                }

                                _enumerator = _sourcesEnumerator.Current.GetAsyncEnumerator(cancellationToken);
                            }

                            try
                            {
                                if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    current = _enumerator.Current;
                                    return true;
                                }
                            }
                            catch
                            {
                                // Ignore
                            }

                            // Done with the current one, go to the next
                            await _enumerator.DisposeAsync().ConfigureAwait(false);
                            _enumerator = null;
                        }

                        break; // case
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }
    }
}
