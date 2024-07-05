// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using Xunit;

namespace Tests
{
    public class For : Tests
    {
        [Fact]
        public void For_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.For<int, int>(null, x => [1]));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.For<int, int>([1], null));
        }

        [Fact]
        public void For1()
        {
            var res = EnumerableEx.For(new[] { 1, 2, 3 }, x => Enumerable.Range(0, x)).ToList();
            Assert.True(res.SequenceEqual([0, 0, 1, 0, 1, 2]));
        }
    }
}
