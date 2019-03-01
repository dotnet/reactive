// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
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
            AssertThrows<ArgumentNullException>(() => EnumerableEx.SelectMany<int, int>(null, new[] { 1 }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.SelectMany<int, int>(new[] { 1 }, null));
        }

        [Fact]
        public void SelectMany1()
        {
            var res = new[] { 1, 2 }.SelectMany(new[] { 'a', 'b', 'c' }).ToList();
            Assert.True(Enumerable.SequenceEqual(res, new[] { 'a', 'b', 'c', 'a', 'b', 'c' }));
        }
    }
}
