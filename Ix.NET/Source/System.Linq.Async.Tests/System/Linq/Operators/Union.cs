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
    public class Union : AsyncEnumerableTests
    {
        [Fact]
        public void Union_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Union(default, Return42));

            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Union(default, Return42, new Eq()));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Union(Return42, default, new Eq()));
        }

        [Fact]
        public async Task Union1()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, 1, 4 }.ToAsyncEnumerable();
            var res = xs.Union(ys);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Union2()
        {
            var xs = new[] { 1, 2, -3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, -1, 4 }.ToAsyncEnumerable();
            var res = xs.Union(ys, new Eq());

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, -3);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Union3()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, 1, 4 }.ToAsyncEnumerable();
            var zs = new[] { 5, 7, 8, 1 }.ToAsyncEnumerable();
            var res = xs.Union(ys).Union(zs);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 7);
            await HasNextAsync(e, 8);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Union_Concurrency()
        {
            var state = new SharedState();
            var xs = new[] { 1, 2, 3 }.ToSharedStateAsyncEnumerable(state);
            var ys = new[] { 3, 5, 1, 4 }.ToSharedStateAsyncEnumerable(state);
            var zs = new[] { 5, 7, 8, 1 }.ToSharedStateAsyncEnumerable(state);

            async Task f() => await xs.Union(ys).Union(zs).LastAsync();

            await f(); // Should not throw
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
