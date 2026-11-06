// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Event arguments carrying a value, from Rx.NET's <c>FromEventPatternTest.cs</c>.
/// </summary>
public class MyEventArgs : EventArgs
{
    /// <summary>Creates the arguments.</summary>
    public MyEventArgs(int value)
    {
        Value = value;
    }

    /// <summary>The value.</summary>
    public int Value { get; private set; }
}
