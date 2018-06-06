// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public partial class Tests
    {
        [Fact]
        public void IsEmtpy_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.IsEmpty<int>(null));
        }

        [Fact]
        public void IsEmpty_Empty()
        {
            Assert.True(Enumerable.Empty<int>().IsEmpty());
        }

        [Fact]
        public void IsEmpty_NonEmpty()
        {
            Assert.False(new[] { 1 }.IsEmpty());
        }

        [Fact]
        public void Min_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Min(null, Comparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Min(new[] { 1 }, null));
        }

        [Fact]
        public void Min()
        {
            Assert.Equal(3, new[] { 5, 3, 7 }.Min(new Mod3Comparer()));
        }

        class Mod3Comparer : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return Comparer<int>.Default.Compare(x % 3, y % 3);
            }
        }

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
        public void MinBy()
        {
            var res = new[] { 2, 5, 0, 7, 4, 3, 6, 2, 1 }.MinBy(x => x % 3);
            Assert.True(res.SequenceEqual(new[] { 0, 3, 6 }));
        }

        [Fact]
        public void MinBy_Empty()
        {
            AssertThrows<InvalidOperationException>(() => Enumerable.Empty<int>().MinBy(x => x));
        }

        [Fact]
        public void Max_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Max(null, Comparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Max(new[] { 1 }, null));
        }

        [Fact]
        public void Max()
        {
            Assert.Equal(5, new[] { 2, 5, 3, 7 }.Max(new Mod7Comparer()));
        }

        class Mod7Comparer : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return Comparer<int>.Default.Compare(x % 7, y % 7);
            }
        }

        [Fact]
        public void MaxBy_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.MaxBy(null, (int x) => x));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.MaxBy(new[] { 1 }, default(Func<int, int>)));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.MaxBy(null, (int x) => x, Comparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.MaxBy(new[] { 1 }, default(Func<int, int>), Comparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.MaxBy(new[] { 1 }, (int x) => x, null));
        }

        [Fact]
        public void MaxBy()
        {
            var res = new[] { 2, 5, 0, 7, 4, 3, 6, 2, 1 }.MaxBy(x => x % 3);
            Assert.True(res.SequenceEqual(new[] { 2, 5, 2 }));
        }

        [Fact]
        public void MaxBy_Empty()
        {
            AssertThrows<InvalidOperationException>(() => Enumerable.Empty<int>().MaxBy(x => x));
        }
    }
}
