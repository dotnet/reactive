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
    public class ToAsyncEnumerable : AsyncEnumerableTests
    {
        [Fact]
        public void ToAsyncEnumerable_Enumerable_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.ToAsyncEnumerable(default(IEnumerable<int>)));
        }

        [Fact]
        public async Task ToAsyncEnumerable_Enumerable_Array()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task ToAsyncEnumerable_Enumerable_Array_ToArray()
        {
            var arr = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            var xs = arr.ToAsyncEnumerable();

            var res = await xs.ToArrayAsync();

            Assert.Equal(arr, res);
        }

        [Fact]
        public async Task ToAsyncEnumerable_Enumerable_Array_ToList()
        {
            var arr = new[] { 1, 2, 3, 4 };
            var xs = arr.ToAsyncEnumerable();

            var res = await xs.ToListAsync();

            Assert.Equal(arr, res);
        }

        [Fact]
        public async Task ToAsyncEnumerable_Enumerable_Array_Count()
        {
            var arr = new[] { 1, 2, 3, 4 };
            var xs = arr.ToAsyncEnumerable();

            var c = await xs.CountAsync();

            Assert.Equal(arr.Length, c);
        }

        [Fact]
        public async Task ToAsyncEnumerable_Enumerable_Array_SequenceIdentity()
        {
            var arr = new[] { 1, 2, 3, 4 };
            var xs = arr.ToAsyncEnumerable();

            await SequenceIdentity(xs);
        }

        [Fact]
        public async Task ToAsyncEnumerable_Enumerable_Iterator()
        {
            var xs = ToAsyncEnumerable_Sequence().ToAsyncEnumerable();
            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        private IEnumerable<int> ToAsyncEnumerable_Sequence()
        {
            yield return 1;
            yield return 2;
            yield return 3;
            yield return 4;
        }

        [Fact]
        public async Task ToAsyncEnumerable_Enumerable_Iterator_Throw()
        {
            var ex = new Exception("Bang");
            var xs = ToAsyncEnumerable_Sequence_Throw(ex).ToAsyncEnumerable();
            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 42);
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        private IEnumerable<int> ToAsyncEnumerable_Sequence_Throw(Exception e)
        {
            yield return 42;
            throw e;
        }

        [Fact]
        public async Task ToAsyncEnumerable_Enumerable_HashSet()
        {
            var set = new HashSet<int>(new[] { 1, 2, 3, 4 });

            var xs = set.ToAsyncEnumerable();
            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task ToAsyncEnumerable_Enumerable_HashSet_ToArray()
        {
            var set = new HashSet<int>(new[] { 1, 2, 3, 4, 5, 6, 7, 8 });

            var xs = set.ToAsyncEnumerable();

            var arr = await xs.ToArrayAsync();

            Assert.True(set.SetEquals(arr));
        }

        [Fact]
        public async Task ToAsyncEnumerable_Enumerable_HashSet_ToList()
        {
            var set = new HashSet<int>(new[] { 1, 2, 3, 4 });
            var xs = set.ToAsyncEnumerable();

            var arr = await xs.ToListAsync();

            Assert.True(set.SetEquals(arr));
        }

        [Fact]
        public async Task ToAsyncEnumerable_Enumerable_HashSet_Count()
        {
            var set = new HashSet<int>(new[] { 1, 2, 3, 4 });
            var xs = set.ToAsyncEnumerable();

            var c = await xs.CountAsync();

            Assert.Equal(set.Count, c);
        }

        [Fact]
        public async Task ToAsyncEnumerable_Enumerable_HashSet_SequenceIdentity()
        {
            var set = new HashSet<int>(new[] { 1, 2, 3, 4 });
            var xs = set.ToAsyncEnumerable();

            await SequenceIdentity(xs);
        }

        [Fact]
        public void ToAsyncEnumerable_Enumerable_HashSet_ICollection()
        {
            var set = new HashSet<int>(new[] { 1, 2, 3, 4 });
            var xs = set.ToAsyncEnumerable();

            var xc = xs as ICollection<int>;

            Assert.NotNull(xc);

            Assert.False(xc!.IsReadOnly);

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
        public void ToAsyncEnumerable_Enumerable_List_IList()
        {
            var set = new List<int> { 1, 2, 3, 4 };
            var xs = set.ToAsyncEnumerable();

            var xl = xs as IList<int>;

            Assert.NotNull(xl);

            Assert.False(xl!.IsReadOnly);

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
        public void ToAsyncEnumerable_Observable_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.ToAsyncEnumerable(default(IObservable<int>)));
        }

        [Fact]
        public async Task ToAsyncEnumerable_Observable_Return()
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

            // NB: Breaking change to align with lazy nature of async iterators.
            // Assert.True(subscribed);

            await HasNextAsync(e, 42);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task ToAsyncEnumerable_Observable_Throw()
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

            // NB: Breaking change to align with lazy nature of async iterators.
            // Assert.True(subscribed);

            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task ToAsyncEnumerable_Observable_Dispose()
        {
            using var stop = new ManualResetEvent(false);

            var xs = new MyObservable<int>(obs =>
            {
                var cts = new CancellationTokenSource();

                Task.Run(async () =>
                {
                    for (var i = 0; !cts.IsCancellationRequested; i++)
                    {
                        await Task.Delay(10);
                        obs.OnNext(i);
                    }

                    stop.Set();
                });

                return new MyDisposable(cts.Cancel);
            }).ToAsyncEnumerable();

            var e = xs.GetAsyncEnumerator();

            for (var i = 0; i < 10; i++)
            {
                await HasNextAsync(e, i);
            }

            await e.DisposeAsync();
            stop.WaitOne();
        }

        [Fact]
        public async Task ToAsyncEnumerable_Observable_Zip()
        {
            using var stop = new ManualResetEvent(false);

            var subCount = 0;

            var xs = new MyObservable<int>(obs =>
            {
                subCount++;

                var cts = new CancellationTokenSource();

                Task.Run(async () =>
                {
                    for (var i = 0; !cts.IsCancellationRequested; i++)
                    {
                        await Task.Delay(10);
                        obs.OnNext(i);
                    }

                    stop.Set();
                });

                return new MyDisposable(cts.Cancel);
            }).ToAsyncEnumerable();

            var e = xs.Zip(xs, (l, r) => l == r).GetAsyncEnumerator();

            for (var i = 0; i < 10; i++)
            {
                await HasNextAsync(e, true);
            }

            await e.DisposeAsync();
            stop.WaitOne();

            Assert.Equal(2, subCount);
        }

        [Fact]
        public async Task ToAsyncEnumerable_Observable_Cancel()
        {
            using var stop = new ManualResetEvent(false);

            var xs = new MyObservable<int>(obs =>
            {
                var cts = new CancellationTokenSource();

                Task.Run(async () =>
                {
                    for (var i = 0; !cts.IsCancellationRequested; i++)
                    {
                        await Task.Delay(10);
                        obs.OnNext(i);
                    }

                    stop.Set();
                });

                return new MyDisposable(cts.Cancel);
            }).ToAsyncEnumerable();

            using var c = new CancellationTokenSource();

            var e = xs.GetAsyncEnumerator(c.Token);

            for (var i = 0; i < 10; i++)
            {
                await HasNextAsync(e, i);
            }

            c.Cancel();
            stop.WaitOne();
        }

        [Fact]
        public async Task ToAsyncEnumerable_Observable6_Async()
        {
            using var stop = new ManualResetEvent(false);

            var xs = new MyObservable<int>(obs =>
            {
                var cts = new CancellationTokenSource();

                Task.Run(async () =>
                {
                    for (var i = 0; !cts.IsCancellationRequested; i++)
                    {
                        await Task.Yield();
                        obs.OnNext(i);
                    }

                    stop.Set();
                });

                return new MyDisposable(cts.Cancel);
            }).ToAsyncEnumerable();

            var e = xs.GetAsyncEnumerator();

            for (var i = 0; i < 10_000; i++)
            {
                await HasNextAsync(e, i);
            }

            await e.DisposeAsync();
            stop.WaitOne();
        }

        // TODO: Add more tests for Observable conversion.

        [Fact]
        public void ToAsyncEnumerable_Task_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.ToAsyncEnumerable(default(Task<int>)));
        }

        [Fact]
        public async Task ToAsyncEnumerable_Task_With_Completed_TaskAsync()
        {
            var task = Task.Factory.StartNew(() => 36);

            var xs = task.ToAsyncEnumerable();
            var e = xs.GetAsyncEnumerator();

            Assert.True(await e.MoveNextAsync());
            Assert.Equal(36, e.Current);
            Assert.False(await e.MoveNextAsync());
        }

        [Fact]
        public async Task ToAsyncEnumerable_Task_With_Faulted_TaskAsync()
        {
            var ex = new InvalidOperationException();
            var tcs = new TaskCompletionSource<int>();
            tcs.SetException(ex);

            var xs = tcs.Task.ToAsyncEnumerable();
            var e = xs.GetAsyncEnumerator();

            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task ToAsyncEnumerable_Task_With_Canceled_TaskAsync()
        {
            var tcs = new TaskCompletionSource<int>();
            tcs.SetCanceled();

            var xs = tcs.Task.ToAsyncEnumerable();
            var e = xs.GetAsyncEnumerator();

            await AssertThrowsAsync<TaskCanceledException>(e.MoveNextAsync().AsTask());
        }

        private sealed class MyObservable<T> : IObservable<T>
        {
            private readonly Func<IObserver<T>, IDisposable> _subscribe;

            public MyObservable(Func<IObserver<T>, IDisposable> subscribe) => _subscribe = subscribe;

            public IDisposable Subscribe(IObserver<T> observer) => _subscribe(observer);
        }

        private sealed class MyDisposable : IDisposable
        {
            private readonly Action _dispose;

            public MyDisposable(Action dispose) => _dispose = dispose;

            public void Dispose() => _dispose();
        }
    }
}
