// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using Microsoft.Reactive.Testing;

namespace ReactiveTests
{
    public class MockDisposable : List<long>, IDisposable
    {
        private TestScheduler _scheduler;

        public MockDisposable(TestScheduler scheduler)
        {
            this._scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            Add(scheduler.Clock);
        }

        public void Dispose()
        {
            Add(_scheduler.Clock);
        }
    }
}
