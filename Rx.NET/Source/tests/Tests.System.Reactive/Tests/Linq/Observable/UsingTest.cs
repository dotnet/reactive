// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class UsingTest : ReactiveTest
    {

        [Fact]
        public void Using_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Using(null, DummyFunc<IDisposable, IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Using(DummyFunc<IDisposable>.Instance, (Func<IDisposable, IObservable<int>>)null));
            ReactiveAssert.Throws</*some*/Exception>(() => Observable.Using(() => DummyDisposable.Instance, d => default(IObservable<int>)).Subscribe());
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Using(() => DummyDisposable.Instance, d => DummyObservable<int>.Instance).Subscribe(null));
        }

        [Fact]
        public void Using_Null()
        {
            var scheduler = new TestScheduler();

            var disposeInvoked = 0L;
            var createInvoked = 0L;
            var xs = default(ITestableObservable<long>);
            var disposable = default(MockDisposable);
            var _d = default(MockDisposable);

            var res = scheduler.Start(() =>
                Observable.Using(
                    () =>
                    {
                        disposeInvoked++;
                        disposable = default;
                        return disposable;
                    },
                    d =>
                    {
                        _d = d;
                        createInvoked++;
                        xs = scheduler.CreateColdObservable(
                            OnNext(100, scheduler.Clock),
                            OnCompleted<long>(200));
                        return xs;
                    }
                )
            );

            Assert.Same(disposable, _d);

            res.Messages.AssertEqual(
                OnNext(300, 200L),
                OnCompleted<long>(400)
            );

            Assert.Equal(1, createInvoked);
            Assert.Equal(1, disposeInvoked);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            Assert.Null(disposable);
        }

        [Fact]
        public void Using_Complete()
        {
            var scheduler = new TestScheduler();

            var disposeInvoked = 0;
            var createInvoked = 0;
            var xs = default(ITestableObservable<long>);
            var disposable = default(MockDisposable);
            var _d = default(MockDisposable);

            var res = scheduler.Start(() =>
                Observable.Using(
                    () =>
                    {
                        disposeInvoked++;
                        disposable = new MockDisposable(scheduler);
                        return disposable;
                    },
                    d =>
                    {
                        _d = d;
                        createInvoked++;
                        xs = scheduler.CreateColdObservable(
                            OnNext(100, scheduler.Clock),
                            OnCompleted<long>(200));
                        return xs;
                    }
                )
            );

            Assert.Same(disposable, _d);

            res.Messages.AssertEqual(
                OnNext(300, 200L),
                OnCompleted<long>(400)
            );

            Assert.Equal(1, createInvoked);
            Assert.Equal(1, disposeInvoked);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            disposable.AssertEqual(
                200,
                400
            );
        }

        [Fact]
        public void Using_Error()
        {
            var scheduler = new TestScheduler();

            var disposeInvoked = 0;
            var createInvoked = 0;
            var xs = default(ITestableObservable<long>);
            var disposable = default(MockDisposable);
            var _d = default(MockDisposable);
            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Using(
                    () =>
                    {
                        disposeInvoked++;
                        disposable = new MockDisposable(scheduler);
                        return disposable;
                    },
                    d =>
                    {
                        _d = d;
                        createInvoked++;
                        xs = scheduler.CreateColdObservable(
                            OnNext(100, scheduler.Clock),
                            OnError<long>(200, ex));
                        return xs;
                    }
                )
            );

            Assert.Same(disposable, _d);

            res.Messages.AssertEqual(
                OnNext(300, 200L),
                OnError<long>(400, ex)
            );

            Assert.Equal(1, createInvoked);
            Assert.Equal(1, disposeInvoked);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            disposable.AssertEqual(
                200,
                400
            );
        }

        [Fact]
        public void Using_Dispose()
        {
            var scheduler = new TestScheduler();

            var disposeInvoked = 0;
            var createInvoked = 0;
            var xs = default(ITestableObservable<long>);
            var disposable = default(MockDisposable);
            var _d = default(MockDisposable);

            var res = scheduler.Start(() =>
                Observable.Using(
                    () =>
                    {
                        disposeInvoked++;
                        disposable = new MockDisposable(scheduler);
                        return disposable;
                    },
                    d =>
                    {
                        _d = d;
                        createInvoked++;
                        xs = scheduler.CreateColdObservable(
                            OnNext(100, scheduler.Clock),
                            OnNext(1000, scheduler.Clock + 1));
                        return xs;
                    }
                )
            );

            Assert.Same(disposable, _d);

            res.Messages.AssertEqual(
                OnNext(300, 200L)
            );

            Assert.Equal(1, createInvoked);
            Assert.Equal(1, disposeInvoked);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            disposable.AssertEqual(
                200,
                1000
            );
        }

        [Fact]
        public void Using_ThrowResourceSelector()
        {
            var scheduler = new TestScheduler();

            var disposeInvoked = 0;
            var createInvoked = 0;
            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Using<int, IDisposable>(
                    () =>
                    {
                        disposeInvoked++;
                        throw ex;
                    },
                    d =>
                    {
                        createInvoked++;
                        return Observable.Never<int>();
                    }
                )
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );

            Assert.Equal(0, createInvoked);
            Assert.Equal(1, disposeInvoked);
        }

        [Fact]
        public void Using_ThrowResourceUsage()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var disposeInvoked = 0;
            var createInvoked = 0;
            var disposable = default(MockDisposable);

            var res = scheduler.Start(() =>
                Observable.Using<int, IDisposable>(
                    () =>
                    {
                        disposeInvoked++;
                        disposable = new MockDisposable(scheduler);
                        return disposable;
                    },
                    d =>
                    {
                        createInvoked++;
                        throw ex;
                    }
                )
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );

            Assert.Equal(1, createInvoked);
            Assert.Equal(1, disposeInvoked);

            disposable.AssertEqual(
                200,
                200
            );
        }

        [Fact]
        public void Using_NestedCompleted()
        {
            var order = "";

            Observable.Using(() => Disposable.Create(() => order += "3"),
                _ => Observable.Using(() => Disposable.Create(() => order += "2"),
                    __ => Observable.Using(() => Disposable.Create(() => order += "1"),
                        ___ => Observable.Return(Unit.Default))))
                .Finally(() => order += "4")
                .Subscribe();

            Assert.Equal("1234", order);
        }

        [Fact]
        public void Using_NestedDisposed()
        {
            var order = "";

            Observable.Using(() => Disposable.Create(() => order += "3"),
                _ => Observable.Using(() => Disposable.Create(() => order += "2"),
                    __ => Observable.Using(() => Disposable.Create(() => order += "1"),
                        ___ => Observable.Never<Unit>())))
                .Finally(() => order += "4")
                .Subscribe()
                .Dispose();

            Assert.Equal("1234", order);
        }
    }
}
