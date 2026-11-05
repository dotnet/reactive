// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

/// <summary>
/// The shared <c>ObserverTests</c> on AsyncRx.NET, plus the equivalents over its own observer.
/// </summary>
/// <remarks>
/// Equivalent-effect tests over AsyncRx.NET's own observer for the originals whose claim it can
/// meet in its own terms. The two argument checks are over <c>AsyncObserver.Create</c> and
/// <c>AsyncObserver.Synchronize</c>. The five that answer <c>Observer.Checked</c> are over the
/// plain observer, which needs no wrapper: by the Step 81 decision a notification arriving while
/// <c>OnNextAsync</c> is delivering is refused with <see cref="InvalidOperationException"/>, and
/// one arriving during or after a terminal is dropped, where Rx.NET's <c>Checked</c> throws in
/// both cases; the names say which. <c>Observer_Synchronize_Forwards</c> is the three blocks of
/// <c>Observer_Synchronize_AsyncLock</c> over the <c>AsyncGate</c> form; the ten-thread originals
/// have Step 80's <c>Synchronize_ConcurrentProducers</c>.
/// </remarks>
public abstract class AsyncRxObserverTests(ExecutionShape shape) : ObserverTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape);

    [TestMethod]
    public void Create_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => AsyncObserver.Create<int>(null!));
        Assert.ThrowsExactly<ArgumentNullException>(
            () => AsyncObserver.Create<int>(null!, _ => default, () => default));
        Assert.ThrowsExactly<ArgumentNullException>(
            () => AsyncObserver.Create<int>(_ => default, null!, () => default));
        Assert.ThrowsExactly<ArgumentNullException>(
            () => AsyncObserver.Create<int>(_ => default, _ => default, null!));
    }

    [TestMethod]
    public async Task Observer_AlreadyTerminated_Completed_Dropped()
    {
        var m = 0;
        var n = 0;

        var o = AsyncObserver.Create<int>(
            _ =>
            {
                m++;
                return default;
            },
            _ =>
            {
                Assert.Fail();
                return default;
            },
            () =>
            {
                n++;
                return default;
            });

        await o.OnNextAsync(1);
        await o.OnNextAsync(2);
        await o.OnCompletedAsync();

        await o.OnCompletedAsync();
        await o.OnErrorAsync(new Exception());
        await o.OnNextAsync(3);

        Assert.AreEqual(2, m);
        Assert.AreEqual(1, n);
    }

    [TestMethod]
    public async Task Observer_AlreadyTerminated_Error_Dropped()
    {
        var m = 0;
        var n = 0;

        var o = AsyncObserver.Create<int>(
            _ =>
            {
                m++;
                return default;
            },
            _ =>
            {
                n++;
                return default;
            },
            () =>
            {
                Assert.Fail();
                return default;
            });

        await o.OnNextAsync(1);
        await o.OnNextAsync(2);
        await o.OnErrorAsync(new Exception());

        await o.OnCompletedAsync();
        await o.OnErrorAsync(new Exception());
        await o.OnNextAsync(3);

        Assert.AreEqual(2, m);
        Assert.AreEqual(1, n);
    }

    [TestMethod]
    public async Task Observer_Reentrant_Next_Refused()
    {
        var n = 0;

        IAsyncObserver<int> o = null!;
        o = AsyncObserver.Create<int>(
            async _ =>
            {
                n++;

                await Assert.ThrowsExactlyAsync<InvalidOperationException>(
                    async () => await o.OnNextAsync(9));
                await Assert.ThrowsExactlyAsync<InvalidOperationException>(
                    async () => await o.OnErrorAsync(new Exception()));
                await Assert.ThrowsExactlyAsync<InvalidOperationException>(
                    async () => await o.OnCompletedAsync());
            },
            _ =>
            {
                Assert.Fail();
                return default;
            },
            () =>
            {
                Assert.Fail();
                return default;
            });

        await o.OnNextAsync(1);

        Assert.AreEqual(1, n);
    }

    [TestMethod]
    public async Task Observer_Reentrant_Error_Dropped()
    {
        var n = 0;

        IAsyncObserver<int> o = null!;
        o = AsyncObserver.Create<int>(
            _ =>
            {
                Assert.Fail();
                return default;
            },
            async _ =>
            {
                n++;

                await o.OnNextAsync(9);
                await o.OnErrorAsync(new Exception());
                await o.OnCompletedAsync();
            },
            () =>
            {
                Assert.Fail();
                return default;
            });

        await o.OnErrorAsync(new Exception());

        Assert.AreEqual(1, n);
    }

    [TestMethod]
    public async Task Observer_Reentrant_Completed_Dropped()
    {
        var n = 0;

        IAsyncObserver<int> o = null!;
        o = AsyncObserver.Create<int>(
            _ =>
            {
                Assert.Fail();
                return default;
            },
            _ =>
            {
                Assert.Fail();
                return default;
            },
            async () =>
            {
                n++;

                await o.OnNextAsync(9);
                await o.OnErrorAsync(new Exception());
                await o.OnCompletedAsync();
            });

        await o.OnCompletedAsync();

        Assert.AreEqual(1, n);
    }

    [TestMethod]
    public void Observer_Synchronize_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => AsyncObserver.Synchronize<int>(null!));

        Assert.ThrowsExactly<ArgumentNullException>(
            () => AsyncObserver.Synchronize<int>(null!, new AsyncGate()));
        Assert.ThrowsExactly<ArgumentNullException>(
            () => AsyncObserver.Synchronize(AsyncObserver.Create<int>(_ => default), null!));
    }

    [TestMethod]
    public async Task Observer_Synchronize_Forwards()
    {
        {
            var res = false;

            var o = AsyncObserver.Create<int>(
                x =>
                {
                    res = x == 1;
                    return default;
                },
                _ =>
                {
                    Assert.Fail();
                    return default;
                },
                () =>
                {
                    Assert.Fail();
                    return default;
                });

            var s = AsyncObserver.Synchronize(o);

            await s.OnNextAsync(1);

            Assert.IsTrue(res);
        }

        {
            var res = default(Exception);

            var err = new Exception();

            var o = AsyncObserver.Create<int>(
                _ =>
                {
                    Assert.Fail();
                    return default;
                },
                ex =>
                {
                    res = ex;
                    return default;
                },
                () =>
                {
                    Assert.Fail();
                    return default;
                });

            var s = AsyncObserver.Synchronize(o);

            await s.OnErrorAsync(err);

            Assert.AreSame(err, res);
        }

        {
            var res = false;

            var o = AsyncObserver.Create<int>(
                _ =>
                {
                    Assert.Fail();
                    return default;
                },
                _ =>
                {
                    Assert.Fail();
                    return default;
                },
                () =>
                {
                    res = true;
                    return default;
                });

            var s = AsyncObserver.Synchronize(o);

            await s.OnCompletedAsync();

            Assert.IsTrue(res);
        }
    }
}

[TestClass]
public sealed class AsyncRxObserverTests_SynchronousCompletion() : AsyncRxObserverTests(ExecutionShape.SynchronousCompletion);

[TestClass]
public sealed class AsyncRxObserverTests_ForcedYield() : AsyncRxObserverTests(ExecutionShape.ForcedYield);
