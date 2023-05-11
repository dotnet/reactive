// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using Microsoft.Reactive.Testing;

namespace ReactiveTests
{
#pragma warning disable CA1816 // (Overridable IDisposable.) This is a specialized base type, and it would be inappropriate to encourage anyone to build derived types that do more in Dispose.
    public class MockDisposable : List<long>, IDisposable
    {
        private readonly TestScheduler _scheduler;

        public MockDisposable(TestScheduler scheduler)
        {
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            Add(scheduler.Clock);
        }

        public void Dispose()
        {
            Add(_scheduler.Clock);
        }
    }
#pragma warning restore CA1816
}
