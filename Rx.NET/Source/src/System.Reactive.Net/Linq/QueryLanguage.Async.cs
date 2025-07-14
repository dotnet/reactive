﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    internal partial class QueryLanguage
    {
        #region FromAsyncPattern

        #region Func

        public virtual Func<IObservable<TResult>> FromAsyncPattern<TResult>(Func<AsyncCallback, object?, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            return () =>
            {
                var subject = new AsyncSubject<TResult>();
                try
                {
                    begin(iar =>
                    {
                        // Note: Even if the callback completes synchronously, outgoing On* calls
                        //       cannot throw in user code since there can't be any subscribers
                        //       to the AsyncSubject yet. Therefore, there is no need to protect
                        //       against exceptions that'd be caught below and sent (incorrectly)
                        //       into the Observable.Throw sequence being constructed.
                        TResult result;
                        try
                        {
                            result = end(iar);
                        }
                        catch (Exception exception)
                        {
                            subject.OnError(exception);
                            return;
                        }
                        subject.OnNext(result);
                        subject.OnCompleted();
                    }, null);
                }
                catch (Exception exception)
                {
                    return Observable.Throw<TResult>(exception, SchedulerDefaults.AsyncConversions);
                }
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, IObservable<TResult>> FromAsyncPattern<T1, TResult>(Func<T1, AsyncCallback, object?, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            return x =>
            {
                var subject = new AsyncSubject<TResult>();
                try
                {
                    begin(x, iar =>
                    {
                        // See remark on FromAsyncPattern<TResult>.
                        TResult result;
                        try
                        {
                            result = end(iar);
                        }
                        catch (Exception exception)
                        {
                            subject.OnError(exception);
                            return;
                        }
                        subject.OnNext(result);
                        subject.OnCompleted();
                    }, null);
                }
                catch (Exception exception)
                {
                    return Observable.Throw<TResult>(exception, SchedulerDefaults.AsyncConversions);
                }
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, IObservable<TResult>> FromAsyncPattern<T1, T2, TResult>(Func<T1, T2, AsyncCallback, object?, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            return (x, y) =>
            {
                var subject = new AsyncSubject<TResult>();
                try
                {
                    begin(x, y, iar =>
                    {
                        // See remark on FromAsyncPattern<TResult>.
                        TResult result;
                        try
                        {
                            result = end(iar);
                        }
                        catch (Exception exception)
                        {
                            subject.OnError(exception);
                            return;
                        }
                        subject.OnNext(result);
                        subject.OnCompleted();
                    }, null);
                }
                catch (Exception exception)
                {
                    return Observable.Throw<TResult>(exception, SchedulerDefaults.AsyncConversions);
                }
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, TResult>(Func<T1, T2, T3, AsyncCallback, object?, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            return (x, y, z) =>
            {
                var subject = new AsyncSubject<TResult>();
                try
                {
                    begin(x, y, z, iar =>
                    {
                        // See remark on FromAsyncPattern<TResult>.
                        TResult result;
                        try
                        {
                            result = end(iar);
                        }
                        catch (Exception exception)
                        {
                            subject.OnError(exception);
                            return;
                        }
                        subject.OnNext(result);
                        subject.OnCompleted();
                    }, null);
                }
                catch (Exception exception)
                {
                    return Observable.Throw<TResult>(exception, SchedulerDefaults.AsyncConversions);
                }
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, AsyncCallback, object?, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            return (x, y, z, a) =>
            {
                var subject = new AsyncSubject<TResult>();
                try
                {
                    begin(x, y, z, a, iar =>
                    {
                        // See remark on FromAsyncPattern<TResult>.
                        TResult result;
                        try
                        {
                            result = end(iar);
                        }
                        catch (Exception exception)
                        {
                            subject.OnError(exception);
                            return;
                        }
                        subject.OnNext(result);
                        subject.OnCompleted();
                    }, null);
                }
                catch (Exception exception)
                {
                    return Observable.Throw<TResult>(exception, SchedulerDefaults.AsyncConversions);
                }
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, AsyncCallback, object?, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            return (x, y, z, a, b) =>
            {
                var subject = new AsyncSubject<TResult>();
                try
                {
                    begin(x, y, z, a, b, iar =>
                    {
                        // See remark on FromAsyncPattern<TResult>.
                        TResult result;
                        try
                        {
                            result = end(iar);
                        }
                        catch (Exception exception)
                        {
                            subject.OnError(exception);
                            return;
                        }
                        subject.OnNext(result);
                        subject.OnCompleted();
                    }, null);
                }
                catch (Exception exception)
                {
                    return Observable.Throw<TResult>(exception, SchedulerDefaults.AsyncConversions);
                }
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, AsyncCallback, object?, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            return (x, y, z, a, b, c) =>
            {
                var subject = new AsyncSubject<TResult>();
                try
                {
                    begin(x, y, z, a, b, c, iar =>
                    {
                        // See remark on FromAsyncPattern<TResult>.
                        TResult result;
                        try
                        {
                            result = end(iar);
                        }
                        catch (Exception exception)
                        {
                            subject.OnError(exception);
                            return;
                        }
                        subject.OnNext(result);
                        subject.OnCompleted();
                    }, null);
                }
                catch (Exception exception)
                {
                    return Observable.Throw<TResult>(exception, SchedulerDefaults.AsyncConversions);
                }
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, AsyncCallback, object?, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            return (x, y, z, a, b, c, d) =>
            {
                var subject = new AsyncSubject<TResult>();
                try
                {
                    begin(x, y, z, a, b, c, d, iar =>
                    {
                        // See remark on FromAsyncPattern<TResult>.
                        TResult result;
                        try
                        {
                            result = end(iar);
                        }
                        catch (Exception exception)
                        {
                            subject.OnError(exception);
                            return;
                        }
                        subject.OnNext(result);
                        subject.OnCompleted();
                    }, null);
                }
                catch (Exception exception)
                {
                    return Observable.Throw<TResult>(exception, SchedulerDefaults.AsyncConversions);
                }
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, AsyncCallback, object?, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            return (x, y, z, a, b, c, d, e) =>
            {
                var subject = new AsyncSubject<TResult>();
                try
                {
                    begin(x, y, z, a, b, c, d, e, iar =>
                    {
                        // See remark on FromAsyncPattern<TResult>.
                        TResult result;
                        try
                        {
                            result = end(iar);
                        }
                        catch (Exception exception)
                        {
                            subject.OnError(exception);
                            return;
                        }
                        subject.OnNext(result);
                        subject.OnCompleted();
                    }, null);
                }
                catch (Exception exception)
                {
                    return Observable.Throw<TResult>(exception, SchedulerDefaults.AsyncConversions);
                }
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, AsyncCallback, object?, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            return (x, y, z, a, b, c, d, e, f) =>
            {
                var subject = new AsyncSubject<TResult>();
                try
                {
                    begin(x, y, z, a, b, c, d, e, f, iar =>
                    {
                        // See remark on FromAsyncPattern<TResult>.
                        TResult result;
                        try
                        {
                            result = end(iar);
                        }
                        catch (Exception exception)
                        {
                            subject.OnError(exception);
                            return;
                        }
                        subject.OnNext(result);
                        subject.OnCompleted();
                    }, null);
                }
                catch (Exception exception)
                {
                    return Observable.Throw<TResult>(exception, SchedulerDefaults.AsyncConversions);
                }
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, AsyncCallback, object?, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            return (x, y, z, a, b, c, d, e, f, g) =>
            {
                var subject = new AsyncSubject<TResult>();
                try
                {
                    begin(x, y, z, a, b, c, d, e, f, g, iar =>
                    {
                        // See remark on FromAsyncPattern<TResult>.
                        TResult result;
                        try
                        {
                            result = end(iar);
                        }
                        catch (Exception exception)
                        {
                            subject.OnError(exception);
                            return;
                        }
                        subject.OnNext(result);
                        subject.OnCompleted();
                    }, null);
                }
                catch (Exception exception)
                {
                    return Observable.Throw<TResult>(exception, SchedulerDefaults.AsyncConversions);
                }
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, AsyncCallback, object?, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            return (x, y, z, a, b, c, d, e, f, g, h) =>
            {
                var subject = new AsyncSubject<TResult>();
                try
                {
                    begin(x, y, z, a, b, c, d, e, f, g, h, iar =>
                    {
                        // See remark on FromAsyncPattern<TResult>.
                        TResult result;
                        try
                        {
                            result = end(iar);
                        }
                        catch (Exception exception)
                        {
                            subject.OnError(exception);
                            return;
                        }
                        subject.OnNext(result);
                        subject.OnCompleted();
                    }, null);
                }
                catch (Exception exception)
                {
                    return Observable.Throw<TResult>(exception, SchedulerDefaults.AsyncConversions);
                }
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, AsyncCallback, object?, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            return (x, y, z, a, b, c, d, e, f, g, h, i) =>
            {
                var subject = new AsyncSubject<TResult>();
                try
                {
                    begin(x, y, z, a, b, c, d, e, f, g, h, i, iar =>
                    {
                        // See remark on FromAsyncPattern<TResult>.
                        TResult result;
                        try
                        {
                            result = end(iar);
                        }
                        catch (Exception exception)
                        {
                            subject.OnError(exception);
                            return;
                        }
                        subject.OnNext(result);
                        subject.OnCompleted();
                    }, null);
                }
                catch (Exception exception)
                {
                    return Observable.Throw<TResult>(exception, SchedulerDefaults.AsyncConversions);
                }
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, AsyncCallback, object?, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            return (x, y, z, a, b, c, d, e, f, g, h, i, j) =>
            {
                var subject = new AsyncSubject<TResult>();
                try
                {
                    begin(x, y, z, a, b, c, d, e, f, g, h, i, j, iar =>
                    {
                        // See remark on FromAsyncPattern<TResult>.
                        TResult result;
                        try
                        {
                            result = end(iar);
                        }
                        catch (Exception exception)
                        {
                            subject.OnError(exception);
                            return;
                        }
                        subject.OnNext(result);
                        subject.OnCompleted();
                    }, null);
                }
                catch (Exception exception)
                {
                    return Observable.Throw<TResult>(exception, SchedulerDefaults.AsyncConversions);
                }
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, AsyncCallback, object?, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            return (x, y, z, a, b, c, d, e, f, g, h, i, j, k) =>
            {
                var subject = new AsyncSubject<TResult>();
                try
                {
                    begin(x, y, z, a, b, c, d, e, f, g, h, i, j, k, iar =>
                    {
                        // See remark on FromAsyncPattern<TResult>.
                        TResult result;
                        try
                        {
                            result = end(iar);
                        }
                        catch (Exception exception)
                        {
                            subject.OnError(exception);
                            return;
                        }
                        subject.OnNext(result);
                        subject.OnCompleted();
                    }, null);
                }
                catch (Exception exception)
                {
                    return Observable.Throw<TResult>(exception, SchedulerDefaults.AsyncConversions);
                }
                return subject.AsObservable();
            };
        }

        #endregion

        #region Action

        public virtual Func<IObservable<Unit>> FromAsyncPattern(Func<AsyncCallback, object?, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            return FromAsyncPattern(begin, iar =>
            {
                end(iar);
                return Unit.Default;
            });
        }

        public virtual Func<T1, IObservable<Unit>> FromAsyncPattern<T1>(Func<T1, AsyncCallback, object?, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            return FromAsyncPattern(begin, iar =>
            {
                end(iar);
                return Unit.Default;
            });
        }

        public virtual Func<T1, T2, IObservable<Unit>> FromAsyncPattern<T1, T2>(Func<T1, T2, AsyncCallback, object?, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            return FromAsyncPattern(begin, iar =>
            {
                end(iar);
                return Unit.Default;
            });
        }

        public virtual Func<T1, T2, T3, IObservable<Unit>> FromAsyncPattern<T1, T2, T3>(Func<T1, T2, T3, AsyncCallback, object?, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            return FromAsyncPattern(begin, iar =>
            {
                end(iar);
                return Unit.Default;
            });
        }

        public virtual Func<T1, T2, T3, T4, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4>(Func<T1, T2, T3, T4, AsyncCallback, object?, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            return FromAsyncPattern(begin, iar =>
            {
                end(iar);
                return Unit.Default;
            });
        }

        public virtual Func<T1, T2, T3, T4, T5, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, AsyncCallback, object?, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            return FromAsyncPattern(begin, iar =>
            {
                end(iar);
                return Unit.Default;
            });
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6, AsyncCallback, object?, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            return FromAsyncPattern(begin, iar =>
            {
                end(iar);
                return Unit.Default;
            });
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7, AsyncCallback, object?, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            return FromAsyncPattern(begin, iar =>
            {
                end(iar);
                return Unit.Default;
            });
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T1, T2, T3, T4, T5, T6, T7, T8, AsyncCallback, object?, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            return FromAsyncPattern(begin, iar =>
            {
                end(iar);
                return Unit.Default;
            });
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, AsyncCallback, object?, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            return FromAsyncPattern(begin, iar =>
            {
                end(iar);
                return Unit.Default;
            });
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, AsyncCallback, object?, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            return FromAsyncPattern(begin, iar =>
            {
                end(iar);
                return Unit.Default;
            });
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, AsyncCallback, object?, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            return FromAsyncPattern(begin, iar =>
            {
                end(iar);
                return Unit.Default;
            });
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, AsyncCallback, object?, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            return FromAsyncPattern(begin, iar =>
            {
                end(iar);
                return Unit.Default;
            });
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, AsyncCallback, object?, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            return FromAsyncPattern(begin, iar =>
            {
                end(iar);
                return Unit.Default;
            });
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, AsyncCallback, object?, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            return FromAsyncPattern(begin, iar =>
            {
                end(iar);
                return Unit.Default;
            });
        }

        #endregion

        #endregion

        #region Start[Async]

        #region Func

        public virtual IObservable<TSource> Start<TSource>(Func<TSource> function)
        {
            return ToAsync(function)();
        }

        public virtual IObservable<TSource> Start<TSource>(Func<TSource> function, IScheduler scheduler)
        {
            return ToAsync(function, scheduler)();
        }

        public virtual IObservable<TSource> StartAsync<TSource>(Func<Task<TSource>> functionAsync)
        {
            return StartAsyncImpl(functionAsync, new TaskObservationOptions.Value(null, ignoreExceptionsAfterUnsubscribe: false));
        }

        public virtual IObservable<TSource> StartAsync<TSource>(Func<Task<TSource>> functionAsync, in TaskObservationOptions.Value options)
        {
            return StartAsyncImpl(functionAsync, options);
        }

        private IObservable<TSource> StartAsyncImpl<TSource>(Func<Task<TSource>> functionAsync, in TaskObservationOptions.Value options)
        {
            Task<TSource> task;
            try
            {
                task = functionAsync();
            }
            catch (Exception exception)
            {
                return Throw<TSource>(exception);
            }

            return task.ToObservable(options);
        }

        public virtual IObservable<TSource> StartAsync<TSource>(Func<CancellationToken, Task<TSource>> functionAsync)
        {
            return StartAsyncImpl(functionAsync, new TaskObservationOptions.Value(null, false));
        }

        public virtual IObservable<TSource> StartAsync<TSource>(Func<CancellationToken, Task<TSource>> functionAsync, in TaskObservationOptions.Value options)
        {
            return StartAsyncImpl(functionAsync, options);
        }

        private IObservable<TSource> StartAsyncImpl<TSource>(Func<CancellationToken, Task<TSource>> functionAsync, in TaskObservationOptions.Value options)
        {
            var cancellable = new CancellationDisposable();

            Task<TSource> task;
            try
            {
                task = functionAsync(cancellable.Token);
            }
            catch (Exception exception)
            {
                return Throw<TSource>(exception);
            }

            var result = task.ToObservable(options);

            return new StartAsyncObservable<TSource>(cancellable, result);
        }

        private sealed class StartAsyncObservable<TSource> : ObservableBase<TSource>
        {
            private readonly CancellationDisposable _cancellable;
            private readonly IObservable<TSource> _result;

            public StartAsyncObservable(CancellationDisposable cancellable, IObservable<TSource> result)
            {
                _cancellable = cancellable;
                _result = result;
            }

            protected override IDisposable SubscribeCore(IObserver<TSource> observer)
            {
                //
                // [OK] Use of unsafe Subscribe: result is an AsyncSubject<TSource>.
                //
                var subscription = _result.Subscribe/*Unsafe*/(observer);
                return StableCompositeDisposable.Create(_cancellable, subscription);
            }
        }

        #endregion

        #region Action

        public virtual IObservable<Unit> Start(Action action)
        {
            return ToAsync(action, SchedulerDefaults.AsyncConversions)();
        }

        public virtual IObservable<Unit> Start(Action action, IScheduler scheduler)
        {
            return ToAsync(action, scheduler)();
        }

        public virtual IObservable<Unit> StartAsync(Func<Task> actionAsync)
        {
            return StartAsyncImpl(actionAsync, new TaskObservationOptions.Value(null, ignoreExceptionsAfterUnsubscribe: false));
        }

        public virtual IObservable<Unit> StartAsync(Func<Task> actionAsync, in TaskObservationOptions.Value options)
        {
            return StartAsyncImpl(actionAsync, options);
        }

        private IObservable<Unit> StartAsyncImpl(Func<Task> actionAsync, in TaskObservationOptions.Value options)
        {
            Task task;
            try
            {
                task = actionAsync();
            }
            catch (Exception exception)
            {
                return Throw<Unit>(exception);
            }

            return task.ToObservable(options);
        }

        public virtual IObservable<Unit> StartAsync(Func<CancellationToken, Task> actionAsync)
        {
            return StartAsyncImpl(actionAsync, new TaskObservationOptions.Value(null, ignoreExceptionsAfterUnsubscribe: false));
        }

        public virtual IObservable<Unit> StartAsync(Func<CancellationToken, Task> actionAsync, in TaskObservationOptions.Value options)
        {
            return StartAsyncImpl(actionAsync, options);
        }

        private IObservable<Unit> StartAsyncImpl(Func<CancellationToken, Task> actionAsync, in TaskObservationOptions.Value options)
        {
            var cancellable = new CancellationDisposable();

            Task task;
            try
            {
                task = actionAsync(cancellable.Token);
            }
            catch (Exception exception)
            {
                return Throw<Unit>(exception);
            }

            var result = task.ToObservable(options);

            return new StartAsyncObservable<Unit>(cancellable, result);
        }

        #endregion

        #endregion

        #region FromAsync

        #region Func

        public virtual IObservable<TResult> FromAsync<TResult>(Func<Task<TResult>> functionAsync)
        {
            return Defer(() => StartAsync(functionAsync));
        }

        public virtual IObservable<TResult> FromAsync<TResult>(Func<CancellationToken, Task<TResult>> functionAsync)
        {
            return Defer(() => StartAsync(functionAsync));
        }

        public virtual IObservable<TResult> FromAsync<TResult>(Func<Task<TResult>> functionAsync, TaskObservationOptions.Value options)
        {
            return Defer(() => StartAsync(functionAsync, options));
        }

        public virtual IObservable<TResult> FromAsync<TResult>(Func<CancellationToken, Task<TResult>> functionAsync, TaskObservationOptions.Value options)
        {
            return Defer(() => StartAsync(functionAsync, options));
        }

        #endregion

        #region Action

        public virtual IObservable<Unit> FromAsync(Func<Task> actionAsync)
        {
            return Defer(() => StartAsync(actionAsync));
        }

        public virtual IObservable<Unit> FromAsync(Func<CancellationToken, Task> actionAsync)
        {
            return Defer(() => StartAsync(actionAsync));
        }

        public virtual IObservable<Unit> FromAsync(Func<Task> actionAsync, TaskObservationOptions.Value options)
        {
            return Defer(() => StartAsync(actionAsync, options));
        }

        public virtual IObservable<Unit> FromAsync(Func<CancellationToken, Task> actionAsync, TaskObservationOptions.Value options)
        {
            return Defer(() => StartAsync(actionAsync, options));
        }

        #endregion

        #endregion

        #region ToAsync

        #region Func

        public virtual Func<IObservable<TResult>> ToAsync<TResult>(Func<TResult> function)
        {
            return ToAsync(function, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<IObservable<TResult>> ToAsync<TResult>(Func<TResult> function, IScheduler scheduler)
        {
            return () =>
            {
                var subject = new AsyncSubject<TResult>();
                scheduler.ScheduleAction((function, subject), state =>
                {
                    TResult result;
                    try
                    {
                        result = state.function();
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(result);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T, IObservable<TResult>> ToAsync<T, TResult>(Func<T, TResult> function)
        {
            return ToAsync(function, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T, IObservable<TResult>> ToAsync<T, TResult>(Func<T, TResult> function, IScheduler scheduler)
        {
            return first =>
            {
                var subject = new AsyncSubject<TResult>();
                scheduler.ScheduleAction((function, subject, first), state =>
                {
                    TResult result;
                    try
                    {
                        result = state.function(state.first);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(result);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, IObservable<TResult>> ToAsync<T1, T2, TResult>(Func<T1, T2, TResult> function)
        {
            return ToAsync(function, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T1, T2, IObservable<TResult>> ToAsync<T1, T2, TResult>(Func<T1, T2, TResult> function, IScheduler scheduler)
        {
            return (first, second) =>
            {
                var subject = new AsyncSubject<TResult>();
                scheduler.ScheduleAction((subject, function, first, second), state =>
                {
                    TResult result;
                    try
                    {
                        result = state.function(state.first, state.second);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(result);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, IObservable<TResult>> ToAsync<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> function)
        {
            return ToAsync(function, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T1, T2, T3, IObservable<TResult>> ToAsync<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> function, IScheduler scheduler)
        {
            return (first, second, third) =>
            {
                var subject = new AsyncSubject<TResult>();
                scheduler.ScheduleAction((subject, function, first, second, third), state =>
                {
                    TResult result;
                    try
                    {
                        result = state.function(state.first, state.second, state.third);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(result);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, IObservable<TResult>> ToAsync<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> function)
        {
            return ToAsync(function, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T1, T2, T3, T4, IObservable<TResult>> ToAsync<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> function, IScheduler scheduler)
        {
            return (first, second, third, fourth) =>
            {
                var subject = new AsyncSubject<TResult>();
                scheduler.ScheduleAction((subject, function, first, second, third, fourth), state =>
                {
                    TResult result;
                    try
                    {
                        result = state.function(state.first, state.second, state.third, state.fourth);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(result);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> function)
        {
            return ToAsync(function, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T1, T2, T3, T4, T5, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> function, IScheduler scheduler)
        {
            return (first, second, third, fourth, fifth) =>
            {
                var subject = new AsyncSubject<TResult>();
                scheduler.ScheduleAction((subject, function, first, second, third, fourth, fifth), state =>
                {
                    TResult result;
                    try
                    {
                        result = state.function(state.first, state.second, state.third, state.fourth, state.fifth);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(result);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> function)
        {
            return ToAsync(function, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> function, IScheduler scheduler)
        {
            return (first, second, third, fourth, fifth, sixth) =>
            {
                var subject = new AsyncSubject<TResult>();
                scheduler.ScheduleAction((subject, function, first, second, third, fourth, fifth, sixth), state =>
                {
                    TResult result;
                    try
                    {
                        result = state.function(state.first, state.second, state.third, state.fourth, state.fifth, state.sixth);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(result);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> function)
        {
            return ToAsync(function, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> function, IScheduler scheduler)
        {
            return (first, second, third, fourth, fifth, sixth, seventh) =>
            {
                var subject = new AsyncSubject<TResult>();
                scheduler.ScheduleAction((subject, function, first, second, third, fourth, fifth, sixth, seventh), state =>
                {
                    TResult result;
                    try
                    {
                        result = state.function(state.first, state.second, state.third, state.fourth, state.fifth, state.sixth, state.seventh);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(result);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> function)
        {
            return ToAsync(function, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> function, IScheduler scheduler)
        {
            return (first, second, third, fourth, fifth, sixth, seventh, eight) =>
            {
                var subject = new AsyncSubject<TResult>();
                scheduler.ScheduleAction((subject, function, first, second, third, fourth, fifth, sixth, seventh, eight), state =>
                {
                    TResult result;
                    try
                    {
                        result = state.function(state.first, state.second, state.third, state.fourth, state.fifth, state.sixth, state.seventh, state.eight);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(result);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> function)
        {
            return ToAsync(function, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> function, IScheduler scheduler)
        {
            return (first, second, third, fourth, fifth, sixth, seventh, eight, ninth) =>
            {
                var subject = new AsyncSubject<TResult>();
                scheduler.ScheduleAction((subject, function, first, second, third, fourth, fifth, sixth, seventh, eight, ninth), state =>
                {
                    TResult result;
                    try
                    {
                        result = state.function(state.first, state.second, state.third, state.fourth, state.fifth, state.sixth, state.seventh, state.eight, state.ninth);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(result);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> function)
        {
            return ToAsync(function, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> function, IScheduler scheduler)
        {
            return (first, second, third, fourth, fifth, sixth, seventh, eight, ninth, tenth) =>
            {
                var subject = new AsyncSubject<TResult>();
                scheduler.ScheduleAction((subject, function, first, second, third, fourth, fifth, sixth, seventh, eight, ninth, tenth), state =>
                {
                    TResult result;
                    try
                    {
                        result = state.function(state.first, state.second, state.third, state.fourth, state.fifth, state.sixth, state.seventh, state.eight, state.ninth, state.tenth);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(result);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> function)
        {
            return ToAsync(function, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> function, IScheduler scheduler)
        {
            return (first, second, third, fourth, fifth, sixth, seventh, eight, ninth, tenth, eleventh) =>
            {
                var subject = new AsyncSubject<TResult>();
                scheduler.ScheduleAction((subject, function, first, second, third, fourth, fifth, sixth, seventh, eight, ninth, tenth, eleventh), state =>
                {
                    TResult result;
                    try
                    {
                        result = state.function(state.first, state.second, state.third, state.fourth, state.fifth, state.sixth, state.seventh, state.eight, state.ninth, state.tenth, state.eleventh);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(result);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> function)
        {
            return ToAsync(function, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> function, IScheduler scheduler)
        {
            return (first, second, third, fourth, fifth, sixth, seventh, eight, ninth, tenth, eleventh, twelfth) =>
            {
                var subject = new AsyncSubject<TResult>();
                scheduler.ScheduleAction((subject, function, first, second, third, fourth, fifth, sixth, seventh, eight, ninth, tenth, eleventh, twelfth), state =>
                {
                    TResult result;
                    try
                    {
                        result = state.function(state.first, state.second, state.third, state.fourth, state.fifth, state.sixth, state.seventh, state.eight, state.ninth, state.tenth, state.eleventh, state.twelfth);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(result);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> function)
        {
            return ToAsync(function, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> function, IScheduler scheduler)
        {
            return (first, second, third, fourth, fifth, sixth, seventh, eight, ninth, tenth, eleventh, twelfth, thirteenth) =>
            {
                var subject = new AsyncSubject<TResult>();
                scheduler.ScheduleAction((subject, function, first, second, third, fourth, fifth, sixth, seventh, eight, ninth, tenth, eleventh, twelfth, thirteenth), state =>
                {
                    TResult result;
                    try
                    {
                        result = state.function(state.first, state.second, state.third, state.fourth, state.fifth, state.sixth, state.seventh, state.eight, state.ninth, state.tenth, state.eleventh, state.twelfth, state.thirteenth);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(result);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> function)
        {
            return ToAsync(function, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> function, IScheduler scheduler)
        {
            return (first, second, third, fourth, fifth, sixth, seventh, eight, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth) =>
            {
                var subject = new AsyncSubject<TResult>();
                scheduler.ScheduleAction((subject, function, first, second, third, fourth, fifth, sixth, seventh, eight, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth), state =>
                {
                    TResult result;
                    try
                    {
                        result = state.function(state.first, state.second, state.third, state.fourth, state.fifth, state.sixth, state.seventh, state.eight, state.ninth, state.tenth, state.eleventh, state.twelfth, state.thirteenth, state.fourteenth);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(result);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> function)
        {
            return ToAsync(function, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> function, IScheduler scheduler)
        {
            return (first, second, third, fourth, fifth, sixth, seventh, eight, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth) =>
            {
                var subject = new AsyncSubject<TResult>();
                scheduler.ScheduleAction((subject, function, first, second, third, fourth, fifth, sixth, seventh, eight, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth), state =>
                {
                    TResult result;
                    try
                    {
                        result = state.function(state.first, state.second, state.third, state.fourth, state.fifth, state.sixth, state.seventh, state.eight, state.ninth, state.tenth, state.eleventh, state.twelfth, state.thirteenth, state.fourteenth, state.fifteenth);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(result);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> function)
        {
            return ToAsync(function, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> function, IScheduler scheduler)
        {
            return (first, second, third, fourth, fifth, sixth, seventh, eight, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth, sixteenth) =>
            {
                var subject = new AsyncSubject<TResult>();
                scheduler.ScheduleAction((subject, function, first, second, third, fourth, fifth, sixth, seventh, eight, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth, sixteenth), state =>
                {
                    TResult result;
                    try
                    {
                        result = state.function(state.first, state.second, state.third, state.fourth, state.fifth, state.sixth, state.seventh, state.eight, state.ninth, state.tenth, state.eleventh, state.twelfth, state.thirteenth, state.fourteenth, state.fifteenth, state.sixteenth);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(result);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        #endregion

        #region Action

        public virtual Func<IObservable<Unit>> ToAsync(Action action)
        {
            return ToAsync(action, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<IObservable<Unit>> ToAsync(Action action, IScheduler scheduler)
        {
            return () =>
            {
                var subject = new AsyncSubject<Unit>();
                scheduler.ScheduleAction((subject, action), state =>
                {
                    try
                    {
                        state.action();
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(Unit.Default);
                    state.subject.OnCompleted();
                });

                return subject.AsObservable();
            };
        }

        public virtual Func<TSource, IObservable<Unit>> ToAsync<TSource>(Action<TSource> action)
        {
            return ToAsync(action, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<TSource, IObservable<Unit>> ToAsync<TSource>(Action<TSource> action, IScheduler scheduler)
        {
            return first =>
            {
                var subject = new AsyncSubject<Unit>();
                scheduler.ScheduleAction((subject, action, first), state =>
                {
                    try
                    {
                        state.action(state.first);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(Unit.Default);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, IObservable<Unit>> ToAsync<T1, T2>(Action<T1, T2> action)
        {
            return ToAsync(action, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T1, T2, IObservable<Unit>> ToAsync<T1, T2>(Action<T1, T2> action, IScheduler scheduler)
        {
            return (first, second) =>
            {
                var subject = new AsyncSubject<Unit>();
                scheduler.ScheduleAction((subject, action, first, second), state =>
                {
                    try
                    {
                        state.action(state.first, state.second);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(Unit.Default);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, IObservable<Unit>> ToAsync<T1, T2, T3>(Action<T1, T2, T3> action)
        {
            return ToAsync(action, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T1, T2, T3, IObservable<Unit>> ToAsync<T1, T2, T3>(Action<T1, T2, T3> action, IScheduler scheduler)
        {
            return (first, second, third) =>
            {
                var subject = new AsyncSubject<Unit>();
                scheduler.ScheduleAction((subject, action, first, second, third), state =>
                {
                    try
                    {
                        state.action(state.first, state.second, state.third);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(Unit.Default);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, IObservable<Unit>> ToAsync<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action)
        {
            return ToAsync(action, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T1, T2, T3, T4, IObservable<Unit>> ToAsync<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, IScheduler scheduler)
        {
            return (first, second, third, fourth) =>
            {
                var subject = new AsyncSubject<Unit>();
                scheduler.ScheduleAction((subject, action, first, second, third, fourth), state =>
                {
                    try
                    {
                        state.action(state.first, state.second, state.third, state.fourth);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(Unit.Default);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action)
        {
            return ToAsync(action, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T1, T2, T3, T4, T5, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action, IScheduler scheduler)
        {
            return (first, second, third, fourth, fifth) =>
            {
                var subject = new AsyncSubject<Unit>();
                scheduler.ScheduleAction((subject, action, first, second, third, fourth, fifth), state =>
                {
                    try
                    {
                        state.action(state.first, state.second, state.third, state.fourth, state.fifth);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(Unit.Default);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action)
        {
            return ToAsync(action, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action, IScheduler scheduler)
        {
            return (first, second, third, fourth, fifth, sixth) =>
            {
                var subject = new AsyncSubject<Unit>();
                scheduler.ScheduleAction((subject, action, first, second, third, fourth, fifth, sixth), state =>
                {
                    try
                    {
                        state.action(state.first, state.second, state.third, state.fourth, state.fifth, state.sixth);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(Unit.Default);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action)
        {
            return ToAsync(action, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action, IScheduler scheduler)
        {
            return (first, second, third, fourth, fifth, sixth, seventh) =>
            {
                var subject = new AsyncSubject<Unit>();
                scheduler.ScheduleAction((subject, action, first, second, third, fourth, fifth, sixth, seventh), state =>
                {
                    try
                    {
                        state.action(state.first, state.second, state.third, state.fourth, state.fifth, state.sixth, state.seventh);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(Unit.Default);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            return ToAsync(action, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action, IScheduler scheduler)
        {
            return (first, second, third, fourth, fifth, sixth, seventh, eighth) =>
            {
                var subject = new AsyncSubject<Unit>();
                scheduler.ScheduleAction((subject, action, first, second, third, fourth, fifth, sixth, seventh, eighth), state =>
                {
                    try
                    {
                        state.action(state.first, state.second, state.third, state.fourth, state.fifth, state.sixth, state.seventh, state.eighth);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(Unit.Default);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
        {
            return ToAsync(action, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, IScheduler scheduler)
        {
            return (first, second, third, fourth, fifth, sixth, seventh, eighth, ninth) =>
            {
                var subject = new AsyncSubject<Unit>();
                scheduler.ScheduleAction((subject, action, first, second, third, fourth, fifth, sixth, seventh, eighth, ninth), state =>
                {
                    try
                    {
                        state.action(state.first, state.second, state.third, state.fourth, state.fifth, state.sixth, state.seventh, state.eighth, state.ninth);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(Unit.Default);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
        {
            return ToAsync(action, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, IScheduler scheduler)
        {
            return (first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth) =>
            {
                var subject = new AsyncSubject<Unit>();
                scheduler.ScheduleAction((subject, action, first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth), state =>
                {
                    try
                    {
                        state.action(state.first, state.second, state.third, state.fourth, state.fifth, state.sixth, state.seventh, state.eighth, state.ninth, state.tenth);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(Unit.Default);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
        {
            return ToAsync(action, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, IScheduler scheduler)
        {
            return (first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh) =>
            {
                var subject = new AsyncSubject<Unit>();
                scheduler.ScheduleAction((subject, action, first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh), state =>
                {
                    try
                    {
                        state.action(state.first, state.second, state.third, state.fourth, state.fifth, state.sixth, state.seventh, state.eighth, state.ninth, state.tenth, state.eleventh);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(Unit.Default);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
        {
            return ToAsync(action, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, IScheduler scheduler)
        {
            return (first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth) =>
            {
                var subject = new AsyncSubject<Unit>();
                scheduler.ScheduleAction((subject, action, first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth), state =>
                {
                    try
                    {
                        state.action(state.first, state.second, state.third, state.fourth, state.fifth, state.sixth, state.seventh, state.eighth, state.ninth, state.tenth, state.eleventh, state.twelfth);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(Unit.Default);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
        {
            return ToAsync(action, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, IScheduler scheduler)
        {
            return (first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth) =>
            {
                var subject = new AsyncSubject<Unit>();
                scheduler.ScheduleAction((subject, action, first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth), state =>
                {
                    try
                    {
                        state.action(state.first, state.second, state.third, state.fourth, state.fifth, state.sixth, state.seventh, state.eighth, state.ninth, state.tenth, state.eleventh, state.twelfth, state.thirteenth);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(Unit.Default);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
        {
            return ToAsync(action, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, IScheduler scheduler)
        {
            return (first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth) =>
            {
                var subject = new AsyncSubject<Unit>();
                scheduler.ScheduleAction((subject, action, first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth), state =>
                {
                    try
                    {
                        state.action(state.first, state.second, state.third, state.fourth, state.fifth, state.sixth, state.seventh, state.eighth, state.ninth, state.tenth, state.eleventh, state.twelfth, state.thirteenth, state.fourteenth);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(Unit.Default);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
        {
            return ToAsync(action, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, IScheduler scheduler)
        {
            return (first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth) =>
            {
                var subject = new AsyncSubject<Unit>();
                scheduler.ScheduleAction((subject, action, first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth), state =>
                {
                    try
                    {
                        state.action(state.first, state.second, state.third, state.fourth, state.fifth, state.sixth, state.seventh, state.eighth, state.ninth, state.tenth, state.eleventh, state.twelfth, state.thirteenth, state.fourteenth, state.fifteenth);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(Unit.Default);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
        {
            return ToAsync(action, SchedulerDefaults.AsyncConversions);
        }

        public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, IScheduler scheduler)
        {
            return (first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth, sixteenth) =>
            {
                var subject = new AsyncSubject<Unit>();
                scheduler.ScheduleAction((subject, action, first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth, sixteenth), state =>
                {
                    try
                    {
                        state.action(state.first, state.second, state.third, state.fourth, state.fifth, state.sixth, state.seventh, state.eighth, state.ninth, state.tenth, state.eleventh, state.twelfth, state.thirteenth, state.fourteenth, state.fifteenth, state.sixteenth);
                    }
                    catch (Exception exception)
                    {
                        state.subject.OnError(exception);
                        return;
                    }
                    state.subject.OnNext(Unit.Default);
                    state.subject.OnCompleted();
                });
                return subject.AsObservable();
            };
        }

        #endregion

        #endregion
    }
}
