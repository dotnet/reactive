// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<Int32> Sum(this IAsyncObservable<Int32> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Int32>(observer => source.SubscribeAsync(AsyncObserver.SumInt32(observer)));
        }

        public static IAsyncObservable<Int32> Sum<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Int32> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Int32>(observer => source.SubscribeAsync(AsyncObserver.SumInt32(observer, selector)));
        }

        public static IAsyncObservable<Int32> Sum<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<Int32>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Int32>(observer => source.SubscribeAsync(AsyncObserver.SumInt32(observer, selector)));
        }

        public static IAsyncObservable<Int32?> Sum(this IAsyncObservable<Int32?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Int32?>(observer => source.SubscribeAsync(AsyncObserver.SumNullableInt32(observer)));
        }

        public static IAsyncObservable<Int32?> Sum<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Int32?> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Int32?>(observer => source.SubscribeAsync(AsyncObserver.SumNullableInt32(observer, selector)));
        }

        public static IAsyncObservable<Int32?> Sum<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<Int32?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Int32?>(observer => source.SubscribeAsync(AsyncObserver.SumNullableInt32(observer, selector)));
        }

        public static IAsyncObservable<Int64> Sum(this IAsyncObservable<Int64> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Int64>(observer => source.SubscribeAsync(AsyncObserver.SumInt64(observer)));
        }

        public static IAsyncObservable<Int64> Sum<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Int64> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Int64>(observer => source.SubscribeAsync(AsyncObserver.SumInt64(observer, selector)));
        }

        public static IAsyncObservable<Int64> Sum<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<Int64>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Int64>(observer => source.SubscribeAsync(AsyncObserver.SumInt64(observer, selector)));
        }

        public static IAsyncObservable<Int64?> Sum(this IAsyncObservable<Int64?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Int64?>(observer => source.SubscribeAsync(AsyncObserver.SumNullableInt64(observer)));
        }

        public static IAsyncObservable<Int64?> Sum<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Int64?> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Int64?>(observer => source.SubscribeAsync(AsyncObserver.SumNullableInt64(observer, selector)));
        }

        public static IAsyncObservable<Int64?> Sum<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<Int64?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Int64?>(observer => source.SubscribeAsync(AsyncObserver.SumNullableInt64(observer, selector)));
        }

        public static IAsyncObservable<Single> Sum(this IAsyncObservable<Single> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Single>(observer => source.SubscribeAsync(AsyncObserver.SumSingle(observer)));
        }

        public static IAsyncObservable<Single> Sum<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Single> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Single>(observer => source.SubscribeAsync(AsyncObserver.SumSingle(observer, selector)));
        }

        public static IAsyncObservable<Single> Sum<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<Single>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Single>(observer => source.SubscribeAsync(AsyncObserver.SumSingle(observer, selector)));
        }

        public static IAsyncObservable<Single?> Sum(this IAsyncObservable<Single?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Single?>(observer => source.SubscribeAsync(AsyncObserver.SumNullableSingle(observer)));
        }

        public static IAsyncObservable<Single?> Sum<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Single?> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Single?>(observer => source.SubscribeAsync(AsyncObserver.SumNullableSingle(observer, selector)));
        }

        public static IAsyncObservable<Single?> Sum<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<Single?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Single?>(observer => source.SubscribeAsync(AsyncObserver.SumNullableSingle(observer, selector)));
        }

        public static IAsyncObservable<Double> Sum(this IAsyncObservable<Double> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Double>(observer => source.SubscribeAsync(AsyncObserver.SumDouble(observer)));
        }

        public static IAsyncObservable<Double> Sum<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Double> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Double>(observer => source.SubscribeAsync(AsyncObserver.SumDouble(observer, selector)));
        }

        public static IAsyncObservable<Double> Sum<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<Double>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Double>(observer => source.SubscribeAsync(AsyncObserver.SumDouble(observer, selector)));
        }

        public static IAsyncObservable<Double?> Sum(this IAsyncObservable<Double?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Double?>(observer => source.SubscribeAsync(AsyncObserver.SumNullableDouble(observer)));
        }

        public static IAsyncObservable<Double?> Sum<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Double?> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Double?>(observer => source.SubscribeAsync(AsyncObserver.SumNullableDouble(observer, selector)));
        }

        public static IAsyncObservable<Double?> Sum<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<Double?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Double?>(observer => source.SubscribeAsync(AsyncObserver.SumNullableDouble(observer, selector)));
        }

        public static IAsyncObservable<Decimal> Sum(this IAsyncObservable<Decimal> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Decimal>(observer => source.SubscribeAsync(AsyncObserver.SumDecimal(observer)));
        }

        public static IAsyncObservable<Decimal> Sum<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Decimal> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Decimal>(observer => source.SubscribeAsync(AsyncObserver.SumDecimal(observer, selector)));
        }

        public static IAsyncObservable<Decimal> Sum<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<Decimal>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Decimal>(observer => source.SubscribeAsync(AsyncObserver.SumDecimal(observer, selector)));
        }

        public static IAsyncObservable<Decimal?> Sum(this IAsyncObservable<Decimal?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Decimal?>(observer => source.SubscribeAsync(AsyncObserver.SumNullableDecimal(observer)));
        }

        public static IAsyncObservable<Decimal?> Sum<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Decimal?> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Decimal?>(observer => source.SubscribeAsync(AsyncObserver.SumNullableDecimal(observer, selector)));
        }

        public static IAsyncObservable<Decimal?> Sum<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<Decimal?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<Decimal?>(observer => source.SubscribeAsync(AsyncObserver.SumNullableDecimal(observer, selector)));
        }

    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> SumInt32<TSource>(this IAsyncObserver<Int32> observer, Func<TSource, Int32> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(SumInt32(observer), selector);
        }

        public static IAsyncObserver<TSource> SumInt32<TSource>(this IAsyncObserver<Int32> observer, Func<TSource, Task<Int32>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(SumInt32(observer), selector);
        }

        public static IAsyncObserver<TSource> SumNullableInt32<TSource>(this IAsyncObserver<Int32?> observer, Func<TSource, Int32?> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(SumNullableInt32(observer), selector);
        }

        public static IAsyncObserver<TSource> SumNullableInt32<TSource>(this IAsyncObserver<Int32?> observer, Func<TSource, Task<Int32?>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(SumNullableInt32(observer), selector);
        }

        public static IAsyncObserver<TSource> SumInt64<TSource>(this IAsyncObserver<Int64> observer, Func<TSource, Int64> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(SumInt64(observer), selector);
        }

        public static IAsyncObserver<TSource> SumInt64<TSource>(this IAsyncObserver<Int64> observer, Func<TSource, Task<Int64>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(SumInt64(observer), selector);
        }

        public static IAsyncObserver<TSource> SumNullableInt64<TSource>(this IAsyncObserver<Int64?> observer, Func<TSource, Int64?> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(SumNullableInt64(observer), selector);
        }

        public static IAsyncObserver<TSource> SumNullableInt64<TSource>(this IAsyncObserver<Int64?> observer, Func<TSource, Task<Int64?>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(SumNullableInt64(observer), selector);
        }

        public static IAsyncObserver<TSource> SumSingle<TSource>(this IAsyncObserver<Single> observer, Func<TSource, Single> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(SumSingle(observer), selector);
        }

        public static IAsyncObserver<TSource> SumSingle<TSource>(this IAsyncObserver<Single> observer, Func<TSource, Task<Single>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(SumSingle(observer), selector);
        }

        public static IAsyncObserver<TSource> SumNullableSingle<TSource>(this IAsyncObserver<Single?> observer, Func<TSource, Single?> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(SumNullableSingle(observer), selector);
        }

        public static IAsyncObserver<TSource> SumNullableSingle<TSource>(this IAsyncObserver<Single?> observer, Func<TSource, Task<Single?>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(SumNullableSingle(observer), selector);
        }

        public static IAsyncObserver<TSource> SumDouble<TSource>(this IAsyncObserver<Double> observer, Func<TSource, Double> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(SumDouble(observer), selector);
        }

        public static IAsyncObserver<TSource> SumDouble<TSource>(this IAsyncObserver<Double> observer, Func<TSource, Task<Double>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(SumDouble(observer), selector);
        }

        public static IAsyncObserver<TSource> SumNullableDouble<TSource>(this IAsyncObserver<Double?> observer, Func<TSource, Double?> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(SumNullableDouble(observer), selector);
        }

        public static IAsyncObserver<TSource> SumNullableDouble<TSource>(this IAsyncObserver<Double?> observer, Func<TSource, Task<Double?>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(SumNullableDouble(observer), selector);
        }

        public static IAsyncObserver<TSource> SumDecimal<TSource>(this IAsyncObserver<Decimal> observer, Func<TSource, Decimal> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(SumDecimal(observer), selector);
        }

        public static IAsyncObserver<TSource> SumDecimal<TSource>(this IAsyncObserver<Decimal> observer, Func<TSource, Task<Decimal>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(SumDecimal(observer), selector);
        }

        public static IAsyncObserver<TSource> SumNullableDecimal<TSource>(this IAsyncObserver<Decimal?> observer, Func<TSource, Decimal?> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(SumNullableDecimal(observer), selector);
        }

        public static IAsyncObserver<TSource> SumNullableDecimal<TSource>(this IAsyncObserver<Decimal?> observer, Func<TSource, Task<Decimal?>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Select(SumNullableDecimal(observer), selector);
        }

    }
}
