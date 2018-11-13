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
    public class Expand : AsyncEnumerableExTests
    {
        [Fact]
        public void Expand_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Expand(default(IAsyncEnumerable<int>), x => default(IAsyncEnumerable<int>)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Expand(Return42, default(Func<int, IAsyncEnumerable<int>>)));
        }

        [Fact]
        public async Task Expand1Async()
        {
            var xs = new[] { 2, 3 }.ToAsyncEnumerable().Expand(x => AsyncEnumerableEx.Return(x - 1).Repeat(x - 1));

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 1);
            await NoNextAsync(e);
        }

        [Fact]
        public void Expand2()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 2, 3 }.ToAsyncEnumerable().Expand(new Func<int, IAsyncEnumerable<int>>(x => { throw ex; }));

            var e = xs.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
        }

        [Fact]
        public async Task Expand3Async()
        {
            var xs = new[] { 2, 3 }.ToAsyncEnumerable().Expand(x => default(IAsyncEnumerable<int>));

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is NullReferenceException);
        }

        [Fact]
        public async Task Expand4()
        {
            var xs = new[] { 2, 3 }.ToAsyncEnumerable().Expand(x => AsyncEnumerableEx.Return(x - 1).Repeat(x - 1));

            await SequenceIdentity(xs);
        }
    }
}
