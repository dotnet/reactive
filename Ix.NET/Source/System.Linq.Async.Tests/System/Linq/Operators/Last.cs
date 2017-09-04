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
    public class Last : AsyncEnumerableTests
    {
        [Fact]
        public async Task Last_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Last<int>(default(IAsyncEnumerable<int>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Last<int>(default(IAsyncEnumerable<int>), x => true));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Last<int>(Return42, default(Func<int, bool>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Last<int>(default(IAsyncEnumerable<int>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Last<int>(default(IAsyncEnumerable<int>), x => true, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Last<int>(Return42, default(Func<int, bool>), CancellationToken.None));
        }

        [Fact]
        public void Last1()
        {
            var res = AsyncEnumerable.Empty<int>().Last();
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void Last2()
        {
            var res = AsyncEnumerable.Empty<int>().Last(x => true);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void Last3()
        {
            var res = Return42.Last(x => x % 2 != 0);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void Last4()
        {
            var res = Return42.Last();
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void Last5()
        {
            var res = Return42.Last(x => x % 2 == 0);
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void Last6()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).Last();
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Last7()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).Last(x => true);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Last8()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().Last();
            Assert.Equal(90, res.Result);
        }

        [Fact]
        public void Last9()
        {
            var res = new[] { 42, 23, 45, 90 }.ToAsyncEnumerable().Last(x => x % 2 != 0);
            Assert.Equal(45, res.Result);
        }
    }
}
