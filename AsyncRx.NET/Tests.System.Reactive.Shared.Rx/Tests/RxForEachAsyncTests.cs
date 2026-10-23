// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// The shared <c>ForEachAsync</c> scenarios on Rx.NET, plus its three <c>DisposeThrows</c> tests.
/// </summary>
/// <remarks>
/// The originals say what they are: regression guards for an exception thrown by a
/// subscription's <c>Dispose</c> while <c>ForEachAsync</c> completes, which may reach the task
/// or evaporate depending on whether Rx.NET's current-thread trampoline is already installed
/// higher up the stack, and which the third (<c>SkipCI</c>) catches through
/// <c>Scheduler.Catch</c>. The race is in Rx.NET's current-thread scheduler, which AsyncRx.NET
/// does not have, so they run here against Rx.NET only, as the originals but for MSTest's
/// assertions and the schedulers named in full, since the shared base has a <c>Scheduler</c>
/// property.
/// </remarks>
[TestClass]
public sealed class RxForEachAsyncTests : ForEachAsyncTests
{
    protected override IRxTarget Target => RxTarget.Instance;

    [TestMethod]
    public void ForEachAsync_DisposeThrows1()
    {
        var cts = new CancellationTokenSource();
        var ex = new Exception();

        var xs = Observable.Create<int>(observer =>
        {
            return new CompositeDisposable(
                Observable.Range(0, 10, global::System.Reactive.Concurrency.Scheduler.CurrentThread).Subscribe(observer),
                Disposable.Create(() => { throw ex; })
            );
        });

        var lst = new List<int>();
        var t = xs.ForEachAsync(lst.Add, cts.Token);

        //
        // Unfortunately, this doesn't throw for CurrentThread scheduling. The
        // subscription completes prior to assignment of the disposable, so we
        // succeed calling the TrySetResult method for the OnCompleted handler
        // prior to observing the exception of the Dispose operation, which is
        // surfacing upon assignment to the SingleAssignmentDisposable. As a
        // result, the exception evaporates.
        //
        // It'd be a breaking change at this point to rethrow the exception in
        // that case, so we're merely asserting regressions here.
        //
        try
        {
            t!.Wait();
        }
        catch
        {
            Assert.Fail();
        }
    }

    [TestMethod]
    public void ForEachAsync_DisposeThrows2()
    {
        var cts = new CancellationTokenSource();
        var ex = new Exception();

        var xs = Observable.Create<int>(observer =>
        {
            return new CompositeDisposable(
                Observable.Range(0, 10, global::System.Reactive.Concurrency.Scheduler.CurrentThread).Subscribe(observer),
                Disposable.Create(() => { throw ex; })
            );
        });

        var lst = new List<int>();

        var t = default(Task);

        global::System.Reactive.Concurrency.Scheduler.CurrentThread.Schedule(() =>
        {
            t = xs.ForEachAsync(lst.Add, cts.Token);
        });

        //
        // If the trampoline of the CurrentThread has been installed higher
        // up the stack, the assignment of the subscription's disposable to
        // the SingleAssignmentDisposable can complete prior to the Dispose
        // method being called from the OnCompleted handler. In this case,
        // the OnCompleted handler's invocation of Dispose will cause the
        // exception to occur, and it bubbles out through TrySetException.
        //
        try
        {
            t!.Wait();
        }
        catch (AggregateException err)
        {
            Assert.HasCount(1, err.InnerExceptions);
            Assert.AreSame(ex, err.InnerExceptions[0]);
        }
    }

    [TestMethod]
    [TestCategory("SkipCI")]
    public void ForEachAsync_DisposeThrows()
    {
        //
        // Unfortunately, this test is non-deterministic due to the race
        // conditions described above in the tests using the CurrentThread
        // scheduler. The exception can come out through the OnCompleted
        // handler but can equally well get swallowed if the main thread
        // hasn't reached the assignment of the disposable yet, causing
        // the OnCompleted handler to win the race. The user can deal with
        // this by hooking an exception handler to the scheduler, so we
        // assert this behavior here.
        //
        // It'd be a breaking change at this point to change rethrowing
        // behavior, so we're merely asserting regressions here.
        //

        var hasCaughtEscapingException = 0;

        var cts = new CancellationTokenSource();
        var ex = new Exception();

        var s = global::System.Reactive.Concurrency.Scheduler.Default.Catch<Exception>(err =>
        {
            Volatile.Write(ref hasCaughtEscapingException, 1);
            return ex == err;
        });

        while (Volatile.Read(ref hasCaughtEscapingException) == 0)
        {
            var xs = Observable.Create<int>(observer =>
            {
                return new CompositeDisposable(
                    Observable.Range(0, 10, s).Subscribe(observer),
                    Disposable.Create(() => { throw ex; })
                );
            });

            var lst = new List<int>();
            var t = xs.ForEachAsync(lst.Add, cts.Token);

            try
            {
                t!.Wait();
            }
            catch (AggregateException err)
            {
                Assert.HasCount(1, err.InnerExceptions);
                Assert.AreSame(ex, err.InnerExceptions[0]);
            }
        }
    }
}
