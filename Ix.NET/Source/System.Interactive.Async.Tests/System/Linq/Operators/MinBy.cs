// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class MinBy : AsyncEnumerableExTests
    {
        [Fact]
        public async Task MinBy_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinBy(default(IAsyncEnumerable<int>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinBy(Return42, default(Func<int, int>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinBy(default(IAsyncEnumerable<int>), x => x, Comparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinBy(Return42, default(Func<int, int>), Comparer<int>.Default));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinBy(default(IAsyncEnumerable<int>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinBy(Return42, default(Func<int, int>), CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinBy(default(IAsyncEnumerable<int>), x => x, Comparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinBy(Return42, default(Func<int, int>), Comparer<int>.Default, CancellationToken.None));
        }

        [Fact]
        public void MinBy1()
        {
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().MinBy(x => x / 2);
            var res = xs.Result;

            Assert.True(res.SequenceEqual(new[] { 3, 2 }));
        }

        [Fact]
        public void MinBy2()
        {
            var xs = new int[0].ToAsyncEnumerable().MinBy(x => x / 2);

            AssertThrows<Exception>(() => xs.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void MinBy3()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().MinBy(x => { if (x == 3) throw ex; return x; });

            AssertThrows<Exception>(() => xs.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void MinBy4()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().MinBy(x => { if (x == 4) throw ex; return x; });

            AssertThrows<Exception>(() => xs.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void MinBy5()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().Concat(Throw<int>(ex)).MinBy(x => x, Comparer<int>.Default);

            AssertThrows<Exception>(() => xs.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }
    }
}
