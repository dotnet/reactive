// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Globalization;

namespace System.Reactive
{
    /// <summary>
    /// Represents value with a timestamp on it.
    /// The timestamp typically represents the time the value was received, using an IScheduler's clock to obtain the current time.
    /// </summary>
    /// <typeparam name="T">The type of the value being timestamped.</typeparam>
    [Serializable]
    [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Timestamped", Justification = "Reviewed and agreed upon.")]
    public readonly struct Timestamped<T> : IEquatable<Timestamped<T>>
    {
        /// <summary>
        /// Constructs a timestamped value.
        /// </summary>
        /// <param name="value">The value to be annotated with a timestamp.</param>
        /// <param name="timestamp">Timestamp associated with the value.</param>
        public Timestamped(T value, DateTimeOffset timestamp)
        {
            Timestamp = timestamp;
            Value = value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Gets the timestamp.
        /// </summary>
        public DateTimeOffset Timestamp { get; }

        /// <summary>
        /// Deconstructs the timestamped value into a value and a timestamp.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="timestamp">Timestamp associated with the value.</param>
        public void Deconstruct(out T value, out DateTimeOffset timestamp) => (value, timestamp) = (Value, Timestamp);

        /// <summary>
        /// Determines whether the current <see cref="Timestamped{T}" /> value has the same <see cref="Value"/> and <see cref="Timestamp"/> as a specified <see cref="Timestamped{T}" /> value.
        /// </summary>
        /// <param name="other">An object to compare to the current <see cref="Timestamped{T}" /> value.</param>
        /// <returns><c>true</c> if both <see cref="Timestamped{T}" /> values have the same <see cref="Value"/> and <see cref="Timestamp"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(Timestamped<T> other) => other.Timestamp.Equals(Timestamp) && EqualityComparer<T>.Default.Equals(Value, other.Value);

        /// <summary>
        /// Determines whether the two specified <see cref="Timestamped{T}" /> values have the same <see cref="Value"/> and <see cref="Timestamp"/>.
        /// </summary>
        /// <param name="first">The first <see cref="Timestamped{T}" /> value to compare.</param>
        /// <param name="second">The second <see cref="Timestamped{T}" /> value to compare.</param>
        /// <returns><c>true</c> if the first <see cref="Timestamped{T}" /> value has the same <see cref="Value"/> and <see cref="Timestamp"/> as the second <see cref="Timestamped{T}" /> value; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Timestamped<T> first, Timestamped<T> second) => first.Equals(second);

        /// <summary>
        /// Determines whether the two specified <see cref="Timestamped{T}" /> values don't have the same <see cref="Value"/> and <see cref="Timestamp"/>.
        /// </summary>
        /// <param name="first">The first <see cref="Timestamped{T}" /> value to compare.</param>
        /// <param name="second">The second <see cref="Timestamped{T}" /> value to compare.</param>
        /// <returns><c>true</c> if the first <see cref="Timestamped{T}" /> value has a different <see cref="Value"/> or <see cref="Timestamp"/> as the second <see cref="Timestamped{T}" /> value; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Timestamped<T> first, Timestamped<T> second) => !first.Equals(second);

        /// <summary>
        /// Determines whether the specified System.Object is equal to the current <see cref="Timestamped{T}" />.
        /// </summary>
        /// <param name="obj">The System.Object to compare with the current <see cref="Timestamped{T}" />.</param>
        /// <returns><c>true</c> if the specified System.Object is equal to the current <see cref="Timestamped{T}" />; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj) => obj is Timestamped<T> timestamped && Equals(timestamped);

        /// <summary>
        /// Returns the hash code for the current <see cref="Timestamped{T}" /> value.
        /// </summary>
        /// <returns>A hash code for the current <see cref="Timestamped{T}" /> value.</returns>
        public override int GetHashCode()
        {
            // TODO: Use proper hash code combiner.
            return Timestamp.GetHashCode() ^ (Value?.GetHashCode() ?? 1979);
        }

        /// <summary>
        /// Returns a string representation of the current <see cref="Timestamped{T}" /> value.
        /// </summary>
        /// <returns>String representation of the current <see cref="Timestamped{T}" /> value.</returns>
        public override string ToString() => string.Format(CultureInfo.CurrentCulture, "{0}@{1}", Value, Timestamp);
    }

    /// <summary>
    /// A helper class with a factory method for creating <see cref="Timestamped{T}" /> instances.
    /// </summary>
    public static class Timestamped
    {
        /// <summary>
        /// Creates an instance of a <see cref="Timestamped{T}" />.  This is syntactic sugar that uses type inference
        /// to avoid specifying a type in a constructor call, which is very useful when using anonymous types.
        /// </summary>
        /// <param name="value">The value to be annotated with a timestamp.</param>
        /// <param name="timestamp">Timestamp associated with the value.</param>
        /// <returns>Creates a new timestamped value.</returns>
        public static Timestamped<T> Create<T>(T value, DateTimeOffset timestamp)
        {
            return new Timestamped<T>(value, timestamp);
        }
    }
}
