// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<Int32> Min(this IAsyncObservable<Int32> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Int32>(observer => source.SubscribeSafeAsync(AsyncObserver.MinInt32(observer)));
        }

        public static IAsyncObservable<Int32> Min<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Int32> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Int32>(observer => source.SubscribeSafeAsync(AsyncObserver.MinInt32(observer, selector)));
        }

        public static IAsyncObservable<Int32> Min<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<Int32>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Int32>(observer => source.SubscribeSafeAsync(AsyncObserver.MinInt32(observer, selector)));
        }

        public static IAsyncObservable<Int32?> Min(this IAsyncObservable<Int32?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Int32?>(observer => source.SubscribeSafeAsync(AsyncObserver.MinNullableInt32(observer)));
        }

        public static IAsyncObservable<Int32?> Min<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Int32?> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Int32?>(observer => source.SubscribeSafeAsync(AsyncObserver.MinNullableInt32(observer, selector)));
        }

        public static IAsyncObservable<Int32?> Min<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<Int32?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Int32?>(observer => source.SubscribeSafeAsync(AsyncObserver.MinNullableInt32(observer, selector)));
        }

        public static IAsyncObservable<Int64> Min(this IAsyncObservable<Int64> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Int64>(observer => source.SubscribeSafeAsync(AsyncObserver.MinInt64(observer)));
        }

        public static IAsyncObservable<Int64> Min<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Int64> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Int64>(observer => source.SubscribeSafeAsync(AsyncObserver.MinInt64(observer, selector)));
        }

        public static IAsyncObservable<Int64> Min<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<Int64>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Int64>(observer => source.SubscribeSafeAsync(AsyncObserver.MinInt64(observer, selector)));
        }

        public static IAsyncObservable<Int64?> Min(this IAsyncObservable<Int64?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Int64?>(observer => source.SubscribeSafeAsync(AsyncObserver.MinNullableInt64(observer)));
        }

        public static IAsyncObservable<Int64?> Min<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Int64?> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Int64?>(observer => source.SubscribeSafeAsync(AsyncObserver.MinNullableInt64(observer, selector)));
        }

        public static IAsyncObservable<Int64?> Min<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<Int64?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Int64?>(observer => source.SubscribeSafeAsync(AsyncObserver.MinNullableInt64(observer, selector)));
        }

        public static IAsyncObservable<Single> Min(this IAsyncObservable<Single> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Single>(observer => source.SubscribeSafeAsync(AsyncObserver.MinSingle(observer)));
        }

        public static IAsyncObservable<Single> Min<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Single> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Single>(observer => source.SubscribeSafeAsync(AsyncObserver.MinSingle(observer, selector)));
        }

        public static IAsyncObservable<Single> Min<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<Single>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Single>(observer => source.SubscribeSafeAsync(AsyncObserver.MinSingle(observer, selector)));
        }

        public static IAsyncObservable<Single?> Min(this IAsyncObservable<Single?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Single?>(observer => source.SubscribeSafeAsync(AsyncObserver.MinNullableSingle(observer)));
        }

        public static IAsyncObservable<Single?> Min<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Single?> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Single?>(observer => source.SubscribeSafeAsync(AsyncObserver.MinNullableSingle(observer, selector)));
        }

        public static IAsyncObservable<Single?> Min<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<Single?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Single?>(observer => source.SubscribeSafeAsync(AsyncObserver.MinNullableSingle(observer, selector)));
        }

        public static IAsyncObservable<Double> Min(this IAsyncObservable<Double> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Double>(observer => source.SubscribeSafeAsync(AsyncObserver.MinDouble(observer)));
        }

        public static IAsyncObservable<Double> Min<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Double> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Double>(observer => source.SubscribeSafeAsync(AsyncObserver.MinDouble(observer, selector)));
        }

        public static IAsyncObservable<Double> Min<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<Double>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Double>(observer => source.SubscribeSafeAsync(AsyncObserver.MinDouble(observer, selector)));
        }

        public static IAsyncObservable<Double?> Min(this IAsyncObservable<Double?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Double?>(observer => source.SubscribeSafeAsync(AsyncObserver.MinNullableDouble(observer)));
        }

        public static IAsyncObservable<Double?> Min<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Double?> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Double?>(observer => source.SubscribeSafeAsync(AsyncObserver.MinNullableDouble(observer, selector)));
        }

        public static IAsyncObservable<Double?> Min<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<Double?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Double?>(observer => source.SubscribeSafeAsync(AsyncObserver.MinNullableDouble(observer, selector)));
        }

        public static IAsyncObservable<Decimal> Min(this IAsyncObservable<Decimal> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Decimal>(observer => source.SubscribeSafeAsync(AsyncObserver.MinDecimal(observer)));
        }

        public static IAsyncObservable<Decimal> Min<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Decimal> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Decimal>(observer => source.SubscribeSafeAsync(AsyncObserver.MinDecimal(observer, selector)));
        }

        public static IAsyncObservable<Decimal> Min<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<Decimal>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Decimal>(observer => source.SubscribeSafeAsync(AsyncObserver.MinDecimal(observer, selector)));
        }

        public static IAsyncObservable<Decimal?> Min(this IAsyncObservable<Decimal?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Decimal?>(observer => source.SubscribeSafeAsync(AsyncObserver.MinNullableDecimal(observer)));
        }

        public static IAsyncObservable<Decimal?> Min<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Decimal?> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Decimal?>(observer => source.SubscribeSafeAsync(AsyncObserver.MinNullableDecimal(observer, selector)));
        }

        public static IAsyncObservable<Decimal?> Min<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<Decimal?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Decimal?>(observer => source.SubscribeSafeAsync(AsyncObserver.MinNullableDecimal(observer, selector)));
        }

    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> MinInt32<TSource>(IAsyncObserver<Int32> observer, Func<TSource, Int32> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MinInt32(observer), selector);
        }

        public static IAsyncObserver<TSource> MinInt32<TSource>(IAsyncObserver<Int32> observer, Func<TSource, Task<Int32>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MinInt32(observer), selector);
        }

        public static IAsyncObserver<TSource> MinNullableInt32<TSource>(IAsyncObserver<Int32?> observer, Func<TSource, Int32?> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MinNullableInt32(observer), selector);
        }

        public static IAsyncObserver<TSource> MinNullableInt32<TSource>(IAsyncObserver<Int32?> observer, Func<TSource, Task<Int32?>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MinNullableInt32(observer), selector);
        }

        public static IAsyncObserver<TSource> MinInt64<TSource>(IAsyncObserver<Int64> observer, Func<TSource, Int64> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MinInt64(observer), selector);
        }

        public static IAsyncObserver<TSource> MinInt64<TSource>(IAsyncObserver<Int64> observer, Func<TSource, Task<Int64>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MinInt64(observer), selector);
        }

        public static IAsyncObserver<TSource> MinNullableInt64<TSource>(IAsyncObserver<Int64?> observer, Func<TSource, Int64?> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MinNullableInt64(observer), selector);
        }

        public static IAsyncObserver<TSource> MinNullableInt64<TSource>(IAsyncObserver<Int64?> observer, Func<TSource, Task<Int64?>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MinNullableInt64(observer), selector);
        }

        public static IAsyncObserver<TSource> MinSingle<TSource>(IAsyncObserver<Single> observer, Func<TSource, Single> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MinSingle(observer), selector);
        }

        public static IAsyncObserver<TSource> MinSingle<TSource>(IAsyncObserver<Single> observer, Func<TSource, Task<Single>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MinSingle(observer), selector);
        }

        public static IAsyncObserver<TSource> MinNullableSingle<TSource>(IAsyncObserver<Single?> observer, Func<TSource, Single?> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MinNullableSingle(observer), selector);
        }

        public static IAsyncObserver<TSource> MinNullableSingle<TSource>(IAsyncObserver<Single?> observer, Func<TSource, Task<Single?>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MinNullableSingle(observer), selector);
        }

        public static IAsyncObserver<TSource> MinDouble<TSource>(IAsyncObserver<Double> observer, Func<TSource, Double> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MinDouble(observer), selector);
        }

        public static IAsyncObserver<TSource> MinDouble<TSource>(IAsyncObserver<Double> observer, Func<TSource, Task<Double>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MinDouble(observer), selector);
        }

        public static IAsyncObserver<TSource> MinNullableDouble<TSource>(IAsyncObserver<Double?> observer, Func<TSource, Double?> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MinNullableDouble(observer), selector);
        }

        public static IAsyncObserver<TSource> MinNullableDouble<TSource>(IAsyncObserver<Double?> observer, Func<TSource, Task<Double?>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MinNullableDouble(observer), selector);
        }

        public static IAsyncObserver<TSource> MinDecimal<TSource>(IAsyncObserver<Decimal> observer, Func<TSource, Decimal> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MinDecimal(observer), selector);
        }

        public static IAsyncObserver<TSource> MinDecimal<TSource>(IAsyncObserver<Decimal> observer, Func<TSource, Task<Decimal>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MinDecimal(observer), selector);
        }

        public static IAsyncObserver<TSource> MinNullableDecimal<TSource>(IAsyncObserver<Decimal?> observer, Func<TSource, Decimal?> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MinNullableDecimal(observer), selector);
        }

        public static IAsyncObserver<TSource> MinNullableDecimal<TSource>(IAsyncObserver<Decimal?> observer, Func<TSource, Task<Decimal?>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MinNullableDecimal(observer), selector);
        }

    }
}
