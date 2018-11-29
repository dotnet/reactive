// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
    namespace System.Threading.Tasks
    {
        public static class AsyncEnumerableExtensions
        {
            public static ConfiguredAsyncEnumerable<T> ConfigureAwait<T>(this IAsyncEnumerable<T> enumerable, bool continueOnCapturedContext)
            {
                if (enumerable == null)
                    throw Error.ArgumentNull(nameof(enumerable));

                return new ConfiguredAsyncEnumerable<T>(enumerable, continueOnCapturedContext);
            }

            // REVIEW: Explicit implementation of the interfaces allows for composition with other "modifier operators" such as WithCancellation.
            //         We expect that the "await foreach" statement will bind to the public struct methods, thus avoiding boxing.

            public readonly struct ConfiguredAsyncEnumerable<T> : IAsyncEnumerable<T>
            {
                private readonly IAsyncEnumerable<T> _enumerable;
                private readonly bool _continueOnCapturedContext;

                internal ConfiguredAsyncEnumerable(IAsyncEnumerable<T> enumerable, bool continueOnCapturedContext)
                {
                    _enumerable = enumerable;
                    _continueOnCapturedContext = continueOnCapturedContext;
                }

                public ConfiguredAsyncEnumerator GetAsyncEnumerator(CancellationToken cancellationToken)
                {
                    cancellationToken.ThrowIfCancellationRequested(); // NB: [LDM-2018-11-28] Equivalent to async iterator behavior.

                    return new ConfiguredAsyncEnumerator(_enumerable.GetAsyncEnumerator(cancellationToken), _continueOnCapturedContext);
                }

                IAsyncEnumerator<T> IAsyncEnumerable<T>.GetAsyncEnumerator(CancellationToken cancellationToken) =>
                    GetAsyncEnumerator(cancellationToken);

                public readonly struct ConfiguredAsyncEnumerator : IAsyncEnumerator<T>
                {
                    private readonly IAsyncEnumerator<T> _enumerator;
                    private readonly bool _continueOnCapturedContext;

                    internal ConfiguredAsyncEnumerator(IAsyncEnumerator<T> enumerator, bool continueOnCapturedContext)
                    {
                        _enumerator = enumerator;
                        _continueOnCapturedContext = continueOnCapturedContext;
                    }

                    public ConfiguredValueTaskAwaitable<bool> MoveNextAsync() =>
                        _enumerator.MoveNextAsync().ConfigureAwait(_continueOnCapturedContext);

                    public T Current => _enumerator.Current;

                    public ConfiguredValueTaskAwaitable DisposeAsync() =>
                        _enumerator.DisposeAsync().ConfigureAwait(_continueOnCapturedContext);

                    async ValueTask<bool> IAsyncEnumerator<T>.MoveNextAsync() =>
                        await _enumerator.MoveNextAsync().ConfigureAwait(_continueOnCapturedContext);

                    async ValueTask IAsyncDisposable.DisposeAsync() =>
                        await _enumerator.DisposeAsync().ConfigureAwait(_continueOnCapturedContext);
                }
            }
        }
    }
}
