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
    public class ElementAtOrDefault : AsyncEnumerableTests
    {
        [Fact]
        public async Task ElementAtOrDefault_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ElementAtOrDefault<int>(default(IAsyncEnumerable<int>), 0));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => AsyncEnumerable.ElementAtOrDefault<int>(Return42, -1));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ElementAtOrDefault<int>(default(IAsyncEnumerable<int>), 0, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => AsyncEnumerable.ElementAtOrDefault<int>(Return42, -1, CancellationToken.None));
        }

        [Fact]
        public void ElementAtOrDefault1()
        {
            var res = AsyncEnumerable.Empty<int>().ElementAtOrDefault(0);
            Assert.Equal(0, res.Result);
        }

        [Fact]
        public void ElementAtOrDefault2()
        {
            var res = Return42.ElementAtOrDefault(0);
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void ElementAtOrDefault3()
        {
            var res = Return42.ElementAtOrDefault(1);
            Assert.Equal(0, res.Result);
        }

        [Fact]
        public void ElementAtOrDefault4()
        {
            var res = new[] { 1, 42, 3 }.ToAsyncEnumerable().ElementAtOrDefault(1);
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void ElementAtOrDefault5()
        {
            var res = new[] { 1, 42, 3 }.ToAsyncEnumerable().ElementAtOrDefault(7);
            Assert.Equal(0, res.Result);
        }

        [Fact]
        public void ElementAtOrDefault6()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).ElementAtOrDefault(15);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }
    }
}
