// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<(T1, T2)> Zip<T1, T2>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));
            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));

            return Create<(T1, T2)>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (observer1, observer2) = AsyncObserver.Zip(observer);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> Zip<T1, T2, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, Func<T1, T2, TResult> selector)
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

                var (observer1, observer2) = AsyncObserver.Zip(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> Zip<T1, T2, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, Func<T1, T2, Task<TResult>> selector)
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

                var (observer1, observer2) = AsyncObserver.Zip(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<(T1, T2, T3)> Zip<T1, T2, T3>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3)
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

                var (observer1, observer2, observer3) = AsyncObserver.Zip(observer);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> Zip<T1, T2, T3, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, Func<T1, T2, T3, TResult> selector)
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

                var (observer1, observer2, observer3) = AsyncObserver.Zip(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> Zip<T1, T2, T3, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, Func<T1, T2, T3, Task<TResult>> selector)
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

                var (observer1, observer2, observer3) = AsyncObserver.Zip(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<(T1, T2, T3, T4)> Zip<T1, T2, T3, T4>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4)
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

                var (observer1, observer2, observer3, observer4) = AsyncObserver.Zip(observer);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> Zip<T1, T2, T3, T4, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, Func<T1, T2, T3, T4, TResult> selector)
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

                var (observer1, observer2, observer3, observer4) = AsyncObserver.Zip(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> Zip<T1, T2, T3, T4, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, Func<T1, T2, T3, T4, Task<TResult>> selector)
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

                var (observer1, observer2, observer3, observer4) = AsyncObserver.Zip(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<(T1, T2, T3, T4, T5)> Zip<T1, T2, T3, T4, T5>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5)
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

                var (observer1, observer2, observer3, observer4, observer5) = AsyncObserver.Zip(observer);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> Zip<T1, T2, T3, T4, T5, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, Func<T1, T2, T3, T4, T5, TResult> selector)
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

                var (observer1, observer2, observer3, observer4, observer5) = AsyncObserver.Zip(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TResult> Zip<T1, T2, T3, T4, T5, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, Func<T1, T2, T3, T4, T5, Task<TResult>> selector)
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

                var (observer1, observer2, observer3, observer4, observer5) = AsyncObserver.Zip(observer, selector);

                var sub1 = source1.SubscribeSafeAsync(observer1).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub2 = source2.SubscribeSafeAsync(observer2).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub3 = source3.SubscribeSafeAsync(observer3).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub4 = source4.SubscribeSafeAsync(observer4).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();
                var sub5 = source5.SubscribeSafeAsync(observer5).ContinueWith(disposable => d.AddAsync(disposable.Result)).Unwrap();

                await Task.WhenAll(sub1, sub2, sub3, sub4, sub5).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<(T1, T2, T3, T4, T5, T6)> Zip<T1, T2, T3, T4, T5, T6>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6)
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

                var (observer1, observer2, observer3, observer4, observer5, observer6) = AsyncObserver.Zip(observer);

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

        public static IAsyncObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, Func<T1, T2, T3, T4, T5, T6, TResult> selector)
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

                var (observer1, observer2, observer3, observer4, observer5, observer6) = AsyncObserver.Zip(observer, selector);

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

        public static IAsyncObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, Func<T1, T2, T3, T4, T5, T6, Task<TResult>> selector)
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

                var (observer1, observer2, observer3, observer4, observer5, observer6) = AsyncObserver.Zip(observer, selector);

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

        public static IAsyncObservable<(T1, T2, T3, T4, T5, T6, T7)> Zip<T1, T2, T3, T4, T5, T6, T7>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7)
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

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7) = AsyncObserver.Zip(observer);

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

        public static IAsyncObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, Func<T1, T2, T3, T4, T5, T6, T7, TResult> selector)
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

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7) = AsyncObserver.Zip(observer, selector);

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

        public static IAsyncObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, Func<T1, T2, T3, T4, T5, T6, T7, Task<TResult>> selector)
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

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7) = AsyncObserver.Zip(observer, selector);

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

        public static IAsyncObservable<(T1, T2, T3, T4, T5, T6, T7, T8)> Zip<T1, T2, T3, T4, T5, T6, T7, T8>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8)
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

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8) = AsyncObserver.Zip(observer);

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

        public static IAsyncObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> selector)
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

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8) = AsyncObserver.Zip(observer, selector);

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

        public static IAsyncObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, Func<T1, T2, T3, T4, T5, T6, T7, T8, Task<TResult>> selector)
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

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8) = AsyncObserver.Zip(observer, selector);

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

        public static IAsyncObservable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9)
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

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9) = AsyncObserver.Zip(observer);

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

        public static IAsyncObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> selector)
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

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9) = AsyncObserver.Zip(observer, selector);

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

        public static IAsyncObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task<TResult>> selector)
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

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9) = AsyncObserver.Zip(observer, selector);

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

        public static IAsyncObservable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10)
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

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10) = AsyncObserver.Zip(observer);

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

        public static IAsyncObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> selector)
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

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10) = AsyncObserver.Zip(observer, selector);

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

        public static IAsyncObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task<TResult>> selector)
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

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10) = AsyncObserver.Zip(observer, selector);

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

        public static IAsyncObservable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, IAsyncObservable<T11> source11)
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

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10, observer11) = AsyncObserver.Zip(observer);

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

        public static IAsyncObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, IAsyncObservable<T11> source11, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> selector)
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

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10, observer11) = AsyncObserver.Zip(observer, selector);

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

        public static IAsyncObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, IAsyncObservable<T11> source11, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task<TResult>> selector)
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

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10, observer11) = AsyncObserver.Zip(observer, selector);

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

        public static IAsyncObservable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, IAsyncObservable<T11> source11, IAsyncObservable<T12> source12)
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

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10, observer11, observer12) = AsyncObserver.Zip(observer);

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

        public static IAsyncObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, IAsyncObservable<T11> source11, IAsyncObservable<T12> source12, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> selector)
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

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10, observer11, observer12) = AsyncObserver.Zip(observer, selector);

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

        public static IAsyncObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, IAsyncObservable<T11> source11, IAsyncObservable<T12> source12, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task<TResult>> selector)
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

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10, observer11, observer12) = AsyncObserver.Zip(observer, selector);

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

        public static IAsyncObservable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, IAsyncObservable<T11> source11, IAsyncObservable<T12> source12, IAsyncObservable<T13> source13)
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

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10, observer11, observer12, observer13) = AsyncObserver.Zip(observer);

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

        public static IAsyncObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, IAsyncObservable<T11> source11, IAsyncObservable<T12> source12, IAsyncObservable<T13> source13, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> selector)
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

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10, observer11, observer12, observer13) = AsyncObserver.Zip(observer, selector);

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

        public static IAsyncObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, IAsyncObservable<T11> source11, IAsyncObservable<T12> source12, IAsyncObservable<T13> source13, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task<TResult>> selector)
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

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10, observer11, observer12, observer13) = AsyncObserver.Zip(observer, selector);

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

        public static IAsyncObservable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, IAsyncObservable<T11> source11, IAsyncObservable<T12> source12, IAsyncObservable<T13> source13, IAsyncObservable<T14> source14)
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

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10, observer11, observer12, observer13, observer14) = AsyncObserver.Zip(observer);

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

        public static IAsyncObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, IAsyncObservable<T11> source11, IAsyncObservable<T12> source12, IAsyncObservable<T13> source13, IAsyncObservable<T14> source14, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> selector)
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

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10, observer11, observer12, observer13, observer14) = AsyncObserver.Zip(observer, selector);

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

        public static IAsyncObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, IAsyncObservable<T11> source11, IAsyncObservable<T12> source12, IAsyncObservable<T13> source13, IAsyncObservable<T14> source14, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task<TResult>> selector)
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

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10, observer11, observer12, observer13, observer14) = AsyncObserver.Zip(observer, selector);

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

        public static IAsyncObservable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, IAsyncObservable<T11> source11, IAsyncObservable<T12> source12, IAsyncObservable<T13> source13, IAsyncObservable<T14> source14, IAsyncObservable<T15> source15)
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

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10, observer11, observer12, observer13, observer14, observer15) = AsyncObserver.Zip(observer);

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

        public static IAsyncObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, IAsyncObservable<T11> source11, IAsyncObservable<T12> source12, IAsyncObservable<T13> source13, IAsyncObservable<T14> source14, IAsyncObservable<T15> source15, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> selector)
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

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10, observer11, observer12, observer13, observer14, observer15) = AsyncObserver.Zip(observer, selector);

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

        public static IAsyncObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this IAsyncObservable<T1> source1, IAsyncObservable<T2> source2, IAsyncObservable<T3> source3, IAsyncObservable<T4> source4, IAsyncObservable<T5> source5, IAsyncObservable<T6> source6, IAsyncObservable<T7> source7, IAsyncObservable<T8> source8, IAsyncObservable<T9> source9, IAsyncObservable<T10> source10, IAsyncObservable<T11> source11, IAsyncObservable<T12> source12, IAsyncObservable<T13> source13, IAsyncObservable<T14> source14, IAsyncObservable<T15> source15, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task<TResult>> selector)
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

                var (observer1, observer2, observer3, observer4, observer5, observer6, observer7, observer8, observer9, observer10, observer11, observer12, observer13, observer14, observer15) = AsyncObserver.Zip(observer, selector);

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
        public static (IAsyncObserver<T1>, IAsyncObserver<T2>) Zip<T1, T2>(IAsyncObserver<(T1, T2)> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var gate = new AsyncLock();

            var values1 = new Queue<T1>();
            var values2 = new Queue<T2>();
            var isDone = new bool[2];

            IAsyncObserver<T> CreateObserver<T>(int index, Queue<T> queue) =>
                Create<T>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            queue.Enqueue(x);

                            if (values1.Count > 0 && values2.Count > 0)
                            {
                                await observer.OnNextAsync((values1.Dequeue(), values2.Dequeue())).ConfigureAwait(false);
                            }
                            else
                            {
                                var allDone = true;

                                for (var i = 0; i < 2; i++)
                                {
                                    if (i != index && !isDone[i])
                                    {
                                        allDone = false;
                                        break;
                                    }
                                }

                                if (allDone)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
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
                            isDone[index] = true;

                            var allDone = true;

                            for (var i = 0; i < 2; i++)
                            {
                                if (!isDone[i])
                                {
                                    allDone = false;
                                    break;
                                }
                            }

                            if (allDone)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            return
            (
                CreateObserver<T1>(1, values1),
                CreateObserver<T2>(2, values2)
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>) Zip<T1, T2, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, TResult> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Zip<T1, T2, TResult>(observer, (x1, x2) => Task.FromResult(selector(x1, x2)));
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>) Zip<T1, T2, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, Task<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var gate = new AsyncLock();

            var values1 = new Queue<T1>();
            var values2 = new Queue<T2>();
            var isDone = new bool[2];

            IAsyncObserver<T> CreateObserver<T>(int index, Queue<T> queue) =>
                Create<T>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            queue.Enqueue(x);

                            if (values1.Count > 0 && values2.Count > 0)
                            {
                                TResult res;

                                try
                                {
                                    res = await selector(values1.Dequeue(), values2.Dequeue()).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else
                            {
                                var allDone = true;

                                for (var i = 0; i < 2; i++)
                                {
                                    if (i != index && !isDone[i])
                                    {
                                        allDone = false;
                                        break;
                                    }
                                }

                                if (allDone)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
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
                            isDone[index] = true;

                            var allDone = true;

                            for (var i = 0; i < 2; i++)
                            {
                                if (!isDone[i])
                                {
                                    allDone = false;
                                    break;
                                }
                            }

                            if (allDone)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            return
            (
                CreateObserver<T1>(1, values1),
                CreateObserver<T2>(2, values2)
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>) Zip<T1, T2, T3>(IAsyncObserver<(T1, T2, T3)> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var gate = new AsyncLock();

            var values1 = new Queue<T1>();
            var values2 = new Queue<T2>();
            var values3 = new Queue<T3>();
            var isDone = new bool[3];

            IAsyncObserver<T> CreateObserver<T>(int index, Queue<T> queue) =>
                Create<T>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            queue.Enqueue(x);

                            if (values1.Count > 0 && values2.Count > 0 && values3.Count > 0)
                            {
                                await observer.OnNextAsync((values1.Dequeue(), values2.Dequeue(), values3.Dequeue())).ConfigureAwait(false);
                            }
                            else
                            {
                                var allDone = true;

                                for (var i = 0; i < 3; i++)
                                {
                                    if (i != index && !isDone[i])
                                    {
                                        allDone = false;
                                        break;
                                    }
                                }

                                if (allDone)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
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
                            isDone[index] = true;

                            var allDone = true;

                            for (var i = 0; i < 3; i++)
                            {
                                if (!isDone[i])
                                {
                                    allDone = false;
                                    break;
                                }
                            }

                            if (allDone)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            return
            (
                CreateObserver<T1>(1, values1),
                CreateObserver<T2>(2, values2),
                CreateObserver<T3>(3, values3)
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>) Zip<T1, T2, T3, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, TResult> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Zip<T1, T2, T3, TResult>(observer, (x1, x2, x3) => Task.FromResult(selector(x1, x2, x3)));
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>) Zip<T1, T2, T3, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, Task<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var gate = new AsyncLock();

            var values1 = new Queue<T1>();
            var values2 = new Queue<T2>();
            var values3 = new Queue<T3>();
            var isDone = new bool[3];

            IAsyncObserver<T> CreateObserver<T>(int index, Queue<T> queue) =>
                Create<T>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            queue.Enqueue(x);

                            if (values1.Count > 0 && values2.Count > 0 && values3.Count > 0)
                            {
                                TResult res;

                                try
                                {
                                    res = await selector(values1.Dequeue(), values2.Dequeue(), values3.Dequeue()).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else
                            {
                                var allDone = true;

                                for (var i = 0; i < 3; i++)
                                {
                                    if (i != index && !isDone[i])
                                    {
                                        allDone = false;
                                        break;
                                    }
                                }

                                if (allDone)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
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
                            isDone[index] = true;

                            var allDone = true;

                            for (var i = 0; i < 3; i++)
                            {
                                if (!isDone[i])
                                {
                                    allDone = false;
                                    break;
                                }
                            }

                            if (allDone)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            return
            (
                CreateObserver<T1>(1, values1),
                CreateObserver<T2>(2, values2),
                CreateObserver<T3>(3, values3)
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>) Zip<T1, T2, T3, T4>(IAsyncObserver<(T1, T2, T3, T4)> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var gate = new AsyncLock();

            var values1 = new Queue<T1>();
            var values2 = new Queue<T2>();
            var values3 = new Queue<T3>();
            var values4 = new Queue<T4>();
            var isDone = new bool[4];

            IAsyncObserver<T> CreateObserver<T>(int index, Queue<T> queue) =>
                Create<T>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            queue.Enqueue(x);

                            if (values1.Count > 0 && values2.Count > 0 && values3.Count > 0 && values4.Count > 0)
                            {
                                await observer.OnNextAsync((values1.Dequeue(), values2.Dequeue(), values3.Dequeue(), values4.Dequeue())).ConfigureAwait(false);
                            }
                            else
                            {
                                var allDone = true;

                                for (var i = 0; i < 4; i++)
                                {
                                    if (i != index && !isDone[i])
                                    {
                                        allDone = false;
                                        break;
                                    }
                                }

                                if (allDone)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
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
                            isDone[index] = true;

                            var allDone = true;

                            for (var i = 0; i < 4; i++)
                            {
                                if (!isDone[i])
                                {
                                    allDone = false;
                                    break;
                                }
                            }

                            if (allDone)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            return
            (
                CreateObserver<T1>(1, values1),
                CreateObserver<T2>(2, values2),
                CreateObserver<T3>(3, values3),
                CreateObserver<T4>(4, values4)
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>) Zip<T1, T2, T3, T4, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, TResult> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Zip<T1, T2, T3, T4, TResult>(observer, (x1, x2, x3, x4) => Task.FromResult(selector(x1, x2, x3, x4)));
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>) Zip<T1, T2, T3, T4, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, Task<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var gate = new AsyncLock();

            var values1 = new Queue<T1>();
            var values2 = new Queue<T2>();
            var values3 = new Queue<T3>();
            var values4 = new Queue<T4>();
            var isDone = new bool[4];

            IAsyncObserver<T> CreateObserver<T>(int index, Queue<T> queue) =>
                Create<T>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            queue.Enqueue(x);

                            if (values1.Count > 0 && values2.Count > 0 && values3.Count > 0 && values4.Count > 0)
                            {
                                TResult res;

                                try
                                {
                                    res = await selector(values1.Dequeue(), values2.Dequeue(), values3.Dequeue(), values4.Dequeue()).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else
                            {
                                var allDone = true;

                                for (var i = 0; i < 4; i++)
                                {
                                    if (i != index && !isDone[i])
                                    {
                                        allDone = false;
                                        break;
                                    }
                                }

                                if (allDone)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
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
                            isDone[index] = true;

                            var allDone = true;

                            for (var i = 0; i < 4; i++)
                            {
                                if (!isDone[i])
                                {
                                    allDone = false;
                                    break;
                                }
                            }

                            if (allDone)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            return
            (
                CreateObserver<T1>(1, values1),
                CreateObserver<T2>(2, values2),
                CreateObserver<T3>(3, values3),
                CreateObserver<T4>(4, values4)
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>) Zip<T1, T2, T3, T4, T5>(IAsyncObserver<(T1, T2, T3, T4, T5)> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var gate = new AsyncLock();

            var values1 = new Queue<T1>();
            var values2 = new Queue<T2>();
            var values3 = new Queue<T3>();
            var values4 = new Queue<T4>();
            var values5 = new Queue<T5>();
            var isDone = new bool[5];

            IAsyncObserver<T> CreateObserver<T>(int index, Queue<T> queue) =>
                Create<T>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            queue.Enqueue(x);

                            if (values1.Count > 0 && values2.Count > 0 && values3.Count > 0 && values4.Count > 0 && values5.Count > 0)
                            {
                                await observer.OnNextAsync((values1.Dequeue(), values2.Dequeue(), values3.Dequeue(), values4.Dequeue(), values5.Dequeue())).ConfigureAwait(false);
                            }
                            else
                            {
                                var allDone = true;

                                for (var i = 0; i < 5; i++)
                                {
                                    if (i != index && !isDone[i])
                                    {
                                        allDone = false;
                                        break;
                                    }
                                }

                                if (allDone)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
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
                            isDone[index] = true;

                            var allDone = true;

                            for (var i = 0; i < 5; i++)
                            {
                                if (!isDone[i])
                                {
                                    allDone = false;
                                    break;
                                }
                            }

                            if (allDone)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            return
            (
                CreateObserver<T1>(1, values1),
                CreateObserver<T2>(2, values2),
                CreateObserver<T3>(3, values3),
                CreateObserver<T4>(4, values4),
                CreateObserver<T5>(5, values5)
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>) Zip<T1, T2, T3, T4, T5, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, TResult> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Zip<T1, T2, T3, T4, T5, TResult>(observer, (x1, x2, x3, x4, x5) => Task.FromResult(selector(x1, x2, x3, x4, x5)));
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>) Zip<T1, T2, T3, T4, T5, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, Task<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var gate = new AsyncLock();

            var values1 = new Queue<T1>();
            var values2 = new Queue<T2>();
            var values3 = new Queue<T3>();
            var values4 = new Queue<T4>();
            var values5 = new Queue<T5>();
            var isDone = new bool[5];

            IAsyncObserver<T> CreateObserver<T>(int index, Queue<T> queue) =>
                Create<T>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            queue.Enqueue(x);

                            if (values1.Count > 0 && values2.Count > 0 && values3.Count > 0 && values4.Count > 0 && values5.Count > 0)
                            {
                                TResult res;

                                try
                                {
                                    res = await selector(values1.Dequeue(), values2.Dequeue(), values3.Dequeue(), values4.Dequeue(), values5.Dequeue()).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else
                            {
                                var allDone = true;

                                for (var i = 0; i < 5; i++)
                                {
                                    if (i != index && !isDone[i])
                                    {
                                        allDone = false;
                                        break;
                                    }
                                }

                                if (allDone)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
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
                            isDone[index] = true;

                            var allDone = true;

                            for (var i = 0; i < 5; i++)
                            {
                                if (!isDone[i])
                                {
                                    allDone = false;
                                    break;
                                }
                            }

                            if (allDone)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            return
            (
                CreateObserver<T1>(1, values1),
                CreateObserver<T2>(2, values2),
                CreateObserver<T3>(3, values3),
                CreateObserver<T4>(4, values4),
                CreateObserver<T5>(5, values5)
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>) Zip<T1, T2, T3, T4, T5, T6>(IAsyncObserver<(T1, T2, T3, T4, T5, T6)> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var gate = new AsyncLock();

            var values1 = new Queue<T1>();
            var values2 = new Queue<T2>();
            var values3 = new Queue<T3>();
            var values4 = new Queue<T4>();
            var values5 = new Queue<T5>();
            var values6 = new Queue<T6>();
            var isDone = new bool[6];

            IAsyncObserver<T> CreateObserver<T>(int index, Queue<T> queue) =>
                Create<T>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            queue.Enqueue(x);

                            if (values1.Count > 0 && values2.Count > 0 && values3.Count > 0 && values4.Count > 0 && values5.Count > 0 && values6.Count > 0)
                            {
                                await observer.OnNextAsync((values1.Dequeue(), values2.Dequeue(), values3.Dequeue(), values4.Dequeue(), values5.Dequeue(), values6.Dequeue())).ConfigureAwait(false);
                            }
                            else
                            {
                                var allDone = true;

                                for (var i = 0; i < 6; i++)
                                {
                                    if (i != index && !isDone[i])
                                    {
                                        allDone = false;
                                        break;
                                    }
                                }

                                if (allDone)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
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
                            isDone[index] = true;

                            var allDone = true;

                            for (var i = 0; i < 6; i++)
                            {
                                if (!isDone[i])
                                {
                                    allDone = false;
                                    break;
                                }
                            }

                            if (allDone)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            return
            (
                CreateObserver<T1>(1, values1),
                CreateObserver<T2>(2, values2),
                CreateObserver<T3>(3, values3),
                CreateObserver<T4>(4, values4),
                CreateObserver<T5>(5, values5),
                CreateObserver<T6>(6, values6)
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>) Zip<T1, T2, T3, T4, T5, T6, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, TResult> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Zip<T1, T2, T3, T4, T5, T6, TResult>(observer, (x1, x2, x3, x4, x5, x6) => Task.FromResult(selector(x1, x2, x3, x4, x5, x6)));
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>) Zip<T1, T2, T3, T4, T5, T6, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, Task<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var gate = new AsyncLock();

            var values1 = new Queue<T1>();
            var values2 = new Queue<T2>();
            var values3 = new Queue<T3>();
            var values4 = new Queue<T4>();
            var values5 = new Queue<T5>();
            var values6 = new Queue<T6>();
            var isDone = new bool[6];

            IAsyncObserver<T> CreateObserver<T>(int index, Queue<T> queue) =>
                Create<T>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            queue.Enqueue(x);

                            if (values1.Count > 0 && values2.Count > 0 && values3.Count > 0 && values4.Count > 0 && values5.Count > 0 && values6.Count > 0)
                            {
                                TResult res;

                                try
                                {
                                    res = await selector(values1.Dequeue(), values2.Dequeue(), values3.Dequeue(), values4.Dequeue(), values5.Dequeue(), values6.Dequeue()).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else
                            {
                                var allDone = true;

                                for (var i = 0; i < 6; i++)
                                {
                                    if (i != index && !isDone[i])
                                    {
                                        allDone = false;
                                        break;
                                    }
                                }

                                if (allDone)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
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
                            isDone[index] = true;

                            var allDone = true;

                            for (var i = 0; i < 6; i++)
                            {
                                if (!isDone[i])
                                {
                                    allDone = false;
                                    break;
                                }
                            }

                            if (allDone)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            return
            (
                CreateObserver<T1>(1, values1),
                CreateObserver<T2>(2, values2),
                CreateObserver<T3>(3, values3),
                CreateObserver<T4>(4, values4),
                CreateObserver<T5>(5, values5),
                CreateObserver<T6>(6, values6)
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>) Zip<T1, T2, T3, T4, T5, T6, T7>(IAsyncObserver<(T1, T2, T3, T4, T5, T6, T7)> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var gate = new AsyncLock();

            var values1 = new Queue<T1>();
            var values2 = new Queue<T2>();
            var values3 = new Queue<T3>();
            var values4 = new Queue<T4>();
            var values5 = new Queue<T5>();
            var values6 = new Queue<T6>();
            var values7 = new Queue<T7>();
            var isDone = new bool[7];

            IAsyncObserver<T> CreateObserver<T>(int index, Queue<T> queue) =>
                Create<T>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            queue.Enqueue(x);

                            if (values1.Count > 0 && values2.Count > 0 && values3.Count > 0 && values4.Count > 0 && values5.Count > 0 && values6.Count > 0 && values7.Count > 0)
                            {
                                await observer.OnNextAsync((values1.Dequeue(), values2.Dequeue(), values3.Dequeue(), values4.Dequeue(), values5.Dequeue(), values6.Dequeue(), values7.Dequeue())).ConfigureAwait(false);
                            }
                            else
                            {
                                var allDone = true;

                                for (var i = 0; i < 7; i++)
                                {
                                    if (i != index && !isDone[i])
                                    {
                                        allDone = false;
                                        break;
                                    }
                                }

                                if (allDone)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
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
                            isDone[index] = true;

                            var allDone = true;

                            for (var i = 0; i < 7; i++)
                            {
                                if (!isDone[i])
                                {
                                    allDone = false;
                                    break;
                                }
                            }

                            if (allDone)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            return
            (
                CreateObserver<T1>(1, values1),
                CreateObserver<T2>(2, values2),
                CreateObserver<T3>(3, values3),
                CreateObserver<T4>(4, values4),
                CreateObserver<T5>(5, values5),
                CreateObserver<T6>(6, values6),
                CreateObserver<T7>(7, values7)
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>) Zip<T1, T2, T3, T4, T5, T6, T7, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, TResult> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Zip<T1, T2, T3, T4, T5, T6, T7, TResult>(observer, (x1, x2, x3, x4, x5, x6, x7) => Task.FromResult(selector(x1, x2, x3, x4, x5, x6, x7)));
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>) Zip<T1, T2, T3, T4, T5, T6, T7, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, Task<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var gate = new AsyncLock();

            var values1 = new Queue<T1>();
            var values2 = new Queue<T2>();
            var values3 = new Queue<T3>();
            var values4 = new Queue<T4>();
            var values5 = new Queue<T5>();
            var values6 = new Queue<T6>();
            var values7 = new Queue<T7>();
            var isDone = new bool[7];

            IAsyncObserver<T> CreateObserver<T>(int index, Queue<T> queue) =>
                Create<T>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            queue.Enqueue(x);

                            if (values1.Count > 0 && values2.Count > 0 && values3.Count > 0 && values4.Count > 0 && values5.Count > 0 && values6.Count > 0 && values7.Count > 0)
                            {
                                TResult res;

                                try
                                {
                                    res = await selector(values1.Dequeue(), values2.Dequeue(), values3.Dequeue(), values4.Dequeue(), values5.Dequeue(), values6.Dequeue(), values7.Dequeue()).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else
                            {
                                var allDone = true;

                                for (var i = 0; i < 7; i++)
                                {
                                    if (i != index && !isDone[i])
                                    {
                                        allDone = false;
                                        break;
                                    }
                                }

                                if (allDone)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
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
                            isDone[index] = true;

                            var allDone = true;

                            for (var i = 0; i < 7; i++)
                            {
                                if (!isDone[i])
                                {
                                    allDone = false;
                                    break;
                                }
                            }

                            if (allDone)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            return
            (
                CreateObserver<T1>(1, values1),
                CreateObserver<T2>(2, values2),
                CreateObserver<T3>(3, values3),
                CreateObserver<T4>(4, values4),
                CreateObserver<T5>(5, values5),
                CreateObserver<T6>(6, values6),
                CreateObserver<T7>(7, values7)
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>) Zip<T1, T2, T3, T4, T5, T6, T7, T8>(IAsyncObserver<(T1, T2, T3, T4, T5, T6, T7, T8)> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var gate = new AsyncLock();

            var values1 = new Queue<T1>();
            var values2 = new Queue<T2>();
            var values3 = new Queue<T3>();
            var values4 = new Queue<T4>();
            var values5 = new Queue<T5>();
            var values6 = new Queue<T6>();
            var values7 = new Queue<T7>();
            var values8 = new Queue<T8>();
            var isDone = new bool[8];

            IAsyncObserver<T> CreateObserver<T>(int index, Queue<T> queue) =>
                Create<T>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            queue.Enqueue(x);

                            if (values1.Count > 0 && values2.Count > 0 && values3.Count > 0 && values4.Count > 0 && values5.Count > 0 && values6.Count > 0 && values7.Count > 0 && values8.Count > 0)
                            {
                                await observer.OnNextAsync((values1.Dequeue(), values2.Dequeue(), values3.Dequeue(), values4.Dequeue(), values5.Dequeue(), values6.Dequeue(), values7.Dequeue(), values8.Dequeue())).ConfigureAwait(false);
                            }
                            else
                            {
                                var allDone = true;

                                for (var i = 0; i < 8; i++)
                                {
                                    if (i != index && !isDone[i])
                                    {
                                        allDone = false;
                                        break;
                                    }
                                }

                                if (allDone)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
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
                            isDone[index] = true;

                            var allDone = true;

                            for (var i = 0; i < 8; i++)
                            {
                                if (!isDone[i])
                                {
                                    allDone = false;
                                    break;
                                }
                            }

                            if (allDone)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            return
            (
                CreateObserver<T1>(1, values1),
                CreateObserver<T2>(2, values2),
                CreateObserver<T3>(3, values3),
                CreateObserver<T4>(4, values4),
                CreateObserver<T5>(5, values5),
                CreateObserver<T6>(6, values6),
                CreateObserver<T7>(7, values7),
                CreateObserver<T8>(8, values8)
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>) Zip<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Zip<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(observer, (x1, x2, x3, x4, x5, x6, x7, x8) => Task.FromResult(selector(x1, x2, x3, x4, x5, x6, x7, x8)));
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>) Zip<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, Task<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var gate = new AsyncLock();

            var values1 = new Queue<T1>();
            var values2 = new Queue<T2>();
            var values3 = new Queue<T3>();
            var values4 = new Queue<T4>();
            var values5 = new Queue<T5>();
            var values6 = new Queue<T6>();
            var values7 = new Queue<T7>();
            var values8 = new Queue<T8>();
            var isDone = new bool[8];

            IAsyncObserver<T> CreateObserver<T>(int index, Queue<T> queue) =>
                Create<T>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            queue.Enqueue(x);

                            if (values1.Count > 0 && values2.Count > 0 && values3.Count > 0 && values4.Count > 0 && values5.Count > 0 && values6.Count > 0 && values7.Count > 0 && values8.Count > 0)
                            {
                                TResult res;

                                try
                                {
                                    res = await selector(values1.Dequeue(), values2.Dequeue(), values3.Dequeue(), values4.Dequeue(), values5.Dequeue(), values6.Dequeue(), values7.Dequeue(), values8.Dequeue()).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else
                            {
                                var allDone = true;

                                for (var i = 0; i < 8; i++)
                                {
                                    if (i != index && !isDone[i])
                                    {
                                        allDone = false;
                                        break;
                                    }
                                }

                                if (allDone)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
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
                            isDone[index] = true;

                            var allDone = true;

                            for (var i = 0; i < 8; i++)
                            {
                                if (!isDone[i])
                                {
                                    allDone = false;
                                    break;
                                }
                            }

                            if (allDone)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            return
            (
                CreateObserver<T1>(1, values1),
                CreateObserver<T2>(2, values2),
                CreateObserver<T3>(3, values3),
                CreateObserver<T4>(4, values4),
                CreateObserver<T5>(5, values5),
                CreateObserver<T6>(6, values6),
                CreateObserver<T7>(7, values7),
                CreateObserver<T8>(8, values8)
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>) Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9>(IAsyncObserver<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var gate = new AsyncLock();

            var values1 = new Queue<T1>();
            var values2 = new Queue<T2>();
            var values3 = new Queue<T3>();
            var values4 = new Queue<T4>();
            var values5 = new Queue<T5>();
            var values6 = new Queue<T6>();
            var values7 = new Queue<T7>();
            var values8 = new Queue<T8>();
            var values9 = new Queue<T9>();
            var isDone = new bool[9];

            IAsyncObserver<T> CreateObserver<T>(int index, Queue<T> queue) =>
                Create<T>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            queue.Enqueue(x);

                            if (values1.Count > 0 && values2.Count > 0 && values3.Count > 0 && values4.Count > 0 && values5.Count > 0 && values6.Count > 0 && values7.Count > 0 && values8.Count > 0 && values9.Count > 0)
                            {
                                await observer.OnNextAsync((values1.Dequeue(), values2.Dequeue(), values3.Dequeue(), values4.Dequeue(), values5.Dequeue(), values6.Dequeue(), values7.Dequeue(), values8.Dequeue(), values9.Dequeue())).ConfigureAwait(false);
                            }
                            else
                            {
                                var allDone = true;

                                for (var i = 0; i < 9; i++)
                                {
                                    if (i != index && !isDone[i])
                                    {
                                        allDone = false;
                                        break;
                                    }
                                }

                                if (allDone)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
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
                            isDone[index] = true;

                            var allDone = true;

                            for (var i = 0; i < 9; i++)
                            {
                                if (!isDone[i])
                                {
                                    allDone = false;
                                    break;
                                }
                            }

                            if (allDone)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            return
            (
                CreateObserver<T1>(1, values1),
                CreateObserver<T2>(2, values2),
                CreateObserver<T3>(3, values3),
                CreateObserver<T4>(4, values4),
                CreateObserver<T5>(5, values5),
                CreateObserver<T6>(6, values6),
                CreateObserver<T7>(7, values7),
                CreateObserver<T8>(8, values8),
                CreateObserver<T9>(9, values9)
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>) Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(observer, (x1, x2, x3, x4, x5, x6, x7, x8, x9) => Task.FromResult(selector(x1, x2, x3, x4, x5, x6, x7, x8, x9)));
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>) Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var gate = new AsyncLock();

            var values1 = new Queue<T1>();
            var values2 = new Queue<T2>();
            var values3 = new Queue<T3>();
            var values4 = new Queue<T4>();
            var values5 = new Queue<T5>();
            var values6 = new Queue<T6>();
            var values7 = new Queue<T7>();
            var values8 = new Queue<T8>();
            var values9 = new Queue<T9>();
            var isDone = new bool[9];

            IAsyncObserver<T> CreateObserver<T>(int index, Queue<T> queue) =>
                Create<T>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            queue.Enqueue(x);

                            if (values1.Count > 0 && values2.Count > 0 && values3.Count > 0 && values4.Count > 0 && values5.Count > 0 && values6.Count > 0 && values7.Count > 0 && values8.Count > 0 && values9.Count > 0)
                            {
                                TResult res;

                                try
                                {
                                    res = await selector(values1.Dequeue(), values2.Dequeue(), values3.Dequeue(), values4.Dequeue(), values5.Dequeue(), values6.Dequeue(), values7.Dequeue(), values8.Dequeue(), values9.Dequeue()).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else
                            {
                                var allDone = true;

                                for (var i = 0; i < 9; i++)
                                {
                                    if (i != index && !isDone[i])
                                    {
                                        allDone = false;
                                        break;
                                    }
                                }

                                if (allDone)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
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
                            isDone[index] = true;

                            var allDone = true;

                            for (var i = 0; i < 9; i++)
                            {
                                if (!isDone[i])
                                {
                                    allDone = false;
                                    break;
                                }
                            }

                            if (allDone)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            return
            (
                CreateObserver<T1>(1, values1),
                CreateObserver<T2>(2, values2),
                CreateObserver<T3>(3, values3),
                CreateObserver<T4>(4, values4),
                CreateObserver<T5>(5, values5),
                CreateObserver<T6>(6, values6),
                CreateObserver<T7>(7, values7),
                CreateObserver<T8>(8, values8),
                CreateObserver<T9>(9, values9)
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>) Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(IAsyncObserver<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var gate = new AsyncLock();

            var values1 = new Queue<T1>();
            var values2 = new Queue<T2>();
            var values3 = new Queue<T3>();
            var values4 = new Queue<T4>();
            var values5 = new Queue<T5>();
            var values6 = new Queue<T6>();
            var values7 = new Queue<T7>();
            var values8 = new Queue<T8>();
            var values9 = new Queue<T9>();
            var values10 = new Queue<T10>();
            var isDone = new bool[10];

            IAsyncObserver<T> CreateObserver<T>(int index, Queue<T> queue) =>
                Create<T>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            queue.Enqueue(x);

                            if (values1.Count > 0 && values2.Count > 0 && values3.Count > 0 && values4.Count > 0 && values5.Count > 0 && values6.Count > 0 && values7.Count > 0 && values8.Count > 0 && values9.Count > 0 && values10.Count > 0)
                            {
                                await observer.OnNextAsync((values1.Dequeue(), values2.Dequeue(), values3.Dequeue(), values4.Dequeue(), values5.Dequeue(), values6.Dequeue(), values7.Dequeue(), values8.Dequeue(), values9.Dequeue(), values10.Dequeue())).ConfigureAwait(false);
                            }
                            else
                            {
                                var allDone = true;

                                for (var i = 0; i < 10; i++)
                                {
                                    if (i != index && !isDone[i])
                                    {
                                        allDone = false;
                                        break;
                                    }
                                }

                                if (allDone)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
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
                            isDone[index] = true;

                            var allDone = true;

                            for (var i = 0; i < 10; i++)
                            {
                                if (!isDone[i])
                                {
                                    allDone = false;
                                    break;
                                }
                            }

                            if (allDone)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            return
            (
                CreateObserver<T1>(1, values1),
                CreateObserver<T2>(2, values2),
                CreateObserver<T3>(3, values3),
                CreateObserver<T4>(4, values4),
                CreateObserver<T5>(5, values5),
                CreateObserver<T6>(6, values6),
                CreateObserver<T7>(7, values7),
                CreateObserver<T8>(8, values8),
                CreateObserver<T9>(9, values9),
                CreateObserver<T10>(10, values10)
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>) Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(observer, (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10) => Task.FromResult(selector(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10)));
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>) Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var gate = new AsyncLock();

            var values1 = new Queue<T1>();
            var values2 = new Queue<T2>();
            var values3 = new Queue<T3>();
            var values4 = new Queue<T4>();
            var values5 = new Queue<T5>();
            var values6 = new Queue<T6>();
            var values7 = new Queue<T7>();
            var values8 = new Queue<T8>();
            var values9 = new Queue<T9>();
            var values10 = new Queue<T10>();
            var isDone = new bool[10];

            IAsyncObserver<T> CreateObserver<T>(int index, Queue<T> queue) =>
                Create<T>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            queue.Enqueue(x);

                            if (values1.Count > 0 && values2.Count > 0 && values3.Count > 0 && values4.Count > 0 && values5.Count > 0 && values6.Count > 0 && values7.Count > 0 && values8.Count > 0 && values9.Count > 0 && values10.Count > 0)
                            {
                                TResult res;

                                try
                                {
                                    res = await selector(values1.Dequeue(), values2.Dequeue(), values3.Dequeue(), values4.Dequeue(), values5.Dequeue(), values6.Dequeue(), values7.Dequeue(), values8.Dequeue(), values9.Dequeue(), values10.Dequeue()).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else
                            {
                                var allDone = true;

                                for (var i = 0; i < 10; i++)
                                {
                                    if (i != index && !isDone[i])
                                    {
                                        allDone = false;
                                        break;
                                    }
                                }

                                if (allDone)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
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
                            isDone[index] = true;

                            var allDone = true;

                            for (var i = 0; i < 10; i++)
                            {
                                if (!isDone[i])
                                {
                                    allDone = false;
                                    break;
                                }
                            }

                            if (allDone)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            return
            (
                CreateObserver<T1>(1, values1),
                CreateObserver<T2>(2, values2),
                CreateObserver<T3>(3, values3),
                CreateObserver<T4>(4, values4),
                CreateObserver<T5>(5, values5),
                CreateObserver<T6>(6, values6),
                CreateObserver<T7>(7, values7),
                CreateObserver<T8>(8, values8),
                CreateObserver<T9>(9, values9),
                CreateObserver<T10>(10, values10)
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>, IAsyncObserver<T11>) Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(IAsyncObserver<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var gate = new AsyncLock();

            var values1 = new Queue<T1>();
            var values2 = new Queue<T2>();
            var values3 = new Queue<T3>();
            var values4 = new Queue<T4>();
            var values5 = new Queue<T5>();
            var values6 = new Queue<T6>();
            var values7 = new Queue<T7>();
            var values8 = new Queue<T8>();
            var values9 = new Queue<T9>();
            var values10 = new Queue<T10>();
            var values11 = new Queue<T11>();
            var isDone = new bool[11];

            IAsyncObserver<T> CreateObserver<T>(int index, Queue<T> queue) =>
                Create<T>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            queue.Enqueue(x);

                            if (values1.Count > 0 && values2.Count > 0 && values3.Count > 0 && values4.Count > 0 && values5.Count > 0 && values6.Count > 0 && values7.Count > 0 && values8.Count > 0 && values9.Count > 0 && values10.Count > 0 && values11.Count > 0)
                            {
                                await observer.OnNextAsync((values1.Dequeue(), values2.Dequeue(), values3.Dequeue(), values4.Dequeue(), values5.Dequeue(), values6.Dequeue(), values7.Dequeue(), values8.Dequeue(), values9.Dequeue(), values10.Dequeue(), values11.Dequeue())).ConfigureAwait(false);
                            }
                            else
                            {
                                var allDone = true;

                                for (var i = 0; i < 11; i++)
                                {
                                    if (i != index && !isDone[i])
                                    {
                                        allDone = false;
                                        break;
                                    }
                                }

                                if (allDone)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
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
                            isDone[index] = true;

                            var allDone = true;

                            for (var i = 0; i < 11; i++)
                            {
                                if (!isDone[i])
                                {
                                    allDone = false;
                                    break;
                                }
                            }

                            if (allDone)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            return
            (
                CreateObserver<T1>(1, values1),
                CreateObserver<T2>(2, values2),
                CreateObserver<T3>(3, values3),
                CreateObserver<T4>(4, values4),
                CreateObserver<T5>(5, values5),
                CreateObserver<T6>(6, values6),
                CreateObserver<T7>(7, values7),
                CreateObserver<T8>(8, values8),
                CreateObserver<T9>(9, values9),
                CreateObserver<T10>(10, values10),
                CreateObserver<T11>(11, values11)
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>, IAsyncObserver<T11>) Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(observer, (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11) => Task.FromResult(selector(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11)));
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>, IAsyncObserver<T11>) Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var gate = new AsyncLock();

            var values1 = new Queue<T1>();
            var values2 = new Queue<T2>();
            var values3 = new Queue<T3>();
            var values4 = new Queue<T4>();
            var values5 = new Queue<T5>();
            var values6 = new Queue<T6>();
            var values7 = new Queue<T7>();
            var values8 = new Queue<T8>();
            var values9 = new Queue<T9>();
            var values10 = new Queue<T10>();
            var values11 = new Queue<T11>();
            var isDone = new bool[11];

            IAsyncObserver<T> CreateObserver<T>(int index, Queue<T> queue) =>
                Create<T>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            queue.Enqueue(x);

                            if (values1.Count > 0 && values2.Count > 0 && values3.Count > 0 && values4.Count > 0 && values5.Count > 0 && values6.Count > 0 && values7.Count > 0 && values8.Count > 0 && values9.Count > 0 && values10.Count > 0 && values11.Count > 0)
                            {
                                TResult res;

                                try
                                {
                                    res = await selector(values1.Dequeue(), values2.Dequeue(), values3.Dequeue(), values4.Dequeue(), values5.Dequeue(), values6.Dequeue(), values7.Dequeue(), values8.Dequeue(), values9.Dequeue(), values10.Dequeue(), values11.Dequeue()).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else
                            {
                                var allDone = true;

                                for (var i = 0; i < 11; i++)
                                {
                                    if (i != index && !isDone[i])
                                    {
                                        allDone = false;
                                        break;
                                    }
                                }

                                if (allDone)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
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
                            isDone[index] = true;

                            var allDone = true;

                            for (var i = 0; i < 11; i++)
                            {
                                if (!isDone[i])
                                {
                                    allDone = false;
                                    break;
                                }
                            }

                            if (allDone)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            return
            (
                CreateObserver<T1>(1, values1),
                CreateObserver<T2>(2, values2),
                CreateObserver<T3>(3, values3),
                CreateObserver<T4>(4, values4),
                CreateObserver<T5>(5, values5),
                CreateObserver<T6>(6, values6),
                CreateObserver<T7>(7, values7),
                CreateObserver<T8>(8, values8),
                CreateObserver<T9>(9, values9),
                CreateObserver<T10>(10, values10),
                CreateObserver<T11>(11, values11)
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>, IAsyncObserver<T11>, IAsyncObserver<T12>) Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(IAsyncObserver<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var gate = new AsyncLock();

            var values1 = new Queue<T1>();
            var values2 = new Queue<T2>();
            var values3 = new Queue<T3>();
            var values4 = new Queue<T4>();
            var values5 = new Queue<T5>();
            var values6 = new Queue<T6>();
            var values7 = new Queue<T7>();
            var values8 = new Queue<T8>();
            var values9 = new Queue<T9>();
            var values10 = new Queue<T10>();
            var values11 = new Queue<T11>();
            var values12 = new Queue<T12>();
            var isDone = new bool[12];

            IAsyncObserver<T> CreateObserver<T>(int index, Queue<T> queue) =>
                Create<T>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            queue.Enqueue(x);

                            if (values1.Count > 0 && values2.Count > 0 && values3.Count > 0 && values4.Count > 0 && values5.Count > 0 && values6.Count > 0 && values7.Count > 0 && values8.Count > 0 && values9.Count > 0 && values10.Count > 0 && values11.Count > 0 && values12.Count > 0)
                            {
                                await observer.OnNextAsync((values1.Dequeue(), values2.Dequeue(), values3.Dequeue(), values4.Dequeue(), values5.Dequeue(), values6.Dequeue(), values7.Dequeue(), values8.Dequeue(), values9.Dequeue(), values10.Dequeue(), values11.Dequeue(), values12.Dequeue())).ConfigureAwait(false);
                            }
                            else
                            {
                                var allDone = true;

                                for (var i = 0; i < 12; i++)
                                {
                                    if (i != index && !isDone[i])
                                    {
                                        allDone = false;
                                        break;
                                    }
                                }

                                if (allDone)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
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
                            isDone[index] = true;

                            var allDone = true;

                            for (var i = 0; i < 12; i++)
                            {
                                if (!isDone[i])
                                {
                                    allDone = false;
                                    break;
                                }
                            }

                            if (allDone)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            return
            (
                CreateObserver<T1>(1, values1),
                CreateObserver<T2>(2, values2),
                CreateObserver<T3>(3, values3),
                CreateObserver<T4>(4, values4),
                CreateObserver<T5>(5, values5),
                CreateObserver<T6>(6, values6),
                CreateObserver<T7>(7, values7),
                CreateObserver<T8>(8, values8),
                CreateObserver<T9>(9, values9),
                CreateObserver<T10>(10, values10),
                CreateObserver<T11>(11, values11),
                CreateObserver<T12>(12, values12)
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>, IAsyncObserver<T11>, IAsyncObserver<T12>) Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(observer, (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12) => Task.FromResult(selector(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12)));
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>, IAsyncObserver<T11>, IAsyncObserver<T12>) Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var gate = new AsyncLock();

            var values1 = new Queue<T1>();
            var values2 = new Queue<T2>();
            var values3 = new Queue<T3>();
            var values4 = new Queue<T4>();
            var values5 = new Queue<T5>();
            var values6 = new Queue<T6>();
            var values7 = new Queue<T7>();
            var values8 = new Queue<T8>();
            var values9 = new Queue<T9>();
            var values10 = new Queue<T10>();
            var values11 = new Queue<T11>();
            var values12 = new Queue<T12>();
            var isDone = new bool[12];

            IAsyncObserver<T> CreateObserver<T>(int index, Queue<T> queue) =>
                Create<T>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            queue.Enqueue(x);

                            if (values1.Count > 0 && values2.Count > 0 && values3.Count > 0 && values4.Count > 0 && values5.Count > 0 && values6.Count > 0 && values7.Count > 0 && values8.Count > 0 && values9.Count > 0 && values10.Count > 0 && values11.Count > 0 && values12.Count > 0)
                            {
                                TResult res;

                                try
                                {
                                    res = await selector(values1.Dequeue(), values2.Dequeue(), values3.Dequeue(), values4.Dequeue(), values5.Dequeue(), values6.Dequeue(), values7.Dequeue(), values8.Dequeue(), values9.Dequeue(), values10.Dequeue(), values11.Dequeue(), values12.Dequeue()).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else
                            {
                                var allDone = true;

                                for (var i = 0; i < 12; i++)
                                {
                                    if (i != index && !isDone[i])
                                    {
                                        allDone = false;
                                        break;
                                    }
                                }

                                if (allDone)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
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
                            isDone[index] = true;

                            var allDone = true;

                            for (var i = 0; i < 12; i++)
                            {
                                if (!isDone[i])
                                {
                                    allDone = false;
                                    break;
                                }
                            }

                            if (allDone)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            return
            (
                CreateObserver<T1>(1, values1),
                CreateObserver<T2>(2, values2),
                CreateObserver<T3>(3, values3),
                CreateObserver<T4>(4, values4),
                CreateObserver<T5>(5, values5),
                CreateObserver<T6>(6, values6),
                CreateObserver<T7>(7, values7),
                CreateObserver<T8>(8, values8),
                CreateObserver<T9>(9, values9),
                CreateObserver<T10>(10, values10),
                CreateObserver<T11>(11, values11),
                CreateObserver<T12>(12, values12)
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>, IAsyncObserver<T11>, IAsyncObserver<T12>, IAsyncObserver<T13>) Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(IAsyncObserver<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var gate = new AsyncLock();

            var values1 = new Queue<T1>();
            var values2 = new Queue<T2>();
            var values3 = new Queue<T3>();
            var values4 = new Queue<T4>();
            var values5 = new Queue<T5>();
            var values6 = new Queue<T6>();
            var values7 = new Queue<T7>();
            var values8 = new Queue<T8>();
            var values9 = new Queue<T9>();
            var values10 = new Queue<T10>();
            var values11 = new Queue<T11>();
            var values12 = new Queue<T12>();
            var values13 = new Queue<T13>();
            var isDone = new bool[13];

            IAsyncObserver<T> CreateObserver<T>(int index, Queue<T> queue) =>
                Create<T>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            queue.Enqueue(x);

                            if (values1.Count > 0 && values2.Count > 0 && values3.Count > 0 && values4.Count > 0 && values5.Count > 0 && values6.Count > 0 && values7.Count > 0 && values8.Count > 0 && values9.Count > 0 && values10.Count > 0 && values11.Count > 0 && values12.Count > 0 && values13.Count > 0)
                            {
                                await observer.OnNextAsync((values1.Dequeue(), values2.Dequeue(), values3.Dequeue(), values4.Dequeue(), values5.Dequeue(), values6.Dequeue(), values7.Dequeue(), values8.Dequeue(), values9.Dequeue(), values10.Dequeue(), values11.Dequeue(), values12.Dequeue(), values13.Dequeue())).ConfigureAwait(false);
                            }
                            else
                            {
                                var allDone = true;

                                for (var i = 0; i < 13; i++)
                                {
                                    if (i != index && !isDone[i])
                                    {
                                        allDone = false;
                                        break;
                                    }
                                }

                                if (allDone)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
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
                            isDone[index] = true;

                            var allDone = true;

                            for (var i = 0; i < 13; i++)
                            {
                                if (!isDone[i])
                                {
                                    allDone = false;
                                    break;
                                }
                            }

                            if (allDone)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            return
            (
                CreateObserver<T1>(1, values1),
                CreateObserver<T2>(2, values2),
                CreateObserver<T3>(3, values3),
                CreateObserver<T4>(4, values4),
                CreateObserver<T5>(5, values5),
                CreateObserver<T6>(6, values6),
                CreateObserver<T7>(7, values7),
                CreateObserver<T8>(8, values8),
                CreateObserver<T9>(9, values9),
                CreateObserver<T10>(10, values10),
                CreateObserver<T11>(11, values11),
                CreateObserver<T12>(12, values12),
                CreateObserver<T13>(13, values13)
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>, IAsyncObserver<T11>, IAsyncObserver<T12>, IAsyncObserver<T13>) Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(observer, (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13) => Task.FromResult(selector(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13)));
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>, IAsyncObserver<T11>, IAsyncObserver<T12>, IAsyncObserver<T13>) Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var gate = new AsyncLock();

            var values1 = new Queue<T1>();
            var values2 = new Queue<T2>();
            var values3 = new Queue<T3>();
            var values4 = new Queue<T4>();
            var values5 = new Queue<T5>();
            var values6 = new Queue<T6>();
            var values7 = new Queue<T7>();
            var values8 = new Queue<T8>();
            var values9 = new Queue<T9>();
            var values10 = new Queue<T10>();
            var values11 = new Queue<T11>();
            var values12 = new Queue<T12>();
            var values13 = new Queue<T13>();
            var isDone = new bool[13];

            IAsyncObserver<T> CreateObserver<T>(int index, Queue<T> queue) =>
                Create<T>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            queue.Enqueue(x);

                            if (values1.Count > 0 && values2.Count > 0 && values3.Count > 0 && values4.Count > 0 && values5.Count > 0 && values6.Count > 0 && values7.Count > 0 && values8.Count > 0 && values9.Count > 0 && values10.Count > 0 && values11.Count > 0 && values12.Count > 0 && values13.Count > 0)
                            {
                                TResult res;

                                try
                                {
                                    res = await selector(values1.Dequeue(), values2.Dequeue(), values3.Dequeue(), values4.Dequeue(), values5.Dequeue(), values6.Dequeue(), values7.Dequeue(), values8.Dequeue(), values9.Dequeue(), values10.Dequeue(), values11.Dequeue(), values12.Dequeue(), values13.Dequeue()).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else
                            {
                                var allDone = true;

                                for (var i = 0; i < 13; i++)
                                {
                                    if (i != index && !isDone[i])
                                    {
                                        allDone = false;
                                        break;
                                    }
                                }

                                if (allDone)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
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
                            isDone[index] = true;

                            var allDone = true;

                            for (var i = 0; i < 13; i++)
                            {
                                if (!isDone[i])
                                {
                                    allDone = false;
                                    break;
                                }
                            }

                            if (allDone)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            return
            (
                CreateObserver<T1>(1, values1),
                CreateObserver<T2>(2, values2),
                CreateObserver<T3>(3, values3),
                CreateObserver<T4>(4, values4),
                CreateObserver<T5>(5, values5),
                CreateObserver<T6>(6, values6),
                CreateObserver<T7>(7, values7),
                CreateObserver<T8>(8, values8),
                CreateObserver<T9>(9, values9),
                CreateObserver<T10>(10, values10),
                CreateObserver<T11>(11, values11),
                CreateObserver<T12>(12, values12),
                CreateObserver<T13>(13, values13)
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>, IAsyncObserver<T11>, IAsyncObserver<T12>, IAsyncObserver<T13>, IAsyncObserver<T14>) Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(IAsyncObserver<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var gate = new AsyncLock();

            var values1 = new Queue<T1>();
            var values2 = new Queue<T2>();
            var values3 = new Queue<T3>();
            var values4 = new Queue<T4>();
            var values5 = new Queue<T5>();
            var values6 = new Queue<T6>();
            var values7 = new Queue<T7>();
            var values8 = new Queue<T8>();
            var values9 = new Queue<T9>();
            var values10 = new Queue<T10>();
            var values11 = new Queue<T11>();
            var values12 = new Queue<T12>();
            var values13 = new Queue<T13>();
            var values14 = new Queue<T14>();
            var isDone = new bool[14];

            IAsyncObserver<T> CreateObserver<T>(int index, Queue<T> queue) =>
                Create<T>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            queue.Enqueue(x);

                            if (values1.Count > 0 && values2.Count > 0 && values3.Count > 0 && values4.Count > 0 && values5.Count > 0 && values6.Count > 0 && values7.Count > 0 && values8.Count > 0 && values9.Count > 0 && values10.Count > 0 && values11.Count > 0 && values12.Count > 0 && values13.Count > 0 && values14.Count > 0)
                            {
                                await observer.OnNextAsync((values1.Dequeue(), values2.Dequeue(), values3.Dequeue(), values4.Dequeue(), values5.Dequeue(), values6.Dequeue(), values7.Dequeue(), values8.Dequeue(), values9.Dequeue(), values10.Dequeue(), values11.Dequeue(), values12.Dequeue(), values13.Dequeue(), values14.Dequeue())).ConfigureAwait(false);
                            }
                            else
                            {
                                var allDone = true;

                                for (var i = 0; i < 14; i++)
                                {
                                    if (i != index && !isDone[i])
                                    {
                                        allDone = false;
                                        break;
                                    }
                                }

                                if (allDone)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
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
                            isDone[index] = true;

                            var allDone = true;

                            for (var i = 0; i < 14; i++)
                            {
                                if (!isDone[i])
                                {
                                    allDone = false;
                                    break;
                                }
                            }

                            if (allDone)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            return
            (
                CreateObserver<T1>(1, values1),
                CreateObserver<T2>(2, values2),
                CreateObserver<T3>(3, values3),
                CreateObserver<T4>(4, values4),
                CreateObserver<T5>(5, values5),
                CreateObserver<T6>(6, values6),
                CreateObserver<T7>(7, values7),
                CreateObserver<T8>(8, values8),
                CreateObserver<T9>(9, values9),
                CreateObserver<T10>(10, values10),
                CreateObserver<T11>(11, values11),
                CreateObserver<T12>(12, values12),
                CreateObserver<T13>(13, values13),
                CreateObserver<T14>(14, values14)
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>, IAsyncObserver<T11>, IAsyncObserver<T12>, IAsyncObserver<T13>, IAsyncObserver<T14>) Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(observer, (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14) => Task.FromResult(selector(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14)));
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>, IAsyncObserver<T11>, IAsyncObserver<T12>, IAsyncObserver<T13>, IAsyncObserver<T14>) Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var gate = new AsyncLock();

            var values1 = new Queue<T1>();
            var values2 = new Queue<T2>();
            var values3 = new Queue<T3>();
            var values4 = new Queue<T4>();
            var values5 = new Queue<T5>();
            var values6 = new Queue<T6>();
            var values7 = new Queue<T7>();
            var values8 = new Queue<T8>();
            var values9 = new Queue<T9>();
            var values10 = new Queue<T10>();
            var values11 = new Queue<T11>();
            var values12 = new Queue<T12>();
            var values13 = new Queue<T13>();
            var values14 = new Queue<T14>();
            var isDone = new bool[14];

            IAsyncObserver<T> CreateObserver<T>(int index, Queue<T> queue) =>
                Create<T>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            queue.Enqueue(x);

                            if (values1.Count > 0 && values2.Count > 0 && values3.Count > 0 && values4.Count > 0 && values5.Count > 0 && values6.Count > 0 && values7.Count > 0 && values8.Count > 0 && values9.Count > 0 && values10.Count > 0 && values11.Count > 0 && values12.Count > 0 && values13.Count > 0 && values14.Count > 0)
                            {
                                TResult res;

                                try
                                {
                                    res = await selector(values1.Dequeue(), values2.Dequeue(), values3.Dequeue(), values4.Dequeue(), values5.Dequeue(), values6.Dequeue(), values7.Dequeue(), values8.Dequeue(), values9.Dequeue(), values10.Dequeue(), values11.Dequeue(), values12.Dequeue(), values13.Dequeue(), values14.Dequeue()).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else
                            {
                                var allDone = true;

                                for (var i = 0; i < 14; i++)
                                {
                                    if (i != index && !isDone[i])
                                    {
                                        allDone = false;
                                        break;
                                    }
                                }

                                if (allDone)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
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
                            isDone[index] = true;

                            var allDone = true;

                            for (var i = 0; i < 14; i++)
                            {
                                if (!isDone[i])
                                {
                                    allDone = false;
                                    break;
                                }
                            }

                            if (allDone)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            return
            (
                CreateObserver<T1>(1, values1),
                CreateObserver<T2>(2, values2),
                CreateObserver<T3>(3, values3),
                CreateObserver<T4>(4, values4),
                CreateObserver<T5>(5, values5),
                CreateObserver<T6>(6, values6),
                CreateObserver<T7>(7, values7),
                CreateObserver<T8>(8, values8),
                CreateObserver<T9>(9, values9),
                CreateObserver<T10>(10, values10),
                CreateObserver<T11>(11, values11),
                CreateObserver<T12>(12, values12),
                CreateObserver<T13>(13, values13),
                CreateObserver<T14>(14, values14)
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>, IAsyncObserver<T11>, IAsyncObserver<T12>, IAsyncObserver<T13>, IAsyncObserver<T14>, IAsyncObserver<T15>) Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(IAsyncObserver<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var gate = new AsyncLock();

            var values1 = new Queue<T1>();
            var values2 = new Queue<T2>();
            var values3 = new Queue<T3>();
            var values4 = new Queue<T4>();
            var values5 = new Queue<T5>();
            var values6 = new Queue<T6>();
            var values7 = new Queue<T7>();
            var values8 = new Queue<T8>();
            var values9 = new Queue<T9>();
            var values10 = new Queue<T10>();
            var values11 = new Queue<T11>();
            var values12 = new Queue<T12>();
            var values13 = new Queue<T13>();
            var values14 = new Queue<T14>();
            var values15 = new Queue<T15>();
            var isDone = new bool[15];

            IAsyncObserver<T> CreateObserver<T>(int index, Queue<T> queue) =>
                Create<T>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            queue.Enqueue(x);

                            if (values1.Count > 0 && values2.Count > 0 && values3.Count > 0 && values4.Count > 0 && values5.Count > 0 && values6.Count > 0 && values7.Count > 0 && values8.Count > 0 && values9.Count > 0 && values10.Count > 0 && values11.Count > 0 && values12.Count > 0 && values13.Count > 0 && values14.Count > 0 && values15.Count > 0)
                            {
                                await observer.OnNextAsync((values1.Dequeue(), values2.Dequeue(), values3.Dequeue(), values4.Dequeue(), values5.Dequeue(), values6.Dequeue(), values7.Dequeue(), values8.Dequeue(), values9.Dequeue(), values10.Dequeue(), values11.Dequeue(), values12.Dequeue(), values13.Dequeue(), values14.Dequeue(), values15.Dequeue())).ConfigureAwait(false);
                            }
                            else
                            {
                                var allDone = true;

                                for (var i = 0; i < 15; i++)
                                {
                                    if (i != index && !isDone[i])
                                    {
                                        allDone = false;
                                        break;
                                    }
                                }

                                if (allDone)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
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
                            isDone[index] = true;

                            var allDone = true;

                            for (var i = 0; i < 15; i++)
                            {
                                if (!isDone[i])
                                {
                                    allDone = false;
                                    break;
                                }
                            }

                            if (allDone)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            return
            (
                CreateObserver<T1>(1, values1),
                CreateObserver<T2>(2, values2),
                CreateObserver<T3>(3, values3),
                CreateObserver<T4>(4, values4),
                CreateObserver<T5>(5, values5),
                CreateObserver<T6>(6, values6),
                CreateObserver<T7>(7, values7),
                CreateObserver<T8>(8, values8),
                CreateObserver<T9>(9, values9),
                CreateObserver<T10>(10, values10),
                CreateObserver<T11>(11, values11),
                CreateObserver<T12>(12, values12),
                CreateObserver<T13>(13, values13),
                CreateObserver<T14>(14, values14),
                CreateObserver<T15>(15, values15)
            );
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>, IAsyncObserver<T11>, IAsyncObserver<T12>, IAsyncObserver<T13>, IAsyncObserver<T14>, IAsyncObserver<T15>) Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(observer, (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14, x15) => Task.FromResult(selector(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14, x15)));
        }

        public static (IAsyncObserver<T1>, IAsyncObserver<T2>, IAsyncObserver<T3>, IAsyncObserver<T4>, IAsyncObserver<T5>, IAsyncObserver<T6>, IAsyncObserver<T7>, IAsyncObserver<T8>, IAsyncObserver<T9>, IAsyncObserver<T10>, IAsyncObserver<T11>, IAsyncObserver<T12>, IAsyncObserver<T13>, IAsyncObserver<T14>, IAsyncObserver<T15>) Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var gate = new AsyncLock();

            var values1 = new Queue<T1>();
            var values2 = new Queue<T2>();
            var values3 = new Queue<T3>();
            var values4 = new Queue<T4>();
            var values5 = new Queue<T5>();
            var values6 = new Queue<T6>();
            var values7 = new Queue<T7>();
            var values8 = new Queue<T8>();
            var values9 = new Queue<T9>();
            var values10 = new Queue<T10>();
            var values11 = new Queue<T11>();
            var values12 = new Queue<T12>();
            var values13 = new Queue<T13>();
            var values14 = new Queue<T14>();
            var values15 = new Queue<T15>();
            var isDone = new bool[15];

            IAsyncObserver<T> CreateObserver<T>(int index, Queue<T> queue) =>
                Create<T>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            queue.Enqueue(x);

                            if (values1.Count > 0 && values2.Count > 0 && values3.Count > 0 && values4.Count > 0 && values5.Count > 0 && values6.Count > 0 && values7.Count > 0 && values8.Count > 0 && values9.Count > 0 && values10.Count > 0 && values11.Count > 0 && values12.Count > 0 && values13.Count > 0 && values14.Count > 0 && values15.Count > 0)
                            {
                                TResult res;

                                try
                                {
                                    res = await selector(values1.Dequeue(), values2.Dequeue(), values3.Dequeue(), values4.Dequeue(), values5.Dequeue(), values6.Dequeue(), values7.Dequeue(), values8.Dequeue(), values9.Dequeue(), values10.Dequeue(), values11.Dequeue(), values12.Dequeue(), values13.Dequeue(), values14.Dequeue(), values15.Dequeue()).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(res).ConfigureAwait(false);
                            }
                            else
                            {
                                var allDone = true;

                                for (var i = 0; i < 15; i++)
                                {
                                    if (i != index && !isDone[i])
                                    {
                                        allDone = false;
                                        break;
                                    }
                                }

                                if (allDone)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
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
                            isDone[index] = true;

                            var allDone = true;

                            for (var i = 0; i < 15; i++)
                            {
                                if (!isDone[i])
                                {
                                    allDone = false;
                                    break;
                                }
                            }

                            if (allDone)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            return
            (
                CreateObserver<T1>(1, values1),
                CreateObserver<T2>(2, values2),
                CreateObserver<T3>(3, values3),
                CreateObserver<T4>(4, values4),
                CreateObserver<T5>(5, values5),
                CreateObserver<T6>(6, values6),
                CreateObserver<T7>(7, values7),
                CreateObserver<T8>(8, values8),
                CreateObserver<T9>(9, values9),
                CreateObserver<T10>(10, values10),
                CreateObserver<T11>(11, values11),
                CreateObserver<T12>(12, values12),
                CreateObserver<T13>(13, values13),
                CreateObserver<T14>(14, values14),
                CreateObserver<T15>(15, values15)
            );
        }

    }
}
