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
/// delivery is in progress. In progress means until the task the first delivery returned has
/// completed, which since Step 81 includes a handler that has yielded; the test holds its first
/// delivery by awaiting, which the guard before Step 81, covering only the call into the handler,
/// let the second push through.
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
        var proceed =
            new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        var o = new ConcurrentlyProducingSource(
            Enumerable.Range(0, 100),
            Enumerable.Range(100, 100),
            secondStartsAfter: firstDelivering.Task);

        // The guard holds the observer busy until the task a delivery returned has completed,
        // so this handler holds its first delivery by awaiting, as an asynchronous consumer
        // does, until the second producer has been refused. It yields before releasing the
        // second producer so that the push being refused arrives after the delivery's
        // synchronous part has returned, which is the part a guard that covered only that
        // part would let through (one that signalled from the synchronous part was refused by
        // a race, since the pool thread could push before the handler had returned).
        await o.SubscribeAsync(AsyncObserver.Create<int>(async x =>
        {
            if (x == 0)
            {
                await Task.Yield();
                firstDelivering.TrySetResult();
                await proceed.Task;
            }
        }));

        // Were the second push accepted, no failure would ever arrive and the test would hang
        // instead of failing; the bound turns that into a failure.
        var refused = await o.FirstFailure.WaitAsync(TimeSpan.FromSeconds(10));

        proceed.TrySetResult();
        await o.Done;

        Assert.IsInstanceOfType<InvalidOperationException>(refused);
        Assert.HasCount(1, o.Failures);
    }
}
