// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerableEx
    {
        // REVIEW: Add support for IAsyncDisposable resources.

        public static IAsyncEnumerable<TSource> Using<TSource, TResource>(Func<TResource> resourceFactory, Func<TResource, IAsyncEnumerable<TSource>> enumerableFactory) where TResource : IDisposable
        {
            if (resourceFactory == null)
                throw Error.ArgumentNull(nameof(resourceFactory));
            if (enumerableFactory == null)
                throw Error.ArgumentNull(nameof(enumerableFactory));

#if USE_ASYNC_ITERATOR
            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                using (var resource = resourceFactory())
                {
                    await foreach (var item in AsyncEnumerableExtensions.WithCancellation(enumerableFactory(resource), cancellationToken).ConfigureAwait(false))
                    {
                        yield return item;
                    }
                }
            }
#else
            return new UsingAsyncIterator<TSource, TResource>(resourceFactory, enumerableFactory);
#endif
        }

        public static IAsyncEnumerable<TSource> Using<TSource, TResource>(Func<Task<TResource>> resourceFactory, Func<TResource, ValueTask<IAsyncEnumerable<TSource>>> enumerableFactory) where TResource : IDisposable
        {
            if (resourceFactory == null)
                throw Error.ArgumentNull(nameof(resourceFactory));
            if (enumerableFactory == null)
                throw Error.ArgumentNull(nameof(enumerableFactory));

#if USE_ASYNC_ITERATOR
            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                using (var resource = await resourceFactory().ConfigureAwait(false))
                {
                    await foreach (var item in AsyncEnumerableExtensions.WithCancellation((await enumerableFactory(resource).ConfigureAwait(false)), cancellationToken).ConfigureAwait(false))
                    {
                        yield return item;
                    }
                }
            }
#else
            return new UsingAsyncIteratorWithTask<TSource, TResource>(resourceFactory, enumerableFactory);
#endif
        }

#if !NO_DEEP_CANCELLATION
        public static IAsyncEnumerable<TSource> Using<TSource, TResource>(Func<CancellationToken, Task<TResource>> resourceFactory, Func<TResource, CancellationToken, ValueTask<IAsyncEnumerable<TSource>>> enumerableFactory) where TResource : IDisposable
        {
            if (resourceFactory == null)
                throw Error.ArgumentNull(nameof(resourceFactory));
            if (enumerableFactory == null)
                throw Error.ArgumentNull(nameof(enumerableFactory));

#if USE_ASYNC_ITERATOR
            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                using (var resource = await resourceFactory(cancellationToken).ConfigureAwait(false))
                {
                    await foreach (var item in AsyncEnumerableExtensions.WithCancellation((await enumerableFactory(resource, cancellationToken).ConfigureAwait(false)), cancellationToken).ConfigureAwait(false))
                    {
                        yield return item;
                    }
                }
            }
#else
            return new UsingAsyncIteratorWithTaskAndCancellation<TSource, TResource>(resourceFactory, enumerableFactory);
#endif
        }
#endif

#if !USE_ASYNC_ITERATOR
        private sealed class UsingAsyncIterator<TSource, TResource> : AsyncIterator<TSource> where TResource : IDisposable
        {
            private readonly Func<TResource, IAsyncEnumerable<TSource>> _enumerableFactory;
            private readonly Func<TResource> _resourceFactory;

            private IAsyncEnumerator<TSource> _enumerator;
            private TResource _resource;

            public UsingAsyncIterator(Func<TResource> resourceFactory, Func<TResource, IAsyncEnumerable<TSource>> enumerableFactory)
            {
                Debug.Assert(resourceFactory != null);
                Debug.Assert(enumerableFactory != null);

                _resourceFactory = resourceFactory;
                _enumerableFactory = enumerableFactory;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new UsingAsyncIterator<TSource, TResource>(_resourceFactory, _enumerableFactory);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                }

                if (_resource != null)
                {
                    _resource.Dispose();
                    _resource = default;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                // NB: Earlier behavior of this operator was more eager, causing the resource factory to be called upon calling
                //     GetAsyncEnumerator. This is inconsistent with asynchronous "using" and with a C# 8.0 async iterator with
                //     a using statement inside, so this logic got moved to MoveNextAsync instead.

                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _resource = _resourceFactory();
                        _enumerator = _enumerableFactory(_resource).GetAsyncEnumerator(_cancellationToken);
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            _current = _enumerator.Current;
                            return true;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
        }

        private sealed class UsingAsyncIteratorWithTask<TSource, TResource> : AsyncIterator<TSource> where TResource : IDisposable
        {
            private readonly Func<TResource, ValueTask<IAsyncEnumerable<TSource>>> _enumerableFactory;
            private readonly Func<Task<TResource>> _resourceFactory;

            private IAsyncEnumerable<TSource> _enumerable;
            private IAsyncEnumerator<TSource> _enumerator;
            private TResource _resource;

            public UsingAsyncIteratorWithTask(Func<Task<TResource>> resourceFactory, Func<TResource, ValueTask<IAsyncEnumerable<TSource>>> enumerableFactory)
            {
                Debug.Assert(resourceFactory != null);
                Debug.Assert(enumerableFactory != null);

                _resourceFactory = resourceFactory;
                _enumerableFactory = enumerableFactory;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new UsingAsyncIteratorWithTask<TSource, TResource>(_resourceFactory, _enumerableFactory);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                }

                if (_resource != null)
                {
                    _resource.Dispose();
                    _resource = default;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _resource = await _resourceFactory().ConfigureAwait(false);
                        _enumerable = await _enumerableFactory(_resource).ConfigureAwait(false);

                        _enumerator = _enumerable.GetAsyncEnumerator(_cancellationToken);
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            _current = _enumerator.Current;
                            return true;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
        }

#if !NO_DEEP_CANCELLATION
        private sealed class UsingAsyncIteratorWithTaskAndCancellation<TSource, TResource> : AsyncIterator<TSource> where TResource : IDisposable
        {
            private readonly Func<TResource, CancellationToken, ValueTask<IAsyncEnumerable<TSource>>> _enumerableFactory;
            private readonly Func<CancellationToken, Task<TResource>> _resourceFactory;

            private IAsyncEnumerable<TSource> _enumerable;
            private IAsyncEnumerator<TSource> _enumerator;
            private TResource _resource;

            public UsingAsyncIteratorWithTaskAndCancellation(Func<CancellationToken, Task<TResource>> resourceFactory, Func<TResource, CancellationToken, ValueTask<IAsyncEnumerable<TSource>>> enumerableFactory)
            {
                Debug.Assert(resourceFactory != null);
                Debug.Assert(enumerableFactory != null);

                _resourceFactory = resourceFactory;
                _enumerableFactory = enumerableFactory;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new UsingAsyncIteratorWithTaskAndCancellation<TSource, TResource>(_resourceFactory, _enumerableFactory);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                }

                if (_resource != null)
                {
                    _resource.Dispose();
                    _resource = default;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _resource = await _resourceFactory(_cancellationToken).ConfigureAwait(false);
                        _enumerable = await _enumerableFactory(_resource, _cancellationToken).ConfigureAwait(false);

                        _enumerator = _enumerable.GetAsyncEnumerator(_cancellationToken);
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            _current = _enumerator.Current;
                            return true;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
        }
#endif
#endif
    }
}
