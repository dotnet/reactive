// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using Xunit;

namespace Tests
{
#if !NETCOREAPP2_1_OR_GREATER
    public class SkipLast : Tests
    {
        [Fact]
        public void SkipLast_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.SkipLast<int>(null, 5));
            AssertThrows<ArgumentOutOfRangeException>(() => EnumerableEx.SkipLast<int>([1], -1));
        }

        [Fact]
        public void SkipLast_Empty()
        {
            var e = Enumerable.Empty<int>();
            var r = e.SkipLast(1).ToList();
            Assert.True(Enumerable.SequenceEqual(r, e));
        }

        [Fact]
        public void SkipLast_All()
        {
            var e = Enumerable.Range(0, 5);
            var r = e.SkipLast(0).ToList();
            Assert.True(Enumerable.SequenceEqual(r, e));
        }

        [Fact]
        public void SkipLast_Part()
        {
            var e = Enumerable.Range(0, 5);
            var r = e.SkipLast(3).ToList();
            Assert.True(Enumerable.SequenceEqual(r, e.Take(2)));
        }
    }
#endif
}
