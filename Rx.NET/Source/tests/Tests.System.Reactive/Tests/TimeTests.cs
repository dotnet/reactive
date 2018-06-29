// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive;
using Xunit;

namespace ReactiveTests.Tests
{

    public class TimeTests
    {
        [Fact]
        public void TimeInterval_Ctor_Properties()
        {
            var ti = new TimeInterval<int>(42, TimeSpan.FromSeconds(123.45));
            Assert.Equal(42, ti.Value);
            Assert.Equal(TimeSpan.FromSeconds(123.45), ti.Interval);
        }

        [Fact]
        public void TimeInterval_Equals()
        {
            var ti = new TimeInterval<int>(42, TimeSpan.FromSeconds(123.45));
            Assert.False(ti.Equals("x"));
            Assert.False(ti.Equals("x"));
            Assert.True(ti.Equals(ti));
            Assert.True(((object)ti).Equals(ti));

            var t2 = new TimeInterval<int>(43, TimeSpan.FromSeconds(123.45));
            Assert.False(ti.Equals(t2));
            Assert.False(((object)ti).Equals(t2));

            var t3 = new TimeInterval<int>(42, TimeSpan.FromSeconds(123.56));
            Assert.False(ti.Equals(t3));
            Assert.False(((object)ti).Equals(t3));

            var t4 = new TimeInterval<int>(42, TimeSpan.FromSeconds(123.45));
            Assert.True(ti.Equals(t4));
            Assert.True(((object)ti).Equals(t4));
        }

        [Fact]
        public void TimeInterval_GetHashCode()
        {
            var ti = new TimeInterval<string>(null, TimeSpan.FromSeconds(123.45));
            Assert.True(ti.GetHashCode() != 0);
            Assert.Equal(ti.GetHashCode(), ti.GetHashCode());

            var t2 = new TimeInterval<string>("", TimeSpan.FromSeconds(123.45));
            Assert.NotEqual(ti.GetHashCode(), t2.GetHashCode());
        }

        [Fact]
        public void TimeInterval_EqualsOperators()
        {
            var ti = new TimeInterval<int>(42, TimeSpan.FromSeconds(123.45));
            var t2 = new TimeInterval<int>(43, TimeSpan.FromSeconds(123.45));
            Assert.False(ti == t2);
            Assert.False(t2 == ti);
            Assert.True(ti != t2);
            Assert.True(t2 != ti);

            var t3 = new TimeInterval<int>(42, TimeSpan.FromSeconds(123.56));
            Assert.False(ti == t3);
            Assert.False(t3 == ti);
            Assert.True(ti != t3);
            Assert.True(t3 != ti);

            var t4 = new TimeInterval<int>(42, TimeSpan.FromSeconds(123.45));
            Assert.True(ti == t4);
            Assert.True(t4 == ti);
            Assert.False(ti != t4);
            Assert.False(t4 != ti);
        }

        [Fact]
        public void TimeInterval_ToString()
        {
            var ti = new TimeInterval<int>(42, TimeSpan.FromSeconds(123.45));
            Assert.True(ti.ToString().Contains(42.ToString()));
            Assert.True(ti.ToString().Contains(TimeSpan.FromSeconds(123.45).ToString()));
        }

        [Fact]
        public void Timestamped_Create()
        {
            var o = DateTimeOffset.UtcNow;
            var ti = Timestamped.Create(42, o);
            Assert.Equal(42, ti.Value);
            Assert.Equal(o, ti.Timestamp);
        }

        [Fact]
        public void Timestamped_Ctor_Properties()
        {
            var o = new DateTimeOffset();
            var ti = new Timestamped<int>(42, o);
            Assert.Equal(42, ti.Value);
            Assert.Equal(o, ti.Timestamp);
        }

        [Fact]
        public void Timestamped_Equals()
        {
            var ti = new Timestamped<int>(42, new DateTimeOffset());
            Assert.False(ti.Equals("x"));
            Assert.False(ti.Equals("x"));
            Assert.True(ti.Equals(ti));
            Assert.True(((object)ti).Equals(ti));

            var t2 = new Timestamped<int>(43, new DateTimeOffset());
            Assert.False(ti.Equals(t2));
            Assert.False(((object)ti).Equals(t2));

            var t3 = new Timestamped<int>(42, new DateTimeOffset().AddDays(1));
            Assert.False(ti.Equals(t3));
            Assert.False(((object)ti).Equals(t3));

            var t4 = new Timestamped<int>(42, new DateTimeOffset());
            Assert.True(ti.Equals(t4));
            Assert.True(((object)ti).Equals(t4));
        }

        [Fact]
        public void Timestamped_GetHashCode()
        {
            var ti = new Timestamped<string>(null, new DateTimeOffset());
            Assert.True(ti.GetHashCode() != 0);
            Assert.Equal(ti.GetHashCode(), ti.GetHashCode());

            var t2 = new Timestamped<string>("", new DateTimeOffset());
            Assert.NotEqual(ti.GetHashCode(), t2.GetHashCode());
        }

        [Fact]
        public void Timestamped_EqualsOperators()
        {
            var o = new DateTimeOffset();

            var ti = new Timestamped<int>(42, o);
            var t2 = new Timestamped<int>(43, o);
            Assert.False(ti == t2);
            Assert.False(t2 == ti);
            Assert.True(ti != t2);
            Assert.True(t2 != ti);

            var t3 = new Timestamped<int>(42, o.AddDays(1));
            Assert.False(ti == t3);
            Assert.False(t3 == ti);
            Assert.True(ti != t3);
            Assert.True(t3 != ti);

            var t4 = new Timestamped<int>(42, o);
            Assert.True(ti == t4);
            Assert.True(t4 == ti);
            Assert.False(ti != t4);
            Assert.False(t4 != ti);
        }

        [Fact]
        public void Timestamped_ToString()
        {
            var o = new DateTimeOffset();
            var ti = new Timestamped<int>(42, o);
            Assert.True(ti.ToString().Contains(42.ToString()));
            Assert.True(ti.ToString().Contains(o.ToString()));
        }
    }
}
