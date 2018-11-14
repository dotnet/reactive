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
        private static async Task<int> MinCore(IAsyncEnumerable<int> source, CancellationToken cancellationToken)
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
                    if (x < value)
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

        private static async Task<int?> MinCore(IAsyncEnumerable<int?> source, CancellationToken cancellationToken)
        {
            int? value = null;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                // so we don't have to keep testing for nullity.
                do
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    value = e.Current;
                }
                while (!value.HasValue);

                // Keep hold of the wrapped value, and do comparisons on that, rather than
                // using the lifted operation each time.
                var valueVal = value.GetValueOrDefault();
                while (await e.MoveNextAsync().ConfigureAwait(false))
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
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<long> MinCore(IAsyncEnumerable<long> source, CancellationToken cancellationToken)
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
                    if (x < value)
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

        private static async Task<long?> MinCore(IAsyncEnumerable<long?> source, CancellationToken cancellationToken)
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
                while (await e.MoveNextAsync().ConfigureAwait(false))
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
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<float> MinCore(IAsyncEnumerable<float> source, CancellationToken cancellationToken)
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
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var x = e.Current;
                    if (x < value)
                    {
                        value = x;
                    }

                    // Normally NaN < anything is false, as is anything < NaN
                    // However, this leads to some irksome outcomes in Min and Max.
                    // If we use those semantics then Min(NaN, 5.0) is NaN, but
                    // Min(5.0, NaN) is 5.0!  To fix this, we impose a total
                    // ordering where NaN is smaller than every value, including
                    // negative infinity.
                    // Not testing for NaN therefore isn't an option, but since we
                    // can't find a smaller value, we can short-circuit.
                    else if (float.IsNaN(x))
                    {
                        return x;
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<float?> MinCore(IAsyncEnumerable<float?> source, CancellationToken cancellationToken)
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
                while (await e.MoveNextAsync().ConfigureAwait(false))
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
                        else if (float.IsNaN(x))
                        {
                            return cur;
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

        private static async Task<double> MinCore(IAsyncEnumerable<double> source, CancellationToken cancellationToken)
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
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var x = e.Current;
                    if (x < value)
                    {
                        value = x;
                    }
                    else if (double.IsNaN(x))
                    {
                        return x;
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<double?> MinCore(IAsyncEnumerable<double?> source, CancellationToken cancellationToken)
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
                while (await e.MoveNextAsync().ConfigureAwait(false))
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
                        else if (double.IsNaN(x))
                        {
                            return cur;
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

        private static async Task<decimal> MinCore(IAsyncEnumerable<decimal> source, CancellationToken cancellationToken)
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
                    if (x < value)
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

        private static async Task<decimal?> MinCore(IAsyncEnumerable<decimal?> source, CancellationToken cancellationToken)
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
                    if (cur.HasValue && x < valueVal)
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

        private static async Task<int> MinCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken)
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
                    if (x < value)
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

        private static async Task<int?> MinCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken)
        {
            int? value = null;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                // so we don't have to keep testing for nullity.
                do
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    value = selector(e.Current);
                }
                while (!value.HasValue);

                // Keep hold of the wrapped value, and do comparisons on that, rather than
                // using the lifted operation each time.
                var valueVal = value.GetValueOrDefault();
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var cur = selector(e.Current);
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
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<long> MinCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken)
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
                    if (x < value)
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

        private static async Task<long?> MinCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken)
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
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var cur = selector(e.Current);
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
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<float> MinCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken)
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
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var x = selector(e.Current);
                    if (x < value)
                    {
                        value = x;
                    }

                    // Normally NaN < anything is false, as is anything < NaN
                    // However, this leads to some irksome outcomes in Min and Max.
                    // If we use those semantics then Min(NaN, 5.0) is NaN, but
                    // Min(5.0, NaN) is 5.0!  To fix this, we impose a total
                    // ordering where NaN is smaller than every value, including
                    // negative infinity.
                    // Not testing for NaN therefore isn't an option, but since we
                    // can't find a smaller value, we can short-circuit.
                    else if (float.IsNaN(x))
                    {
                        return x;
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<float?> MinCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken)
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
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var cur = selector(e.Current);
                    if (cur.HasValue)
                    {
                        var x = cur.GetValueOrDefault();
                        if (x < valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                        else if (float.IsNaN(x))
                        {
                            return cur;
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

        private static async Task<double> MinCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken)
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
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var x = selector(e.Current);
                    if (x < value)
                    {
                        value = x;
                    }
                    else if (double.IsNaN(x))
                    {
                        return x;
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<double?> MinCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken)
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
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var cur = selector(e.Current);
                    if (cur.HasValue)
                    {
                        var x = cur.GetValueOrDefault();
                        if (x < valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                        else if (double.IsNaN(x))
                        {
                            return cur;
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

        private static async Task<decimal> MinCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken)
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
                    if (x < value)
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

        private static async Task<decimal?> MinCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken)
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
                    if (cur.HasValue && x < valueVal)
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

        private static async Task<int> MinCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int>> selector, CancellationToken cancellationToken)
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
                    if (x < value)
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

        private static async Task<int?> MinCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int?>> selector, CancellationToken cancellationToken)
        {
            int? value = null;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                // so we don't have to keep testing for nullity.
                do
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    value = await selector(e.Current).ConfigureAwait(false);
                }
                while (!value.HasValue);

                // Keep hold of the wrapped value, and do comparisons on that, rather than
                // using the lifted operation each time.
                var valueVal = value.GetValueOrDefault();
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var cur = await selector(e.Current).ConfigureAwait(false);
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
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<long> MinCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long>> selector, CancellationToken cancellationToken)
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
                    if (x < value)
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

        private static async Task<long?> MinCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long?>> selector, CancellationToken cancellationToken)
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
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var cur = await selector(e.Current).ConfigureAwait(false);
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
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<float> MinCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float>> selector, CancellationToken cancellationToken)
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
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var x = await selector(e.Current).ConfigureAwait(false);
                    if (x < value)
                    {
                        value = x;
                    }

                    // Normally NaN < anything is false, as is anything < NaN
                    // However, this leads to some irksome outcomes in Min and Max.
                    // If we use those semantics then Min(NaN, 5.0) is NaN, but
                    // Min(5.0, NaN) is 5.0!  To fix this, we impose a total
                    // ordering where NaN is smaller than every value, including
                    // negative infinity.
                    // Not testing for NaN therefore isn't an option, but since we
                    // can't find a smaller value, we can short-circuit.
                    else if (float.IsNaN(x))
                    {
                        return x;
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<float?> MinCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float?>> selector, CancellationToken cancellationToken)
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
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var cur = await selector(e.Current).ConfigureAwait(false);
                    if (cur.HasValue)
                    {
                        var x = cur.GetValueOrDefault();
                        if (x < valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                        else if (float.IsNaN(x))
                        {
                            return cur;
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

        private static async Task<double> MinCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double>> selector, CancellationToken cancellationToken)
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
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var x = await selector(e.Current).ConfigureAwait(false);
                    if (x < value)
                    {
                        value = x;
                    }
                    else if (double.IsNaN(x))
                    {
                        return x;
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return value;
        }

        private static async Task<double?> MinCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double?>> selector, CancellationToken cancellationToken)
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
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var cur = await selector(e.Current).ConfigureAwait(false);
                    if (cur.HasValue)
                    {
                        var x = cur.GetValueOrDefault();
                        if (x < valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                        else if (double.IsNaN(x))
                        {
                            return cur;
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

        private static async Task<decimal> MinCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal>> selector, CancellationToken cancellationToken)
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
                    if (x < value)
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

        private static async Task<decimal?> MinCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal?>> selector, CancellationToken cancellationToken)
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
                    if (cur.HasValue && x < valueVal)
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
