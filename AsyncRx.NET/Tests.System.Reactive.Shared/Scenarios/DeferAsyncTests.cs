// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>DeferAsync</c> scenarios, from Rx.NET's <c>DeferAsyncTest.cs</c>.</summary>
/// <remarks>
/// 3 of the file's 4 tests (<c>DeferAsync_ArgumentChecking</c> is the code-generated stratum):
/// Rx.NET's <c>Defer</c> over a task-returning factory and its <c>DeferAsync</c> over one that
/// takes a token, as the <c>Seq.Defer</c> and <c>Seq.DeferAsync</c> overloads over a
/// <c>ValueTask</c>. The factories are the originals' <c>Task.Factory.StartNew</c> calls
/// awaited; <c>ToEnumerable().ToList()</c> becomes <c>ToListAsync</c>, awaited, and the
/// subscribe and disposal of the cancellation scenario are awaited.
/// </remarks>
public abstract class DeferAsyncTests : SharedReactiveTest
{
    [TestMethod]
    public async Task DeferAsync_Simple()
    {
        var xs = Seq.Defer(async () => await Task.Factory.StartNew(() => Seq.Return(42)));

        var res = await ToListAsync(xs);

        Assert.IsTrue(new[] { 42 }.SequenceEqual(res));
    }

    [TestMethod]
    public async Task DeferAsync_WithCancel_Simple()
    {
        var xs = Seq.DeferAsync(async ct => await Task.Factory.StartNew(() => Seq.Return(42)));

        var res = await ToListAsync(xs);

        Assert.IsTrue(new[] { 42 }.SequenceEqual(res));
    }

    [TestMethod]
    public async Task DeferAsync_WithCancel_Cancel()
    {
        var N = 10;// 0000;
        for (var i = 0; i < N; i++)
        {
            var e = new ManualResetEvent(false);
            var called = false;

            var xs = Seq.DeferAsync(async ct => await Task.Factory.StartNew(() =>
            {
                e.Set();

                while (!ct.IsCancellationRequested)
                {
                    ;
                }

                return Seq.Defer(() => { called = true; return Seq.Return(42); });
            }));

            var d = await xs.SubscribeAsync(Scheduler, _ => { });

            e.WaitOne();
            await d.DisposeAsync();

            Assert.IsFalse(called);
        }
    }
}
