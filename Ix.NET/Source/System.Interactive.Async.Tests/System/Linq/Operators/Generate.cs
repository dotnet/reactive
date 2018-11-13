// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class Generate : AsyncEnumerableExTests
    {
        [Fact]
        public void Generate_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Generate<int, int>(0, default, x => x, x => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Generate<int, int>(0, x => true, default, x => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Generate<int, int>(0, x => true, x => x, default));
        }

        [Fact]
        public async Task Generate1()
        {
            var xs = AsyncEnumerableEx.Generate(0, x => x < 5, x => x + 1, x => x * x);

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 0);
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 9);
            await HasNextAsync(e, 16);
            await NoNextAsync(e);
            await e.DisposeAsync();
        }

        [Fact]
        public async Task Generate2Async()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerableEx.Generate(0, x => { throw ex; }, x => x + 1, x => x * x);

            var e = xs.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Generate3Async()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerableEx.Generate(0, x => true, x => x + 1, x => { if (x == 1) throw ex; return x * x; });

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 0);
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Generate4Async()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerableEx.Generate(0, x => true, x => { throw ex; }, x => x * x);

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 0);
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Generate5()
        {
            var xs = AsyncEnumerableEx.Generate(0, x => x < 5, x => x + 1, x => x * x);

            await SequenceIdentity(xs);
        }
    }
}
