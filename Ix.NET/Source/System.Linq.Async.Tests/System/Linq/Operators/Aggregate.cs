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
    public class Aggregate : AsyncEnumerableTests
    {
        [Fact]
        public async Task Aggregate_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int>(null, (x, y) => x + y));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int>(AsyncEnumerable.Return(42), default(Func<int, int, int>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int, int>(null, 0, (x, y) => x + y));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int, int>(AsyncEnumerable.Return(42), 0, default(Func<int, int, int>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int, int, int>(null, 0, (x, y) => x + y, z => z));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int, int, int>(AsyncEnumerable.Return(42), 0, default(Func<int, int, int>), z => z));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int, int, int>(AsyncEnumerable.Return(42), 0, (x, y) => x + y, default(Func<int, int>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int>(null, (x, y) => x + y, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int>(AsyncEnumerable.Return(42), default(Func<int, int, int>), CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int, int>(null, 0, (x, y) => x + y, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int, int>(AsyncEnumerable.Return(42), 0, default(Func<int, int, int>), CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int, int, int>(null, 0, (x, y) => x + y, z => z, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int, int, int>(AsyncEnumerable.Return(42), 0, default(Func<int, int, int>), z => z, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int, int, int>(AsyncEnumerable.Return(42), 0, (x, y) => x + y, default(Func<int, int>), CancellationToken.None));
        }

        [Fact]
        public void Aggregate1()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Aggregate((x, y) => x * y);
            Assert.Equal(24, ys.Result);
        }

        [Fact]
        public void Aggregate2()
        {
            var xs = new int[0].ToAsyncEnumerable();
            var ys = xs.Aggregate((x, y) => x * y);
            AssertThrows<Exception>(() => ys.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void Aggregate3()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Throw<int>(ex);
            var ys = xs.Aggregate((x, y) => x * y);
            AssertThrows<Exception>(() => ys.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Aggregate4()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Aggregate(new Func<int, int, int>((x, y) => { throw ex; }));
            AssertThrows<Exception>(() => ys.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Aggregate5()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Aggregate(1, (x, y) => x * y);
            Assert.Equal(24, ys.Result);
        }

        [Fact]
        public void Aggregate6()
        {
            var xs = new int[0].ToAsyncEnumerable();
            var ys = xs.Aggregate(1, (x, y) => x * y);
            Assert.Equal(1, ys.Result);
        }

        [Fact]
        public void Aggregate7()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Throw<int>(ex);
            var ys = xs.Aggregate(1, (x, y) => x * y);
            AssertThrows<Exception>(() => ys.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Aggregate8()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Aggregate(1, new Func<int, int, int>((x, y) => { throw ex; }));
            AssertThrows<Exception>(() => ys.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Aggregate9()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Aggregate(1, (x, y) => x * y, x => x + 1);
            Assert.Equal(25, ys.Result);
        }

        [Fact]
        public void Aggregate10()
        {
            var xs = new int[0].ToAsyncEnumerable();
            var ys = xs.Aggregate(1, (x, y) => x * y, x => x + 1);
            Assert.Equal(2, ys.Result);
        }

        [Fact]
        public void Aggregate11()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Throw<int>(ex);
            var ys = xs.Aggregate(1, (x, y) => x * y, x => x + 1);
            AssertThrows<Exception>(() => ys.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Aggregate12()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Aggregate(1, (x, y) => { throw ex; }, x => x + 1);
            AssertThrows<Exception>(() => ys.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Aggregate13()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Aggregate<int, int, int>(1, (x, y) => x * y, x => { throw ex; });
            AssertThrows<Exception>(() => ys.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }
    }
}
