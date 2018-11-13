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
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToList<int>(default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToList<int>(default, CancellationToken.None));
        }

        [Fact]
        public async Task ToList1()
        {
            var xs = new[] { 42, 25, 39 };
            var res = xs.ToAsyncEnumerable().ToList();
            Assert.True((await res).SequenceEqual(xs));
        }

        [Fact]
        public async Task ToList2()
        {
            var xs = AsyncEnumerable.Empty<int>();
            var res = xs.ToList();
            Assert.True((await res).Count == 0);
        }

        [Fact]
        public void ToList3()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).ToList();
            AssertThrowsAsync(res, ex);
        }
    }
}
