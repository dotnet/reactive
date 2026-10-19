// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.ComponentModel;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Rx.NET's double for the reflection-based <c>FromEventPattern</c> variance tests: events whose
/// argument or sender type is more derived than the one asked for.
/// </summary>
public class FromEventPattern_VarianceCheck
{
#pragma warning disable 67
    /// <summary>An event with the base event arguments.</summary>
    public event EventHandler<EventArgs>? E1;

    /// <summary>An event with derived event arguments.</summary>
    public event EventHandler<CancelEventArgs>? E2;

    /// <summary>An event with a typed sender.</summary>
    public event Action<string, EventArgs>? E3;
#pragma warning restore 67

    /// <summary>Raises <see cref="E2"/>.</summary>
    public void OnE2(CancelEventArgs args)
    {
        E2?.Invoke(this, args);
    }

    /// <summary>Raises <see cref="E3"/>.</summary>
    public void OnE3(string sender)
    {
        E3?.Invoke(sender, EventArgs.Empty);
    }
}
