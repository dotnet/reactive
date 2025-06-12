﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class MaxBy : Tests
    {
#if !NET6_0_OR_GREATER
        [Fact]
        public void MaxBy_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.MaxBy(null, (int x) => x));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.MaxBy([1], default(Func<int, int>)));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.MaxBy(null, (int x) => x, Comparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.MaxBy([1], default, Comparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.MaxBy([1], (int x) => x, null));
        }

        [Fact]
        public void MaxBy1()
        {
            var res = new[] { 2, 5, 0, 7, 4, 3, 6, 2, 1 }.MaxBy(x => x % 3);
            Assert.True(res.SequenceEqual([2, 5, 2]));
        }

        [Fact]
        public void MaxBy_Empty()
        {
            AssertThrows<InvalidOperationException>(() => Enumerable.Empty<int>().MaxBy(x => x));
        }
#endif
    }
}
