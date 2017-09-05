// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Distinct : Tests
    {
        [Fact]
        public void Distinct_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Distinct<int, int>(null, _ => _));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Distinct<int, int>(new[] { 1 }, null));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Distinct<int, int>(null, _ => _, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Distinct<int, int>(new[] { 1 }, null, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Distinct<int, int>(new[] { 1 }, _ => _, null));
        }

        [Fact]
        public void Distinct1()
        {
            var res = Enumerable.Range(0, 10).Distinct(x => x % 5).ToList();
            Assert.True(Enumerable.SequenceEqual(res, Enumerable.Range(0, 5)));
        }

        [Fact]
        public void Distinct2()
        {
            var res = Enumerable.Range(0, 10).Distinct(x => x % 5, new MyEqualityComparer()).ToList();
            Assert.True(Enumerable.SequenceEqual(res, new[] { 0, 1 }));
        }

        private sealed class MyEqualityComparer : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return x % 2 == y % 2;
            }

            public int GetHashCode(int obj)
            {
                return EqualityComparer<int>.Default.GetHashCode(obj % 2);
            }
        }
    }
}
