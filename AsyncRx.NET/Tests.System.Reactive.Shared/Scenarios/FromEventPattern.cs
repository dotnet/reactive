// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Diagnostics;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Rx.NET's event-bearing double for the <c>FromEventPattern</c> tests, from its
/// <c>FromEventPatternTest.cs</c>: one event per delegate shape, each with a method that raises
/// it, two whose accessors throw, and a static one.
/// </summary>
/// <remarks>
/// The upstream name is kept so the migrated tests read as the originals; the operator is
/// <c>Seq.FromEventPattern</c>, so the two do not meet.
/// </remarks>
public class FromEventPattern
{
    /// <summary>Event arguments carrying an id, equal when the ids are.</summary>
    [DebuggerDisplay("{Id}")]
    public class TestEventArgs : EventArgs, IEquatable<TestEventArgs>
    {
        /// <summary>The id the raising method was given.</summary>
        public int Id { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Id.ToString();
        }

        /// <inheritdoc/>
        public bool Equals(TestEventArgs? other)
        {
            if (other == this)
            {
                return true;
            }

            if (other == null)
            {
                return false;
            }

            return other.Id == Id;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return Equals(obj as TestEventArgs);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return Id;
        }
    }

    /// <summary>A custom event-pattern delegate.</summary>
    public delegate void TestEventHandler(object sender, TestEventArgs eventArgs);

    /// <summary>An event of the custom delegate type.</summary>
    public event TestEventHandler? E1;

    /// <summary>Raises <see cref="E1"/>.</summary>
    public void M1(int i)
    {
        E1?.Invoke(this, new TestEventArgs { Id = i });
    }

    /// <summary>A generic <c>EventHandler</c> event.</summary>
    public event EventHandler<TestEventArgs>? E2;

    /// <summary>Raises <see cref="E2"/>.</summary>
    public void M2(int i)
    {
        E2?.Invoke(this, new TestEventArgs { Id = i });
    }

    /// <summary>An <c>Action</c> event with the event-pattern shape.</summary>
    public event Action<object, TestEventArgs>? E3;

    /// <summary>Raises <see cref="E3"/>.</summary>
    public void M3(int i)
    {
        E3?.Invoke(this, new TestEventArgs { Id = i });
    }

    /// <summary>An <c>Action</c> event with a single argument.</summary>
    public event Action<int>? E4;

    /// <summary>Raises <see cref="E4"/>.</summary>
    public void M4(int i)
    {
        E4?.Invoke(i);
    }

    /// <summary>An event whose add accessor throws.</summary>
    public event TestEventHandler AddThrows
    {
        add { throw new InvalidOperationException(); }
        remove { }
    }

    /// <summary>An event whose remove accessor throws.</summary>
    public event TestEventHandler RemoveThrows
    {
        add { }
        remove { throw new InvalidOperationException(); }
    }

    /// <summary>A non-generic <c>EventHandler</c> event.</summary>
    public event EventHandler? E5;

    /// <summary>Raises <see cref="E5"/>.</summary>
    public void M5(int i)
    {
        E5?.Invoke(this, new TestEventArgs { Id = i });
    }

    /// <summary>A static event.</summary>
    public static event EventHandler<TestEventArgs>? E6;

    /// <summary>Raises <see cref="E6"/>.</summary>
    public static void M6(int i)
    {
        E6?.Invoke(null, new TestEventArgs { Id = i });
    }
}
