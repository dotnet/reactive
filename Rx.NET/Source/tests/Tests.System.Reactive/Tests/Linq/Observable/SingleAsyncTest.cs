// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Assert = Xunit.Assert;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class SingleAsyncTest : ReactiveTest
    {

        [TestMethod]
        public void SingleAsync_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SingleAsync(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SingleAsync(default(IObservable<int>), _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SingleAsync(DummyObservable<int>.Instance, default));
        }

        [TestMethod]
        public void SingleAsync_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.SingleAsync()
            );

            res.Messages.AssertEqual(
                OnError<int>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void SingleAsync_One()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.SingleAsync()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void SingleAsync_Many()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.SingleAsync()
            );

            res.Messages.AssertEqual(
                OnError<int>(220, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void SingleAsync_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.SingleAsync()
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void SingleAsync_Predicate()
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

            var res = scheduler.Start(() =>
                xs.SingleAsync(x => x % 2 == 1)
            );

            res.Messages.AssertEqual(
                OnError<int>(240, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void SingleAsync_Predicate_Empty()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.SingleAsync(x => x % 2 == 1)
            );

            res.Messages.AssertEqual(
                OnError<int>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void SingleAsync_Predicate_One()
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

            var res = scheduler.Start(() =>
                xs.SingleAsync(x => x == 4)
            );

            res.Messages.AssertEqual(
                OnNext(250, 4),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void SingleAsync_Predicate_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.SingleAsync(x => x > 10)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void SingleAsync_PredicateThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.SingleAsync(x => { if (x < 4) { return false; } throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod] // https://github.com/dotnet/reactive/issues/1235
        public void MeaningfulStackTrace()
        {
            static async Task Core()
            {
                static void AssertException(Exception e)
                {
                    Assert.IsType(typeof(InvalidOperationException), e);

                    Assert.NotNull(e.StackTrace);
                    Assert.NotEqual("", e.StackTrace);

                    Assert.True(e.StackTrace.Contains("SingleAsync"));
                }

                var xs = Observable.Range(0, 2).SingleAsync();

                try
                {
                    await xs;
                }
                catch (Exception e)
                {
                    AssertException(e);
                }

                try
                {
                    await xs.ToTask();
                }
                catch (Exception e)
                {
                    AssertException(e);
                }

                var tcs = new TaskCompletionSource<bool>();

                xs.Subscribe(
                    _ => { },
                    e => tcs.SetException(e),
                    () => tcs.SetResult(false));

                try
                {
                    await tcs.Task;
                }
                catch (Exception e)
                {
                    AssertException(e);
                }
            }

            Core().GetAwaiter().GetResult();
        }
    }
}
