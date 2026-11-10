// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 


using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

/// <summary>
/// The shared <c>AppendPrependTests</c> on AsyncRx.NET.
/// </summary>
/// <remarks>
/// Nothing of its own: the two real-time originals are over schedulers AsyncRx.NET does not have.
/// </remarks>
public abstract class AsyncRxAppendPrependTests(ExecutionShape shape) : AppendPrependTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape);

}

[TestClass]
public sealed class AsyncRxAppendPrependTests_SynchronousCompletion() : AsyncRxAppendPrependTests(ExecutionShape.SynchronousCompletion);

[TestClass]
public sealed class AsyncRxAppendPrependTests_ForcedYield() : AsyncRxAppendPrependTests(ExecutionShape.ForcedYield);
