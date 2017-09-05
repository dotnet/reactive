// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using Xunit;

namespace Tests
{
    public class While : Tests
    {
        [Fact]
        public void While_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.While<int>(null, new[] { 1 }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.While<int>(() => true, null));
        }

        [Fact]
        public void While1()
        {
            var x = 5;
            var res = EnumerableEx.While(() => x > 0, EnumerableEx.Defer(() => new[] { x }).Do(_ => x--)).ToList();
            Assert.True(Enumerable.SequenceEqual(res, new[] { 5, 4, 3, 2, 1 }));
        }

        [Fact]
        public void While2()
        {
            var x = 0;
            var res = EnumerableEx.While(() => x > 0, EnumerableEx.Defer(() => new[] { x }).Do(_ => x--)).ToList();
            Assert.True(Enumerable.SequenceEqual(res, new int[0]));
        }
    }
}
