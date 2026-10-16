// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Microsoft.Reactive.Testing;

using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// The shared <c>Buffer</c> scenarios on Rx.NET, plus one over its own scheduler.
/// </summary>
/// <remarks>
/// <c>BufferWithTime_TickWhileOnCompleted</c> is the original: a subscribe that delivers at once
/// advances the <c>TestScheduler</c> from inside the handler, so that the buffer timers fire
/// while the completion is still being delivered, and the test's claim is that nothing throws.
/// The pump forbids advancing from inside a dispatch (as Rx.NET's scheduler does once it is
/// running), so the shape has no async counterpart; it runs against Rx.NET directly.
/// </remarks>
[TestClass]
public sealed class RxBufferTests : BufferTests
{
    protected override IRxTarget Target => RxTarget.Instance;

    [TestMethod]
    public void BufferWithTime_TickWhileOnCompleted()
    {
        var scheduler = new TestScheduler();

        Observable.Return(1)
            .Buffer(TimeSpan.FromMilliseconds(1), TimeSpan.FromMilliseconds(2), scheduler)
            .Subscribe(v =>
            {
                scheduler.AdvanceBy(TimeSpan.FromMilliseconds(1).Ticks);
            });
    }
}
