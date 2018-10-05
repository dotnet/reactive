// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerableEx
    {
        public static IAsyncEnumerable<TSource> Using<TSource, TResource>(Func<TResource> resourceFactory, Func<TResource, IAsyncEnumerable<TSource>> enumerableFactory) where TResource : IDisposable
        {
            if (resourceFactory == null)
                throw new ArgumentNullException(nameof(resourceFactory));
            if (enumerableFactory == null)
                throw new ArgumentNullException(nameof(enumerableFactory));

            return new UsingAsyncIterator<TSource, TResource>(resourceFactory, enumerableFactory);
        }

        public static IAsyncEnumerable<TSource> Using<TSource, TResource>(Func<Task<TResource>> resourceFactory, Func<TResource, Task<IAsyncEnumerable<TSource>>> enumerableFactory) where TResource : IDisposable
        {
            if (resourceFactory == null)
                throw new ArgumentNullException(nameof(resourceFactory));
            if (enumerableFactory == null)
                throw new ArgumentNullException(nameof(enumerableFactory));

            return new UsingAsyncIteratorWithTask<TSource, TResource>(resourceFactory, enumerableFactory);
        }

        private sealed class UsingAsyncIterator<TSource, TResource> : AsyncIterator<TSource> where TResource : IDisposable
        {
            private readonly Func<TResource, IAsyncEnumerable<TSource>> enumerableFactory;
            private readonly Func<TResource> resourceFactory;

            private IAsyncEnumerable<TSource> enumerable;
            private IAsyncEnumerator<TSource> enumerator;
            private TResource resource;

            public UsingAsyncIterator(Func<TResource> resourceFactory, Func<TResource, IAsyncEnumerable<TSource>> enumerableFactory)
            {
                Debug.Assert(resourceFactory != null);
                Debug.Assert(enumerableFactory != null);

                this.resourceFactory = resourceFactory;
                this.enumerableFactory = enumerableFactory;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new UsingAsyncIterator<TSource, TResource>(resourceFactory, enumerableFactory);
            }

            public override async ValueTask DisposeAsync()
            {
                if (enumerator != null)
                {
                    await enumerator.DisposeAsync().ConfigureAwait(false);
                    enumerator = null;
                }

                if (resource != null)
                {
                    resource.Dispose();
                    resource = default(TResource);
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = enumerable.GetAsyncEnumerator();
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            current = enumerator.Current;
                            return true;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }

            protected override void OnGetEnumerator()
            {
                resource = resourceFactory();
                enumerable = enumerableFactory(resource);

                base.OnGetEnumerator();
            }
        }

        private sealed class UsingAsyncIteratorWithTask<TSource, TResource> : AsyncIterator<TSource> where TResource : IDisposable
        {
            private readonly Func<TResource, Task<IAsyncEnumerable<TSource>>> enumerableFactory;
            private readonly Func<Task<TResource>> resourceFactory;

            private IAsyncEnumerable<TSource> enumerable;
            private IAsyncEnumerator<TSource> enumerator;
            private TResource resource;

            public UsingAsyncIteratorWithTask(Func<Task<TResource>> resourceFactory, Func<TResource, Task<IAsyncEnumerable<TSource>>> enumerableFactory)
            {
                Debug.Assert(resourceFactory != null);
                Debug.Assert(enumerableFactory != null);

                this.resourceFactory = resourceFactory;
                this.enumerableFactory = enumerableFactory;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new UsingAsyncIteratorWithTask<TSource, TResource>(resourceFactory, enumerableFactory);
            }

            public override async ValueTask DisposeAsync()
            {
                if (enumerator != null)
                {
                    await enumerator.DisposeAsync().ConfigureAwait(false);
                    enumerator = null;
                }

                if (resource != null)
                {
                    resource.Dispose();
                    resource = default(TResource);
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        resource = await resourceFactory().ConfigureAwait(false);
                        enumerable = await enumerableFactory(resource).ConfigureAwait(false);

                        enumerator = enumerable.GetAsyncEnumerator();
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            current = enumerator.Current;
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
