// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Threading;

namespace Tests
{
    public partial class AsyncTests
    {
        [Fact]
        public void ToAsyncEnumerable_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToAsyncEnumerable<int>(default(IEnumerable<int>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToAsyncEnumerable<int>(default(IObservable<int>)));
        }

        [Fact]
        public void ToAsyncEnumerable1()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var e = xs.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void ToAsyncEnumerable2()
        {
            var ex = new Exception("Bang");
            var xs = ToAsyncEnumerable_Sequence(ex).ToAsyncEnumerable();
            var e = xs.GetEnumerator();
            HasNext(e, 42);
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).InnerExceptions.Single() == ex);
        }

        private IEnumerable<int> ToAsyncEnumerable_Sequence(Exception e)
        {
            yield return 42;
            throw e;
        }

        [Fact]
        public void ToAsyncEnumerable3()
        {
            var subscribed = false;

            var xs = new MyObservable<int>(obs =>
            {
                subscribed = true;

                obs.OnNext(42);
                obs.OnCompleted();

                return new MyDisposable(() => { });
            }).ToAsyncEnumerable();

            Assert.False(subscribed);

            var e = xs.GetEnumerator();

            Assert.True(subscribed);

            HasNext(e, 42);
            NoNext(e);
        }

        [Fact]
        public void ToAsyncEnumerable4()
        {
            var ex = new Exception("Bang!");
            var subscribed = false;

            var xs = new MyObservable<int>(obs =>
            {
                subscribed = true;

                obs.OnError(ex);

                return new MyDisposable(() => { });
            }).ToAsyncEnumerable();

            Assert.False(subscribed);

            var e = xs.GetEnumerator();

            Assert.True(subscribed);

            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).InnerExceptions.Single() == ex);
        }

        [Fact]
        public void ToAsyncEnumerable_With_Completed_Task()
        {
            var task = Task.Factory.StartNew(() => 36);

            var xs = task.ToAsyncEnumerable();
            var e = xs.GetEnumerator();

            Assert.True(e.MoveNext().Result);
            Assert.Equal(36, e.Current);
            Assert.False(e.MoveNext().Result);
        }

        [Fact]
        public void ToAsyncEnumerable_With_Faulted_Task()
        {
            var ex = new InvalidOperationException();
            var tcs = new TaskCompletionSource<int>();
            tcs.SetException(ex);

            var xs = tcs.Task.ToAsyncEnumerable();
            var e = xs.GetEnumerator();

            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).InnerExceptions.Single() == ex);
        }

        [Fact]
        public void ToAsyncEnumerable_With_Canceled_Task()
        {
            var tcs = new TaskCompletionSource<int>();
            tcs.SetCanceled();

            var xs = tcs.Task.ToAsyncEnumerable();
            var e = xs.GetEnumerator();

            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).InnerExceptions.Single() is TaskCanceledException);
        }

        class MyObservable<T> : IObservable<T>
        {
            private Func<IObserver<T>, IDisposable> _subscribe;

            public MyObservable(Func<IObserver<T>, IDisposable> subscribe)
            {
                _subscribe = subscribe;
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                return _subscribe(observer);
            }
        }

        class MyDisposable : IDisposable
        {
            private Action _dispose;

            public MyDisposable(Action dispose)
            {
                _dispose = dispose;
            }

            public void Dispose()
            {
                _dispose();
            }
        }

        [Fact]
        public void ToEnumerable_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToEnumerable<int>(null));
        }

        [Fact]
        public void ToEnumerable1()
        {
            var xs = AsyncEnumerable.Return(42).ToEnumerable();
            Assert.True(xs.SequenceEqual(new[] { 42 }));
        }

        [Fact]
        public void ToEnumerable2()
        {
            var xs = AsyncEnumerable.Empty<int>().ToEnumerable();
            Assert.True(xs.SequenceEqual(new int[0]));
        }

        [Fact]
        public void ToEnumerable3()
        {
            var ex = new Exception("Bang");
            var xs = AsyncEnumerable.Throw<int>(ex).ToEnumerable();
            AssertThrows<Exception>(() => xs.GetEnumerator().MoveNext(), ex_ => ((AggregateException)ex_).InnerExceptions.Single() == ex);
        }

        [Fact]
        public void ToObservable_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToObservable<int>(null));
        }

        [Fact]
        public void ToObservable1()
        {
            var fail = false;
            var evt = new ManualResetEvent(false);

            var xs = AsyncEnumerable.Empty<int>().ToObservable();
            xs.Subscribe(new MyObserver<int>(
                x =>
                {
                    fail = true;
                },
                ex =>
                {
                    fail = true;
                    evt.Set();
                },
                () =>
                {
                    evt.Set();
                }
            ));

            evt.WaitOne();
            Assert.False(fail);
        }

        [Fact]
        public void ToObservable2()
        {
            var lst = new List<int>();
            var fail = false;
            var evt = new ManualResetEvent(false);

            var xs = AsyncEnumerable.Return(42).ToObservable();
            xs.Subscribe(new MyObserver<int>(
                x =>
                {
                    lst.Add(x);
                },
                ex =>
                {
                    fail = true;
                    evt.Set();
                },
                () =>
                {
                    evt.Set();
                }
            ));

            evt.WaitOne();
            Assert.False(fail);
            Assert.True(lst.SequenceEqual(new[] { 42 }));
        }

        [Fact]
        public void ToObservable3()
        {
            var lst = new List<int>();
            var fail = false;
            var evt = new ManualResetEvent(false);

            var xs = AsyncEnumerable.Range(0, 10).ToObservable();
            xs.Subscribe(new MyObserver<int>(
                x =>
                {
                    lst.Add(x);
                },
                ex =>
                {
                    fail = true;
                    evt.Set();
                },
                () =>
                {
                    evt.Set();
                }
            ));

            evt.WaitOne();
            Assert.False(fail);
            Assert.True(lst.SequenceEqual(Enumerable.Range(0, 10)));
        }

        [Fact]
        public void ToObservable4()
        {
            var ex1 = new Exception("Bang!");
            var ex_ = default(Exception);
            var fail = false;
            var evt = new ManualResetEvent(false);

            var xs = AsyncEnumerable.Throw<int>(ex1).ToObservable();
            xs.Subscribe(new MyObserver<int>(
                x =>
                {
                    fail = true;
                },
                ex =>
                {
                    ex_ = ex;
                    evt.Set();
                },
                () =>
                {
                    fail = true;
                    evt.Set();
                }
            ));

            evt.WaitOne();
            Assert.False(fail);
            Assert.Equal(ex1, ((AggregateException)ex_).InnerExceptions.Single());
        }

        [Fact]
        public void ToObservable_disposes_enumerator_on_completion()
        {
            var fail = false;
            var evt = new ManualResetEvent(false);

            var ae = AsyncEnumerable.CreateEnumerable(
                () => AsyncEnumerable.CreateEnumerator<int>(
                    ct => Task.FromResult(false),
                    () => { throw new InvalidOperationException(); },
                    () => { evt.Set(); }));

            ae
                .ToObservable()
                .Subscribe(new MyObserver<int>(
                    x =>
                    {
                        fail = true;
                    },
                    ex =>
                    {
                        fail = true;
                    },
                    () =>
                    {
                    }
                ));

            evt.WaitOne();
            Assert.False(fail);
        }

        [Fact]
        public void ToObservable_disposes_enumerator_when_subscription_is_disposed()
        {
            var fail = false;
            var evt = new ManualResetEvent(false);
            var subscription = default(IDisposable);
            var subscriptionAssignedTcs = new TaskCompletionSource<object>();

            var ae = AsyncEnumerable.CreateEnumerable(
                () => AsyncEnumerable.CreateEnumerator(
                    async ct =>
                    {
                        await subscriptionAssignedTcs.Task;
                        return true;
                    },
                    () => 1,
                    () => { evt.Set(); }));

            subscription = ae
                .ToObservable()
                .Subscribe(new MyObserver<int>(
                    x =>
                    {
                        subscription.Dispose();
                    },
                    ex =>
                    {
                        fail = true;
                    },
                    () =>
                    {
                        fail = true;
                    }
                ));

            subscriptionAssignedTcs.SetResult(null);
            evt.WaitOne();

            Assert.False(fail);
        }

        [Fact]
        public void ToObservable_does_not_call_MoveNext_again_when_subscription_is_disposed()
        {
            var fail = false;
            var moveNextCount = 0;
            var evt = new ManualResetEvent(false);
            var subscription = default(IDisposable);
            var subscriptionAssignedTcs = new TaskCompletionSource<object>();

            var ae = AsyncEnumerable.CreateEnumerable(
                () => AsyncEnumerable.CreateEnumerator(
                    async ct =>
                    {
                        await subscriptionAssignedTcs.Task;

                        moveNextCount++;
                        return true;
                    },
                    () => 1,
                    () => { evt.Set(); }));

            subscription = ae
                .ToObservable()
                .Subscribe(new MyObserver<int>(
                    x =>
                    {
                        subscription.Dispose();
                    },
                    ex =>
                    {
                        fail = true;
                    },
                    () =>
                    {
                        fail = true;
                    }
                ));

            subscriptionAssignedTcs.SetResult(null);
            evt.WaitOne();

            Assert.Equal(1, moveNextCount);
            Assert.False(fail);
        }

        class MyObserver<T> : IObserver<T>
        {
            private Action<T> _onNext;
            private Action<Exception> _onError;
            private Action _onCompleted;

            public MyObserver(Action<T> onNext, Action<Exception> onError, Action onCompleted)
            {
                _onNext = onNext;
                _onError = onError;
                _onCompleted = onCompleted;
            }

            public void OnCompleted()
            {
                _onCompleted();
            }

            public void OnError(Exception error)
            {
                _onError(error);
            }

            public void OnNext(T value)
            {
                _onNext(value);
            }
        }
    }
}