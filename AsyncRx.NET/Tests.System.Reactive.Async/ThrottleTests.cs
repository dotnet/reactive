// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using ReactiveTests.Fx;
using Xunit;

namespace ReactiveTests
{
    public class ThrottleTests : ReactiveTest
    {
        [Fact]
        public async Task Throttle_TimeSpan_AllPass()
        {
            var scheduler = new TestAsyncScheduler();

            var xs = await scheduler.CreateHotObservable(
                OnNext(150, 0),
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnNext(300, 4),
                OnCompleted<int>(400)
            );

            var res = await scheduler.Start(() =>
                xs.Throttle(TimeSpan.FromTicks(20), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(230, 1),
                OnNext(260, 2),
                OnNext(290, 3),
                OnNext(320, 4),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }
    }
}
