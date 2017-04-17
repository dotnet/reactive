// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Globalization;

namespace System.Reactive
{
    internal abstract class Either<TLeft, TRight>
    {
        private Either()
        {
        }

        public static Either<TLeft, TRight> CreateLeft(TLeft value) =>  new Left(value);

        public static Either<TLeft, TRight> CreateRight(TRight value) => new Right(value);

        public abstract TResult Switch<TResult>(Func<TLeft, TResult> caseLeft, Func<TRight, TResult> caseRight);
        public abstract void Switch(Action<TLeft> caseLeft, Action<TRight> caseRight);

        public sealed class Left : Either<TLeft, TRight>, IEquatable<Left>
        {
            public Left(TLeft value)
            {
                Value = value;
            }

            public TLeft Value { get; }

            public override TResult Switch<TResult>(Func<TLeft, TResult> caseLeft, Func<TRight, TResult> caseRight) => caseLeft(Value);

            public override void Switch(Action<TLeft> caseLeft, Action<TRight> caseRight) => caseLeft(Value);

            public bool Equals(Left other)
            {
                if (other == this)
                    return true;
                if (other == null)
                    return false;

                return EqualityComparer<TLeft>.Default.Equals(Value, other.Value);
            }

            public override bool Equals(object obj) => Equals(obj as Left);

            public override int GetHashCode() => EqualityComparer<TLeft>.Default.GetHashCode(Value);

            public override string ToString()
            {
                return string.Format(CultureInfo.CurrentCulture, "Left({0})", Value);
            }
        }

        public sealed class Right : Either<TLeft, TRight>, IEquatable<Right>
        {
            public Right(TRight value)
            {
                Value = value;
            }

            public TRight Value { get; }

            public override TResult Switch<TResult>(Func<TLeft, TResult> caseLeft, Func<TRight, TResult> caseRight) => caseRight(Value);

            public override void Switch(Action<TLeft> caseLeft, Action<TRight> caseRight) => caseRight(Value);

            public bool Equals(Right other)
            {
                if (other == this)
                    return true;
                if (other == null)
                    return false;

                return EqualityComparer<TRight>.Default.Equals(Value, other.Value);
            }

            public override bool Equals(object obj) => Equals(obj as Right);

            public override int GetHashCode() => EqualityComparer<TRight>.Default.GetHashCode(Value);

            public override string ToString() => string.Format(CultureInfo.CurrentCulture, "Right({0})", Value);
        }
    }
}
