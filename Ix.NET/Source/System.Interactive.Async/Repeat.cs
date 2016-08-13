// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TResult> Repeat<TResult>(TResult element, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return Enumerable.Repeat(element, count)
                             .ToAsyncEnumerable();
        }

        public static IAsyncEnumerable<TResult> Repeat<TResult>(TResult element)
        {
            return new RepeatElementAsyncIterator<TResult>(element);
        }

        public static IAsyncEnumerable<TSource> Repeat<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return new RepeatSequenceAsyncIterator<TSource>(source, count);
        }

        public static IAsyncEnumerable<TSource> Repeat<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new RepeatSequenceAsyncIterator<TSource>(source, -1);
        }

        private sealed class RepeatElementAsyncIterator<TResult> : AsyncIterator<TResult>
        {
            private readonly TResult element;

            public RepeatElementAsyncIterator(TResult element)
            {
                this.element = element;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new RepeatElementAsyncIterator<TResult>(element);
            }

            protected override Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                current = element;
                return TaskExt.True;
            }
        }

        private sealed class RepeatSequenceAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly int count;
            private readonly bool isInfinite;
            private readonly IAsyncEnumerable<TSource> source;

            private int currentCount;
            private IAsyncEnumerator<TSource> enumerator;

            public RepeatSequenceAsyncIterator(IAsyncEnumerable<TSource> source, int count)
            {
                this.source = source;
                this.count = count;
                isInfinite = count < 0;
                currentCount = count;
            }


            public override AsyncIterator<TSource> Clone()
            {
                return new RepeatSequenceAsyncIterator<TSource>(source, count);
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
                    case AsyncIteratorState.Allocated:

                        if (enumerator != null)
                        {
                            enumerator.Dispose();
                            enumerator = null;
                        }

                        if (!isInfinite && currentCount-- == 0)
                            break;

                        enumerator = source.GetEnumerator();
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (await enumerator.MoveNext(cancellationToken)
                                            .ConfigureAwait(false))
                        {
                            current = enumerator.Current;
                            return true;
                        }

                        goto case AsyncIteratorState.Allocated;
                        
                }


                Dispose();

                return false;
            }
        }
    }
}