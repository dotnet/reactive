// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Reactive
{
    /// <summary>
    /// Represents a type with a single value. This type is often used to denote the successful completion of a void-returning method (C#) or a Sub procedure (Visual Basic).
    /// </summary>
#if !NO_SERIALIZABLE
    [Serializable]
#endif
    public struct Unit : IEquatable<Unit>
    {
        /// <summary>
        /// Determines whether the specified Unit values is equal to the current Unit. Because Unit has a single value, this always returns true.
        /// </summary>
        /// <param name="other">An object to compare to the current Unit value.</param>
        /// <returns>Because Unit has a single value, this always returns true.</returns>
        public bool Equals(Unit other)
        {
            return true;
        }

        /// <summary>
        /// Determines whether the specified System.Object is equal to the current Unit.
        /// </summary>
        /// <param name="obj">The System.Object to compare with the current Unit.</param>
        /// <returns>true if the specified System.Object is a Unit value; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return obj is Unit;
        }

        /// <summary>
        /// Returns the hash code for the current Unit value.
        /// </summary>
        /// <returns>A hash code for the current Unit value.</returns>
        public override int GetHashCode()
        {
            return 0;
        }

        /// <summary>
        /// Returns a string representation of the current Unit value.
        /// </summary>
        /// <returns>String representation of the current Unit value.</returns>
        public override string ToString()
        {
            return "()";
        }

        /// <summary>
        /// Determines whether the two specified Unit values are equal. Because Unit has a single value, this always returns true.
        /// </summary>
        /// <param name="first">The first Unit value to compare.</param>
        /// <param name="second">The second Unit value to compare.</param>
        /// <returns>Because Unit has a single value, this always returns true.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "first", Justification = "Parameter required for operator overloading."), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "second", Justification = "Parameter required for operator overloading.")]
        public static bool operator ==(Unit first, Unit second)
        {
            return true;
        }

        /// <summary>
        /// Determines whether the two specified Unit values are not equal. Because Unit has a single value, this always returns false.
        /// </summary>
        /// <param name="first">The first Unit value to compare.</param>
        /// <param name="second">The second Unit value to compare.</param>
        /// <returns>Because Unit has a single value, this always returns false.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "first", Justification = "Parameter required for operator overloading."), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "second", Justification = "Parameter required for operator overloading.")]
        public static bool operator !=(Unit first, Unit second)
        {
            return false;
        }

        static readonly Unit _default = new Unit();

        /// <summary>
        /// Gets the single unit value.
        /// </summary>
        public static Unit Default { get { return _default; } }
    }
}
