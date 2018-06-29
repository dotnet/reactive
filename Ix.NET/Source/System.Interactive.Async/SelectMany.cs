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
        public static IAsyncEnumerable<TOther> SelectMany<TSource, TOther>(this IAsyncEnumerable<TSource> source, IAsyncEnumerable<TOther> other)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return source.SelectMany(_ => other);
        }


        public static IAsyncEnumerable<TResult> SelectMany<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, IAsyncEnumerable<TResult>> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new SelectManyAsyncIterator<TSource, TResult>(source, selector);
        }

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, IAsyncEnumerable<TResult>> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new SelectManyWithIndexAsyncIterator<TSource, TResult>(source, selector);
        }

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, IAsyncEnumerable<TCollection>> selector, Func<TSource, TCollection, TResult> resultSelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }

            return new SelectManyAsyncIterator<TSource, TCollection, TResult>(source, selector, resultSelector);
        }

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, IAsyncEnumerable<TCollection>> selector, Func<TSource, TCollection, TResult> resultSelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }

            return new SelectManyWithIndexAsyncIterator<TSource, TCollection, TResult>(source, selector, resultSelector);
        }

        private sealed class SelectManyAsyncIterator<TSource, TResult> : AsyncIterator<TResult>
        {
            private const int State_Source = 1;
            private const int State_Result = 2;

            private readonly Func<TSource, IAsyncEnumerable<TResult>> selector;
            private readonly IAsyncEnumerable<TSource> source;

            private int mode;
            private IAsyncEnumerator<TResult> resultEnumerator;
            private IAsyncEnumerator<TSource> sourceEnumerator;

            public SelectManyAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, IAsyncEnumerable<TResult>> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                this.source = source;
                this.selector = selector;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new SelectManyAsyncIterator<TSource, TResult>(source, selector);
            }

            public override void Dispose()
            {
                if (sourceEnumerator != null)
                {
                    sourceEnumerator.Dispose();
                    sourceEnumerator = null;
                }

                if (resultEnumerator != null)
                {
                    resultEnumerator.Dispose();
                    resultEnumerator = null;
                }

                base.Dispose();
            }

            protected override async Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        sourceEnumerator = source.GetEnumerator();
                        mode = State_Source;
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (mode)
                        {
                            case State_Source:
                                if (await sourceEnumerator.MoveNext(cancellationToken)
                                                          .ConfigureAwait(false))
                                {
                                    resultEnumerator?.Dispose();
                                    resultEnumerator = selector(sourceEnumerator.Current)
                                        .GetEnumerator();

                                    mode = State_Result;
                                    goto case State_Result;
                                }
                                break;

                            case State_Result:
                                if (await resultEnumerator.MoveNext(cancellationToken)
                                                          .ConfigureAwait(false))
                                {
                                    current = resultEnumerator.Current;
                                    return true;
                                }

                                mode = State_Source;
                                goto case State_Source; // loop
                        }

                        break;
                }

                Dispose();
                return false;
            }
        }

        private sealed class SelectManyAsyncIterator<TSource, TCollection, TResult> : AsyncIterator<TResult>
        {
            private const int State_Source = 1;
            private const int State_Result = 2;

            private readonly Func<TSource, IAsyncEnumerable<TCollection>> collectionSelector;
            private readonly Func<TSource, TCollection, TResult> resultSelector;
            private readonly IAsyncEnumerable<TSource> source;

            private TSource currentSource;
            private int mode;
            private IAsyncEnumerator<TCollection> resultEnumerator;
            private IAsyncEnumerator<TSource> sourceEnumerator;

            public SelectManyAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, IAsyncEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
            {
                Debug.Assert(source != null);
                Debug.Assert(collectionSelector != null);
                Debug.Assert(resultSelector != null);

                this.source = source;
                this.collectionSelector = collectionSelector;
                this.resultSelector = resultSelector;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new SelectManyAsyncIterator<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
            }

            public override void Dispose()
            {
                if (sourceEnumerator != null)
                {
                    sourceEnumerator.Dispose();
                    sourceEnumerator = null;
                }

                if (resultEnumerator != null)
                {
                    resultEnumerator.Dispose();
                    resultEnumerator = null;
                }

                currentSource = default;

                base.Dispose();
            }

            protected override async Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        sourceEnumerator = source.GetEnumerator();
                        mode = State_Source;
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (mode)
                        {
                            case State_Source:
                                if (await sourceEnumerator.MoveNext(cancellationToken)
                                                          .ConfigureAwait(false))
                                {
                                    resultEnumerator?.Dispose();
                                    currentSource = sourceEnumerator.Current;
                                    resultEnumerator = collectionSelector(currentSource)
                                        .GetEnumerator();

                                    mode = State_Result;
                                    goto case State_Result;
                                }
                                break;

                            case State_Result:
                                if (await resultEnumerator.MoveNext(cancellationToken)
                                                          .ConfigureAwait(false))
                                {
                                    current = resultSelector(currentSource, resultEnumerator.Current);
                                    return true;
                                }

                                mode = State_Source;
                                goto case State_Source; // loop
                        }

                        break;
                }

                Dispose();
                return false;
            }
        }

        private sealed class SelectManyWithIndexAsyncIterator<TSource, TCollection, TResult> : AsyncIterator<TResult>
        {
            private const int State_Source = 1;
            private const int State_Result = 2;

            private readonly Func<TSource, int, IAsyncEnumerable<TCollection>> collectionSelector;
            private readonly Func<TSource, TCollection, TResult> resultSelector;
            private readonly IAsyncEnumerable<TSource> source;

            private TSource currentSource;
            private int index;
            private int mode;
            private IAsyncEnumerator<TCollection> resultEnumerator;
            private IAsyncEnumerator<TSource> sourceEnumerator;

            public SelectManyWithIndexAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, int, IAsyncEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
            {
                Debug.Assert(source != null);
                Debug.Assert(collectionSelector != null);
                Debug.Assert(resultSelector != null);

                this.source = source;
                this.collectionSelector = collectionSelector;
                this.resultSelector = resultSelector;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new SelectManyWithIndexAsyncIterator<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
            }

            public override void Dispose()
            {
                if (sourceEnumerator != null)
                {
                    sourceEnumerator.Dispose();
                    sourceEnumerator = null;
                }

                if (resultEnumerator != null)
                {
                    resultEnumerator.Dispose();
                    resultEnumerator = null;
                }

                currentSource = default;

                base.Dispose();
            }

            protected override async Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        sourceEnumerator = source.GetEnumerator();
                        index = -1;
                        mode = State_Source;
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (mode)
                        {
                            case State_Source:
                                if (await sourceEnumerator.MoveNext(cancellationToken)
                                                          .ConfigureAwait(false))
                                {
                                    resultEnumerator?.Dispose();
                                    currentSource = sourceEnumerator.Current;

                                    checked
                                    {
                                        index++;
                                    }

                                    resultEnumerator = collectionSelector(currentSource, index)
                                        .GetEnumerator();

                                    mode = State_Result;
                                    goto case State_Result;
                                }
                                break;

                            case State_Result:
                                if (await resultEnumerator.MoveNext(cancellationToken)
                                                          .ConfigureAwait(false))
                                {
                                    current = resultSelector(currentSource, resultEnumerator.Current);
                                    return true;
                                }

                                mode = State_Source;
                                goto case State_Source; // loop
                        }

                        break;
                }

                Dispose();
                return false;
            }
        }

        private sealed class SelectManyWithIndexAsyncIterator<TSource, TResult> : AsyncIterator<TResult>
        {
            private const int State_Source = 1;
            private const int State_Result = 2;

            private readonly Func<TSource, int, IAsyncEnumerable<TResult>> selector;
            private readonly IAsyncEnumerable<TSource> source;

            private int index;
            private int mode;
            private IAsyncEnumerator<TResult> resultEnumerator;
            private IAsyncEnumerator<TSource> sourceEnumerator;

            public SelectManyWithIndexAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, int, IAsyncEnumerable<TResult>> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                this.source = source;
                this.selector = selector;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new SelectManyWithIndexAsyncIterator<TSource, TResult>(source, selector);
            }

            public override void Dispose()
            {
                if (sourceEnumerator != null)
                {
                    sourceEnumerator.Dispose();
                    sourceEnumerator = null;
                }

                if (resultEnumerator != null)
                {
                    resultEnumerator.Dispose();
                    resultEnumerator = null;
                }

                base.Dispose();
            }

            protected override async Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        sourceEnumerator = source.GetEnumerator();
                        index = -1;
                        mode = State_Source;
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (mode)
                        {
                            case State_Source:
                                if (await sourceEnumerator.MoveNext(cancellationToken)
                                                          .ConfigureAwait(false))
                                {
                                    resultEnumerator?.Dispose();
                                    checked
                                    {
                                        index++;
                                    }
                                    resultEnumerator = selector(sourceEnumerator.Current, index)
                                        .GetEnumerator();

                                    mode = State_Result;
                                    goto case State_Result;
                                }
                                break;

                            case State_Result:
                                if (await resultEnumerator.MoveNext(cancellationToken)
                                                          .ConfigureAwait(false))
                                {
                                    current = resultEnumerator.Current;
                                    return true;
                                }

                                mode = State_Source;
                                goto case State_Source; // loop
                        }

                        break;
                }

                Dispose();
                return false;
            }
        }
    }
}