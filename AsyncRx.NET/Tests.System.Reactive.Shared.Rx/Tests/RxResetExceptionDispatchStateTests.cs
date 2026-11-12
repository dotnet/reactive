// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// Rx.NET's own <c>ResetExceptionDispatchState</c> tests, from its
/// <c>ResetExceptionDispatchStateTest.cs</c>.
/// </summary>
/// <remarks>
/// Both of the file's tests, as the originals but for MSTest's assertions and a nullable local,
/// against Rx.NET only. <c>ResetExceptionDispatchState</c> is a one-sided name (the
/// argument-checking item lists it): it clears the dispatch state of an error so that awaiting the
/// same failing sequence, or a replayed one, throws it with the same stack trace each time.
/// </remarks>
[TestClass]
public sealed class RxResetExceptionDispatchStateTests
{
    [TestMethod]
    public async Task ResetExceptionDispatchState_Throw_Consistent_StackTrace_On_Await()
    {
        var ts = Observable.Throw<int>(new Exception("Aaargh!")).ResetExceptionDispatchState();

        string? stackTrace = null;
        for (var i = 0; i < 3; i++)
        {
            try
            {
                _ = await ts;
            }
            catch (Exception e)
            {
                if (stackTrace is null)
                {
                    stackTrace = e.StackTrace;
                }
                else
                {
                    Assert.AreEqual(stackTrace, e.StackTrace);
                }
            }
        }
    }

    [TestMethod]
    public async Task ResetExceptionDispatchState_Replay_Consistent_StackTrace_On_Await()
    {
        var cts = Observable.Throw<int>(new Exception("Aaargh!"), CurrentThreadScheduler.Instance).Replay(1);
        cts.Connect();
        var ts = cts.ResetExceptionDispatchState();

        string? stackTrace = null;
        for (var i = 0; i < 3; i++)
        {
            try
            {
                _ = await ts;
            }
            catch (Exception e)
            {
                if (stackTrace is null)
                {
                    stackTrace = e.StackTrace;
                }
                else
                {
                    Assert.AreEqual(stackTrace, e.StackTrace);
                }
            }
        }
    }
}
