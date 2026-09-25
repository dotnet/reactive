// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Shared.Rx;

[TestClass]
public sealed class RxDelayTests : DelayTests
{
    protected override IRxTarget Target => RxTarget.Instance;
}
