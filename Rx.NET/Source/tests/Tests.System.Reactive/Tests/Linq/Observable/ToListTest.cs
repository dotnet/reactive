// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class ToListTest : ReactiveTest
    {

        [Fact]
        public void ToList_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToList<int>(null));
        }

        [Fact]
        public void ToList_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5),
                OnCompleted<int>(660)
            );

            var res = scheduler.Start(() =>
                xs.ToList()
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(660, l => l.SequenceEqual(new[] { 2, 3, 4, 5 })),
                OnCompleted<IList<int>>(660)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 660)
            );
        }

        [Fact]
        public void ToList_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5),
                OnError<int>(660, ex)
            );

            var res = scheduler.Start(() =>
                xs.ToList()
            );

            res.Messages.AssertEqual(
                OnError<IList<int>>(660, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 660)
            );
        }

        [Fact]
        public void ToList_Disposed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5)
            );

            var res = scheduler.Start(() =>
                xs.ToList()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

    }
}
