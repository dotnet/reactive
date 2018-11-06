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
        public static IAsyncEnumerable<TResult> SelectMany<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, IAsyncEnumerable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new SelectManyAsyncIterator<TSource, TResult>(source, selector);
        }

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<IAsyncEnumerable<TResult>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new SelectManyAsyncIteratorWithTask<TSource, TResult>(source, selector);
        }

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, IAsyncEnumerable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new SelectManyWithIndexAsyncIterator<TSource, TResult>(source, selector);
        }

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, Task<IAsyncEnumerable<TResult>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new SelectManyWithIndexAsyncIteratorWithTask<TSource, TResult>(source, selector);
        }

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, IAsyncEnumerable<TCollection>> selector, Func<TSource, TCollection, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return new SelectManyAsyncIterator<TSource, TCollection, TResult>(source, selector, resultSelector);
        }

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<IAsyncEnumerable<TCollection>>> selector, Func<TSource, TCollection, Task<TResult>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return new SelectManyAsyncIteratorWithTask<TSource, TCollection, TResult>(source, selector, resultSelector);
        }

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, IAsyncEnumerable<TCollection>> selector, Func<TSource, TCollection, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return new SelectManyWithIndexAsyncIterator<TSource, TCollection, TResult>(source, selector, resultSelector);
        }

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, Task<IAsyncEnumerable<TCollection>>> selector, Func<TSource, TCollection, Task<TResult>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return new SelectManyWithIndexAsyncIteratorWithTask<TSource, TCollection, TResult>(source, selector, resultSelector);
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

            public override async ValueTask DisposeAsync()
            {
                if (sourceEnumerator != null)
                {
                    await sourceEnumerator.DisposeAsync().ConfigureAwait(false);
                    sourceEnumerator = null;
                }

                if (resultEnumerator != null)
                {
                    await resultEnumerator.DisposeAsync().ConfigureAwait(false);
                    resultEnumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        sourceEnumerator = source.GetAsyncEnumerator(cancellationToken);
                        mode = State_Source;
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (mode)
                        {
                            case State_Source:
                                if (await sourceEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    if (resultEnumerator != null)
                                    {
                                        await resultEnumerator.DisposeAsync().ConfigureAwait(false);
                                    }

                                    var inner = selector(sourceEnumerator.Current);
                                    resultEnumerator = inner.GetAsyncEnumerator(cancellationToken);

                                    mode = State_Result;
                                    goto case State_Result;
                                }
                                break;

                            case State_Result:
                                if (await resultEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    current = resultEnumerator.Current;
                                    return true;
                                }

                                mode = State_Source;
                                goto case State_Source; // loop
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

        private sealed class SelectManyAsyncIteratorWithTask<TSource, TResult> : AsyncIterator<TResult>
        {
            private const int State_Source = 1;
            private const int State_Result = 2;

            private readonly Func<TSource, Task<IAsyncEnumerable<TResult>>> selector;
            private readonly IAsyncEnumerable<TSource> source;

            private int mode;
            private IAsyncEnumerator<TResult> resultEnumerator;
            private IAsyncEnumerator<TSource> sourceEnumerator;

            public SelectManyAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TSource, Task<IAsyncEnumerable<TResult>>> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                this.source = source;
                this.selector = selector;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new SelectManyAsyncIteratorWithTask<TSource, TResult>(source, selector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (sourceEnumerator != null)
                {
                    await sourceEnumerator.DisposeAsync().ConfigureAwait(false);
                    sourceEnumerator = null;
                }

                if (resultEnumerator != null)
                {
                    await resultEnumerator.DisposeAsync().ConfigureAwait(false);
                    resultEnumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        sourceEnumerator = source.GetAsyncEnumerator(cancellationToken);
                        mode = State_Source;
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (mode)
                        {
                            case State_Source:
                                if (await sourceEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    if (resultEnumerator != null)
                                    {
                                        await resultEnumerator.DisposeAsync().ConfigureAwait(false);
                                    }

                                    var inner = await selector(sourceEnumerator.Current).ConfigureAwait(false);
                                    resultEnumerator = inner.GetAsyncEnumerator(cancellationToken);

                                    mode = State_Result;
                                    goto case State_Result;
                                }
                                break;

                            case State_Result:
                                if (await resultEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    current = resultEnumerator.Current;
                                    return true;
                                }

                                mode = State_Source;
                                goto case State_Source; // loop
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
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

            public override async ValueTask DisposeAsync()
            {
                if (sourceEnumerator != null)
                {
                    await sourceEnumerator.DisposeAsync().ConfigureAwait(false);
                    sourceEnumerator = null;
                }

                if (resultEnumerator != null)
                {
                    await resultEnumerator.DisposeAsync().ConfigureAwait(false);
                    resultEnumerator = null;
                }

                currentSource = default(TSource);

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        sourceEnumerator = source.GetAsyncEnumerator(cancellationToken);
                        mode = State_Source;
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (mode)
                        {
                            case State_Source:
                                if (await sourceEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    if (resultEnumerator != null)
                                    {
                                        await resultEnumerator.DisposeAsync().ConfigureAwait(false);
                                    }

                                    currentSource = sourceEnumerator.Current;
                                    var inner = collectionSelector(currentSource);
                                    resultEnumerator = inner.GetAsyncEnumerator(cancellationToken);

                                    mode = State_Result;
                                    goto case State_Result;
                                }
                                break;

                            case State_Result:
                                if (await resultEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    current = resultSelector(currentSource, resultEnumerator.Current);
                                    return true;
                                }

                                mode = State_Source;
                                goto case State_Source; // loop
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

        private sealed class SelectManyAsyncIteratorWithTask<TSource, TCollection, TResult> : AsyncIterator<TResult>
        {
            private const int State_Source = 1;
            private const int State_Result = 2;

            private readonly Func<TSource, Task<IAsyncEnumerable<TCollection>>> collectionSelector;
            private readonly Func<TSource, TCollection, Task<TResult>> resultSelector;
            private readonly IAsyncEnumerable<TSource> source;

            private TSource currentSource;
            private int mode;
            private IAsyncEnumerator<TCollection> resultEnumerator;
            private IAsyncEnumerator<TSource> sourceEnumerator;

            public SelectManyAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TSource, Task<IAsyncEnumerable<TCollection>>> collectionSelector, Func<TSource, TCollection, Task<TResult>> resultSelector)
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
                return new SelectManyAsyncIteratorWithTask<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (sourceEnumerator != null)
                {
                    await sourceEnumerator.DisposeAsync().ConfigureAwait(false);
                    sourceEnumerator = null;
                }

                if (resultEnumerator != null)
                {
                    await resultEnumerator.DisposeAsync().ConfigureAwait(false);
                    resultEnumerator = null;
                }

                currentSource = default(TSource);

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        sourceEnumerator = source.GetAsyncEnumerator(cancellationToken);
                        mode = State_Source;
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (mode)
                        {
                            case State_Source:
                                if (await sourceEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    if (resultEnumerator != null)
                                    {
                                        await resultEnumerator.DisposeAsync().ConfigureAwait(false);
                                    }

                                    currentSource = sourceEnumerator.Current;
                                    var inner = await collectionSelector(currentSource).ConfigureAwait(false);
                                    resultEnumerator = inner.GetAsyncEnumerator(cancellationToken);

                                    mode = State_Result;
                                    goto case State_Result;
                                }
                                break;

                            case State_Result:
                                if (await resultEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    current = await resultSelector(currentSource, resultEnumerator.Current).ConfigureAwait(false);
                                    return true;
                                }

                                mode = State_Source;
                                goto case State_Source; // loop
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
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

            public override async ValueTask DisposeAsync()
            {
                if (sourceEnumerator != null)
                {
                    await sourceEnumerator.DisposeAsync().ConfigureAwait(false);
                    sourceEnumerator = null;
                }

                if (resultEnumerator != null)
                {
                    await resultEnumerator.DisposeAsync().ConfigureAwait(false);
                    resultEnumerator = null;
                }

                currentSource = default(TSource);

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        sourceEnumerator = source.GetAsyncEnumerator(cancellationToken);
                        index = -1;
                        mode = State_Source;
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (mode)
                        {
                            case State_Source:
                                if (await sourceEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    if (resultEnumerator != null)
                                    {
                                        await resultEnumerator.DisposeAsync().ConfigureAwait(false);
                                    }

                                    currentSource = sourceEnumerator.Current;

                                    checked
                                    {
                                        index++;
                                    }

                                    var inner = collectionSelector(currentSource, index);
                                    resultEnumerator = inner.GetAsyncEnumerator(cancellationToken);

                                    mode = State_Result;
                                    goto case State_Result;
                                }
                                break;

                            case State_Result:
                                if (await resultEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    current = resultSelector(currentSource, resultEnumerator.Current);
                                    return true;
                                }

                                mode = State_Source;
                                goto case State_Source; // loop
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

        private sealed class SelectManyWithIndexAsyncIteratorWithTask<TSource, TCollection, TResult> : AsyncIterator<TResult>
        {
            private const int State_Source = 1;
            private const int State_Result = 2;

            private readonly Func<TSource, int, Task<IAsyncEnumerable<TCollection>>> collectionSelector;
            private readonly Func<TSource, TCollection, Task<TResult>> resultSelector;
            private readonly IAsyncEnumerable<TSource> source;

            private TSource currentSource;
            private int index;
            private int mode;
            private IAsyncEnumerator<TCollection> resultEnumerator;
            private IAsyncEnumerator<TSource> sourceEnumerator;

            public SelectManyWithIndexAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TSource, int, Task<IAsyncEnumerable<TCollection>>> collectionSelector, Func<TSource, TCollection, Task<TResult>> resultSelector)
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
                return new SelectManyWithIndexAsyncIteratorWithTask<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (sourceEnumerator != null)
                {
                    await sourceEnumerator.DisposeAsync().ConfigureAwait(false);
                    sourceEnumerator = null;
                }

                if (resultEnumerator != null)
                {
                    await resultEnumerator.DisposeAsync().ConfigureAwait(false);
                    resultEnumerator = null;
                }

                currentSource = default(TSource);

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        sourceEnumerator = source.GetAsyncEnumerator(cancellationToken);
                        index = -1;
                        mode = State_Source;
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (mode)
                        {
                            case State_Source:
                                if (await sourceEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    if (resultEnumerator != null)
                                    {
                                        await resultEnumerator.DisposeAsync().ConfigureAwait(false);
                                    }

                                    currentSource = sourceEnumerator.Current;

                                    checked
                                    {
                                        index++;
                                    }

                                    var inner = await collectionSelector(currentSource, index).ConfigureAwait(false);
                                    resultEnumerator = inner.GetAsyncEnumerator(cancellationToken);

                                    mode = State_Result;
                                    goto case State_Result;
                                }
                                break;

                            case State_Result:
                                if (await resultEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    current = await resultSelector(currentSource, resultEnumerator.Current).ConfigureAwait(false);
                                    return true;
                                }

                                mode = State_Source;
                                goto case State_Source; // loop
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
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

            public override async ValueTask DisposeAsync()
            {
                if (sourceEnumerator != null)
                {
                    await sourceEnumerator.DisposeAsync().ConfigureAwait(false);
                    sourceEnumerator = null;
                }

                if (resultEnumerator != null)
                {
                    await resultEnumerator.DisposeAsync().ConfigureAwait(false);
                    resultEnumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        sourceEnumerator = source.GetAsyncEnumerator(cancellationToken);
                        index = -1;
                        mode = State_Source;
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (mode)
                        {
                            case State_Source:
                                if (await sourceEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    if (resultEnumerator != null)
                                    {
                                        await resultEnumerator.DisposeAsync().ConfigureAwait(false);
                                    }

                                    checked
                                    {
                                        index++;
                                    }

                                    var inner = selector(sourceEnumerator.Current, index);
                                    resultEnumerator = inner.GetAsyncEnumerator(cancellationToken);

                                    mode = State_Result;
                                    goto case State_Result;
                                }
                                break;

                            case State_Result:
                                if (await resultEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    current = resultEnumerator.Current;
                                    return true;
                                }

                                mode = State_Source;
                                goto case State_Source; // loop
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

        private sealed class SelectManyWithIndexAsyncIteratorWithTask<TSource, TResult> : AsyncIterator<TResult>
        {
            private const int State_Source = 1;
            private const int State_Result = 2;

            private readonly Func<TSource, int, Task<IAsyncEnumerable<TResult>>> selector;
            private readonly IAsyncEnumerable<TSource> source;

            private int index;
            private int mode;
            private IAsyncEnumerator<TResult> resultEnumerator;
            private IAsyncEnumerator<TSource> sourceEnumerator;

            public SelectManyWithIndexAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TSource, int, Task<IAsyncEnumerable<TResult>>> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                this.source = source;
                this.selector = selector;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new SelectManyWithIndexAsyncIteratorWithTask<TSource, TResult>(source, selector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (sourceEnumerator != null)
                {
                    await sourceEnumerator.DisposeAsync().ConfigureAwait(false);
                    sourceEnumerator = null;
                }

                if (resultEnumerator != null)
                {
                    await resultEnumerator.DisposeAsync().ConfigureAwait(false);
                    resultEnumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        sourceEnumerator = source.GetAsyncEnumerator(cancellationToken);
                        index = -1;
                        mode = State_Source;
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (mode)
                        {
                            case State_Source:
                                if (await sourceEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    if (resultEnumerator != null)
                                    {
                                        await resultEnumerator.DisposeAsync().ConfigureAwait(false);
                                    }

                                    checked
                                    {
                                        index++;
                                    }

                                    var inner = await selector(sourceEnumerator.Current, index).ConfigureAwait(false);
                                    resultEnumerator = inner.GetAsyncEnumerator(cancellationToken);

                                    mode = State_Result;
                                    goto case State_Result;
                                }
                                break;

                            case State_Result:
                                if (await resultEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    current = resultEnumerator.Current;
                                    return true;
                                }

                                mode = State_Source;
                                goto case State_Source; // loop
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }
    }
}
