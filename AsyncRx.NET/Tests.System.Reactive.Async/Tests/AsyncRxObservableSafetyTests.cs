// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 


using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

/// <summary>
/// The shared <c>ObservableSafetyTests</c> on AsyncRx.NET.
/// </summary>
/// <remarks>
/// Nothing of its own: the four <c>*_ObserverThrows</c> originals claim the synchronous
/// <c>Create</c> behaviour the <c>Create</c> scenarios keep Rx.NET-only.
/// </remarks>
public abstract class AsyncRxObservableSafetyTests(ExecutionShape shape) : ObservableSafetyTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape);

}

[TestClass]
public sealed class AsyncRxObservableSafetyTests_SynchronousCompletion() : AsyncRxObservableSafetyTests(ExecutionShape.SynchronousCompletion);

[TestClass]
public sealed class AsyncRxObservableSafetyTests_ForcedYield() : AsyncRxObservableSafetyTests(ExecutionShape.ForcedYield);
