// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Rx.NET's event-bearing double for the <c>FromEvent</c> tests, from its
/// <c>FromEventPatternTest.cs</c>: an <c>Action</c> event, an <c>Action&lt;int&gt;</c> one and
/// one of a custom delegate type.
/// </summary>
/// <remarks>
/// The upstream name is kept so the migrated tests read as the originals; the operator is
/// <c>Seq.FromEvent</c>, so the two do not meet.
/// </remarks>
public class FromEvent
{
    /// <summary>An <c>Action</c> event.</summary>
    public event Action? A;

    /// <summary>Raises <see cref="A"/>.</summary>
    public void OnA()
    {
        A?.Invoke();
    }

    /// <summary>An <c>Action&lt;int&gt;</c> event.</summary>
    public event Action<int>? B;

    /// <summary>Raises <see cref="B"/>.</summary>
    public void OnB(int x)
    {
        B?.Invoke(x);
    }

    /// <summary>An event of the custom <see cref="MyAction"/> type.</summary>
    public event MyAction? C;

    /// <summary>Raises <see cref="C"/>.</summary>
    public void OnC(int x)
    {
        C?.Invoke(x);
    }
}
