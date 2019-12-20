// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{

    public class TestSchedulerTest
    {
        [Fact]
        public void Test_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new TestScheduler().Start<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => new TestScheduler().Start<int>(null, 10));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => new TestScheduler().Start(() => Observable.Empty<int>(), 10));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => new TestScheduler().Start(() => Observable.Empty<int>(), 10, 15, 5));
        }
    }
}
