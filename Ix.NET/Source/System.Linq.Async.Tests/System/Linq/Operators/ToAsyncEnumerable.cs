// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class ToAsyncEnumerable : AsyncEnumerableTests
    {
        [Fact]
        public void ToAsyncEnumerable_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToAsyncEnumerable(default(IEnumerable<int>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToAsyncEnumerable(default(IObservable<int>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToAsyncEnumerable(default(Task<int>)));
        }

        [Fact]
        public void ToAsyncEnumerable1()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var e = xs.GetAsyncEnumerator();
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
            var e = xs.GetAsyncEnumerator();
            HasNext(e, 42);
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).InnerExceptions.Single() == ex);
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

            var e = xs.GetAsyncEnumerator();

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

            var e = xs.GetAsyncEnumerator();

            Assert.True(subscribed);

            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).InnerExceptions.Single() == ex);
        }

        [Fact]
        public void ToAsyncEnumerable5()
        {
            var set = new HashSet<int>(new[] { 1, 2, 3, 4 });

            var xs = set.ToAsyncEnumerable();
            var e = xs.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public async Task ToAsyncEnumerable6()
        {
            var set = new HashSet<int>(new[] { 1, 2, 3, 4, 5, 6, 7, 8 });

            var xs = set.ToAsyncEnumerable();

            var arr = await xs.ToArray();

            Assert.True(set.SetEquals(arr));
        }

        [Fact]
        public async Task ToAsyncEnumerable7()
        {
            var set = new HashSet<int>(new[] { 1, 2, 3, 4 });
            var xs = set.ToAsyncEnumerable();

            var arr = await xs.ToList();

            Assert.True(set.SetEquals(arr));
        }

        [Fact]
        public async Task ToAsyncEnumerable8()
        {
            var set = new HashSet<int>(new[] { 1, 2, 3, 4 });
            var xs = set.ToAsyncEnumerable();

            var c = await xs.Count();

            Assert.Equal(set.Count, c);
        }

        [Fact]
        public async Task ToAsyncEnumerable9()
        {
            var set = new HashSet<int>(new[] { 1, 2, 3, 4 });
            var xs = set.ToAsyncEnumerable();

            await SequenceIdentity(xs);
        }

        [Fact]
        public async Task ToAsyncEnumerable10()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            await SequenceIdentity(xs);
        }

        [Fact]
        public void ToAsyncEnumerable11()
        {
            var set = new HashSet<int>(new[] { 1, 2, 3, 4 });
            var xs = set.ToAsyncEnumerable();

            var xc = xs as ICollection<int>;

            Assert.NotNull(xc);

            Assert.False(xc.IsReadOnly);

            xc.Add(5);


            Assert.True(xc.Contains(5));

            Assert.True(xc.Remove(5));

            var arr = new int[4];
            xc.CopyTo(arr, 0);
            Assert.True(arr.SequenceEqual(xc));
            xc.Clear();
            Assert.Equal(0, xc.Count);
        }

        [Fact]
        public void ToAsyncEnumerable12()
        {
            var set = new List<int> { 1, 2, 3, 4 };
            var xs = set.ToAsyncEnumerable();

            var xl = xs as IList<int>;

            Assert.NotNull(xl);

            Assert.False(xl.IsReadOnly);

            xl.Add(5);


            Assert.True(xl.Contains(5));

            Assert.True(xl.Remove(5));

            xl.Insert(2, 10);

            Assert.Equal(2, xl.IndexOf(10));
            xl.RemoveAt(2);

            xl[0] = 7;
            Assert.Equal(7, xl[0]);

            var arr = new int[4];
            xl.CopyTo(arr, 0);
            Assert.True(arr.SequenceEqual(xl));
            xl.Clear();
            Assert.Equal(0, xl.Count);

        }

        [Fact]
        public void ToAsyncEnumerable_With_Completed_Task()
        {
            var task = Task.Factory.StartNew(() => 36);

            var xs = task.ToAsyncEnumerable();
            var e = xs.GetAsyncEnumerator();

            Assert.True(e.MoveNextAsync().Result);
            Assert.Equal(36, e.Current);
            Assert.False(e.MoveNextAsync().Result);
        }

        [Fact]
        public void ToAsyncEnumerable_With_Faulted_Task()
        {
            var ex = new InvalidOperationException();
            var tcs = new TaskCompletionSource<int>();
            tcs.SetException(ex);

            var xs = tcs.Task.ToAsyncEnumerable();
            var e = xs.GetAsyncEnumerator();

            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).InnerExceptions.Single() == ex);
        }

        [Fact]
        public void ToAsyncEnumerable_With_Canceled_Task()
        {
            var tcs = new TaskCompletionSource<int>();
            tcs.SetCanceled();

            var xs = tcs.Task.ToAsyncEnumerable();
            var e = xs.GetAsyncEnumerator();

            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).InnerExceptions.Single() is TaskCanceledException);
        }

        private sealed class MyObservable<T> : IObservable<T>
        {
            private readonly Func<IObserver<T>, IDisposable> _subscribe;

            public MyObservable(Func<IObserver<T>, IDisposable> subscribe)
            {
                _subscribe = subscribe;
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                return _subscribe(observer);
            }
        }

        private sealed class MyDisposable : IDisposable
        {
            private readonly Action _dispose;

            public MyDisposable(Action dispose)
            {
                _dispose = dispose;
            }

            public void Dispose()
            {
                _dispose();
            }
        }
    }
}
