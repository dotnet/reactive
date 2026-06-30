// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ToObservable1(bool ignoreExceptionsAfterUnsubscribe)
        {
            using var evt = new ManualResetEvent(false);

            var fail = false;

            var xs = AsyncEnumerable.Empty<int>().ToObservable(ignoreExceptionsAfterUnsubscribe);
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ToObservable2(bool ignoreExceptionsAfterUnsubscribe)
        {
            using var evt = new ManualResetEvent(false);

            var lst = new List<int>();
            var fail = false;

            var xs = Return42.ToObservable(ignoreExceptionsAfterUnsubscribe);
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
            Assert.True(lst.SequenceEqual([42]));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ToObservable3(bool ignoreExceptionsAfterUnsubscribe)
        {
            using var evt = new ManualResetEvent(false);

            var lst = new List<int>();
            var fail = false;

            var xs = AsyncEnumerable.Range(0, 10).ToObservable(ignoreExceptionsAfterUnsubscribe);
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ToObservable_ThrowOnMoveNext(bool ignoreExceptionsAfterUnsubscribe)
        {
            using var evt = new ManualResetEvent(false);

            var ex1 = new Exception("Bang!");
            var ex_ = default(Exception);
            var fail = false;

            var xs = Throw<int>(ex1).ToObservable(ignoreExceptionsAfterUnsubscribe);
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ToObservable_ThrowOnCurrent(bool ignoreExceptionsAfterUnsubscribe)
        {
            var ex1 = new Exception("Bang!");
            var ex_ = default(Exception);
            var fail = false;

            var ae = AsyncEnumerable.Create(
                _ => new ThrowOnCurrentAsyncEnumerator(ex1)
            );

            ae.ToObservable(ignoreExceptionsAfterUnsubscribe)
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ToObservable_DisposesEnumeratorOnCompletion(bool ignoreExceptionsAfterUnsubscribe)
        {
            using var evt = new ManualResetEvent(false);

            var fail = false;

            var ae = AsyncEnumerable.Create(
                _ => AsyncEnumerator.Create<int>(
                    () => new ValueTask<bool>(false),
                    () => { throw new InvalidOperationException(); },
                    () => { evt.Set(); return default; }));

            ae
                .ToObservable(ignoreExceptionsAfterUnsubscribe)
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ToObservable_DisposesEnumeratorWhenSubscriptionIsDisposed(bool ignoreExceptionsAfterUnsubscribe)
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
                .ToObservable(ignoreExceptionsAfterUnsubscribe)
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ToObservable_DesNotCallMoveNextAgainWhenSubscriptionIsDisposed(bool ignoreExceptionsAfterUnsubscribe)
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
                .ToObservable(ignoreExceptionsAfterUnsubscribe)
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
        
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ToObservable_SupportsLargeEnumerable(bool ignoreExceptionsAfterUnsubscribe)
        {
            using var evt = new ManualResetEvent(false);

            var fail = false;

            var xs = AsyncEnumerable.Range(0, 10000).ToObservable(ignoreExceptionsAfterUnsubscribe);
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ToObservable_ShouldNotCrashOnEnumeratorDispose(bool ignoreExceptionsAfterUnsubscribe)
        {
            var exception = new Exception("Exception message");
            Exception? received = null;
            var enumerable = AsyncEnumerable.Create<int>(_ => throw exception);
            using var evt = new ManualResetEvent(false);

            var observable = enumerable.ToObservable(ignoreExceptionsAfterUnsubscribe);
            observable.Subscribe(new MyObserver<int>(_ =>
                                                     {
                                                         evt.Set();
                                                     },
                                                     e =>
                                                     {
                                                         received = e;
                                                         evt.Set();
                                                     }, () =>
                                                     {
                                                         evt.Set();
                                                     }));

            evt.WaitOne();
            Assert.NotNull(received);
            Assert.Equal(exception.Message, received!.Message);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ToObservable_ShouldForwardExceptionOnGetEnumeratorAsync(bool ignoreExceptionsAfterUnsubscribe)
        {
            var exception = new Exception("Exception message");
            Exception? recievedException = null;
            var enumerable = AsyncEnumerable.Create<int>(_ => throw exception);
            using var evt = new ManualResetEvent(false);

            var observable = enumerable.ToObservable(ignoreExceptionsAfterUnsubscribe);
            observable.Subscribe(new MyObserver<int>(_ =>
                                                     {
                                                         evt.Set();
                                                     },
                                                     e =>
                                                     {
                                                         recievedException = e;
                                                         evt.Set();
                                                     }, () =>
                                                     {
                                                         evt.Set();
                                                     }));

            evt.WaitOne();
            Assert.NotNull(recievedException);
            Assert.Equal(exception.Message, recievedException!.Message);
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
            private readonly Exception _exception;
            public ThrowOnCurrentAsyncEnumerator(Exception ex)
            {
                _exception = ex;
            }

            public int Current => throw _exception;
            public ValueTask DisposeAsync() => default;
            public ValueTask<bool> MoveNextAsync() => new(true);
        }
    }
}
