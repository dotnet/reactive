// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Rx.NET's double with one event-pattern event carrying a value.</summary>
public class MyEventSource
{
    /// <summary>The event.</summary>
    public event EventHandler<MyEventArgs>? Bar;

    /// <summary>Raises <see cref="Bar"/> with <paramref name="value"/>.</summary>
    public void OnBar(int value)
    {
        Bar?.Invoke(this, new MyEventArgs(value));
    }
}
