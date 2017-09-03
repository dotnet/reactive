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
    public class ToList : AsyncEnumerableTests
    {
        [Fact]
        public async Task ToList_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToList<int>(default(IAsyncEnumerable<int>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToList<int>(default(IAsyncEnumerable<int>), CancellationToken.None));
        }

        [Fact]
        public void ToList1()
        {
            var xs = new[] { 42, 25, 39 };
            var res = xs.ToAsyncEnumerable().ToList();
            Assert.True(res.Result.SequenceEqual(xs));
        }

        [Fact]
        public void ToList2()
        {
            var xs = AsyncEnumerable.Empty<int>();
            var res = xs.ToList();
            Assert.True(res.Result.Count == 0);
        }

        [Fact]
        public void ToList3()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).ToList();
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }
    }
}
