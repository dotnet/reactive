// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Tests
{
    public partial class AsyncTests
    {
        public AsyncTests()
        {
            TaskScheduler.UnobservedTaskException += (o, e) =>
            {
            };
        }

        /*
        [Fact]
        public void TestPushPopAsync()
        {
            var stack = new Stack<int>();
            var count = 10;

            var observable = Observable.Generate(
              0,
              i => i < count,
              i => i + 1,
              i => i,
              i => TimeSpan.FromMilliseconds(1), // change this to 0 to avoid the problem [1]
              Scheduler.ThreadPool);

            var task = DoSomethingAsync(observable, stack);

            // we give it a timeout so the test can fail instead of hang
            task.Wait(TimeSpan.FromSeconds(2));

            Assert.Equal(10, stack.Count);
        }

        private Task DoSomethingAsync(IObservable<int> observable, Stack<int> stack)
        {
            var ae = observable
              .ToAsyncEnumerable()
                //.Do(i => Debug.WriteLine("Bug-fixing side effect: " + i))   // [2]
              .GetEnumerator();

            var tcs = new TaskCompletionSource<object>();

            var a = default(Action);
            a = new Action(() =>
            {
                ae.MoveNext().ContinueWith(t =>
                {
                    if (t.Result)
                    {
                        var i = ae.Current;
                        Debug.WriteLine("Doing something with " + i);
                        Thread.Sleep(50);
                        stack.Push(i);
                        a();
                    }
                    else
                        tcs.TrySetResult(null);
                });
            });

            a();

            return tcs.Task;
        }
        */
#if !NO_THREAD
        static IEnumerable<int> Xs(Action a)
        {
            try
            {
                var rnd = new Random();

                while (true)
                {
                    yield return rnd.Next(0, 43);
                    Thread.Sleep(rnd.Next(0, 500));
                }
            }
            finally
            {
                a();
            }
        }
#endif

        [Fact]
        public async void CorrectDispose()
        {
            var disposed = new TaskCompletionSource<bool>();

            var xs = new[] { 1, 2, 3 }.WithDispose(() =>
            {
                disposed.TrySetResult(true);
            }).ToAsyncEnumerable();

            var ys = xs.Select(x => x + 1);

            var e = ys.GetAsyncEnumerator();

            // We have to call move next because otherwise the internal enumerator is never allocated
            await e.MoveNextAsync();
            await e.DisposeAsync();

            await disposed.Task;

            Assert.True(disposed.Task.Result);

            Assert.False(e.MoveNextAsync().Result);

            var next = await e.MoveNextAsync();
            Assert.False(next);
        }

        [Fact]
        public async Task DisposesUponError()
        {
            var disposed = new TaskCompletionSource<bool>();

            var xs = new[] { 1, 2, 3 }.WithDispose(() =>
            {
                disposed.SetResult(true);
            }).ToAsyncEnumerable();

            var ex = new Exception("Bang!");
            var ys = xs.Select(x => { if (x == 1) throw ex; return x; });

            var e = ys.GetAsyncEnumerator();
            await Assert.ThrowsAsync<Exception>(() => e.MoveNextAsync());

            var result = await disposed.Task;
            Assert.True(result);
        }

        static IEnumerable<int> Blocking(ManualResetEvent evt, ManualResetEvent blockingStarted)
        {
            blockingStarted.Set();
            evt.WaitOne();
            yield return 42;
        }

        [Fact]
        public async Task TakeOneFromSelectMany()
        {
            var enumerable = AsyncEnumerable
                .Return(0)
                .SelectMany(_ => AsyncEnumerable.Return("Check"))
                .Take(1)
                .Do(_ => { });

            Assert.Equal("Check", await enumerable.First());
        }

        [Fact]
        public void SelectManyDisposeInvokedOnlyOnce()
        {
            var disposeCounter = new DisposeCounter();

            var result = AsyncEnumerable.Return(1).SelectMany(i => disposeCounter).Select(i => i).ToList().Result;

            Assert.Equal(0, result.Count);
            Assert.Equal(1, disposeCounter.DisposeCount);
        }

        [Fact]
        public void SelectManyInnerDispose()
        {
            var disposes = Enumerable.Range(0, 10).Select(_ => new DisposeCounter()).ToList();

            var result = AsyncEnumerable.Range(0, 10).SelectMany(i => disposes[i]).Select(i => i).ToList().Result;

            Assert.Equal(0, result.Count);
            Assert.True(disposes.All(d => d.DisposeCount == 1));
        }

        [Fact]
        public void DisposeAfterCreation()
        {
            var enumerable = AsyncEnumerable.Return(0) as IDisposable;
            enumerable?.Dispose();
        }

        private class DisposeCounter : IAsyncEnumerable<object>
        {
            public int DisposeCount { get; private set; }

            public IAsyncEnumerator<object> GetAsyncEnumerator()
            {
                return new Enumerator(this);
            }

            private class Enumerator : IAsyncEnumerator<object>
            {
                private readonly DisposeCounter _disposeCounter;

                public Enumerator(DisposeCounter disposeCounter)
                {
                    _disposeCounter = disposeCounter;
                }

                public Task DisposeAsync()
                {
                    _disposeCounter.DisposeCount++;
                    return Task.FromResult(true);
                }

                public Task<bool> MoveNextAsync()
                {
                    return Task.Factory.StartNew(() => false);
                }

                public object Current { get; private set; }
            }
        }
    }

    static class MyExt
    {
        public static IEnumerable<T> WithDispose<T>(this IEnumerable<T> source, Action a)
        {
            return EnumerableEx.Create(() =>
            {
                var e = source.GetEnumerator();
                return new Enumerator<T>(e.MoveNext, () => e.Current, () => { e.Dispose(); a(); });
            });
        }

        public static IAsyncEnumerable<T> WithDispose<T>(this IAsyncEnumerable<T> source, Action a)
        {
            return AsyncEnumerable.CreateEnumerable<T>(() =>
            {
                var e = source.GetAsyncEnumerator();
                return AsyncEnumerable.CreateEnumerator<T>(e.MoveNextAsync, () => e.Current, async () => { await e.DisposeAsync(); a(); });
            });
        }

        class Enumerator<T> : IEnumerator<T>
        {
            private readonly Func<bool> _moveNext;
            private readonly Func<T> _current;
            private readonly Action _dispose;

            public Enumerator(Func<bool> moveNext, Func<T> current, Action dispose)
            {
                _moveNext = moveNext;
                _current = current;
                _dispose = dispose;
            }

            public T Current
            {
                get { return _current(); }
            }

            public void Dispose()
            {
                _dispose();
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                return _moveNext();
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }
        }

    }
}