// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class MinBy : Tests
    {
        [Fact]
        public void MinBy_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.MinBy(null, (int x) => x));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.MinBy(new[] { 1 }, default(Func<int, int>)));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.MinBy(null, (int x) => x, Comparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.MinBy(new[] { 1 }, default(Func<int, int>), Comparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.MinBy(new[] { 1 }, (int x) => x, null));
        }

        [Fact]
        public void MinBy1()
        {
            var res = new[] { 2, 5, 0, 7, 4, 3, 6, 2, 1 }.MinBy(x => x % 3);
            Assert.True(res.SequenceEqual(new[] { 0, 3, 6 }));
        }

        [Fact]
        public void MinBy_Empty()
        {
            AssertThrows<InvalidOperationException>(() => Enumerable.Empty<int>().MinBy(x => x));
        }
    }
}
