// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> DefaultIfEmpty<TSource>(this IAsyncEnumerable<TSource> source, TSource defaultValue)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new DefaultIfEmptyAsyncIterator<TSource>(source, defaultValue);
        }

        public static IAsyncEnumerable<TSource> DefaultIfEmpty<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return DefaultIfEmpty(source, default(TSource));
        }

        private sealed class DefaultIfEmptyAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly IAsyncEnumerable<TSource> source;
            private readonly TSource defaultValue;
            private IAsyncEnumerator<TSource> enumerator;

            public DefaultIfEmptyAsyncIterator(IAsyncEnumerable<TSource> source, TSource defaultValue)
            {
                this.source = source;
                this.defaultValue = defaultValue;
                Debug.Assert(source != null);
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new DefaultIfEmptyAsyncIterator<TSource>(source, defaultValue);
            }

            public override void Dispose()
            {
                if (enumerator != null)
                {
                    enumerator.Dispose();
                    enumerator = null;
                }

                base.Dispose();
            }

            protected override async Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case State.Allocated:
                        enumerator = source.GetEnumerator();
                        if (await enumerator.MoveNext(cancellationToken)
                                            .ConfigureAwait(false))
                        {
                            current = enumerator.Current;
                            state = State.Iterating;
                        }
                        else
                        {
                            current = defaultValue;
                            state = State.Disposed; 
                        }
                        return true;

                    case State.Iterating:
                        if (await enumerator.MoveNext(cancellationToken)
                                            .ConfigureAwait(false))
                        {
                            current = enumerator.Current;
                            return true;
                        }
                        break;
                }

                Dispose();
                return false;
            }
        }
    }
}