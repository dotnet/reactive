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
    public class Intersect : AsyncEnumerableTests
    {
        [Fact]
        public void Intersect_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Intersect(default, Return42));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Intersect(Return42, default));

            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Intersect(default, Return42, new Eq()));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Intersect(Return42, default, new Eq()));
        }

        [Fact]
        public async Task Intersect_Simple()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, 1, 4 }.ToAsyncEnumerable();
            var res = xs.Intersect(ys);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 3);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Intersect_EqualityComparer()
        {
            var xs = new[] { 1, 2, -3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, -1, 4 }.ToAsyncEnumerable();
            var res = xs.Intersect(ys, new Eq());

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, -3);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Intersect_SequenceIdentity()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, 1, 4 }.ToAsyncEnumerable();
            var res = xs.Intersect(ys);

            await SequenceIdentity(res);
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
