// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;

// https://github.com/Reactive-Extensions/Rx.NET/issues/134

namespace ReactiveTests.Tests
{
    public class WeakObservableTest : ReactiveTest
    {
        [Fact]
        public void ToWeakObservable_Complete()
        {
            var scheduler = new TestScheduler();

            long lastItem = -1;
            var xs = Observable.Interval(TimeSpan.FromSeconds(1), scheduler);
            xs.ToWeakObservable()
                .Subscribe(m => lastItem = m);

            scheduler.AdvanceBy(TimeSpan.FromSeconds(2));

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            scheduler.AdvanceBy(TimeSpan.FromSeconds(2));

            Assert.Equal(1, lastItem);
        }
    }
}
