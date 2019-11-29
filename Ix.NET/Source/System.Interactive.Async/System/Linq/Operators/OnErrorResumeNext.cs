// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerableEx
    {
        /// <summary>
        /// Concatenates the second async-enumerable sequence to the first async-enumerable sequence upon successful or exceptional termination of the first.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="first">First async-enumerable sequence whose exception (if any) is caught.</param>
        /// <param name="second">Second async-enumerable sequence used to produce results after the first sequence terminates.</param>
        /// <returns>An async-enumerable sequence that concatenates the first and second sequence, even if the first sequence terminates exceptionally.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> is null.</exception>
        public static IAsyncEnumerable<TSource> OnErrorResumeNext<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw Error.ArgumentNull(nameof(first));
            if (second == null)
                throw Error.ArgumentNull(nameof(second));

            return OnErrorResumeNextCore(new[] { first, second });
        }

        /// <summary>
        /// Concatenates all of the specified async-enumerable sequences, even if the previous async-enumerable sequence terminated exceptionally.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Observable sequences to concatenate.</param>
        /// <returns>An async-enumerable sequence that concatenates the source sequences, even if a sequence terminates exceptionally.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        public static IAsyncEnumerable<TSource> OnErrorResumeNext<TSource>(params IAsyncEnumerable<TSource>[] sources)
        {
            if (sources == null)
                throw Error.ArgumentNull(nameof(sources));

            return OnErrorResumeNextCore(sources);
        }

        /// <summary>
        /// Concatenates all async-enumerable sequences in the given enumerable sequence, even if the previous async-enumerable sequence terminated exceptionally.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Observable sequences to concatenate.</param>
        /// <returns>An async-enumerable sequence that concatenates the source sequences, even if a sequence terminates exceptionally.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
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

            private IAsyncEnumerator<TSource>? _enumerator;
            private IEnumerator<IAsyncEnumerable<TSource>>? _sourcesEnumerator;

            public OnErrorResumeNextAsyncIterator(IEnumerable<IAsyncEnumerable<TSource>> sources)
            {
                _sources = sources;
            }

            public override AsyncIteratorBase<TSource> Clone()
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

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _sourcesEnumerator = _sources.GetEnumerator();

                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (true)
                        {
                            if (_enumerator == null)
                            {
                                if (!_sourcesEnumerator!.MoveNext())
                                {
                                    break; // while -- done, nothing else to do
                                }

                                _enumerator = _sourcesEnumerator.Current.GetAsyncEnumerator(_cancellationToken);
                            }

                            try
                            {
                                if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    _current = _enumerator.Current;
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
