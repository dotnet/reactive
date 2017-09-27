// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(this IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector, Func<IGroupedAsyncObservable<TKey, TSource>, IAsyncObservable<TDuration>> durationSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));

            return Create<IGroupedAsyncObservable<TKey, TSource>>(observer => GroupByUntilCore<TSource, TKey, TSource, TDuration>(source, observer, (o, d) => AsyncObserver.GroupByUntil(o, d, keySelector, durationSelector)));
        }

        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(this IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector, Func<IGroupedAsyncObservable<TKey, TSource>, IAsyncObservable<TDuration>> durationSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Create<IGroupedAsyncObservable<TKey, TSource>>(observer => GroupByUntilCore<TSource, TKey, TSource, TDuration>(source, observer, (o, d) => AsyncObserver.GroupByUntil(o, d, keySelector, durationSelector, comparer)));
        }

        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(this IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector, Func<IGroupedAsyncObservable<TKey, TSource>, IAsyncObservable<TDuration>> durationSelector, int capacity)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            return Create<IGroupedAsyncObservable<TKey, TSource>>(observer => GroupByUntilCore<TSource, TKey, TSource, TDuration>(source, observer, (o, d) => AsyncObserver.GroupByUntil(o, d, keySelector, durationSelector, capacity)));
        }

        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(this IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector, Func<IGroupedAsyncObservable<TKey, TSource>, IAsyncObservable<TDuration>> durationSelector, int capacity, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Create<IGroupedAsyncObservable<TKey, TSource>>(observer => GroupByUntilCore<TSource, TKey, TSource, TDuration>(source, observer, (o, d) => AsyncObserver.GroupByUntil(o, d, keySelector, durationSelector, capacity, comparer)));
        }

        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(this IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<IGroupedAsyncObservable<TKey, TElement>, IAsyncObservable<TDuration>> durationSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));

            return Create<IGroupedAsyncObservable<TKey, TElement>>(observer => GroupByUntilCore<TSource, TKey, TElement, TDuration>(source, observer, (o, d) => AsyncObserver.GroupByUntil(o, d, keySelector, elementSelector, durationSelector)));
        }

        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(this IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<IGroupedAsyncObservable<TKey, TElement>, IAsyncObservable<TDuration>> durationSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Create<IGroupedAsyncObservable<TKey, TElement>>(observer => GroupByUntilCore<TSource, TKey, TElement, TDuration>(source, observer, (o, d) => AsyncObserver.GroupByUntil(o, d, keySelector, elementSelector, durationSelector, comparer)));
        }

        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(this IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<IGroupedAsyncObservable<TKey, TElement>, IAsyncObservable<TDuration>> durationSelector, int capacity)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            return Create<IGroupedAsyncObservable<TKey, TElement>>(observer => GroupByUntilCore<TSource, TKey, TElement, TDuration>(source, observer, (o, d) => AsyncObserver.GroupByUntil(o, d, keySelector, elementSelector, durationSelector, capacity)));
        }

        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(this IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<IGroupedAsyncObservable<TKey, TElement>, IAsyncObservable<TDuration>> durationSelector, int capacity, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Create<IGroupedAsyncObservable<TKey, TElement>>(observer => GroupByUntilCore<TSource, TKey, TElement, TDuration>(source, observer, (o, d) => AsyncObserver.GroupByUntil(o, d, keySelector, elementSelector, durationSelector, capacity, comparer)));
        }

        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(this IAsyncObservable<TSource> source, Func<TSource, Task<TKey>> keySelector, Func<IGroupedAsyncObservable<TKey, TSource>, IAsyncObservable<TDuration>> durationSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));

            return Create<IGroupedAsyncObservable<TKey, TSource>>(observer => GroupByUntilCore<TSource, TKey, TSource, TDuration>(source, observer, (o, d) => AsyncObserver.GroupByUntil(o, d, keySelector, durationSelector)));
        }

        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(this IAsyncObservable<TSource> source, Func<TSource, Task<TKey>> keySelector, Func<IGroupedAsyncObservable<TKey, TSource>, IAsyncObservable<TDuration>> durationSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Create<IGroupedAsyncObservable<TKey, TSource>>(observer => GroupByUntilCore<TSource, TKey, TSource, TDuration>(source, observer, (o, d) => AsyncObserver.GroupByUntil(o, d, keySelector, durationSelector, comparer)));
        }

        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(this IAsyncObservable<TSource> source, Func<TSource, Task<TKey>> keySelector, Func<IGroupedAsyncObservable<TKey, TSource>, IAsyncObservable<TDuration>> durationSelector, int capacity)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            return Create<IGroupedAsyncObservable<TKey, TSource>>(observer => GroupByUntilCore<TSource, TKey, TSource, TDuration>(source, observer, (o, d) => AsyncObserver.GroupByUntil(o, d, keySelector, durationSelector, capacity)));
        }

        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(this IAsyncObservable<TSource> source, Func<TSource, Task<TKey>> keySelector, Func<IGroupedAsyncObservable<TKey, TSource>, IAsyncObservable<TDuration>> durationSelector, int capacity, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Create<IGroupedAsyncObservable<TKey, TSource>>(observer => GroupByUntilCore<TSource, TKey, TSource, TDuration>(source, observer, (o, d) => AsyncObserver.GroupByUntil(o, d, keySelector, durationSelector, capacity, comparer)));
        }

        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(this IAsyncObservable<TSource> source, Func<TSource, Task<TKey>> keySelector, Func<TSource, Task<TElement>> elementSelector, Func<IGroupedAsyncObservable<TKey, TElement>, IAsyncObservable<TDuration>> durationSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));

            return Create<IGroupedAsyncObservable<TKey, TElement>>(observer => GroupByUntilCore<TSource, TKey, TElement, TDuration>(source, observer, (o, d) => AsyncObserver.GroupByUntil(o, d, keySelector, elementSelector, durationSelector)));
        }

        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(this IAsyncObservable<TSource> source, Func<TSource, Task<TKey>> keySelector, Func<TSource, Task<TElement>> elementSelector, Func<IGroupedAsyncObservable<TKey, TElement>, IAsyncObservable<TDuration>> durationSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Create<IGroupedAsyncObservable<TKey, TElement>>(observer => GroupByUntilCore<TSource, TKey, TElement, TDuration>(source, observer, (o, d) => AsyncObserver.GroupByUntil(o, d, keySelector, elementSelector, durationSelector, comparer)));
        }

        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(this IAsyncObservable<TSource> source, Func<TSource, Task<TKey>> keySelector, Func<TSource, Task<TElement>> elementSelector, Func<IGroupedAsyncObservable<TKey, TElement>, IAsyncObservable<TDuration>> durationSelector, int capacity)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            return Create<IGroupedAsyncObservable<TKey, TElement>>(observer => GroupByUntilCore<TSource, TKey, TElement, TDuration>(source, observer, (o, d) => AsyncObserver.GroupByUntil(o, d, keySelector, elementSelector, durationSelector, capacity)));
        }

        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(this IAsyncObservable<TSource> source, Func<TSource, Task<TKey>> keySelector, Func<TSource, Task<TElement>> elementSelector, Func<IGroupedAsyncObservable<TKey, TElement>, IAsyncObservable<TDuration>> durationSelector, int capacity, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Create<IGroupedAsyncObservable<TKey, TElement>>(observer => GroupByUntilCore<TSource, TKey, TElement, TDuration>(source, observer, (o, d) => AsyncObserver.GroupByUntil(o, d, keySelector, elementSelector, durationSelector, capacity, comparer)));
        }

        private static async Task<IAsyncDisposable> GroupByUntilCore<TSource, TKey, TElement, TDuration>(IAsyncObservable<TSource> source, IAsyncObserver<IGroupedAsyncObservable<TKey, TElement>> observer, Func<IAsyncObserver<IGroupedAsyncObservable<TKey, TElement>>, IAsyncDisposable, Task<(IAsyncObserver<TSource>, IAsyncDisposable)>> createObserver)
        {
            var d = new SingleAssignmentAsyncDisposable();

            var (sink, subscription) = await createObserver(observer, d).ConfigureAwait(false);

            var inner = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);
            await d.AssignAsync(inner).ConfigureAwait(false);

            return subscription;
        }
    }

    partial class AsyncObserver
    {
        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> GroupByUntil<TSource, TKey, TDuration>(IAsyncObserver<IGroupedAsyncObservable<TKey, TSource>> observer, IAsyncDisposable subscription, Func<TSource, TKey> keySelector, Func<IGroupedAsyncObservable<TKey, TSource>, IAsyncObservable<TDuration>> durationSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));

            return GroupByUntil(observer, subscription, keySelector, durationSelector, int.MaxValue, EqualityComparer<TKey>.Default);
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> GroupByUntil<TSource, TKey, TDuration>(IAsyncObserver<IGroupedAsyncObservable<TKey, TSource>> observer, IAsyncDisposable subscription, Func<TSource, TKey> keySelector, Func<IGroupedAsyncObservable<TKey, TSource>, IAsyncObservable<TDuration>> durationSelector, IEqualityComparer<TKey> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return GroupByUntil(observer, subscription, keySelector, durationSelector, int.MaxValue, comparer);
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> GroupByUntil<TSource, TKey, TDuration>(IAsyncObserver<IGroupedAsyncObservable<TKey, TSource>> observer, IAsyncDisposable subscription, Func<TSource, TKey> keySelector, Func<IGroupedAsyncObservable<TKey, TSource>, IAsyncObservable<TDuration>> durationSelector, int capacity)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            return GroupByUntil(observer, subscription, keySelector, durationSelector, capacity, EqualityComparer<TKey>.Default);
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> GroupByUntil<TSource, TKey, TDuration>(IAsyncObserver<IGroupedAsyncObservable<TKey, TSource>> observer, IAsyncDisposable subscription, Func<TSource, TKey> keySelector, Func<IGroupedAsyncObservable<TKey, TSource>, IAsyncObservable<TDuration>> durationSelector, int capacity, IEqualityComparer<TKey> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return GroupByUntil(observer, subscription, x => Task.FromResult(keySelector(x)), durationSelector, capacity, comparer);
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> GroupByUntil<TSource, TKey, TElement, TDuration>(IAsyncObserver<IGroupedAsyncObservable<TKey, TElement>> observer, IAsyncDisposable subscription, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<IGroupedAsyncObservable<TKey, TElement>, IAsyncObservable<TDuration>> durationSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));

            return GroupByUntil(observer, subscription, keySelector, elementSelector, durationSelector, int.MaxValue, EqualityComparer<TKey>.Default);
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> GroupByUntil<TSource, TKey, TElement, TDuration>(IAsyncObserver<IGroupedAsyncObservable<TKey, TElement>> observer, IAsyncDisposable subscription, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<IGroupedAsyncObservable<TKey, TElement>, IAsyncObservable<TDuration>> durationSelector, IEqualityComparer<TKey> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return GroupByUntil(observer, subscription, keySelector, elementSelector, durationSelector, int.MaxValue, comparer);
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> GroupByUntil<TSource, TKey, TElement, TDuration>(IAsyncObserver<IGroupedAsyncObservable<TKey, TElement>> observer, IAsyncDisposable subscription, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<IGroupedAsyncObservable<TKey, TElement>, IAsyncObservable<TDuration>> durationSelector, int capacity)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            return GroupByUntil(observer, subscription, keySelector, elementSelector, durationSelector, capacity, EqualityComparer<TKey>.Default);
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> GroupByUntil<TSource, TKey, TElement, TDuration>(IAsyncObserver<IGroupedAsyncObservable<TKey, TElement>> observer, IAsyncDisposable subscription, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<IGroupedAsyncObservable<TKey, TElement>, IAsyncObservable<TDuration>> durationSelector, int capacity, IEqualityComparer<TKey> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return GroupByUntil<TSource, TKey, TElement, TDuration>(observer, subscription, x => Task.FromResult(keySelector(x)), x => Task.FromResult(elementSelector(x)), durationSelector, capacity, comparer);
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> GroupByUntil<TSource, TKey, TDuration>(IAsyncObserver<IGroupedAsyncObservable<TKey, TSource>> observer, IAsyncDisposable subscription, Func<TSource, Task<TKey>> keySelector, Func<IGroupedAsyncObservable<TKey, TSource>, IAsyncObservable<TDuration>> durationSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));

            return GroupByUntil(observer, subscription, keySelector, durationSelector, int.MaxValue, EqualityComparer<TKey>.Default);
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> GroupByUntil<TSource, TKey, TDuration>(IAsyncObserver<IGroupedAsyncObservable<TKey, TSource>> observer, IAsyncDisposable subscription, Func<TSource, Task<TKey>> keySelector, Func<IGroupedAsyncObservable<TKey, TSource>, IAsyncObservable<TDuration>> durationSelector, IEqualityComparer<TKey> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return GroupByUntil(observer, subscription, keySelector, durationSelector, int.MaxValue, comparer);
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> GroupByUntil<TSource, TKey, TDuration>(IAsyncObserver<IGroupedAsyncObservable<TKey, TSource>> observer, IAsyncDisposable subscription, Func<TSource, Task<TKey>> keySelector, Func<IGroupedAsyncObservable<TKey, TSource>, IAsyncObservable<TDuration>> durationSelector, int capacity)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            return GroupByUntil(observer, subscription, keySelector, durationSelector, capacity, EqualityComparer<TKey>.Default);
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> GroupByUntil<TSource, TKey, TDuration>(IAsyncObserver<IGroupedAsyncObservable<TKey, TSource>> observer, IAsyncDisposable subscription, Func<TSource, Task<TKey>> keySelector, Func<IGroupedAsyncObservable<TKey, TSource>, IAsyncObservable<TDuration>> durationSelector, int capacity, IEqualityComparer<TKey> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return GroupByUntil(observer, subscription, keySelector, x => Task.FromResult(x), durationSelector, capacity, comparer);
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> GroupByUntil<TSource, TKey, TElement, TDuration>(IAsyncObserver<IGroupedAsyncObservable<TKey, TElement>> observer, IAsyncDisposable subscription, Func<TSource, Task<TKey>> keySelector, Func<TSource, Task<TElement>> elementSelector, Func<IGroupedAsyncObservable<TKey, TElement>, IAsyncObservable<TDuration>> durationSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));

            return GroupByUntil(observer, subscription, keySelector, elementSelector, durationSelector, int.MaxValue, EqualityComparer<TKey>.Default);
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> GroupByUntil<TSource, TKey, TElement, TDuration>(IAsyncObserver<IGroupedAsyncObservable<TKey, TElement>> observer, IAsyncDisposable subscription, Func<TSource, Task<TKey>> keySelector, Func<TSource, Task<TElement>> elementSelector, Func<IGroupedAsyncObservable<TKey, TElement>, IAsyncObservable<TDuration>> durationSelector, IEqualityComparer<TKey> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return GroupByUntil(observer, subscription, keySelector, elementSelector, durationSelector, int.MaxValue, comparer);
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> GroupByUntil<TSource, TKey, TElement, TDuration>(IAsyncObserver<IGroupedAsyncObservable<TKey, TElement>> observer, IAsyncDisposable subscription, Func<TSource, Task<TKey>> keySelector, Func<TSource, Task<TElement>> elementSelector, Func<IGroupedAsyncObservable<TKey, TElement>, IAsyncObservable<TDuration>> durationSelector, int capacity)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            return GroupByUntil(observer, subscription, keySelector, elementSelector, durationSelector, capacity, EqualityComparer<TKey>.Default);
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> GroupByUntil<TSource, TKey, TElement, TDuration>(IAsyncObserver<IGroupedAsyncObservable<TKey, TElement>> observer, IAsyncDisposable subscription, Func<TSource, Task<TKey>> keySelector, Func<TSource, Task<TElement>> elementSelector, Func<IGroupedAsyncObservable<TKey, TElement>, IAsyncObservable<TDuration>> durationSelector, int capacity, IEqualityComparer<TKey> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (durationSelector == null)
                throw new ArgumentNullException(nameof(durationSelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return CoreAsync();

            // REVIEW: Concurrent execution of a duration callback and an event could lead to an OnNext call being queued in an AsyncLockObserver
            //         after a duration callback makes an OnCompleted call. This seems to be the case in sync Rx as well.

            async Task<(IAsyncObserver<TSource>, IAsyncDisposable)> CoreAsync()
            {
                var d = new CompositeAsyncDisposable();

                await d.AddAsync(subscription).ConfigureAwait(false);

                var refCount = new RefCountAsyncDisposable(d);

                var groups = default(ConcurrentDictionary<TKey, IAsyncSubject<TElement>>);

                if (capacity == int.MaxValue)
                {
                    groups = new ConcurrentDictionary<TKey, IAsyncSubject<TElement>>(comparer);
                }
                else
                {
                    groups = new ConcurrentDictionary<TKey, IAsyncSubject<TElement>>(Environment.ProcessorCount * 4, capacity, comparer);
                }

                var gate = new AsyncLock();

                var nullGate = new object();
                var nullGroup = default(IAsyncSubject<TElement>);

                async Task OnErrorAsync(Exception ex)
                {
                    var nullGroupLocal = default(IAsyncSubject<TElement>);

                    lock (nullGate)
                    {
                        nullGroupLocal = nullGroup;
                    }

                    if (nullGroupLocal != null)
                    {
                        await nullGroupLocal.OnErrorAsync(ex).ConfigureAwait(false);
                    }

                    foreach (var group in groups.Values)
                    {
                        await group.OnErrorAsync(ex).ConfigureAwait(false);
                    }

                    using (await gate.LockAsync().ConfigureAwait(false))
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    }
                }

                return
                    (
                        Create<TSource>
                        (
                            async x =>
                            {
                                var key = default(TKey);

                                try
                                {
                                    key = await keySelector(x).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                var shouldEmit = false;
                                var group = default(IAsyncSubject<TElement>);

                                if (key == null)
                                {
                                    lock (nullGate)
                                    {
                                        if (nullGroup == null)
                                        {
                                            var subject = new SequentialSimpleAsyncSubject<TElement>();
                                            nullGroup = AsyncSubject.Create(new AsyncQueueLockAsyncObserver<TElement>(subject), subject);
                                            shouldEmit = true;
                                        }
                                    }

                                    group = nullGroup;
                                }
                                else
                                {
                                    try
                                    {
                                        if (!groups.TryGetValue(key, out group))
                                        {
                                            var subject = new SequentialSimpleAsyncSubject<TElement>();
                                            group = AsyncSubject.Create(new AsyncQueueLockAsyncObserver<TElement>(subject), subject);

                                            if (groups.TryAdd(key, group))
                                            {
                                                shouldEmit = true;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        await OnErrorAsync(ex).ConfigureAwait(false);
                                        return;
                                    }
                                }

                                if (shouldEmit)
                                {
                                    var g = new GroupedAsyncObservable<TKey, TElement>(key, group, refCount);

                                    var duration = default(IAsyncObservable<TDuration>);

                                    try
                                    {
                                        duration = durationSelector(g);
                                    }
                                    catch (Exception ex)
                                    {
                                        await OnErrorAsync(ex).ConfigureAwait(false);
                                        return;
                                    }

                                    using (await gate.LockAsync().ConfigureAwait(false))
                                    {
                                        await observer.OnNextAsync(g).ConfigureAwait(false);
                                    }

                                    var durationSubscription = new SingleAssignmentAsyncDisposable();

                                    async Task Expire()
                                    {
                                        if (key == null)
                                        {
                                            var oldNullGroup = default(IAsyncSubject<TElement>);

                                            lock (nullGate)
                                            {
                                                oldNullGroup = nullGroup;
                                                nullGroup = null;
                                            }

                                            if (oldNullGroup != null)
                                            {
                                                await oldNullGroup.OnCompletedAsync().ConfigureAwait(false);
                                            }
                                        }
                                        else
                                        {
                                            if (groups.TryRemove(key, out var oldGroup))
                                            {
                                                await oldGroup.OnCompletedAsync().ConfigureAwait(false);
                                            }
                                        }

                                        await durationSubscription.DisposeAsync().ConfigureAwait(false);
                                        await d.RemoveAsync(durationSubscription).ConfigureAwait(false);
                                    }

                                    var durationObserver = Create<TDuration>(
                                        y => Expire(),
                                        OnErrorAsync,
                                        Expire
                                    );

                                    await d.AddAsync(durationSubscription).ConfigureAwait(false);
                                    var durationSubscriptionInner = await duration.SubscribeSafeAsync(durationObserver).ConfigureAwait(false);
                                    await durationSubscription.AssignAsync(durationSubscriptionInner).ConfigureAwait(false);
                                }

                                var element = default(TElement);

                                try
                                {
                                    element = await elementSelector(x).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await group.OnNextAsync(element).ConfigureAwait(false);
                            },
                            OnErrorAsync,
                            async () =>
                            {
                                if (nullGroup != null)
                                {
                                    await nullGroup.OnCompletedAsync().ConfigureAwait(false);
                                }

                                foreach (var group in groups.Values)
                                {
                                    await group.OnCompletedAsync().ConfigureAwait(false);
                                }

                                using (await gate.LockAsync().ConfigureAwait(false))
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
                            }
                        ),
                        refCount
                    );
            }
        }
    }
}
