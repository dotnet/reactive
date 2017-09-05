// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Repeat : Tests
    {
        [Fact]
        public void RepeatElementInfinite()
        {
            var xs = EnumerableEx.Repeat(42).Take(1000);
            Assert.True(xs.All(x => x == 42));
            Assert.True(xs.Count() == 1000);
        }

        [Fact]
        public void RepeatSequence_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Repeat<int>(null));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Repeat<int>(null, 5));
            AssertThrows<ArgumentOutOfRangeException>(() => EnumerableEx.Repeat<int>(new[] { 1 }, -1));
        }

        [Fact]
        public void RepeatSequence1()
        {
            var i = 0;
            var xs = new[] { 1, 2 }.Do(_ => i++).Repeat();

            var res = xs.Take(10).ToList();
            Assert.Equal(10, res.Count);
            Assert.True(res.Buffer(2).Select(b => b.Sum()).All(x => x == 3));
            Assert.Equal(10, i);
        }

        [Fact]
        public void RepeatSequence2()
        {
            var i = 0;
            var xs = new[] { 1, 2 }.Do(_ => i++).Repeat(5);

            var res = xs.ToList();
            Assert.Equal(10, res.Count);
            Assert.True(res.Buffer(2).Select(b => b.Sum()).All(x => x == 3));
            Assert.Equal(10, i);
        }
    }
}
