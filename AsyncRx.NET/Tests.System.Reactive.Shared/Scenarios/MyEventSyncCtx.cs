// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Rx.NET's synchronization context double: runs each post at once with itself current, and
/// counts the posts.
/// </summary>
public class MyEventSyncCtx : SynchronizationContext
{
    /// <summary>How many posts were made.</summary>
    public int PostCount { get; private set; }

    /// <inheritdoc/>
    public override void Post(SendOrPostCallback d, object? state)
    {
        var old = Current;
        SetSynchronizationContext(this);
        try
        {
            PostCount++;
            d(state);
        }
        finally
        {
            SetSynchronizationContext(old);
        }
    }
}
