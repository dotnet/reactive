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
    public class DistinctUntilChanged : AsyncEnumerableExTests
    {
        [Fact]
        public void DistinctUntilChanged_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.DistinctUntilChanged(default(IAsyncEnumerable<int>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.DistinctUntilChanged(default, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.DistinctUntilChanged(Return42, default));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.DistinctUntilChanged(default(IAsyncEnumerable<int>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.DistinctUntilChanged(Return42, default(Func<int, int>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.DistinctUntilChanged(default(IAsyncEnumerable<int>), x => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.DistinctUntilChanged(Return42, default(Func<int, int>), EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.DistinctUntilChanged(Return42, x => x, default));
        }

        [Fact]
        public void DistinctUntilChanged1()
        {
            var xs = new[] { 1, 2, 2, 3, 4, 4, 4, 4, 5, 6, 6, 7, 3, 2, 2, 1, 1 }.ToAsyncEnumerable().DistinctUntilChanged();

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            HasNext(e, 5);
            HasNext(e, 6);
            HasNext(e, 7);
            HasNext(e, 3);
            HasNext(e, 2);
            HasNext(e, 1);
            NoNext(e);
        }

        [Fact]
        public void DistinctUntilChanged2()
        {
            var xs = new[] { 1, 2, 3, 4, 3, 5, 2 }.ToAsyncEnumerable().DistinctUntilChanged(x => (x + 1) / 2);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 3);
            HasNext(e, 5);
            HasNext(e, 2);
            NoNext(e);
        }

        [Fact]
        public void DistinctUntilChanged3()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4, 3, 5, 2 }.ToAsyncEnumerable().DistinctUntilChanged(x => { if (x == 4) throw ex; return x; });

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
        }

        [Fact]
        public async Task DistinctUntilChanged4()
        {
            var xs = new[] { 1, 2, 2, 3, 4, 4, 4, 4, 5, 6, 6, 7, 3, 2, 2, 1, 1 }.ToAsyncEnumerable().DistinctUntilChanged();

            await SequenceIdentity(xs);
        }

        [Fact]
        public async Task DistinctUntilChanged5()
        {
            var xs = new[] { 1, 2, 3, 4, 3, 5, 2 }.ToAsyncEnumerable().DistinctUntilChanged(x => (x + 1) / 2);

            await SequenceIdentity(xs);
        }
    }
}
