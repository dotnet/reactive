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

            await Task.WhenAll(t1.AsTask(), t2.AsTask());

            Assert.Equal(1, state.ConcurrentAccessCount);
        }

        [Fact]
        public async Task SequentialTask()
        {
            var state = new SharedState();

            await state.GetTask();
            await state.GetTask();

            Assert.Equal(0, state.ConcurrentAccessCount);
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

            await Task.WhenAll(t1.AsTask(), t2.AsTask());

            Assert.Equal(1, state.ConcurrentAccessCount);
        }

        [Fact]
        public async Task SequentialMoveNextAsync()
        {
            var state = new SharedState();

            var seq1 = AsyncEnumerable.Range(0, 10).ToSharedStateAsyncEnumerable(state);
            var seq2 = AsyncEnumerable.Range(0, 10).ToSharedStateAsyncEnumerable(state);

            var e1 = seq1.GetAsyncEnumerator();
            var e2 = seq2.GetAsyncEnumerator();

            await e1.MoveNextAsync();
            await e2.MoveNextAsync();

            Assert.Equal(0, state.ConcurrentAccessCount);
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

            await Task.WhenAll(t1.AsTask(), t2.AsTask());

            Assert.Equal(1, state.ConcurrentAccessCount);
        }

        [Fact]
        public async Task SequentialDisposeAsync()
        {
            var state = new SharedState();

            var seq1 = AsyncEnumerable.Range(0, 10).ToSharedStateAsyncEnumerable(state);
            var seq2 = AsyncEnumerable.Range(0, 10).ToSharedStateAsyncEnumerable(state);

            var e1 = seq1.GetAsyncEnumerator();
            var e2 = seq2.GetAsyncEnumerator();

            await e1.DisposeAsync();
            await e2.DisposeAsync();

            Assert.Equal(0, state.ConcurrentAccessCount);
        }
    }
}
