// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
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
            AssertThrows<ArgumentNullException>(() => EnumerableEx.For<int, int>(null, x => new[] { 1 }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.For<int, int>(new[] { 1 }, null));
        }

        [Fact]
        public void For1()
        {
            var res = EnumerableEx.For(new[] { 1, 2, 3 }, x => Enumerable.Range(0, x)).ToList();
            Assert.True(res.SequenceEqual(new[] { 0, 0, 1, 0, 1, 2 }));
        }
    }
}
