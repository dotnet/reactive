using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;

namespace Tests
{
    static public class TestExts
    {
        public static IAsyncEnumerable<T> ToSharedStateAsyncEnumerable<T>(this IEnumerable<T> enumerable, SharedState state = null)
        {
            return new SharedStateAsyncEnumerable<T>(enumerable.ToAsyncEnumerable(), state ?? new SharedState());
        }

        public static IAsyncEnumerable<T> ToSharedStateAsyncEnumerable<T>(this IAsyncEnumerable<T> enumerable, SharedState state = null)
        {
            return new SharedStateAsyncEnumerable<T>(enumerable, state ?? new SharedState());
        }

        private class SharedStateAsyncEnumerable<T> : IAsyncEnumerable<T>
        {
            private readonly IAsyncEnumerable<T> _enumerable;
            private readonly SharedState _state;

            public SharedStateAsyncEnumerable(IAsyncEnumerable<T> enumerable, SharedState sharedState)
            {
                _enumerable = enumerable;
                _state = sharedState;
            }

            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken ca) => new Enumerator(_enumerable.GetAsyncEnumerator(ca), _state);

            private class Enumerator : IAsyncEnumerator<T>
            {
                private readonly IAsyncEnumerator<T> _enumerator;
                private readonly SharedState _state;

                public Enumerator(IAsyncEnumerator<T> asyncEnumerator, SharedState state)
                {
                    _enumerator = asyncEnumerator;
                    _state = state;
                }

                public T Current => _enumerator.Current;

                public async ValueTask DisposeAsync()
                {
                    using (_state.Use())
                    {
                        await Task.Yield();
                        await _enumerator.DisposeAsync();
                    }
                }

                public async ValueTask<bool> MoveNextAsync()
                {
                    using (_state.Use())
                    { 
                        await Task.Yield();
                        return await _enumerator.MoveNextAsync();
                    }
                }
            }
        }
    }
}
