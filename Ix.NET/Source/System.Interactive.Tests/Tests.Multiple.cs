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
        public void Concat_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Concat(default(IEnumerable<int>[])));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Concat(default(IEnumerable<IEnumerable<int>>)));
        }

        [Fact]
        public void Concat1()
        {
            var res = new[]
            {
                new[] { 1, 2, 3 },
                new[] { 4, 5 }
            }.Concat();

            Assert.True(Enumerable.SequenceEqual(res, new[] { 1, 2, 3, 4, 5 }));
        }

        [Fact]
        public void Concat2()
        {
            var i = 0;
            var xss = Enumerable.Range(0, 3).Select(x => Enumerable.Range(0, x + 1)).Do(_ => ++i);

            var res = xss.Concat().Select(x => i + " - " + x).ToList();

            Assert.True(Enumerable.SequenceEqual(res, new[] { 
                "1 - 0",
                "2 - 0",
                "2 - 1",
                "3 - 0",
                "3 - 1",
                "3 - 2",
            }));
        }

        [Fact]
        public void Concat3()
        {
            var res = EnumerableEx.Concat(
                new[] { 1, 2, 3 },
                new[] { 4, 5 }
            );

            Assert.True(Enumerable.SequenceEqual(res, new[] { 1, 2, 3, 4, 5 }));
        }

        [Fact]
        public void SelectMany_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.SelectMany<int, int>(null, new[] { 1 }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.SelectMany<int, int>(new[] { 1 }, null));
        }

        [Fact]
        public void SelectMany()
        {
            var res = new[] { 1, 2 }.SelectMany(new[] { 'a', 'b', 'c' }).ToList();
            Assert.True(Enumerable.SequenceEqual(res, new[] { 'a', 'b', 'c', 'a', 'b', 'c' }));
        }
    }
}
