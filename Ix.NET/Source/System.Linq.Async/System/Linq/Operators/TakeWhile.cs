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
        public static IAsyncEnumerable<TSource> TakeWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    if (!predicate(element))
                    {
                        break;
                    }

                    yield return element;
                }
            }
        }

        public static IAsyncEnumerable<TSource> TakeWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                var index = -1;

                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    checked
                    {
                        index++;
                    }

                    if (!predicate(element, index))
                    {
                        break;
                    }

                    yield return element;
                }
            }
        }

        internal static IAsyncEnumerable<TSource> TakeWhileAwaitCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    if (!await predicate(element).ConfigureAwait(false))
                    {
                        break;
                    }

                    yield return element;
                }
            }
        }

#if !NO_DEEP_CANCELLATION
        internal static IAsyncEnumerable<TSource> TakeWhileAwaitWithCancellationCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    if (!await predicate(element, cancellationToken).ConfigureAwait(false))
                    {
                        break;
                    }

                    yield return element;
                }
            }
        }
#endif

        internal static IAsyncEnumerable<TSource> TakeWhileAwaitCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<bool>> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                var index = -1;

                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    checked
                    {
                        index++;
                    }

                    if (!await predicate(element, index).ConfigureAwait(false))
                    {
                        break;
                    }

                    yield return element;
                }
            }
        }

#if !NO_DEEP_CANCELLATION
        internal static IAsyncEnumerable<TSource> TakeWhileAwaitWithCancellationCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, ValueTask<bool>> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                var index = -1;

                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    checked
                    {
                        index++;
                    }

                    if (!await predicate(element, index, cancellationToken).ConfigureAwait(false))
                    {
                        break;
                    }

                    yield return element;
                }
            }
        }
#endif
    }
}
