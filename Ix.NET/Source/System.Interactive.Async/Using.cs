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
        public static IAsyncEnumerable<TSource> Using<TSource, TResource>(Func<TResource> resourceFactory, Func<TResource, IAsyncEnumerable<TSource>> enumerableFactory) where TResource : IDisposable
        {
            if (resourceFactory == null)
            {
                throw new ArgumentNullException(nameof(resourceFactory));
            }

            if (enumerableFactory == null)
            {
                throw new ArgumentNullException(nameof(enumerableFactory));
            }

            return new UsingAsyncIterator<TSource, TResource>(resourceFactory, enumerableFactory);
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

            public override void Dispose()
            {
                if (enumerator != null)
                {
                    enumerator.Dispose();
                    enumerator = null;
                }

                if (resource != null)
                {
                    resource.Dispose();
                    resource = default;
                }

                base.Dispose();
            }

            protected override async Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = enumerable.GetEnumerator();
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (await enumerator.MoveNext(cancellationToken)
                                               .ConfigureAwait(false))
                        {
                            current = enumerator.Current;
                            return true;
                        }

                        Dispose();
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
    }
}