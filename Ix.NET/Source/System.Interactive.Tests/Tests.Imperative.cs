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

        [Fact]
        public void If_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.If<int>(null, new[] { 1 }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.If<int>(() => true, null));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.If<int>(null, new[] { 1 }, new[] { 1 }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.If<int>(() => true, null, new[] { 1 }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.If<int>(() => true, new[] { 1 }, null));
        }

        [Fact]
        public void If1()
        {
            var x = 5;
            var res = EnumerableEx.If(() => x > 0, new[] { +1 }, new[] { -1 });

            Assert.Equal(+1, res.Single());

            x = -x;
            Assert.Equal(-1, res.Single());
        }

        [Fact]
        public void If2()
        {
            var x = 5;
            var res = EnumerableEx.If(() => x > 0, new[] { +1 });

            Assert.Equal(+1, res.Single());

            x = -x;
            Assert.True(res.IsEmpty());
        }

        [Fact]
        public void Case_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Case<int, int>(null, new Dictionary<int, IEnumerable<int>>()));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Case<int, int>(() => 1, null));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Case<int, int>(null, new Dictionary<int, IEnumerable<int>>(), new[] { 1 }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Case<int, int>(() => 1, null, new[] { 1 }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Case<int, int>(() => 1, new Dictionary<int, IEnumerable<int>>(), null));
        }

        [Fact]
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

            Assert.Equal('b', res.Single());
            Assert.Equal('b', res.Single());

            x = 0;
            Assert.Equal('a', res.Single());

            x = 2;
            Assert.Equal('c', res.Single());

            x = 3;
            Assert.Equal('d', res.Single());

            d = 'e';
            Assert.Equal('e', res.Single());

            x = 4;
            Assert.True(res.IsEmpty());
        }

        [Fact]
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

            Assert.Equal('b', res.Single());
            Assert.Equal('b', res.Single());

            x = 0;
            Assert.Equal('a', res.Single());

            x = 2;
            Assert.Equal('c', res.Single());

            x = 3;
            Assert.Equal('d', res.Single());

            d = 'e';
            Assert.Equal('e', res.Single());

            x = 4;
            Assert.Equal('z', res.Single());
        }

        [Fact]
        public void For_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.For<int, int>(null, x => new[] { 1 }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.For<int, int>(new[] { 1 }, null));
        }

        [Fact]
        public void For()
        {
            var res = EnumerableEx.For(new[] { 1, 2, 3 }, x => Enumerable.Range(0, x)).ToList();
            Assert.True(res.SequenceEqual(new[] { 0, 0, 1, 0, 1, 2 }));
        }
    }
}
