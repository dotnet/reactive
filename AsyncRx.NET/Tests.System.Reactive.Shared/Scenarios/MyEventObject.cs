// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Rx.NET's double for the reflection-based <c>FromEventPattern</c> scheduler tests: an instance
/// and a static event whose accessors call back into the test.
/// </summary>
public class MyEventObject
{
    /// <summary>Called by the add accessors.</summary>
    public static Action? Add;

    /// <summary>Called by the remove accessors.</summary>
    public static Action? Remove;

    /// <summary>The instance event.</summary>
    public event EventHandler<MyEventArgs> I
    {
        add { Add!(); }
        remove { Remove!(); }
    }

    /// <summary>The static event.</summary>
    public static event EventHandler<MyEventArgs> S
    {
        add { Add!(); }
        remove { Remove!(); }
    }
}
