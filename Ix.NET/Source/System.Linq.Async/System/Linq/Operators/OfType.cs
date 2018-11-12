// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TResult> OfType<TResult>(this IAsyncEnumerable<object> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return new OfTypeAsyncIterator<TResult>(source);
        }

        internal sealed class OfTypeAsyncIterator<TResult> : AsyncIterator<TResult>
        {
            private readonly IAsyncEnumerable<object> _source;
            private IAsyncEnumerator<object> _enumerator;

            public OfTypeAsyncIterator(IAsyncEnumerable<object> source)
            {
                Debug.Assert(source != null);

                _source = source;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new OfTypeAsyncIterator<TResult>(_source);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(cancellationToken);
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var item = _enumerator.Current;
                            if (item is TResult res)
                            {
                                current = res;
                                return true;
                            }
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
        }
    }
}
