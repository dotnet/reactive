// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Reactive;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class TimeTests
    {
        [TestMethod]
        public void TimeInterval_Ctor_Properties()
        {
            var ti = new TimeInterval<int>(42, TimeSpan.FromSeconds(123.45));
            Assert.AreEqual(42, ti.Value);
            Assert.AreEqual(TimeSpan.FromSeconds(123.45), ti.Interval);
        }

        [TestMethod]
        public void TimeInterval_Equals()
        {
            var ti = new TimeInterval<int>(42, TimeSpan.FromSeconds(123.45));
            Assert.IsFalse(ti.Equals("x"));
            Assert.IsFalse(((object)ti).Equals("x"));
            Assert.IsTrue(ti.Equals(ti));
            Assert.IsTrue(((object)ti).Equals(ti));

            var t2 = new TimeInterval<int>(43, TimeSpan.FromSeconds(123.45));
            Assert.IsFalse(ti.Equals(t2));
            Assert.IsFalse(((object)ti).Equals(t2));

            var t3 = new TimeInterval<int>(42, TimeSpan.FromSeconds(123.56));
            Assert.IsFalse(ti.Equals(t3));
            Assert.IsFalse(((object)ti).Equals(t3));

            var t4 = new TimeInterval<int>(42, TimeSpan.FromSeconds(123.45));
            Assert.IsTrue(ti.Equals(t4));
            Assert.IsTrue(((object)ti).Equals(t4));
        }

        [TestMethod]
        public void TimeInterval_GetHashCode()
        {
            var ti = new TimeInterval<string>(null, TimeSpan.FromSeconds(123.45));
            Assert.IsTrue(ti.GetHashCode() != 0);
            Assert.AreEqual(ti.GetHashCode(), ti.GetHashCode());

            var t2 = new TimeInterval<string>("", TimeSpan.FromSeconds(123.45));
            Assert.AreNotEqual(ti.GetHashCode(), t2.GetHashCode());
        }

        [TestMethod]
        public void TimeInterval_EqualsOperators()
        {
            var ti = new TimeInterval<int>(42, TimeSpan.FromSeconds(123.45));
            var t2 = new TimeInterval<int>(43, TimeSpan.FromSeconds(123.45));
            Assert.IsFalse(ti == t2);
            Assert.IsFalse(t2 == ti);
            Assert.IsTrue(ti != t2);
            Assert.IsTrue(t2 != ti);

            var t3 = new TimeInterval<int>(42, TimeSpan.FromSeconds(123.56));
            Assert.IsFalse(ti == t3);
            Assert.IsFalse(t3 == ti);
            Assert.IsTrue(ti != t3);
            Assert.IsTrue(t3 != ti);

            var t4 = new TimeInterval<int>(42, TimeSpan.FromSeconds(123.45));
            Assert.IsTrue(ti == t4);
            Assert.IsTrue(t4 == ti);
            Assert.IsFalse(ti != t4);
            Assert.IsFalse(t4 != ti);
        }

        [TestMethod]
        public void TimeInterval_ToString()
        {
            var ti = new TimeInterval<int>(42, TimeSpan.FromSeconds(123.45));
            Assert.IsTrue(ti.ToString().Contains(42.ToString()));
            Assert.IsTrue(ti.ToString().Contains(TimeSpan.FromSeconds(123.45).ToString()));
        }

        [TestMethod]
        public void TimeStamped_Ctor_Properties()
        {
            var o = new DateTimeOffset();
            var ti = new Timestamped<int>(42, o);
            Assert.AreEqual(42, ti.Value);
            Assert.AreEqual(o, ti.Timestamp);
        }

        [TestMethod]
        public void TimeStamped_Equals()
        {
            var ti = new Timestamped<int>(42, new DateTimeOffset());
            Assert.IsFalse(ti.Equals("x"));
            Assert.IsFalse(((object)ti).Equals("x"));
            Assert.IsTrue(ti.Equals(ti));
            Assert.IsTrue(((object)ti).Equals(ti));

            var t2 = new Timestamped<int>(43, new DateTimeOffset());
            Assert.IsFalse(ti.Equals(t2));
            Assert.IsFalse(((object)ti).Equals(t2));

            var t3 = new Timestamped<int>(42, new DateTimeOffset().AddDays(1));
            Assert.IsFalse(ti.Equals(t3));
            Assert.IsFalse(((object)ti).Equals(t3));

            var t4 = new Timestamped<int>(42, new DateTimeOffset());
            Assert.IsTrue(ti.Equals(t4));
            Assert.IsTrue(((object)ti).Equals(t4));
        }

        [TestMethod]
        public void TimeStamped_GetHashCode()
        {
            var ti = new Timestamped<string>(null, new DateTimeOffset());
            Assert.IsTrue(ti.GetHashCode() != 0);
            Assert.AreEqual(ti.GetHashCode(), ti.GetHashCode());

            var t2 = new Timestamped<string>("", new DateTimeOffset());
            Assert.AreNotEqual(ti.GetHashCode(), t2.GetHashCode());
        }

        [TestMethod]
        public void TimeStamped_EqualsOperators()
        {
            var o = new DateTimeOffset();

            var ti = new Timestamped<int>(42, o);
            var t2 = new Timestamped<int>(43, o);
            Assert.IsFalse(ti == t2);
            Assert.IsFalse(t2 == ti);
            Assert.IsTrue(ti != t2);
            Assert.IsTrue(t2 != ti);

            var t3 = new Timestamped<int>(42, o.AddDays(1));
            Assert.IsFalse(ti == t3);
            Assert.IsFalse(t3 == ti);
            Assert.IsTrue(ti != t3);
            Assert.IsTrue(t3 != ti);

            var t4 = new Timestamped<int>(42, o);
            Assert.IsTrue(ti == t4);
            Assert.IsTrue(t4 == ti);
            Assert.IsFalse(ti != t4);
            Assert.IsFalse(t4 != ti);
        }

        [TestMethod]
        public void TimeStamped_ToString()
        {
            var o = new DateTimeOffset();
            var ti = new Timestamped<int>(42, o);
            Assert.IsTrue(ti.ToString().Contains(42.ToString()));
            Assert.IsTrue(ti.ToString().Contains(o.ToString()));
        }
    }
}
