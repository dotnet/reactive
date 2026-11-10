// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>Rx.NET's own <c>Synchronize</c> tests, from its <c>SynchronizeTest.cs</c>.</summary>
/// <remarks>
/// 3 of the file's 4 tests (the argument-checking test is the code-generated stratum), as the
/// originals but for MSTest's assertions, against Rx.NET only. <c>Synchronize_Range</c> and
/// <c>Synchronize_Throw</c> hold Rx.NET's <c>object</c> gate with <c>lock</c> on the test
/// thread and assert that nothing is delivered until it is released: a claim about monitor
/// semantics. AsyncRx.NET's overload takes an <c>AsyncGate</c>, whose re-entrancy is per
/// logical flow, so work the holder schedules (the subscription made under the held gate, and
/// the deliveries it leads to) is not excluded by it; Step 75 verified with a probe that all
/// hundred values arrive while the gate is held. <c>Synchronize_BadObservable</c> pushes into
/// one observer from two threads at once and relies on <c>Synchronize()</c> to serialize them;
/// on AsyncRx.NET every observer derived from <c>AsyncObserverBase</c> rejects a notification
/// that arrives while it is processing another, before <c>Synchronize</c>'s gate is reached,
/// which is the Step 39 deferred question in its sharpest form. Both wait on the maintainer's
/// answers, recorded in the deferred items.
/// </remarks>
[TestClass]
public sealed class RxSynchronizeTests
{
    [TestMethod]
    public void Synchronize_Range()
    {
        var i = 0;
        var outsideLock = true;

        var gate = new object();
        lock (gate)
        {
            outsideLock = false;
            Observable.Range(0, 100, NewThreadScheduler.Default).Synchronize(gate).Subscribe(x => i++, () => { Assert.IsTrue(outsideLock); });
            Thread.Sleep(100);
            Assert.AreEqual(0, i);
            outsideLock = true;
        }

        while (i < 100)
        {
            Thread.Sleep(10);
            lock (gate)
            {
                var start = i;
                Thread.Sleep(100);
                Assert.AreEqual(start, i);
            }
        }
    }

    [TestMethod]
    public void Synchronize_Throw()
    {
        var ex = new Exception();

        var resLock = new object();
        var e = default(Exception);

        var outsideLock = true;

        var gate = new object();
        lock (gate)
        {
            outsideLock = false;
            Observable.Throw<int>(ex, NewThreadScheduler.Default).Synchronize(gate).Subscribe(x => { Assert.Fail(); }, err => { lock (resLock) { e = err; } }, () => { Assert.IsTrue(outsideLock); });
            Thread.Sleep(100);
            Assert.IsNull(e);
            outsideLock = true;
        }

        while (true)
        {
            lock (resLock)
            {
                if (e != null)
                {
                    break;
                }
            }
        }

        Assert.AreSame(ex, e);
    }

    [TestMethod]
    public void Synchronize_BadObservable()
    {
        var o = Observable.Create<int>(obs =>
        {
            var t1 = new Thread(() =>
            {
                for (var i = 0; i < 100; i++)
                {
                    obs.OnNext(i);
                }
            });

            new Thread(() =>
            {
                t1.Start();

                for (var i = 100; i < 200; i++)
                {
                    obs.OnNext(i);
                }

                t1.Join();
                obs.OnCompleted();
            }).Start();

            return () => { };
        });

        var evt = new ManualResetEvent(false);

        var sum = 0;
        o.Synchronize().Subscribe(x => sum += x, () => { evt.Set(); });

        evt.WaitOne();
        Assert.AreEqual(Enumerable.Range(0, 200).Sum(), sum);
    }
}
