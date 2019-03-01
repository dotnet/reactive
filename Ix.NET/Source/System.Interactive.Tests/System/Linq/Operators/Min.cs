// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Min : Tests
    {
        [Fact]
        public void Min_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Min(null, Comparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Min(new[] { 1 }, null));
        }

        [Fact]
        public void Min1()
        {
            Assert.Equal(3, new[] { 5, 3, 7 }.Min(new Mod3Comparer()));
        }

        private sealed class Mod3Comparer : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return Comparer<int>.Default.Compare(x % 3, y % 3);
            }
        }
    }
}
