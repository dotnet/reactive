// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    using ObservableImpl;

    internal partial class QueryLanguage
    {
        #region + Amb +

        public virtual IObservable<TSource> Amb<TSource>(IObservable<TSource> first, IObservable<TSource> second)
        {
            return new Amb<TSource>(first, second);
        }

        public virtual IObservable<TSource> Amb<TSource>(params IObservable<TSource>[] sources)
        {
            return new AmbManyArray<TSource>(sources);
        }

        public virtual IObservable<TSource> Amb<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return new AmbManyEnumerable<TSource>(sources);
        }

        #endregion

        #region + Buffer +

        public virtual IObservable<IList<TSource>> Buffer<TSource, TBufferClosing>(IObservable<TSource> source, Func<IObservable<TBufferClosing>> bufferClosingSelector)
        {
            return new Buffer<TSource, TBufferClosing>.Selector(source, bufferClosingSelector);
        }

        public virtual IObservable<IList<TSource>> Buffer<TSource, TBufferOpening, TBufferClosing>(IObservable<TSource> source, IObservable<TBufferOpening> bufferOpenings, Func<TBufferOpening, IObservable<TBufferClosing>> bufferClosingSelector)
        {
            return source.Window(bufferOpenings, bufferClosingSelector).SelectMany(ToList);
        }

        public virtual IObservable<IList<TSource>> Buffer<TSource, TBufferBoundary>(IObservable<TSource> source, IObservable<TBufferBoundary> bufferBoundaries)
        {
            return new Buffer<TSource, TBufferBoundary>.Boundaries(source, bufferBoundaries);
        }

        #endregion

        #region + Catch +

        public virtual IObservable<TSource> Catch<TSource, TException>(IObservable<TSource> source, Func<TException, IObservable<TSource>> handler) where TException : Exception
        {
            return new Catch<TSource, TException>(source, handler);
        }

        public virtual IObservable<TSource> Catch<TSource>(IObservable<TSource> first, IObservable<TSource> second)
        {
            return Catch_(new[] { first, second });
        }

        public virtual IObservable<TSource> Catch<TSource>(params IObservable<TSource>[] sources)
        {
            return Catch_(sources);
        }

        public virtual IObservable<TSource> Catch<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return Catch_(sources);
        }

        private static IObservable<TSource> Catch_<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return new Catch<TSource>(sources);
        }

        #endregion

        #region + CombineLatest +

        public virtual IObservable<TResult> CombineLatest<TFirst, TSecond, TResult>(IObservable<TFirst> first, IObservable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            return new CombineLatest<TFirst, TSecond, TResult>(first, second, resultSelector);
        }

        public virtual IObservable<TResult> CombineLatest<TSource, TResult>(IEnumerable<IObservable<TSource>> sources, Func<IList<TSource>, TResult> resultSelector)
        {
            return CombineLatest_(sources, resultSelector);
        }

        public virtual IObservable<IList<TSource>> CombineLatest<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return CombineLatest_(sources, res => res.ToList());
        }

        public virtual IObservable<IList<TSource>> CombineLatest<TSource>(params IObservable<TSource>[] sources)
        {
            return CombineLatest_(sources, res => res.ToList());
        }

        private static IObservable<TResult> CombineLatest_<TSource, TResult>(IEnumerable<IObservable<TSource>> sources, Func<IList<TSource>, TResult> resultSelector)
        {
            return new CombineLatest<TSource, TResult>(sources, resultSelector);
        }

        #endregion

        #region + Concat +

        public virtual IObservable<TSource> Concat<TSource>(IObservable<TSource> first, IObservable<TSource> second)
        {
            return Concat_(new[] { first, second });
        }

        public virtual IObservable<TSource> Concat<TSource>(params IObservable<TSource>[] sources)
        {
            return Concat_(sources);
        }

        public virtual IObservable<TSource> Concat<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return Concat_(sources);
        }

        private static IObservable<TSource> Concat_<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return new Concat<TSource>(sources);
        }

        public virtual IObservable<TSource> Concat<TSource>(IObservable<IObservable<TSource>> sources)
        {
            return Concat_(sources);
        }

        public virtual IObservable<TSource> Concat<TSource>(IObservable<Task<TSource>> sources)
        {
            return Concat_(Select(sources, TaskObservableExtensions.ToObservable));
        }

        private static IObservable<TSource> Concat_<TSource>(IObservable<IObservable<TSource>> sources)
        {
            return new ConcatMany<TSource>(sources);
        }

        #endregion

        #region + Merge +

        public virtual IObservable<TSource> Merge<TSource>(IObservable<IObservable<TSource>> sources)
        {
            return Merge_(sources);
        }

        public virtual IObservable<TSource> Merge<TSource>(IObservable<Task<TSource>> sources)
        {
            return new Merge<TSource>.Tasks(sources);
        }

        public virtual IObservable<TSource> Merge<TSource>(IObservable<IObservable<TSource>> sources, int maxConcurrent)
        {
            return Merge_(sources, maxConcurrent);
        }

        public virtual IObservable<TSource> Merge<TSource>(IEnumerable<IObservable<TSource>> sources, int maxConcurrent)
        {
            return Merge_(sources.ToObservable(SchedulerDefaults.ConstantTimeOperations), maxConcurrent);
        }

        public virtual IObservable<TSource> Merge<TSource>(IEnumerable<IObservable<TSource>> sources, int maxConcurrent, IScheduler scheduler)
        {
            return Merge_(sources.ToObservable(scheduler), maxConcurrent);
        }

        public virtual IObservable<TSource> Merge<TSource>(IObservable<TSource> first, IObservable<TSource> second)
        {
            return Merge_(new[] { first, second }.ToObservable(SchedulerDefaults.ConstantTimeOperations));
        }

        public virtual IObservable<TSource> Merge<TSource>(IObservable<TSource> first, IObservable<TSource> second, IScheduler scheduler)
        {
            return Merge_(new[] { first, second }.ToObservable(scheduler));
        }

        public virtual IObservable<TSource> Merge<TSource>(params IObservable<TSource>[] sources)
        {
            return Merge_(sources.ToObservable(SchedulerDefaults.ConstantTimeOperations));
        }

        public virtual IObservable<TSource> Merge<TSource>(IScheduler scheduler, params IObservable<TSource>[] sources)
        {
            return Merge_(sources.ToObservable(scheduler));
        }

        public virtual IObservable<TSource> Merge<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return Merge_(sources.ToObservable(SchedulerDefaults.ConstantTimeOperations));
        }

        public virtual IObservable<TSource> Merge<TSource>(IEnumerable<IObservable<TSource>> sources, IScheduler scheduler)
        {
            return Merge_(sources.ToObservable(scheduler));
        }

        private static IObservable<TSource> Merge_<TSource>(IObservable<IObservable<TSource>> sources)
        {
            return new Merge<TSource>.Observables(sources);
        }

        private static IObservable<TSource> Merge_<TSource>(IObservable<IObservable<TSource>> sources, int maxConcurrent)
        {
            return new Merge<TSource>.ObservablesMaxConcurrency(sources, maxConcurrent);
        }

        #endregion

        #region + OnErrorResumeNext +

        public virtual IObservable<TSource> OnErrorResumeNext<TSource>(IObservable<TSource> first, IObservable<TSource> second)
        {
            return OnErrorResumeNext_(new[] { first, second });
        }

        public virtual IObservable<TSource> OnErrorResumeNext<TSource>(params IObservable<TSource>[] sources)
        {
            return OnErrorResumeNext_(sources);
        }

        public virtual IObservable<TSource> OnErrorResumeNext<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return OnErrorResumeNext_(sources);
        }

        private static IObservable<TSource> OnErrorResumeNext_<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return new OnErrorResumeNext<TSource>(sources);
        }

        #endregion

        #region + SkipUntil +

        public virtual IObservable<TSource> SkipUntil<TSource, TOther>(IObservable<TSource> source, IObservable<TOther> other)
        {
            return new SkipUntil<TSource, TOther>(source, other);
        }

        #endregion

        #region + Switch +

        public virtual IObservable<TSource> Switch<TSource>(IObservable<IObservable<TSource>> sources)
        {
            return Switch_(sources);
        }

        public virtual IObservable<TSource> Switch<TSource>(IObservable<Task<TSource>> sources)
        {
            return Switch_(Select(sources, TaskObservableExtensions.ToObservable));
        }

        private static IObservable<TSource> Switch_<TSource>(IObservable<IObservable<TSource>> sources)
        {
            return new Switch<TSource>(sources);
        }

        #endregion

        #region + TakeUntil +

        public virtual IObservable<TSource> TakeUntil<TSource, TOther>(IObservable<TSource> source, IObservable<TOther> other)
        {
            return new TakeUntil<TSource, TOther>(source, other);
        }

        public virtual IObservable<TSource> TakeUntil<TSource>(IObservable<TSource> source, Func<TSource, bool> stopPredicate)
        {
            return new TakeUntilPredicate<TSource>(source, stopPredicate);
        }

        #endregion

        #region + Window +

        public virtual IObservable<IObservable<TSource>> Window<TSource, TWindowClosing>(IObservable<TSource> source, Func<IObservable<TWindowClosing>> windowClosingSelector)
        {
            return new Window<TSource, TWindowClosing>.Selector(source, windowClosingSelector);
        }

        public virtual IObservable<IObservable<TSource>> Window<TSource, TWindowOpening, TWindowClosing>(IObservable<TSource> source, IObservable<TWindowOpening> windowOpenings, Func<TWindowOpening, IObservable<TWindowClosing>> windowClosingSelector)
        {
            return windowOpenings.GroupJoin(source, windowClosingSelector, _ => Observable.Empty<Unit>(), (_, window) => window);
        }

        public virtual IObservable<IObservable<TSource>> Window<TSource, TWindowBoundary>(IObservable<TSource> source, IObservable<TWindowBoundary> windowBoundaries)
        {
            return new Window<TSource, TWindowBoundary>.Boundaries(source, windowBoundaries);
        }

        #endregion

        #region + WithLatestFrom +

        public virtual IObservable<TResult> WithLatestFrom<TFirst, TSecond, TResult>(IObservable<TFirst> first, IObservable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            return new WithLatestFrom<TFirst, TSecond, TResult>(first, second, resultSelector);
        }

        #endregion

        #region + Zip +

        public virtual IObservable<TResult> Zip<TFirst, TSecond, TResult>(IObservable<TFirst> first, IObservable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            return new Zip<TFirst, TSecond, TResult>.Observable(first, second, resultSelector);
        }

        public virtual IObservable<TResult> Zip<TSource, TResult>(IEnumerable<IObservable<TSource>> sources, Func<IList<TSource>, TResult> resultSelector)
        {
            return Zip_(sources).Select(resultSelector);
        }

        public virtual IObservable<IList<TSource>> Zip<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return Zip_(sources);
        }

        public virtual IObservable<IList<TSource>> Zip<TSource>(params IObservable<TSource>[] sources)
        {
            return Zip_(sources);
        }

        private static IObservable<IList<TSource>> Zip_<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return new Zip<TSource>(sources);
        }

        public virtual IObservable<TResult> Zip<TFirst, TSecond, TResult>(IObservable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            return new Zip<TFirst, TSecond, TResult>.Enumerable(first, second, resultSelector);
        }

        #endregion
    }
}
