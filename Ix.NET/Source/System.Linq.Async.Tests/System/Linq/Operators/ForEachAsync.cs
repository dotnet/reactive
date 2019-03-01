// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

#if !HAS_AWAIT_FOREACH
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class ForEachAsync : AsyncEnumerableTests
    {
        [Fact]
        public async Task ForEachAsync_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync<int>(default, x => { }));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync(Return42, default(Action<int>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync<int>(default, (x, i) => { }));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync(Return42, default(Action<int, int>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync<int>(default, x => { }, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync(Return42, default(Action<int>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync<int>(default, (x, i) => { }, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync(Return42, default(Action<int, int>), CancellationToken.None));
        }

        [Fact]
        public async Task ForEachAsync_Simple()
        {
            var sum = 0;
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();

            await xs.ForEachAsync(x => sum += x);
            Assert.Equal(10, sum);
        }

        [Fact]
        public async Task ForEachAsync_Indexed()
        {
            var sum = 0;
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();

            await xs.ForEachAsync((x, i) => sum += x * i);
            Assert.Equal(1 * 0 + 2 * 1 + 3 * 2 + 4 * 3, sum);
        }

        [Fact]
        public async Task ForEachAsync_Throws_Action()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex);

            await AssertThrowsAsync(xs.ForEachAsync(x => { throw ex; }), ex);
        }

        [Fact]
        public async Task ForEachAsync_Indexed_Throws_Action()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex);

            await AssertThrowsAsync(xs.ForEachAsync((int x, int i) => { throw ex; }), ex);
        }

        [Fact]
        public async Task ForEachAwaitAsync_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAwaitAsync<int>(default, x => Task.CompletedTask));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAwaitAsync(Return42, default(Func<int, Task>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAwaitAsync<int>(default, (x, i) => Task.CompletedTask));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAwaitAsync(Return42, default(Func<int, int, Task>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAwaitAsync<int>(default, x => Task.CompletedTask, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAwaitAsync(Return42, default(Func<int, Task>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAwaitAsync(default, (int x, int i) => Task.CompletedTask, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAwaitAsync(Return42, default(Func<int, int, Task>), CancellationToken.None));
        }

        [Fact]
        public async Task ForEachAwaitAsync_Simple()
        {
            var sum = 0;
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();

            await xs.ForEachAwaitAsync(x => { sum += x; return Task.CompletedTask; });
            Assert.Equal(10, sum);
        }

        [Fact]
        public async Task ForEachAwaitAsync_Indexed()
        {
            var sum = 0;
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();

            await xs.ForEachAwaitAsync((x, i) => { sum += x * i; return Task.CompletedTask; });
            Assert.Equal(1 * 0 + 2 * 1 + 3 * 2 + 4 * 3, sum);
        }

        [Fact]
        public async Task ForEachAwaitAsync_Throws_Action()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex);

            await AssertThrowsAsync(xs.ForEachAsync(x => Task.FromException(ex)), ex);
        }

        [Fact]
        public async Task ForEachAwaitAsync_Indexed_Throws_Action()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex);

            await AssertThrowsAsync(xs.ForEachAsync((int x, int i) => Task.FromException(ex)), ex);
        }

        [Fact]
        public async Task ForEachAwaitWithCancellationAsync_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAwaitWithCancellationAsync(default, (int x, CancellationToken ct) => Task.CompletedTask, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAwaitWithCancellationAsync(Return42, default(Func<int, CancellationToken, Task>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAwaitWithCancellationAsync<int>(default, (x, i, ct) => Task.CompletedTask, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAwaitWithCancellationAsync(Return42, default(Func<int, int, CancellationToken, Task>), CancellationToken.None));
        }

        [Fact]
        public async Task ForEachAwaitWithCancellationAsync_Simple()
        {
            var sum = 0;
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();

            await xs.ForEachAwaitWithCancellationAsync((int x, CancellationToken ct) => { sum += x; return Task.CompletedTask; }, CancellationToken.None);
            Assert.Equal(10, sum);
        }

        [Fact]
        public async Task ForEachAwaitWithCancellationAsync_Indexed()
        {
            var sum = 0;
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();

            await xs.ForEachAwaitWithCancellationAsync((x, i, ct) => { sum += x * i; return Task.CompletedTask; }, CancellationToken.None);
            Assert.Equal(1 * 0 + 2 * 1 + 3 * 2 + 4 * 3, sum);
        }

        [Fact]
        public async Task ForEachAwaitWithCancellationAsync_Throws_Action()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex);

            await AssertThrowsAsync(xs.ForEachAwaitWithCancellationAsync((int x, CancellationToken ct) => Task.FromException(ex), CancellationToken.None), ex);
        }

        [Fact]
        public async Task ForEachAwaitWithCancellationAsync_Indexed_Throws_Action()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex);

            await AssertThrowsAsync(xs.ForEachAwaitWithCancellationAsync((x, i, ct) => Task.FromException(ex), CancellationToken.None), ex);
        }
    }
}
#endif
