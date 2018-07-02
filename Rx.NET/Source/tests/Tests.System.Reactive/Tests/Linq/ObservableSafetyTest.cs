// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{

    public partial class ObservableSafetyTest : ReactiveTest
    {
        [Fact]
        public void SubscribeSafe_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.SubscribeSafe(default, Observer.Create<int>(_ => { })));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.SubscribeSafe(Observable.Return(42), default));
        }

        [Fact]
        public void Safety_Subscription1()
        {
            var ex = new Exception();

            var xs = new RogueObservable(ex);
            var res = xs.Where(x => true).Select(x => x);

            var err = default(Exception);
            var d = res.Subscribe(x => { Assert.True(false); }, ex_ => { err = ex_; }, () => { Assert.True(false); });

            Assert.Same(ex, err);

            d.Dispose();
        }

        [Fact]
        public void Safety_Subscription2()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 42),
                OnNext(220, 43),
                OnNext(230, 44),
                OnNext(240, 45),
                OnCompleted<int>(250)
            );

            var ys = new RogueObservable(ex);

            var res = scheduler.Start(() =>
                xs.Merge(ys)
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 200)
            );
        }

        private class RogueObservable : IObservable<int>
        {
            private readonly Exception _ex;

            public RogueObservable(Exception ex)
            {
                _ex = ex;
            }

            public IDisposable Subscribe(IObserver<int> observer)
            {
                throw _ex;
            }
        }

        [Fact]
        public void ObservableBase_ObserverThrows()
        {
            var ex = new Exception();

            var failed = new ManualResetEvent(false);
            var disposed = new ManualResetEvent(false);
            var err = default(Exception);

            var xs = Observable.Create<int>(observer =>
            {
                Scheduler.Default.Schedule(() =>
                {
                    try
                    {
                        observer.OnNext(42);
                    }
                    catch (Exception ex_)
                    {
                        err = ex_;
                        failed.Set();
                    }
                });

                return () => { disposed.Set(); };
            });

            xs.Subscribe(x =>
            {
                throw ex;
            });

            // Can't use WaitAll - we're on an STA thread.
            disposed.WaitOne();
            failed.WaitOne();

            Assert.Same(ex, err);
        }

        [Fact]
        public void ObservableBase_ObserverThrows_CustomObserver()
        {
            var ex = new Exception();

            var failed = new ManualResetEvent(false);
            var disposed = new ManualResetEvent(false);
            var err = default(Exception);

            var xs = Observable.Create<int>(observer =>
            {
                Scheduler.Default.Schedule(() =>
                {
                    try
                    {
                        observer.OnNext(42);
                    }
                    catch (Exception ex_)
                    {
                        err = ex_;
                        failed.Set();
                    }
                });

                return () => { disposed.Set(); };
            });

            xs.Subscribe(new MyObserver(_ => true, ex));

            // Can't use WaitAll - we're on an STA thread.
            disposed.WaitOne();
            failed.WaitOne();

            Assert.Same(ex, err);
        }

        [Fact]
        public void Producer_ObserverThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, 1),
                OnNext(225, 2),
                OnNext(235, 3)
            );

            var res = xs.CombineLatest(ys, (x, y) => x + y); // This creates a Producer object

            scheduler.ScheduleAbsolute(200, () =>
            {
                res.Subscribe(z =>
                {
                    if (z == 4)
                    {
                        throw ex;
                    }
                });
            });

            try
            {
                scheduler.Start();
                Assert.True(false);
            }
            catch (Exception err)
            {
                Assert.Same(ex, err);
            }

            Assert.Equal(225, scheduler.Clock);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [Fact]
        public void Producer_ObserverThrows_CustomObserver()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, 1),
                OnNext(225, 2),
                OnNext(235, 3)
            );

            var res = xs.CombineLatest(ys, (x, y) => x + y); // This creates a Producer object

            scheduler.ScheduleAbsolute(200, () =>
            {
                res.Subscribe(new MyObserver(x => x == 4, ex));
            });

            try
            {
                scheduler.Start();
                Assert.True(false);
            }
            catch (Exception err)
            {
                Assert.Same(ex, err);
            }

            Assert.Equal(225, scheduler.Clock);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        private class MyObserver : ObserverBase<int>
        {
            private readonly Func<int, bool> _predicate;
            private readonly Exception _exception;

            public MyObserver(Func<int, bool> predicate, Exception exception)
            {
                _predicate = predicate;
                _exception = exception;
            }

            protected override void OnNextCore(int value)
            {
                if (_predicate(value))
                {
                    throw _exception;
                }
            }

            protected override void OnErrorCore(Exception error)
            {
            }

            protected override void OnCompletedCore()
            {
            }
        }

    }
}
