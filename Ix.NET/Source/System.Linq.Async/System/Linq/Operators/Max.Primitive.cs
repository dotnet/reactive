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
        private static async Task<int> MaxCore(IAsyncEnumerable<int> source, CancellationToken cancellationToken)
        {
            int value;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                value = e.Current;
                while (await e.MoveNextAsync().ConfigureAwait(false))
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
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<int?> MaxCore(IAsyncEnumerable<int?> source, CancellationToken cancellationToken)
        {
            int? value = null;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                do
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    value = e.Current;
                }
                while (!value.HasValue);

                var valueVal = value.GetValueOrDefault();
                if (valueVal >= 0)
                {
                    // We can fast-path this case where we know HasValue will
                    // never affect the outcome, without constantly checking
                    // if we're in such a state. Similar fast-paths could
                    // be done for other cases, but as all-positive
                    // or mostly-positive integer values are quite common in real-world
                    // uses, it's only been done in this direction for int? and long?.
                    while (await e.MoveNextAsync().ConfigureAwait(false))
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
                    while (await e.MoveNextAsync().ConfigureAwait(false))
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
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<long> MaxCore(IAsyncEnumerable<long> source, CancellationToken cancellationToken)
        {
            long value;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                value = e.Current;
                while (await e.MoveNextAsync().ConfigureAwait(false))
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
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<long?> MaxCore(IAsyncEnumerable<long?> source, CancellationToken cancellationToken)
        {
            long? value = null;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                do
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    value = e.Current;
                }
                while (!value.HasValue);

                var valueVal = value.GetValueOrDefault();
                if (valueVal >= 0)
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
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
                    while (await e.MoveNextAsync().ConfigureAwait(false))
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
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<double> MaxCore(IAsyncEnumerable<double> source, CancellationToken cancellationToken)
        {
            double value;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                value = e.Current;

                // As described in a comment on Min(IAsyncEnumerable<double>) NaN is ordered
                // less than all other values. We need to do explicit checks to ensure this, but
                // once we've found a value that is not NaN we need no longer worry about it,
                // so first loop until such a value is found (or not, as the case may be).
                while (double.IsNaN(value))
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    value = e.Current;
                }

                while (await e.MoveNextAsync().ConfigureAwait(false))
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
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<double?> MaxCore(IAsyncEnumerable<double?> source, CancellationToken cancellationToken)
        {
            double? value = null;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                do
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    value = e.Current;
                }
                while (!value.HasValue);

                var valueVal = value.GetValueOrDefault();
                while (double.IsNaN(valueVal))
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    var cur = e.Current;
                    if (cur.HasValue)
                    {
                        valueVal = (value = cur).GetValueOrDefault();
                    }
                }

                while (await e.MoveNextAsync().ConfigureAwait(false))
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
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<float> MaxCore(IAsyncEnumerable<float> source, CancellationToken cancellationToken)
        {
            float value;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                value = e.Current;
                while (float.IsNaN(value))
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    value = e.Current;
                }

                while (await e.MoveNextAsync().ConfigureAwait(false))
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
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<float?> MaxCore(IAsyncEnumerable<float?> source, CancellationToken cancellationToken)
        {
            float? value = null;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                do
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    value = e.Current;
                }
                while (!value.HasValue);

                var valueVal = value.GetValueOrDefault();
                while (float.IsNaN(valueVal))
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    var cur = e.Current;
                    if (cur.HasValue)
                    {
                        valueVal = (value = cur).GetValueOrDefault();
                    }
                }

                while (await e.MoveNextAsync().ConfigureAwait(false))
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
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<decimal> MaxCore(IAsyncEnumerable<decimal> source, CancellationToken cancellationToken)
        {
            decimal value;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                value = e.Current;
                while (await e.MoveNextAsync().ConfigureAwait(false))
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
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<decimal?> MaxCore(IAsyncEnumerable<decimal?> source, CancellationToken cancellationToken)
        {
            decimal? value = null;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                do
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    value = e.Current;
                }
                while (!value.HasValue);

                var valueVal = value.GetValueOrDefault();
                while (await e.MoveNextAsync().ConfigureAwait(false))
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
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<int> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken)
        {
            int value;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                value = selector(e.Current);
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var x = selector(e.Current);
                    if (x > value)
                    {
                        value = x;
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<int?> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken)
        {
            int? value = null;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                do
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    value = selector(e.Current);
                }
                while (!value.HasValue);

                var valueVal = value.GetValueOrDefault();
                if (valueVal >= 0)
                {
                    // We can fast-path this case where we know HasValue will
                    // never affect the outcome, without constantly checking
                    // if we're in such a state. Similar fast-paths could
                    // be done for other cases, but as all-positive
                    // or mostly-positive integer values are quite common in real-world
                    // uses, it's only been done in this direction for int? and long?.
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var cur = selector(e.Current);
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
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var cur = selector(e.Current);
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
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<long> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken)
        {
            long value;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                value = selector(e.Current);
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var x = selector(e.Current);
                    if (x > value)
                    {
                        value = x;
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<long?> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken)
        {
            long? value = null;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                do
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    value = selector(e.Current);
                }
                while (!value.HasValue);

                var valueVal = value.GetValueOrDefault();
                if (valueVal >= 0)
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var cur = selector(e.Current);
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
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var cur = selector(e.Current);
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
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<float> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken)
        {
            float value;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                value = selector(e.Current);
                while (float.IsNaN(value))
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    value = selector(e.Current);
                }

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var x = selector(e.Current);
                    if (x > value)
                    {
                        value = x;
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<float?> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken)
        {
            float? value = null;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                do
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    value = selector(e.Current);
                }
                while (!value.HasValue);

                var valueVal = value.GetValueOrDefault();
                while (float.IsNaN(valueVal))
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    var cur = selector(e.Current);
                    if (cur.HasValue)
                    {
                        valueVal = (value = cur).GetValueOrDefault();
                    }
                }

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var cur = selector(e.Current);
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
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<double> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken)
        {
            double value;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                value = selector(e.Current);

                // As described in a comment on Min(IAsyncEnumerable<double>) NaN is ordered
                // less than all other values. We need to do explicit checks to ensure this, but
                // once we've found a value that is not NaN we need no longer worry about it,
                // so first loop until such a value is found (or not, as the case may be).
                while (double.IsNaN(value))
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    value = selector(e.Current);
                }

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var x = selector(e.Current);
                    if (x > value)
                    {
                        value = x;
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<double?> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken)
        {
            double? value = null;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                do
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    value = selector(e.Current);
                }
                while (!value.HasValue);

                var valueVal = value.GetValueOrDefault();
                while (double.IsNaN(valueVal))
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    var cur = selector(e.Current);
                    if (cur.HasValue)
                    {
                        valueVal = (value = cur).GetValueOrDefault();
                    }
                }

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var cur = selector(e.Current);
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
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<decimal> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken)
        {
            decimal value;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                value = selector(e.Current);
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var x = selector(e.Current);
                    if (x > value)
                    {
                        value = x;
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<decimal?> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken)
        {
            decimal? value = null;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                do
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    value = selector(e.Current);
                }
                while (!value.HasValue);

                var valueVal = value.GetValueOrDefault();
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var cur = selector(e.Current);
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
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<int> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, Task<int>> selector, CancellationToken cancellationToken)
        {
            int value;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                value = await selector(e.Current).ConfigureAwait(false);
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var x = await selector(e.Current).ConfigureAwait(false);
                    if (x > value)
                    {
                        value = x;
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<int?> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, Task<int?>> selector, CancellationToken cancellationToken)
        {
            int? value = null;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                do
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    value = await selector(e.Current).ConfigureAwait(false);
                }
                while (!value.HasValue);

                var valueVal = value.GetValueOrDefault();
                if (valueVal >= 0)
                {
                    // We can fast-path this case where we know HasValue will
                    // never affect the outcome, without constantly checking
                    // if we're in such a state. Similar fast-paths could
                    // be done for other cases, but as all-positive
                    // or mostly-positive integer values are quite common in real-world
                    // uses, it's only been done in this direction for int? and long?.
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var cur = await selector(e.Current).ConfigureAwait(false);
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
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var cur = await selector(e.Current).ConfigureAwait(false);
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
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<long> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, Task<long>> selector, CancellationToken cancellationToken)
        {
            long value;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                value = await selector(e.Current).ConfigureAwait(false);
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var x = await selector(e.Current).ConfigureAwait(false);
                    if (x > value)
                    {
                        value = x;
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<long?> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, Task<long?>> selector, CancellationToken cancellationToken)
        {
            long? value = null;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                do
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    value = await selector(e.Current).ConfigureAwait(false);
                }
                while (!value.HasValue);

                var valueVal = value.GetValueOrDefault();
                if (valueVal >= 0)
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var cur = await selector(e.Current).ConfigureAwait(false);
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
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var cur = await selector(e.Current).ConfigureAwait(false);
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
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<float> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, Task<float>> selector, CancellationToken cancellationToken)
        {
            float value;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                value = await selector(e.Current).ConfigureAwait(false);
                while (float.IsNaN(value))
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    value = await selector(e.Current).ConfigureAwait(false);
                }

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var x = await selector(e.Current).ConfigureAwait(false);
                    if (x > value)
                    {
                        value = x;
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<float?> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, Task<float?>> selector, CancellationToken cancellationToken)
        {
            float? value = null;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                do
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    value = await selector(e.Current).ConfigureAwait(false);
                }
                while (!value.HasValue);

                var valueVal = value.GetValueOrDefault();
                while (float.IsNaN(valueVal))
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    var cur = await selector(e.Current).ConfigureAwait(false);
                    if (cur.HasValue)
                    {
                        valueVal = (value = cur).GetValueOrDefault();
                    }
                }

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var cur = await selector(e.Current).ConfigureAwait(false);
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
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<double> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, Task<double>> selector, CancellationToken cancellationToken)
        {
            double value;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                value = await selector(e.Current).ConfigureAwait(false);

                // As described in a comment on Min(IAsyncEnumerable<double>) NaN is ordered
                // less than all other values. We need to do explicit checks to ensure this, but
                // once we've found a value that is not NaN we need no longer worry about it,
                // so first loop until such a value is found (or not, as the case may be).
                while (double.IsNaN(value))
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    value = await selector(e.Current).ConfigureAwait(false);
                }

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var x = await selector(e.Current).ConfigureAwait(false);
                    if (x > value)
                    {
                        value = x;
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<double?> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, Task<double?>> selector, CancellationToken cancellationToken)
        {
            double? value = null;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                do
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    value = await selector(e.Current).ConfigureAwait(false);
                }
                while (!value.HasValue);

                var valueVal = value.GetValueOrDefault();
                while (double.IsNaN(valueVal))
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    var cur = await selector(e.Current).ConfigureAwait(false);
                    if (cur.HasValue)
                    {
                        valueVal = (value = cur).GetValueOrDefault();
                    }
                }

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var cur = await selector(e.Current).ConfigureAwait(false);
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
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<decimal> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, Task<decimal>> selector, CancellationToken cancellationToken)
        {
            decimal value;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                value = await selector(e.Current).ConfigureAwait(false);
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var x = await selector(e.Current).ConfigureAwait(false);
                    if (x > value)
                    {
                        value = x;
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<decimal?> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, Task<decimal?>> selector, CancellationToken cancellationToken)
        {
            decimal? value = null;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                do
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    value = await selector(e.Current).ConfigureAwait(false);
                }
                while (!value.HasValue);

                var valueVal = value.GetValueOrDefault();
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var cur = await selector(e.Current).ConfigureAwait(false);
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
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }
    }
}
