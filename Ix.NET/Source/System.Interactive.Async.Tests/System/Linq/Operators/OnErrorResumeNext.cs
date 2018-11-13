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
    public class OnErrorResumeNext : AsyncEnumerableExTests
    {
        [Fact]
        public void OnErrorResumeNext_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.OnErrorResumeNext<int>(default, Return42));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.OnErrorResumeNext<int>(Return42, default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.OnErrorResumeNext<int>(default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.OnErrorResumeNext<int>(default(IEnumerable<IAsyncEnumerable<int>>)));
        }

        [Fact]
        public async Task OnErrorResumeNext7Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();

            var res = AsyncEnumerableEx.OnErrorResumeNext(xs, ys);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task OnErrorResumeNext8Async()
        {
            var ex = new InvalidOperationException("Bang!");

            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Concat(Throw<int>(ex));
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();

            var res = AsyncEnumerableEx.OnErrorResumeNext(xs, ys);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task OnErrorResumeNext9Async()
        {
            var ex = new InvalidOperationException("Bang!");

            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Concat(Throw<int>(ex));
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();

            var res = AsyncEnumerableEx.OnErrorResumeNext(new[] { xs, xs, ys, ys });

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 6);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task OnErrorResumeNext10Async()
        {
            var ex = new Exception("Bang!");
            var res = OnErrorResumeNextXss(ex).OnErrorResumeNext();

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);

            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        private IEnumerable<IAsyncEnumerable<int>> OnErrorResumeNextXss(Exception ex)
        {
            yield return new[] { 1, 2, 3 }.ToAsyncEnumerable().Concat(Throw<int>(new Exception("!!!")));
            throw ex;
        }

        [Fact]
        public async Task OnErrorResumeNext11Async()
        {
            var ex = new InvalidOperationException("Bang!");

            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Concat(Throw<int>(ex));

            var res = AsyncEnumerableEx.OnErrorResumeNext(new[] { xs, xs });

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task OnErrorResumeNext12Async()
        {
            var res = AsyncEnumerableEx.OnErrorResumeNext(Enumerable.Empty<IAsyncEnumerable<int>>());

            var e = res.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task OnErrorResumeNext13()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();

            var res = AsyncEnumerableEx.OnErrorResumeNext(xs, ys);

            await SequenceIdentity(res);
        }
    }
}
