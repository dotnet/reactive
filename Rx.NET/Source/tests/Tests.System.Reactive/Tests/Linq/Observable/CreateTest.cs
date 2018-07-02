// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class CreateTest : ReactiveTest
    {

        [Fact]
        public void Create_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Create(default(Func<IObserver<int>, Action>)));

            //
            // BREAKING CHANGE v2.0 > v1.x - Returning null from Subscribe means "nothing to do upon unsubscription"
            //                               all null-coalesces to Disposable.Empty.
            //
            //ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Create<int>(o => default(Action)).Subscribe(DummyObserver<int>.Instance));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Create<int>(o => () => { }).Subscribe(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Create<int>(o =>
            {
                o.OnError(null);
                return () => { };
            }).Subscribe(null));
        }

        [Fact]
        public void Create_NullCoalescingAction()
        {
            var xs = Observable.Create<int>(o =>
            {
                o.OnNext(42);
                return default(Action);
            });

            var lst = new List<int>();
            var d = xs.Subscribe(lst.Add);
            d.Dispose();

            Assert.True(lst.SequenceEqual(new[] { 42 }));
        }

        [Fact]
        public void Create_Next()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Create<int>(o =>
                {
                    o.OnNext(1);
                    o.OnNext(2);
                    return () => { };
                })
            );

            res.Messages.AssertEqual(
                OnNext(200, 1),
                OnNext(200, 2)
            );
        }

        [Fact]
        public void Create_Completed()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Create<int>(o =>
                {
                    o.OnCompleted();
                    o.OnNext(100);
                    o.OnError(new Exception());
                    o.OnCompleted();
                    return () => { };
                })
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(200)
            );
        }

        [Fact]
        public void Create_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Create<int>(o =>
                {
                    o.OnError(ex);
                    o.OnNext(100);
                    o.OnError(new Exception());
                    o.OnCompleted();
                    return () => { };
                })
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );
        }

        [Fact]
        public void Create_Exception()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() =>
                Observable.Create(new Func<IObserver<int>, Action>(o => { throw new InvalidOperationException(); })).Subscribe());
        }

        [Fact]
        public void Create_Dispose()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Create<int>(o =>
                {
                    var stopped = false;

                    o.OnNext(1);
                    o.OnNext(2);
                    scheduler.Schedule(TimeSpan.FromTicks(600), () =>
                    {
                        if (!stopped)
                        {
                            o.OnNext(3);
                        }
                    });
                    scheduler.Schedule(TimeSpan.FromTicks(700), () =>
                    {
                        if (!stopped)
                        {
                            o.OnNext(4);
                        }
                    });
                    scheduler.Schedule(TimeSpan.FromTicks(900), () =>
                    {
                        if (!stopped)
                        {
                            o.OnNext(5);
                        }
                    });
                    scheduler.Schedule(TimeSpan.FromTicks(1100), () =>
                    {
                        if (!stopped)
                        {
                            o.OnNext(6);
                        }
                    });

                    return () => { stopped = true; };
                })
            );

            res.Messages.AssertEqual(
                OnNext(200, 1),
                OnNext(200, 2),
                OnNext(800, 3),
                OnNext(900, 4)
            );
        }

        [Fact]
        public void Create_ObserverThrows()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() =>
                Observable.Create<int>(o =>
                {
                    o.OnNext(1);
                    return () => { };
                }).Subscribe(x => { throw new InvalidOperationException(); }));
            ReactiveAssert.Throws<InvalidOperationException>(() =>
                Observable.Create<int>(o =>
                {
                    o.OnError(new Exception());
                    return () => { };
                }).Subscribe(x => { }, ex => { throw new InvalidOperationException(); }));
            ReactiveAssert.Throws<InvalidOperationException>(() =>
                Observable.Create<int>(o =>
                {
                    o.OnCompleted();
                    return () => { };
                }).Subscribe(x => { }, ex => { }, () => { throw new InvalidOperationException(); }));
        }

        [Fact]
        public void CreateWithDisposable_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Create(default(Func<IObserver<int>, IDisposable>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Create<int>(o => DummyDisposable.Instance).Subscribe(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Create<int>(o =>
            {
                o.OnError(null);
                return DummyDisposable.Instance;
            }).Subscribe(null));
        }

        [Fact]
        public void CreateWithDisposable_NullCoalescingAction()
        {
            var xs = Observable.Create<int>(o =>
            {
                o.OnNext(42);
                return default(IDisposable);
            });

            var lst = new List<int>();
            var d = xs.Subscribe(lst.Add);
            d.Dispose();

            Assert.True(lst.SequenceEqual(new[] { 42 }));
        }

        [Fact]
        public void CreateWithDisposable_Next()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Create<int>(o =>
                {
                    o.OnNext(1);
                    o.OnNext(2);
                    return Disposable.Empty;
                })
            );

            res.Messages.AssertEqual(
                OnNext(200, 1),
                OnNext(200, 2)
            );
        }

        [Fact]
        public void CreateWithDisposable_Completed()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Create<int>(o =>
                {
                    o.OnCompleted();
                    o.OnNext(100);
                    o.OnError(new Exception());
                    o.OnCompleted();
                    return Disposable.Empty;
                })
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(200)
            );
        }

        [Fact]
        public void CreateWithDisposable_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Create<int>(o =>
                {
                    o.OnError(ex);
                    o.OnNext(100);
                    o.OnError(new Exception());
                    o.OnCompleted();
                    return Disposable.Empty;
                })
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );
        }

        [Fact]
        public void CreateWithDisposable_Exception()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() =>
                Observable.Create(new Func<IObserver<int>, IDisposable>(o => { throw new InvalidOperationException(); })).Subscribe());
        }

        [Fact]
        public void CreateWithDisposable_Dispose()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Create<int>(o =>
                {
                    var d = new BooleanDisposable();

                    o.OnNext(1);
                    o.OnNext(2);
                    scheduler.Schedule(TimeSpan.FromTicks(600), () =>
                    {
                        if (!d.IsDisposed)
                        {
                            o.OnNext(3);
                        }
                    });
                    scheduler.Schedule(TimeSpan.FromTicks(700), () =>
                    {
                        if (!d.IsDisposed)
                        {
                            o.OnNext(4);
                        }
                    });
                    scheduler.Schedule(TimeSpan.FromTicks(900), () =>
                    {
                        if (!d.IsDisposed)
                        {
                            o.OnNext(5);
                        }
                    });
                    scheduler.Schedule(TimeSpan.FromTicks(1100), () =>
                    {
                        if (!d.IsDisposed)
                        {
                            o.OnNext(6);
                        }
                    });

                    return d;
                })
            );

            res.Messages.AssertEqual(
                OnNext(200, 1),
                OnNext(200, 2),
                OnNext(800, 3),
                OnNext(900, 4)
            );
        }

        [Fact]
        public void CreateWithDisposable_ObserverThrows()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() =>
                Observable.Create<int>(o =>
                {
                    o.OnNext(1);
                    return Disposable.Empty;
                }).Subscribe(x => { throw new InvalidOperationException(); }));
            ReactiveAssert.Throws<InvalidOperationException>(() =>
                Observable.Create<int>(o =>
                {
                    o.OnError(new Exception());
                    return Disposable.Empty;
                }).Subscribe(x => { }, ex => { throw new InvalidOperationException(); }));
            ReactiveAssert.Throws<InvalidOperationException>(() =>
                Observable.Create<int>(o =>
                {
                    o.OnCompleted();
                    return Disposable.Empty;
                }).Subscribe(x => { }, ex => { }, () => { throw new InvalidOperationException(); }));
        }

        [Fact]
        public void Iterate_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.Create<int>(default));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.Create(DummyFunc<IObserver<int>, IEnumerable<IObservable<object>>>.Instance).Subscribe(null));
        }

        private IEnumerable<IObservable<object>> ToIterate_Complete(IObservable<int> xs, IObservable<int> ys, IObservable<int> zs, IObserver<int> observer)
        {
            observer.OnNext(1);
            yield return xs.Select(x => new object());

            observer.OnNext(2);
            yield return ys.Select(x => new object());

            observer.OnNext(3);
            observer.OnCompleted();
            yield return zs.Select(x => new object());

            observer.OnNext(4);
        }

        [Fact]
        public void Iterate_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnCompleted<int>(50)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnCompleted<int>(30)
            );

            var zs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnNext(50, 5),
                OnCompleted<int>(60)
            );

            var res = scheduler.Start(() => ObservableEx.Create<int>(observer => ToIterate_Complete(xs, ys, zs, observer)));

            res.Messages.AssertEqual(
                OnNext(200, 1),
                OnNext(250, 2),
                OnNext(280, 3),
                OnCompleted<int>(280)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(250, 280)
            );

            zs.Subscriptions.AssertEqual(
                Subscribe(280, 280)
            );
        }

        private IEnumerable<IObservable<object>> ToIterate_Complete_Implicit(IObservable<int> xs, IObservable<int> ys, IObservable<int> zs, IObserver<int> observer)
        {
            observer.OnNext(1);
            yield return xs.Select(x => new object());

            observer.OnNext(2);
            yield return ys.Select(x => new object());

            observer.OnNext(3);
            yield return zs.Select(x => new object());

            observer.OnNext(4);
        }

        [Fact]
        public void Iterate_Complete_Implicit()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnCompleted<int>(50)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnCompleted<int>(30)
            );

            var zs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnNext(50, 5),
                OnCompleted<int>(60)
            );

            var res = scheduler.Start(() => ObservableEx.Create<int>(observer => ToIterate_Complete_Implicit(xs, ys, zs, observer)));

            res.Messages.AssertEqual(
                OnNext(200, 1),
                OnNext(250, 2),
                OnNext(280, 3),
                OnNext(340, 4),
                OnCompleted<int>(340)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(250, 280)
            );

            zs.Subscriptions.AssertEqual(
                Subscribe(280, 340)
            );
        }

        private IEnumerable<IObservable<object>> ToIterate_Throw(IObservable<int> xs, IObservable<int> ys, IObservable<int> zs, IObserver<int> observer, Exception ex)
        {
            observer.OnNext(1);
            yield return xs.Select(x => new object());

            observer.OnNext(2);
            yield return ys.Select(x => new object());

            observer.OnNext(3);

            if (xs != null)
            {
                throw ex;
            }

            yield return zs.Select(x => new object());

            observer.OnNext(4);
            observer.OnCompleted();
        }

        [Fact]
        public void Iterate_Iterator_Throw()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnCompleted<int>(50)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnCompleted<int>(30)
            );

            var zs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnNext(50, 5),
                OnCompleted<int>(60)
            );

            var ex = new Exception();

            var res = scheduler.Start(() => ObservableEx.Create<int>(observer => ToIterate_Throw(xs, ys, zs, observer, ex)));

            res.Messages.AssertEqual(
                OnNext(200, 1),
                OnNext(250, 2),
                OnNext(280, 3),
                OnError<int>(280, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(250, 280)
            );

            zs.Subscriptions.AssertEqual(
            );
        }

        private IEnumerable<IObservable<object>> ToIterate_Error(IObservable<int> xs, IObservable<int> ys, IObservable<int> zs, IObserver<int> observer, Exception ex)
        {
            observer.OnNext(1);
            yield return xs.Select(x => new object());

            observer.OnNext(2);
            observer.OnError(ex);

            yield return ys.Select(x => new object());

            observer.OnNext(3);

            yield return zs.Select(x => new object());

            observer.OnNext(4);
            observer.OnCompleted();
        }

        [Fact]
        public void Iterate_Iterator_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnCompleted<int>(50)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnCompleted<int>(30)
            );

            var zs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnNext(50, 5),
                OnCompleted<int>(60)
            );

            var res = scheduler.Start(() => ObservableEx.Create<int>(observer => ToIterate_Error(xs, ys, zs, observer, ex)));

            res.Messages.AssertEqual(
                OnNext(200, 1),
                OnNext(250, 2),
                OnError<int>(250, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(250, 250)
            );

            zs.Subscriptions.AssertEqual(
            );
        }

        private IEnumerable<IObservable<object>> ToIterate_Complete_Dispose(IObservable<int> xs, IObservable<int> ys, IObservable<int> zs, IObserver<int> observer)
        {
            observer.OnNext(1);
            yield return xs.Select(x => new object());

            observer.OnNext(2);
            yield return ys.Select(x => new object());

            observer.OnNext(3);
            yield return zs.Select(x => new object());

            observer.OnNext(4);
        }

        [Fact]
        public void Iterate_Complete_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnCompleted<int>(50)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnCompleted<int>(30)
            );

            var zs = scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnNext(500, 5),
                OnNext(600, 6),
                OnNext(700, 7),
                OnNext(800, 8),
                OnNext(900, 9),
                OnNext(1000, 10)
            );

            var res = scheduler.Start(() => ObservableEx.Create<int>(observer => ToIterate_Complete_Dispose(xs, ys, zs, observer)));

            res.Messages.AssertEqual(
                OnNext(200, 1),
                OnNext(250, 2),
                OnNext(280, 3)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(250, 280)
            );

            zs.Subscriptions.AssertEqual(
                Subscribe(280, 1000)
            );
        }

        [Fact]
        public void IteratorScenario()
        {
            var xs = ObservableEx.Create<int>(o => _IteratorScenario(100, 1000, o));

            xs.AssertEqual(new[] { 100, 1000 }.ToObservable());
        }

        private static IEnumerable<IObservable<object>> _IteratorScenario(int x, int y, IObserver<int> results)
        {
            var xs = Observable.Range(1, x).ToListObservable();
            yield return xs;

            results.OnNext(xs.Value);

            var ys = Observable.Range(1, y).ToListObservable();
            yield return ys;

            results.OnNext(ys.Value);
        }

        [Fact]
        public void Iterate_Void_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.Create(default));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.Create(DummyFunc<IEnumerable<IObservable<object>>>.Instance).Subscribe(null));
        }

        private IEnumerable<IObservable<object>> ToIterate_Void_Complete(IObservable<int> xs, IObservable<int> ys, IObservable<int> zs)
        {
            yield return xs.Select(x => new object());

            yield return ys.Select(x => new object());

            yield return zs.Select(x => new object());
        }

        [Fact]
        public void Iterate_Void_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnCompleted<int>(50)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnCompleted<int>(30)
            );

            var zs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnNext(50, 5),
                OnCompleted<int>(60)
            );

            var res = scheduler.Start(() => ObservableEx.Create(() => ToIterate_Void_Complete(xs, ys, zs)));

            res.Messages.AssertEqual(
                OnCompleted<Unit>(340)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(250, 280)
            );

            zs.Subscriptions.AssertEqual(
                Subscribe(280, 340)
            );
        }

        private IEnumerable<IObservable<object>> ToIterate_Void_Complete_Implicit(IObservable<int> xs, IObservable<int> ys, IObservable<int> zs)
        {
            yield return xs.Select(x => new object());

            yield return ys.Select(x => new object());

            yield return zs.Select(x => new object());
        }

        [Fact]
        public void Iterate_Void_Complete_Implicit()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnCompleted<int>(50)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnCompleted<int>(30)
            );

            var zs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnNext(50, 5),
                OnCompleted<int>(60)
            );

            var res = scheduler.Start(() => ObservableEx.Create(() => ToIterate_Void_Complete_Implicit(xs, ys, zs)));

            res.Messages.AssertEqual(
                OnCompleted<Unit>(340)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(250, 280)
            );

            zs.Subscriptions.AssertEqual(
                Subscribe(280, 340)
            );
        }

        private IEnumerable<IObservable<object>> ToIterate_Void_Throw(IObservable<int> xs, IObservable<int> ys, IObservable<int> zs, Exception ex)
        {
            yield return xs.Select(x => new object());

            yield return ys.Select(x => new object());

            if (xs != null)
            {
                throw ex;
            }

            yield return zs.Select(x => new object());
        }

        [Fact]
        public void Iterate_Void_Iterator_Throw()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnCompleted<int>(50)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnCompleted<int>(30)
            );

            var zs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnNext(50, 5),
                OnCompleted<int>(60)
            );

            var ex = new Exception();

            var res = scheduler.Start(() => ObservableEx.Create(() => ToIterate_Void_Throw(xs, ys, zs, ex)));

            res.Messages.AssertEqual(
                OnError<Unit>(280, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(250, 280)
            );

            zs.Subscriptions.AssertEqual(
            );
        }

        private IEnumerable<IObservable<object>> ToIterate_Void_Complete_Dispose(IObservable<int> xs, IObservable<int> ys, IObservable<int> zs)
        {
            yield return xs.Select(x => new object());

            yield return ys.Select(x => new object());

            yield return zs.Select(x => new object());
        }

        [Fact]
        public void Iterate_Void_Complete_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnCompleted<int>(50)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnCompleted<int>(30)
            );

            var zs = scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnNext(500, 5),
                OnNext(600, 6),
                OnNext(700, 7),
                OnNext(800, 8),
                OnNext(900, 9),
                OnNext(1000, 10)
            );

            var res = scheduler.Start(() => ObservableEx.Create(() => ToIterate_Void_Complete_Dispose(xs, ys, zs)));

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(250, 280)
            );

            zs.Subscriptions.AssertEqual(
                Subscribe(280, 1000)
            );
        }

        [Fact]
        public void Iterate_Void_Func_Throw()
        {
            var scheduler = new TestScheduler();

            var obs = scheduler.Start(() => ObservableEx.Create(() => { throw new InvalidOperationException(); }));

            Assert.Equal(1, obs.Messages.Count);

            var notification = obs.Messages[0].Value;
            Assert.Equal(NotificationKind.OnError, notification.Kind);
            Assert.IsType<InvalidOperationException>(notification.Exception);
        }

        private static IEnumerable<IObservable<object>> _IteratorScenario_Void(int x, int y)
        {
            var xs = Observable.Range(1, x).ToListObservable();
            yield return xs;

            var ys = Observable.Range(1, y).ToListObservable();
            yield return ys;
        }

        [Fact]
        public void IteratorScenario_Void()
        {
            var xs = ObservableEx.Create(() => _IteratorScenario_Void(100, 1000));

            xs.AssertEqual(new Unit[] { }.ToObservable());
        }

    }
}
