// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class DoTest : ReactiveTest
    {

        [Fact]
        public void Do_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Do(someObservable, (Action<int>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Do<int>(null, _ => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Do(someObservable, x => { }, (Action)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Do(someObservable, null, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Do<int>(null, x => { }, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Do(someObservable, x => { }, (Action<Exception>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Do(someObservable, null, (Exception _) => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Do<int>(null, x => { }, (Exception _) => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Do(someObservable, x => { }, (Exception _) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Do(someObservable, x => { }, null, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Do(someObservable, null, (Exception _) => { }, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Do<int>(null, x => { }, (Exception _) => { }, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Do(null, Observer.Create<int>(i => { })));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Do(someObservable, default(IObserver<int>)));
        }

        [Fact]
        public void Do_ShouldSeeAllValues()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            var i = 0;
            var sum = 2 + 3 + 4 + 5;
            var res = scheduler.Start(() =>
                xs.Do(x => { i++; sum -= x; })
            );

            Assert.Equal(4, i);
            Assert.Equal(0, sum);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Do_PlainAction()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            var i = 0;
            var res = scheduler.Start(() =>
                xs.Do(_ => { i++; })
            );

            Assert.Equal(4, i);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Do_NextCompleted()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            var i = 0;
            var sum = 2 + 3 + 4 + 5;
            var completed = false;
            var res = scheduler.Start(() =>
                xs.Do(x => { i++; sum -= x; }, () => { completed = true; })
            );

            Assert.Equal(4, i);
            Assert.Equal(0, sum);
            Assert.True(completed);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Do_NextCompleted_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>();

            var i = 0;
            var completed = false;
            var res = scheduler.Start(() =>
                xs.Do(x => { i++; }, () => { completed = true; })
            );

            Assert.Equal(0, i);
            Assert.False(completed);

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Do_NextError()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnError<int>(250, ex)
            );

            var i = 0;
            var sum = 2 + 3 + 4 + 5;
            var sawError = false;
            var res = scheduler.Start(() =>
                xs.Do(x => { i++; sum -= x; }, e => { sawError = e == ex; })
            );

            Assert.Equal(4, i);
            Assert.Equal(0, sum);
            Assert.True(sawError);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnError<int>(250, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Do_NextErrorNot()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            var i = 0;
            var sum = 2 + 3 + 4 + 5;
            var sawError = false;
            var res = scheduler.Start(() =>
                xs.Do(x => { i++; sum -= x; }, _ => { sawError = true; })
            );

            Assert.Equal(4, i);
            Assert.Equal(0, sum);
            Assert.False(sawError);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Do_NextErrorCompleted()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            var i = 0;
            var sum = 2 + 3 + 4 + 5;
            var sawError = false;
            var hasCompleted = false;
            var res = scheduler.Start(() =>
                xs.Do(x => { i++; sum -= x; }, e => { sawError = true; }, () => { hasCompleted = true; })
            );

            Assert.Equal(4, i);
            Assert.Equal(0, sum);
            Assert.False(sawError);
            Assert.True(hasCompleted);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Do_NextErrorCompletedError()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnError<int>(250, ex)
            );

            var i = 0;
            var sum = 2 + 3 + 4 + 5;
            var sawError = false;
            var hasCompleted = false;
            var res = scheduler.Start(() =>
                xs.Do(x => { i++; sum -= x; }, e => { sawError = e == ex; }, () => { hasCompleted = true; })
            );

            Assert.Equal(4, i);
            Assert.Equal(0, sum);
            Assert.True(sawError);
            Assert.False(hasCompleted);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnError<int>(250, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Do_NextErrorCompletedNever()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>();

            var i = 0;
            var sawError = false;
            var hasCompleted = false;
            var res = scheduler.Start(() =>
                xs.Do(x => { i++; }, e => { sawError = true; }, () => { hasCompleted = true; })
            );

            Assert.Equal(0, i);
            Assert.False(sawError);
            Assert.False(hasCompleted);

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Do_Observer_SomeDataWithError()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnError<int>(250, ex)
            );

            var i = 0;
            var sum = 2 + 3 + 4 + 5;
            var sawError = false;
            var hasCompleted = false;
            var res = scheduler.Start(() =>
                xs.Do(Observer.Create<int>(x => { i++; sum -= x; }, e => { sawError = e == ex; }, () => { hasCompleted = true; }))
            );

            Assert.Equal(4, i);
            Assert.Equal(0, sum);
            Assert.True(sawError);
            Assert.False(hasCompleted);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnError<int>(250, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Do_Observer_SomeDataWithoutError()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            var i = 0;
            var sum = 2 + 3 + 4 + 5;
            var sawError = false;
            var hasCompleted = false;
            var res = scheduler.Start(() =>
                xs.Do(Observer.Create<int>(x => { i++; sum -= x; }, e => { sawError = true; }, () => { hasCompleted = true; }))
            );

            Assert.Equal(4, i);
            Assert.Equal(0, sum);
            Assert.False(sawError);
            Assert.True(hasCompleted);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Do1422_Next_NextThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Do(x => { throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Do1422_NextCompleted_NextThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Do(x => { throw ex; }, () => { })
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Do1422_NextCompleted_CompletedThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Do(x => { }, () => { throw ex; })
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnError<int>(250, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Do1422_NextError_NextThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Do(x => { throw ex; }, _ => { })
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Do1422_NextError_ErrorThrows()
        {
            var scheduler = new TestScheduler();

            var ex1 = new Exception();
            var ex2 = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex1)
            );

            var res = scheduler.Start(() =>
                xs.Do(x => { }, _ => { throw ex2; })
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex2)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Do1422_NextErrorCompleted_NextThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Do(x => { throw ex; }, _ => { }, () => { })
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Do1422_NextErrorCompleted_ErrorThrows()
        {
            var scheduler = new TestScheduler();

            var ex1 = new Exception();
            var ex2 = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex1)
            );

            var res = scheduler.Start(() =>
                xs.Do(x => { }, _ => { throw ex2; }, () => { })
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex2)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Do1422_NextErrorCompleted_CompletedThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Do(x => { }, _ => { }, () => { throw ex; })
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnError<int>(250, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Do1422_Observer_NextThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Do(Observer.Create<int>(x => { throw ex; }, _ => { }, () => { }))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Do1422_Observer_ErrorThrows()
        {
            var scheduler = new TestScheduler();

            var ex1 = new Exception();
            var ex2 = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex1)
            );

            var res = scheduler.Start(() =>
                xs.Do(Observer.Create<int>(x => { }, _ => { throw ex2; }, () => { }))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex2)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Do1422_Observer_CompletedThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Do(Observer.Create<int>(x => { }, _ => { }, () => { throw ex; }))
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnError<int>(250, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

    }
}
