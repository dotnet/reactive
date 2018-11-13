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
    public class Select : AsyncEnumerableTests
    {
        [Fact]
        public void Select_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Select<int, int>(default, x => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Select<int, int>(default, (x, i) => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Select(Return42, default(Func<int, int>)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Select(Return42, default(Func<int, int, int>)));
        }

        [Fact]
        public async Task Select1()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = xs.Select(x => (char)('a' + x));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 'a');
            await HasNextAsync(e, 'b');
            await HasNextAsync(e, 'c');
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Select2()
        {
            var xs = new[] { 8, 5, 7 }.ToAsyncEnumerable();
            var ys = xs.Select((x, i) => (char)('a' + i));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 'a');
            await HasNextAsync(e, 'b');
            await HasNextAsync(e, 'c');
            await NoNextAsync(e);
        }

        [Fact]
        public void Select3()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = xs.Select(x => 1 / x);

            var e = ys.GetAsyncEnumerator();
            AssertThrowsAsync<DivideByZeroException>(e.MoveNextAsync().AsTask());
        }

        [Fact]
        public void Select4()
        {
            var xs = new[] { 8, 5, 7 }.ToAsyncEnumerable();
            var ys = xs.Select((x, i) => 1 / i);

            var e = ys.GetAsyncEnumerator();
            AssertThrowsAsync<DivideByZeroException>(e.MoveNextAsync().AsTask());
        }

        [Fact]
        public async Task Select5()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = xs.Select(i => i + 3).Select(x => (char)('a' + x));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 'd');
            await HasNextAsync(e, 'e');
            await HasNextAsync(e, 'f');
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Select7()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = xs.Select(x => (char)('a' + x));

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task Select8()
        {
            var xs = new[] { 8, 5, 7 }.ToAsyncEnumerable();
            var ys = xs.Select((x, i) => (char)('a' + i));

            await SequenceIdentity(ys);
        }
    }
}
