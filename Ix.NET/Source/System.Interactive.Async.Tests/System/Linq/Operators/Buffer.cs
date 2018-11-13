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
    public class Buffer : AsyncEnumerableExTests
    {
        [Fact]
        public void Buffer_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Buffer(default(IAsyncEnumerable<int>), 1));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Buffer(default(IAsyncEnumerable<int>), 1, 1));

            Assert.Throws<ArgumentOutOfRangeException>(() => AsyncEnumerableEx.Buffer(Return42, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => AsyncEnumerableEx.Buffer(Return42, -1, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => AsyncEnumerableEx.Buffer(Return42, 1, -1));
        }

        [Fact]
        public async Task Buffer1Async()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable().Buffer(2);

            var e = xs.GetAsyncEnumerator();

            Assert.True(await e.MoveNextAsync());
            Assert.True(e.Current.SequenceEqual(new[] { 1, 2 }));

            Assert.True(await e.MoveNextAsync());
            Assert.True(e.Current.SequenceEqual(new[] { 3, 4 }));

            Assert.True(await e.MoveNextAsync());
            Assert.True(e.Current.SequenceEqual(new[] { 5 }));

            Assert.False(await e.MoveNextAsync());
        }

        [Fact]
        public async Task Buffer2Async()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable().Buffer(3, 2);

            var e = xs.GetAsyncEnumerator();

            Assert.True(await e.MoveNextAsync());
            Assert.True(e.Current.SequenceEqual(new[] { 1, 2, 3 }));

            Assert.True(await e.MoveNextAsync());
            Assert.True(e.Current.SequenceEqual(new[] { 3, 4, 5 }));

            Assert.True(await e.MoveNextAsync());
            Assert.True(e.Current.SequenceEqual(new[] { 5 }));

            Assert.False(await e.MoveNextAsync());
        }

        [Fact]
        public async Task Buffer3Async()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable().Buffer(2, 3);

            var e = xs.GetAsyncEnumerator();

            Assert.True(await e.MoveNextAsync());
            Assert.True(e.Current.SequenceEqual(new[] { 1, 2 }));

            Assert.True(await e.MoveNextAsync());
            Assert.True(e.Current.SequenceEqual(new[] { 4, 5 }));

            Assert.False(await e.MoveNextAsync());
        }

        [Fact]
        public async Task Buffer4()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable().Buffer(3, 2);

            await SequenceIdentity(xs);
        }
    }
}
