﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive
{
    /// <summary>
    /// Represents a type with a single value. This type is often used to denote the successful completion of a void-returning method (C#) or a Sub procedure (Visual Basic).
    /// </summary>
    [Serializable]
    public readonly struct Unit : IEquatable<Unit>
    {
        /// <summary>
        /// Determines whether the specified <see cref="Unit"/> value is equal to the current <see cref="Unit"/>. Because <see cref="Unit"/> has a single value, this always returns <c>true</c>.
        /// </summary>
        /// <param name="other">An object to compare to the current <see cref="Unit"/> value.</param>
        /// <returns>Because <see cref="Unit"/> has a single value, this always returns <c>true</c>.</returns>
        public bool Equals(Unit other) => true;

        /// <summary>
        /// Determines whether the specified System.Object is equal to the current <see cref="Unit"/>.
        /// </summary>
        /// <param name="obj">The System.Object to compare with the current <see cref="Unit"/>.</param>
        /// <returns><c>true</c> if the specified System.Object is a <see cref="Unit"/> value; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj) => obj is Unit;

        /// <summary>
        /// Returns the hash code for the current <see cref="Unit"/> value.
        /// </summary>
        /// <returns>A hash code for the current <see cref="Unit"/> value.</returns>
        public override int GetHashCode() => 0;

        /// <summary>
        /// Returns a string representation of the current <see cref="Unit"/> value.
        /// </summary>
        /// <returns>String representation of the current <see cref="Unit"/> value.</returns>
        public override string ToString() => "()";

        /// <summary>
        /// Determines whether the two specified <see cref="Unit"/> values are equal. Because <see cref="Unit"/> has a single value, this always returns <c>true</c>.
        /// </summary>
        /// <param name="first">The first <see cref="Unit"/> value to compare.</param>
        /// <param name="second">The second <see cref="Unit"/> value to compare.</param>
        /// <returns>Because <see cref="Unit"/> has a single value, this always returns <c>true</c>.</returns>
#pragma warning disable IDE0060 // (Remove unused parameter.) Required part of public API
        public static bool operator ==(Unit first, Unit second) => true;

        /// <summary>
        /// Determines whether the two specified <see cref="Unit"/> values are not equal. Because <see cref="Unit"/> has a single value, this always returns <c>false</c>.
        /// </summary>
        /// <param name="first">The first <see cref="Unit"/> value to compare.</param>
        /// <param name="second">The second <see cref="Unit"/> value to compare.</param>
        /// <returns>Because <see cref="Unit"/> has a single value, this always returns <c>false</c>.</returns>
        public static bool operator !=(Unit first, Unit second) => false;
#pragma warning restore IDE0060

        /// <summary>
        /// Gets the single <see cref="Unit"/> value.
        /// </summary>
        public static Unit Default => default;
    }
}
