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
    public class Reverse : AsyncEnumerableTests
    {
        [Fact]
        public void Reverse_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Reverse<int>(default(IAsyncEnumerable<int>)));
        }

        [Fact]
        public void Reverse1()
        {
            var xs = AsyncEnumerable.Empty<int>();
            var ys = xs.Reverse();

            var e = ys.GetAsyncEnumerator();
            NoNext(e);
        }

        [Fact]
        public void Reverse2()
        {
            var xs = Return42;
            var ys = xs.Reverse();

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 42);
            NoNext(e);
        }

        [Fact]
        public void Reverse3()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.Reverse();

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 3);
            HasNext(e, 2);
            HasNext(e, 1);
            NoNext(e);
        }

        [Fact]
        public void Reverse4()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex);
            var ys = xs.Reverse();

            var e = ys.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
        }

        [Fact]
        public async Task Reverse5()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.Reverse();

            Assert.Equal(new[] { 3, 2, 1 }, await ys.ToArray());
        }

        [Fact]
        public async Task Reverse6()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.Reverse();

            Assert.Equal(new[] { 3, 2, 1 }, await ys.ToList());
        }

        [Fact]
        public async Task Reverse7()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.Reverse();

            Assert.Equal(3, await ys.Count());
        }

        [Fact]
        public async Task Reverse8()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.Reverse();

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task Reverse9()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.Reverse().Prepend(4); // to trigger onlyIfCheap

            Assert.Equal(new[] { 4, 3, 2, 1 }, await ys.ToArray());
        }
    }
}
