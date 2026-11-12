// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;
using System.Reactive.Subjects;

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

/// <summary>
/// The shared <c>RegressionTests</c> on AsyncRx.NET, plus one equivalent.
/// </summary>
/// <remarks>
/// <c>Bug_1130</c> over <c>AsyncObservable.Start</c>: its result is not an <c>IAsyncSubject</c>,
/// as the original claims of Rx.NET's <c>Start</c>.
/// </remarks>
public abstract class AsyncRxRegressionTests(ExecutionShape shape) : RegressionTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape);

    [TestMethod]
    public void Bug_1130()
    {
        var xs = AsyncObservable.Start(() => 5);
        Assert.IsNull(xs as IAsyncSubject<int, int>);
    }
}

[TestClass]
public sealed class AsyncRxRegressionTests_SynchronousCompletion() : AsyncRxRegressionTests(ExecutionShape.SynchronousCompletion);

[TestClass]
public sealed class AsyncRxRegressionTests_ForcedYield() : AsyncRxRegressionTests(ExecutionShape.ForcedYield);
