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
        public static IAsyncEnumerable<TResult> SelectAsync<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<TResult>> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new SelectAsyncEnumerableAsyncIterator<TSource, TResult>(source, selector);
        }

        public static IAsyncEnumerable<TResult> SelectAsync<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, Task<TResult>> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new SelectAsyncWithCancelationEnumerableAsyncIterator<TSource, TResult>(source, selector);
        }

        public static IAsyncEnumerable<TResult> SelectAsync<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, Task<TResult>> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new SelectAsyncEnumerableWithIndexAsyncIterator<TSource, TResult>(source, selector);
        }
        public static IAsyncEnumerable<TResult> SelectAsync<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, Task<TResult>> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new SelectAsyncWithCancelationEnumerableWithIndexAsyncIterator<TSource, TResult>(source, selector);
        }

        internal sealed class SelectAsyncEnumerableAsyncIterator<TSource, TResult> : AsyncIterator<TResult>
        {
            private readonly Func<TSource, Task<TResult>> selector;
            private readonly IAsyncEnumerable<TSource> source;

            private IAsyncEnumerator<TSource> enumerator;

            public SelectAsyncEnumerableAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, Task<TResult>> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                this.source = source;
                this.selector = selector;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new SelectAsyncEnumerableAsyncIterator<TSource, TResult>(source, selector);
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
                        enumerator = source.GetEnumerator();
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (await enumerator.MoveNext(cancellationToken)
                                            .ConfigureAwait(false))
                        {
                            current = await selector(enumerator.Current).ConfigureAwait(false);
                            return true;
                        }

                        Dispose();
                        break;
                }

                return false;
            }
        }

        internal sealed class SelectAsyncWithCancelationEnumerableAsyncIterator<TSource, TResult> : AsyncIterator<TResult>
        {
            private readonly Func<TSource, CancellationToken, Task<TResult>> selector;
            private readonly IAsyncEnumerable<TSource> source;

            private IAsyncEnumerator<TSource> enumerator;

            public SelectAsyncWithCancelationEnumerableAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, Task<TResult>> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                this.source = source;
                this.selector = selector;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new SelectAsyncWithCancelationEnumerableAsyncIterator<TSource, TResult>(source, selector);
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
                        enumerator = source.GetEnumerator();
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (await enumerator.MoveNext(cancellationToken)
                                            .ConfigureAwait(false))
                        {
                            current = await selector(enumerator.Current, cancellationToken)
                                                    .ConfigureAwait(false);
                            return true;
                        }

                        Dispose();
                        break;
                }

                return false;
            }
        }

        internal sealed class SelectAsyncEnumerableWithIndexAsyncIterator<TSource, TResult> : AsyncIterator<TResult>
        {
            private readonly Func<TSource, int, Task<TResult>> selector;
            private readonly IAsyncEnumerable<TSource> source;
            private IAsyncEnumerator<TSource> enumerator;
            private int index;

            public SelectAsyncEnumerableWithIndexAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, int, Task<TResult>> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                this.source = source;
                this.selector = selector;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new SelectAsyncEnumerableWithIndexAsyncIterator<TSource, TResult>(source, selector);
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
                        enumerator = source.GetEnumerator();
                        index = -1;
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (await enumerator.MoveNext(cancellationToken)
                                            .ConfigureAwait(false))
                        {
                            checked
                            {
                                index++;
                            }
                            current = await selector(enumerator.Current, index).ConfigureAwait(false);
                            return true;
                        }

                        Dispose();
                        break;
                }

                return false;
            }
        }

        internal sealed class SelectAsyncWithCancelationEnumerableWithIndexAsyncIterator<TSource, TResult> : AsyncIterator<TResult>
        {
            private readonly Func<TSource, int, CancellationToken, Task<TResult>> selector;
            private readonly IAsyncEnumerable<TSource> source;
            private IAsyncEnumerator<TSource> enumerator;
            private int index;

            public SelectAsyncWithCancelationEnumerableWithIndexAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, Task<TResult>> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                this.source = source;
                this.selector = selector;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new SelectAsyncWithCancelationEnumerableWithIndexAsyncIterator<TSource, TResult>(source, selector);
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
                        enumerator = source.GetEnumerator();
                        index = -1;
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (await enumerator.MoveNext(cancellationToken)
                                            .ConfigureAwait(false))
                        {
                            checked
                            {
                                index++;
                            }
                            current = await selector(enumerator.Current, index, cancellationToken)
                                                    .ConfigureAwait(false);
                            return true;
                        }

                        Dispose();
                        break;
                }

                return false;
            }
        }
    }
}
