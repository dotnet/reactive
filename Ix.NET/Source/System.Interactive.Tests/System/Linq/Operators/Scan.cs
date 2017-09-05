// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Scan : Tests
    {
        [Fact]
        public void Scan_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Scan<int>(null, (x, y) => x + y));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Scan<int>(new[] { 1 }, null));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Scan<int, int>(null, 0, (x, y) => x + y));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Scan<int, int>(new[] { 1 }, 0, null));
        }

        [Fact]
        public void Scan1()
        {
            var res = Enumerable.Range(0, 5).Scan((n, x) => n + x).ToList();
            Assert.True(Enumerable.SequenceEqual(res, new[] { 1, 3, 6, 10 }));
        }

        [Fact]
        public void Scan2()
        {
            var res = Enumerable.Range(0, 5).Scan(10, (n, x) => n - x).ToList();
            Assert.True(Enumerable.SequenceEqual(res, new[] { 10, 9, 7, 4, 0 }));
        }
    }
}
