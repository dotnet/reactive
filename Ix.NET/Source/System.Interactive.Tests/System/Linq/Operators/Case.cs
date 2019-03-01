// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Case : Tests
    {
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
    }
}
