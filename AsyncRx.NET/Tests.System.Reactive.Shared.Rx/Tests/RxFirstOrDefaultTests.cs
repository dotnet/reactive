// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;

// Rx.NET marks its blocking operators obsolete, pointing at the async forms; these tests exist
// to cover them all the same, as the originals do under a project-wide suppression.
#pragma warning disable CS0618

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>Rx.NET's own <c>FirstOrDefault</c> tests, from its <c>FirstOrDefaultTest.cs</c>.</summary>
/// <remarks>
/// <c>FirstOrDefault</c> is the blocking operator: it waits for the result of <c>FirstOrDefaultAsync</c> and
/// returns it. AsyncRx.NET has no blocking operators, and what these tests claim about the
/// element chosen is what the shared <c>FirstOrDefaultAsync</c> scenarios claim in virtual time; what is
/// theirs alone is the blocking adapter, which only Rx.NET has, so they run here against
/// Rx.NET only (the maintainer's decision, Step 46). 6 of the file's 7 tests (the
/// argument-checking test is the code-generated stratum), as the originals but for MSTest's
/// assertions: <c>Assert.ThrowsExactly</c> for <c>ReactiveAssert.Throws</c>, and
/// <c>Assert.AreSame</c> on the thrown instance where the original asserts it is the one
/// expected.
/// <c>FirstOrDefault_NoDoubleSet</c> is a regression test for a race inside Rx.NET's blocking
/// adapter (a <c>ManualResetEvent</c> set twice, the second time after its disposal), with the
/// original's private <see cref="O"/>, a source that hands its observer to the test.
/// </remarks>
[TestClass]
public sealed class RxFirstOrDefaultTests
{
    [TestMethod]
    public void FirstOrDefault_Empty()
    {
        Assert.AreEqual(default, Observable.Empty<int>().FirstOrDefault());
    }

    [TestMethod]
    public void FirstOrDefaultPredicate_Empty()
    {
        Assert.AreEqual(default, Observable.Empty<int>().FirstOrDefault(_ => true));
    }

    [TestMethod]
    public void FirstOrDefault_Return()
    {
        var value = 42;
        Assert.AreEqual(value, Observable.Return(value).FirstOrDefault());
    }

    [TestMethod]
    public void FirstOrDefault_Throw()
    {
        var ex = new Exception();

        var xs = Observable.Throw<int>(ex);

        Assert.AreSame(ex, Assert.ThrowsExactly<Exception>(() => xs.FirstOrDefault()));
    }

    [TestMethod]
    public void FirstOrDefault_Range()
    {
        var value = 42;
        Assert.AreEqual(value, Observable.Range(value, 10).FirstOrDefault());
    }

    [TestMethod]
    public void FirstOrDefault_NoDoubleSet()
    {
        //
        // Regression test for a possible race condition caused by Return style operators
        // that could trigger two Set calls on a ManualResetEvent, causing it to get
        // disposed in between those two calls (cf. FirstOrDefaultInternal). This led
        // to an exception will the following stack trace:
        //
        //    System.ObjectDisposedException: Safe handle has been closed
        //       at System.Runtime.InteropServices.SafeHandle.DangerousAddRef(Boolean& success)
        //       at System.StubHelpers.StubHelpers.SafeHandleAddRef(SafeHandle pHandle, Boolean& success)
        //       at Microsoft.Win32.Win32Native.SetEvent(SafeWaitHandle handle)
        //       at System.Threading.EventWaitHandle.Set()
        //       at System.Reactive.Linq.QueryLanguage.<>c__DisplayClass458_1`1.<FirstOrDefaultInternal>b__2()
        //

        var o = new O();

        Scheduler.Default.Schedule(() =>
        {
            var x = o.FirstOrDefault();
        });

        o.Wait();

        o.Next();

        Thread.Sleep(100); // enough time to let the ManualResetEvent dispose

        o.Done();
    }

    private sealed class O : IObservable<int>
    {
        private readonly ManualResetEvent _event = new(false);
        private IObserver<int>? _observer;

        public void Wait()
        {
            _event.WaitOne();
        }

        public void Next()
        {
            _observer!.OnNext(42);
        }

        public void Done()
        {
            _observer!.OnCompleted();
        }

        public IDisposable Subscribe(IObserver<int> observer)
        {
            _observer = observer;
            _event.Set();
            return Disposable.Empty;
        }
    }
}
