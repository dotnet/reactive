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

            var e = ys.GetEnumerator();

            // We have to call move next because otherwise the internal enumerator is never allocated
            await e.MoveNext();
            e.Dispose();

            await disposed.Task;

            Assert.True(disposed.Task.Result);

            Assert.False(e.MoveNext().Result);

            var next = await e.MoveNext();
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

            var e = ys.GetEnumerator();
            await Assert.ThrowsAsync<Exception>(() => e.MoveNext());

            var result = await disposed.Task;
            Assert.True(result);
        }

        [Fact]
        public async Task CorrectCancel()
        {
            var disposed = new TaskCompletionSource<bool>();

            var xs = new CancellationTestAsyncEnumerable().WithDispose(() =>
            {
                disposed.TrySetResult(true);
            });

            var ys = xs.Select(x => x + 1).Where(x => true);

            var e = ys.GetEnumerator();
            var cts = new CancellationTokenSource();
            var t = e.MoveNext(cts.Token);

            cts.Cancel();

            try
            {
                t.Wait(WaitTimeoutMs);
            }
            catch
            {
                // Don't care about the outcome; we could have made it to element 1
                // but we could also have cancelled the MoveNext-calling task. Either
                // way, we want to wait for the task to be completed and check that
            }
            finally
            {
                // the cancellation bubbled all the way up to the source to dispose
                // it. This design is chosen because cancelling a MoveNext call leaves
                // the enumerator in an indeterminate state. Further interactions with
                // it should be forbidden.

                var result = await disposed.Task;
                Assert.True(result);
            }

            Assert.False(await e.MoveNext());
        }

        [Fact]
        public void CanCancelMoveNext()
        {
            var xs = new CancellationTestAsyncEnumerable().Select(x => x).Where(x => true);

            var e = xs.GetEnumerator();
            var cts = new CancellationTokenSource();
            var t = e.MoveNext(cts.Token);

            cts.Cancel();

            try
            {
                t.Wait(WaitTimeoutMs);
                Assert.True(false);
            }
            catch
            {
                Assert.True(t.IsCanceled);
            }
        }

        /// <summary>
        /// Waits WaitTimeoutMs or until cancellation is requested. If cancellation was not requested, MoveNext returns true.
        /// </summary>
        internal sealed class CancellationTestAsyncEnumerable : IAsyncEnumerable<int>
        {
            private readonly int iterationsBeforeDelay;

            public CancellationTestAsyncEnumerable(int iterationsBeforeDelay = 0)
            {
                this.iterationsBeforeDelay = iterationsBeforeDelay;
            }
            IAsyncEnumerator<int> IAsyncEnumerable<int>.GetEnumerator() => GetEnumerator();

            public TestEnumerator GetEnumerator() => new TestEnumerator(iterationsBeforeDelay);


            internal sealed class TestEnumerator : IAsyncEnumerator<int>
            {
                private readonly int iterationsBeforeDelay;

                public TestEnumerator(int iterationsBeforeDelay)
                {
                    this.iterationsBeforeDelay = iterationsBeforeDelay;
                }
                int i = -1;
                public void Dispose()
                {
                }

                public CancellationToken LastToken { get; private set; }
                public bool MoveNextWasCalled { get; private set; }

                public int Current => i;
                
                public async Task<bool> MoveNext(CancellationToken cancellationToken)
                {
                    LastToken = cancellationToken;
                    MoveNextWasCalled = true;
                  
                    i++;
                    if (Current >= iterationsBeforeDelay)
                    {
                        await Task.Delay(WaitTimeoutMs, cancellationToken);
                    }
                    cancellationToken.ThrowIfCancellationRequested();
                    return true;
                }
            }
        }

        /// <summary>
        /// Waits WaitTimeoutMs or until cancellation is requested. If cancellation was not requested, MoveNext returns true.
        /// </summary>
        private sealed class CancellationTestEnumerable<T> : IEnumerable<T>
        {
            public CancellationTestEnumerable()
            {
            }
            public IEnumerator<T> GetEnumerator() => new TestEnumerator();

            private sealed class TestEnumerator : IEnumerator<T>
            {
                private readonly CancellationTokenSource cancellationTokenSource;

                public TestEnumerator()
                {
                    cancellationTokenSource = new CancellationTokenSource();
                }
                public void Dispose()
                {
                    cancellationTokenSource.Cancel();
                }

                public void Reset()
                {
                  
                }

                object IEnumerator.Current => Current;

                public T Current { get; }

                public bool MoveNext()
                {
                    Task.Delay(WaitTimeoutMs, cancellationTokenSource.Token).Wait();
                    cancellationTokenSource.Token.ThrowIfCancellationRequested();
                    return true;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        [Fact]
        public void ToAsyncEnumeratorCannotCancelOnceRunning()
        {
            var evt = new ManualResetEvent(false);
            var isRunningEvent = new ManualResetEvent(false);
            var xs = Blocking(evt, isRunningEvent).ToAsyncEnumerable();

            var e = xs.GetEnumerator();
            var cts = new CancellationTokenSource();


            Task<bool> t = null;
            var tMoveNext =Task.Run(
                () =>
                {
                    // This call *will* block
                    t = e.MoveNext(cts.Token);
                });
         

            isRunningEvent.WaitOne();
            cts.Cancel();

            try
            {
                tMoveNext.Wait(0);
                Assert.False(t.IsCanceled);
            }
            catch
            {
                // T will still be null
                Assert.Null(t);
            }


            // enable it to finish
            evt.Set();
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

            public IAsyncEnumerator<object> GetEnumerator()
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

                public void Dispose()
                {
                    _disposeCounter.DisposeCount++;
                }

                public Task<bool> MoveNext(CancellationToken _)
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
                var e = source.GetEnumerator();
                return AsyncEnumerable.CreateEnumerator<T>(e.MoveNext, () => e.Current, () => { e.Dispose(); a(); });
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