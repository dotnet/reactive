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
    public class ElementAt : AsyncEnumerableTests
    {
        [Fact]
        public async Task ElementAt_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ElementAt<int>(default, 0));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => AsyncEnumerable.ElementAt<int>(Return42, -1));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ElementAt<int>(default, 0, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => AsyncEnumerable.ElementAt<int>(Return42, -1, CancellationToken.None));
        }

        [Fact]
        public void ElementAt1()
        {
            var res = AsyncEnumerable.Empty<int>().ElementAt(0);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is ArgumentOutOfRangeException);
        }

        [Fact]
        public void ElementAt2()
        {
            var res = Return42.ElementAt(0);
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void ElementAt3()
        {
            var res = Return42.ElementAt(1);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is ArgumentOutOfRangeException);
        }

        [Fact]
        public void ElementAt4()
        {
            var res = new[] { 1, 42, 3 }.ToAsyncEnumerable().ElementAt(1);
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void ElementAt5()
        {
            var res = new[] { 1, 42, 3 }.ToAsyncEnumerable().ElementAt(7);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is ArgumentOutOfRangeException);
        }

        [Fact]
        public void ElementAt6()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).ElementAt(15);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
        }
    }
}
