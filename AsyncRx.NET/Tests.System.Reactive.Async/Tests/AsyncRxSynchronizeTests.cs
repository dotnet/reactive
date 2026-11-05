// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Async.Tests;

/// <summary>
/// AsyncRx.NET's own <c>Synchronize</c> tests, over a source that pushes concurrently.
/// </summary>
/// <remarks>
/// Rx.NET's <c>Synchronize_BadObservable</c> runs against Rx.NET only (<c>RxSynchronizeTests</c>):
/// its source is built in the test from two threads, which the shared model cannot describe.
/// These are its AsyncRx.NET counterpart over <see cref="ConcurrentlyProducingSource"/>: the
/// first shows <c>Synchronize()</c> serializing two tasks' pushes so that every value arrives;
/// the second shows why the operator is needed here, and pins the maintainer's decision
/// (Step 80) that every other observer in the library rejects a notification arriving while
/// another is in progress: the same source, subscribed without <c>Synchronize</c>, has its second
/// task's first push refused with <see cref="InvalidOperationException"/> while the first task's
/// delivery is in progress. In progress means synchronously so: the guard covers the call into
/// the handler and not a handler that has yielded, which Step 80 found and the maintainer is to
/// rule on; the test holds its first delivery by blocking, as the guard requires.
/// </remarks>
[TestClass]
public sealed class AsyncRxSynchronizeTests
{
    [TestMethod]
    public async Task Synchronize_ConcurrentProducers()
    {
        var o = new ConcurrentlyProducingSource(
            Enumerable.Range(0, 100),
            Enumerable.Range(100, 100));

        var done = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var sum = 0;
        await o.Synchronize().SubscribeAsync(x => sum += x, () => done.TrySetResult());

        // The producers finish whether or not a push was refused; a refusal is the claim failing,
        // and is reported here rather than by never completing.
        await o.Done;
        Assert.IsEmpty(o.Failures);

        await done.Task;

        Assert.AreEqual(Enumerable.Range(0, 200).Sum(), sum);
    }

    [TestMethod]
    public async Task Synchronize_ConcurrentProducers_RejectedWithoutSynchronize()
    {
        var firstDelivering =
            new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var proceed = new ManualResetEvent(false);

        var o = new ConcurrentlyProducingSource(
            Enumerable.Range(0, 100),
            Enumerable.Range(100, 100),
            secondStartsAfter: firstDelivering.Task);

        // The guard refuses a notification that arrives while another's delivery is in progress
        // synchronously: it marks the observer busy around the call into the handler and idle
        // again when that call returns, so a handler that has yielded is not covered (Step 80).
        // This handler therefore holds its delivery by blocking the delivering task's thread,
        // as a slow synchronous consumer would, until the second producer has been refused.
        await o.SubscribeAsync(AsyncObserver.Create<int>(x =>
        {
            if (x == 0)
            {
                firstDelivering.TrySetResult();
                proceed.WaitOne();
            }

            return default;
        }));

        var refused = await o.FirstFailure;

        proceed.Set();
        await o.Done;

        Assert.IsInstanceOfType<InvalidOperationException>(refused);
        Assert.HasCount(1, o.Failures);
    }
}
