// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace System.Reactive.Linq
{
    using ObservableImpl;

    internal partial class QueryLanguage
    {
        #region + Multicast +

        public virtual IConnectableObservable<TResult> Multicast<TSource, TResult>(IObservable<TSource> source, ISubject<TSource, TResult> subject)
        {
            return new ConnectableObservable<TSource, TResult>(source, subject);
        }

        public virtual IObservable<TResult> Multicast<TSource, TIntermediate, TResult>(IObservable<TSource> source, Func<ISubject<TSource, TIntermediate>> subjectSelector, Func<IObservable<TIntermediate>, IObservable<TResult>> selector)
        {
            return new Multicast<TSource, TIntermediate, TResult>(source, subjectSelector, selector);
        }

        #endregion

        #region + Publish +

        public virtual IConnectableObservable<TSource> Publish<TSource>(IObservable<TSource> source)
        {
            return source.Multicast(new Subject<TSource>());
        }

        public virtual IObservable<TResult> Publish<TSource, TResult>(IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector)
        {
            return source.Multicast(() => new Subject<TSource>(), selector);
        }

        public virtual IConnectableObservable<TSource> Publish<TSource>(IObservable<TSource> source, TSource initialValue)
        {
            return source.Multicast(new BehaviorSubject<TSource>(initialValue));
        }

        public virtual IObservable<TResult> Publish<TSource, TResult>(IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector, TSource initialValue)
        {
            return source.Multicast(() => new BehaviorSubject<TSource>(initialValue), selector);
        }

        #endregion

        #region + PublishLast +

        public virtual IConnectableObservable<TSource> PublishLast<TSource>(IObservable<TSource> source)
        {
            return source.Multicast(new AsyncSubject<TSource>());
        }

        public virtual IObservable<TResult> PublishLast<TSource, TResult>(IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector)
        {
            return source.Multicast(() => new AsyncSubject<TSource>(), selector);
        }

        #endregion

        #region + RefCount +

        public virtual IObservable<TSource> RefCount<TSource>(IConnectableObservable<TSource> source)
        {
            return new RefCount<TSource>(source);
        }

        #endregion

        #region + Replay +

        public virtual IConnectableObservable<TSource> Replay<TSource>(IObservable<TSource> source)
        {
            return source.Multicast(new ReplaySubject<TSource>());
        }

        public virtual IConnectableObservable<TSource> Replay<TSource>(IObservable<TSource> source, IScheduler scheduler)
        {
            return source.Multicast(new ReplaySubject<TSource>(scheduler));
        }

        public virtual IObservable<TResult> Replay<TSource, TResult>(IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector)
        {
            return source.Multicast(() => new ReplaySubject<TSource>(), selector);
        }

        public virtual IObservable<TResult> Replay<TSource, TResult>(IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector, IScheduler scheduler)
        {
            return source.Multicast(() => new ReplaySubject<TSource>(scheduler), selector);
        }

        public virtual IConnectableObservable<TSource> Replay<TSource>(IObservable<TSource> source, TimeSpan window)
        {
            return source.Multicast(new ReplaySubject<TSource>(window));
        }

        public virtual IObservable<TResult> Replay<TSource, TResult>(IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector, TimeSpan window)
        {
            return source.Multicast(() => new ReplaySubject<TSource>(window), selector);
        }

        public virtual IConnectableObservable<TSource> Replay<TSource>(IObservable<TSource> source, TimeSpan window, IScheduler scheduler)
        {
            return source.Multicast(new ReplaySubject<TSource>(window, scheduler));
        }

        public virtual IObservable<TResult> Replay<TSource, TResult>(IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector, TimeSpan window, IScheduler scheduler)
        {
            return source.Multicast(() => new ReplaySubject<TSource>(window, scheduler), selector);
        }

        public virtual IConnectableObservable<TSource> Replay<TSource>(IObservable<TSource> source, int bufferSize, IScheduler scheduler)
        {
            return source.Multicast(new ReplaySubject<TSource>(bufferSize, scheduler));
        }

        public virtual IObservable<TResult> Replay<TSource, TResult>(IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector, int bufferSize, IScheduler scheduler)
        {
            return source.Multicast(() => new ReplaySubject<TSource>(bufferSize, scheduler), selector);
        }

        public virtual IConnectableObservable<TSource> Replay<TSource>(IObservable<TSource> source, int bufferSize)
        {
            return source.Multicast(new ReplaySubject<TSource>(bufferSize));
        }

        public virtual IObservable<TResult> Replay<TSource, TResult>(IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector, int bufferSize)
        {
            return source.Multicast(() => new ReplaySubject<TSource>(bufferSize), selector);
        }

        public virtual IConnectableObservable<TSource> Replay<TSource>(IObservable<TSource> source, int bufferSize, TimeSpan window)
        {
            return source.Multicast(new ReplaySubject<TSource>(bufferSize, window));
        }

        public virtual IObservable<TResult> Replay<TSource, TResult>(IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector, int bufferSize, TimeSpan window)
        {
            return source.Multicast(() => new ReplaySubject<TSource>(bufferSize, window), selector);
        }

        public virtual IConnectableObservable<TSource> Replay<TSource>(IObservable<TSource> source, int bufferSize, TimeSpan window, IScheduler scheduler)
        {
            return source.Multicast(new ReplaySubject<TSource>(bufferSize, window, scheduler));
        }

        public virtual IObservable<TResult> Replay<TSource, TResult>(IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector, int bufferSize, TimeSpan window, IScheduler scheduler)
        {
            return source.Multicast(() => new ReplaySubject<TSource>(bufferSize, window, scheduler), selector);
        }

        #endregion
    }
}
