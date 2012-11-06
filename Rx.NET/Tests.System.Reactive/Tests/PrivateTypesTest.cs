// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !SILVERLIGHT // Reflection security restrictions
using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Reactive.Testing;
using System.Reactive.Linq;

namespace ReactiveTests.Tests
{
    [TestClass]
    public partial class PrivateTypesTest : ReactiveTest
    {
        [TestMethod]
        public void EitherValueRoundtrip()
        {
            {
                var value = 42;
                var left = (Either<int, string>.Left)Either<int, string>.CreateLeft(value);
                Assert.AreEqual(value, left.Value);
            }
            {
                var value = "42";
                var right = (Either<int, string>.Right)Either<int, string>.CreateRight(value);
                Assert.AreEqual(value, right.Value);
            }
        }

        [TestMethod]
        public void EitherEqualsEquatable()
        {
            {
                var value = 42;
                var left = (Either<int, string>.Left)Either<int, string>.CreateLeft(value);

                Assert.IsTrue(left.Equals(left));
                Assert.IsFalse(left.Equals((Either<int, string>.Left)null));

                var other = (Either<int, string>.Left)Either<int, string>.CreateLeft(value + 1);
                Assert.IsFalse(left.Equals(other));
            }
            {
                var value = "42";
                var right = (Either<int, string>.Right)Either<int, string>.CreateRight(value);

                Assert.IsTrue(right.Equals(right));
                Assert.IsFalse(right.Equals((Either<int, string>.Right)null));

                var other = (Either<int, string>.Right)Either<int, string>.CreateRight(value + "1");
                Assert.IsFalse(right.Equals(other));
            }
        }

        [TestMethod]
        public void EitherEqualsObject()
        {
            {
                var value = 42;
                var left = (Either<int, string>.Left)Either<int, string>.CreateLeft(value);

                Assert.IsTrue(left.Equals((object)left));
                Assert.IsFalse(left.Equals((object)null));

                var other = (Either<int, string>.Left)Either<int, string>.CreateLeft(value + 1);
                Assert.IsFalse(left.Equals((object)other));
            }
            {
                var value = "42";
                var right = (Either<int, string>.Right)Either<int, string>.CreateRight(value);

                Assert.IsTrue(right.Equals((object)right));
                Assert.IsFalse(right.Equals((object)null));

                var other = (Either<int, string>.Right)Either<int, string>.CreateRight(value + "1");
                Assert.IsFalse(right.Equals((object)other));
            }
        }

        [TestMethod]
        public void EitherGetHashCode()
        {
            {
                var left = (Either<int, string>.Left)Either<int, string>.CreateLeft(42);
                var other = (Either<int, string>.Left)Either<int, string>.CreateLeft(43);
                Assert.AreNotEqual(left.GetHashCode(), other.GetHashCode());
            }
            {
                var right = (Either<int, string>.Right)Either<int, string>.CreateRight("42");
                var other = (Either<int, string>.Right)Either<int, string>.CreateRight("43");
                Assert.AreNotEqual(right.GetHashCode(), other.GetHashCode());
            }
        }

        [TestMethod]
        public void EitherToString()
        {
            {
                var left = (Either<int, string>.Left)Either<int, string>.CreateLeft(42);
                Assert.IsTrue(left.ToString() == "Left(42)");
            }
            {
                var right = (Either<int, string>.Right)Either<int, string>.CreateRight("42");
                Assert.IsTrue(right.ToString() == "Right(42)");
            }
        }

        [TestMethod]
        public void EitherSwitchFunc()
        {
            {
                var value = 42;
                var left = (Either<int, string>.Left)Either<int, string>.CreateLeft(value);
                Assert.AreEqual(left.Switch<int>(l => l, r => r.Length), value);
            }
            {
                var value = "42";
                var right = (Either<int, string>.Right)Either<int, string>.CreateRight(value);
                Assert.AreEqual(right.Switch<int>(l => l, r => r.Length), value.Length);
            }
        }

        [TestMethod]
        public void EitherSwitchAction()
        {
            {
                var value = 42;
                var left = (Either<int, string>.Left)Either<int, string>.CreateLeft(value);
                int res = 0;
                left.Switch(l => { res = 1; }, r => { res = 2;});
                Assert.AreEqual(1, res);
            }
            {
                var value = "42";
                var right = (Either<int, string>.Right)Either<int, string>.CreateRight(value);
                int res = 0;
                right.Switch(l => { res = 1; }, r => { res = 2; });
                Assert.AreEqual(2, res);
            }
        }
    }

    class EitherBase
    {
        protected object _value;

        public override bool Equals(object obj)
        {
            var equ = _value.GetType().GetMethods().Where(m => m.Name == "Equals" && m.GetParameters()[0].ParameterType == typeof(object)).Single();
            return (bool)equ.Invoke(_value, new object[] { obj is EitherBase ? ((EitherBase)obj)._value : obj });
        }

        public override int GetHashCode()
        {
            return (int)_value.GetType().GetMethod("GetHashCode").Invoke(_value, null);
        }

        public override string ToString()
        {
            return (string)_value.GetType().GetMethod("ToString").Invoke(_value, null);
        }
    }

    class Either<TLeft, TRight> : EitherBase
    {
        public static Either<TLeft, TRight> CreateLeft(TLeft value)
        {
            var tpe = typeof(Observable).Assembly.GetTypes().Single(t => t.Name == "Either`2").MakeGenericType(typeof(TLeft), typeof(TRight));
            var mth = tpe.GetMethod("CreateLeft");
            var res = mth.Invoke(null, new object[] { value });
            return new Either<TLeft, TRight>.Left(res);
        }

        public static Either<TLeft, TRight> CreateRight(TRight value)
        {
            var tpe = typeof(Observable).Assembly.GetTypes().Single(t => t.Name == "Either`2").MakeGenericType(typeof(TLeft), typeof(TRight));
            var mth = tpe.GetMethod("CreateRight");
            var res = mth.Invoke(null, new object[] { value });
            return new Either<TLeft, TRight>.Right(res);
        }

        public TResult Switch<TResult>(Func<TLeft, TResult> caseLeft, Func<TRight, TResult> caseRight)
        {
            var mth = _value.GetType().GetMethods().Where(m => m.Name == "Switch" && m.ReturnType != typeof(void)).Single().MakeGenericMethod(typeof(TResult));
            return (TResult)mth.Invoke(_value, new object[] { caseLeft, caseRight });
        }

        public void Switch(Action<TLeft> caseLeft, Action<TRight> caseRight)
        {
            var mth = _value.GetType().GetMethods().Where(m => m.Name == "Switch" && m.ReturnType == typeof(void)).Single();
            mth.Invoke(_value, new object[] { caseLeft, caseRight });
        }

        public sealed class Left : Either<TLeft, TRight>, IEquatable<Left>
        {
            public TLeft Value
            {
                get
                {
                    return (TLeft)_value.GetType().GetProperty("Value").GetValue(_value, null);
                }
            }

            public Left(object value)
            {
                _value = value;
            }

            public bool Equals(Left other)
            {
                var equ = _value.GetType().GetMethods().Where(m => m.Name == "Equals" && m.GetParameters()[0].ParameterType != typeof(object)).Single();
                return (bool)equ.Invoke(_value, new object[] { other == null ? null : other._value });
            }
        }

        public sealed class Right : Either<TLeft, TRight>, IEquatable<Right>
        {
            public TRight Value
            {
                get
                {
                    return (TRight)_value.GetType().GetProperty("Value").GetValue(_value, null);
                }
            }

            public Right(object value)
            {
                _value = value;
            }

            public bool Equals(Right other)
            {
                var equ = _value.GetType().GetMethods().Where(m => m.Name == "Equals" && m.GetParameters()[0].ParameterType != typeof(object)).Single();
                return (bool)equ.Invoke(_value, new object[] { other == null ? null : other._value });
            }
        }
    }
}
#endif
