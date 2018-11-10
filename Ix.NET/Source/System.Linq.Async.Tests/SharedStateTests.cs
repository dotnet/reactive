using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class SharedStateTests
    {
        [Fact]
        public async Task ConcurrentTask()
        {
            var state = new SharedState();

            var t1 = state.GetTask();
            var t2 = state.GetTask();

            Task f() => Task.WhenAll(t1.AsTask(), t2.AsTask());

            await Assert.ThrowsAsync<InvalidOperationException>(f);
        }

        [Fact]
        public async Task SequentialTask()
        {
            var state = new SharedState();

            async Task f()
            {
                await state.GetTask();
                await state.GetTask();
            }

            await f(); // Should not throw
        }

        [Fact]
        public async Task ConcurrentMoveNextAsync()
        {
            var state = new SharedState();

            var seq1 = AsyncEnumerable.Range(0, 10).ToSharedStateAsyncEnumerable(state);
            var seq2 = AsyncEnumerable.Range(0, 10).ToSharedStateAsyncEnumerable(state);

            var e1 = seq1.GetAsyncEnumerator();
            var e2 = seq2.GetAsyncEnumerator();

            var t1 = e1.MoveNextAsync();
            var t2 = e2.MoveNextAsync();

            Task f() => Task.WhenAll(t1.AsTask(), t2.AsTask());

            await Assert.ThrowsAsync<InvalidOperationException>(f);
        }

        [Fact]
        public async Task SequentialMoveNextAsync()
        {
            var state = new SharedState();

            var seq1 = AsyncEnumerable.Range(0, 10).ToSharedStateAsyncEnumerable(state);
            var seq2 = AsyncEnumerable.Range(0, 10).ToSharedStateAsyncEnumerable(state);

            var e1 = seq1.GetAsyncEnumerator();
            var e2 = seq2.GetAsyncEnumerator();

            async Task f()
            {
                await e1.MoveNextAsync();
                await e2.MoveNextAsync();
            }

            await f(); // Should not throw
        }

        [Fact]
        public async Task ConcurrentDisposeAsync()
        {
            var state = new SharedState();

            var seq1 = AsyncEnumerable.Range(0, 10).ToSharedStateAsyncEnumerable(state);
            var seq2 = AsyncEnumerable.Range(0, 10).ToSharedStateAsyncEnumerable(state);

            var e1 = seq1.GetAsyncEnumerator();
            var e2 = seq2.GetAsyncEnumerator();

            var t1 = e1.DisposeAsync();
            var t2 = e2.DisposeAsync();

            Task f() => Task.WhenAll(t1.AsTask(), t2.AsTask());

            await Assert.ThrowsAsync<InvalidOperationException>(f);
        }

        [Fact]
        public async Task SequentialDisposeAsync()
        {
            var state = new SharedState();

            var seq1 = AsyncEnumerable.Range(0, 10).ToSharedStateAsyncEnumerable(state);
            var seq2 = AsyncEnumerable.Range(0, 10).ToSharedStateAsyncEnumerable(state);

            var e1 = seq1.GetAsyncEnumerator();
            var e2 = seq2.GetAsyncEnumerator();

            async Task f()
            {
                await e1.DisposeAsync();
                await e2.DisposeAsync();
            }

            await f(); // Should not throw
        }
    }
}
