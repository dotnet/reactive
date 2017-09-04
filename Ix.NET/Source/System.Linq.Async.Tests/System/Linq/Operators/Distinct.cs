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
    public class Distinct : AsyncEnumerableTests
    {
        [Fact]
        public void Distinct_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Distinct<int>(default(IAsyncEnumerable<int>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Distinct<int>(default(IAsyncEnumerable<int>), EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Distinct<int>(Return42, default(IEqualityComparer<int>)));
        }

        [Fact]
        public void Distinct1()
        {
            var xs = new[] { 1, 2, 1, 3, 5, 2, 1, 4 }.ToAsyncEnumerable().Distinct();

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 5);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void Distinct2()
        {
            var xs = new[] { 1, -2, -1, 3, 5, 2, 1, 4 }.ToAsyncEnumerable().Distinct(new Eq());

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, -2);
            HasNext(e, 3);
            HasNext(e, 5);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public async Task Distinct3()
        {
            var xs = new[] { 1, 2, 1, 3, 5, 2, 1, 4 }.ToAsyncEnumerable().Distinct();

            var res = new[] { 1, 2, 3, 5, 4 };
            Assert.True(res.SequenceEqual(await xs.ToArray()));
        }

        [Fact]
        public async Task Distinct4()
        {
            var xs = new[] { 1, 2, 1, 3, 5, 2, 1, 4 }.ToAsyncEnumerable().Distinct();

            var res = new List<int> { 1, 2, 3, 5, 4 };
            Assert.True(res.SequenceEqual(await xs.ToList()));
        }

        [Fact]
        public async Task Distinct5()
        {
            var xs = new[] { 1, 2, 1, 3, 5, 2, 1, 4 }.ToAsyncEnumerable().Distinct();

            Assert.Equal(5, await xs.Count());
        }

        [Fact]
        public async Task Distinct10()
        {
            var xs = new[] { 1, 2, 1, 3, 5, 2, 1, 4 }.ToAsyncEnumerable().Distinct();

            await SequenceIdentity(xs);
        }

        [Fact]
        public void Distinct12()
        {
            var xs = AsyncEnumerable.Empty<int>().Distinct();

            var e = xs.GetAsyncEnumerator();

            NoNext(e);
        }

        private sealed class Eq : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return EqualityComparer<int>.Default.Equals(Math.Abs(x), Math.Abs(y));
            }

            public int GetHashCode(int obj)
            {
                return EqualityComparer<int>.Default.GetHashCode(Math.Abs(obj));
            }
        }
    }
}
