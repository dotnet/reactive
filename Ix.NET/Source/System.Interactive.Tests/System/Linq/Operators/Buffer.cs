// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Buffer : Tests
    {
        [Fact]
        public void Buffer_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Buffer<int>(null, 5));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Buffer<int>(null, 5, 3));
            AssertThrows<ArgumentOutOfRangeException>(() => EnumerableEx.Buffer<int>([1], 0));
            AssertThrows<ArgumentOutOfRangeException>(() => EnumerableEx.Buffer<int>([1], 5, 0));
            AssertThrows<ArgumentOutOfRangeException>(() => EnumerableEx.Buffer<int>([1], 0, 3));
        }

        [Fact]
        public void Buffer1()
        {
            var rng = Enumerable.Range(0, 10);

            var res = rng.Buffer(3).ToList();
            Assert.Equal(4, res.Count);

            Assert.True(res[0].SequenceEqual([0, 1, 2]));
            Assert.True(res[1].SequenceEqual([3, 4, 5]));
            Assert.True(res[2].SequenceEqual([6, 7, 8]));
            Assert.True(res[3].SequenceEqual([9]));
        }

        [Fact]
        public void Buffer2()
        {
            var rng = Enumerable.Range(0, 10);

            var res = rng.Buffer(5).ToList();
            Assert.Equal(2, res.Count);

            Assert.True(res[0].SequenceEqual([0, 1, 2, 3, 4]));
            Assert.True(res[1].SequenceEqual([5, 6, 7, 8, 9]));
        }

        [Fact]
        public void Buffer3()
        {
            var rng = Enumerable.Empty<int>();

            var res = rng.Buffer(5).ToList();
            Assert.Empty(res);
        }

        [Fact]
        public void Buffer4()
        {
            var rng = Enumerable.Range(0, 10);

            var res = rng.Buffer(3, 2).ToList();
            Assert.Equal(5, res.Count);

            Assert.True(res[0].SequenceEqual([0, 1, 2]));
            Assert.True(res[1].SequenceEqual([2, 3, 4]));
            Assert.True(res[2].SequenceEqual([4, 5, 6]));
            Assert.True(res[3].SequenceEqual([6, 7, 8]));
            Assert.True(res[4].SequenceEqual([8, 9]));
        }

        [Fact]
        public void Buffer5()
        {
            var rng = Enumerable.Range(0, 10);

            var res = rng.Buffer(3, 4).ToList();
            Assert.Equal(3, res.Count);

            Assert.True(res[0].SequenceEqual([0, 1, 2]));
            Assert.True(res[1].SequenceEqual([4, 5, 6]));
            Assert.True(res[2].SequenceEqual([8, 9]));
        }
    }
}
