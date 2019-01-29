// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

#if !HAS_AWAIT_FOREACH

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        // REVIEW: Once we have C# 8.0 language support, we may want to do away with these methods. An open question is how to
        //         provide support for cancellation, which could be offered through WithCancellation on the source. If we still
        //         want to keep these methods, they may be a candidate for System.Interactive.Async if we consider them to be
        //         non-standard (i.e. IEnumerable<T> doesn't have a ForEach extension method either).

        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> action, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (action == null)
                throw Error.ArgumentNull(nameof(action));

            return Core(source, action, cancellationToken);

            static async Task Core(IAsyncEnumerable<TSource> _source, Action<TSource> _action, CancellationToken _cancellationToken)
            {
#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    _action(item);
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        _action(e.Current);
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif
            }
        }

        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource, int> action, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (action == null)
                throw Error.ArgumentNull(nameof(action));

            return Core(source, action, cancellationToken);

            static async Task Core(IAsyncEnumerable<TSource> _source, Action<TSource, int> _action, CancellationToken _cancellationToken)
            {
                var index = 0;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    _action(item, checked(index++));
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        _action(e.Current, checked(index++));
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif
            }
        }

        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task> action, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (action == null)
                throw Error.ArgumentNull(nameof(action));

            return Core(source, action, cancellationToken);

            static async Task Core(IAsyncEnumerable<TSource> _source, Func<TSource, Task> _action, CancellationToken _cancellationToken)
            {
#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    await _action(item).ConfigureAwait(false);
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        await _action(e.Current).ConfigureAwait(false);
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif
            }
        }

        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, Task> action, CancellationToken cancellationToken)
        {
            return Core(source, action, cancellationToken);

            static async Task Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, Task> _action, CancellationToken _cancellationToken)
            {
#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    await _action(item, _cancellationToken).ConfigureAwait(false);
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        await _action(e.Current, _cancellationToken).ConfigureAwait(false);
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif
            }
        }

        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, Task> action, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (action == null)
                throw Error.ArgumentNull(nameof(action));

            return Core(source, action, cancellationToken);

            static async Task Core(IAsyncEnumerable<TSource> _source, Func<TSource, int, Task> _action, CancellationToken _cancellationToken)
            {
                var index = 0;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    await _action(item, checked(index++)).ConfigureAwait(false);
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        await _action(e.Current, checked(index++)).ConfigureAwait(false);
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif
            }
        }

        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, Task> action, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (action == null)
                throw Error.ArgumentNull(nameof(action));

            return Core(source, action, cancellationToken);

            static async Task Core(IAsyncEnumerable<TSource> _source, Func<TSource, int, CancellationToken, Task> _action, CancellationToken _cancellationToken)
            {
                var index = 0;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    await _action(item, checked(index++), _cancellationToken).ConfigureAwait(false);
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        await _action(e.Current, checked(index++), _cancellationToken).ConfigureAwait(false);
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif
            }
        }
    }
}

#endif
