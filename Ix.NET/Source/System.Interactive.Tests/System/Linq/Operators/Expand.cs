// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Expand : Tests
    {
        [Fact]
        public void Expand_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Expand<int>(null, _ => new[] { _ }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Expand<int>(new[] { 1 }, null));
        }

        [Fact]
        public void Expand1()
        {
            var res = new[] { 0 }.Expand(x => new[] { x + 1 }).Take(10).ToList();
            Assert.True(Enumerable.SequenceEqual(res, Enumerable.Range(0, 10)));
        }

        [Fact]
        public void Expand2()
        {
            var res = new[] { 3 }.Expand(x => Enumerable.Range(0, x)).ToList();
            var exp = new[] {
                3,
                0, 1, 2,
                0,
                0, 1,
                0
            };
            Assert.True(Enumerable.SequenceEqual(res, exp));
        }
    }
}
