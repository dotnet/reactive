// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerableEx
    {
        public static IAsyncEnumerable<TValue> Return<TValue>(TValue value)
        {
            return new ReturnEnumerable<TValue>(value);
        }

        // FIXME: AsyncListPartition is internal to the project System.Linq.Async
        // project, not sure how to expose it here
        private sealed class ReturnEnumerable<TValue> : IAsyncEnumerable<TValue>
        {
            private readonly TValue _value;

            public ReturnEnumerable(TValue value)
            {
                _value = value;
            }

            public IAsyncEnumerator<TValue> GetAsyncEnumerator(CancellationToken cancellationToken)
            {
                return new ReturnEnumerator(_value);
            }

            private sealed class ReturnEnumerator : IAsyncEnumerator<TValue>
            {
                public TValue Current { get; private set; }

                private bool _once;

                public ReturnEnumerator(TValue current)
                {
                    Current = current;
                }

                public ValueTask DisposeAsync()
                {
                    Current = default;
                    return default;
                }

                public ValueTask<bool> MoveNextAsync()
                {
                    if (_once)
                    {
                        return new ValueTask<bool>(false);
                    }
                    _once = true;
                    return new ValueTask<bool>(true);
                }
            }
        }
    }
}
