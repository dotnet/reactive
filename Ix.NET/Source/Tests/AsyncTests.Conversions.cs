// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
#if !NO_TPL
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace Tests
{
    public partial class AsyncTests
    {
        [TestMethod]
        public void ToAsyncEnumerable_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToAsyncEnumerable<int>(default(IEnumerable<int>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToAsyncEnumerable<int>(default(IObservable<int>)));
        }

        [TestMethod]
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

        [TestMethod]
        public void ToAsyncEnumerable2()
        {
            var ex = new Exception("Bang");
            var xs = ToAsyncEnumerable_Sequence(ex).ToAsyncEnumerable();
            var e = xs.GetEnumerator();
            HasNext(e, 42);
            AssertThrows<Exception>(() => e.MoveNext().Wait(), ex_ => ((AggregateException)ex_).InnerExceptions.Single() == ex);
        }

        private IEnumerable<int> ToAsyncEnumerable_Sequence(Exception e)
        {
            yield return 42;
            throw e;
        }

        [TestMethod]
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

            Assert.IsFalse(subscribed);

            var e = xs.GetEnumerator();

            Assert.IsTrue(subscribed);

            HasNext(e, 42);
            NoNext(e);
        }

        [TestMethod]
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

            Assert.IsFalse(subscribed);

            var e = xs.GetEnumerator();

            Assert.IsTrue(subscribed);

            AssertThrows<Exception>(() => e.MoveNext().Wait(), ex_ => ((AggregateException)ex_).InnerExceptions.Single() == ex);
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

        [TestMethod]
        public void ToEnumerable_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToEnumerable<int>(null));
        }

        [TestMethod]
        public void ToEnumerable1()
        {
            var xs = AsyncEnumerable.Return(42).ToEnumerable();
            Assert.IsTrue(xs.SequenceEqual(new[] { 42 }));
        }

        [TestMethod]
        public void ToEnumerable2()
        {
            var xs = AsyncEnumerable.Empty<int>().ToEnumerable();
            Assert.IsTrue(xs.SequenceEqual(new int[0]));
        }

        [TestMethod]
        public void ToEnumerable3()
        {
            var ex = new Exception("Bang");
            var xs = AsyncEnumerable.Throw<int>(ex).ToEnumerable();
            AssertThrows<Exception>(() => xs.GetEnumerator().MoveNext(), ex_ => ((AggregateException)ex_).InnerExceptions.Single() == ex);
        }

#if !NO_RXINTERFACES
        [TestMethod]
        public void ToObservable_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToObservable<int>(null));
        }

        [TestMethod]
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
            Assert.IsFalse(fail);
        }

        [TestMethod]
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
            Assert.IsFalse(fail);
            Assert.IsTrue(lst.SequenceEqual(new[] { 42 }));
        }

        [TestMethod]
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
            Assert.IsFalse(fail);
            Assert.IsTrue(lst.SequenceEqual(Enumerable.Range(0, 10)));
        }

        [TestMethod]
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
            Assert.IsFalse(fail);
            Assert.AreEqual(ex1, ((AggregateException)ex_).InnerExceptions.Single());
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
#endif
    }
}

#endif