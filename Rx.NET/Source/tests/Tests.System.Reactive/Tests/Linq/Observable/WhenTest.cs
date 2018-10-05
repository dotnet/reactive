// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Joins;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class WhenTest : ReactiveTest
    {

        [Fact]
        public void When_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.When<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.When((IEnumerable<Plan<int>>)null));
        }

        [Fact]
        public void WhenMultipleDataSymmetric()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnCompleted<int>(270)
            );

            var res = scheduler.Start(() =>
                Observable.When(
                    xs.And(ys).Then((x, y) => x + y)
                )
            );

            res.Messages.AssertEqual(
                OnNext(240, 1 + 4),
                OnNext(250, 2 + 5),
                OnNext(260, 3 + 6),
                OnCompleted<int>(270)
            );
        }

        [Fact]
        public void WhenMultipleDataAsymmetric()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(240, 4),
                OnNext(250, 5),
                OnCompleted<int>(270)
            );

            var res = scheduler.Start(() =>
                Observable.When(
                    xs.And(ys).Then((x, y) => x + y)
                )
            );

            res.Messages.AssertEqual(
                OnNext(240, 1 + 4),
                OnNext(250, 2 + 5),
                OnCompleted<int>(270)
            );
        }

        [Fact]
        public void WhenEmptyEmpty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnCompleted<int>(240)
            );

            var ys = scheduler.CreateHotObservable(
                OnCompleted<int>(270)
            );

            var res = scheduler.Start(() =>
                Observable.When(
                    xs.And(ys).Then((x, y) => x + y)
                )
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(270)
            );
        }

        [Fact]
        public void WhenNeverNever()
        {
            var scheduler = new TestScheduler();

            var xs = Observable.Never<int>();
            var ys = Observable.Never<int>().AsObservable();

            var res = scheduler.Start(() =>
                Observable.When(
                    xs.And(ys).Then((x, y) => x + y)
                )
            );

            res.Messages.AssertEqual(
            );
        }

        [Fact]
        public void WhenThrowNonEmpty()
        {
            var ex = new Exception();
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnError<int>(240, ex)
            );

            var ys = scheduler.CreateHotObservable(
                OnCompleted<int>(270)
            );

            var res = scheduler.Start(() =>
                Observable.When(
                    xs.And(ys).Then((x, y) => x + y)
                )
            );

            res.Messages.AssertEqual(
                OnError<int>(240, ex)
            );
        }

        [Fact]
        public void ComplicatedWhen()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnCompleted<int>(270)
            );

            var zs = scheduler.CreateHotObservable(
                OnNext(220, 7),
                OnNext(230, 8),
                OnNext(240, 9),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                Observable.When(
                    xs.And(ys).Then((x, y) => x + y),
                    xs.And(zs).Then((x, z) => x * z),
                    ys.And(zs).Then((y, z) => y - z)
                )
            );

            res.Messages.AssertEqual(
                OnNext(220, 1 * 7),
                OnNext(230, 2 * 8),
                OnNext(240, 3 + 4),
                OnNext(250, 5 - 9),
                OnCompleted<int>(300)
            );
        }

        [Fact]
        public void When_PlansIteratorThrows()
        {
            var ex = new Exception();
            var _e = default(Exception);

            GetPlans(ex).When().Subscribe(_ => { }, e => { _e = e; });
            Assert.Same(_e, ex);
        }

        private IEnumerable<Plan<int>> GetPlans(Exception ex)
        {
            if (ex != null)
            {
                throw ex;
            }

            yield break;
        }

        [Fact]
        public void SameSource()
        {
            var source = Observable.Range(1, 5);

            var list = Observable.When(source.And(source).Then((a, b) => a + b))
                .ToList().First();

            Assert.Equal(new List<int>() { 2, 4, 6, 8, 10 }, list);
        }
    }
}
