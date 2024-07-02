// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using Xunit;

namespace Tests
{
    public class SelectMany : Tests
    {
        [Fact]
        public void SelectMany_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.SelectMany<int, int>(null, [1]));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.SelectMany<int, int>([1], null));
        }

        [Fact]
        public void SelectMany1()
        {
            var res = new[] { 1, 2 }.SelectMany(new[] { 'a', 'b', 'c' }).ToList();
            Assert.True(Enumerable.SequenceEqual(res, ['a', 'b', 'c', 'a', 'b', 'c']));
        }
    }
}
