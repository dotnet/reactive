// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using Xunit;

namespace Tests
{
    public class DoWhile : Tests
    {
        [Fact]
        public void DoWhile_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.DoWhile<int>(new[] { 1 }, null));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.DoWhile<int>(null, () => true));
        }

        [Fact]
        public void DoWhile1()
        {
            var x = 5;
            var res = EnumerableEx.DoWhile(EnumerableEx.Defer(() => new[] { x }).Do(_ => x--), () => x > 0).ToList();
            Assert.True(Enumerable.SequenceEqual(res, new[] { 5, 4, 3, 2, 1 }));
        }

        [Fact]
        public void DoWhile2()
        {
            var x = 0;
            var res = EnumerableEx.DoWhile(EnumerableEx.Defer(() => new[] { x }).Do(_ => x--), () => x > 0).ToList();
            Assert.True(Enumerable.SequenceEqual(res, new[] { 0 }));
        }
    }
}
