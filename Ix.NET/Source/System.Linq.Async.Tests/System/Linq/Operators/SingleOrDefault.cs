// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class SingleOrDefault : AsyncEnumerableTests
    {
        [Fact]
        public async Task SingleOrDefault_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefault<int>(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefault<int>(null, x => true));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefault<int>(AsyncEnumerable.Return(42), default(Func<int, bool>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefault<int>(null, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefault<int>(null, x => true, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefault<int>(AsyncEnumerable.Return(42), default(Func<int, bool>), CancellationToken.None));
        }

        [Fact]
        public void SingleOrDefault1()
        {
            var res = AsyncEnumerable.Empty<int>().SingleOrDefault();
            Assert.Equal(0, res.Result);
        }

        [Fact]
        public void SingleOrDefault2()
        {
            var res = AsyncEnumerable.Empty<int>().SingleOrDefault(x => true);
            Assert.Equal(0, res.Result);
        }

        [Fact]
        public void SingleOrDefault3()
        {
            var res = AsyncEnumerable.Return(42).SingleOrDefault(x => x % 2 != 0);
            Assert.Equal(0, res.Result);
        }

        [Fact]
        public void SingleOrDefault4()
        {
            var res = AsyncEnumerable.Return(42).SingleOrDefault();
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void SingleOrDefault5()
        {
            var res = AsyncEnumerable.Return(42).SingleOrDefault(x => x % 2 == 0);
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void SingleOrDefault6()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).SingleOrDefault();
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void SingleOrDefault7()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).SingleOrDefault(x => true);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void SingleOrDefault8()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleOrDefault();
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void SingleOrDefault9()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleOrDefault(x => x % 2 != 0);
            Assert.Equal(45, res.Result);
        }

        [Fact]
        public void SingleOrDefault10()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleOrDefault(x => x < 10);
            Assert.Equal(0, res.Result);
        }

        [Fact]
        public void SingleOrDefault11()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleOrDefault(x => true);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void SingleOrDefault12()
        {
            var res = new int[0].ToAsyncEnumerable().SingleOrDefault();
            Assert.Equal(0, res.Result);
        }
    }
}
