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
    public class Finally : AsyncEnumerableExTests
    {
        [Fact]
        public void Finally_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Finally(default(IAsyncEnumerable<int>), () => { }));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Finally(Return42, default(Action)));
        }

        [Fact]
        public async Task Finally1Async()
        {
            var b = false;

            var xs = AsyncEnumerable.Empty<int>().Finally(() => { b = true; });

            var e = xs.GetAsyncEnumerator();

            Assert.False(b);
            await NoNextAsync(e);

            Assert.True(b);
        }

        [Fact]
        public async Task Finally2Async()
        {
            var b = false;

            var xs = Return42.Finally(() => { b = true; });

            var e = xs.GetAsyncEnumerator();

            Assert.False(b);
            await HasNextAsync(e, 42);

            Assert.False(b);
            await NoNextAsync(e);

            Assert.True(b);
        }

        [Fact]
        public async Task Finally3Async()
        {
            var ex = new Exception("Bang!");

            var b = false;

            var xs = Throw<int>(ex).Finally(() => { b = true; });

            var e = xs.GetAsyncEnumerator();

            Assert.False(b);
            await AssertThrowsAsync(e.MoveNextAsync(), ex);

            Assert.True(b);
        }

        [Fact]
        public async Task Finally4Async()
        {
            var b = false;

            var xs = new[] { 1, 2 }.ToAsyncEnumerable().Finally(() => { b = true; });

            var e = xs.GetAsyncEnumerator();

            Assert.False(b);
            await HasNextAsync(e, 1);

            Assert.False(b);
            await HasNextAsync(e, 2);

            Assert.False(b);
            await NoNextAsync(e);

            Assert.True(b);
        }

        [Fact]
        public async Task Finally5()
        {
            var b = false;

            var xs = new[] { 1, 2 }.ToAsyncEnumerable().Finally(() => { b = true; });

            var e = xs.GetAsyncEnumerator();

            Assert.False(b);
            await HasNextAsync(e, 1);

            await e.DisposeAsync();

            Assert.True(b);
        }

        [Fact]
        public async Task Finally7()
        {
            var i = 0;
            var xs = new[] { 1, 2 }.ToAsyncEnumerable().Finally(() => { i++; });

            await SequenceIdentity(xs);
            Assert.Equal(2, i);
        }
    }
}
