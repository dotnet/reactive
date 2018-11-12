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
    public class Except : AsyncEnumerableTests
    {
        [Fact]
        public void Except_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Except<int>(default, Return42));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Except<int>(Return42, default));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Except<int>(default, Return42, new Eq()));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Except<int>(Return42, null, new Eq()));
        }

        [Fact]
        public void Except1()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, 1, 4 }.ToAsyncEnumerable();
            var res = xs.Except(ys);

            var e = res.GetAsyncEnumerator();
            HasNext(e, 2);
            NoNext(e);
        }

        [Fact]
        public void Except2()
        {
            var xs = new[] { 1, 2, -3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, -1, 4 }.ToAsyncEnumerable();
            var res = xs.Except(ys, new Eq());

            var e = res.GetAsyncEnumerator();
            HasNext(e, 2);
            NoNext(e);
        }

        [Fact]
        public async Task Except3()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, 1, 4 }.ToAsyncEnumerable();
            var res = xs.Except(ys);

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
