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
#if HAS_VALUETUPLE
        public static IAsyncEnumerable<(TFirst First, TSecond Second)> Zip<TFirst, TSecond>(this IAsyncEnumerable<TFirst> first, IAsyncEnumerable<TSecond> second)
        {
            if (first == null)
                throw Error.ArgumentNull(nameof(first));
            if (second == null)
                throw Error.ArgumentNull(nameof(second));

            return Create(Core);

            async IAsyncEnumerator<(TFirst, TSecond)> Core(CancellationToken cancellationToken)
            {
                await using (var e1 = first.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    await using (var e2 = second.GetConfiguredAsyncEnumerator(cancellationToken, false))
                    {
                        while (await e1.MoveNextAsync() && await e2.MoveNextAsync())
                        {
                            yield return (e1.Current, e2.Current);
                        }
                    }
                }
            }
        }
#endif

        public static IAsyncEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IAsyncEnumerable<TFirst> first, IAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> selector)
        {
            if (first == null)
                throw Error.ArgumentNull(nameof(first));
            if (second == null)
                throw Error.ArgumentNull(nameof(second));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Create(Core);

            async IAsyncEnumerator<TResult> Core(CancellationToken cancellationToken)
            {
                await using (var e1 = first.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    await using (var e2 = second.GetConfiguredAsyncEnumerator(cancellationToken, false))
                    {
                        while (await e1.MoveNextAsync() && await e2.MoveNextAsync())
                        {
                            yield return selector(e1.Current, e2.Current);
                        }
                    }
                }
            }
        }

        internal static IAsyncEnumerable<TResult> ZipAwaitCore<TFirst, TSecond, TResult>(this IAsyncEnumerable<TFirst> first, IAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, ValueTask<TResult>> selector)
        {
            if (first == null)
                throw Error.ArgumentNull(nameof(first));
            if (second == null)
                throw Error.ArgumentNull(nameof(second));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Create(Core);

            async IAsyncEnumerator<TResult> Core(CancellationToken cancellationToken)
            {
                await using (var e1 = first.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    await using (var e2 = second.GetConfiguredAsyncEnumerator(cancellationToken, false))
                    {
                        while (await e1.MoveNextAsync() && await e2.MoveNextAsync())
                        {
                            yield return await selector(e1.Current, e2.Current).ConfigureAwait(false);
                        }
                    }
                }
            }
        }

#if !NO_DEEP_CANCELLATION
        internal static IAsyncEnumerable<TResult> ZipAwaitWithCancellationCore<TFirst, TSecond, TResult>(this IAsyncEnumerable<TFirst> first, IAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, CancellationToken, ValueTask<TResult>> selector)
        {
            if (first == null)
                throw Error.ArgumentNull(nameof(first));
            if (second == null)
                throw Error.ArgumentNull(nameof(second));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Create(Core);

            async IAsyncEnumerator<TResult> Core(CancellationToken cancellationToken)
            {
                await using (var e1 = first.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    await using (var e2 = second.GetConfiguredAsyncEnumerator(cancellationToken, false))
                    {
                        while (await e1.MoveNextAsync() && await e2.MoveNextAsync())
                        {
                            yield return await selector(e1.Current, e2.Current, cancellationToken).ConfigureAwait(false);
                        }
                    }
                }
            }
        }
#endif
    }
}
