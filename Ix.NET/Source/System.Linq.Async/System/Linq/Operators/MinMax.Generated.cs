// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static ValueTask<int> MaxAsync(this IAsyncEnumerable<int> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<int> Core(IAsyncEnumerable<int> _source, CancellationToken _cancellationToken)
            {
                int value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = e.Current;

                    while (await e.MoveNextAsync())
                    {
                        var x = e.Current;
                        if (x > value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<int> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<int> Core(IAsyncEnumerable<TSource> _source, Func<TSource, int> _selector, CancellationToken _cancellationToken)
            {
                int value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = _selector(e.Current);

                    while (await e.MoveNextAsync())
                    {
                        var x = _selector(e.Current);
                        if (x > value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<int> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<int> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<int>> _selector, CancellationToken _cancellationToken)
            {
                int value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = await _selector(e.Current).ConfigureAwait(false);

                    while (await e.MoveNextAsync())
                    {
                        var x = await _selector(e.Current).ConfigureAwait(false);
                        if (x > value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<int> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<int>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<int> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<int>> _selector, CancellationToken _cancellationToken)
            {
                int value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);

                    while (await e.MoveNextAsync())
                    {
                        var x = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                        if (x > value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#endif
        public static ValueTask<int?> MaxAsync(this IAsyncEnumerable<int?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<int?> Core(IAsyncEnumerable<int?> _source, CancellationToken _cancellationToken)
            {
                int? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = e.Current;
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    if (valueVal >= 0)
                    {
                        // We can fast-path this case where we know HasValue will
                        // never affect the outcome, without constantly checking
                        // if we're in such a state. Similar fast-paths could
                        // be done for other cases, but as all-positive or mostly-
                        // positive integer values are quite common in real-world
                        // uses, it's only been done for int? and long?.

                        while (await e.MoveNextAsync())
                        {
                            var cur = e.Current;
                            var x = cur.GetValueOrDefault();

                            if (x > valueVal)
                            {
                                valueVal = x;
                                value = cur;
                            }
                        }
                    }
                    else
                    {
                        while (await e.MoveNextAsync())
                        {
                            var cur = e.Current;
                            var x = cur.GetValueOrDefault();

                            // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                            // unless nulls either never happen or always happen.
                            if (cur.HasValue & x > valueVal)
                            {
                                valueVal = x;
                                value = cur;
                            }
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<int?> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<int?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, int?> _selector, CancellationToken _cancellationToken)
            {
                int? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = _selector(e.Current);
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    if (valueVal >= 0)
                    {
                        // We can fast-path this case where we know HasValue will
                        // never affect the outcome, without constantly checking
                        // if we're in such a state. Similar fast-paths could
                        // be done for other cases, but as all-positive or mostly-
                        // positive integer values are quite common in real-world
                        // uses, it's only been done for int? and long?.

                        while (await e.MoveNextAsync())
                        {
                            var cur = _selector(e.Current);
                            var x = cur.GetValueOrDefault();

                            if (x > valueVal)
                            {
                                valueVal = x;
                                value = cur;
                            }
                        }
                    }
                    else
                    {
                        while (await e.MoveNextAsync())
                        {
                            var cur = _selector(e.Current);
                            var x = cur.GetValueOrDefault();

                            // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                            // unless nulls either never happen or always happen.
                            if (cur.HasValue & x > valueVal)
                            {
                                valueVal = x;
                                value = cur;
                            }
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<int?> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<int?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<int?>> _selector, CancellationToken _cancellationToken)
            {
                int? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = await _selector(e.Current).ConfigureAwait(false);
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    if (valueVal >= 0)
                    {
                        // We can fast-path this case where we know HasValue will
                        // never affect the outcome, without constantly checking
                        // if we're in such a state. Similar fast-paths could
                        // be done for other cases, but as all-positive or mostly-
                        // positive integer values are quite common in real-world
                        // uses, it's only been done for int? and long?.

                        while (await e.MoveNextAsync())
                        {
                            var cur = await _selector(e.Current).ConfigureAwait(false);
                            var x = cur.GetValueOrDefault();

                            if (x > valueVal)
                            {
                                valueVal = x;
                                value = cur;
                            }
                        }
                    }
                    else
                    {
                        while (await e.MoveNextAsync())
                        {
                            var cur = await _selector(e.Current).ConfigureAwait(false);
                            var x = cur.GetValueOrDefault();

                            // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                            // unless nulls either never happen or always happen.
                            if (cur.HasValue & x > valueVal)
                            {
                                valueVal = x;
                                value = cur;
                            }
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<int?> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<int?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<int?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<int?>> _selector, CancellationToken _cancellationToken)
            {
                int? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    if (valueVal >= 0)
                    {
                        // We can fast-path this case where we know HasValue will
                        // never affect the outcome, without constantly checking
                        // if we're in such a state. Similar fast-paths could
                        // be done for other cases, but as all-positive or mostly-
                        // positive integer values are quite common in real-world
                        // uses, it's only been done for int? and long?.

                        while (await e.MoveNextAsync())
                        {
                            var cur = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                            var x = cur.GetValueOrDefault();

                            if (x > valueVal)
                            {
                                valueVal = x;
                                value = cur;
                            }
                        }
                    }
                    else
                    {
                        while (await e.MoveNextAsync())
                        {
                            var cur = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                            var x = cur.GetValueOrDefault();

                            // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                            // unless nulls either never happen or always happen.
                            if (cur.HasValue & x > valueVal)
                            {
                                valueVal = x;
                                value = cur;
                            }
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#endif
        public static ValueTask<long> MaxAsync(this IAsyncEnumerable<long> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<long> Core(IAsyncEnumerable<long> _source, CancellationToken _cancellationToken)
            {
                long value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = e.Current;

                    while (await e.MoveNextAsync())
                    {
                        var x = e.Current;
                        if (x > value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<long> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<long> Core(IAsyncEnumerable<TSource> _source, Func<TSource, long> _selector, CancellationToken _cancellationToken)
            {
                long value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = _selector(e.Current);

                    while (await e.MoveNextAsync())
                    {
                        var x = _selector(e.Current);
                        if (x > value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<long> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<long> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<long>> _selector, CancellationToken _cancellationToken)
            {
                long value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = await _selector(e.Current).ConfigureAwait(false);

                    while (await e.MoveNextAsync())
                    {
                        var x = await _selector(e.Current).ConfigureAwait(false);
                        if (x > value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<long> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<long>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<long> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<long>> _selector, CancellationToken _cancellationToken)
            {
                long value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);

                    while (await e.MoveNextAsync())
                    {
                        var x = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                        if (x > value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#endif
        public static ValueTask<long?> MaxAsync(this IAsyncEnumerable<long?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<long?> Core(IAsyncEnumerable<long?> _source, CancellationToken _cancellationToken)
            {
                long? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = e.Current;
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    if (valueVal >= 0)
                    {
                        // We can fast-path this case where we know HasValue will
                        // never affect the outcome, without constantly checking
                        // if we're in such a state. Similar fast-paths could
                        // be done for other cases, but as all-positive or mostly-
                        // positive integer values are quite common in real-world
                        // uses, it's only been done for int? and long?.

                        while (await e.MoveNextAsync())
                        {
                            var cur = e.Current;
                            var x = cur.GetValueOrDefault();

                            if (x > valueVal)
                            {
                                valueVal = x;
                                value = cur;
                            }
                        }
                    }
                    else
                    {
                        while (await e.MoveNextAsync())
                        {
                            var cur = e.Current;
                            var x = cur.GetValueOrDefault();

                            // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                            // unless nulls either never happen or always happen.
                            if (cur.HasValue & x > valueVal)
                            {
                                valueVal = x;
                                value = cur;
                            }
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<long?> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<long?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, long?> _selector, CancellationToken _cancellationToken)
            {
                long? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = _selector(e.Current);
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    if (valueVal >= 0)
                    {
                        // We can fast-path this case where we know HasValue will
                        // never affect the outcome, without constantly checking
                        // if we're in such a state. Similar fast-paths could
                        // be done for other cases, but as all-positive or mostly-
                        // positive integer values are quite common in real-world
                        // uses, it's only been done for int? and long?.

                        while (await e.MoveNextAsync())
                        {
                            var cur = _selector(e.Current);
                            var x = cur.GetValueOrDefault();

                            if (x > valueVal)
                            {
                                valueVal = x;
                                value = cur;
                            }
                        }
                    }
                    else
                    {
                        while (await e.MoveNextAsync())
                        {
                            var cur = _selector(e.Current);
                            var x = cur.GetValueOrDefault();

                            // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                            // unless nulls either never happen or always happen.
                            if (cur.HasValue & x > valueVal)
                            {
                                valueVal = x;
                                value = cur;
                            }
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<long?> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<long?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<long?>> _selector, CancellationToken _cancellationToken)
            {
                long? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = await _selector(e.Current).ConfigureAwait(false);
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    if (valueVal >= 0)
                    {
                        // We can fast-path this case where we know HasValue will
                        // never affect the outcome, without constantly checking
                        // if we're in such a state. Similar fast-paths could
                        // be done for other cases, but as all-positive or mostly-
                        // positive integer values are quite common in real-world
                        // uses, it's only been done for int? and long?.

                        while (await e.MoveNextAsync())
                        {
                            var cur = await _selector(e.Current).ConfigureAwait(false);
                            var x = cur.GetValueOrDefault();

                            if (x > valueVal)
                            {
                                valueVal = x;
                                value = cur;
                            }
                        }
                    }
                    else
                    {
                        while (await e.MoveNextAsync())
                        {
                            var cur = await _selector(e.Current).ConfigureAwait(false);
                            var x = cur.GetValueOrDefault();

                            // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                            // unless nulls either never happen or always happen.
                            if (cur.HasValue & x > valueVal)
                            {
                                valueVal = x;
                                value = cur;
                            }
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<long?> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<long?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<long?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<long?>> _selector, CancellationToken _cancellationToken)
            {
                long? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    if (valueVal >= 0)
                    {
                        // We can fast-path this case where we know HasValue will
                        // never affect the outcome, without constantly checking
                        // if we're in such a state. Similar fast-paths could
                        // be done for other cases, but as all-positive or mostly-
                        // positive integer values are quite common in real-world
                        // uses, it's only been done for int? and long?.

                        while (await e.MoveNextAsync())
                        {
                            var cur = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                            var x = cur.GetValueOrDefault();

                            if (x > valueVal)
                            {
                                valueVal = x;
                                value = cur;
                            }
                        }
                    }
                    else
                    {
                        while (await e.MoveNextAsync())
                        {
                            var cur = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                            var x = cur.GetValueOrDefault();

                            // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                            // unless nulls either never happen or always happen.
                            if (cur.HasValue & x > valueVal)
                            {
                                valueVal = x;
                                value = cur;
                            }
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#endif
        public static ValueTask<float> MaxAsync(this IAsyncEnumerable<float> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<float> Core(IAsyncEnumerable<float> _source, CancellationToken _cancellationToken)
            {
                float value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = e.Current;

                    // NaN is ordered less than all other values. We need to do explicit checks
                    // to ensure this, but once we've found a value that is not NaN we need no
                    // longer worry about it, so first loop until such a value is found (or not,
                    // as the case may be).

                    while (float.IsNaN(value))
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = e.Current;
                    }

                    while (await e.MoveNextAsync())
                    {
                        var x = e.Current;
                        if (x > value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<float> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<float> Core(IAsyncEnumerable<TSource> _source, Func<TSource, float> _selector, CancellationToken _cancellationToken)
            {
                float value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = _selector(e.Current);

                    // NaN is ordered less than all other values. We need to do explicit checks
                    // to ensure this, but once we've found a value that is not NaN we need no
                    // longer worry about it, so first loop until such a value is found (or not,
                    // as the case may be).

                    while (float.IsNaN(value))
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = _selector(e.Current);
                    }

                    while (await e.MoveNextAsync())
                    {
                        var x = _selector(e.Current);
                        if (x > value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<float> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<float> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<float>> _selector, CancellationToken _cancellationToken)
            {
                float value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = await _selector(e.Current).ConfigureAwait(false);

                    // NaN is ordered less than all other values. We need to do explicit checks
                    // to ensure this, but once we've found a value that is not NaN we need no
                    // longer worry about it, so first loop until such a value is found (or not,
                    // as the case may be).

                    while (float.IsNaN(value))
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = await _selector(e.Current).ConfigureAwait(false);
                    }

                    while (await e.MoveNextAsync())
                    {
                        var x = await _selector(e.Current).ConfigureAwait(false);
                        if (x > value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<float> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<float>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<float> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<float>> _selector, CancellationToken _cancellationToken)
            {
                float value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);

                    // NaN is ordered less than all other values. We need to do explicit checks
                    // to ensure this, but once we've found a value that is not NaN we need no
                    // longer worry about it, so first loop until such a value is found (or not,
                    // as the case may be).

                    while (float.IsNaN(value))
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                    }

                    while (await e.MoveNextAsync())
                    {
                        var x = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                        if (x > value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#endif
        public static ValueTask<float?> MaxAsync(this IAsyncEnumerable<float?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<float?> Core(IAsyncEnumerable<float?> _source, CancellationToken _cancellationToken)
            {
                float? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = e.Current;
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    // NaN is ordered less than all other values. We need to do explicit checks
                    // to ensure this, but once we've found a value that is not NaN we need no
                    // longer worry about it, so first loop until such a value is found (or not,
                    // as the case may be).

                    while (float.IsNaN(valueVal))
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        var cur = e.Current;

                        if (cur.HasValue)
                        {
                            valueVal = (value = cur).GetValueOrDefault();
                        }
                    }

                    while (await e.MoveNextAsync())
                    {
                        var cur = e.Current;
                        var x = cur.GetValueOrDefault();

                        // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                        // unless nulls either never happen or always happen.
                        if (cur.HasValue & x > valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<float?> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<float?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, float?> _selector, CancellationToken _cancellationToken)
            {
                float? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = _selector(e.Current);
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    // NaN is ordered less than all other values. We need to do explicit checks
                    // to ensure this, but once we've found a value that is not NaN we need no
                    // longer worry about it, so first loop until such a value is found (or not,
                    // as the case may be).

                    while (float.IsNaN(valueVal))
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        var cur = _selector(e.Current);

                        if (cur.HasValue)
                        {
                            valueVal = (value = cur).GetValueOrDefault();
                        }
                    }

                    while (await e.MoveNextAsync())
                    {
                        var cur = _selector(e.Current);
                        var x = cur.GetValueOrDefault();

                        // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                        // unless nulls either never happen or always happen.
                        if (cur.HasValue & x > valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<float?> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<float?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<float?>> _selector, CancellationToken _cancellationToken)
            {
                float? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = await _selector(e.Current).ConfigureAwait(false);
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    // NaN is ordered less than all other values. We need to do explicit checks
                    // to ensure this, but once we've found a value that is not NaN we need no
                    // longer worry about it, so first loop until such a value is found (or not,
                    // as the case may be).

                    while (float.IsNaN(valueVal))
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        var cur = await _selector(e.Current).ConfigureAwait(false);

                        if (cur.HasValue)
                        {
                            valueVal = (value = cur).GetValueOrDefault();
                        }
                    }

                    while (await e.MoveNextAsync())
                    {
                        var cur = await _selector(e.Current).ConfigureAwait(false);
                        var x = cur.GetValueOrDefault();

                        // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                        // unless nulls either never happen or always happen.
                        if (cur.HasValue & x > valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<float?> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<float?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<float?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<float?>> _selector, CancellationToken _cancellationToken)
            {
                float? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    // NaN is ordered less than all other values. We need to do explicit checks
                    // to ensure this, but once we've found a value that is not NaN we need no
                    // longer worry about it, so first loop until such a value is found (or not,
                    // as the case may be).

                    while (float.IsNaN(valueVal))
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        var cur = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);

                        if (cur.HasValue)
                        {
                            valueVal = (value = cur).GetValueOrDefault();
                        }
                    }

                    while (await e.MoveNextAsync())
                    {
                        var cur = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                        var x = cur.GetValueOrDefault();

                        // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                        // unless nulls either never happen or always happen.
                        if (cur.HasValue & x > valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#endif
        public static ValueTask<double> MaxAsync(this IAsyncEnumerable<double> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<double> Core(IAsyncEnumerable<double> _source, CancellationToken _cancellationToken)
            {
                double value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = e.Current;

                    // NaN is ordered less than all other values. We need to do explicit checks
                    // to ensure this, but once we've found a value that is not NaN we need no
                    // longer worry about it, so first loop until such a value is found (or not,
                    // as the case may be).

                    while (double.IsNaN(value))
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = e.Current;
                    }

                    while (await e.MoveNextAsync())
                    {
                        var x = e.Current;
                        if (x > value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<double> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double> Core(IAsyncEnumerable<TSource> _source, Func<TSource, double> _selector, CancellationToken _cancellationToken)
            {
                double value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = _selector(e.Current);

                    // NaN is ordered less than all other values. We need to do explicit checks
                    // to ensure this, but once we've found a value that is not NaN we need no
                    // longer worry about it, so first loop until such a value is found (or not,
                    // as the case may be).

                    while (double.IsNaN(value))
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = _selector(e.Current);
                    }

                    while (await e.MoveNextAsync())
                    {
                        var x = _selector(e.Current);
                        if (x > value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<double> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<double>> _selector, CancellationToken _cancellationToken)
            {
                double value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = await _selector(e.Current).ConfigureAwait(false);

                    // NaN is ordered less than all other values. We need to do explicit checks
                    // to ensure this, but once we've found a value that is not NaN we need no
                    // longer worry about it, so first loop until such a value is found (or not,
                    // as the case may be).

                    while (double.IsNaN(value))
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = await _selector(e.Current).ConfigureAwait(false);
                    }

                    while (await e.MoveNextAsync())
                    {
                        var x = await _selector(e.Current).ConfigureAwait(false);
                        if (x > value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<double> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<double>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<double>> _selector, CancellationToken _cancellationToken)
            {
                double value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);

                    // NaN is ordered less than all other values. We need to do explicit checks
                    // to ensure this, but once we've found a value that is not NaN we need no
                    // longer worry about it, so first loop until such a value is found (or not,
                    // as the case may be).

                    while (double.IsNaN(value))
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                    }

                    while (await e.MoveNextAsync())
                    {
                        var x = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                        if (x > value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#endif
        public static ValueTask<double?> MaxAsync(this IAsyncEnumerable<double?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<double?> Core(IAsyncEnumerable<double?> _source, CancellationToken _cancellationToken)
            {
                double? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = e.Current;
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    // NaN is ordered less than all other values. We need to do explicit checks
                    // to ensure this, but once we've found a value that is not NaN we need no
                    // longer worry about it, so first loop until such a value is found (or not,
                    // as the case may be).

                    while (double.IsNaN(valueVal))
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        var cur = e.Current;

                        if (cur.HasValue)
                        {
                            valueVal = (value = cur).GetValueOrDefault();
                        }
                    }

                    while (await e.MoveNextAsync())
                    {
                        var cur = e.Current;
                        var x = cur.GetValueOrDefault();

                        // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                        // unless nulls either never happen or always happen.
                        if (cur.HasValue & x > valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<double?> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, double?> _selector, CancellationToken _cancellationToken)
            {
                double? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = _selector(e.Current);
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    // NaN is ordered less than all other values. We need to do explicit checks
                    // to ensure this, but once we've found a value that is not NaN we need no
                    // longer worry about it, so first loop until such a value is found (or not,
                    // as the case may be).

                    while (double.IsNaN(valueVal))
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        var cur = _selector(e.Current);

                        if (cur.HasValue)
                        {
                            valueVal = (value = cur).GetValueOrDefault();
                        }
                    }

                    while (await e.MoveNextAsync())
                    {
                        var cur = _selector(e.Current);
                        var x = cur.GetValueOrDefault();

                        // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                        // unless nulls either never happen or always happen.
                        if (cur.HasValue & x > valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<double?> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<double?>> _selector, CancellationToken _cancellationToken)
            {
                double? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = await _selector(e.Current).ConfigureAwait(false);
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    // NaN is ordered less than all other values. We need to do explicit checks
                    // to ensure this, but once we've found a value that is not NaN we need no
                    // longer worry about it, so first loop until such a value is found (or not,
                    // as the case may be).

                    while (double.IsNaN(valueVal))
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        var cur = await _selector(e.Current).ConfigureAwait(false);

                        if (cur.HasValue)
                        {
                            valueVal = (value = cur).GetValueOrDefault();
                        }
                    }

                    while (await e.MoveNextAsync())
                    {
                        var cur = await _selector(e.Current).ConfigureAwait(false);
                        var x = cur.GetValueOrDefault();

                        // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                        // unless nulls either never happen or always happen.
                        if (cur.HasValue & x > valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<double?> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<double?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<double?>> _selector, CancellationToken _cancellationToken)
            {
                double? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    // NaN is ordered less than all other values. We need to do explicit checks
                    // to ensure this, but once we've found a value that is not NaN we need no
                    // longer worry about it, so first loop until such a value is found (or not,
                    // as the case may be).

                    while (double.IsNaN(valueVal))
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        var cur = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);

                        if (cur.HasValue)
                        {
                            valueVal = (value = cur).GetValueOrDefault();
                        }
                    }

                    while (await e.MoveNextAsync())
                    {
                        var cur = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                        var x = cur.GetValueOrDefault();

                        // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                        // unless nulls either never happen or always happen.
                        if (cur.HasValue & x > valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#endif
        public static ValueTask<decimal> MaxAsync(this IAsyncEnumerable<decimal> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<decimal> Core(IAsyncEnumerable<decimal> _source, CancellationToken _cancellationToken)
            {
                decimal value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = e.Current;

                    while (await e.MoveNextAsync())
                    {
                        var x = e.Current;
                        if (x > value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<decimal> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<decimal> Core(IAsyncEnumerable<TSource> _source, Func<TSource, decimal> _selector, CancellationToken _cancellationToken)
            {
                decimal value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = _selector(e.Current);

                    while (await e.MoveNextAsync())
                    {
                        var x = _selector(e.Current);
                        if (x > value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<decimal> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<decimal> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<decimal>> _selector, CancellationToken _cancellationToken)
            {
                decimal value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = await _selector(e.Current).ConfigureAwait(false);

                    while (await e.MoveNextAsync())
                    {
                        var x = await _selector(e.Current).ConfigureAwait(false);
                        if (x > value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<decimal> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<decimal>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<decimal> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<decimal>> _selector, CancellationToken _cancellationToken)
            {
                decimal value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);

                    while (await e.MoveNextAsync())
                    {
                        var x = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                        if (x > value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#endif
        public static ValueTask<decimal?> MaxAsync(this IAsyncEnumerable<decimal?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<decimal?> Core(IAsyncEnumerable<decimal?> _source, CancellationToken _cancellationToken)
            {
                decimal? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = e.Current;
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    while (await e.MoveNextAsync())
                    {
                        var cur = e.Current;
                        var x = cur.GetValueOrDefault();

                        if (cur.HasValue && x > valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<decimal?> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<decimal?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, decimal?> _selector, CancellationToken _cancellationToken)
            {
                decimal? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = _selector(e.Current);
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    while (await e.MoveNextAsync())
                    {
                        var cur = _selector(e.Current);
                        var x = cur.GetValueOrDefault();

                        if (cur.HasValue && x > valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<decimal?> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<decimal?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<decimal?>> _selector, CancellationToken _cancellationToken)
            {
                decimal? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = await _selector(e.Current).ConfigureAwait(false);
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    while (await e.MoveNextAsync())
                    {
                        var cur = await _selector(e.Current).ConfigureAwait(false);
                        var x = cur.GetValueOrDefault();

                        if (cur.HasValue && x > valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<decimal?> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<decimal?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<decimal?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<decimal?>> _selector, CancellationToken _cancellationToken)
            {
                decimal? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    while (await e.MoveNextAsync())
                    {
                        var cur = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                        var x = cur.GetValueOrDefault();

                        if (cur.HasValue && x > valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#endif
        public static ValueTask<int> MinAsync(this IAsyncEnumerable<int> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<int> Core(IAsyncEnumerable<int> _source, CancellationToken _cancellationToken)
            {
                int value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = e.Current;

                    while (await e.MoveNextAsync())
                    {
                        var x = e.Current;
                        if (x < value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<int> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<int> Core(IAsyncEnumerable<TSource> _source, Func<TSource, int> _selector, CancellationToken _cancellationToken)
            {
                int value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = _selector(e.Current);

                    while (await e.MoveNextAsync())
                    {
                        var x = _selector(e.Current);
                        if (x < value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<int> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<int> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<int>> _selector, CancellationToken _cancellationToken)
            {
                int value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = await _selector(e.Current).ConfigureAwait(false);

                    while (await e.MoveNextAsync())
                    {
                        var x = await _selector(e.Current).ConfigureAwait(false);
                        if (x < value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<int> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<int>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<int> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<int>> _selector, CancellationToken _cancellationToken)
            {
                int value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);

                    while (await e.MoveNextAsync())
                    {
                        var x = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                        if (x < value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#endif
        public static ValueTask<int?> MinAsync(this IAsyncEnumerable<int?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<int?> Core(IAsyncEnumerable<int?> _source, CancellationToken _cancellationToken)
            {
                int? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = e.Current;
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    while (await e.MoveNextAsync())
                    {
                        var cur = e.Current;
                        var x = cur.GetValueOrDefault();

                        // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                        // unless nulls either never happen or always happen.
                        if (cur.HasValue & x < valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<int?> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<int?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, int?> _selector, CancellationToken _cancellationToken)
            {
                int? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = _selector(e.Current);
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    while (await e.MoveNextAsync())
                    {
                        var cur = _selector(e.Current);
                        var x = cur.GetValueOrDefault();

                        // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                        // unless nulls either never happen or always happen.
                        if (cur.HasValue & x < valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<int?> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<int?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<int?>> _selector, CancellationToken _cancellationToken)
            {
                int? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = await _selector(e.Current).ConfigureAwait(false);
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    while (await e.MoveNextAsync())
                    {
                        var cur = await _selector(e.Current).ConfigureAwait(false);
                        var x = cur.GetValueOrDefault();

                        // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                        // unless nulls either never happen or always happen.
                        if (cur.HasValue & x < valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<int?> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<int?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<int?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<int?>> _selector, CancellationToken _cancellationToken)
            {
                int? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    while (await e.MoveNextAsync())
                    {
                        var cur = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                        var x = cur.GetValueOrDefault();

                        // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                        // unless nulls either never happen or always happen.
                        if (cur.HasValue & x < valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#endif
        public static ValueTask<long> MinAsync(this IAsyncEnumerable<long> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<long> Core(IAsyncEnumerable<long> _source, CancellationToken _cancellationToken)
            {
                long value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = e.Current;

                    while (await e.MoveNextAsync())
                    {
                        var x = e.Current;
                        if (x < value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<long> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<long> Core(IAsyncEnumerable<TSource> _source, Func<TSource, long> _selector, CancellationToken _cancellationToken)
            {
                long value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = _selector(e.Current);

                    while (await e.MoveNextAsync())
                    {
                        var x = _selector(e.Current);
                        if (x < value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<long> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<long> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<long>> _selector, CancellationToken _cancellationToken)
            {
                long value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = await _selector(e.Current).ConfigureAwait(false);

                    while (await e.MoveNextAsync())
                    {
                        var x = await _selector(e.Current).ConfigureAwait(false);
                        if (x < value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<long> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<long>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<long> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<long>> _selector, CancellationToken _cancellationToken)
            {
                long value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);

                    while (await e.MoveNextAsync())
                    {
                        var x = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                        if (x < value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#endif
        public static ValueTask<long?> MinAsync(this IAsyncEnumerable<long?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<long?> Core(IAsyncEnumerable<long?> _source, CancellationToken _cancellationToken)
            {
                long? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = e.Current;
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    while (await e.MoveNextAsync())
                    {
                        var cur = e.Current;
                        var x = cur.GetValueOrDefault();

                        // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                        // unless nulls either never happen or always happen.
                        if (cur.HasValue & x < valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<long?> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<long?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, long?> _selector, CancellationToken _cancellationToken)
            {
                long? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = _selector(e.Current);
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    while (await e.MoveNextAsync())
                    {
                        var cur = _selector(e.Current);
                        var x = cur.GetValueOrDefault();

                        // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                        // unless nulls either never happen or always happen.
                        if (cur.HasValue & x < valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<long?> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<long?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<long?>> _selector, CancellationToken _cancellationToken)
            {
                long? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = await _selector(e.Current).ConfigureAwait(false);
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    while (await e.MoveNextAsync())
                    {
                        var cur = await _selector(e.Current).ConfigureAwait(false);
                        var x = cur.GetValueOrDefault();

                        // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                        // unless nulls either never happen or always happen.
                        if (cur.HasValue & x < valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<long?> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<long?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<long?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<long?>> _selector, CancellationToken _cancellationToken)
            {
                long? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    while (await e.MoveNextAsync())
                    {
                        var cur = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                        var x = cur.GetValueOrDefault();

                        // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                        // unless nulls either never happen or always happen.
                        if (cur.HasValue & x < valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#endif
        public static ValueTask<float> MinAsync(this IAsyncEnumerable<float> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<float> Core(IAsyncEnumerable<float> _source, CancellationToken _cancellationToken)
            {
                float value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = e.Current;

                    while (await e.MoveNextAsync())
                    {
                        var x = e.Current;
                        if (x < value)
                        {
                            value = x;
                        }
                        else
                        {
                            // Normally NaN < anything is false, as is anything < NaN
                            // However, this leads to some irksome outcomes in Min and Max.
                            // If we use those semantics then Min(NaN, 5.0) is NaN, but
                            // Min(5.0, NaN) is 5.0!  To fix this, we impose a total
                            // ordering where NaN is smaller than every value, including
                            // negative infinity.
                            // Not testing for NaN therefore isn't an option, but since we
                            // can't find a smaller value, we can short-circuit.

                            if (float.IsNaN(x))
                            {
                                return x;
                            }
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<float> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<float> Core(IAsyncEnumerable<TSource> _source, Func<TSource, float> _selector, CancellationToken _cancellationToken)
            {
                float value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = _selector(e.Current);

                    while (await e.MoveNextAsync())
                    {
                        var x = _selector(e.Current);
                        if (x < value)
                        {
                            value = x;
                        }
                        else
                        {
                            // Normally NaN < anything is false, as is anything < NaN
                            // However, this leads to some irksome outcomes in Min and Max.
                            // If we use those semantics then Min(NaN, 5.0) is NaN, but
                            // Min(5.0, NaN) is 5.0!  To fix this, we impose a total
                            // ordering where NaN is smaller than every value, including
                            // negative infinity.
                            // Not testing for NaN therefore isn't an option, but since we
                            // can't find a smaller value, we can short-circuit.

                            if (float.IsNaN(x))
                            {
                                return x;
                            }
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<float> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<float> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<float>> _selector, CancellationToken _cancellationToken)
            {
                float value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = await _selector(e.Current).ConfigureAwait(false);

                    while (await e.MoveNextAsync())
                    {
                        var x = await _selector(e.Current).ConfigureAwait(false);
                        if (x < value)
                        {
                            value = x;
                        }
                        else
                        {
                            // Normally NaN < anything is false, as is anything < NaN
                            // However, this leads to some irksome outcomes in Min and Max.
                            // If we use those semantics then Min(NaN, 5.0) is NaN, but
                            // Min(5.0, NaN) is 5.0!  To fix this, we impose a total
                            // ordering where NaN is smaller than every value, including
                            // negative infinity.
                            // Not testing for NaN therefore isn't an option, but since we
                            // can't find a smaller value, we can short-circuit.

                            if (float.IsNaN(x))
                            {
                                return x;
                            }
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<float> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<float>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<float> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<float>> _selector, CancellationToken _cancellationToken)
            {
                float value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);

                    while (await e.MoveNextAsync())
                    {
                        var x = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                        if (x < value)
                        {
                            value = x;
                        }
                        else
                        {
                            // Normally NaN < anything is false, as is anything < NaN
                            // However, this leads to some irksome outcomes in Min and Max.
                            // If we use those semantics then Min(NaN, 5.0) is NaN, but
                            // Min(5.0, NaN) is 5.0!  To fix this, we impose a total
                            // ordering where NaN is smaller than every value, including
                            // negative infinity.
                            // Not testing for NaN therefore isn't an option, but since we
                            // can't find a smaller value, we can short-circuit.

                            if (float.IsNaN(x))
                            {
                                return x;
                            }
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#endif
        public static ValueTask<float?> MinAsync(this IAsyncEnumerable<float?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<float?> Core(IAsyncEnumerable<float?> _source, CancellationToken _cancellationToken)
            {
                float? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = e.Current;
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    while (await e.MoveNextAsync())
                    {
                        var cur = e.Current;
                        if (cur.HasValue)
                        {
                            var x = cur.GetValueOrDefault();
                            if (x < valueVal)
                            {
                                valueVal = x;
                                value = cur;
                            }
                            else
                            {
                                // Normally NaN < anything is false, as is anything < NaN
                                // However, this leads to some irksome outcomes in Min and Max.
                                // If we use those semantics then Min(NaN, 5.0) is NaN, but
                                // Min(5.0, NaN) is 5.0!  To fix this, we impose a total
                                // ordering where NaN is smaller than every value, including
                                // negative infinity.
                                // Not testing for NaN therefore isn't an option, but since we
                                // can't find a smaller value, we can short-circuit.

                                if (float.IsNaN(x))
                                {
                                    return cur;
                                }
                            }
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<float?> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<float?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, float?> _selector, CancellationToken _cancellationToken)
            {
                float? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = _selector(e.Current);
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    while (await e.MoveNextAsync())
                    {
                        var cur = _selector(e.Current);
                        if (cur.HasValue)
                        {
                            var x = cur.GetValueOrDefault();
                            if (x < valueVal)
                            {
                                valueVal = x;
                                value = cur;
                            }
                            else
                            {
                                // Normally NaN < anything is false, as is anything < NaN
                                // However, this leads to some irksome outcomes in Min and Max.
                                // If we use those semantics then Min(NaN, 5.0) is NaN, but
                                // Min(5.0, NaN) is 5.0!  To fix this, we impose a total
                                // ordering where NaN is smaller than every value, including
                                // negative infinity.
                                // Not testing for NaN therefore isn't an option, but since we
                                // can't find a smaller value, we can short-circuit.

                                if (float.IsNaN(x))
                                {
                                    return cur;
                                }
                            }
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<float?> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<float?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<float?>> _selector, CancellationToken _cancellationToken)
            {
                float? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = await _selector(e.Current).ConfigureAwait(false);
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    while (await e.MoveNextAsync())
                    {
                        var cur = await _selector(e.Current).ConfigureAwait(false);
                        if (cur.HasValue)
                        {
                            var x = cur.GetValueOrDefault();
                            if (x < valueVal)
                            {
                                valueVal = x;
                                value = cur;
                            }
                            else
                            {
                                // Normally NaN < anything is false, as is anything < NaN
                                // However, this leads to some irksome outcomes in Min and Max.
                                // If we use those semantics then Min(NaN, 5.0) is NaN, but
                                // Min(5.0, NaN) is 5.0!  To fix this, we impose a total
                                // ordering where NaN is smaller than every value, including
                                // negative infinity.
                                // Not testing for NaN therefore isn't an option, but since we
                                // can't find a smaller value, we can short-circuit.

                                if (float.IsNaN(x))
                                {
                                    return cur;
                                }
                            }
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<float?> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<float?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<float?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<float?>> _selector, CancellationToken _cancellationToken)
            {
                float? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    while (await e.MoveNextAsync())
                    {
                        var cur = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                        if (cur.HasValue)
                        {
                            var x = cur.GetValueOrDefault();
                            if (x < valueVal)
                            {
                                valueVal = x;
                                value = cur;
                            }
                            else
                            {
                                // Normally NaN < anything is false, as is anything < NaN
                                // However, this leads to some irksome outcomes in Min and Max.
                                // If we use those semantics then Min(NaN, 5.0) is NaN, but
                                // Min(5.0, NaN) is 5.0!  To fix this, we impose a total
                                // ordering where NaN is smaller than every value, including
                                // negative infinity.
                                // Not testing for NaN therefore isn't an option, but since we
                                // can't find a smaller value, we can short-circuit.

                                if (float.IsNaN(x))
                                {
                                    return cur;
                                }
                            }
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#endif
        public static ValueTask<double> MinAsync(this IAsyncEnumerable<double> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<double> Core(IAsyncEnumerable<double> _source, CancellationToken _cancellationToken)
            {
                double value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = e.Current;

                    while (await e.MoveNextAsync())
                    {
                        var x = e.Current;
                        if (x < value)
                        {
                            value = x;
                        }
                        else
                        {
                            // Normally NaN < anything is false, as is anything < NaN
                            // However, this leads to some irksome outcomes in Min and Max.
                            // If we use those semantics then Min(NaN, 5.0) is NaN, but
                            // Min(5.0, NaN) is 5.0!  To fix this, we impose a total
                            // ordering where NaN is smaller than every value, including
                            // negative infinity.
                            // Not testing for NaN therefore isn't an option, but since we
                            // can't find a smaller value, we can short-circuit.

                            if (double.IsNaN(x))
                            {
                                return x;
                            }
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<double> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double> Core(IAsyncEnumerable<TSource> _source, Func<TSource, double> _selector, CancellationToken _cancellationToken)
            {
                double value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = _selector(e.Current);

                    while (await e.MoveNextAsync())
                    {
                        var x = _selector(e.Current);
                        if (x < value)
                        {
                            value = x;
                        }
                        else
                        {
                            // Normally NaN < anything is false, as is anything < NaN
                            // However, this leads to some irksome outcomes in Min and Max.
                            // If we use those semantics then Min(NaN, 5.0) is NaN, but
                            // Min(5.0, NaN) is 5.0!  To fix this, we impose a total
                            // ordering where NaN is smaller than every value, including
                            // negative infinity.
                            // Not testing for NaN therefore isn't an option, but since we
                            // can't find a smaller value, we can short-circuit.

                            if (double.IsNaN(x))
                            {
                                return x;
                            }
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<double> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<double>> _selector, CancellationToken _cancellationToken)
            {
                double value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = await _selector(e.Current).ConfigureAwait(false);

                    while (await e.MoveNextAsync())
                    {
                        var x = await _selector(e.Current).ConfigureAwait(false);
                        if (x < value)
                        {
                            value = x;
                        }
                        else
                        {
                            // Normally NaN < anything is false, as is anything < NaN
                            // However, this leads to some irksome outcomes in Min and Max.
                            // If we use those semantics then Min(NaN, 5.0) is NaN, but
                            // Min(5.0, NaN) is 5.0!  To fix this, we impose a total
                            // ordering where NaN is smaller than every value, including
                            // negative infinity.
                            // Not testing for NaN therefore isn't an option, but since we
                            // can't find a smaller value, we can short-circuit.

                            if (double.IsNaN(x))
                            {
                                return x;
                            }
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<double> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<double>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<double>> _selector, CancellationToken _cancellationToken)
            {
                double value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);

                    while (await e.MoveNextAsync())
                    {
                        var x = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                        if (x < value)
                        {
                            value = x;
                        }
                        else
                        {
                            // Normally NaN < anything is false, as is anything < NaN
                            // However, this leads to some irksome outcomes in Min and Max.
                            // If we use those semantics then Min(NaN, 5.0) is NaN, but
                            // Min(5.0, NaN) is 5.0!  To fix this, we impose a total
                            // ordering where NaN is smaller than every value, including
                            // negative infinity.
                            // Not testing for NaN therefore isn't an option, but since we
                            // can't find a smaller value, we can short-circuit.

                            if (double.IsNaN(x))
                            {
                                return x;
                            }
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#endif
        public static ValueTask<double?> MinAsync(this IAsyncEnumerable<double?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<double?> Core(IAsyncEnumerable<double?> _source, CancellationToken _cancellationToken)
            {
                double? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = e.Current;
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    while (await e.MoveNextAsync())
                    {
                        var cur = e.Current;
                        if (cur.HasValue)
                        {
                            var x = cur.GetValueOrDefault();
                            if (x < valueVal)
                            {
                                valueVal = x;
                                value = cur;
                            }
                            else
                            {
                                // Normally NaN < anything is false, as is anything < NaN
                                // However, this leads to some irksome outcomes in Min and Max.
                                // If we use those semantics then Min(NaN, 5.0) is NaN, but
                                // Min(5.0, NaN) is 5.0!  To fix this, we impose a total
                                // ordering where NaN is smaller than every value, including
                                // negative infinity.
                                // Not testing for NaN therefore isn't an option, but since we
                                // can't find a smaller value, we can short-circuit.

                                if (double.IsNaN(x))
                                {
                                    return cur;
                                }
                            }
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<double?> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, double?> _selector, CancellationToken _cancellationToken)
            {
                double? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = _selector(e.Current);
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    while (await e.MoveNextAsync())
                    {
                        var cur = _selector(e.Current);
                        if (cur.HasValue)
                        {
                            var x = cur.GetValueOrDefault();
                            if (x < valueVal)
                            {
                                valueVal = x;
                                value = cur;
                            }
                            else
                            {
                                // Normally NaN < anything is false, as is anything < NaN
                                // However, this leads to some irksome outcomes in Min and Max.
                                // If we use those semantics then Min(NaN, 5.0) is NaN, but
                                // Min(5.0, NaN) is 5.0!  To fix this, we impose a total
                                // ordering where NaN is smaller than every value, including
                                // negative infinity.
                                // Not testing for NaN therefore isn't an option, but since we
                                // can't find a smaller value, we can short-circuit.

                                if (double.IsNaN(x))
                                {
                                    return cur;
                                }
                            }
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<double?> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<double?>> _selector, CancellationToken _cancellationToken)
            {
                double? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = await _selector(e.Current).ConfigureAwait(false);
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    while (await e.MoveNextAsync())
                    {
                        var cur = await _selector(e.Current).ConfigureAwait(false);
                        if (cur.HasValue)
                        {
                            var x = cur.GetValueOrDefault();
                            if (x < valueVal)
                            {
                                valueVal = x;
                                value = cur;
                            }
                            else
                            {
                                // Normally NaN < anything is false, as is anything < NaN
                                // However, this leads to some irksome outcomes in Min and Max.
                                // If we use those semantics then Min(NaN, 5.0) is NaN, but
                                // Min(5.0, NaN) is 5.0!  To fix this, we impose a total
                                // ordering where NaN is smaller than every value, including
                                // negative infinity.
                                // Not testing for NaN therefore isn't an option, but since we
                                // can't find a smaller value, we can short-circuit.

                                if (double.IsNaN(x))
                                {
                                    return cur;
                                }
                            }
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<double?> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<double?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<double?>> _selector, CancellationToken _cancellationToken)
            {
                double? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    while (await e.MoveNextAsync())
                    {
                        var cur = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                        if (cur.HasValue)
                        {
                            var x = cur.GetValueOrDefault();
                            if (x < valueVal)
                            {
                                valueVal = x;
                                value = cur;
                            }
                            else
                            {
                                // Normally NaN < anything is false, as is anything < NaN
                                // However, this leads to some irksome outcomes in Min and Max.
                                // If we use those semantics then Min(NaN, 5.0) is NaN, but
                                // Min(5.0, NaN) is 5.0!  To fix this, we impose a total
                                // ordering where NaN is smaller than every value, including
                                // negative infinity.
                                // Not testing for NaN therefore isn't an option, but since we
                                // can't find a smaller value, we can short-circuit.

                                if (double.IsNaN(x))
                                {
                                    return cur;
                                }
                            }
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#endif
        public static ValueTask<decimal> MinAsync(this IAsyncEnumerable<decimal> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<decimal> Core(IAsyncEnumerable<decimal> _source, CancellationToken _cancellationToken)
            {
                decimal value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = e.Current;

                    while (await e.MoveNextAsync())
                    {
                        var x = e.Current;
                        if (x < value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<decimal> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<decimal> Core(IAsyncEnumerable<TSource> _source, Func<TSource, decimal> _selector, CancellationToken _cancellationToken)
            {
                decimal value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = _selector(e.Current);

                    while (await e.MoveNextAsync())
                    {
                        var x = _selector(e.Current);
                        if (x < value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<decimal> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<decimal> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<decimal>> _selector, CancellationToken _cancellationToken)
            {
                decimal value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = await _selector(e.Current).ConfigureAwait(false);

                    while (await e.MoveNextAsync())
                    {
                        var x = await _selector(e.Current).ConfigureAwait(false);
                        if (x < value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<decimal> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<decimal>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<decimal> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<decimal>> _selector, CancellationToken _cancellationToken)
            {
                decimal value;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);

                    while (await e.MoveNextAsync())
                    {
                        var x = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                        if (x < value)
                        {
                            value = x;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#endif
        public static ValueTask<decimal?> MinAsync(this IAsyncEnumerable<decimal?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<decimal?> Core(IAsyncEnumerable<decimal?> _source, CancellationToken _cancellationToken)
            {
                decimal? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = e.Current;
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    while (await e.MoveNextAsync())
                    {
                        var cur = e.Current;
                        var x = cur.GetValueOrDefault();

                        if (cur.HasValue && x < valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<decimal?> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<decimal?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, decimal?> _selector, CancellationToken _cancellationToken)
            {
                decimal? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = _selector(e.Current);
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    while (await e.MoveNextAsync())
                    {
                        var cur = _selector(e.Current);
                        var x = cur.GetValueOrDefault();

                        if (cur.HasValue && x < valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

        public static ValueTask<decimal?> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<decimal?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<decimal?>> _selector, CancellationToken _cancellationToken)
            {
                decimal? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = await _selector(e.Current).ConfigureAwait(false);
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    while (await e.MoveNextAsync())
                    {
                        var cur = await _selector(e.Current).ConfigureAwait(false);
                        var x = cur.GetValueOrDefault();

                        if (cur.HasValue && x < valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<decimal?> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<decimal?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<decimal?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<decimal?>> _selector, CancellationToken _cancellationToken)
            {
                decimal? value = null;

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                    // so we don't have to keep testing for nullity.
                    do
                    {
                        if (!await e.MoveNextAsync())
                        {
                            return value;
                        }

                        value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                    }
                    while (!value.HasValue);

                    // Keep hold of the wrapped value, and do comparisons on that, rather than
                    // using the lifted operation each time.
                    var valueVal = value.GetValueOrDefault();

                    while (await e.MoveNextAsync())
                    {
                        var cur = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);
                        var x = cur.GetValueOrDefault();

                        if (cur.HasValue && x < valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }

                return value;
            }
        }

#endif
    }
}
