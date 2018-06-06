// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Globalization;

namespace System.Reactive
{
    /// <summary>
    /// Represents a value associated with time interval information.
    /// The time interval can represent the time it took to produce the value, the interval relative to a previous value, the value's delivery time relative to a base, etc.
    /// </summary>
    /// <typeparam name="T">The type of the value being annotated with time interval information.</typeparam>
#if !NO_SERIALIZABLE
    [Serializable]
#endif
    public struct TimeInterval<T> : IEquatable<TimeInterval<T>>
    {
        /// <summary>
        /// Constructs a time interval value.
        /// </summary>
        /// <param name="value">The value to be annotated with a time interval.</param>
        /// <param name="interval">Time interval associated with the value.</param>
        public TimeInterval(T value, TimeSpan interval)
        {
            Interval = interval;
            Value = value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Gets the interval.
        /// </summary>
        public TimeSpan Interval { get; }

        /// <summary>
        /// Determines whether the current <see cref="TimeInterval{T}"/> value has the same <see cref="Value"/> and <see cref="Interval"/> as a specified <see cref="TimeInterval{T}"/> value.
        /// </summary>
        /// <param name="other">An object to compare to the current <see cref="TimeInterval{T}"/> value.</param>
        /// <returns><c>true</c> if both <see cref="TimeInterval{T}"/> values have the same <see cref="Value"/> and <see cref="Interval"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(TimeInterval<T> other)
        {
            return other.Interval.Equals(Interval) && EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        /// <summary>
        /// Determines whether the two specified <see cref="TimeInterval{T}"/> values have the same <see cref="Value"/> and <see cref="Interval"/>.
        /// </summary>
        /// <param name="first">The first <see cref="TimeInterval{T}"/> value to compare.</param>
        /// <param name="second">The second <see cref="TimeInterval{T}"/> value to compare.</param>
        /// <returns><c>true</c> if the first <see cref="TimeInterval{T}"/> value has the same <see cref="Value"/> and <see cref="Interval"/> as the second <see cref="TimeInterval{T}"/> value; otherwise, <c>false</c>.</returns>
        public static bool operator ==(TimeInterval<T> first, TimeInterval<T> second)
        {
            return first.Equals(second);
        }

        /// <summary>
        /// Determines whether the two specified <see cref="TimeInterval{T}"/> values don't have the same <see cref="Value"/> and <see cref="Interval"/>.
        /// </summary>
        /// <param name="first">The first <see cref="TimeInterval{T}"/> value to compare.</param>
        /// <param name="second">The second <see cref="TimeInterval{T}"/> value to compare.</param>
        /// <returns><c>true</c> if the first <see cref="TimeInterval{T}"/> value has a different <see cref="Value"/> or <see cref="Interval"/> as the second <see cref="TimeInterval{T}"/> value; otherwise, <c>false</c>.</returns>
        public static bool operator !=(TimeInterval<T> first, TimeInterval<T> second)
        {
            return !first.Equals(second);
        }

        /// <summary>
        /// Determines whether the specified System.Object is equal to the current <see cref="TimeInterval{T}"/>.
        /// </summary>
        /// <param name="obj">The System.Object to compare with the current <see cref="TimeInterval{T}"/>.</param>
        /// <returns><c>true</c> if the specified System.Object is equal to the current <see cref="TimeInterval{T}"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is TimeInterval<T>))
                return false;

            var other = (TimeInterval<T>)obj;
            return Equals(other);
        }

        /// <summary>
        /// Returns the hash code for the current <see cref="TimeInterval{T}"/> value.
        /// </summary>
        /// <returns>A hash code for the current <see cref="TimeInterval{T}"/> value.</returns>
        public override int GetHashCode()
        {
            return Interval.GetHashCode() ^ (Value?.GetHashCode() ?? 1963);
        }

        /// <summary>
        /// Returns a string representation of the current <see cref="TimeInterval{T}"/> value.
        /// </summary>
        /// <returns>String representation of the current <see cref="TimeInterval{T}"/> value.</returns>
        public override string ToString()
        {
            return String.Format(CultureInfo.CurrentCulture, "{0}@{1}", Value, Interval);
        }
    }
}
