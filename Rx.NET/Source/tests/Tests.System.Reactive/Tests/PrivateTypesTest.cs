// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{

    public partial class PrivateTypesTest : ReactiveTest
    {
        [Fact]
        public void EitherValueRoundtrip()
        {
            {
                var value = 42;
                var left = (Either<int, string>.Left)Either<int, string>.CreateLeft(value);
                Assert.Equal(value, left.Value);
            }
            {
                var value = "42";
                var right = (Either<int, string>.Right)Either<int, string>.CreateRight(value);
                Assert.Equal(value, right.Value);
            }
        }

        [Fact]
        public void EitherEqualsEquatable()
        {
            {
                var value = 42;
                var left = (Either<int, string>.Left)Either<int, string>.CreateLeft(value);

                Assert.True(left.Equals(left));
                Assert.False(left.Equals(null));

                var other = (Either<int, string>.Left)Either<int, string>.CreateLeft(value + 1);
                Assert.False(left.Equals(other));
            }
            {
                var value = "42";
                var right = (Either<int, string>.Right)Either<int, string>.CreateRight(value);

                Assert.True(right.Equals(right));
                Assert.False(right.Equals(null));

                var other = (Either<int, string>.Right)Either<int, string>.CreateRight(value + "1");
                Assert.False(right.Equals(other));
            }
        }

        [Fact]
        public void EitherEqualsObject()
        {
            {
                var value = 42;
                var left = (Either<int, string>.Left)Either<int, string>.CreateLeft(value);

                Assert.True(left.Equals((object)left));
                Assert.False(left.Equals((object)null));

                var other = (Either<int, string>.Left)Either<int, string>.CreateLeft(value + 1);
                Assert.False(left.Equals((object)other));
            }
            {
                var value = "42";
                var right = (Either<int, string>.Right)Either<int, string>.CreateRight(value);

                Assert.True(right.Equals((object)right));
                Assert.False(right.Equals((object)null));

                var other = (Either<int, string>.Right)Either<int, string>.CreateRight(value + "1");
                Assert.False(right.Equals((object)other));
            }
        }

        [Fact]
        public void EitherGetHashCode()
        {
            {
                var left = (Either<int, string>.Left)Either<int, string>.CreateLeft(42);
                var other = (Either<int, string>.Left)Either<int, string>.CreateLeft(43);
                Assert.NotEqual(left.GetHashCode(), other.GetHashCode());
            }
            {
                var right = (Either<int, string>.Right)Either<int, string>.CreateRight("42");
                var other = (Either<int, string>.Right)Either<int, string>.CreateRight("43");
                Assert.NotEqual(right.GetHashCode(), other.GetHashCode());
            }
        }

        [Fact]
        public void EitherToString()
        {
            {
                var left = (Either<int, string>.Left)Either<int, string>.CreateLeft(42);
                Assert.True(left.ToString() == "Left(42)");
            }
            {
                var right = (Either<int, string>.Right)Either<int, string>.CreateRight("42");
                Assert.True(right.ToString() == "Right(42)");
            }
        }

        [Fact]
        public void EitherSwitchFunc()
        {
            {
                var value = 42;
                var left = (Either<int, string>.Left)Either<int, string>.CreateLeft(value);
                Assert.Equal(left.Switch(l => l, r => r.Length), value);
            }
            {
                var value = "42";
                var right = (Either<int, string>.Right)Either<int, string>.CreateRight(value);
                Assert.Equal(right.Switch(l => l, r => r.Length), value.Length);
            }
        }

        [Fact]
        public void EitherSwitchAction()
        {
            {
                var value = 42;
                var left = (Either<int, string>.Left)Either<int, string>.CreateLeft(value);
                var res = 0;
                left.Switch(l => { res = 1; }, r => { res = 2; });
                Assert.Equal(1, res);
            }
            {
                var value = "42";
                var right = (Either<int, string>.Right)Either<int, string>.CreateRight(value);
                var res = 0;
                right.Switch(l => { res = 1; }, r => { res = 2; });
                Assert.Equal(2, res);
            }
        }
    }

    internal class EitherBase
    {
        protected object ProxyValue;

        public override bool Equals(object obj)
        {
            return ProxyValue.Equals(obj is EitherBase ? ((EitherBase)obj).ProxyValue : obj);
        }

        public override int GetHashCode()
        {
            return (int)ProxyValue.GetType().GetMethod(nameof(GetHashCode)).Invoke(ProxyValue, null);
        }

        public override string ToString()
        {
            return (string)ProxyValue.GetType().GetMethod(nameof(ToString)).Invoke(ProxyValue, null);
        }
    }

    internal class Either<TLeft, TRight> : EitherBase
    {
        public static Either<TLeft, TRight> CreateLeft(TLeft value)
        {
            return new Left(System.Reactive.Either<TLeft, TRight>.CreateLeft(value));
        }

        public static Either<TLeft, TRight> CreateRight(TRight value)
        {
            return new Right(System.Reactive.Either<TLeft, TRight>.CreateRight(value));
        }

        public TResult Switch<TResult>(Func<TLeft, TResult> caseLeft, Func<TRight, TResult> caseRight)
        {
            return ProxyValue switch
            {
                System.Reactive.Either<TLeft, TRight>.Left left => left.Switch(caseLeft, caseRight),
                System.Reactive.Either<TLeft, TRight>.Right right => right.Switch(caseLeft, caseRight),
                _ => throw new InvalidOperationException($"This instance was created using an unsupported type {ProxyValue.GetType()} for a {nameof(ProxyValue)}"),
            };
        }

        public void Switch(Action<TLeft> caseLeft, Action<TRight> caseRight)
        {
            switch (ProxyValue)
            {
                case System.Reactive.Either<TLeft, TRight>.Left left:
                    left.Switch(caseLeft, caseRight);
                    break;

                case System.Reactive.Either<TLeft, TRight>.Right right:
                    right.Switch(caseLeft, caseRight);
                    break;

                default:
                    throw new InvalidOperationException($"This instance was created using an unsupported type {ProxyValue.GetType()} for a {nameof(ProxyValue)}");
            }
        }

        public sealed class Left : Either<TLeft, TRight>, IEquatable<Left>
        {
            public TLeft Value
            {
                get
                {
                    return (TLeft)ProxyValue.GetType().GetProperty(nameof(Value)).GetValue(ProxyValue, null);
                }
            }

            public Left(System.Reactive.Either<TLeft, TRight> value)
            {
                ProxyValue = value;
            }

            public bool Equals(Left other)
            {
                var equ = ProxyValue.GetType().GetMethods().Where(m => m.Name == nameof(Equals) && m.GetParameters()[0].ParameterType != typeof(object)).Single();
                return (bool)equ.Invoke(ProxyValue, new object[] { other?.ProxyValue });
            }
        }

        public sealed class Right : Either<TLeft, TRight>, IEquatable<Right>
        {
            public TRight Value
            {
                get
                {
                    return (TRight)ProxyValue.GetType().GetProperty(nameof(Value)).GetValue(ProxyValue, null);
                }
            }

            public Right(System.Reactive.Either<TLeft, TRight> value)
            {
                ProxyValue = value;
            }

            public bool Equals(Right other)
            {
                var equ = ProxyValue.GetType().GetMethods().Where(m => m.Name == nameof(Equals) && m.GetParameters()[0].ParameterType != typeof(object)).Single();
                return (bool)equ.Invoke(ProxyValue, new object[] { other?.ProxyValue });
            }
        }
    }
}
