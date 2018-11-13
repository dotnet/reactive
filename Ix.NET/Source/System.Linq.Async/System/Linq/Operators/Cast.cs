// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TResult> Cast<TResult>(this IAsyncEnumerable<object> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            // Check to see if it already is and short-circuit
            if (source is IAsyncEnumerable<TResult> typedSource)
            {
                return typedSource;
            }

            return new CastAsyncIterator<TResult>(source);
        }

        internal sealed class CastAsyncIterator<TResult> : AsyncIterator<TResult>
        {
            private readonly IAsyncEnumerable<object> _source;
            private IAsyncEnumerator<object> _enumerator;

            public CastAsyncIterator(IAsyncEnumerable<object> source)
            {
                Debug.Assert(source != null);

                _source = source;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new CastAsyncIterator<TResult>(_source);
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
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(_cancellationToken);
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            _current = (TResult)_enumerator.Current;
                            return true;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
        }
    }
}
