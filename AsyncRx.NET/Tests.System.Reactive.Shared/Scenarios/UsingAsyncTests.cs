// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>UsingAsync</c> scenarios, from Rx.NET's <c>UsingAsyncTest.cs</c>.</summary>
/// <remarks>
/// 3 of the file's 4 tests (<c>UsingAsync_ArgumentChecking</c> is the code-generated stratum):
/// Rx.NET's asynchronous <c>Using</c>, whose two factories take a token cancelled when the
/// subscriber unsubscribes while they run, as the asynchronous <c>Seq.Using</c> overload over
/// <c>ValueTask</c>s. The factories are the originals' <c>Task.Factory.StartNew</c> calls
/// awaited, the resources the originals' <c>Disposable</c>s, <c>ToEnumerable().ToList()</c>
/// becomes <c>ToListAsync</c>, awaited, and the subscribes and disposals of the two
/// cancellation scenarios are awaited.
/// </remarks>
public abstract class UsingAsyncTests : SharedReactiveTest
{
    [TestMethod]
    public async Task UsingAsync_Simple()
    {
        var done = new CountdownEvent(1);

        var xs = Seq.Using(
            async ct => await Task.Factory.StartNew(() => Disposable.Create(() => done.Signal())),
            async (_, ct) => await Task.Factory.StartNew(() => Seq.Return(42))
        );

        var res = await ToListAsync(xs);

        Assert.IsTrue(new[] { 42 }.SequenceEqual(res));
        Assert.IsTrue(done.Wait(5000), "done.Wait(5000)");
    }

    [TestMethod]
    public async Task UsingAsync_CancelResource()
    {
        var N = 10;// 0000;
        for (var i = 0; i < N; i++)
        {
            var called = false;

            var s = new ManualResetEvent(false);
            var e = new ManualResetEvent(false);
            var x = new ManualResetEvent(false);

            var xs = Seq.Using(
                async ct => await Task.Factory.StartNew(() =>
                {
                    s.Set();
                    e.WaitOne();
                    while (!ct.IsCancellationRequested)
                    {
                        ;
                    }

                    x.Set();
                    return Disposable.Empty;
                }),
                async (_, ct) =>
                {
                    called = true;
                    return await Task.Factory.StartNew(() =>
                        Seq.Return(42)
                    );
                }
            );

            var d = await xs.SubscribeAsync(Scheduler, _ => { });

            s.WaitOne();
            await d.DisposeAsync();

            e.Set();
            x.WaitOne();

            Assert.IsFalse(called);
        }
    }

    [TestMethod]
    public async Task UsingAsync_CancelFactory()
    {
        var N = 10;// 0000;
        for (var i = 0; i < N; i++)
        {
            var gate = new object();
            var disposed = false;
            var called = false;

            var s = new ManualResetEvent(false);
            var e = new ManualResetEvent(false);
            var x = new ManualResetEvent(false);

            var xs = Seq.Using(
                async ct => await Task.Factory.StartNew(() =>
                    Disposable.Create(() =>
                    {
                        lock (gate)
                        {
                            disposed = true;
                        }
                    })
                ),
                async (_, ct) => await Task.Factory.StartNew(() =>
                {
                    s.Set();
                    e.WaitOne();
                    while (!ct.IsCancellationRequested)
                    {
                        ;
                    }

                    x.Set();
                    return Seq.Defer(() =>
                    {
                        called = true;
                        return Seq.Return(42);
                    });
                })
            );

            var d = await xs.SubscribeAsync(Scheduler, _ => { });

            s.WaitOne();

            //
            // This will *eventually* set the CancellationToken. There's a fundamental race between observing the CancellationToken
            // and returning the IDisposable that will set the CancellationTokenSource. Notice this is reflected in the code above,
            // by looping until the CancellationToken is set.
            //
            await d.DisposeAsync();

            e.Set();
            x.WaitOne();

            while (true)
            {
                lock (gate)
                {
                    if (disposed)
                    {
                        break;
                    }
                }
            }

            Assert.IsFalse(called, i.ToString());
        }
    }
}
