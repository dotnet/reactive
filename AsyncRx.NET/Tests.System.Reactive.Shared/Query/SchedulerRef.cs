// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// A scheduler argument in a description: the platform's own scheduler, carried as
/// <see cref="object"/>. The test's <see cref="TestScheduler"/> is one; the result of its
/// <c>DisableOptimizations()</c> is another.
/// </summary>
public class SchedulerRef(object native, string description)
{
    public object Native => native;

    public override string ToString() => description;
}
