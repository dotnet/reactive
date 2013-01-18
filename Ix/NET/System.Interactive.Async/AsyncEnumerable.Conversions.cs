// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> ToAsyncEnumerable<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Create(() =>
            {
                var e = source.GetEnumerator();

                return Create(
                    ct => Task.Factory.StartNew(() =>
                    {
                        var res = default(bool);
                        try
                        {
                            res = e.MoveNext();
                        }
                        finally
                        {
                            if (!res)
                                e.Dispose();
                        }
                        return res;
                    }, ct),
                    () => e.Current,
                    () => e.Dispose()
                );
            });
        }

        public static IEnumerable<TSource> ToEnumerable<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return ToEnumerable_(source);
        }

        private static IEnumerable<TSource> ToEnumerable_<TSource>(IAsyncEnumerable<TSource> source)
        {
            using (var e = source.GetEnumerator())
            {
                while (true)
                {
                    var t = e.MoveNext(CancellationToken.None);
                    t.Wait();
                    if (!t.Result)
                        break;
                    var c = e.Current;
                    yield return c;
                }
            }
        }

#if !NO_RXINTERFACES
        public static IAsyncEnumerable<TSource> ToAsyncEnumerable<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Create(() =>
            {
                var observer = new ToAsyncEnumerableObserver<TSource>();
                observer.Queue = new Queue<Either<TSource, Exception, bool>>();

                var subscription = source.Subscribe(observer);

                return Create(
                    (ct, tcs) =>
                    {
                        lock (observer.Queue)
                        {
                            if (observer.Queue.Count > 0)
                            {
                                var n = observer.Queue.Dequeue();
                                n.Switch(
                                    x =>
                                    {
                                        observer.Current = x;
                                        tcs.TrySetResult(true);
                                    },
                                    ex =>
                                    {
                                        tcs.TrySetException(ex);
                                    },
                                    _ =>
                                    {
                                        tcs.TrySetResult(false);
                                    }
                                );
                            }
                            else
                                observer.TaskCompletionSource = tcs;
                        }

                        return tcs.Task;
                    },
                    () => observer.Current,
                    () =>
                    {
                        subscription.Dispose();
                        // Should we cancel in-flight operations somehow?
                    });
            });
        }

        class ToAsyncEnumerableObserver<T> : IObserver<T>
        {
            public Queue<Either<T, Exception, bool>> Queue { get; set; }
            public T Current { get; set; }
            public TaskCompletionSource<bool> TaskCompletionSource { get; set; }

            public void OnCompleted()
            {
                lock (Queue)
                {
                    if (TaskCompletionSource == null)
                        Queue.Enqueue(new Either<T, Exception, bool>.Choice3(true));
                    else
                    {
                        TaskCompletionSource.SetResult(false);
                        TaskCompletionSource = null;
                    }
                }
            }

            public void OnError(Exception error)
            {
                lock (Queue)
                {
                    if (TaskCompletionSource == null)
                        Queue.Enqueue(new Either<T, Exception, bool>.Choice2(error));
                    else
                    {
                        TaskCompletionSource.SetException(error);
                        TaskCompletionSource = null;
                    }
                }
            }

            public void OnNext(T value)
            {
                lock (Queue)
                {
                    if (TaskCompletionSource == null)
                        Queue.Enqueue(new Either<T, Exception, bool>.Choice1(value));
                    else
                    {
                        Current = value;
                        TaskCompletionSource.SetResult(true);
                        TaskCompletionSource = null;
                    }
                }
            }
        }

        abstract class Either<T, U, V>
        {
            public abstract void Switch(Action<T> choice1, Action<U> choice2, Action<V> choice3);

            public class Choice1 : Either<T, U, V>
            {
                public Choice1(T value) { Value = value; }
                public T Value { get; private set; }

                public override void Switch(Action<T> choice1, Action<U> choice2, Action<V> choice3)
                {
                    choice1(Value);
                }
            }

            public class Choice2 : Either<T, U, V>
            {
                public Choice2(U value) { Value = value; }
                public U Value { get; private set; }

                public override void Switch(Action<T> choice1, Action<U> choice2, Action<V> choice3)
                {
                    choice2(Value);
                }
            }

            public class Choice3 : Either<T, U, V>
            {
                public Choice3(V value) { Value = value; }
                public V Value { get; private set; }

                public override void Switch(Action<T> choice1, Action<U> choice2, Action<V> choice3)
                {
                    choice3(Value);
                }
            }
        }

        public static IObservable<TSource> ToObservable<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return new ToObservableObservable<TSource>(source);
        }

        class ToObservableObservable<T> : IObservable<T>
        {
            private readonly IAsyncEnumerable<T> source;

            public ToObservableObservable(IAsyncEnumerable<T> source)
            {
                this.source = source;
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                var ctd = new CancellationTokenDisposable();
                var e = source.GetEnumerator();

                var f = default(Action);
                f = () => e.MoveNext(ctd.Token).ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        observer.OnError(t.Exception);
                        e.Dispose();
                    }
                    else if (t.IsCanceled)
                    {
                        e.Dispose();
                    }
                    else if (t.IsCompleted)
                    {
                        if (t.Result)
                        {
                            observer.OnNext(e.Current);
                            f();
                        }
                        else
                        {
                            observer.OnCompleted();
                            e.Dispose();
                        }
                    }
                }, ctd.Token);

                f();

                return new CompositeDisposable(ctd, e);
            }
        }
#endif
    }
}
