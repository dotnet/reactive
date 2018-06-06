// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    using ObservableImpl;

    internal partial class QueryLanguage
    {
        #region + Cast +

        public virtual IObservable<TResult> Cast<TResult>(IObservable<object> source)
        {
            return new Cast<object, TResult>(source);
        }

        #endregion

        #region + DefaultIfEmpty +

        public virtual IObservable<TSource> DefaultIfEmpty<TSource>(IObservable<TSource> source)
        {
            return new DefaultIfEmpty<TSource>(source, default(TSource));
        }

        public virtual IObservable<TSource> DefaultIfEmpty<TSource>(IObservable<TSource> source, TSource defaultValue)
        {
            return new DefaultIfEmpty<TSource>(source, defaultValue);
        }

        #endregion

        #region + Distinct +

        public virtual IObservable<TSource> Distinct<TSource>(IObservable<TSource> source)
        {
            return new Distinct<TSource, TSource>(source, x => x, EqualityComparer<TSource>.Default);
        }

        public virtual IObservable<TSource> Distinct<TSource>(IObservable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            return new Distinct<TSource, TSource>(source, x => x, comparer);
        }

        public virtual IObservable<TSource> Distinct<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return new Distinct<TSource, TKey>(source, keySelector, EqualityComparer<TKey>.Default);
        }

        public virtual IObservable<TSource> Distinct<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return new Distinct<TSource, TKey>(source, keySelector, comparer);
        }

        #endregion

        #region + GroupBy +

        public virtual IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            return GroupBy_<TSource, TKey, TElement>(source, keySelector, elementSelector, null, EqualityComparer<TKey>.Default);
        }

        public virtual IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return GroupBy_<TSource, TKey, TSource>(source, keySelector, x => x, null, comparer);
        }

        public virtual IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return GroupBy_<TSource, TKey, TSource>(source, keySelector, x => x, null, EqualityComparer<TKey>.Default);
        }

        public virtual IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            return GroupBy_<TSource, TKey, TElement>(source, keySelector, elementSelector, null, comparer);
        }

        public virtual IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, int capacity)
        {
            return GroupBy_<TSource, TKey, TElement>(source, keySelector, elementSelector, capacity, EqualityComparer<TKey>.Default);
        }

        public virtual IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector, int capacity, IEqualityComparer<TKey> comparer)
        {
            return GroupBy_<TSource, TKey, TSource>(source, keySelector, x => x, capacity, comparer);
        }

        public virtual IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector, int capacity)
        {
            return GroupBy_<TSource, TKey, TSource>(source, keySelector, x => x, capacity, EqualityComparer<TKey>.Default);
        }

        public virtual IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, int capacity, IEqualityComparer<TKey> comparer)
        {
            return GroupBy_<TSource, TKey, TElement>(source, keySelector, elementSelector, capacity, comparer);
        }

        private static IObservable<IGroupedObservable<TKey, TElement>> GroupBy_<TSource, TKey, TElement>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, int? capacity, IEqualityComparer<TKey> comparer)
        {
            return new GroupBy<TSource, TKey, TElement>(source, keySelector, elementSelector, capacity, comparer);
        }

        #endregion

        #region + GroupByUntil +

        public virtual IObservable<IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector, IEqualityComparer<TKey> comparer)
        {
            return GroupByUntil_<TSource, TKey, TElement, TDuration>(source, keySelector, elementSelector, durationSelector, null, comparer);
        }

        public virtual IObservable<IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector)
        {
            return GroupByUntil_<TSource, TKey, TElement, TDuration>(source, keySelector, elementSelector, durationSelector, null, EqualityComparer<TKey>.Default);
        }

        public virtual IObservable<IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<IGroupedObservable<TKey, TSource>, IObservable<TDuration>> durationSelector, IEqualityComparer<TKey> comparer)
        {
            return GroupByUntil_<TSource, TKey, TSource, TDuration>(source, keySelector, x => x, durationSelector, null, comparer);
        }

        public virtual IObservable<IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<IGroupedObservable<TKey, TSource>, IObservable<TDuration>> durationSelector)
        {
            return GroupByUntil_<TSource, TKey, TSource, TDuration>(source, keySelector, x => x, durationSelector, null, EqualityComparer<TKey>.Default);
        }

        public virtual IObservable<IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector, int capacity, IEqualityComparer<TKey> comparer)
        {
            return GroupByUntil_<TSource, TKey, TElement, TDuration>(source, keySelector, elementSelector, durationSelector, capacity, comparer);
        }

        public virtual IObservable<IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector, int capacity)
        {
            return GroupByUntil_<TSource, TKey, TElement, TDuration>(source, keySelector, elementSelector, durationSelector, capacity, EqualityComparer<TKey>.Default);
        }

        public virtual IObservable<IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<IGroupedObservable<TKey, TSource>, IObservable<TDuration>> durationSelector, int capacity, IEqualityComparer<TKey> comparer)
        {
            return GroupByUntil_<TSource, TKey, TSource, TDuration>(source, keySelector, x => x, durationSelector, capacity, comparer);
        }

        public virtual IObservable<IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<IGroupedObservable<TKey, TSource>, IObservable<TDuration>> durationSelector, int capacity)
        {
            return GroupByUntil_<TSource, TKey, TSource, TDuration>(source, keySelector, x => x, durationSelector, capacity, EqualityComparer<TKey>.Default);
        }

        private static IObservable<IGroupedObservable<TKey, TElement>> GroupByUntil_<TSource, TKey, TElement, TDuration>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector, int? capacity, IEqualityComparer<TKey> comparer)
        {
            return new GroupByUntil<TSource, TKey, TElement, TDuration>(source, keySelector, elementSelector, durationSelector, capacity, comparer);
        }

        #endregion

        #region + GroupJoin +

        public virtual IObservable<TResult> GroupJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(IObservable<TLeft> left, IObservable<TRight> right, Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector, Func<TRight, IObservable<TRightDuration>> rightDurationSelector, Func<TLeft, IObservable<TRight>, TResult> resultSelector)
        {
            return GroupJoin_<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(left, right, leftDurationSelector, rightDurationSelector, resultSelector);
        }

        private static IObservable<TResult> GroupJoin_<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(IObservable<TLeft> left, IObservable<TRight> right, Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector, Func<TRight, IObservable<TRightDuration>> rightDurationSelector, Func<TLeft, IObservable<TRight>, TResult> resultSelector)
        {
            return new GroupJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(left, right, leftDurationSelector, rightDurationSelector, resultSelector);
        }

        #endregion

        #region + Join +

        public virtual IObservable<TResult> Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(IObservable<TLeft> left, IObservable<TRight> right, Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector, Func<TRight, IObservable<TRightDuration>> rightDurationSelector, Func<TLeft, TRight, TResult> resultSelector)
        {
            return Join_<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(left, right, leftDurationSelector, rightDurationSelector, resultSelector);
        }

        private static IObservable<TResult> Join_<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(IObservable<TLeft> left, IObservable<TRight> right, Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector, Func<TRight, IObservable<TRightDuration>> rightDurationSelector, Func<TLeft, TRight, TResult> resultSelector)
        {
            return new Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(left, right, leftDurationSelector, rightDurationSelector, resultSelector);
        }

        #endregion

        #region + OfType +

        public virtual IObservable<TResult> OfType<TResult>(IObservable<object> source)
        {
            return new OfType<object, TResult>(source);
        }

        #endregion

        #region + Select +

        public virtual IObservable<TResult> Select<TSource, TResult>(IObservable<TSource> source, Func<TSource, TResult> selector)
        {
            // CONSIDER: Add fusion for Select/Select pairs.

            return new Select<TSource, TResult>.Selector(source, selector);
        }

        public virtual IObservable<TResult> Select<TSource, TResult>(IObservable<TSource> source, Func<TSource, int, TResult> selector)
        {
            return new Select<TSource, TResult>.SelectorIndexed(source, selector);
        }

        #endregion

        #region + SelectMany +

        public virtual IObservable<TOther> SelectMany<TSource, TOther>(IObservable<TSource> source, IObservable<TOther> other)
        {
            return SelectMany_<TSource, TOther>(source, _ => other);
        }

        public virtual IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, IObservable<TResult>> selector)
        {
            return SelectMany_<TSource, TResult>(source, selector);
        }

        public virtual IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, int, IObservable<TResult>> selector)
        {
            return SelectMany_<TSource, TResult>(source, selector);
        }

        public virtual IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, Task<TResult>> selector)
        {
            return new SelectMany<TSource, TResult>.TaskSelector(source, (x, token) => selector(x));
        }

        public virtual IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, int, Task<TResult>> selector)
        {
            return new SelectMany<TSource, TResult>.TaskSelectorIndexed(source, (x, i, token) => selector(x, i));
        }

        public virtual IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, CancellationToken, Task<TResult>> selector)
        {
            return new SelectMany<TSource, TResult>.TaskSelector(source, selector);
        }

        public virtual IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, int, CancellationToken, Task<TResult>> selector)
        {
            return new SelectMany<TSource, TResult>.TaskSelectorIndexed(source, selector);
        }

        public virtual IObservable<TResult> SelectMany<TSource, TCollection, TResult>(IObservable<TSource> source, Func<TSource, IObservable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            return SelectMany_<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        }

        public virtual IObservable<TResult> SelectMany<TSource, TCollection, TResult>(IObservable<TSource> source, Func<TSource, int, IObservable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector)
        {
            return SelectMany_<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        }

        public virtual IObservable<TResult> SelectMany<TSource, TTaskResult, TResult>(IObservable<TSource> source, Func<TSource, Task<TTaskResult>> taskSelector, Func<TSource, TTaskResult, TResult> resultSelector)
        {
            return new SelectMany<TSource, TTaskResult, TResult>.TaskSelector(source, (x, token) => taskSelector(x), resultSelector);
        }

        public virtual IObservable<TResult> SelectMany<TSource, TTaskResult, TResult>(IObservable<TSource> source, Func<TSource, int, Task<TTaskResult>> taskSelector, Func<TSource, int, TTaskResult, TResult> resultSelector)
        {
            return new SelectMany<TSource, TTaskResult, TResult>.TaskSelectorIndexed(source, (x, i, token) => taskSelector(x, i), resultSelector);
        }

        public virtual IObservable<TResult> SelectMany<TSource, TTaskResult, TResult>(IObservable<TSource> source, Func<TSource, CancellationToken, Task<TTaskResult>> taskSelector, Func<TSource, TTaskResult, TResult> resultSelector)
        {
            return new SelectMany<TSource, TTaskResult, TResult>.TaskSelector(source, taskSelector, resultSelector);
        }

        public virtual IObservable<TResult> SelectMany<TSource, TTaskResult, TResult>(IObservable<TSource> source, Func<TSource, int, CancellationToken, Task<TTaskResult>> taskSelector, Func<TSource, int, TTaskResult, TResult> resultSelector)
        {
            return new SelectMany<TSource, TTaskResult, TResult>.TaskSelectorIndexed(source, taskSelector, resultSelector);
        }

        private static IObservable<TResult> SelectMany_<TSource, TResult>(IObservable<TSource> source, Func<TSource, IObservable<TResult>> selector)
        {
            return new SelectMany<TSource, TResult>.ObservableSelector(source, selector);
        }
        
        private static IObservable<TResult> SelectMany_<TSource, TResult>(IObservable<TSource> source, Func<TSource, int, IObservable<TResult>> selector)
        {
            return new SelectMany<TSource, TResult>.ObservableSelectorIndexed(source, selector);
        }

        private static IObservable<TResult> SelectMany_<TSource, TCollection, TResult>(IObservable<TSource> source, Func<TSource, IObservable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            return new SelectMany<TSource, TCollection, TResult>.ObservableSelector(source, collectionSelector, resultSelector);
        }

        private static IObservable<TResult> SelectMany_<TSource, TCollection, TResult>(IObservable<TSource> source, Func<TSource, int, IObservable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector)
        {
            return new SelectMany<TSource, TCollection, TResult>.ObservableSelectorIndexed(source, collectionSelector, resultSelector);
        }

        public virtual IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, IObservable<TResult>> onNext, Func<Exception, IObservable<TResult>> onError, Func<IObservable<TResult>> onCompleted)
        {
            return new SelectMany<TSource, TResult>.ObservableSelectors(source, onNext, onError, onCompleted);
        }

        public virtual IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, int, IObservable<TResult>> onNext, Func<Exception, IObservable<TResult>> onError, Func<IObservable<TResult>> onCompleted)
        {
            return new SelectMany<TSource, TResult>.ObservableSelectorsIndexed(source, onNext, onError, onCompleted);
        }

        public virtual IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
        {
            return new SelectMany<TSource, TResult>.EnumerableSelector(source, selector);
        }

        public virtual IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector)
        {
            return new SelectMany<TSource, TResult>.EnumerableSelectorIndexed(source, selector);
        }

        public virtual IObservable<TResult> SelectMany<TSource, TCollection, TResult>(IObservable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            return SelectMany_<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        }

        public virtual IObservable<TResult> SelectMany<TSource, TCollection, TResult>(IObservable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector)
        {
            return SelectMany_<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        }

        private static IObservable<TResult> SelectMany_<TSource, TCollection, TResult>(IObservable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            return new SelectMany<TSource, TCollection, TResult>.EnumerableSelector(source, collectionSelector, resultSelector);
        }

        private static IObservable<TResult> SelectMany_<TSource, TCollection, TResult>(IObservable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector)
        {
            return new SelectMany<TSource, TCollection, TResult>.EnumerableSelectorIndexed(source, collectionSelector, resultSelector);
        }

        #endregion

        #region + Skip +

        public virtual IObservable<TSource> Skip<TSource>(IObservable<TSource> source, int count)
        {
            if (source is Skip<TSource>.Count skip)
                return skip.Combine(count);

            return new Skip<TSource>.Count(source, count);
        }

        #endregion

        #region + SkipWhile +

        public virtual IObservable<TSource> SkipWhile<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            return new SkipWhile<TSource>.Predicate(source, predicate);
        }

        public virtual IObservable<TSource> SkipWhile<TSource>(IObservable<TSource> source, Func<TSource, int, bool> predicate)
        {
            return new SkipWhile<TSource>.PredicateIndexed(source, predicate);
        }

        #endregion

        #region + Take +

        public virtual IObservable<TSource> Take<TSource>(IObservable<TSource> source, int count)
        {
            if (count == 0)
                return Empty<TSource>();

            return Take_(source, count);
        }

        public virtual IObservable<TSource> Take<TSource>(IObservable<TSource> source, int count, IScheduler scheduler)
        {
            if (count == 0)
                return Empty<TSource>(scheduler);

            return Take_(source, count);
        }

        private static IObservable<TSource> Take_<TSource>(IObservable<TSource> source, int count)
        {
            if (source is Take<TSource>.Count take)
                return take.Combine(count);

            return new Take<TSource>.Count(source, count);
        }

        #endregion

        #region + TakeWhile +

        public virtual IObservable<TSource> TakeWhile<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            return new TakeWhile<TSource>.Predicate(source, predicate);
        }

        public virtual IObservable<TSource> TakeWhile<TSource>(IObservable<TSource> source, Func<TSource, int, bool> predicate)
        {
            return new TakeWhile<TSource>.PredicateIndexed(source, predicate);
        }

        #endregion

        #region + Where +

        public virtual IObservable<TSource> Where<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source is Where<TSource>.Predicate where)
                return where.Combine(predicate);

            return new Where<TSource>.Predicate(source, predicate);
        }

        public virtual IObservable<TSource> Where<TSource>(IObservable<TSource> source, Func<TSource, int, bool> predicate)
        {
            return new Where<TSource>.PredicateIndexed(source, predicate);
        }

        #endregion
    }
}
