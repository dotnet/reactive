// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxFirstAsyncTests(ExecutionShape shape)
    : FirstAsyncTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape);
}

[TestClass]
public sealed class AsyncRxFirstAsyncTests_SynchronousCompletion() : AsyncRxFirstAsyncTests(ExecutionShape.SynchronousCompletion);

[TestClass]
public sealed class AsyncRxFirstAsyncTests_ForcedYield() : AsyncRxFirstAsyncTests(ExecutionShape.ForcedYield);
