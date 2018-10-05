// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
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
        protected object _value;

        public override bool Equals(object obj)
        {
            var equ = _value.GetType().GetMethods().Where(m => m.Name == "Equals" && m.GetParameters()[0].ParameterType == typeof(object)).Single();
            return (bool)equ.Invoke(_value, new object[] { obj is EitherBase ? ((EitherBase)obj)._value : obj });
        }

        public override int GetHashCode()
        {
            return (int)_value.GetType().GetMethod(nameof(GetHashCode)).Invoke(_value, null);
        }

        public override string ToString()
        {
            return (string)_value.GetType().GetMethod(nameof(ToString)).Invoke(_value, null);
        }
    }

    internal class Either<TLeft, TRight> : EitherBase
    {
        public static Either<TLeft, TRight> CreateLeft(TLeft value)
        {
            var tpe = typeof(Observable).GetTypeInfo().Assembly.GetTypes().Single(t => t.Name == "Either`2").MakeGenericType(typeof(TLeft), typeof(TRight));
            var mth = tpe.GetMethod(nameof(CreateLeft));
            var res = mth.Invoke(null, new object[] { value });
            return new Left(res);
        }

        public static Either<TLeft, TRight> CreateRight(TRight value)
        {
            var tpe = typeof(Observable).GetTypeInfo().Assembly.GetTypes().Single(t => t.Name == "Either`2").MakeGenericType(typeof(TLeft), typeof(TRight));
            var mth = tpe.GetMethod(nameof(CreateRight));
            var res = mth.Invoke(null, new object[] { value });
            return new Right(res);
        }

        public TResult Switch<TResult>(Func<TLeft, TResult> caseLeft, Func<TRight, TResult> caseRight)
        {
            var mth = _value.GetType().GetMethods().Where(m => m.Name == nameof(Switch) && m.ReturnType != typeof(void)).Single().MakeGenericMethod(typeof(TResult));
            return (TResult)mth.Invoke(_value, new object[] { caseLeft, caseRight });
        }

        public void Switch(Action<TLeft> caseLeft, Action<TRight> caseRight)
        {
            var mth = _value.GetType().GetMethods().Where(m => m.Name == nameof(Switch) && m.ReturnType == typeof(void)).Single();
            mth.Invoke(_value, new object[] { caseLeft, caseRight });
        }

        public sealed class Left : Either<TLeft, TRight>, IEquatable<Left>
        {
            public TLeft Value
            {
                get
                {
                    return (TLeft)_value.GetType().GetProperty(nameof(Value)).GetValue(_value, null);
                }
            }

            public Left(object value)
            {
                _value = value;
            }

            public bool Equals(Left other)
            {
                var equ = _value.GetType().GetMethods().Where(m => m.Name == nameof(Equals) && m.GetParameters()[0].ParameterType != typeof(object)).Single();
                return (bool)equ.Invoke(_value, new object[] { other?._value });
            }
        }

        public sealed class Right : Either<TLeft, TRight>, IEquatable<Right>
        {
            public TRight Value
            {
                get
                {
                    return (TRight)_value.GetType().GetProperty(nameof(Value)).GetValue(_value, null);
                }
            }

            public Right(object value)
            {
                _value = value;
            }

            public bool Equals(Right other)
            {
                var equ = _value.GetType().GetMethods().Where(m => m.Name == nameof(Equals) && m.GetParameters()[0].ParameterType != typeof(object)).Single();
                return (bool)equ.Invoke(_value, new object[] { other?._value });
            }
        }
    }
}
