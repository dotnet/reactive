// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing;

namespace Tests.System.Reactive.Shared;

/// <summary>Recorded subscriptions, assertable in the shared (sync) vocabulary.</summary>
public readonly struct SubscriptionLog<T>(IPlatform platform, object native, string source)
{
    public object Native => native;

    public void AssertEqual(params Subscription[] expected)
    {
        try
        {
            platform.AssertEqual(this, expected);
        }
        catch (Exception ex)
        {
            throw new AssertFailedException($"Subscriptions of {source}: {ex.Message}", ex);
        }
    }
}
