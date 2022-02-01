// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    internal readonly struct Maybe<T> : IEquatable<Maybe<T>>
    {
        public Maybe(T value)
        {
            HasValue = true;
            Value = value;
        }

        public bool HasValue { get; }
        public T Value { get; }

        public bool Equals(Maybe<T> other) => HasValue == other.HasValue && EqualityComparer<T>.Default.Equals(Value, other.Value);
        public override bool Equals(object? other) => other is Maybe<T> m && Equals(m);
        public override int GetHashCode() => HasValue ? EqualityComparer<T>.Default.GetHashCode(Value!) : 0;

        public static bool operator ==(Maybe<T> first, Maybe<T> second) => first.Equals(second);
        public static bool operator !=(Maybe<T> first, Maybe<T> second) => !first.Equals(second);
    }
}
