// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class DistinctUntilChanged : Tests
    {
        [Fact]
        public void DistinctUntilChanged_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.DistinctUntilChanged<int>(null));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.DistinctUntilChanged<int>(null, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.DistinctUntilChanged<int>(new[] { 1 }, null));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.DistinctUntilChanged<int, int>(null, _ => _));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.DistinctUntilChanged<int, int>(new[] { 1 }, null));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.DistinctUntilChanged<int, int>(null, _ => _, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.DistinctUntilChanged<int, int>(new[] { 1 }, null, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.DistinctUntilChanged<int, int>(new[] { 1 }, _ => _, null));
        }

        [Fact]
        public void DistinctUntilChanged1()
        {
            var res = new[] { 1, 2, 2, 3, 3, 3, 2, 2, 1 }.DistinctUntilChanged().ToList();
            Assert.True(Enumerable.SequenceEqual(res, new[] { 1, 2, 3, 2, 1 }));
        }

        [Fact]
        public void DistinctUntilChanged2()
        {
            var res = new[] { 1, 1, 2, 3, 4, 5, 5, 6, 7 }.DistinctUntilChanged(x => x / 2).ToList();
            Assert.True(Enumerable.SequenceEqual(res, new[] { 1, 2, 4, 6 }));
        }
    }
}
