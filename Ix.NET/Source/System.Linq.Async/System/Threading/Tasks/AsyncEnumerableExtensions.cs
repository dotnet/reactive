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
                    throw new ArgumentNullException(nameof(enumerable));

                return new ConfiguredAsyncEnumerable<T>(enumerable, continueOnCapturedContext);
            }

            public struct ConfiguredAsyncEnumerable<T>
            {
                private readonly IAsyncEnumerable<T> _enumerable;
                private readonly bool _continueOnCapturedContext;

                internal ConfiguredAsyncEnumerable(IAsyncEnumerable<T> enumerable, bool continueOnCapturedContext)
                {
                    _enumerable = enumerable;
                    _continueOnCapturedContext = continueOnCapturedContext;
                }

                public ConfiguredAsyncEnumerator GetAsyncEnumerator() =>
                    new ConfiguredAsyncEnumerator(_enumerable.GetAsyncEnumerator(), _continueOnCapturedContext);

                public struct ConfiguredAsyncEnumerator
                {
                    private readonly IAsyncEnumerator<T> _enumerator;
                    private readonly bool _continueOnCapturedContext;

                    internal ConfiguredAsyncEnumerator(IAsyncEnumerator<T> enumerator, bool continueOnCapturedContext)
                    {
                        _enumerator = enumerator;
                        _continueOnCapturedContext = continueOnCapturedContext;
                    }

                    public ConfiguredTaskAwaitable<bool> MoveNextAsync() =>
                        _enumerator.MoveNextAsync().ConfigureAwait(_continueOnCapturedContext);

                    public T Current => _enumerator.Current;

                    public ConfiguredTaskAwaitable DisposeAsync() =>
                        _enumerator.DisposeAsync().ConfigureAwait(_continueOnCapturedContext);
                }
            }
        }
    }
}
