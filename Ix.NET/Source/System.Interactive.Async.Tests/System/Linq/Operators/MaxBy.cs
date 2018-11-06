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
    public class MaxBy : AsyncEnumerableExTests
    {
        [Fact]
        public async Task MaxBy_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MaxBy(default(IAsyncEnumerable<int>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MaxBy(Return42, default(Func<int, int>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MaxBy(default(IAsyncEnumerable<int>), x => x, Comparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MaxBy(Return42, default(Func<int, int>), Comparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MaxBy(Return42, x => x, default(IComparer<int>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MaxBy(default(IAsyncEnumerable<int>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MaxBy(Return42, default(Func<int, int>), CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MaxBy(default(IAsyncEnumerable<int>), x => x, Comparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MaxBy(Return42, default(Func<int, int>), Comparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MaxBy(Return42, x => x, default, CancellationToken.None));
        }

        [Fact]
        public void MaxBy1()
        {
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().MaxBy(x => x / 2);
            var res = xs.Result;

            Assert.True(res.SequenceEqual(new[] { 7, 6 }));
        }

        [Fact]
        public void MaxBy2()
        {
            var xs = new int[0].ToAsyncEnumerable().MaxBy(x => x / 2);

            AssertThrows<Exception>(() => xs.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void MaxBy3()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().MaxBy(x => { if (x == 3) throw ex; return x; });

            AssertThrows<Exception>(() => xs.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void MaxBy4()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().MaxBy(x => { if (x == 4) throw ex; return x; });

            AssertThrows<Exception>(() => xs.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void MaxBy5()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().Concat(Throw<int>(ex)).MaxBy(x => x, Comparer<int>.Default);

            AssertThrows<Exception>(() => xs.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }
    }
}
