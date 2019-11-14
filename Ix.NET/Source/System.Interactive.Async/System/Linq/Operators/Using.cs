// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
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

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                using var resource = resourceFactory();

                await foreach (var item in enumerableFactory(resource).WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    yield return item;
                }
            }
        }

        public static IAsyncEnumerable<TSource> Using<TSource, TResource>(Func<Task<TResource>> resourceFactory, Func<TResource, ValueTask<IAsyncEnumerable<TSource>>> enumerableFactory) where TResource : IDisposable
        {
            if (resourceFactory == null)
                throw Error.ArgumentNull(nameof(resourceFactory));
            if (enumerableFactory == null)
                throw Error.ArgumentNull(nameof(enumerableFactory));

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                using var resource = await resourceFactory().ConfigureAwait(false);

                await foreach (var item in (await enumerableFactory(resource).ConfigureAwait(false)).WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    yield return item;
                }
            }
        }

#if !NO_DEEP_CANCELLATION
        public static IAsyncEnumerable<TSource> Using<TSource, TResource>(Func<CancellationToken, Task<TResource>> resourceFactory, Func<TResource, CancellationToken, ValueTask<IAsyncEnumerable<TSource>>> enumerableFactory) where TResource : IDisposable
        {
            if (resourceFactory == null)
                throw Error.ArgumentNull(nameof(resourceFactory));
            if (enumerableFactory == null)
                throw Error.ArgumentNull(nameof(enumerableFactory));

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                using var resource = await resourceFactory(cancellationToken).ConfigureAwait(false);

                await foreach (var item in (await enumerableFactory(resource, cancellationToken).ConfigureAwait(false)).WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    yield return item;
                }
            }
        }
#endif
    }
}
