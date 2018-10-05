// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;

namespace System.Reactive.Linq
{
    using ObservableImpl;

    internal partial class QueryLanguage
    {
        #region - Append -

        public virtual IObservable<TSource> Append<TSource>(IObservable<TSource> source, TSource value)
        {
            return Append_(source, value, SchedulerDefaults.ConstantTimeOperations);
        }

        public virtual IObservable<TSource> Append<TSource>(IObservable<TSource> source, TSource value, IScheduler scheduler)
        {
            return Append_(source, value, scheduler);
        }

        private static IObservable<TSource> Append_<TSource>(IObservable<TSource> source, TSource value, IScheduler scheduler)
        {
            if (source is AppendPrepend<TSource>.IAppendPrepend ap && ap.Scheduler == scheduler)
            {
                return ap.Append(value);
            }
            if (scheduler == ImmediateScheduler.Instance)
            {
                return new AppendPrepend<TSource>.SingleImmediate(source, value, append: true);
            }
            return new AppendPrepend<TSource>.SingleValue(source, value, scheduler, append: true);
        }

        #endregion

        #region + AsObservable +

        public virtual IObservable<TSource> AsObservable<TSource>(IObservable<TSource> source)
        {
            if (source is AsObservable<TSource> asObservable)
            {
                return asObservable;
            }

            return new AsObservable<TSource>(source);
        }

        #endregion

        #region + Buffer +

        public virtual IObservable<IList<TSource>> Buffer<TSource>(IObservable<TSource> source, int count)
        {
            return new Buffer<TSource>.CountExact(source, count);
        }

        public virtual IObservable<IList<TSource>> Buffer<TSource>(IObservable<TSource> source, int count, int skip)
        {
            if (count > skip)
            {
                return new Buffer<TSource>.CountOverlap(source, count, skip);
            }

            if (count < skip)
            {
                return new Buffer<TSource>.CountSkip(source, count, skip);
            }
            // count == skip
            return new Buffer<TSource>.CountExact(source, count);
        }

        #endregion

        #region + Dematerialize +

        public virtual IObservable<TSource> Dematerialize<TSource>(IObservable<Notification<TSource>> source)
        {
            if (source is Materialize<TSource> materialize)
            {
                return materialize.Dematerialize();
            }

            return new Dematerialize<TSource>(source);
        }

        #endregion

        #region + DistinctUntilChanged +

        public virtual IObservable<TSource> DistinctUntilChanged<TSource>(IObservable<TSource> source)
        {
            return DistinctUntilChanged_(source, x => x, EqualityComparer<TSource>.Default);
        }

        public virtual IObservable<TSource> DistinctUntilChanged<TSource>(IObservable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            return DistinctUntilChanged_(source, x => x, comparer);
        }

        public virtual IObservable<TSource> DistinctUntilChanged<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return DistinctUntilChanged_(source, keySelector, EqualityComparer<TKey>.Default);
        }

        public virtual IObservable<TSource> DistinctUntilChanged<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return DistinctUntilChanged_(source, keySelector, comparer);
        }

        private static IObservable<TSource> DistinctUntilChanged_<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return new DistinctUntilChanged<TSource, TKey>(source, keySelector, comparer);
        }

        #endregion

        #region + Do +

        public virtual IObservable<TSource> Do<TSource>(IObservable<TSource> source, Action<TSource> onNext)
        {
            return new Do<TSource>.OnNext(source, onNext);
        }

        public virtual IObservable<TSource> Do<TSource>(IObservable<TSource> source, Action<TSource> onNext, Action onCompleted)
        {
            return Do_(source, onNext, Stubs<Exception>.Ignore, onCompleted);
        }

        public virtual IObservable<TSource> Do<TSource>(IObservable<TSource> source, Action<TSource> onNext, Action<Exception> onError)
        {
            return Do_(source, onNext, onError, Stubs.Nop);
        }

        public virtual IObservable<TSource> Do<TSource>(IObservable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
        {
            return Do_(source, onNext, onError, onCompleted);
        }

        public virtual IObservable<TSource> Do<TSource>(IObservable<TSource> source, IObserver<TSource> observer)
        {
            return new Do<TSource>.Observer(source, observer);
        }

        private static IObservable<TSource> Do_<TSource>(IObservable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
        {
            return new Do<TSource>.Actions(source, onNext, onError, onCompleted);
        }

        #endregion

        #region + Finally +

        public virtual IObservable<TSource> Finally<TSource>(IObservable<TSource> source, Action finallyAction)
        {
            return new Finally<TSource>(source, finallyAction);
        }

        #endregion

        #region + IgnoreElements +

        public virtual IObservable<TSource> IgnoreElements<TSource>(IObservable<TSource> source)
        {
            if (source is IgnoreElements<TSource> ignoreElements)
            {
                return ignoreElements;
            }

            return new IgnoreElements<TSource>(source);
        }

        #endregion

        #region + Materialize +

        public virtual IObservable<Notification<TSource>> Materialize<TSource>(IObservable<TSource> source)
        {
            //
            // NOTE: Peephole optimization of xs.Dematerialize().Materialize() should not be performed. It's possible for xs to
            //       contain multiple terminal notifications, which won't survive a Dematerialize().Materialize() chain. In case
            //       a reduction to xs.AsObservable() would be performed, those notification elements would survive.
            //

            return new Materialize<TSource>(source);
        }

        #endregion

        #region - Prepend -

        public virtual IObservable<TSource> Prepend<TSource>(IObservable<TSource> source, TSource value)
        {
            return Prepend_(source, value, SchedulerDefaults.ConstantTimeOperations);
        }

        public virtual IObservable<TSource> Prepend<TSource>(IObservable<TSource> source, TSource value, IScheduler scheduler)
        {
            return Prepend_(source, value, scheduler);
        }

        private static IObservable<TSource> Prepend_<TSource>(IObservable<TSource> source, TSource value, IScheduler scheduler)
        {
            if (source is AppendPrepend<TSource>.IAppendPrepend ap && ap.Scheduler == scheduler)
            {
                return ap.Prepend(value);
            }

            if (scheduler == ImmediateScheduler.Instance)
            {
                return new AppendPrepend<TSource>.SingleImmediate(source, value, append: false);
            }

            return new AppendPrepend<TSource>.SingleValue(source, value, scheduler, append: false);
        }

        #endregion

        #region - Repeat -

        public virtual IObservable<TSource> Repeat<TSource>(IObservable<TSource> source)
        {
            return RepeatInfinite(source).Concat();
        }

        private static IEnumerable<T> RepeatInfinite<T>(T value)
        {
            while (true)
            {
                yield return value;
            }
        }

        public virtual IObservable<TSource> Repeat<TSource>(IObservable<TSource> source, int repeatCount)
        {
            return Enumerable.Repeat(source, repeatCount).Concat();
        }

        public virtual IObservable<TSource> RepeatWhen<TSource, TSignal>(IObservable<TSource> source, Func<IObservable<object>, IObservable<TSignal>> handler)
        {
            return new RepeatWhen<TSource, TSignal>(source, handler);
        }

        #endregion

        #region - Retry -

        public virtual IObservable<TSource> Retry<TSource>(IObservable<TSource> source)
        {
            return RepeatInfinite(source).Catch();
        }

        public virtual IObservable<TSource> Retry<TSource>(IObservable<TSource> source, int retryCount)
        {
            return Enumerable.Repeat(source, retryCount).Catch();
        }

        public virtual IObservable<TSource> RetryWhen<TSource, TSignal>(IObservable<TSource> source, Func<IObservable<Exception>, IObservable<TSignal>> handler)
        {
            return new RetryWhen<TSource, TSignal>(source, handler);
        }


        #endregion

        #region + Scan +

        public virtual IObservable<TAccumulate> Scan<TSource, TAccumulate>(IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            return new Scan<TSource, TAccumulate>(source, seed, accumulator);
        }

        public virtual IObservable<TSource> Scan<TSource>(IObservable<TSource> source, Func<TSource, TSource, TSource> accumulator)
        {
            return new Scan<TSource>(source, accumulator);
        }

        #endregion

        #region + SkipLast +

        public virtual IObservable<TSource> SkipLast<TSource>(IObservable<TSource> source, int count)
        {
            return new SkipLast<TSource>.Count(source, count);
        }

        #endregion

        #region - StartWith -

        public virtual IObservable<TSource> StartWith<TSource>(IObservable<TSource> source, params TSource[] values)
        {
            return StartWith_(source, SchedulerDefaults.ConstantTimeOperations, values);
        }

        public virtual IObservable<TSource> StartWith<TSource>(IObservable<TSource> source, IScheduler scheduler, params TSource[] values)
        {
            return StartWith_(source, scheduler, values);
        }

        public virtual IObservable<TSource> StartWith<TSource>(IObservable<TSource> source, IEnumerable<TSource> values)
        {
            return StartWith(source, SchedulerDefaults.ConstantTimeOperations, values);
        }

        public virtual IObservable<TSource> StartWith<TSource>(IObservable<TSource> source, IScheduler scheduler, IEnumerable<TSource> values)
        {
            //
            // NOTE: For some reason, someone introduced this signature in the Observable class, which is inconsistent with the Rx pattern
            //       of putting the IScheduler last. It also wasn't wired up through IQueryLanguage. When introducing this method in the
            //       IQueryLanguage interface, we went for consistency with the public API, hence the odd position of the IScheduler.
            //

            if (!(values is TSource[] valueArray))
            {
                var valueList = new List<TSource>(values);
                valueArray = valueList.ToArray();
            }

            return StartWith_(source, scheduler, valueArray);
        }

        private static IObservable<TSource> StartWith_<TSource>(IObservable<TSource> source, IScheduler scheduler, params TSource[] values)
        {
            return values.ToObservable(scheduler).Concat(source);
        }

        #endregion

        #region + TakeLast +

        public virtual IObservable<TSource> TakeLast<TSource>(IObservable<TSource> source, int count)
        {
            return TakeLast_(source, count, SchedulerDefaults.Iteration);
        }

        public virtual IObservable<TSource> TakeLast<TSource>(IObservable<TSource> source, int count, IScheduler scheduler)
        {
            return TakeLast_(source, count, scheduler);
        }

        private static IObservable<TSource> TakeLast_<TSource>(IObservable<TSource> source, int count, IScheduler scheduler)
        {
            return new TakeLast<TSource>.Count(source, count, scheduler);
        }

        public virtual IObservable<IList<TSource>> TakeLastBuffer<TSource>(IObservable<TSource> source, int count)
        {
            return new TakeLastBuffer<TSource>.Count(source, count);
        }

        #endregion

        #region + Window +

        public virtual IObservable<IObservable<TSource>> Window<TSource>(IObservable<TSource> source, int count, int skip)
        {
            return Window_(source, count, skip);
        }

        public virtual IObservable<IObservable<TSource>> Window<TSource>(IObservable<TSource> source, int count)
        {
            return Window_(source, count, count);
        }

        private static IObservable<IObservable<TSource>> Window_<TSource>(IObservable<TSource> source, int count, int skip)
        {
            return new Window<TSource>.Count(source, count, skip);
        }

        #endregion
    }
}
