// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using Xunit;

namespace Tests
{
#if !NETCOREAPP2_1
    public class TakeLast : Tests
    {
        [Fact]
        public void TakeLast_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.TakeLast<int>(null, 5));
            AssertThrows<ArgumentOutOfRangeException>(() => EnumerableEx.TakeLast<int>(new[] { 1 }, -1));
        }

        [Fact]
        public void TakeLast_TakeZero()
        {
            var e = Enumerable.Range(1, 5);
            var r = e.TakeLast(0).ToList();
            Assert.True(Enumerable.SequenceEqual(r, Enumerable.Empty<int>()));
        }

        [Fact]
        public void TakeLast_Empty()
        {
            var e = Enumerable.Empty<int>();
            var r = e.TakeLast(1).ToList();
            Assert.True(Enumerable.SequenceEqual(r, e));
        }

        [Fact]
        public void TakeLast_All()
        {
            var e = Enumerable.Range(0, 5);
            var r = e.TakeLast(5).ToList();
            Assert.True(Enumerable.SequenceEqual(r, e));
        }

        [Fact]
        public void TakeLast_Part()
        {
            var e = Enumerable.Range(0, 5);
            var r = e.TakeLast(3).ToList();
            Assert.True(Enumerable.SequenceEqual(r, e.Skip(2)));
        }
    }
#endif
}
