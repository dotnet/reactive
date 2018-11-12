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
    public class ToDictionary : AsyncEnumerableTests
    {
        [Fact]
        public async Task ToDictionary_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int>(default, x => 0));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary(Return42, default(Func<int, int>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int>(default, x => 0, EqualityComparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary(Return42, default(Func<int, int>), EqualityComparer<int>.Default));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(default, x => 0, x => 0));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(Return42, default, x => 0));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(Return42, x => 0, default));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(default, x => 0, x => 0, EqualityComparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary(Return42, default, x => 0, EqualityComparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(Return42, x => 0, default, EqualityComparer<int>.Default));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int>(default, x => 0, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary(Return42, default(Func<int, int>), CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int>(default, x => 0, EqualityComparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary(Return42, default(Func<int, int>), EqualityComparer<int>.Default, CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(default, x => 0, x => 0, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(Return42, default, x => 0, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(Return42, x => 0, default, CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(default, x => 0, x => 0, EqualityComparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary(Return42, default, x => 0, EqualityComparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(Return42, x => 0, default, EqualityComparer<int>.Default, CancellationToken.None));
        }

        [Fact]
        public void ToDictionary1()
        {
            var xs = new[] { 1, 4 }.ToAsyncEnumerable();
            var res = xs.ToDictionary(x => x % 2).Result;
            Assert.True(res[0] == 4);
            Assert.True(res[1] == 1);
        }

        [Fact]
        public void ToDictionary2()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            AssertThrows<Exception>(() => xs.ToDictionary(x => x % 2).Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is ArgumentException);
        }

        [Fact]
        public void ToDictionary3()
        {
            var xs = new[] { 1, 4 }.ToAsyncEnumerable();
            var res = xs.ToDictionary(x => x % 2, x => x + 1).Result;
            Assert.True(res[0] == 5);
            Assert.True(res[1] == 2);
        }

        [Fact]
        public void ToDictionary4()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            AssertThrows<Exception>(() => xs.ToDictionary(x => x % 2, x => x + 1).Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is ArgumentException);
        }

        [Fact]
        public void ToDictionary5()
        {
            var xs = new[] { 1, 4 }.ToAsyncEnumerable();
            var res = xs.ToDictionary(x => x % 2, new Eq()).Result;
            Assert.True(res[0] == 4);
            Assert.True(res[1] == 1);
        }

        [Fact]
        public void ToDictionary6()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            AssertThrows<Exception>(() => xs.ToDictionary(x => x % 2, new Eq()).Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is ArgumentException);
        }

        [Fact]
        public void ToDictionary7()
        {
            var xs = new[] { 1, 4 }.ToAsyncEnumerable();
            var res = xs.ToDictionary(x => x % 2, x => x, new Eq()).Result;
            Assert.True(res[0] == 4);
            Assert.True(res[1] == 1);
        }

        private sealed class Eq : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return EqualityComparer<int>.Default.Equals(Math.Abs(x), Math.Abs(y));
            }

            public int GetHashCode(int obj)
            {
                return EqualityComparer<int>.Default.GetHashCode(Math.Abs(obj));
            }
        }
    }
}
