// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Max : Tests
    {
        [Fact]
        public void Max_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Max(null, Comparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Max(new[] { 1 }, null));
        }

        [Fact]
        public void Max1()
        {
            Assert.Equal(5, new[] { 2, 5, 3, 7 }.Max(new Mod7Comparer()));
        }

        private sealed class Mod7Comparer : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return Comparer<int>.Default.Compare(x % 7, y % 7);
            }
        }
    }
}
