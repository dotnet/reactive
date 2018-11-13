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
    public class Concat : AsyncEnumerableExTests
    {
        [Fact]
        public void Concat_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Concat<int>(default(IAsyncEnumerable<int>[])));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Concat<int>(default(IEnumerable<IAsyncEnumerable<int>>)));
        }

        [Fact]
        public void Concat4()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5 }.ToAsyncEnumerable();
            var zs = new[] { 6, 7, 8 }.ToAsyncEnumerable();

            var res = AsyncEnumerableEx.Concat(xs, ys, zs);

            var e = res.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            HasNext(e, 5);
            HasNext(e, 6);
            HasNext(e, 7);
            HasNext(e, 8);
            NoNext(e);
        }

        [Fact]
        public void Concat5()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5 }.ToAsyncEnumerable();
            var zs = Throw<int>(ex);

            var res = AsyncEnumerableEx.Concat(xs, ys, zs);

            var e = res.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            HasNext(e, 5);
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
        }

        [Fact]
        public void Concat6()
        {
            var res = AsyncEnumerableEx.Concat(ConcatXss());

            var e = res.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            HasNext(e, 5);
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single().Message == "Bang!");
        }

        [Fact]
        public async Task Concat9()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5 }.ToAsyncEnumerable();
            var zs = new[] { 6, 7, 8 }.ToAsyncEnumerable();

            var res = AsyncEnumerableEx.Concat(xs, ys, zs);

            await SequenceIdentity(res);
        }

        private static IEnumerable<IAsyncEnumerable<int>> ConcatXss()
        {
            yield return new[] { 1, 2, 3 }.ToAsyncEnumerable();
            yield return new[] { 4, 5 }.ToAsyncEnumerable();
            throw new Exception("Bang!");
        }
    }
}
