// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return CreateAsyncObservable<IGroupedAsyncObservable<TKey, TSource>>.From(
                source,
                keySelector,
                static (source, keySelector, observer) => GroupByCore(source, observer, (o, d) => AsyncObserver.GroupBy(o, d, keySelector)));
        }

        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return CreateAsyncObservable<IGroupedAsyncObservable<TKey, TSource>>.From(
                source,
                (keySelector, comparer),
                static (source, state, observer) => GroupByCore(source, observer, (o, d) => AsyncObserver.GroupBy(o, d, state.keySelector, state.comparer)));
        }

        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector, int capacity)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            return CreateAsyncObservable<IGroupedAsyncObservable<TKey, TSource>>.From(
                source,
                (keySelector, capacity),
                static (source, state, observer) => GroupByCore(source, observer, (o, d) => AsyncObserver.GroupBy(o, d, state.keySelector, state.capacity)));
        }

        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector, int capacity, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return CreateAsyncObservable<IGroupedAsyncObservable<TKey, TSource>>.From(
                source,
                (keySelector, capacity, comparer),
                static (source, state, observer) => GroupByCore(source, observer, (o, d) => AsyncObserver.GroupBy(o, d, state.keySelector, state.capacity, state.comparer)));
        }

        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return CreateAsyncObservable<IGroupedAsyncObservable<TKey, TElement>>.From(
                source,
                (keySelector, elementSelector),
                static (source, state, observer) => GroupByCore(source, observer, (o, d) => AsyncObserver.GroupBy(o, d, state.keySelector, state.elementSelector)));
        }

        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return CreateAsyncObservable<IGroupedAsyncObservable<TKey, TElement>>.From(
                source,
                (keySelector, elementSelector, comparer),
                static (source, state, observer) => GroupByCore(source, observer, (o, d) => AsyncObserver.GroupBy(o, d, state.keySelector, state.elementSelector, state.comparer)));
        }

        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, int capacity)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            return CreateAsyncObservable<IGroupedAsyncObservable<TKey, TElement>>.From(
                source,
                (keySelector, elementSelector, capacity),
                static (source, state, observer) => GroupByCore(source, observer, (o, d) => AsyncObserver.GroupBy(o, d, state.keySelector, state.elementSelector, state.capacity)));
        }

        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, int capacity, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return CreateAsyncObservable<IGroupedAsyncObservable<TKey, TElement>>.From(
                source,
                (keySelector, elementSelector, capacity, comparer),
                static (source, state, observer) => GroupByCore(source, observer, (o, d) => AsyncObserver.GroupBy(o, d, state.keySelector, state.elementSelector, state.capacity, state.comparer)));
        }

        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return CreateAsyncObservable<IGroupedAsyncObservable<TKey, TSource>>.From(
                source,
                keySelector,
                static (source, keySelector, observer) => GroupByCore(source, observer, (o, d) => AsyncObserver.GroupBy(o, d, keySelector)));
        }

        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return CreateAsyncObservable<IGroupedAsyncObservable<TKey, TSource>>.From(
                source,
                (keySelector, comparer),
                static (source, state, observer) => GroupByCore(source, observer, (o, d) => AsyncObserver.GroupBy(o, d, state.keySelector, state.comparer)));
        }

        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, int capacity)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            return CreateAsyncObservable<IGroupedAsyncObservable<TKey, TSource>>.From(
                source,
                (keySelector, capacity),
                static (source, state, observer) => GroupByCore(source, observer, (o, d) => AsyncObserver.GroupBy(o, d, state.keySelector, state.capacity)));
        }

        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, int capacity, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return CreateAsyncObservable<IGroupedAsyncObservable<TKey, TSource>>.From(
                source,
                (keySelector, capacity, comparer),
                static (source, state, observer) => GroupByCore(source, observer, (o, d) => AsyncObserver.GroupBy(o, d, state.keySelector, state.capacity, state.comparer)));
        }

        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TElement>> elementSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return CreateAsyncObservable<IGroupedAsyncObservable<TKey, TElement>>.From(
                source,
                (keySelector, elementSelector),
                static (source, state, observer) => GroupByCore(source, observer, (o, d) => AsyncObserver.GroupBy(o, d, state.keySelector, state.elementSelector)));
        }

        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return CreateAsyncObservable<IGroupedAsyncObservable<TKey, TElement>>.From(
                source,
                (keySelector, elementSelector, comparer),
                static (source, state, observer) => GroupByCore(source, observer, (o, d) => AsyncObserver.GroupBy(o, d, state.keySelector, state.elementSelector, state.comparer)));
        }

        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TElement>> elementSelector, int capacity)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            return CreateAsyncObservable<IGroupedAsyncObservable<TKey, TElement>>.From(
                source,
                (keySelector, elementSelector, capacity),
                static (source, state, observer) => GroupByCore(source, observer, (o, d) => AsyncObserver.GroupBy(o, d, state.keySelector, state.elementSelector, state.capacity)));
        }

        public static IAsyncObservable<IGroupedAsyncObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TElement>> elementSelector, int capacity, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return CreateAsyncObservable< IGroupedAsyncObservable<TKey, TElement>>.From(
                source,
                (keySelector, elementSelector, capacity, comparer),
                static (source, state, observer) => GroupByCore(source, observer, (o, d) => AsyncObserver.GroupBy(o, d, state.keySelector, state.elementSelector, state.capacity, state.comparer)));
        }

        private static async ValueTask<IAsyncDisposable> GroupByCore<TSource, TKey, TElement>(IAsyncObservable<TSource> source, IAsyncObserver<IGroupedAsyncObservable<TKey, TElement>> observer, Func<IAsyncObserver<IGroupedAsyncObservable<TKey, TElement>>, IAsyncDisposable, (IAsyncObserver<TSource>, IAsyncDisposable)> createObserver)
        {
            var d = new SingleAssignmentAsyncDisposable();

            var (sink, subscription) = createObserver(observer, d);

            var inner = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);
            await d.AssignAsync(inner).ConfigureAwait(false);

            return subscription;
        }
    }

    public partial class AsyncObserver
    {
        public static (IAsyncObserver<TSource>, IAsyncDisposable) GroupBy<TSource, TKey>(IAsyncObserver<IGroupedAsyncObservable<TKey, TSource>> observer, IAsyncDisposable subscription, Func<TSource, TKey> keySelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return GroupBy(observer, subscription, keySelector, int.MaxValue, EqualityComparer<TKey>.Default);
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) GroupBy<TSource, TKey>(IAsyncObserver<IGroupedAsyncObservable<TKey, TSource>> observer, IAsyncDisposable subscription, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return GroupBy(observer, subscription, keySelector, int.MaxValue, comparer);
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) GroupBy<TSource, TKey>(IAsyncObserver<IGroupedAsyncObservable<TKey, TSource>> observer, IAsyncDisposable subscription, Func<TSource, TKey> keySelector, int capacity)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            return GroupBy(observer, subscription, keySelector, capacity, EqualityComparer<TKey>.Default);
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) GroupBy<TSource, TKey>(IAsyncObserver<IGroupedAsyncObservable<TKey, TSource>> observer, IAsyncDisposable subscription, Func<TSource, TKey> keySelector, int capacity, IEqualityComparer<TKey> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return GroupBy(observer, subscription, x => new ValueTask<TKey>(keySelector(x)), capacity, comparer);
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) GroupBy<TSource, TKey, TElement>(IAsyncObserver<IGroupedAsyncObservable<TKey, TElement>> observer, IAsyncDisposable subscription, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return GroupBy(observer, subscription, keySelector, elementSelector, int.MaxValue, EqualityComparer<TKey>.Default);
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) GroupBy<TSource, TKey, TElement>(IAsyncObserver<IGroupedAsyncObservable<TKey, TElement>> observer, IAsyncDisposable subscription, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return GroupBy(observer, subscription, keySelector, elementSelector, int.MaxValue, comparer);
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) GroupBy<TSource, TKey, TElement>(IAsyncObserver<IGroupedAsyncObservable<TKey, TElement>> observer, IAsyncDisposable subscription, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, int capacity)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            return GroupBy(observer, subscription, keySelector, elementSelector, capacity, EqualityComparer<TKey>.Default);
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) GroupBy<TSource, TKey, TElement>(IAsyncObserver<IGroupedAsyncObservable<TKey, TElement>> observer, IAsyncDisposable subscription, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, int capacity, IEqualityComparer<TKey> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return GroupBy<TSource, TKey, TElement>(observer, subscription, x => new ValueTask<TKey>(keySelector(x)), x => new ValueTask<TElement>(elementSelector(x)), capacity, comparer);
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) GroupBy<TSource, TKey>(IAsyncObserver<IGroupedAsyncObservable<TKey, TSource>> observer, IAsyncDisposable subscription, Func<TSource, ValueTask<TKey>> keySelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return GroupBy(observer, subscription, keySelector, int.MaxValue, EqualityComparer<TKey>.Default);
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) GroupBy<TSource, TKey>(IAsyncObserver<IGroupedAsyncObservable<TKey, TSource>> observer, IAsyncDisposable subscription, Func<TSource, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return GroupBy(observer, subscription, keySelector, int.MaxValue, comparer);
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) GroupBy<TSource, TKey>(IAsyncObserver<IGroupedAsyncObservable<TKey, TSource>> observer, IAsyncDisposable subscription, Func<TSource, ValueTask<TKey>> keySelector, int capacity)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            return GroupBy(observer, subscription, keySelector, capacity, EqualityComparer<TKey>.Default);
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) GroupBy<TSource, TKey>(IAsyncObserver<IGroupedAsyncObservable<TKey, TSource>> observer, IAsyncDisposable subscription, Func<TSource, ValueTask<TKey>> keySelector, int capacity, IEqualityComparer<TKey> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return GroupBy(observer, subscription, keySelector, x => new ValueTask<TSource>(x), capacity, comparer);
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) GroupBy<TSource, TKey, TElement>(IAsyncObserver<IGroupedAsyncObservable<TKey, TElement>> observer, IAsyncDisposable subscription, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TElement>> elementSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return GroupBy(observer, subscription, keySelector, elementSelector, int.MaxValue, EqualityComparer<TKey>.Default);
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) GroupBy<TSource, TKey, TElement>(IAsyncObserver<IGroupedAsyncObservable<TKey, TElement>> observer, IAsyncDisposable subscription, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return GroupBy(observer, subscription, keySelector, elementSelector, int.MaxValue, comparer);
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) GroupBy<TSource, TKey, TElement>(IAsyncObserver<IGroupedAsyncObservable<TKey, TElement>> observer, IAsyncDisposable subscription, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TElement>> elementSelector, int capacity)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            return GroupBy(observer, subscription, keySelector, elementSelector, capacity, EqualityComparer<TKey>.Default);
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) GroupBy<TSource, TKey, TElement>(IAsyncObserver<IGroupedAsyncObservable<TKey, TElement>> observer, IAsyncDisposable subscription, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TElement>> elementSelector, int capacity, IEqualityComparer<TKey> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            var refCount = new RefCountAsyncDisposable(subscription);

            var groups = default(Dictionary<TKey, SequentialSimpleAsyncSubject<TElement>>);

            if (capacity == int.MaxValue)
            {
                groups = new Dictionary<TKey, SequentialSimpleAsyncSubject<TElement>>(comparer);
            }
            else
            {
                groups = new Dictionary<TKey, SequentialSimpleAsyncSubject<TElement>>(capacity, comparer);
            }

            var nullGroup = default(SequentialSimpleAsyncSubject<TElement>);

            async ValueTask OnErrorAsync(Exception ex)
            {
                if (nullGroup != null)
                {
                    await nullGroup.OnErrorAsync(ex).ConfigureAwait(false);
                }

                foreach (var group in groups.Values)
                {
                    await group.OnErrorAsync(ex).ConfigureAwait(false);
                }

                await observer.OnErrorAsync(ex).ConfigureAwait(false);
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
                            var group = default(SequentialSimpleAsyncSubject<TElement>);

                            if (key == null)
                            {
                                if (nullGroup == null)
                                {
                                    nullGroup = new SequentialSimpleAsyncSubject<TElement>();
                                    shouldEmit = true;
                                }

                                group = nullGroup;
                            }
                            else
                            {
                                try
                                {
                                    if (!groups.TryGetValue(key, out group))
                                    {
                                        group = new SequentialSimpleAsyncSubject<TElement>();
                                        groups.Add(key, group);
                                        shouldEmit = true;
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
                                await observer.OnNextAsync(g).ConfigureAwait(false);
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

                            await observer.OnCompletedAsync().ConfigureAwait(false);
                        }
                    ),
                    refCount
                );
        }
    }
}
