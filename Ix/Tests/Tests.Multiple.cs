// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    public partial class Tests
    {
        [TestMethod]
        public void Concat_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Concat(default(IEnumerable<int>[])));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Concat(default(IEnumerable<IEnumerable<int>>)));
        }

        [TestMethod]
        public void Concat1()
        {
            var res = new[]
            {
                new[] { 1, 2, 3 },
                new[] { 4, 5 }
            }.Concat();

            Assert.IsTrue(Enumerable.SequenceEqual(res, new[] { 1, 2, 3, 4, 5 }));
        }

        [TestMethod]
        public void Concat2()
        {
            var i = 0;
            var xss = Enumerable.Range(0, 3).Select(x => Enumerable.Range(0, x + 1)).Do(_ => ++i);

            var res = xss.Concat().Select(x => i + " - " + x).ToList();

            Assert.IsTrue(Enumerable.SequenceEqual(res, new[] { 
                "1 - 0",
                "2 - 0",
                "2 - 1",
                "3 - 0",
                "3 - 1",
                "3 - 2",
            }));
        }

        [TestMethod]
        public void Concat3()
        {
            var res = EnumerableEx.Concat(
                new[] { 1, 2, 3 },
                new[] { 4, 5 }
            );

            Assert.IsTrue(Enumerable.SequenceEqual(res, new[] { 1, 2, 3, 4, 5 }));
        }

        [TestMethod]
        public void SelectMany_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.SelectMany<int, int>(null, new[] { 1 }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.SelectMany<int, int>(new[] { 1 }, null));
        }

        [TestMethod]
        public void SelectMany()
        {
            var res = new[] { 1, 2 }.SelectMany(new[] { 'a', 'b', 'c' }).ToList();
            Assert.IsTrue(Enumerable.SequenceEqual(res, new[] { 'a', 'b', 'c', 'a', 'b', 'c' }));
        }
    }
}
