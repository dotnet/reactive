// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;

namespace Tests
{
    public class SkipLastTest : Tests
    {
        [Fact]
        public void SkipLast_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.SkipLast<int>(null, 5));
            AssertThrows<ArgumentOutOfRangeException>(() => EnumerableEx.SkipLast<int>(new[] { 1 }, -1));
        }

        [Fact]
        public void SkipLast_Empty()
        {
            var e = Enumerable.Empty<int>();
#if NETCOREAPP2_1 || WINDOWS_UWP
            var r = EnumerableEx.SkipLast(e, 1).ToList();
#else
            var r = e.SkipLast(1).ToList();
#endif
            Assert.True(Enumerable.SequenceEqual(r, e));
        }

        [Fact]
        public void SkipLast_All()
        {
            var e = Enumerable.Range(0, 5);
#if NETCOREAPP2_1 || WINDOWS_UWP
            var r = EnumerableEx.SkipLast(e, 0).ToList();
#else
            var r = e.SkipLast(0).ToList();
#endif
            Assert.True(Enumerable.SequenceEqual(r, e));
        }

        [Fact]
        public void SkipLast_Part()
        {
            var e = Enumerable.Range(0, 5);
#if NETCOREAPP2_1 || WINDOWS_UWP
            var r = EnumerableEx.SkipLast(e, 3).ToList();
#else
            var r = e.SkipLast(3).ToList();
#endif
            Assert.True(Enumerable.SequenceEqual(r, e.Take(2)));
        }
    }
}
