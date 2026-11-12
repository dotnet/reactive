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
/// The shared <c>SubjectTests</c> on AsyncRx.NET, plus the argument checks over its own
/// subjects.
/// </summary>
/// <remarks>
/// The three argument-checking originals that have an AsyncRx.NET counterpart, over
/// <c>SequentialSimpleAsyncSubject</c> and <c>AsyncSubject.Create</c>; a null observer or error is
/// refused as the original expects. <c>Subject_Synchronize_ArgumentChecking</c> has none, as
/// AsyncRx.NET has no <c>Subject.Synchronize</c>.
/// </remarks>
public abstract class AsyncRxSubjectTests(ExecutionShape shape) : SubjectTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape);

    [TestMethod]
    public async Task Subscribe_ArgumentChecking()
    {
        await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            async () => await new SequentialSimpleAsyncSubject<int>().SubscribeAsync(null!));
    }

    [TestMethod]
    public async Task OnError_ArgumentChecking()
    {
        await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            async () => await new SequentialSimpleAsyncSubject<int>().OnErrorAsync(null!));
    }

    [TestMethod]
    public void Subject_Create_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(
            () => AsyncSubject.Create<int>(null!, AsyncObservable.Return(42)));
        Assert.ThrowsExactly<ArgumentNullException>(
            () => AsyncSubject.Create<int>(
                AsyncObserver.Create<int>(x => default),
                null!));
    }
}

[TestClass]
public sealed class AsyncRxSubjectTests_SynchronousCompletion() : AsyncRxSubjectTests(ExecutionShape.SynchronousCompletion);

[TestClass]
public sealed class AsyncRxSubjectTests_ForcedYield() : AsyncRxSubjectTests(ExecutionShape.ForcedYield);
