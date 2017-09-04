// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class ToObservable : AsyncEnumerableTests
    {
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

            var xs = Return42.ToObservable();
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

            var xs = Throw<int>(ex1).ToObservable();
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
                    () => Task.FromResult(false),
                    () => { throw new InvalidOperationException(); },
                    () => { evt.Set(); return Task.FromResult(true); }));

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
                    async () =>
                    {
                        await subscriptionAssignedTcs.Task;
                        return true;
                    },
                    () => 1,
                    () => { evt.Set(); return Task.FromResult(true); }));

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
                    async () =>
                    {
                        await subscriptionAssignedTcs.Task;

                        moveNextCount++;
                        return true;
                    },
                    () => 1,
                    () => { evt.Set(); return Task.FromResult(true); }));

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
