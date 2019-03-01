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
    public class Repeat : AsyncEnumerableExTests
    {
        [Fact]
        public async Task RepeatValue1()
        {
            var xs = AsyncEnumerableEx.Repeat(2);

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 2);
            await e.DisposeAsync();
        }

        [Fact]
        public async Task RepeatValue2()
        {
            var xs = AsyncEnumerableEx.Repeat(2).Take(5);

            await SequenceIdentity(xs);
        }

        [Fact]
        public void RepeatSequence_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Repeat(default(IAsyncEnumerable<int>)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Repeat(default(IAsyncEnumerable<int>), 3));
            Assert.Throws<ArgumentOutOfRangeException>(() => AsyncEnumerableEx.Repeat(Return42, -1));
        }

        [Fact]
        public async Task RepeatSequence1Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Repeat();

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
        }

        [Fact]
        public async Task RepeatSequence2Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Repeat(3);

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task RepeatSequence3Async()
        {
            var i = 0;
            var xs = RepeatXs(() => i++).ToAsyncEnumerable().Repeat(3);

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await NoNextAsync(e);

            Assert.Equal(3, i);
        }

        [Fact]
        public async Task RepeatSequence4Async()
        {
            var i = 0;
            var xs = RepeatXs(() => i++).ToAsyncEnumerable().Repeat(0);

            var e = xs.GetAsyncEnumerator();

            await NoNextAsync(e);
        }

        [Fact]
        public async Task RepeatSequence5()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Repeat(3);

            await SequenceIdentity(xs);
        }

        [Fact]
        public async Task RepeatSequence6Async()
        {
            var xs = new FailRepeat().ToAsyncEnumerable().Repeat();

            var e = xs.GetAsyncEnumerator();
            await AssertThrowsAsync<NotImplementedException>(e.MoveNextAsync().AsTask());
        }

        [Fact]
        public async Task RepeatSequence7Async()
        {
            var xs = new FailRepeat().ToAsyncEnumerable().Repeat(3);

            var e = xs.GetAsyncEnumerator();
            await AssertThrowsAsync<NotImplementedException>(e.MoveNextAsync().AsTask());
        }

        private static IEnumerable<int> RepeatXs(Action started)
        {
            started();

            yield return 1;
            yield return 2;
        }

        private sealed class FailRepeat : IEnumerable<int>
        {
            public IEnumerator<int> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
    }
}
