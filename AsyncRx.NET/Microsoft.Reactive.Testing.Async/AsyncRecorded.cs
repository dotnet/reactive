// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Diagnostics;
using System.Reactive;

namespace Microsoft.Reactive.Testing.Async;

/// <summary>
/// Record of a value including the virtual start and end time of an associated operation.
/// </summary>
/// <typeparam name="T">
/// The type of the recorded value. This is typically some <see cref="Notification{T}"/> describing
/// the operation.
/// </typeparam>
/// <remarks>
/// <para>
/// This is the async counterpart of <see cref="Recorded{T}"/>. Asynchronous operations don't
/// necessarily complete at the same virtual time as they start, which is why <see cref="Time"/>
/// has type <see cref="OperationTime"/>, which defines start and end times.
/// </para>
/// <para>
/// The operation is typically the consumption of a notification—a call to one of the
/// <see cref="IAsyncObserver{T}"/> methods (<c>OnNextAsync</c>, etc.), which is why
/// <typeparamref name="T"/> is typically a <see cref="Notification{T}"/>.
/// </para>
/// <para>
/// In some cases, an operation may have started but not completed by the time the test finishes,
/// in which case the end time will be <see cref="OperationTime.Infinite"/>.
/// </para>
/// </remarks>
[DebuggerDisplay("{ToString(),nq}")]
public readonly struct AsyncRecorded<T> : IEquatable<AsyncRecorded<T>>
{
    /// <summary>
    /// Creates a new recorded value with the given time range and value.
    /// </summary>
    /// <param name="time">
    /// The start and end time of the operation that produced the value. If the operation has not
    /// completed, the end time will be <see cref="OperationTime.Infinite"/>.
    /// </param>
    /// <param name="value">The recorded value.</param>
    public AsyncRecorded(OperationTime time, T value)
    {
        Time = time;
        Value = value;
    }

    /// <summary>
    /// Creates a new recorded value with the given time range and value.
    /// </summary>
    /// <param name="start">The start time of the operation that produced the value.</param>
    /// <param name="end">
    /// The end time of the operation that produced the value, or <see cref="OperationTime.Infinite"/>
    /// if the operation has not completed.</param>
    /// <param name="value">The recorded value.</param>
    public AsyncRecorded(long start, long end, T value)
        : this(new OperationTime(start, end), value)
    {
    }

    /// <summary>
    /// Gets the start and end time of the operation.
    /// </summary>
    /// <remarks>
    /// Operations that are incomplete (either because a test is still in progress, or because the
    /// operation did not complete during the test) will have an <see cref="OperationTime.End"/>
    /// of <see cref="OperationTime.Infinite"/>.
    /// </remarks>
    public OperationTime Time { get; }

    /// <summary>
    /// Gets the virtual time at which delivery of the value started.
    /// </summary>
    public long Start => Time.Start;

    /// <summary>
    /// Gets the virtual time at which the operation completed, or
    /// <see cref="OperationTime.Infinite"/> if the operation has not completed.
    /// </summary>
    public long End => Time.End;

    /// <summary>Gets the recorded value.</summary>
    public T Value { get; }

    /// <inheritdoc/>
    public bool Equals(AsyncRecorded<T> other) =>
        Time == other.Time && EqualityComparer<T>.Default.Equals(Value, other.Value);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is AsyncRecorded<T> other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => unchecked((Time.GetHashCode() * 397) ^ EqualityComparer<T>.Default.GetHashCode(Value!));

    /// <summary>
    /// Compares two <see cref="AsyncRecorded{T}"/> instances for equality.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns><c>true</c> if the instances are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(AsyncRecorded<T> left, AsyncRecorded<T> right) => left.Equals(right);

    /// <summary>
    /// Compares two <see cref="AsyncRecorded{T}"/> instances for inequality.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns><c>true</c> if the instances are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(AsyncRecorded<T> left, AsyncRecorded<T> right) => !left.Equals(right);

    /// <inheritdoc/>
    public override string ToString() => $"{Value}@{Time}";
}
