// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

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
