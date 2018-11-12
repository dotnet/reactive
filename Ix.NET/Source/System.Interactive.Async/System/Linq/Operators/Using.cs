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
        public static IAsyncEnumerable<TSource> Using<TSource, TResource>(Func<TResource> resourceFactory, Func<TResource, IAsyncEnumerable<TSource>> enumerableFactory) where TResource : IDisposable
        {
            if (resourceFactory == null)
                throw Error.ArgumentNull(nameof(resourceFactory));
            if (enumerableFactory == null)
                throw Error.ArgumentNull(nameof(enumerableFactory));

            return new UsingAsyncIterator<TSource, TResource>(resourceFactory, enumerableFactory);
        }

        public static IAsyncEnumerable<TSource> Using<TSource, TResource>(Func<Task<TResource>> resourceFactory, Func<TResource, Task<IAsyncEnumerable<TSource>>> enumerableFactory) where TResource : IDisposable
        {
            if (resourceFactory == null)
                throw Error.ArgumentNull(nameof(resourceFactory));
            if (enumerableFactory == null)
                throw Error.ArgumentNull(nameof(enumerableFactory));

            return new UsingAsyncIteratorWithTask<TSource, TResource>(resourceFactory, enumerableFactory);
        }

        private sealed class UsingAsyncIterator<TSource, TResource> : AsyncIterator<TSource> where TResource : IDisposable
        {
            private readonly Func<TResource, IAsyncEnumerable<TSource>> _enumerableFactory;
            private readonly Func<TResource> _resourceFactory;

            private IAsyncEnumerable<TSource> _enumerable;
            private IAsyncEnumerator<TSource> _enumerator;
            private TResource _resource;

            public UsingAsyncIterator(Func<TResource> resourceFactory, Func<TResource, IAsyncEnumerable<TSource>> enumerableFactory)
            {
                Debug.Assert(resourceFactory != null);
                Debug.Assert(enumerableFactory != null);

                _resourceFactory = resourceFactory;
                _enumerableFactory = enumerableFactory;
            }

            public override AsyncIterator<TSource> Clone()
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

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _enumerable.GetAsyncEnumerator(cancellationToken);
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            current = _enumerator.Current;
                            return true;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }

            protected override void OnGetEnumerator(CancellationToken cancellationToken)
            {
                // REVIEW: Wire cancellation to the functions.

                _resource = _resourceFactory();
                _enumerable = _enumerableFactory(_resource);

                base.OnGetEnumerator(cancellationToken);
            }
        }

        private sealed class UsingAsyncIteratorWithTask<TSource, TResource> : AsyncIterator<TSource> where TResource : IDisposable
        {
            private readonly Func<TResource, Task<IAsyncEnumerable<TSource>>> _enumerableFactory;
            private readonly Func<Task<TResource>> _resourceFactory;

            private IAsyncEnumerable<TSource> _enumerable;
            private IAsyncEnumerator<TSource> _enumerator;
            private TResource _resource;

            public UsingAsyncIteratorWithTask(Func<Task<TResource>> resourceFactory, Func<TResource, Task<IAsyncEnumerable<TSource>>> enumerableFactory)
            {
                Debug.Assert(resourceFactory != null);
                Debug.Assert(enumerableFactory != null);

                _resourceFactory = resourceFactory;
                _enumerableFactory = enumerableFactory;
            }

            public override AsyncIterator<TSource> Clone()
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

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        _resource = await _resourceFactory().ConfigureAwait(false);
                        _enumerable = await _enumerableFactory(_resource).ConfigureAwait(false);

                        _enumerator = _enumerable.GetAsyncEnumerator(cancellationToken);
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            current = _enumerator.Current;
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
