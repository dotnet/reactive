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
    public class Single : AsyncEnumerableTests
    {
        [Fact]
        public async Task Single_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Single<int>(default(IAsyncEnumerable<int>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Single<int>(default(IAsyncEnumerable<int>), x => true));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Single<int>(Return42, default(Func<int, bool>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Single<int>(default(IAsyncEnumerable<int>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Single<int>(default(IAsyncEnumerable<int>), x => true, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Single<int>(Return42, default(Func<int, bool>), CancellationToken.None));
        }

        [Fact]
        public void Single1()
        {
            var res = AsyncEnumerable.Empty<int>().Single();
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void Single2()
        {
            var res = AsyncEnumerable.Empty<int>().Single(x => true);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void Single3()
        {
            var res = Return42.Single(x => x % 2 != 0);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void Single4()
        {
            var res = Return42.Single();
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void Single5()
        {
            var res = Return42.Single(x => x % 2 == 0);
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void Single6()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).Single();
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Single7()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).Single(x => true);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Single8()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().Single();
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void Single9()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().Single(x => x % 2 != 0);
            Assert.Equal(45, res.Result);
        }

        [Fact]
        public void Single10()
        {
            var res = new[] { 42, 23, 45, 90 }.ToAsyncEnumerable().Single(x => x % 2 != 0);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void Single11()
        {
            var res = new int[0].ToAsyncEnumerable().Single();
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }
    }
}
