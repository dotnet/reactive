// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<Int32> Max(this IAsyncObservable<Int32> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Int32>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxInt32(observer)));
        }

        public static IAsyncObservable<Int32> Max<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Int32> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Int32>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxInt32(observer, selector)));
        }

        public static IAsyncObservable<Int32> Max<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<Int32>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Int32>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxInt32(observer, selector)));
        }

        public static IAsyncObservable<Int32?> Max(this IAsyncObservable<Int32?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Int32?>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxNullableInt32(observer)));
        }

        public static IAsyncObservable<Int32?> Max<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Int32?> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Int32?>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxNullableInt32(observer, selector)));
        }

        public static IAsyncObservable<Int32?> Max<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<Int32?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Int32?>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxNullableInt32(observer, selector)));
        }

        public static IAsyncObservable<Int64> Max(this IAsyncObservable<Int64> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Int64>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxInt64(observer)));
        }

        public static IAsyncObservable<Int64> Max<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Int64> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Int64>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxInt64(observer, selector)));
        }

        public static IAsyncObservable<Int64> Max<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<Int64>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Int64>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxInt64(observer, selector)));
        }

        public static IAsyncObservable<Int64?> Max(this IAsyncObservable<Int64?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Int64?>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxNullableInt64(observer)));
        }

        public static IAsyncObservable<Int64?> Max<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Int64?> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Int64?>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxNullableInt64(observer, selector)));
        }

        public static IAsyncObservable<Int64?> Max<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<Int64?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Int64?>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxNullableInt64(observer, selector)));
        }

        public static IAsyncObservable<Single> Max(this IAsyncObservable<Single> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Single>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxSingle(observer)));
        }

        public static IAsyncObservable<Single> Max<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Single> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Single>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxSingle(observer, selector)));
        }

        public static IAsyncObservable<Single> Max<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<Single>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Single>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxSingle(observer, selector)));
        }

        public static IAsyncObservable<Single?> Max(this IAsyncObservable<Single?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Single?>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxNullableSingle(observer)));
        }

        public static IAsyncObservable<Single?> Max<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Single?> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Single?>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxNullableSingle(observer, selector)));
        }

        public static IAsyncObservable<Single?> Max<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<Single?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Single?>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxNullableSingle(observer, selector)));
        }

        public static IAsyncObservable<Double> Max(this IAsyncObservable<Double> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Double>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxDouble(observer)));
        }

        public static IAsyncObservable<Double> Max<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Double> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Double>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxDouble(observer, selector)));
        }

        public static IAsyncObservable<Double> Max<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<Double>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Double>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxDouble(observer, selector)));
        }

        public static IAsyncObservable<Double?> Max(this IAsyncObservable<Double?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Double?>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxNullableDouble(observer)));
        }

        public static IAsyncObservable<Double?> Max<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Double?> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Double?>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxNullableDouble(observer, selector)));
        }

        public static IAsyncObservable<Double?> Max<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<Double?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Double?>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxNullableDouble(observer, selector)));
        }

        public static IAsyncObservable<Decimal> Max(this IAsyncObservable<Decimal> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Decimal>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxDecimal(observer)));
        }

        public static IAsyncObservable<Decimal> Max<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Decimal> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Decimal>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxDecimal(observer, selector)));
        }

        public static IAsyncObservable<Decimal> Max<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<Decimal>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Decimal>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxDecimal(observer, selector)));
        }

        public static IAsyncObservable<Decimal?> Max(this IAsyncObservable<Decimal?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Decimal?>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxNullableDecimal(observer)));
        }

        public static IAsyncObservable<Decimal?> Max<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Decimal?> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Decimal?>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxNullableDecimal(observer, selector)));
        }

        public static IAsyncObservable<Decimal?> Max<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<Decimal?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Decimal?>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxNullableDecimal(observer, selector)));
        }

    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> MaxInt32<TSource>(IAsyncObserver<Int32> observer, Func<TSource, Int32> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MaxInt32(observer), selector);
        }

        public static IAsyncObserver<TSource> MaxInt32<TSource>(IAsyncObserver<Int32> observer, Func<TSource, Task<Int32>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MaxInt32(observer), selector);
        }

        public static IAsyncObserver<TSource> MaxNullableInt32<TSource>(IAsyncObserver<Int32?> observer, Func<TSource, Int32?> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MaxNullableInt32(observer), selector);
        }

        public static IAsyncObserver<TSource> MaxNullableInt32<TSource>(IAsyncObserver<Int32?> observer, Func<TSource, Task<Int32?>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MaxNullableInt32(observer), selector);
        }

        public static IAsyncObserver<TSource> MaxInt64<TSource>(IAsyncObserver<Int64> observer, Func<TSource, Int64> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MaxInt64(observer), selector);
        }

        public static IAsyncObserver<TSource> MaxInt64<TSource>(IAsyncObserver<Int64> observer, Func<TSource, Task<Int64>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MaxInt64(observer), selector);
        }

        public static IAsyncObserver<TSource> MaxNullableInt64<TSource>(IAsyncObserver<Int64?> observer, Func<TSource, Int64?> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MaxNullableInt64(observer), selector);
        }

        public static IAsyncObserver<TSource> MaxNullableInt64<TSource>(IAsyncObserver<Int64?> observer, Func<TSource, Task<Int64?>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MaxNullableInt64(observer), selector);
        }

        public static IAsyncObserver<TSource> MaxSingle<TSource>(IAsyncObserver<Single> observer, Func<TSource, Single> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MaxSingle(observer), selector);
        }

        public static IAsyncObserver<TSource> MaxSingle<TSource>(IAsyncObserver<Single> observer, Func<TSource, Task<Single>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MaxSingle(observer), selector);
        }

        public static IAsyncObserver<TSource> MaxNullableSingle<TSource>(IAsyncObserver<Single?> observer, Func<TSource, Single?> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MaxNullableSingle(observer), selector);
        }

        public static IAsyncObserver<TSource> MaxNullableSingle<TSource>(IAsyncObserver<Single?> observer, Func<TSource, Task<Single?>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MaxNullableSingle(observer), selector);
        }

        public static IAsyncObserver<TSource> MaxDouble<TSource>(IAsyncObserver<Double> observer, Func<TSource, Double> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MaxDouble(observer), selector);
        }

        public static IAsyncObserver<TSource> MaxDouble<TSource>(IAsyncObserver<Double> observer, Func<TSource, Task<Double>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MaxDouble(observer), selector);
        }

        public static IAsyncObserver<TSource> MaxNullableDouble<TSource>(IAsyncObserver<Double?> observer, Func<TSource, Double?> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MaxNullableDouble(observer), selector);
        }

        public static IAsyncObserver<TSource> MaxNullableDouble<TSource>(IAsyncObserver<Double?> observer, Func<TSource, Task<Double?>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MaxNullableDouble(observer), selector);
        }

        public static IAsyncObserver<TSource> MaxDecimal<TSource>(IAsyncObserver<Decimal> observer, Func<TSource, Decimal> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MaxDecimal(observer), selector);
        }

        public static IAsyncObserver<TSource> MaxDecimal<TSource>(IAsyncObserver<Decimal> observer, Func<TSource, Task<Decimal>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MaxDecimal(observer), selector);
        }

        public static IAsyncObserver<TSource> MaxNullableDecimal<TSource>(IAsyncObserver<Decimal?> observer, Func<TSource, Decimal?> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MaxNullableDecimal(observer), selector);
        }

        public static IAsyncObserver<TSource> MaxNullableDecimal<TSource>(IAsyncObserver<Decimal?> observer, Func<TSource, Task<Decimal?>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(MaxNullableDecimal(observer), selector);
        }

    }
}
