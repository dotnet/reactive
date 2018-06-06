// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<(T1, T2)> CombineLatest<T1, T2>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));

            return Create<(T1, T2)>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2) = AsyncObserver.CombineLatest(observer);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> CombineLatest<T1, T2, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, Func<T1, T2, TResult> selector)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TResult>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2) = AsyncObserver.CombineLatest(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> CombineLatest<T1, T2, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, Func<T1, T2, Task<TResult>> selector)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TResult>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2) = AsyncObserver.CombineLatest(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<(T1, T2, T3)> CombineLatest<T1, T2, T3>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));

            return Create<(T1, T2, T3)>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3) = AsyncObserver.CombineLatest(observer);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> CombineLatest<T1, T2, T3, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, Func<T1, T2, T3, TResult> selector)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TResult>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3) = AsyncObserver.CombineLatest(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> CombineLatest<T1, T2, T3, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, Func<T1, T2, T3, Task<TResult>> selector)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TResult>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3) = AsyncObserver.CombineLatest(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<(T1, T2, T3, T4)> CombineLatest<T1, T2, T3, T4>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));

            return Create<(T1, T2, T3, T4)>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4) = AsyncObserver.CombineLatest(observer);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> CombineLatest<T1, T2, T3, T4, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, Func<T1, T2, T3, T4, TResult> selector)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TResult>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4) = AsyncObserver.CombineLatest(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> CombineLatest<T1, T2, T3, T4, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, Func<T1, T2, T3, T4, Task<TResult>> selector)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TResult>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4) = AsyncObserver.CombineLatest(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<(T1, T2, T3, T4, T5)> CombineLatest<T1, T2, T3, T4, T5>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));

            return Create<(T1, T2, T3, T4, T5)>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5) = AsyncObserver.CombineLatest(observer);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, Func<T1, T2, T3, T4, T5, TResult> selector)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TResult>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5) = AsyncObserver.CombineLatest(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, Func<T1, T2, T3, T4, T5, Task<TResult>> selector)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TResult>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5) = AsyncObserver.CombineLatest(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<(T1, T2, T3, T4, T5, T6)> CombineLatest<T1, T2, T3, T4, T5, T6>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (source6 == null)
                throw new ArgumentNullException(nameof(source6));

            return Create<(T1, T2, T3, T4, T5, T6)>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5, observer6) = AsyncObserver.CombineLatest(observer);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub6 = source6.SubscribeSafeAsync(observer6).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5, sub6).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, Func<T1, T2, T3, T4, T5, T6, TResult> selector)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (source6 == null)
                throw new ArgumentNullException(nameof(source6));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TResult>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5, observer6) = AsyncObserver.CombineLatest(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub6 = source6.SubscribeSafeAsync(observer6).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5, sub6).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, Func<T1, T2, T3, T4, T5, T6, Task<TResult>> selector)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (source6 == null)
                throw new ArgumentNullException(nameof(source6));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TResult>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5, observer6) = AsyncObserver.CombineLatest(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub6 = source6.SubscribeSafeAsync(observer6).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5, sub6).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<(T1, T2, T3, T4, T5, T6, T7)> CombineLatest<T1, T2, T3, T4, T5, T6, T7>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (source6 == null)
                throw new ArgumentNullException(nameof(source6));
            if (source7 == null)
                throw new ArgumentNullException(nameof(source7));

            return Create<(T1, T2, T3, T4, T5, T6, T7)>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7) = AsyncObserver.CombineLatest(observer);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub6 = source6.SubscribeSafeAsync(observer6).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub7 = source7.SubscribeSafeAsync(observer7).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5, sub6, sub7).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, Func<T1, T2, T3, T4, T5, T6, T7, TResult> selector)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (source6 == null)
                throw new ArgumentNullException(nameof(source6));
            if (source7 == null)
                throw new ArgumentNullException(nameof(source7));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TResult>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7) = AsyncObserver.CombineLatest(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub6 = source6.SubscribeSafeAsync(observer6).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub7 = source7.SubscribeSafeAsync(observer7).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5, sub6, sub7).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, Func<T1, T2, T3, T4, T5, T6, T7, Task<TResult>> selector)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (source6 == null)
                throw new ArgumentNullException(nameof(source6));
            if (source7 == null)
                throw new ArgumentNullException(nameof(source7));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TResult>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7) = AsyncObserver.CombineLatest(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub6 = source6.SubscribeSafeAsync(observer6).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub7 = source7.SubscribeSafeAsync(observer7).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5, sub6, sub7).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<(T1, T2, T3, T4, T5, T6, T7, T8)> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (source6 == null)
                throw new ArgumentNullException(nameof(source6));
            if (source7 == null)
                throw new ArgumentNullException(nameof(source7));
            if (source8 == null)
                throw new ArgumentNullException(nameof(source8));

            return Create<(T1, T2, T3, T4, T5, T6, T7, T8)>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8) = AsyncObserver.CombineLatest(observer);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub6 = source6.SubscribeSafeAsync(observer6).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub7 = source7.SubscribeSafeAsync(observer7).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub8 = source8.SubscribeSafeAsync(observer8).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5, sub6, sub7, sub8).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> selector)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (source6 == null)
                throw new ArgumentNullException(nameof(source6));
            if (source7 == null)
                throw new ArgumentNullException(nameof(source7));
            if (source8 == null)
                throw new ArgumentNullException(nameof(source8));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TResult>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8) = AsyncObserver.CombineLatest(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub6 = source6.SubscribeSafeAsync(observer6).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub7 = source7.SubscribeSafeAsync(observer7).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub8 = source8.SubscribeSafeAsync(observer8).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5, sub6, sub7, sub8).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, Func<T1, T2, T3, T4, T5, T6, T7, T8, Task<TResult>> selector)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (source6 == null)
                throw new ArgumentNullException(nameof(source6));
            if (source7 == null)
                throw new ArgumentNullException(nameof(source7));
            if (source8 == null)
                throw new ArgumentNullException(nameof(source8));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TResult>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8) = AsyncObserver.CombineLatest(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub6 = source6.SubscribeSafeAsync(observer6).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub7 = source7.SubscribeSafeAsync(observer7).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub8 = source8.SubscribeSafeAsync(observer8).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5, sub6, sub7, sub8).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (source6 == null)
                throw new ArgumentNullException(nameof(source6));
            if (source7 == null)
                throw new ArgumentNullException(nameof(source7));
            if (source8 == null)
                throw new ArgumentNullException(nameof(source8));
            if (source9 == null)
                throw new ArgumentNullException(nameof(source9));

            return Create<(T1, T2, T3, T4, T5, T6, T7, T8, T9)>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9) = AsyncObserver.CombineLatest(observer);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub6 = source6.SubscribeSafeAsync(observer6).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub7 = source7.SubscribeSafeAsync(observer7).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub8 = source8.SubscribeSafeAsync(observer8).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub9 = source9.SubscribeSafeAsync(observer9).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5, sub6, sub7, sub8, sub9).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> selector)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (source6 == null)
                throw new ArgumentNullException(nameof(source6));
            if (source7 == null)
                throw new ArgumentNullException(nameof(source7));
            if (source8 == null)
                throw new ArgumentNullException(nameof(source8));
            if (source9 == null)
                throw new ArgumentNullException(nameof(source9));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TResult>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9) = AsyncObserver.CombineLatest(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub6 = source6.SubscribeSafeAsync(observer6).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub7 = source7.SubscribeSafeAsync(observer7).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub8 = source8.SubscribeSafeAsync(observer8).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub9 = source9.SubscribeSafeAsync(observer9).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5, sub6, sub7, sub8, sub9).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task<TResult>> selector)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (source6 == null)
                throw new ArgumentNullException(nameof(source6));
            if (source7 == null)
                throw new ArgumentNullException(nameof(source7));
            if (source8 == null)
                throw new ArgumentNullException(nameof(source8));
            if (source9 == null)
                throw new ArgumentNullException(nameof(source9));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TResult>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9) = AsyncObserver.CombineLatest(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub6 = source6.SubscribeSafeAsync(observer6).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub7 = source7.SubscribeSafeAsync(observer7).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub8 = source8.SubscribeSafeAsync(observer8).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub9 = source9.SubscribeSafeAsync(observer9).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5, sub6, sub7, sub8, sub9).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (source6 == null)
                throw new ArgumentNullException(nameof(source6));
            if (source7 == null)
                throw new ArgumentNullException(nameof(source7));
            if (source8 == null)
                throw new ArgumentNullException(nameof(source8));
            if (source9 == null)
                throw new ArgumentNullException(nameof(source9));
            if (source10 == null)
                throw new ArgumentNullException(nameof(source10));

            return Create<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10) = AsyncObserver.CombineLatest(observer);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub6 = source6.SubscribeSafeAsync(observer6).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub7 = source7.SubscribeSafeAsync(observer7).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub8 = source8.SubscribeSafeAsync(observer8).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub9 = source9.SubscribeSafeAsync(observer9).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub10 = source10.SubscribeSafeAsync(observer10).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5, sub6, sub7, sub8, sub9, sub10).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> selector)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (source6 == null)
                throw new ArgumentNullException(nameof(source6));
            if (source7 == null)
                throw new ArgumentNullException(nameof(source7));
            if (source8 == null)
                throw new ArgumentNullException(nameof(source8));
            if (source9 == null)
                throw new ArgumentNullException(nameof(source9));
            if (source10 == null)
                throw new ArgumentNullException(nameof(source10));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TResult>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10) = AsyncObserver.CombineLatest(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub6 = source6.SubscribeSafeAsync(observer6).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub7 = source7.SubscribeSafeAsync(observer7).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub8 = source8.SubscribeSafeAsync(observer8).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub9 = source9.SubscribeSafeAsync(observer9).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub10 = source10.SubscribeSafeAsync(observer10).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5, sub6, sub7, sub8, sub9, sub10).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task<TResult>> selector)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (source6 == null)
                throw new ArgumentNullException(nameof(source6));
            if (source7 == null)
                throw new ArgumentNullException(nameof(source7));
            if (source8 == null)
                throw new ArgumentNullException(nameof(source8));
            if (source9 == null)
                throw new ArgumentNullException(nameof(source9));
            if (source10 == null)
                throw new ArgumentNullException(nameof(source10));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TResult>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10) = AsyncObserver.CombineLatest(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub6 = source6.SubscribeSafeAsync(observer6).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub7 = source7.SubscribeSafeAsync(observer7).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub8 = source8.SubscribeSafeAsync(observer8).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub9 = source9.SubscribeSafeAsync(observer9).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub10 = source10.SubscribeSafeAsync(observer10).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5, sub6, sub7, sub8, sub9, sub10).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, IAsyncObservable<T11> source11)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (source6 == null)
                throw new ArgumentNullException(nameof(source6));
            if (source7 == null)
                throw new ArgumentNullException(nameof(source7));
            if (source8 == null)
                throw new ArgumentNullException(nameof(source8));
            if (source9 == null)
                throw new ArgumentNullException(nameof(source9));
            if (source10 == null)
                throw new ArgumentNullException(nameof(source10));
            if (source11 == null)
                throw new ArgumentNullException(nameof(source11));

            return Create<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10, observer11) = AsyncObserver.CombineLatest(observer);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub6 = source6.SubscribeSafeAsync(observer6).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub7 = source7.SubscribeSafeAsync(observer7).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub8 = source8.SubscribeSafeAsync(observer8).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub9 = source9.SubscribeSafeAsync(observer9).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub10 = source10.SubscribeSafeAsync(observer10).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub11 = source11.SubscribeSafeAsync(observer11).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5, sub6, sub7, sub8, sub9, sub10, sub11).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, IAsyncObservable<T11> source11, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> selector)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (source6 == null)
                throw new ArgumentNullException(nameof(source6));
            if (source7 == null)
                throw new ArgumentNullException(nameof(source7));
            if (source8 == null)
                throw new ArgumentNullException(nameof(source8));
            if (source9 == null)
                throw new ArgumentNullException(nameof(source9));
            if (source10 == null)
                throw new ArgumentNullException(nameof(source10));
            if (source11 == null)
                throw new ArgumentNullException(nameof(source11));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TResult>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10, observer11) = AsyncObserver.CombineLatest(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub6 = source6.SubscribeSafeAsync(observer6).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub7 = source7.SubscribeSafeAsync(observer7).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub8 = source8.SubscribeSafeAsync(observer8).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub9 = source9.SubscribeSafeAsync(observer9).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub10 = source10.SubscribeSafeAsync(observer10).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub11 = source11.SubscribeSafeAsync(observer11).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5, sub6, sub7, sub8, sub9, sub10, sub11).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, IAsyncObservable<T11> source11, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task<TResult>> selector)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (source6 == null)
                throw new ArgumentNullException(nameof(source6));
            if (source7 == null)
                throw new ArgumentNullException(nameof(source7));
            if (source8 == null)
                throw new ArgumentNullException(nameof(source8));
            if (source9 == null)
                throw new ArgumentNullException(nameof(source9));
            if (source10 == null)
                throw new ArgumentNullException(nameof(source10));
            if (source11 == null)
                throw new ArgumentNullException(nameof(source11));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TResult>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10, observer11) = AsyncObserver.CombineLatest(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub6 = source6.SubscribeSafeAsync(observer6).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub7 = source7.SubscribeSafeAsync(observer7).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub8 = source8.SubscribeSafeAsync(observer8).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub9 = source9.SubscribeSafeAsync(observer9).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub10 = source10.SubscribeSafeAsync(observer10).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub11 = source11.SubscribeSafeAsync(observer11).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5, sub6, sub7, sub8, sub9, sub10, sub11).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, IAsyncObservable<T11> source11, IAsyncObservable<T12> source12)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (source6 == null)
                throw new ArgumentNullException(nameof(source6));
            if (source7 == null)
                throw new ArgumentNullException(nameof(source7));
            if (source8 == null)
                throw new ArgumentNullException(nameof(source8));
            if (source9 == null)
                throw new ArgumentNullException(nameof(source9));
            if (source10 == null)
                throw new ArgumentNullException(nameof(source10));
            if (source11 == null)
                throw new ArgumentNullException(nameof(source11));
            if (source12 == null)
                throw new ArgumentNullException(nameof(source12));

            return Create<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10, observer11, observer12) = AsyncObserver.CombineLatest(observer);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub6 = source6.SubscribeSafeAsync(observer6).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub7 = source7.SubscribeSafeAsync(observer7).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub8 = source8.SubscribeSafeAsync(observer8).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub9 = source9.SubscribeSafeAsync(observer9).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub10 = source10.SubscribeSafeAsync(observer10).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub11 = source11.SubscribeSafeAsync(observer11).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub12 = source12.SubscribeSafeAsync(observer12).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5, sub6, sub7, sub8, sub9, sub10, sub11, sub12).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, IAsyncObservable<T11> source11, IAsyncObservable<T12> source12, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> selector)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (source6 == null)
                throw new ArgumentNullException(nameof(source6));
            if (source7 == null)
                throw new ArgumentNullException(nameof(source7));
            if (source8 == null)
                throw new ArgumentNullException(nameof(source8));
            if (source9 == null)
                throw new ArgumentNullException(nameof(source9));
            if (source10 == null)
                throw new ArgumentNullException(nameof(source10));
            if (source11 == null)
                throw new ArgumentNullException(nameof(source11));
            if (source12 == null)
                throw new ArgumentNullException(nameof(source12));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TResult>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10, observer11, observer12) = AsyncObserver.CombineLatest(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub6 = source6.SubscribeSafeAsync(observer6).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub7 = source7.SubscribeSafeAsync(observer7).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub8 = source8.SubscribeSafeAsync(observer8).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub9 = source9.SubscribeSafeAsync(observer9).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub10 = source10.SubscribeSafeAsync(observer10).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub11 = source11.SubscribeSafeAsync(observer11).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub12 = source12.SubscribeSafeAsync(observer12).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5, sub6, sub7, sub8, sub9, sub10, sub11, sub12).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, IAsyncObservable<T11> source11, IAsyncObservable<T12> source12, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task<TResult>> selector)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (source6 == null)
                throw new ArgumentNullException(nameof(source6));
            if (source7 == null)
                throw new ArgumentNullException(nameof(source7));
            if (source8 == null)
                throw new ArgumentNullException(nameof(source8));
            if (source9 == null)
                throw new ArgumentNullException(nameof(source9));
            if (source10 == null)
                throw new ArgumentNullException(nameof(source10));
            if (source11 == null)
                throw new ArgumentNullException(nameof(source11));
            if (source12 == null)
                throw new ArgumentNullException(nameof(source12));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TResult>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10, observer11, observer12) = AsyncObserver.CombineLatest(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub6 = source6.SubscribeSafeAsync(observer6).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub7 = source7.SubscribeSafeAsync(observer7).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub8 = source8.SubscribeSafeAsync(observer8).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub9 = source9.SubscribeSafeAsync(observer9).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub10 = source10.SubscribeSafeAsync(observer10).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub11 = source11.SubscribeSafeAsync(observer11).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub12 = source12.SubscribeSafeAsync(observer12).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5, sub6, sub7, sub8, sub9, sub10, sub11, sub12).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, IAsyncObservable<T11> source11, IAsyncObservable<T12> source12, IAsyncObservable<T13> source13)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (source6 == null)
                throw new ArgumentNullException(nameof(source6));
            if (source7 == null)
                throw new ArgumentNullException(nameof(source7));
            if (source8 == null)
                throw new ArgumentNullException(nameof(source8));
            if (source9 == null)
                throw new ArgumentNullException(nameof(source9));
            if (source10 == null)
                throw new ArgumentNullException(nameof(source10));
            if (source11 == null)
                throw new ArgumentNullException(nameof(source11));
            if (source12 == null)
                throw new ArgumentNullException(nameof(source12));
            if (source13 == null)
                throw new ArgumentNullException(nameof(source13));

            return Create<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10, observer11, observer12, observer13) = AsyncObserver.CombineLatest(observer);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub6 = source6.SubscribeSafeAsync(observer6).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub7 = source7.SubscribeSafeAsync(observer7).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub8 = source8.SubscribeSafeAsync(observer8).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub9 = source9.SubscribeSafeAsync(observer9).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub10 = source10.SubscribeSafeAsync(observer10).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub11 = source11.SubscribeSafeAsync(observer11).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub12 = source12.SubscribeSafeAsync(observer12).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub13 = source13.SubscribeSafeAsync(observer13).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5, sub6, sub7, sub8, sub9, sub10, sub11, sub12, sub13).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, IAsyncObservable<T11> source11, IAsyncObservable<T12> source12, IAsyncObservable<T13> source13, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> selector)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (source6 == null)
                throw new ArgumentNullException(nameof(source6));
            if (source7 == null)
                throw new ArgumentNullException(nameof(source7));
            if (source8 == null)
                throw new ArgumentNullException(nameof(source8));
            if (source9 == null)
                throw new ArgumentNullException(nameof(source9));
            if (source10 == null)
                throw new ArgumentNullException(nameof(source10));
            if (source11 == null)
                throw new ArgumentNullException(nameof(source11));
            if (source12 == null)
                throw new ArgumentNullException(nameof(source12));
            if (source13 == null)
                throw new ArgumentNullException(nameof(source13));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TResult>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10, observer11, observer12, observer13) = AsyncObserver.CombineLatest(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub6 = source6.SubscribeSafeAsync(observer6).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub7 = source7.SubscribeSafeAsync(observer7).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub8 = source8.SubscribeSafeAsync(observer8).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub9 = source9.SubscribeSafeAsync(observer9).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub10 = source10.SubscribeSafeAsync(observer10).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub11 = source11.SubscribeSafeAsync(observer11).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub12 = source12.SubscribeSafeAsync(observer12).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub13 = source13.SubscribeSafeAsync(observer13).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5, sub6, sub7, sub8, sub9, sub10, sub11, sub12, sub13).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, IAsyncObservable<T11> source11, IAsyncObservable<T12> source12, IAsyncObservable<T13> source13, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task<TResult>> selector)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (source6 == null)
                throw new ArgumentNullException(nameof(source6));
            if (source7 == null)
                throw new ArgumentNullException(nameof(source7));
            if (source8 == null)
                throw new ArgumentNullException(nameof(source8));
            if (source9 == null)
                throw new ArgumentNullException(nameof(source9));
            if (source10 == null)
                throw new ArgumentNullException(nameof(source10));
            if (source11 == null)
                throw new ArgumentNullException(nameof(source11));
            if (source12 == null)
                throw new ArgumentNullException(nameof(source12));
            if (source13 == null)
                throw new ArgumentNullException(nameof(source13));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TResult>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10, observer11, observer12, observer13) = AsyncObserver.CombineLatest(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub6 = source6.SubscribeSafeAsync(observer6).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub7 = source7.SubscribeSafeAsync(observer7).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub8 = source8.SubscribeSafeAsync(observer8).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub9 = source9.SubscribeSafeAsync(observer9).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub10 = source10.SubscribeSafeAsync(observer10).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub11 = source11.SubscribeSafeAsync(observer11).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub12 = source12.SubscribeSafeAsync(observer12).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub13 = source13.SubscribeSafeAsync(observer13).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5, sub6, sub7, sub8, sub9, sub10, sub11, sub12, sub13).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, IAsyncObservable<T11> source11, IAsyncObservable<T12> source12, IAsyncObservable<T13> source13, IAsyncObservable<T14> source14)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (source6 == null)
                throw new ArgumentNullException(nameof(source6));
            if (source7 == null)
                throw new ArgumentNullException(nameof(source7));
            if (source8 == null)
                throw new ArgumentNullException(nameof(source8));
            if (source9 == null)
                throw new ArgumentNullException(nameof(source9));
            if (source10 == null)
                throw new ArgumentNullException(nameof(source10));
            if (source11 == null)
                throw new ArgumentNullException(nameof(source11));
            if (source12 == null)
                throw new ArgumentNullException(nameof(source12));
            if (source13 == null)
                throw new ArgumentNullException(nameof(source13));
            if (source14 == null)
                throw new ArgumentNullException(nameof(source14));

            return Create<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10, observer11, observer12, observer13, observer14) = AsyncObserver.CombineLatest(observer);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub6 = source6.SubscribeSafeAsync(observer6).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub7 = source7.SubscribeSafeAsync(observer7).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub8 = source8.SubscribeSafeAsync(observer8).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub9 = source9.SubscribeSafeAsync(observer9).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub10 = source10.SubscribeSafeAsync(observer10).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub11 = source11.SubscribeSafeAsync(observer11).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub12 = source12.SubscribeSafeAsync(observer12).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub13 = source13.SubscribeSafeAsync(observer13).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub14 = source14.SubscribeSafeAsync(observer14).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5, sub6, sub7, sub8, sub9, sub10, sub11, sub12, sub13, sub14).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, IAsyncObservable<T11> source11, IAsyncObservable<T12> source12, IAsyncObservable<T13> source13, IAsyncObservable<T14> source14, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> selector)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (source6 == null)
                throw new ArgumentNullException(nameof(source6));
            if (source7 == null)
                throw new ArgumentNullException(nameof(source7));
            if (source8 == null)
                throw new ArgumentNullException(nameof(source8));
            if (source9 == null)
                throw new ArgumentNullException(nameof(source9));
            if (source10 == null)
                throw new ArgumentNullException(nameof(source10));
            if (source11 == null)
                throw new ArgumentNullException(nameof(source11));
            if (source12 == null)
                throw new ArgumentNullException(nameof(source12));
            if (source13 == null)
                throw new ArgumentNullException(nameof(source13));
            if (source14 == null)
                throw new ArgumentNullException(nameof(source14));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TResult>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10, observer11, observer12, observer13, observer14) = AsyncObserver.CombineLatest(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub6 = source6.SubscribeSafeAsync(observer6).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub7 = source7.SubscribeSafeAsync(observer7).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub8 = source8.SubscribeSafeAsync(observer8).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub9 = source9.SubscribeSafeAsync(observer9).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub10 = source10.SubscribeSafeAsync(observer10).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub11 = source11.SubscribeSafeAsync(observer11).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub12 = source12.SubscribeSafeAsync(observer12).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub13 = source13.SubscribeSafeAsync(observer13).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub14 = source14.SubscribeSafeAsync(observer14).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5, sub6, sub7, sub8, sub9, sub10, sub11, sub12, sub13, sub14).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, IAsyncObservable<T11> source11, IAsyncObservable<T12> source12, IAsyncObservable<T13> source13, IAsyncObservable<T14> source14, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task<TResult>> selector)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (source6 == null)
                throw new ArgumentNullException(nameof(source6));
            if (source7 == null)
                throw new ArgumentNullException(nameof(source7));
            if (source8 == null)
                throw new ArgumentNullException(nameof(source8));
            if (source9 == null)
                throw new ArgumentNullException(nameof(source9));
            if (source10 == null)
                throw new ArgumentNullException(nameof(source10));
            if (source11 == null)
                throw new ArgumentNullException(nameof(source11));
            if (source12 == null)
                throw new ArgumentNullException(nameof(source12));
            if (source13 == null)
                throw new ArgumentNullException(nameof(source13));
            if (source14 == null)
                throw new ArgumentNullException(nameof(source14));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TResult>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10, observer11, observer12, observer13, observer14) = AsyncObserver.CombineLatest(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub6 = source6.SubscribeSafeAsync(observer6).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub7 = source7.SubscribeSafeAsync(observer7).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub8 = source8.SubscribeSafeAsync(observer8).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub9 = source9.SubscribeSafeAsync(observer9).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub10 = source10.SubscribeSafeAsync(observer10).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub11 = source11.SubscribeSafeAsync(observer11).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub12 = source12.SubscribeSafeAsync(observer12).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub13 = source13.SubscribeSafeAsync(observer13).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub14 = source14.SubscribeSafeAsync(observer14).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5, sub6, sub7, sub8, sub9, sub10, sub11, sub12, sub13, sub14).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, IAsyncObservable<T11> source11, IAsyncObservable<T12> source12, IAsyncObservable<T13> source13, IAsyncObservable<T14> source14, IAsyncObservable<T15> source15)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (source6 == null)
                throw new ArgumentNullException(nameof(source6));
            if (source7 == null)
                throw new ArgumentNullException(nameof(source7));
            if (source8 == null)
                throw new ArgumentNullException(nameof(source8));
            if (source9 == null)
                throw new ArgumentNullException(nameof(source9));
            if (source10 == null)
                throw new ArgumentNullException(nameof(source10));
            if (source11 == null)
                throw new ArgumentNullException(nameof(source11));
            if (source12 == null)
                throw new ArgumentNullException(nameof(source12));
            if (source13 == null)
                throw new ArgumentNullException(nameof(source13));
            if (source14 == null)
                throw new ArgumentNullException(nameof(source14));
            if (source15 == null)
                throw new ArgumentNullException(nameof(source15));

            return Create<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10, observer11, observer12, observer13, observer14, observer15) = AsyncObserver.CombineLatest(observer);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub6 = source6.SubscribeSafeAsync(observer6).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub7 = source7.SubscribeSafeAsync(observer7).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub8 = source8.SubscribeSafeAsync(observer8).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub9 = source9.SubscribeSafeAsync(observer9).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub10 = source10.SubscribeSafeAsync(observer10).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub11 = source11.SubscribeSafeAsync(observer11).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub12 = source12.SubscribeSafeAsync(observer12).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub13 = source13.SubscribeSafeAsync(observer13).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub14 = source14.SubscribeSafeAsync(observer14).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub15 = source15.SubscribeSafeAsync(observer15).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5, sub6, sub7, sub8, sub9, sub10, sub11, sub12, sub13, sub14, sub15).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, IAsyncObservable<T11> source11, IAsyncObservable<T12> source12, IAsyncObservable<T13> source13, IAsyncObservable<T14> source14, IAsyncObservable<T15> source15, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> selector)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (source6 == null)
                throw new ArgumentNullException(nameof(source6));
            if (source7 == null)
                throw new ArgumentNullException(nameof(source7));
            if (source8 == null)
                throw new ArgumentNullException(nameof(source8));
            if (source9 == null)
                throw new ArgumentNullException(nameof(source9));
            if (source10 == null)
                throw new ArgumentNullException(nameof(source10));
            if (source11 == null)
                throw new ArgumentNullException(nameof(source11));
            if (source12 == null)
                throw new ArgumentNullException(nameof(source12));
            if (source13 == null)
                throw new ArgumentNullException(nameof(source13));
            if (source14 == null)
                throw new ArgumentNullException(nameof(source14));
            if (source15 == null)
                throw new ArgumentNullException(nameof(source15));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TResult>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10, observer11, observer12, observer13, observer14, observer15) = AsyncObserver.CombineLatest(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub6 = source6.SubscribeSafeAsync(observer6).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub7 = source7.SubscribeSafeAsync(observer7).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub8 = source8.SubscribeSafeAsync(observer8).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub9 = source9.SubscribeSafeAsync(observer9).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub10 = source10.SubscribeSafeAsync(observer10).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub11 = source11.SubscribeSafeAsync(observer11).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub12 = source12.SubscribeSafeAsync(observer12).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub13 = source13.SubscribeSafeAsync(observer13).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub14 = source14.SubscribeSafeAsync(observer14).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub15 = source15.SubscribeSafeAsync(observer15).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5, sub6, sub7, sub8, sub9, sub10, sub11, sub12, sub13, sub14, sub15).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, IAsyncObservable<T11> source11, IAsyncObservable<T12> source12, IAsyncObservable<T13> source13, IAsyncObservable<T14> source14, IAsyncObservable<T15> source15, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task<TResult>> selector)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));
            if (source3 == null)
                throw new ArgumentNullException(nameof(source3));
            if (source4 == null)
                throw new ArgumentNullException(nameof(source4));
            if (source5 == null)
                throw new ArgumentNullException(nameof(source5));
            if (source6 == null)
                throw new ArgumentNullException(nameof(source6));
            if (source7 == null)
                throw new ArgumentNullException(nameof(source7));
            if (source8 == null)
                throw new ArgumentNullException(nameof(source8));
            if (source9 == null)
                throw new ArgumentNullException(nameof(source9));
            if (source10 == null)
                throw new ArgumentNullException(nameof(source10));
            if (source11 == null)
                throw new ArgumentNullException(nameof(source11));
            if (source12 == null)
                throw new ArgumentNullException(nameof(source12));
            if (source13 == null)
                throw new ArgumentNullException(nameof(source13));
            if (source14 == null)
                throw new ArgumentNullException(nameof(source14));
            if (source15 == null)
                throw new ArgumentNullException(nameof(source15));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TResult>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10, observer11, observer12, observer13, observer14, observer15) = AsyncObserver.CombineLatest(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub6 = source6.SubscribeSafeAsync(observer6).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub7 = source7.SubscribeSafeAsync(observer7).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub8 = source8.SubscribeSafeAsync(observer8).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub9 = source9.SubscribeSafeAsync(observer9).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub10 = source10.SubscribeSafeAsync(observer10).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub11 = source11.SubscribeSafeAsync(observer11).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub12 = source12.SubscribeSafeAsync(observer12).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub13 = source13.SubscribeSafeAsync(observer13).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub14 = source14.SubscribeSafeAsync(observer14).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub15 = source15.SubscribeSafeAsync(observer15).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5, sub6, sub7, sub8, sub9, sub10, sub11, sub12, sub13, sub14, sub15).ConfigureAwait(false);

                return d;
            });
        }

    }

    partial class AsyncObserver
    {
        public static (IAsyncObserver<T1>, IAsyncObserver<T2>) CombineLatest<T1, T2>(IAsyncObserver<(T1, T2)> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            bool allHasValue = false;

            bool hasValue1 = false;
            bool isDone1 = false;
            T1 latestValue1 = default(T1);
            bool hasValue2 = false;
            bool isDone2 = false;
            T2 latestValue2 = default(T2);

            var gate = new AsyncLock();

            return
            (
                Create<T1>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue1)
                            {
                                hasValue1 = true;
                                allHasValue = hasValue1 && hasValue2;
                            }

                            latestValue1 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2)).ConfigureAwait(false);
                            }
                            else if (isDone2)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone1 = true;

                            if (isDone1 && isDone2)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T2>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue2)
                            {
                                hasValue2 = true;
                                allHasValue = hasValue1 && hasValue2;
                            }

                            latestValue2 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2)).ConfigureAwait(false);
                            }
                            else if (isDone1)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone2 = true;

                            if (isDone1 && isDone2)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                )
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>) CombineLatest<T1, T2, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, TResult> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return CombineLatest<T1, T2, TResult>(observer, (x1, x2) => Task.FromResult(selector(x1, x2)));
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>) CombineLatest<T1, T2, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, Task<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            bool allHasValue = false;

            bool hasValue1 = false;
            bool isDone1 = false;
            T1 latestValue1 = default(T1);
            bool hasValue2 = false;
            bool isDone2 = false;
            T2 latestValue2 = default(T2);

            var gate = new AsyncLock();

            return
            (
                Create<T1>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue1)
                            {
                                hasValue1 = true;
                                allHasValue = hasValue1 && hasValue2;
                            }

                            latestValue1 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone2)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone1 = true;

                            if (isDone1 && isDone2)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T2>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue2)
                            {
                                hasValue2 = true;
                                allHasValue = hasValue1 && hasValue2;
                            }

                            latestValue2 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone2 = true;

                            if (isDone1 && isDone2)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                )
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>) CombineLatest<T1, T2, T3>(IAsyncObserver<(T1, T2, T3)> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            bool allHasValue = false;

            bool hasValue1 = false;
            bool isDone1 = false;
            T1 latestValue1 = default(T1);
            bool hasValue2 = false;
            bool isDone2 = false;
            T2 latestValue2 = default(T2);
            bool hasValue3 = false;
            bool isDone3 = false;
            T3 latestValue3 = default(T3);

            var gate = new AsyncLock();

            return
            (
                Create<T1>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue1)
                            {
                                hasValue1 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3;
                            }

                            latestValue1 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3)).ConfigureAwait(false);
                            }
                            else if (isDone2 && isDone3)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone1 = true;

                            if (isDone1 && isDone2 && isDone3)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T2>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue2)
                            {
                                hasValue2 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3;
                            }

                            latestValue2 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone3)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone2 = true;

                            if (isDone1 && isDone2 && isDone3)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T3>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue3)
                            {
                                hasValue3 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3;
                            }

                            latestValue3 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone3 = true;

                            if (isDone1 && isDone2 && isDone3)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                )
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>) CombineLatest<T1, T2, T3, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, TResult> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return CombineLatest<T1, T2, T3, TResult>(observer, (x1, x2, x3) => Task.FromResult(selector(x1, x2, x3)));
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>) CombineLatest<T1, T2, T3, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, Task<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            bool allHasValue = false;

            bool hasValue1 = false;
            bool isDone1 = false;
            T1 latestValue1 = default(T1);
            bool hasValue2 = false;
            bool isDone2 = false;
            T2 latestValue2 = default(T2);
            bool hasValue3 = false;
            bool isDone3 = false;
            T3 latestValue3 = default(T3);

            var gate = new AsyncLock();

            return
            (
                Create<T1>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue1)
                            {
                                hasValue1 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3;
                            }

                            latestValue1 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone2 && isDone3)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone1 = true;

                            if (isDone1 && isDone2 && isDone3)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T2>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue2)
                            {
                                hasValue2 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3;
                            }

                            latestValue2 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone3)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone2 = true;

                            if (isDone1 && isDone2 && isDone3)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T3>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue3)
                            {
                                hasValue3 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3;
                            }

                            latestValue3 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone3 = true;

                            if (isDone1 && isDone2 && isDone3)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                )
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>) CombineLatest<T1, T2, T3, T4>(IAsyncObserver<(T1, T2, T3, T4)> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            bool allHasValue = false;

            bool hasValue1 = false;
            bool isDone1 = false;
            T1 latestValue1 = default(T1);
            bool hasValue2 = false;
            bool isDone2 = false;
            T2 latestValue2 = default(T2);
            bool hasValue3 = false;
            bool isDone3 = false;
            T3 latestValue3 = default(T3);
            bool hasValue4 = false;
            bool isDone4 = false;
            T4 latestValue4 = default(T4);

            var gate = new AsyncLock();

            return
            (
                Create<T1>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue1)
                            {
                                hasValue1 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4;
                            }

                            latestValue1 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4)).ConfigureAwait(false);
                            }
                            else if (isDone2 && isDone3 && isDone4)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone1 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T2>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue2)
                            {
                                hasValue2 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4;
                            }

                            latestValue2 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone3 && isDone4)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone2 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T3>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue3)
                            {
                                hasValue3 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4;
                            }

                            latestValue3 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone4)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone3 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T4>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue4)
                            {
                                hasValue4 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4;
                            }

                            latestValue4 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone4 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                )
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>) CombineLatest<T1, T2, T3, T4, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, TResult> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return CombineLatest<T1, T2, T3, T4, TResult>(observer, (x1, x2, x3, x4) => Task.FromResult(selector(x1, x2, x3, x4)));
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>) CombineLatest<T1, T2, T3, T4, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, Task<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            bool allHasValue = false;

            bool hasValue1 = false;
            bool isDone1 = false;
            T1 latestValue1 = default(T1);
            bool hasValue2 = false;
            bool isDone2 = false;
            T2 latestValue2 = default(T2);
            bool hasValue3 = false;
            bool isDone3 = false;
            T3 latestValue3 = default(T3);
            bool hasValue4 = false;
            bool isDone4 = false;
            T4 latestValue4 = default(T4);

            var gate = new AsyncLock();

            return
            (
                Create<T1>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue1)
                            {
                                hasValue1 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4;
                            }

                            latestValue1 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone2 && isDone3 && isDone4)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone1 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T2>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue2)
                            {
                                hasValue2 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4;
                            }

                            latestValue2 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone3 && isDone4)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone2 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T3>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue3)
                            {
                                hasValue3 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4;
                            }

                            latestValue3 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone4)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone3 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T4>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue4)
                            {
                                hasValue4 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4;
                            }

                            latestValue4 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone4 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                )
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>) CombineLatest<T1, T2, T3, T4, T5>(IAsyncObserver<(T1, T2, T3, T4, T5)> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            bool allHasValue = false;

            bool hasValue1 = false;
            bool isDone1 = false;
            T1 latestValue1 = default(T1);
            bool hasValue2 = false;
            bool isDone2 = false;
            T2 latestValue2 = default(T2);
            bool hasValue3 = false;
            bool isDone3 = false;
            T3 latestValue3 = default(T3);
            bool hasValue4 = false;
            bool isDone4 = false;
            T4 latestValue4 = default(T4);
            bool hasValue5 = false;
            bool isDone5 = false;
            T5 latestValue5 = default(T5);

            var gate = new AsyncLock();

            return
            (
                Create<T1>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue1)
                            {
                                hasValue1 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5;
                            }

                            latestValue1 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5)).ConfigureAwait(false);
                            }
                            else if (isDone2 && isDone3 && isDone4 && isDone5)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone1 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T2>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue2)
                            {
                                hasValue2 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5;
                            }

                            latestValue2 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone3 && isDone4 && isDone5)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone2 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T3>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue3)
                            {
                                hasValue3 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5;
                            }

                            latestValue3 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone4 && isDone5)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone3 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T4>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue4)
                            {
                                hasValue4 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5;
                            }

                            latestValue4 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone5)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone4 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T5>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue5)
                            {
                                hasValue5 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5;
                            }

                            latestValue5 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone5 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                )
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>) CombineLatest<T1, T2, T3, T4, T5, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, TResult> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return CombineLatest<T1, T2, T3, T4, T5, TResult>(observer, (x1, x2, x3, x4, x5) => Task.FromResult(selector(x1, x2, x3, x4, x5)));
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>) CombineLatest<T1, T2, T3, T4, T5, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, Task<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            bool allHasValue = false;

            bool hasValue1 = false;
            bool isDone1 = false;
            T1 latestValue1 = default(T1);
            bool hasValue2 = false;
            bool isDone2 = false;
            T2 latestValue2 = default(T2);
            bool hasValue3 = false;
            bool isDone3 = false;
            T3 latestValue3 = default(T3);
            bool hasValue4 = false;
            bool isDone4 = false;
            T4 latestValue4 = default(T4);
            bool hasValue5 = false;
            bool isDone5 = false;
            T5 latestValue5 = default(T5);

            var gate = new AsyncLock();

            return
            (
                Create<T1>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue1)
                            {
                                hasValue1 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5;
                            }

                            latestValue1 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone2 && isDone3 && isDone4 && isDone5)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone1 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T2>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue2)
                            {
                                hasValue2 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5;
                            }

                            latestValue2 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone3 && isDone4 && isDone5)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone2 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T3>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue3)
                            {
                                hasValue3 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5;
                            }

                            latestValue3 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone4 && isDone5)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone3 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T4>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue4)
                            {
                                hasValue4 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5;
                            }

                            latestValue4 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone5)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone4 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T5>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue5)
                            {
                                hasValue5 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5;
                            }

                            latestValue5 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone5 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                )
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>) CombineLatest<T1, T2, T3, T4, T5, T6>(IAsyncObserver<(T1, T2, T3, T4, T5, T6)> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            bool allHasValue = false;

            bool hasValue1 = false;
            bool isDone1 = false;
            T1 latestValue1 = default(T1);
            bool hasValue2 = false;
            bool isDone2 = false;
            T2 latestValue2 = default(T2);
            bool hasValue3 = false;
            bool isDone3 = false;
            T3 latestValue3 = default(T3);
            bool hasValue4 = false;
            bool isDone4 = false;
            T4 latestValue4 = default(T4);
            bool hasValue5 = false;
            bool isDone5 = false;
            T5 latestValue5 = default(T5);
            bool hasValue6 = false;
            bool isDone6 = false;
            T6 latestValue6 = default(T6);

            var gate = new AsyncLock();

            return
            (
                Create<T1>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue1)
                            {
                                hasValue1 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6;
                            }

                            latestValue1 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6)).ConfigureAwait(false);
                            }
                            else if (isDone2 && isDone3 && isDone4 && isDone5 && isDone6)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone1 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T2>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue2)
                            {
                                hasValue2 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6;
                            }

                            latestValue2 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone3 && isDone4 && isDone5 && isDone6)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone2 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T3>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue3)
                            {
                                hasValue3 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6;
                            }

                            latestValue3 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone4 && isDone5 && isDone6)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone3 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T4>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue4)
                            {
                                hasValue4 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6;
                            }

                            latestValue4 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone5 && isDone6)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone4 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T5>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue5)
                            {
                                hasValue5 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6;
                            }

                            latestValue5 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone6)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone5 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T6>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue6)
                            {
                                hasValue6 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6;
                            }

                            latestValue6 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone6 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                )
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>) CombineLatest<T1, T2, T3, T4, T5, T6, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, TResult> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return CombineLatest<T1, T2, T3, T4, T5, T6, TResult>(observer, (x1, x2, x3, x4, x5, x6) => Task.FromResult(selector(x1, x2, x3, x4, x5, x6)));
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>) CombineLatest<T1, T2, T3, T4, T5, T6, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, Task<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            bool allHasValue = false;

            bool hasValue1 = false;
            bool isDone1 = false;
            T1 latestValue1 = default(T1);
            bool hasValue2 = false;
            bool isDone2 = false;
            T2 latestValue2 = default(T2);
            bool hasValue3 = false;
            bool isDone3 = false;
            T3 latestValue3 = default(T3);
            bool hasValue4 = false;
            bool isDone4 = false;
            T4 latestValue4 = default(T4);
            bool hasValue5 = false;
            bool isDone5 = false;
            T5 latestValue5 = default(T5);
            bool hasValue6 = false;
            bool isDone6 = false;
            T6 latestValue6 = default(T6);

            var gate = new AsyncLock();

            return
            (
                Create<T1>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue1)
                            {
                                hasValue1 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6;
                            }

                            latestValue1 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone2 && isDone3 && isDone4 && isDone5 && isDone6)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone1 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T2>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue2)
                            {
                                hasValue2 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6;
                            }

                            latestValue2 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone3 && isDone4 && isDone5 && isDone6)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone2 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T3>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue3)
                            {
                                hasValue3 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6;
                            }

                            latestValue3 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone4 && isDone5 && isDone6)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone3 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T4>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue4)
                            {
                                hasValue4 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6;
                            }

                            latestValue4 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone5 && isDone6)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone4 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T5>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue5)
                            {
                                hasValue5 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6;
                            }

                            latestValue5 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone6)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone5 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T6>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue6)
                            {
                                hasValue6 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6;
                            }

                            latestValue6 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone6 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                )
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>) CombineLatest<T1, T2, T3, T4, T5, T6, T7>(IAsyncObserver<(T1, T2, T3, T4, T5, T6, T7)> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            bool allHasValue = false;

            bool hasValue1 = false;
            bool isDone1 = false;
            T1 latestValue1 = default(T1);
            bool hasValue2 = false;
            bool isDone2 = false;
            T2 latestValue2 = default(T2);
            bool hasValue3 = false;
            bool isDone3 = false;
            T3 latestValue3 = default(T3);
            bool hasValue4 = false;
            bool isDone4 = false;
            T4 latestValue4 = default(T4);
            bool hasValue5 = false;
            bool isDone5 = false;
            T5 latestValue5 = default(T5);
            bool hasValue6 = false;
            bool isDone6 = false;
            T6 latestValue6 = default(T6);
            bool hasValue7 = false;
            bool isDone7 = false;
            T7 latestValue7 = default(T7);

            var gate = new AsyncLock();

            return
            (
                Create<T1>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue1)
                            {
                                hasValue1 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7;
                            }

                            latestValue1 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7)).ConfigureAwait(false);
                            }
                            else if (isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone1 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T2>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue2)
                            {
                                hasValue2 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7;
                            }

                            latestValue2 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone2 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T3>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue3)
                            {
                                hasValue3 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7;
                            }

                            latestValue3 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone4 && isDone5 && isDone6 && isDone7)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone3 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T4>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue4)
                            {
                                hasValue4 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7;
                            }

                            latestValue4 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone5 && isDone6 && isDone7)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone4 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T5>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue5)
                            {
                                hasValue5 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7;
                            }

                            latestValue5 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone6 && isDone7)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone5 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T6>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue6)
                            {
                                hasValue6 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7;
                            }

                            latestValue6 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone7)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone6 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T7>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue7)
                            {
                                hasValue7 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7;
                            }

                            latestValue7 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone7 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                )
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>) CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, TResult> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>(observer, (x1, x2, x3, x4, x5, x6, x7) => Task.FromResult(selector(x1, x2, x3, x4, x5, x6, x7)));
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>) CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, Task<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            bool allHasValue = false;

            bool hasValue1 = false;
            bool isDone1 = false;
            T1 latestValue1 = default(T1);
            bool hasValue2 = false;
            bool isDone2 = false;
            T2 latestValue2 = default(T2);
            bool hasValue3 = false;
            bool isDone3 = false;
            T3 latestValue3 = default(T3);
            bool hasValue4 = false;
            bool isDone4 = false;
            T4 latestValue4 = default(T4);
            bool hasValue5 = false;
            bool isDone5 = false;
            T5 latestValue5 = default(T5);
            bool hasValue6 = false;
            bool isDone6 = false;
            T6 latestValue6 = default(T6);
            bool hasValue7 = false;
            bool isDone7 = false;
            T7 latestValue7 = default(T7);

            var gate = new AsyncLock();

            return
            (
                Create<T1>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue1)
                            {
                                hasValue1 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7;
                            }

                            latestValue1 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone1 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T2>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue2)
                            {
                                hasValue2 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7;
                            }

                            latestValue2 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone2 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T3>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue3)
                            {
                                hasValue3 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7;
                            }

                            latestValue3 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone4 && isDone5 && isDone6 && isDone7)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone3 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T4>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue4)
                            {
                                hasValue4 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7;
                            }

                            latestValue4 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone5 && isDone6 && isDone7)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone4 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T5>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue5)
                            {
                                hasValue5 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7;
                            }

                            latestValue5 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone6 && isDone7)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone5 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T6>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue6)
                            {
                                hasValue6 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7;
                            }

                            latestValue6 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone7)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone6 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T7>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue7)
                            {
                                hasValue7 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7;
                            }

                            latestValue7 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone7 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                )
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>) CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8>(IAsyncObserver<(T1, T2, T3, T4, T5, T6, T7, T8)> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            bool allHasValue = false;

            bool hasValue1 = false;
            bool isDone1 = false;
            T1 latestValue1 = default(T1);
            bool hasValue2 = false;
            bool isDone2 = false;
            T2 latestValue2 = default(T2);
            bool hasValue3 = false;
            bool isDone3 = false;
            T3 latestValue3 = default(T3);
            bool hasValue4 = false;
            bool isDone4 = false;
            T4 latestValue4 = default(T4);
            bool hasValue5 = false;
            bool isDone5 = false;
            T5 latestValue5 = default(T5);
            bool hasValue6 = false;
            bool isDone6 = false;
            T6 latestValue6 = default(T6);
            bool hasValue7 = false;
            bool isDone7 = false;
            T7 latestValue7 = default(T7);
            bool hasValue8 = false;
            bool isDone8 = false;
            T8 latestValue8 = default(T8);

            var gate = new AsyncLock();

            return
            (
                Create<T1>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue1)
                            {
                                hasValue1 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8;
                            }

                            latestValue1 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8)).ConfigureAwait(false);
                            }
                            else if (isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone1 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T2>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue2)
                            {
                                hasValue2 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8;
                            }

                            latestValue2 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone2 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T3>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue3)
                            {
                                hasValue3 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8;
                            }

                            latestValue3 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone3 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T4>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue4)
                            {
                                hasValue4 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8;
                            }

                            latestValue4 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone5 && isDone6 && isDone7 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone4 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T5>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue5)
                            {
                                hasValue5 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8;
                            }

                            latestValue5 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone6 && isDone7 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone5 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T6>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue6)
                            {
                                hasValue6 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8;
                            }

                            latestValue6 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone7 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone6 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T7>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue7)
                            {
                                hasValue7 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8;
                            }

                            latestValue7 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone7 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T8>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue8)
                            {
                                hasValue8 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8;
                            }

                            latestValue8 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone8 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                )
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>) CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(observer, (x1, x2, x3, x4, x5, x6, x7, x8) => Task.FromResult(selector(x1, x2, x3, x4, x5, x6, x7, x8)));
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>) CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, Task<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            bool allHasValue = false;

            bool hasValue1 = false;
            bool isDone1 = false;
            T1 latestValue1 = default(T1);
            bool hasValue2 = false;
            bool isDone2 = false;
            T2 latestValue2 = default(T2);
            bool hasValue3 = false;
            bool isDone3 = false;
            T3 latestValue3 = default(T3);
            bool hasValue4 = false;
            bool isDone4 = false;
            T4 latestValue4 = default(T4);
            bool hasValue5 = false;
            bool isDone5 = false;
            T5 latestValue5 = default(T5);
            bool hasValue6 = false;
            bool isDone6 = false;
            T6 latestValue6 = default(T6);
            bool hasValue7 = false;
            bool isDone7 = false;
            T7 latestValue7 = default(T7);
            bool hasValue8 = false;
            bool isDone8 = false;
            T8 latestValue8 = default(T8);

            var gate = new AsyncLock();

            return
            (
                Create<T1>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue1)
                            {
                                hasValue1 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8;
                            }

                            latestValue1 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone1 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T2>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue2)
                            {
                                hasValue2 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8;
                            }

                            latestValue2 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone2 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T3>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue3)
                            {
                                hasValue3 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8;
                            }

                            latestValue3 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone3 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T4>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue4)
                            {
                                hasValue4 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8;
                            }

                            latestValue4 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone5 && isDone6 && isDone7 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone4 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T5>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue5)
                            {
                                hasValue5 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8;
                            }

                            latestValue5 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone6 && isDone7 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone5 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T6>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue6)
                            {
                                hasValue6 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8;
                            }

                            latestValue6 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone7 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone6 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T7>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue7)
                            {
                                hasValue7 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8;
                            }

                            latestValue7 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone7 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T8>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue8)
                            {
                                hasValue8 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8;
                            }

                            latestValue8 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone8 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                )
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>) CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9>(IAsyncObserver<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            bool allHasValue = false;

            bool hasValue1 = false;
            bool isDone1 = false;
            T1 latestValue1 = default(T1);
            bool hasValue2 = false;
            bool isDone2 = false;
            T2 latestValue2 = default(T2);
            bool hasValue3 = false;
            bool isDone3 = false;
            T3 latestValue3 = default(T3);
            bool hasValue4 = false;
            bool isDone4 = false;
            T4 latestValue4 = default(T4);
            bool hasValue5 = false;
            bool isDone5 = false;
            T5 latestValue5 = default(T5);
            bool hasValue6 = false;
            bool isDone6 = false;
            T6 latestValue6 = default(T6);
            bool hasValue7 = false;
            bool isDone7 = false;
            T7 latestValue7 = default(T7);
            bool hasValue8 = false;
            bool isDone8 = false;
            T8 latestValue8 = default(T8);
            bool hasValue9 = false;
            bool isDone9 = false;
            T9 latestValue9 = default(T9);

            var gate = new AsyncLock();

            return
            (
                Create<T1>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue1)
                            {
                                hasValue1 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9;
                            }

                            latestValue1 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9)).ConfigureAwait(false);
                            }
                            else if (isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone1 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T2>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue2)
                            {
                                hasValue2 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9;
                            }

                            latestValue2 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone2 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T3>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue3)
                            {
                                hasValue3 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9;
                            }

                            latestValue3 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone3 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T4>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue4)
                            {
                                hasValue4 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9;
                            }

                            latestValue4 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone4 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T5>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue5)
                            {
                                hasValue5 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9;
                            }

                            latestValue5 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone6 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone5 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T6>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue6)
                            {
                                hasValue6 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9;
                            }

                            latestValue6 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone6 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T7>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue7)
                            {
                                hasValue7 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9;
                            }

                            latestValue7 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone7 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T8>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue8)
                            {
                                hasValue8 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9;
                            }

                            latestValue8 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone8 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T9>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue9)
                            {
                                hasValue9 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9;
                            }

                            latestValue9 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone9 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                )
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>) CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(observer, (x1, x2, x3, x4, x5, x6, x7, x8, x9) => Task.FromResult(selector(x1, x2, x3, x4, x5, x6, x7, x8, x9)));
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>) CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            bool allHasValue = false;

            bool hasValue1 = false;
            bool isDone1 = false;
            T1 latestValue1 = default(T1);
            bool hasValue2 = false;
            bool isDone2 = false;
            T2 latestValue2 = default(T2);
            bool hasValue3 = false;
            bool isDone3 = false;
            T3 latestValue3 = default(T3);
            bool hasValue4 = false;
            bool isDone4 = false;
            T4 latestValue4 = default(T4);
            bool hasValue5 = false;
            bool isDone5 = false;
            T5 latestValue5 = default(T5);
            bool hasValue6 = false;
            bool isDone6 = false;
            T6 latestValue6 = default(T6);
            bool hasValue7 = false;
            bool isDone7 = false;
            T7 latestValue7 = default(T7);
            bool hasValue8 = false;
            bool isDone8 = false;
            T8 latestValue8 = default(T8);
            bool hasValue9 = false;
            bool isDone9 = false;
            T9 latestValue9 = default(T9);

            var gate = new AsyncLock();

            return
            (
                Create<T1>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue1)
                            {
                                hasValue1 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9;
                            }

                            latestValue1 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone1 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T2>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue2)
                            {
                                hasValue2 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9;
                            }

                            latestValue2 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone2 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T3>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue3)
                            {
                                hasValue3 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9;
                            }

                            latestValue3 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone3 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T4>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue4)
                            {
                                hasValue4 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9;
                            }

                            latestValue4 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone4 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T5>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue5)
                            {
                                hasValue5 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9;
                            }

                            latestValue5 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone6 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone5 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T6>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue6)
                            {
                                hasValue6 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9;
                            }

                            latestValue6 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone6 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T7>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue7)
                            {
                                hasValue7 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9;
                            }

                            latestValue7 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone7 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T8>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue8)
                            {
                                hasValue8 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9;
                            }

                            latestValue8 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone8 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T9>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue9)
                            {
                                hasValue9 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9;
                            }

                            latestValue9 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone9 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                )
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>) CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(IAsyncObserver<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            bool allHasValue = false;

            bool hasValue1 = false;
            bool isDone1 = false;
            T1 latestValue1 = default(T1);
            bool hasValue2 = false;
            bool isDone2 = false;
            T2 latestValue2 = default(T2);
            bool hasValue3 = false;
            bool isDone3 = false;
            T3 latestValue3 = default(T3);
            bool hasValue4 = false;
            bool isDone4 = false;
            T4 latestValue4 = default(T4);
            bool hasValue5 = false;
            bool isDone5 = false;
            T5 latestValue5 = default(T5);
            bool hasValue6 = false;
            bool isDone6 = false;
            T6 latestValue6 = default(T6);
            bool hasValue7 = false;
            bool isDone7 = false;
            T7 latestValue7 = default(T7);
            bool hasValue8 = false;
            bool isDone8 = false;
            T8 latestValue8 = default(T8);
            bool hasValue9 = false;
            bool isDone9 = false;
            T9 latestValue9 = default(T9);
            bool hasValue10 = false;
            bool isDone10 = false;
            T10 latestValue10 = default(T10);

            var gate = new AsyncLock();

            return
            (
                Create<T1>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue1)
                            {
                                hasValue1 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10;
                            }

                            latestValue1 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10)).ConfigureAwait(false);
                            }
                            else if (isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone1 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T2>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue2)
                            {
                                hasValue2 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10;
                            }

                            latestValue2 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone2 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T3>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue3)
                            {
                                hasValue3 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10;
                            }

                            latestValue3 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone3 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T4>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue4)
                            {
                                hasValue4 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10;
                            }

                            latestValue4 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone4 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T5>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue5)
                            {
                                hasValue5 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10;
                            }

                            latestValue5 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone5 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T6>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue6)
                            {
                                hasValue6 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10;
                            }

                            latestValue6 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone6 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T7>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue7)
                            {
                                hasValue7 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10;
                            }

                            latestValue7 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone7 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T8>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue8)
                            {
                                hasValue8 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10;
                            }

                            latestValue8 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone8 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T9>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue9)
                            {
                                hasValue9 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10;
                            }

                            latestValue9 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone9 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T10>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue10)
                            {
                                hasValue10 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10;
                            }

                            latestValue10 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone10 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                )
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>) CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(observer, (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10) => Task.FromResult(selector(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10)));
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>) CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            bool allHasValue = false;

            bool hasValue1 = false;
            bool isDone1 = false;
            T1 latestValue1 = default(T1);
            bool hasValue2 = false;
            bool isDone2 = false;
            T2 latestValue2 = default(T2);
            bool hasValue3 = false;
            bool isDone3 = false;
            T3 latestValue3 = default(T3);
            bool hasValue4 = false;
            bool isDone4 = false;
            T4 latestValue4 = default(T4);
            bool hasValue5 = false;
            bool isDone5 = false;
            T5 latestValue5 = default(T5);
            bool hasValue6 = false;
            bool isDone6 = false;
            T6 latestValue6 = default(T6);
            bool hasValue7 = false;
            bool isDone7 = false;
            T7 latestValue7 = default(T7);
            bool hasValue8 = false;
            bool isDone8 = false;
            T8 latestValue8 = default(T8);
            bool hasValue9 = false;
            bool isDone9 = false;
            T9 latestValue9 = default(T9);
            bool hasValue10 = false;
            bool isDone10 = false;
            T10 latestValue10 = default(T10);

            var gate = new AsyncLock();

            return
            (
                Create<T1>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue1)
                            {
                                hasValue1 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10;
                            }

                            latestValue1 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone1 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T2>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue2)
                            {
                                hasValue2 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10;
                            }

                            latestValue2 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone2 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T3>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue3)
                            {
                                hasValue3 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10;
                            }

                            latestValue3 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone3 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T4>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue4)
                            {
                                hasValue4 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10;
                            }

                            latestValue4 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone4 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T5>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue5)
                            {
                                hasValue5 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10;
                            }

                            latestValue5 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone5 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T6>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue6)
                            {
                                hasValue6 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10;
                            }

                            latestValue6 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone6 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T7>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue7)
                            {
                                hasValue7 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10;
                            }

                            latestValue7 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone7 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T8>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue8)
                            {
                                hasValue8 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10;
                            }

                            latestValue8 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone8 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T9>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue9)
                            {
                                hasValue9 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10;
                            }

                            latestValue9 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone9 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T10>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue10)
                            {
                                hasValue10 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10;
                            }

                            latestValue10 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone10 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                )
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>, IAsyncObserver<T11>) CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(IAsyncObserver<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            bool allHasValue = false;

            bool hasValue1 = false;
            bool isDone1 = false;
            T1 latestValue1 = default(T1);
            bool hasValue2 = false;
            bool isDone2 = false;
            T2 latestValue2 = default(T2);
            bool hasValue3 = false;
            bool isDone3 = false;
            T3 latestValue3 = default(T3);
            bool hasValue4 = false;
            bool isDone4 = false;
            T4 latestValue4 = default(T4);
            bool hasValue5 = false;
            bool isDone5 = false;
            T5 latestValue5 = default(T5);
            bool hasValue6 = false;
            bool isDone6 = false;
            T6 latestValue6 = default(T6);
            bool hasValue7 = false;
            bool isDone7 = false;
            T7 latestValue7 = default(T7);
            bool hasValue8 = false;
            bool isDone8 = false;
            T8 latestValue8 = default(T8);
            bool hasValue9 = false;
            bool isDone9 = false;
            T9 latestValue9 = default(T9);
            bool hasValue10 = false;
            bool isDone10 = false;
            T10 latestValue10 = default(T10);
            bool hasValue11 = false;
            bool isDone11 = false;
            T11 latestValue11 = default(T11);

            var gate = new AsyncLock();

            return
            (
                Create<T1>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue1)
                            {
                                hasValue1 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11;
                            }

                            latestValue1 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11)).ConfigureAwait(false);
                            }
                            else if (isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone1 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T2>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue2)
                            {
                                hasValue2 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11;
                            }

                            latestValue2 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone2 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T3>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue3)
                            {
                                hasValue3 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11;
                            }

                            latestValue3 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone3 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T4>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue4)
                            {
                                hasValue4 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11;
                            }

                            latestValue4 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone4 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T5>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue5)
                            {
                                hasValue5 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11;
                            }

                            latestValue5 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone5 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T6>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue6)
                            {
                                hasValue6 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11;
                            }

                            latestValue6 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone6 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T7>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue7)
                            {
                                hasValue7 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11;
                            }

                            latestValue7 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone7 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T8>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue8)
                            {
                                hasValue8 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11;
                            }

                            latestValue8 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone8 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T9>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue9)
                            {
                                hasValue9 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11;
                            }

                            latestValue9 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone9 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T10>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue10)
                            {
                                hasValue10 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11;
                            }

                            latestValue10 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone10 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T11>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue11)
                            {
                                hasValue11 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11;
                            }

                            latestValue11 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone11 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                )
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>, IAsyncObserver<T11>) CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(observer, (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11) => Task.FromResult(selector(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11)));
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>, IAsyncObserver<T11>) CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            bool allHasValue = false;

            bool hasValue1 = false;
            bool isDone1 = false;
            T1 latestValue1 = default(T1);
            bool hasValue2 = false;
            bool isDone2 = false;
            T2 latestValue2 = default(T2);
            bool hasValue3 = false;
            bool isDone3 = false;
            T3 latestValue3 = default(T3);
            bool hasValue4 = false;
            bool isDone4 = false;
            T4 latestValue4 = default(T4);
            bool hasValue5 = false;
            bool isDone5 = false;
            T5 latestValue5 = default(T5);
            bool hasValue6 = false;
            bool isDone6 = false;
            T6 latestValue6 = default(T6);
            bool hasValue7 = false;
            bool isDone7 = false;
            T7 latestValue7 = default(T7);
            bool hasValue8 = false;
            bool isDone8 = false;
            T8 latestValue8 = default(T8);
            bool hasValue9 = false;
            bool isDone9 = false;
            T9 latestValue9 = default(T9);
            bool hasValue10 = false;
            bool isDone10 = false;
            T10 latestValue10 = default(T10);
            bool hasValue11 = false;
            bool isDone11 = false;
            T11 latestValue11 = default(T11);

            var gate = new AsyncLock();

            return
            (
                Create<T1>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue1)
                            {
                                hasValue1 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11;
                            }

                            latestValue1 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone1 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T2>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue2)
                            {
                                hasValue2 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11;
                            }

                            latestValue2 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone2 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T3>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue3)
                            {
                                hasValue3 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11;
                            }

                            latestValue3 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone3 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T4>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue4)
                            {
                                hasValue4 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11;
                            }

                            latestValue4 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone4 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T5>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue5)
                            {
                                hasValue5 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11;
                            }

                            latestValue5 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone5 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T6>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue6)
                            {
                                hasValue6 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11;
                            }

                            latestValue6 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone6 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T7>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue7)
                            {
                                hasValue7 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11;
                            }

                            latestValue7 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone7 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T8>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue8)
                            {
                                hasValue8 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11;
                            }

                            latestValue8 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone8 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T9>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue9)
                            {
                                hasValue9 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11;
                            }

                            latestValue9 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone9 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T10>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue10)
                            {
                                hasValue10 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11;
                            }

                            latestValue10 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone10 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T11>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue11)
                            {
                                hasValue11 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11;
                            }

                            latestValue11 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone11 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                )
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>, IAsyncObserver<T11>, IAsyncObserver<T12>) CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(IAsyncObserver<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            bool allHasValue = false;

            bool hasValue1 = false;
            bool isDone1 = false;
            T1 latestValue1 = default(T1);
            bool hasValue2 = false;
            bool isDone2 = false;
            T2 latestValue2 = default(T2);
            bool hasValue3 = false;
            bool isDone3 = false;
            T3 latestValue3 = default(T3);
            bool hasValue4 = false;
            bool isDone4 = false;
            T4 latestValue4 = default(T4);
            bool hasValue5 = false;
            bool isDone5 = false;
            T5 latestValue5 = default(T5);
            bool hasValue6 = false;
            bool isDone6 = false;
            T6 latestValue6 = default(T6);
            bool hasValue7 = false;
            bool isDone7 = false;
            T7 latestValue7 = default(T7);
            bool hasValue8 = false;
            bool isDone8 = false;
            T8 latestValue8 = default(T8);
            bool hasValue9 = false;
            bool isDone9 = false;
            T9 latestValue9 = default(T9);
            bool hasValue10 = false;
            bool isDone10 = false;
            T10 latestValue10 = default(T10);
            bool hasValue11 = false;
            bool isDone11 = false;
            T11 latestValue11 = default(T11);
            bool hasValue12 = false;
            bool isDone12 = false;
            T12 latestValue12 = default(T12);

            var gate = new AsyncLock();

            return
            (
                Create<T1>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue1)
                            {
                                hasValue1 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12;
                            }

                            latestValue1 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12)).ConfigureAwait(false);
                            }
                            else if (isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone1 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T2>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue2)
                            {
                                hasValue2 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12;
                            }

                            latestValue2 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone2 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T3>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue3)
                            {
                                hasValue3 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12;
                            }

                            latestValue3 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone3 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T4>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue4)
                            {
                                hasValue4 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12;
                            }

                            latestValue4 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone4 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T5>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue5)
                            {
                                hasValue5 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12;
                            }

                            latestValue5 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone5 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T6>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue6)
                            {
                                hasValue6 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12;
                            }

                            latestValue6 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone6 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T7>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue7)
                            {
                                hasValue7 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12;
                            }

                            latestValue7 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone7 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T8>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue8)
                            {
                                hasValue8 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12;
                            }

                            latestValue8 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone8 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T9>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue9)
                            {
                                hasValue9 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12;
                            }

                            latestValue9 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone9 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T10>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue10)
                            {
                                hasValue10 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12;
                            }

                            latestValue10 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone10 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T11>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue11)
                            {
                                hasValue11 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12;
                            }

                            latestValue11 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone11 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T12>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue12)
                            {
                                hasValue12 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12;
                            }

                            latestValue12 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone12 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                )
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>, IAsyncObserver<T11>, IAsyncObserver<T12>) CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(observer, (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12) => Task.FromResult(selector(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12)));
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>, IAsyncObserver<T11>, IAsyncObserver<T12>) CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            bool allHasValue = false;

            bool hasValue1 = false;
            bool isDone1 = false;
            T1 latestValue1 = default(T1);
            bool hasValue2 = false;
            bool isDone2 = false;
            T2 latestValue2 = default(T2);
            bool hasValue3 = false;
            bool isDone3 = false;
            T3 latestValue3 = default(T3);
            bool hasValue4 = false;
            bool isDone4 = false;
            T4 latestValue4 = default(T4);
            bool hasValue5 = false;
            bool isDone5 = false;
            T5 latestValue5 = default(T5);
            bool hasValue6 = false;
            bool isDone6 = false;
            T6 latestValue6 = default(T6);
            bool hasValue7 = false;
            bool isDone7 = false;
            T7 latestValue7 = default(T7);
            bool hasValue8 = false;
            bool isDone8 = false;
            T8 latestValue8 = default(T8);
            bool hasValue9 = false;
            bool isDone9 = false;
            T9 latestValue9 = default(T9);
            bool hasValue10 = false;
            bool isDone10 = false;
            T10 latestValue10 = default(T10);
            bool hasValue11 = false;
            bool isDone11 = false;
            T11 latestValue11 = default(T11);
            bool hasValue12 = false;
            bool isDone12 = false;
            T12 latestValue12 = default(T12);

            var gate = new AsyncLock();

            return
            (
                Create<T1>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue1)
                            {
                                hasValue1 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12;
                            }

                            latestValue1 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone1 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T2>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue2)
                            {
                                hasValue2 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12;
                            }

                            latestValue2 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone2 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T3>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue3)
                            {
                                hasValue3 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12;
                            }

                            latestValue3 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone3 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T4>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue4)
                            {
                                hasValue4 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12;
                            }

                            latestValue4 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone4 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T5>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue5)
                            {
                                hasValue5 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12;
                            }

                            latestValue5 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone5 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T6>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue6)
                            {
                                hasValue6 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12;
                            }

                            latestValue6 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone6 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T7>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue7)
                            {
                                hasValue7 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12;
                            }

                            latestValue7 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone7 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T8>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue8)
                            {
                                hasValue8 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12;
                            }

                            latestValue8 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone8 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T9>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue9)
                            {
                                hasValue9 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12;
                            }

                            latestValue9 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone9 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T10>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue10)
                            {
                                hasValue10 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12;
                            }

                            latestValue10 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone10 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T11>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue11)
                            {
                                hasValue11 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12;
                            }

                            latestValue11 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone11 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T12>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue12)
                            {
                                hasValue12 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12;
                            }

                            latestValue12 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone12 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                )
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>, IAsyncObserver<T11>, IAsyncObserver<T12>, IAsyncObserver<T13>) CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(IAsyncObserver<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            bool allHasValue = false;

            bool hasValue1 = false;
            bool isDone1 = false;
            T1 latestValue1 = default(T1);
            bool hasValue2 = false;
            bool isDone2 = false;
            T2 latestValue2 = default(T2);
            bool hasValue3 = false;
            bool isDone3 = false;
            T3 latestValue3 = default(T3);
            bool hasValue4 = false;
            bool isDone4 = false;
            T4 latestValue4 = default(T4);
            bool hasValue5 = false;
            bool isDone5 = false;
            T5 latestValue5 = default(T5);
            bool hasValue6 = false;
            bool isDone6 = false;
            T6 latestValue6 = default(T6);
            bool hasValue7 = false;
            bool isDone7 = false;
            T7 latestValue7 = default(T7);
            bool hasValue8 = false;
            bool isDone8 = false;
            T8 latestValue8 = default(T8);
            bool hasValue9 = false;
            bool isDone9 = false;
            T9 latestValue9 = default(T9);
            bool hasValue10 = false;
            bool isDone10 = false;
            T10 latestValue10 = default(T10);
            bool hasValue11 = false;
            bool isDone11 = false;
            T11 latestValue11 = default(T11);
            bool hasValue12 = false;
            bool isDone12 = false;
            T12 latestValue12 = default(T12);
            bool hasValue13 = false;
            bool isDone13 = false;
            T13 latestValue13 = default(T13);

            var gate = new AsyncLock();

            return
            (
                Create<T1>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue1)
                            {
                                hasValue1 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13;
                            }

                            latestValue1 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13)).ConfigureAwait(false);
                            }
                            else if (isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone1 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T2>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue2)
                            {
                                hasValue2 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13;
                            }

                            latestValue2 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone2 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T3>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue3)
                            {
                                hasValue3 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13;
                            }

                            latestValue3 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone3 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T4>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue4)
                            {
                                hasValue4 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13;
                            }

                            latestValue4 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone4 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T5>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue5)
                            {
                                hasValue5 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13;
                            }

                            latestValue5 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone5 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T6>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue6)
                            {
                                hasValue6 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13;
                            }

                            latestValue6 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone6 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T7>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue7)
                            {
                                hasValue7 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13;
                            }

                            latestValue7 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone7 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T8>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue8)
                            {
                                hasValue8 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13;
                            }

                            latestValue8 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone8 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T9>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue9)
                            {
                                hasValue9 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13;
                            }

                            latestValue9 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone9 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T10>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue10)
                            {
                                hasValue10 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13;
                            }

                            latestValue10 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone10 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T11>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue11)
                            {
                                hasValue11 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13;
                            }

                            latestValue11 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone11 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T12>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue12)
                            {
                                hasValue12 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13;
                            }

                            latestValue12 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone12 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T13>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue13)
                            {
                                hasValue13 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13;
                            }

                            latestValue13 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone13 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                )
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>, IAsyncObserver<T11>, IAsyncObserver<T12>, IAsyncObserver<T13>) CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(observer, (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13) => Task.FromResult(selector(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13)));
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>, IAsyncObserver<T11>, IAsyncObserver<T12>, IAsyncObserver<T13>) CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            bool allHasValue = false;

            bool hasValue1 = false;
            bool isDone1 = false;
            T1 latestValue1 = default(T1);
            bool hasValue2 = false;
            bool isDone2 = false;
            T2 latestValue2 = default(T2);
            bool hasValue3 = false;
            bool isDone3 = false;
            T3 latestValue3 = default(T3);
            bool hasValue4 = false;
            bool isDone4 = false;
            T4 latestValue4 = default(T4);
            bool hasValue5 = false;
            bool isDone5 = false;
            T5 latestValue5 = default(T5);
            bool hasValue6 = false;
            bool isDone6 = false;
            T6 latestValue6 = default(T6);
            bool hasValue7 = false;
            bool isDone7 = false;
            T7 latestValue7 = default(T7);
            bool hasValue8 = false;
            bool isDone8 = false;
            T8 latestValue8 = default(T8);
            bool hasValue9 = false;
            bool isDone9 = false;
            T9 latestValue9 = default(T9);
            bool hasValue10 = false;
            bool isDone10 = false;
            T10 latestValue10 = default(T10);
            bool hasValue11 = false;
            bool isDone11 = false;
            T11 latestValue11 = default(T11);
            bool hasValue12 = false;
            bool isDone12 = false;
            T12 latestValue12 = default(T12);
            bool hasValue13 = false;
            bool isDone13 = false;
            T13 latestValue13 = default(T13);

            var gate = new AsyncLock();

            return
            (
                Create<T1>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue1)
                            {
                                hasValue1 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13;
                            }

                            latestValue1 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone1 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T2>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue2)
                            {
                                hasValue2 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13;
                            }

                            latestValue2 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone2 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T3>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue3)
                            {
                                hasValue3 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13;
                            }

                            latestValue3 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone3 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T4>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue4)
                            {
                                hasValue4 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13;
                            }

                            latestValue4 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone4 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T5>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue5)
                            {
                                hasValue5 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13;
                            }

                            latestValue5 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone5 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T6>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue6)
                            {
                                hasValue6 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13;
                            }

                            latestValue6 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone6 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T7>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue7)
                            {
                                hasValue7 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13;
                            }

                            latestValue7 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone7 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T8>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue8)
                            {
                                hasValue8 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13;
                            }

                            latestValue8 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone8 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T9>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue9)
                            {
                                hasValue9 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13;
                            }

                            latestValue9 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone9 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T10>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue10)
                            {
                                hasValue10 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13;
                            }

                            latestValue10 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone10 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T11>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue11)
                            {
                                hasValue11 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13;
                            }

                            latestValue11 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone11 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T12>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue12)
                            {
                                hasValue12 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13;
                            }

                            latestValue12 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone12 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T13>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue13)
                            {
                                hasValue13 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13;
                            }

                            latestValue13 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone13 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                )
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>, IAsyncObserver<T11>, IAsyncObserver<T12>, IAsyncObserver<T13>, IAsyncObserver<T14>) CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(IAsyncObserver<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            bool allHasValue = false;

            bool hasValue1 = false;
            bool isDone1 = false;
            T1 latestValue1 = default(T1);
            bool hasValue2 = false;
            bool isDone2 = false;
            T2 latestValue2 = default(T2);
            bool hasValue3 = false;
            bool isDone3 = false;
            T3 latestValue3 = default(T3);
            bool hasValue4 = false;
            bool isDone4 = false;
            T4 latestValue4 = default(T4);
            bool hasValue5 = false;
            bool isDone5 = false;
            T5 latestValue5 = default(T5);
            bool hasValue6 = false;
            bool isDone6 = false;
            T6 latestValue6 = default(T6);
            bool hasValue7 = false;
            bool isDone7 = false;
            T7 latestValue7 = default(T7);
            bool hasValue8 = false;
            bool isDone8 = false;
            T8 latestValue8 = default(T8);
            bool hasValue9 = false;
            bool isDone9 = false;
            T9 latestValue9 = default(T9);
            bool hasValue10 = false;
            bool isDone10 = false;
            T10 latestValue10 = default(T10);
            bool hasValue11 = false;
            bool isDone11 = false;
            T11 latestValue11 = default(T11);
            bool hasValue12 = false;
            bool isDone12 = false;
            T12 latestValue12 = default(T12);
            bool hasValue13 = false;
            bool isDone13 = false;
            T13 latestValue13 = default(T13);
            bool hasValue14 = false;
            bool isDone14 = false;
            T14 latestValue14 = default(T14);

            var gate = new AsyncLock();

            return
            (
                Create<T1>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue1)
                            {
                                hasValue1 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14;
                            }

                            latestValue1 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14)).ConfigureAwait(false);
                            }
                            else if (isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone1 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T2>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue2)
                            {
                                hasValue2 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14;
                            }

                            latestValue2 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone2 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T3>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue3)
                            {
                                hasValue3 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14;
                            }

                            latestValue3 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone3 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T4>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue4)
                            {
                                hasValue4 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14;
                            }

                            latestValue4 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone4 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T5>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue5)
                            {
                                hasValue5 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14;
                            }

                            latestValue5 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone5 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T6>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue6)
                            {
                                hasValue6 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14;
                            }

                            latestValue6 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone6 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T7>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue7)
                            {
                                hasValue7 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14;
                            }

                            latestValue7 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone7 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T8>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue8)
                            {
                                hasValue8 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14;
                            }

                            latestValue8 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone8 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T9>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue9)
                            {
                                hasValue9 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14;
                            }

                            latestValue9 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone9 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T10>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue10)
                            {
                                hasValue10 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14;
                            }

                            latestValue10 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone10 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T11>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue11)
                            {
                                hasValue11 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14;
                            }

                            latestValue11 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone11 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T12>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue12)
                            {
                                hasValue12 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14;
                            }

                            latestValue12 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone12 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T13>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue13)
                            {
                                hasValue13 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14;
                            }

                            latestValue13 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone13 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T14>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue14)
                            {
                                hasValue14 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14;
                            }

                            latestValue14 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone14 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                )
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>, IAsyncObserver<T11>, IAsyncObserver<T12>, IAsyncObserver<T13>, IAsyncObserver<T14>) CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(observer, (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14) => Task.FromResult(selector(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14)));
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>, IAsyncObserver<T11>, IAsyncObserver<T12>, IAsyncObserver<T13>, IAsyncObserver<T14>) CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            bool allHasValue = false;

            bool hasValue1 = false;
            bool isDone1 = false;
            T1 latestValue1 = default(T1);
            bool hasValue2 = false;
            bool isDone2 = false;
            T2 latestValue2 = default(T2);
            bool hasValue3 = false;
            bool isDone3 = false;
            T3 latestValue3 = default(T3);
            bool hasValue4 = false;
            bool isDone4 = false;
            T4 latestValue4 = default(T4);
            bool hasValue5 = false;
            bool isDone5 = false;
            T5 latestValue5 = default(T5);
            bool hasValue6 = false;
            bool isDone6 = false;
            T6 latestValue6 = default(T6);
            bool hasValue7 = false;
            bool isDone7 = false;
            T7 latestValue7 = default(T7);
            bool hasValue8 = false;
            bool isDone8 = false;
            T8 latestValue8 = default(T8);
            bool hasValue9 = false;
            bool isDone9 = false;
            T9 latestValue9 = default(T9);
            bool hasValue10 = false;
            bool isDone10 = false;
            T10 latestValue10 = default(T10);
            bool hasValue11 = false;
            bool isDone11 = false;
            T11 latestValue11 = default(T11);
            bool hasValue12 = false;
            bool isDone12 = false;
            T12 latestValue12 = default(T12);
            bool hasValue13 = false;
            bool isDone13 = false;
            T13 latestValue13 = default(T13);
            bool hasValue14 = false;
            bool isDone14 = false;
            T14 latestValue14 = default(T14);

            var gate = new AsyncLock();

            return
            (
                Create<T1>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue1)
                            {
                                hasValue1 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14;
                            }

                            latestValue1 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone1 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T2>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue2)
                            {
                                hasValue2 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14;
                            }

                            latestValue2 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone2 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T3>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue3)
                            {
                                hasValue3 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14;
                            }

                            latestValue3 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone3 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T4>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue4)
                            {
                                hasValue4 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14;
                            }

                            latestValue4 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone4 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T5>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue5)
                            {
                                hasValue5 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14;
                            }

                            latestValue5 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone5 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T6>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue6)
                            {
                                hasValue6 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14;
                            }

                            latestValue6 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone6 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T7>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue7)
                            {
                                hasValue7 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14;
                            }

                            latestValue7 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone7 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T8>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue8)
                            {
                                hasValue8 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14;
                            }

                            latestValue8 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone8 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T9>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue9)
                            {
                                hasValue9 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14;
                            }

                            latestValue9 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone9 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T10>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue10)
                            {
                                hasValue10 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14;
                            }

                            latestValue10 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone10 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T11>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue11)
                            {
                                hasValue11 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14;
                            }

                            latestValue11 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone11 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T12>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue12)
                            {
                                hasValue12 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14;
                            }

                            latestValue12 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone12 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T13>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue13)
                            {
                                hasValue13 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14;
                            }

                            latestValue13 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone13 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T14>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue14)
                            {
                                hasValue14 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14;
                            }

                            latestValue14 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone14 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                )
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>, IAsyncObserver<T11>, IAsyncObserver<T12>, IAsyncObserver<T13>, IAsyncObserver<T14>, IAsyncObserver<T15>) CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(IAsyncObserver<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            bool allHasValue = false;

            bool hasValue1 = false;
            bool isDone1 = false;
            T1 latestValue1 = default(T1);
            bool hasValue2 = false;
            bool isDone2 = false;
            T2 latestValue2 = default(T2);
            bool hasValue3 = false;
            bool isDone3 = false;
            T3 latestValue3 = default(T3);
            bool hasValue4 = false;
            bool isDone4 = false;
            T4 latestValue4 = default(T4);
            bool hasValue5 = false;
            bool isDone5 = false;
            T5 latestValue5 = default(T5);
            bool hasValue6 = false;
            bool isDone6 = false;
            T6 latestValue6 = default(T6);
            bool hasValue7 = false;
            bool isDone7 = false;
            T7 latestValue7 = default(T7);
            bool hasValue8 = false;
            bool isDone8 = false;
            T8 latestValue8 = default(T8);
            bool hasValue9 = false;
            bool isDone9 = false;
            T9 latestValue9 = default(T9);
            bool hasValue10 = false;
            bool isDone10 = false;
            T10 latestValue10 = default(T10);
            bool hasValue11 = false;
            bool isDone11 = false;
            T11 latestValue11 = default(T11);
            bool hasValue12 = false;
            bool isDone12 = false;
            T12 latestValue12 = default(T12);
            bool hasValue13 = false;
            bool isDone13 = false;
            T13 latestValue13 = default(T13);
            bool hasValue14 = false;
            bool isDone14 = false;
            T14 latestValue14 = default(T14);
            bool hasValue15 = false;
            bool isDone15 = false;
            T15 latestValue15 = default(T15);

            var gate = new AsyncLock();

            return
            (
                Create<T1>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue1)
                            {
                                hasValue1 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14 && hasValue15;
                            }

                            latestValue1 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14, latestValue15)).ConfigureAwait(false);
                            }
                            else if (isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone1 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T2>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue2)
                            {
                                hasValue2 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14 && hasValue15;
                            }

                            latestValue2 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14, latestValue15)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone2 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T3>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue3)
                            {
                                hasValue3 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14 && hasValue15;
                            }

                            latestValue3 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14, latestValue15)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone3 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T4>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue4)
                            {
                                hasValue4 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14 && hasValue15;
                            }

                            latestValue4 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14, latestValue15)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone4 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T5>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue5)
                            {
                                hasValue5 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14 && hasValue15;
                            }

                            latestValue5 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14, latestValue15)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone5 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T6>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue6)
                            {
                                hasValue6 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14 && hasValue15;
                            }

                            latestValue6 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14, latestValue15)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone6 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T7>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue7)
                            {
                                hasValue7 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14 && hasValue15;
                            }

                            latestValue7 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14, latestValue15)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone7 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T8>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue8)
                            {
                                hasValue8 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14 && hasValue15;
                            }

                            latestValue8 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14, latestValue15)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone8 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T9>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue9)
                            {
                                hasValue9 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14 && hasValue15;
                            }

                            latestValue9 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14, latestValue15)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone9 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T10>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue10)
                            {
                                hasValue10 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14 && hasValue15;
                            }

                            latestValue10 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14, latestValue15)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone10 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T11>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue11)
                            {
                                hasValue11 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14 && hasValue15;
                            }

                            latestValue11 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14, latestValue15)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone11 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T12>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue12)
                            {
                                hasValue12 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14 && hasValue15;
                            }

                            latestValue12 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14, latestValue15)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone12 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T13>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue13)
                            {
                                hasValue13 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14 && hasValue15;
                            }

                            latestValue13 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14, latestValue15)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone13 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T14>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue14)
                            {
                                hasValue14 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14 && hasValue15;
                            }

                            latestValue14 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14, latestValue15)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone14 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T15>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue15)
                            {
                                hasValue15 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14 && hasValue15;
                            }

                            latestValue15 = x;

                            if (allHasValue)
                            {
                                await observer.OnNextAsync((latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14, latestValue15)).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone15 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                )
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>, IAsyncObserver<T11>, IAsyncObserver<T12>, IAsyncObserver<T13>, IAsyncObserver<T14>, IAsyncObserver<T15>) CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(observer, (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14, x15) => Task.FromResult(selector(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14, x15)));
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>, IAsyncObserver<T11>, IAsyncObserver<T12>, IAsyncObserver<T13>, IAsyncObserver<T14>, IAsyncObserver<T15>) CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            bool allHasValue = false;

            bool hasValue1 = false;
            bool isDone1 = false;
            T1 latestValue1 = default(T1);
            bool hasValue2 = false;
            bool isDone2 = false;
            T2 latestValue2 = default(T2);
            bool hasValue3 = false;
            bool isDone3 = false;
            T3 latestValue3 = default(T3);
            bool hasValue4 = false;
            bool isDone4 = false;
            T4 latestValue4 = default(T4);
            bool hasValue5 = false;
            bool isDone5 = false;
            T5 latestValue5 = default(T5);
            bool hasValue6 = false;
            bool isDone6 = false;
            T6 latestValue6 = default(T6);
            bool hasValue7 = false;
            bool isDone7 = false;
            T7 latestValue7 = default(T7);
            bool hasValue8 = false;
            bool isDone8 = false;
            T8 latestValue8 = default(T8);
            bool hasValue9 = false;
            bool isDone9 = false;
            T9 latestValue9 = default(T9);
            bool hasValue10 = false;
            bool isDone10 = false;
            T10 latestValue10 = default(T10);
            bool hasValue11 = false;
            bool isDone11 = false;
            T11 latestValue11 = default(T11);
            bool hasValue12 = false;
            bool isDone12 = false;
            T12 latestValue12 = default(T12);
            bool hasValue13 = false;
            bool isDone13 = false;
            T13 latestValue13 = default(T13);
            bool hasValue14 = false;
            bool isDone14 = false;
            T14 latestValue14 = default(T14);
            bool hasValue15 = false;
            bool isDone15 = false;
            T15 latestValue15 = default(T15);

            var gate = new AsyncLock();

            return
            (
                Create<T1>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue1)
                            {
                                hasValue1 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14 && hasValue15;
                            }

                            latestValue1 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14, latestValue15).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone1 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T2>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue2)
                            {
                                hasValue2 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14 && hasValue15;
                            }

                            latestValue2 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14, latestValue15).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone2 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T3>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue3)
                            {
                                hasValue3 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14 && hasValue15;
                            }

                            latestValue3 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14, latestValue15).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone3 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T4>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue4)
                            {
                                hasValue4 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14 && hasValue15;
                            }

                            latestValue4 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14, latestValue15).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone4 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T5>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue5)
                            {
                                hasValue5 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14 && hasValue15;
                            }

                            latestValue5 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14, latestValue15).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone5 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T6>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue6)
                            {
                                hasValue6 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14 && hasValue15;
                            }

                            latestValue6 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14, latestValue15).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone6 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T7>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue7)
                            {
                                hasValue7 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14 && hasValue15;
                            }

                            latestValue7 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14, latestValue15).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone7 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T8>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue8)
                            {
                                hasValue8 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14 && hasValue15;
                            }

                            latestValue8 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14, latestValue15).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone8 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T9>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue9)
                            {
                                hasValue9 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14 && hasValue15;
                            }

                            latestValue9 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14, latestValue15).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone9 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T10>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue10)
                            {
                                hasValue10 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14 && hasValue15;
                            }

                            latestValue10 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14, latestValue15).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone10 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T11>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue11)
                            {
                                hasValue11 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14 && hasValue15;
                            }

                            latestValue11 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14, latestValue15).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone11 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T12>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue12)
                            {
                                hasValue12 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14 && hasValue15;
                            }

                            latestValue12 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14, latestValue15).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone12 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T13>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue13)
                            {
                                hasValue13 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14 && hasValue15;
                            }

                            latestValue13 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14, latestValue15).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone13 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T14>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue14)
                            {
                                hasValue14 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14 && hasValue15;
                            }

                            latestValue14 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14, latestValue15).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone14 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                ),
                Create<T15>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue15)
                            {
                                hasValue15 = true;
                                allHasValue = hasValue1 && hasValue2 && hasValue3 && hasValue4 && hasValue5 && hasValue6 && hasValue7 && hasValue8 && hasValue9 && hasValue10 && hasValue11 && hasValue12 && hasValue13 && hasValue14 && hasValue15;
                            }

                            latestValue15 = x;

                            if (allHasValue)
                            {
                                TResult res;
                                try
                                {
                                    res = await selector(latestValue1, latestValue2, latestValue3, latestValue4, latestValue5, latestValue6, latestValue7, latestValue8, latestValue9, latestValue10, latestValue11, latestValue12, latestValue13, latestValue14, latestValue15).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone15 = true;

                            if (isDone1 && isDone2 && isDone3 && isDone4 && isDone5 && isDone6 && isDone7 && isDone8 && isDone9 && isDone10 && isDone11 && isDone12 && isDone13 && isDone14 && isDone15)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                )
            );
        }

    }
}
