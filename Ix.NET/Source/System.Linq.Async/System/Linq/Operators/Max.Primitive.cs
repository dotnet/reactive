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

            IAsyncEnumerator<int> e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                value = e.Current;

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    int x = e.Current;

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

            IAsyncEnumerator<int?> e = source.GetAsyncEnumerator(cancellationToken);

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

                int valueVal = value.GetValueOrDefault();

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
                        int? cur = e.Current;
                        int x = cur.GetValueOrDefault();

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
                        int? cur = e.Current;
                        int x = cur.GetValueOrDefault();

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

            IAsyncEnumerator<long> e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                value = e.Current;

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    long x = e.Current;

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

            IAsyncEnumerator<long?> e = source.GetAsyncEnumerator(cancellationToken);

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

                long valueVal = value.GetValueOrDefault();

                if (valueVal >= 0)
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        long? cur = e.Current;
                        long x = cur.GetValueOrDefault();

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
                        long? cur = e.Current;
                        long x = cur.GetValueOrDefault();

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

            IAsyncEnumerator<double> e = source.GetAsyncEnumerator(cancellationToken);

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
                    double x = e.Current;

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

            IAsyncEnumerator<double?> e = source.GetAsyncEnumerator(cancellationToken);

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

                double valueVal = value.GetValueOrDefault();

                while (double.IsNaN(valueVal))
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    double? cur = e.Current;

                    if (cur.HasValue)
                    {
                        valueVal = (value = cur).GetValueOrDefault();
                    }
                }

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    double? cur = e.Current;
                    double x = cur.GetValueOrDefault();

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

            IAsyncEnumerator<float> e = source.GetAsyncEnumerator(cancellationToken);

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
                    float x = e.Current;

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

            IAsyncEnumerator<float?> e = source.GetAsyncEnumerator(cancellationToken);

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

                float valueVal = value.GetValueOrDefault();

                while (float.IsNaN(valueVal))
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    float? cur = e.Current;

                    if (cur.HasValue)
                    {
                        valueVal = (value = cur).GetValueOrDefault();
                    }
                }

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    float? cur = e.Current;
                    float x = cur.GetValueOrDefault();

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

            IAsyncEnumerator<decimal> e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                value = e.Current;

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    decimal x = e.Current;

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

            IAsyncEnumerator<decimal?> e = source.GetAsyncEnumerator(cancellationToken);

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

                decimal valueVal = value.GetValueOrDefault();

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    decimal? cur = e.Current;
                    decimal x = cur.GetValueOrDefault();

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

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                value = selector(e.Current);

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    int x = selector(e.Current);

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

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

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

                int valueVal = value.GetValueOrDefault();

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
                        int? cur = selector(e.Current);
                        int x = cur.GetValueOrDefault();

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
                        int? cur = selector(e.Current);
                        int x = cur.GetValueOrDefault();

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

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                value = selector(e.Current);

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    long x = selector(e.Current);

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

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

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

                long valueVal = value.GetValueOrDefault();

                if (valueVal >= 0)
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        long? cur = selector(e.Current);
                        long x = cur.GetValueOrDefault();

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
                        long? cur = selector(e.Current);
                        long x = cur.GetValueOrDefault();

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

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

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
                    float x = selector(e.Current);

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

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

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

                float valueVal = value.GetValueOrDefault();

                while (float.IsNaN(valueVal))
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    float? cur = selector(e.Current);

                    if (cur.HasValue)
                    {
                        valueVal = (value = cur).GetValueOrDefault();
                    }
                }

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    float? cur = selector(e.Current);
                    float x = cur.GetValueOrDefault();

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

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

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
                    double x = selector(e.Current);

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

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

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

                double valueVal = value.GetValueOrDefault();

                while (double.IsNaN(valueVal))
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    double? cur = selector(e.Current);

                    if (cur.HasValue)
                    {
                        valueVal = (value = cur).GetValueOrDefault();
                    }
                }

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    double? cur = selector(e.Current);
                    double x = cur.GetValueOrDefault();

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

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                value = selector(e.Current);

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    decimal x = selector(e.Current);

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

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

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

                decimal valueVal = value.GetValueOrDefault();

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    decimal? cur = selector(e.Current);
                    decimal x = cur.GetValueOrDefault();

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

        private static async Task<int> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int>> selector, CancellationToken cancellationToken)
        {
            int value;

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                value = await selector(e.Current).ConfigureAwait(false);

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    int x = await selector(e.Current).ConfigureAwait(false);

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

        private static async Task<int?> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int?>> selector, CancellationToken cancellationToken)
        {
            int? value = null;

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

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

                int valueVal = value.GetValueOrDefault();

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
                        int? cur = await selector(e.Current).ConfigureAwait(false);
                        int x = cur.GetValueOrDefault();

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
                        int? cur = await selector(e.Current).ConfigureAwait(false);
                        int x = cur.GetValueOrDefault();

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

        private static async Task<long> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long>> selector, CancellationToken cancellationToken)
        {
            long value;

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                value = await selector(e.Current).ConfigureAwait(false);

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    long x = await selector(e.Current).ConfigureAwait(false);

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

        private static async Task<long?> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long?>> selector, CancellationToken cancellationToken)
        {
            long? value = null;

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

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

                long valueVal = value.GetValueOrDefault();

                if (valueVal >= 0)
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        long? cur = await selector(e.Current).ConfigureAwait(false);
                        long x = cur.GetValueOrDefault();

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
                        long? cur = await selector(e.Current).ConfigureAwait(false);
                        long x = cur.GetValueOrDefault();

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

        private static async Task<float> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float>> selector, CancellationToken cancellationToken)
        {
            float value;

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

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
                    float x = await selector(e.Current).ConfigureAwait(false);

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

        private static async Task<float?> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float?>> selector, CancellationToken cancellationToken)
        {
            float? value = null;

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

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

                float valueVal = value.GetValueOrDefault();

                while (float.IsNaN(valueVal))
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    float? cur = await selector(e.Current).ConfigureAwait(false);

                    if (cur.HasValue)
                    {
                        valueVal = (value = cur).GetValueOrDefault();
                    }
                }

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    float? cur = await selector(e.Current).ConfigureAwait(false);
                    float x = cur.GetValueOrDefault();

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

        private static async Task<double> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double>> selector, CancellationToken cancellationToken)
        {
            double value;

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

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
                    double x = await selector(e.Current).ConfigureAwait(false);

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

        private static async Task<double?> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double?>> selector, CancellationToken cancellationToken)
        {
            double? value = null;

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

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

                double valueVal = value.GetValueOrDefault();

                while (double.IsNaN(valueVal))
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    double? cur = await selector(e.Current).ConfigureAwait(false);

                    if (cur.HasValue)
                    {
                        valueVal = (value = cur).GetValueOrDefault();
                    }
                }

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    double? cur = await selector(e.Current).ConfigureAwait(false);
                    double x = cur.GetValueOrDefault();

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

        private static async Task<decimal> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal>> selector, CancellationToken cancellationToken)
        {
            decimal value;

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                value = await selector(e.Current).ConfigureAwait(false);

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    decimal x = await selector(e.Current).ConfigureAwait(false);

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

        private static async Task<decimal?> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal?>> selector, CancellationToken cancellationToken)
        {
            decimal? value = null;

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

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

                decimal valueVal = value.GetValueOrDefault();

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    decimal? cur = await selector(e.Current).ConfigureAwait(false);
                    decimal x = cur.GetValueOrDefault();

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

#if !NO_DEEP_CANCELLATION
        private static async Task<int> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<int>> selector, CancellationToken cancellationToken)
        {
            int value;

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                value = await selector(e.Current, cancellationToken).ConfigureAwait(false);

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    int x = await selector(e.Current, cancellationToken).ConfigureAwait(false);

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

        private static async Task<int?> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<int?>> selector, CancellationToken cancellationToken)
        {
            int? value = null;

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                do
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    value = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                }
                while (!value.HasValue);

                int valueVal = value.GetValueOrDefault();

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
                        int? cur = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                        int x = cur.GetValueOrDefault();

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
                        int? cur = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                        int x = cur.GetValueOrDefault();

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

        private static async Task<long> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<long>> selector, CancellationToken cancellationToken)
        {
            long value;

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                value = await selector(e.Current, cancellationToken).ConfigureAwait(false);

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    long x = await selector(e.Current, cancellationToken).ConfigureAwait(false);

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

        private static async Task<long?> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<long?>> selector, CancellationToken cancellationToken)
        {
            long? value = null;

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                do
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    value = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                }
                while (!value.HasValue);

                long valueVal = value.GetValueOrDefault();

                if (valueVal >= 0)
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        long? cur = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                        long x = cur.GetValueOrDefault();

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
                        long? cur = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                        long x = cur.GetValueOrDefault();

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

        private static async Task<float> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<float>> selector, CancellationToken cancellationToken)
        {
            float value;

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                value = await selector(e.Current, cancellationToken).ConfigureAwait(false);

                while (float.IsNaN(value))
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    value = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                }

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    float x = await selector(e.Current, cancellationToken).ConfigureAwait(false);

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

        private static async Task<float?> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<float?>> selector, CancellationToken cancellationToken)
        {
            float? value = null;

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                do
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    value = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                }
                while (!value.HasValue);

                float valueVal = value.GetValueOrDefault();

                while (float.IsNaN(valueVal))
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    float? cur = await selector(e.Current, cancellationToken).ConfigureAwait(false);

                    if (cur.HasValue)
                    {
                        valueVal = (value = cur).GetValueOrDefault();
                    }
                }

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    float? cur = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                    float x = cur.GetValueOrDefault();

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

        private static async Task<double> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<double>> selector, CancellationToken cancellationToken)
        {
            double value;

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                value = await selector(e.Current, cancellationToken).ConfigureAwait(false);

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

                    value = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                }

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    double x = await selector(e.Current, cancellationToken).ConfigureAwait(false);

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

        private static async Task<double?> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<double?>> selector, CancellationToken cancellationToken)
        {
            double? value = null;

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                do
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    value = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                }
                while (!value.HasValue);

                double valueVal = value.GetValueOrDefault();

                while (double.IsNaN(valueVal))
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    double? cur = await selector(e.Current, cancellationToken).ConfigureAwait(false);

                    if (cur.HasValue)
                    {
                        valueVal = (value = cur).GetValueOrDefault();
                    }
                }

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    double? cur = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                    double x = cur.GetValueOrDefault();

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

        private static async Task<decimal> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<decimal>> selector, CancellationToken cancellationToken)
        {
            decimal value;

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                value = await selector(e.Current, cancellationToken).ConfigureAwait(false);

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    decimal x = await selector(e.Current, cancellationToken).ConfigureAwait(false);

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

        private static async Task<decimal?> MaxCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<decimal?>> selector, CancellationToken cancellationToken)
        {
            decimal? value = null;

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                do
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return value;
                    }

                    value = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                }
                while (!value.HasValue);

                decimal valueVal = value.GetValueOrDefault();

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    decimal? cur = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                    decimal x = cur.GetValueOrDefault();

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
#endif
    }
}
