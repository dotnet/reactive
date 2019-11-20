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
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.ToObservable<int>(null));
        }

        [Fact]
        public void ToObservable1()
        {
            using var evt = new ManualResetEvent(false);

            var fail = false;

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
            using var evt = new ManualResetEvent(false);

            var lst = new List<int>();
            var fail = false;

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
            using var evt = new ManualResetEvent(false);

            var lst = new List<int>();
            var fail = false;

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
        public void ToObservable_ThrowOnMoveNext()
        {
            using var evt = new ManualResetEvent(false);

            var ex1 = new Exception("Bang!");
            var ex_ = default(Exception);
            var fail = false;

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
            Assert.Equal(ex1, ex_);
        }

        [Fact]
        public void ToObservable_ThrowOnCurrent()
        {
            var ex1 = new Exception("Bang!");
            var ex_ = default(Exception);
            var fail = false;

            var ae = AsyncEnumerable.Create(
                _ => new ThrowOnCurrentAsyncEnumerator(ex1)
            );

            ae.ToObservable()
                .Subscribe(new MyObserver<int>(
                x =>
                {
                    fail = true;
                },
                ex =>
                {
                    ex_ = ex;
                },
                () =>
                {
                    fail = true;
                }
            ));

            Assert.False(fail);
            Assert.Equal(ex1, ex_);
        }

        [Fact]
        public void ToObservable_DisposesEnumeratorOnCompletion()
        {
            using var evt = new ManualResetEvent(false);

            var fail = false;

            var ae = AsyncEnumerable.Create(
                _ => AsyncEnumerator.Create<int>(
                    () => new ValueTask<bool>(false),
                    () => { throw new InvalidOperationException(); },
                    () => { evt.Set(); return default; }));

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
        public void ToObservable_DisposesEnumeratorWhenSubscriptionIsDisposed()
        {
            using var evt = new ManualResetEvent(false);

            var fail = false;
            var subscription = default(IDisposable);
            var subscriptionAssignedTcs = new TaskCompletionSource<object>();

            var ae = AsyncEnumerable.Create(
                _ => AsyncEnumerator.Create(
                    async () =>
                    {
                        await subscriptionAssignedTcs.Task;
                        return true;
                    },
                    () => 1,
                    () =>
                    {
                        evt.Set();
                        return default;
                    }));

            subscription = ae
                .ToObservable()
                .Subscribe(new MyObserver<int>(
                    x =>
                    {
                        Assert.NotNull(subscription);
                        subscription!.Dispose();
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
        public void ToObservable_DesNotCallMoveNextAgainWhenSubscriptionIsDisposed()
        {
            using var evt = new ManualResetEvent(false);

            var fail = false;
            var moveNextCount = 0;
            var subscription = default(IDisposable);
            var subscriptionAssignedTcs = new TaskCompletionSource<object>();

            var ae = AsyncEnumerable.Create(
                _ => AsyncEnumerator.Create(
                    async () =>
                    {
                        await subscriptionAssignedTcs.Task;

                        moveNextCount++;
                        return true;
                    },
                    () => 1,
                    () =>
                    {
                        evt.Set();
                        return default;
                    }));

            subscription = ae
                .ToObservable()
                .Subscribe(new MyObserver<int>(
                    x =>
                    {
                        Assert.NotNull(subscription);
                        subscription!.Dispose();
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
        
        [Fact]
        public void ToObservable_SupportsLargeEnumerable()
        {
            using var evt = new ManualResetEvent(false);

            var fail = false;

            var xs = AsyncEnumerable.Range(0, 10000).ToObservable();
            xs.Subscribe(new MyObserver<int>(
                x =>
                {
                    // ok
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

        private sealed class MyObserver<T> : IObserver<T>
        {
            private readonly Action<T> _onNext;
            private readonly Action<Exception> _onError;
            private readonly Action _onCompleted;

            public MyObserver(Action<T> onNext, Action<Exception> onError, Action onCompleted)
            {
                _onNext = onNext;
                _onError = onError;
                _onCompleted = onCompleted;
            }

            public void OnCompleted() => _onCompleted();

            public void OnError(Exception error) => _onError(error);

            public void OnNext(T value) => _onNext(value);
        }

        private sealed class ThrowOnCurrentAsyncEnumerator : IAsyncEnumerator<int>
        {
            readonly private Exception _exception;
            public ThrowOnCurrentAsyncEnumerator(Exception ex)
            {
                _exception = ex;
            }

            public int Current => throw _exception;
            public ValueTask DisposeAsync() => default;
            public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(true);
        }
    }
}
