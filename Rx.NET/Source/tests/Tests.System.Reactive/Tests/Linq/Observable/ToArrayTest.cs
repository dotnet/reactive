// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class ToArrayTest : ReactiveTest
    {

        [Fact]
        public void ToArray_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToArray<int>(null));
        }

        [Fact]
        public void ToArray_Completed()
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
                xs.ToArray()
            );

            res.Messages.AssertEqual(
                OnNext<int[]>(660, a => a.SequenceEqual(new[] { 2, 3, 4, 5 })),
                OnCompleted<int[]>(660)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 660)
            );
        }

        [Fact]
        public void ToArray_Error()
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
                xs.ToArray()
            );

            res.Messages.AssertEqual(
                OnError<int[]>(660, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 660)
            );
        }

        [Fact]
        public void ToArray_Disposed()
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
                xs.ToArray()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

    }
}
