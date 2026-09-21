// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Diagnostics;

namespace Microsoft.Reactive.Testing.Async;

/// <summary>
/// The async counterpart of <see cref="Recorded{T}"/>: because notification delivery is
/// awaited (<c>OnNextAsync</c> etc. return <c>ValueTask</c>), delivery is an operation with
/// an extent in virtual time, so an <see cref="OperationTime"/> is recorded instead of the
/// sync type's single timestamp.
/// </summary>
[DebuggerDisplay("{ToString(),nq}")]
public readonly struct AsyncRecorded<T> : IEquatable<AsyncRecorded<T>>
{
    public AsyncRecorded(OperationTime time, T value)
    {
        Time = time;
        Value = value;
    }

    public AsyncRecorded(long start, long end, T value)
        : this(new OperationTime(start, end), value)
    {
    }

    /// <summary>When delivery of the value started and completed. A record whose time is
    /// still incomplete after the pump finishes diagnoses delivery cut off in flight.</summary>
    public OperationTime Time { get; }

    /// <summary>Virtual time at which delivery of the value started.</summary>
    public long Start => Time.Start;

    /// <summary>Virtual time at which delivery completed, or <see cref="OperationTime.Infinite"/>.</summary>
    public long End => Time.End;

    /// <summary>The recorded value.</summary>
    public T Value { get; }

    public bool Equals(AsyncRecorded<T> other) =>
        Time == other.Time && EqualityComparer<T>.Default.Equals(Value, other.Value);

    public override bool Equals(object? obj) => obj is AsyncRecorded<T> other && Equals(other);

    public override int GetHashCode() => unchecked((Time.GetHashCode() * 397) ^ EqualityComparer<T>.Default.GetHashCode(Value!));

    public static bool operator ==(AsyncRecorded<T> left, AsyncRecorded<T> right) => left.Equals(right);

    public static bool operator !=(AsyncRecorded<T> left, AsyncRecorded<T> right) => !left.Equals(right);

    public override string ToString() => $"{Value}@{Time}";
}
