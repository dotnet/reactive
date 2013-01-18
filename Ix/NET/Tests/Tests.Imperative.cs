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
        public void While_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.While<int>(null, new[] { 1 }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.While<int>(() => true, null));
        }

        [TestMethod]
        public void While1()
        {
            var x = 5;
            var res = EnumerableEx.While(() => x > 0, EnumerableEx.Defer(() => new[] { x }).Do(_ => x--)).ToList();
            Assert.IsTrue(Enumerable.SequenceEqual(res, new[] { 5, 4, 3, 2, 1 }));
        }

        [TestMethod]
        public void While2()
        {
            var x = 0;
            var res = EnumerableEx.While(() => x > 0, EnumerableEx.Defer(() => new[] { x }).Do(_ => x--)).ToList();
            Assert.IsTrue(Enumerable.SequenceEqual(res, new int[0]));
        }

        [TestMethod]
        public void DoWhile_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.DoWhile<int>(new[] { 1 }, null));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.DoWhile<int>(null, () => true));
        }

        [TestMethod]
        public void DoWhile1()
        {
            var x = 5;
            var res = EnumerableEx.DoWhile(EnumerableEx.Defer(() => new[] { x }).Do(_ => x--), () => x > 0).ToList();
            Assert.IsTrue(Enumerable.SequenceEqual(res, new[] { 5, 4, 3, 2, 1 }));
        }

        [TestMethod]
        public void DoWhile2()
        {
            var x = 0;
            var res = EnumerableEx.DoWhile(EnumerableEx.Defer(() => new[] { x }).Do(_ => x--), () => x > 0).ToList();
            Assert.IsTrue(Enumerable.SequenceEqual(res, new[] { 0 }));
        }

        [TestMethod]
        public void If_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.If<int>(null, new[] { 1 }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.If<int>(() => true, null));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.If<int>(null, new[] { 1 }, new[] { 1 }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.If<int>(() => true, null, new[] { 1 }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.If<int>(() => true, new[] { 1 }, null));
        }

        [TestMethod]
        public void If1()
        {
            var x = 5;
            var res = EnumerableEx.If(() => x > 0, new[] { +1 }, new[] { -1 });

            Assert.AreEqual(+1, res.Single());

            x = -x;
            Assert.AreEqual(-1, res.Single());
        }

        [TestMethod]
        public void If2()
        {
            var x = 5;
            var res = EnumerableEx.If(() => x > 0, new[] { +1 });

            Assert.AreEqual(+1, res.Single());

            x = -x;
            Assert.IsTrue(res.IsEmpty());
        }

        [TestMethod]
        public void Case_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Case<int, int>(null, new Dictionary<int, IEnumerable<int>>()));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Case<int, int>(() => 1, null));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Case<int, int>(null, new Dictionary<int, IEnumerable<int>>(), new[] { 1 }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Case<int, int>(() => 1, null, new[] { 1 }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Case<int, int>(() => 1, new Dictionary<int, IEnumerable<int>>(), null));
        }

        [TestMethod]
        public void Case1()
        {
            var x = 1;
            var d = 'd';
            var res = EnumerableEx.Case<int, char>(() => x, new Dictionary<int, IEnumerable<char>>
            {
                { 0, new[] { 'a' } },
                { 1, new[] { 'b' } },
                { 2, new[] { 'c' } },
                { 3, EnumerableEx.Defer(() => new[] { d }) },
            });

            Assert.AreEqual('b', res.Single());
            Assert.AreEqual('b', res.Single());

            x = 0;
            Assert.AreEqual('a', res.Single());

            x = 2;
            Assert.AreEqual('c', res.Single());

            x = 3;
            Assert.AreEqual('d', res.Single());

            d = 'e';
            Assert.AreEqual('e', res.Single());

            x = 4;
            Assert.IsTrue(res.IsEmpty());
        }

        [TestMethod]
        public void Case2()
        {
            var x = 1;
            var d = 'd';
            var res = EnumerableEx.Case<int, char>(() => x, new Dictionary<int, IEnumerable<char>>
            {
                { 0, new[] { 'a' } },
                { 1, new[] { 'b' } },
                { 2, new[] { 'c' } },
                { 3, EnumerableEx.Defer(() => new[] { d }) },
            }, new[] { 'z' });

            Assert.AreEqual('b', res.Single());
            Assert.AreEqual('b', res.Single());

            x = 0;
            Assert.AreEqual('a', res.Single());

            x = 2;
            Assert.AreEqual('c', res.Single());

            x = 3;
            Assert.AreEqual('d', res.Single());

            d = 'e';
            Assert.AreEqual('e', res.Single());

            x = 4;
            Assert.AreEqual('z', res.Single());
        }

        [TestMethod]
        public void For_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.For<int, int>(null, x => new[] { 1 }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.For<int, int>(new[] { 1 }, null));
        }

        [TestMethod]
        public void For()
        {
            var res = EnumerableEx.For(new[] { 1, 2, 3 }, x => Enumerable.Range(0, x)).ToList();
            Assert.IsTrue(res.SequenceEqual(new[] { 0, 0, 1, 0, 1, 2 }));
        }
    }
}
