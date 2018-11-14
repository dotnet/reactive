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
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return new SelectManyAsyncIterator<TSource, TResult>(source, selector);
        }

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<IAsyncEnumerable<TResult>>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return new SelectManyAsyncIteratorWithTask<TSource, TResult>(source, selector);
        }

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, IAsyncEnumerable<TResult>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return new SelectManyWithIndexAsyncIterator<TSource, TResult>(source, selector);
        }

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<IAsyncEnumerable<TResult>>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return new SelectManyWithIndexAsyncIteratorWithTask<TSource, TResult>(source, selector);
        }

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, IAsyncEnumerable<TCollection>> selector, Func<TSource, TCollection, TResult> resultSelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));
            if (resultSelector == null)
                throw Error.ArgumentNull(nameof(resultSelector));

            return new SelectManyAsyncIterator<TSource, TCollection, TResult>(source, selector, resultSelector);
        }

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<IAsyncEnumerable<TCollection>>> selector, Func<TSource, TCollection, ValueTask<TResult>> resultSelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));
            if (resultSelector == null)
                throw Error.ArgumentNull(nameof(resultSelector));

            return new SelectManyAsyncIteratorWithTask<TSource, TCollection, TResult>(source, selector, resultSelector);
        }

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, IAsyncEnumerable<TCollection>> selector, Func<TSource, TCollection, TResult> resultSelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));
            if (resultSelector == null)
                throw Error.ArgumentNull(nameof(resultSelector));

            return new SelectManyWithIndexAsyncIterator<TSource, TCollection, TResult>(source, selector, resultSelector);
        }

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<IAsyncEnumerable<TCollection>>> selector, Func<TSource, TCollection, ValueTask<TResult>> resultSelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));
            if (resultSelector == null)
                throw Error.ArgumentNull(nameof(resultSelector));

            return new SelectManyWithIndexAsyncIteratorWithTask<TSource, TCollection, TResult>(source, selector, resultSelector);
        }

        private sealed class SelectManyAsyncIterator<TSource, TResult> : AsyncIterator<TResult>
        {
            private const int State_Source = 1;
            private const int State_Result = 2;

            private readonly Func<TSource, IAsyncEnumerable<TResult>> _selector;
            private readonly IAsyncEnumerable<TSource> _source;

            private int _mode;
            private IAsyncEnumerator<TResult> _resultEnumerator;
            private IAsyncEnumerator<TSource> _sourceEnumerator;

            public SelectManyAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, IAsyncEnumerable<TResult>> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                _source = source;
                _selector = selector;
            }

            public override AsyncIteratorBase<TResult> Clone()
            {
                return new SelectManyAsyncIterator<TSource, TResult>(_source, _selector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_resultEnumerator != null)
                {
                    await _resultEnumerator.DisposeAsync().ConfigureAwait(false);
                    _resultEnumerator = null;
                }

                if (_sourceEnumerator != null)
                {
                    await _sourceEnumerator.DisposeAsync().ConfigureAwait(false);
                    _sourceEnumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _sourceEnumerator = _source.GetAsyncEnumerator(_cancellationToken);
                        _mode = State_Source;
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (_mode)
                        {
                            case State_Source:
                                if (await _sourceEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    if (_resultEnumerator != null)
                                    {
                                        await _resultEnumerator.DisposeAsync().ConfigureAwait(false);
                                    }

                                    var inner = _selector(_sourceEnumerator.Current);
                                    _resultEnumerator = inner.GetAsyncEnumerator(_cancellationToken);

                                    _mode = State_Result;
                                    goto case State_Result;
                                }
                                break;

                            case State_Result:
                                if (await _resultEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    _current = _resultEnumerator.Current;
                                    return true;
                                }

                                _mode = State_Source;
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

            private readonly Func<TSource, ValueTask<IAsyncEnumerable<TResult>>> _selector;
            private readonly IAsyncEnumerable<TSource> _source;

            private int _mode;
            private IAsyncEnumerator<TResult> _resultEnumerator;
            private IAsyncEnumerator<TSource> _sourceEnumerator;

            public SelectManyAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<IAsyncEnumerable<TResult>>> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                _source = source;
                _selector = selector;
            }

            public override AsyncIteratorBase<TResult> Clone()
            {
                return new SelectManyAsyncIteratorWithTask<TSource, TResult>(_source, _selector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_resultEnumerator != null)
                {
                    await _resultEnumerator.DisposeAsync().ConfigureAwait(false);
                    _resultEnumerator = null;
                }

                if (_sourceEnumerator != null)
                {
                    await _sourceEnumerator.DisposeAsync().ConfigureAwait(false);
                    _sourceEnumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _sourceEnumerator = _source.GetAsyncEnumerator(_cancellationToken);
                        _mode = State_Source;
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (_mode)
                        {
                            case State_Source:
                                if (await _sourceEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    if (_resultEnumerator != null)
                                    {
                                        await _resultEnumerator.DisposeAsync().ConfigureAwait(false);
                                    }

                                    var inner = await _selector(_sourceEnumerator.Current).ConfigureAwait(false);
                                    _resultEnumerator = inner.GetAsyncEnumerator(_cancellationToken);

                                    _mode = State_Result;
                                    goto case State_Result;
                                }
                                break;

                            case State_Result:
                                if (await _resultEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    _current = _resultEnumerator.Current;
                                    return true;
                                }

                                _mode = State_Source;
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

            private readonly Func<TSource, IAsyncEnumerable<TCollection>> _collectionSelector;
            private readonly Func<TSource, TCollection, TResult> _resultSelector;
            private readonly IAsyncEnumerable<TSource> _source;

            private TSource _currentSource;
            private int _mode;
            private IAsyncEnumerator<TCollection> _resultEnumerator;
            private IAsyncEnumerator<TSource> _sourceEnumerator;

            public SelectManyAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, IAsyncEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
            {
                Debug.Assert(source != null);
                Debug.Assert(collectionSelector != null);
                Debug.Assert(resultSelector != null);

                _source = source;
                _collectionSelector = collectionSelector;
                _resultSelector = resultSelector;
            }

            public override AsyncIteratorBase<TResult> Clone()
            {
                return new SelectManyAsyncIterator<TSource, TCollection, TResult>(_source, _collectionSelector, _resultSelector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_resultEnumerator != null)
                {
                    await _resultEnumerator.DisposeAsync().ConfigureAwait(false);
                    _resultEnumerator = null;
                }

                if (_sourceEnumerator != null)
                {
                    await _sourceEnumerator.DisposeAsync().ConfigureAwait(false);
                    _sourceEnumerator = null;
                }

                _currentSource = default;

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _sourceEnumerator = _source.GetAsyncEnumerator(_cancellationToken);
                        _mode = State_Source;
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (_mode)
                        {
                            case State_Source:
                                if (await _sourceEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    if (_resultEnumerator != null)
                                    {
                                        await _resultEnumerator.DisposeAsync().ConfigureAwait(false);
                                    }

                                    _currentSource = _sourceEnumerator.Current;
                                    var inner = _collectionSelector(_currentSource);
                                    _resultEnumerator = inner.GetAsyncEnumerator(_cancellationToken);

                                    _mode = State_Result;
                                    goto case State_Result;
                                }
                                break;

                            case State_Result:
                                if (await _resultEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    _current = _resultSelector(_currentSource, _resultEnumerator.Current);
                                    return true;
                                }

                                _mode = State_Source;
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

            private readonly Func<TSource, ValueTask<IAsyncEnumerable<TCollection>>> _collectionSelector;
            private readonly Func<TSource, TCollection, ValueTask<TResult>> _resultSelector;
            private readonly IAsyncEnumerable<TSource> _source;

            private TSource _currentSource;
            private int _mode;
            private IAsyncEnumerator<TCollection> _resultEnumerator;
            private IAsyncEnumerator<TSource> _sourceEnumerator;

            public SelectManyAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<IAsyncEnumerable<TCollection>>> collectionSelector, Func<TSource, TCollection, ValueTask<TResult>> resultSelector)
            {
                Debug.Assert(source != null);
                Debug.Assert(collectionSelector != null);
                Debug.Assert(resultSelector != null);

                _source = source;
                _collectionSelector = collectionSelector;
                _resultSelector = resultSelector;
            }

            public override AsyncIteratorBase<TResult> Clone()
            {
                return new SelectManyAsyncIteratorWithTask<TSource, TCollection, TResult>(_source, _collectionSelector, _resultSelector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_resultEnumerator != null)
                {
                    await _resultEnumerator.DisposeAsync().ConfigureAwait(false);
                    _resultEnumerator = null;
                }

                if (_sourceEnumerator != null)
                {
                    await _sourceEnumerator.DisposeAsync().ConfigureAwait(false);
                    _sourceEnumerator = null;
                }

                _currentSource = default;

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _sourceEnumerator = _source.GetAsyncEnumerator(_cancellationToken);
                        _mode = State_Source;
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (_mode)
                        {
                            case State_Source:
                                if (await _sourceEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    if (_resultEnumerator != null)
                                    {
                                        await _resultEnumerator.DisposeAsync().ConfigureAwait(false);
                                    }

                                    _currentSource = _sourceEnumerator.Current;
                                    var inner = await _collectionSelector(_currentSource).ConfigureAwait(false);
                                    _resultEnumerator = inner.GetAsyncEnumerator(_cancellationToken);

                                    _mode = State_Result;
                                    goto case State_Result;
                                }
                                break;

                            case State_Result:
                                if (await _resultEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    _current = await _resultSelector(_currentSource, _resultEnumerator.Current).ConfigureAwait(false);
                                    return true;
                                }

                                _mode = State_Source;
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

            private readonly Func<TSource, int, IAsyncEnumerable<TCollection>> _collectionSelector;
            private readonly Func<TSource, TCollection, TResult> _resultSelector;
            private readonly IAsyncEnumerable<TSource> _source;

            private TSource _currentSource;
            private int _index;
            private int _mode;
            private IAsyncEnumerator<TCollection> _resultEnumerator;
            private IAsyncEnumerator<TSource> _sourceEnumerator;

            public SelectManyWithIndexAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, int, IAsyncEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
            {
                Debug.Assert(source != null);
                Debug.Assert(collectionSelector != null);
                Debug.Assert(resultSelector != null);

                _source = source;
                _collectionSelector = collectionSelector;
                _resultSelector = resultSelector;
            }

            public override AsyncIteratorBase<TResult> Clone()
            {
                return new SelectManyWithIndexAsyncIterator<TSource, TCollection, TResult>(_source, _collectionSelector, _resultSelector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_resultEnumerator != null)
                {
                    await _resultEnumerator.DisposeAsync().ConfigureAwait(false);
                    _resultEnumerator = null;
                }

                if (_sourceEnumerator != null)
                {
                    await _sourceEnumerator.DisposeAsync().ConfigureAwait(false);
                    _sourceEnumerator = null;
                }

                _currentSource = default;

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _sourceEnumerator = _source.GetAsyncEnumerator(_cancellationToken);
                        _index = -1;
                        _mode = State_Source;
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (_mode)
                        {
                            case State_Source:
                                if (await _sourceEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    if (_resultEnumerator != null)
                                    {
                                        await _resultEnumerator.DisposeAsync().ConfigureAwait(false);
                                    }

                                    _currentSource = _sourceEnumerator.Current;

                                    checked
                                    {
                                        _index++;
                                    }

                                    var inner = _collectionSelector(_currentSource, _index);
                                    _resultEnumerator = inner.GetAsyncEnumerator(_cancellationToken);

                                    _mode = State_Result;
                                    goto case State_Result;
                                }
                                break;

                            case State_Result:
                                if (await _resultEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    _current = _resultSelector(_currentSource, _resultEnumerator.Current);
                                    return true;
                                }

                                _mode = State_Source;
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

            private readonly Func<TSource, int, ValueTask<IAsyncEnumerable<TCollection>>> _collectionSelector;
            private readonly Func<TSource, TCollection, ValueTask<TResult>> _resultSelector;
            private readonly IAsyncEnumerable<TSource> _source;

            private TSource _currentSource;
            private int _index;
            private int _mode;
            private IAsyncEnumerator<TCollection> _resultEnumerator;
            private IAsyncEnumerator<TSource> _sourceEnumerator;

            public SelectManyWithIndexAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<IAsyncEnumerable<TCollection>>> collectionSelector, Func<TSource, TCollection, ValueTask<TResult>> resultSelector)
            {
                Debug.Assert(source != null);
                Debug.Assert(collectionSelector != null);
                Debug.Assert(resultSelector != null);

                _source = source;
                _collectionSelector = collectionSelector;
                _resultSelector = resultSelector;
            }

            public override AsyncIteratorBase<TResult> Clone()
            {
                return new SelectManyWithIndexAsyncIteratorWithTask<TSource, TCollection, TResult>(_source, _collectionSelector, _resultSelector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_resultEnumerator != null)
                {
                    await _resultEnumerator.DisposeAsync().ConfigureAwait(false);
                    _resultEnumerator = null;
                }

                if (_sourceEnumerator != null)
                {
                    await _sourceEnumerator.DisposeAsync().ConfigureAwait(false);
                    _sourceEnumerator = null;
                }

                _currentSource = default;

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _sourceEnumerator = _source.GetAsyncEnumerator(_cancellationToken);
                        _index = -1;
                        _mode = State_Source;
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (_mode)
                        {
                            case State_Source:
                                if (await _sourceEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    if (_resultEnumerator != null)
                                    {
                                        await _resultEnumerator.DisposeAsync().ConfigureAwait(false);
                                    }

                                    _currentSource = _sourceEnumerator.Current;

                                    checked
                                    {
                                        _index++;
                                    }

                                    var inner = await _collectionSelector(_currentSource, _index).ConfigureAwait(false);
                                    _resultEnumerator = inner.GetAsyncEnumerator(_cancellationToken);

                                    _mode = State_Result;
                                    goto case State_Result;
                                }
                                break;

                            case State_Result:
                                if (await _resultEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    _current = await _resultSelector(_currentSource, _resultEnumerator.Current).ConfigureAwait(false);
                                    return true;
                                }

                                _mode = State_Source;
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

            private readonly Func<TSource, int, IAsyncEnumerable<TResult>> _selector;
            private readonly IAsyncEnumerable<TSource> _source;

            private int _index;
            private int _mode;
            private IAsyncEnumerator<TResult> _resultEnumerator;
            private IAsyncEnumerator<TSource> _sourceEnumerator;

            public SelectManyWithIndexAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, int, IAsyncEnumerable<TResult>> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                _source = source;
                _selector = selector;
            }

            public override AsyncIteratorBase<TResult> Clone()
            {
                return new SelectManyWithIndexAsyncIterator<TSource, TResult>(_source, _selector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_resultEnumerator != null)
                {
                    await _resultEnumerator.DisposeAsync().ConfigureAwait(false);
                    _resultEnumerator = null;
                }

                if (_sourceEnumerator != null)
                {
                    await _sourceEnumerator.DisposeAsync().ConfigureAwait(false);
                    _sourceEnumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _sourceEnumerator = _source.GetAsyncEnumerator(_cancellationToken);
                        _index = -1;
                        _mode = State_Source;
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (_mode)
                        {
                            case State_Source:
                                if (await _sourceEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    if (_resultEnumerator != null)
                                    {
                                        await _resultEnumerator.DisposeAsync().ConfigureAwait(false);
                                    }

                                    checked
                                    {
                                        _index++;
                                    }

                                    var inner = _selector(_sourceEnumerator.Current, _index);
                                    _resultEnumerator = inner.GetAsyncEnumerator(_cancellationToken);

                                    _mode = State_Result;
                                    goto case State_Result;
                                }
                                break;

                            case State_Result:
                                if (await _resultEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    _current = _resultEnumerator.Current;
                                    return true;
                                }

                                _mode = State_Source;
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

            private readonly Func<TSource, int, ValueTask<IAsyncEnumerable<TResult>>> _selector;
            private readonly IAsyncEnumerable<TSource> _source;

            private int _index;
            private int _mode;
            private IAsyncEnumerator<TResult> _resultEnumerator;
            private IAsyncEnumerator<TSource> _sourceEnumerator;

            public SelectManyWithIndexAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<IAsyncEnumerable<TResult>>> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                _source = source;
                _selector = selector;
            }

            public override AsyncIteratorBase<TResult> Clone()
            {
                return new SelectManyWithIndexAsyncIteratorWithTask<TSource, TResult>(_source, _selector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_resultEnumerator != null)
                {
                    await _resultEnumerator.DisposeAsync().ConfigureAwait(false);
                    _resultEnumerator = null;
                }

                if (_sourceEnumerator != null)
                {
                    await _sourceEnumerator.DisposeAsync().ConfigureAwait(false);
                    _sourceEnumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _sourceEnumerator = _source.GetAsyncEnumerator(_cancellationToken);
                        _index = -1;
                        _mode = State_Source;
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (_mode)
                        {
                            case State_Source:
                                if (await _sourceEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    if (_resultEnumerator != null)
                                    {
                                        await _resultEnumerator.DisposeAsync().ConfigureAwait(false);
                                    }

                                    checked
                                    {
                                        _index++;
                                    }

                                    var inner = await _selector(_sourceEnumerator.Current, _index).ConfigureAwait(false);
                                    _resultEnumerator = inner.GetAsyncEnumerator(_cancellationToken);

                                    _mode = State_Result;
                                    goto case State_Result;
                                }
                                break;

                            case State_Result:
                                if (await _resultEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    _current = _resultEnumerator.Current;
                                    return true;
                                }

                                _mode = State_Source;
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
