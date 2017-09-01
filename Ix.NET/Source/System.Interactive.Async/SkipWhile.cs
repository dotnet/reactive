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
        public static IAsyncEnumerable<TSource> SkipWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return new SkipWhileAsyncIterator<TSource>(source, predicate);
        }

        public static IAsyncEnumerable<TSource> SkipWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return new SkipWhileWithIndexAsyncIterator<TSource>(source, predicate);
        }

        public static IAsyncEnumerable<TSource> SkipWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return new SkipWhileAsyncIteratorWithTask<TSource>(source, predicate);
        }

        public static IAsyncEnumerable<TSource> SkipWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, Task<bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return new SkipWhileWithIndexAsyncIteratorWithTask<TSource>(source, predicate);
        }

        private sealed class SkipWhileAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, bool> predicate;
            private readonly IAsyncEnumerable<TSource> source;

            private bool doMoveNext;
            private IAsyncEnumerator<TSource> enumerator;

            public SkipWhileAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
            {
                Debug.Assert(predicate != null);
                Debug.Assert(source != null);

                this.source = source;
                this.predicate = predicate;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new SkipWhileAsyncIterator<TSource>(source, predicate);
            }

            public override async Task DisposeAsync()
            {
                if (enumerator != null)
                {
                    await enumerator.DisposeAsync().ConfigureAwait(false);
                    enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async Task<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetAsyncEnumerator();

                        // skip elements as requested
                        while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var element = enumerator.Current;
                            if (!predicate(element))
                            {
                                doMoveNext = false;
                                state = AsyncIteratorState.Iterating;
                                goto case AsyncIteratorState.Iterating;
                            }
                        }

                        break;

                    case AsyncIteratorState.Iterating:
                        if (doMoveNext && await enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            current = enumerator.Current;
                            return true;
                        }

                        if (!doMoveNext)
                        {
                            current = enumerator.Current;
                            doMoveNext = true;
                            return true;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

        private sealed class SkipWhileWithIndexAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, int, bool> predicate;
            private readonly IAsyncEnumerable<TSource> source;

            private bool doMoveNext;
            private IAsyncEnumerator<TSource> enumerator;
            private int index;

            public SkipWhileWithIndexAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
            {
                Debug.Assert(predicate != null);
                Debug.Assert(source != null);

                this.source = source;
                this.predicate = predicate;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new SkipWhileWithIndexAsyncIterator<TSource>(source, predicate);
            }

            public override async Task DisposeAsync()
            {
                if (enumerator != null)
                {
                    await enumerator.DisposeAsync().ConfigureAwait(false);
                    enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async Task<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetAsyncEnumerator();
                        index = -1;

                        // skip elements as requested
                        while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            checked
                            {
                                index++;
                            }

                            var element = enumerator.Current;
                            if (!predicate(element, index))
                            {
                                doMoveNext = false;
                                state = AsyncIteratorState.Iterating;
                                goto case AsyncIteratorState.Iterating;
                            }
                        }

                        break;

                    case AsyncIteratorState.Iterating:
                        if (doMoveNext && await enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            current = enumerator.Current;
                            return true;
                        }

                        if (!doMoveNext)
                        {
                            current = enumerator.Current;
                            doMoveNext = true;
                            return true;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

        private sealed class SkipWhileAsyncIteratorWithTask<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, Task<bool>> predicate;
            private readonly IAsyncEnumerable<TSource> source;

            private bool doMoveNext;
            private IAsyncEnumerator<TSource> enumerator;

            public SkipWhileAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TSource, Task<bool>> predicate)
            {
                Debug.Assert(predicate != null);
                Debug.Assert(source != null);

                this.source = source;
                this.predicate = predicate;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new SkipWhileAsyncIteratorWithTask<TSource>(source, predicate);
            }

            public override async Task DisposeAsync()
            {
                if (enumerator != null)
                {
                    await enumerator.DisposeAsync().ConfigureAwait(false);
                    enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async Task<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetAsyncEnumerator();

                        // skip elements as requested
                        while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var element = enumerator.Current;
                            if (!await predicate(element).ConfigureAwait(false))
                            {
                                doMoveNext = false;
                                state = AsyncIteratorState.Iterating;
                                goto case AsyncIteratorState.Iterating;
                            }
                        }

                        break;

                    case AsyncIteratorState.Iterating:
                        if (doMoveNext && await enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            current = enumerator.Current;
                            return true;
                        }

                        if (!doMoveNext)
                        {
                            current = enumerator.Current;
                            doMoveNext = true;
                            return true;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

        private sealed class SkipWhileWithIndexAsyncIteratorWithTask<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, int, Task<bool>> predicate;
            private readonly IAsyncEnumerable<TSource> source;

            private bool doMoveNext;
            private IAsyncEnumerator<TSource> enumerator;
            private int index;

            public SkipWhileWithIndexAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TSource, int, Task<bool>> predicate)
            {
                Debug.Assert(predicate != null);
                Debug.Assert(source != null);

                this.source = source;
                this.predicate = predicate;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new SkipWhileWithIndexAsyncIteratorWithTask<TSource>(source, predicate);
            }

            public override async Task DisposeAsync()
            {
                if (enumerator != null)
                {
                    await enumerator.DisposeAsync().ConfigureAwait(false);
                    enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async Task<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetAsyncEnumerator();
                        index = -1;

                        // skip elements as requested
                        while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            checked
                            {
                                index++;
                            }

                            var element = enumerator.Current;
                            if (!await predicate(element, index).ConfigureAwait(false))
                            {
                                doMoveNext = false;
                                state = AsyncIteratorState.Iterating;
                                goto case AsyncIteratorState.Iterating;
                            }
                        }

                        break;

                    case AsyncIteratorState.Iterating:
                        if (doMoveNext && await enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            current = enumerator.Current;
                            return true;
                        }

                        if (!doMoveNext)
                        {
                            current = enumerator.Current;
                            doMoveNext = true;
                            return true;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }
    }
}
