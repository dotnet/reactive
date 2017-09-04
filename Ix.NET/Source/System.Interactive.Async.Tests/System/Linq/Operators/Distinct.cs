// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class Distinct : AsyncEnumerableExTests
    {
        [Fact]
        public void Distinct_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Distinct<int, int>(Return42, default(Func<int, int>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Distinct(default(IAsyncEnumerable<int>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Distinct(Return42, default(Func<int, int>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Distinct(default(IAsyncEnumerable<int>), x => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Distinct(Return42, default(Func<int, int>), EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Distinct(Return42, x => x, default(IEqualityComparer<int>)));
        }

        [Fact]
        public void Distinct1()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable().Distinct(x => x / 2);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public async Task Distinct6()
        {
            var xs = new[] { 1, 2, 1, 3, 5, 2, 1, 4 }.ToAsyncEnumerable().Distinct(k => k);

            var res = new[] { 1, 2, 3, 5, 4 };
            Assert.True(res.SequenceEqual(await xs.ToArray()));
        }

        [Fact]
        public async Task Distinct7()
        {
            var xs = new[] { 1, 2, 1, 3, 5, 2, 1, 4 }.ToAsyncEnumerable().Distinct(k => k);

            var res = new List<int> { 1, 2, 3, 5, 4 };
            Assert.True(res.SequenceEqual(await xs.ToList()));
        }

        [Fact]
        public async Task Distinct8()
        {
            var xs = new[] { 1, 2, 1, 3, 5, 2, 1, 4 }.ToAsyncEnumerable().Distinct(k => k);

            Assert.Equal(5, await xs.Count());
        }

        [Fact]
        public async Task Distinct9()
        {
            var xs = new[] { 1, 2, 1, 3, 5, 2, 1, 4 }.ToAsyncEnumerable().Distinct(k => k);

            await SequenceIdentity(xs);
        }

        [Fact]
        public void Distinct11()
        {
            var xs = AsyncEnumerable.Empty<int>().Distinct(k => k);

            var e = xs.GetAsyncEnumerator();

            NoNext(e);
        }
    }
}
