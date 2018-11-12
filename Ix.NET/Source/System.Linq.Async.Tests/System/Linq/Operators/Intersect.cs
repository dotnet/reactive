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
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Intersect<int>(default, Return42));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Intersect<int>(Return42, default));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Intersect<int>(default, Return42, new Eq()));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Intersect<int>(Return42, default, new Eq()));
        }

        [Fact]
        public void Intersect1()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, 1, 4 }.ToAsyncEnumerable();
            var res = xs.Intersect(ys);

            var e = res.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 3);
            NoNext(e);
        }

        [Fact]
        public void Intersect2()
        {
            var xs = new[] { 1, 2, -3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, -1, 4 }.ToAsyncEnumerable();
            var res = xs.Intersect(ys, new Eq());

            var e = res.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, -3);
            NoNext(e);
        }

        [Fact]
        public async Task Intersect3()
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
