// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Union : AsyncEnumerableTests
    {
        [Fact]
        public void Union_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Union(default, Return42));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Union(default, Return42, new Eq()));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Union(Return42, default, new Eq()));
        }

        [Fact]
        public void Union1()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, 1, 4 }.ToAsyncEnumerable();
            var res = xs.Union(ys);

            var e = res.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 5);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void Union2()
        {
            var xs = new[] { 1, 2, -3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, -1, 4 }.ToAsyncEnumerable();
            var res = xs.Union(ys, new Eq());

            var e = res.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, -3);
            HasNext(e, 5);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void Union3()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, 1, 4 }.ToAsyncEnumerable();
            var zs = new[] { 5, 7, 8, 1 }.ToAsyncEnumerable();
            var res = xs.Union(ys).Union(zs);

            var e = res.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 5);
            HasNext(e, 4);
            HasNext(e, 7);
            HasNext(e, 8);
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
