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
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Buffer(default(IAsyncEnumerable<int>), 1));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Buffer(default(IAsyncEnumerable<int>), 1, 1));

            AssertThrows<ArgumentOutOfRangeException>(() => AsyncEnumerableEx.Buffer(AsyncEnumerable.Return(42), -1));
            AssertThrows<ArgumentOutOfRangeException>(() => AsyncEnumerableEx.Buffer(AsyncEnumerable.Return(42), -1, 1));
            AssertThrows<ArgumentOutOfRangeException>(() => AsyncEnumerableEx.Buffer(AsyncEnumerable.Return(42), 1, -1));
        }

        [Fact]
        public void Buffer1()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable().Buffer(2);

            var e = xs.GetAsyncEnumerator();

            Assert.True(e.MoveNextAsync().Result);
            Assert.True(e.Current.SequenceEqual(new[] { 1, 2 }));

            Assert.True(e.MoveNextAsync().Result);
            Assert.True(e.Current.SequenceEqual(new[] { 3, 4 }));

            Assert.True(e.MoveNextAsync().Result);
            Assert.True(e.Current.SequenceEqual(new[] { 5 }));

            Assert.False(e.MoveNextAsync().Result);
        }

        [Fact]
        public void Buffer2()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable().Buffer(3, 2);

            var e = xs.GetAsyncEnumerator();

            Assert.True(e.MoveNextAsync().Result);
            Assert.True(e.Current.SequenceEqual(new[] { 1, 2, 3 }));

            Assert.True(e.MoveNextAsync().Result);
            Assert.True(e.Current.SequenceEqual(new[] { 3, 4, 5 }));

            Assert.True(e.MoveNextAsync().Result);
            Assert.True(e.Current.SequenceEqual(new[] { 5 }));

            Assert.False(e.MoveNextAsync().Result);
        }

        [Fact]
        public void Buffer3()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable().Buffer(2, 3);

            var e = xs.GetAsyncEnumerator();

            Assert.True(e.MoveNextAsync().Result);
            Assert.True(e.Current.SequenceEqual(new[] { 1, 2 }));

            Assert.True(e.MoveNextAsync().Result);
            Assert.True(e.Current.SequenceEqual(new[] { 4, 5 }));

            Assert.False(e.MoveNextAsync().Result);
        }

        [Fact]
        public async Task Buffer4()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable().Buffer(3, 2);

            await SequenceIdentity(xs);
        }
    }
}
