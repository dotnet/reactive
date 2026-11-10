// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Subjects;

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

/// <summary>
/// The shared <c>AsyncSubjectTests</c> on AsyncRx.NET, plus the argument checks over its own
/// subjects.
/// </summary>
/// <remarks>
/// The two argument-checking originals over <c>SequentialAsyncAsyncSubject</c>: a null observer or
/// error is refused as the original expects.
/// </remarks>
public abstract class AsyncRxAsyncSubjectTests(ExecutionShape shape) : AsyncSubjectTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape);

    [TestMethod]
    public async Task Subscribe_ArgumentChecking()
    {
        await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            async () => await new SequentialAsyncAsyncSubject<int>().SubscribeAsync(null!));
    }

    [TestMethod]
    public async Task OnError_ArgumentChecking()
    {
        await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            async () => await new SequentialAsyncAsyncSubject<int>().OnErrorAsync(null!));
    }
}

[TestClass]
public sealed class AsyncRxAsyncSubjectTests_SynchronousCompletion() : AsyncRxAsyncSubjectTests(ExecutionShape.SynchronousCompletion);

[TestClass]
public sealed class AsyncRxAsyncSubjectTests_ForcedYield() : AsyncRxAsyncSubjectTests(ExecutionShape.ForcedYield);
