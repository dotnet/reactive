// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Reactive.Testing;

namespace ReactiveTests
{
    public class MockDisposable : List<long>, IDisposable
    {
        TestScheduler scheduler;

        public MockDisposable(TestScheduler scheduler)
        {
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            this.scheduler = scheduler;
            Add(scheduler.Clock);
        }

        public void Dispose()
        {
            Add(scheduler.Clock);
        }
    }
}
