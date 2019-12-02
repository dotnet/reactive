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

        /// <summary>
        /// Constructs an async-enumerable sequence that depends on a resource object, whose lifetime is tied to the resulting async-enumerable sequence's lifetime.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the produced sequence.</typeparam>
        /// <typeparam name="TResource">The type of the resource used during the generation of the resulting sequence. Needs to implement <see cref="IDisposable"/>.</typeparam>
        /// <param name="resourceFactory">Factory function to obtain a resource object.</param>
        /// <param name="enumerableFactory">Factory function to obtain an async-enumerable sequence that depends on the obtained resource.</param>
        /// <returns>An async-enumerable sequence whose lifetime controls the lifetime of the dependent resource object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="resourceFactory"/> or <paramref name="enumerableFactory"/> is null.</exception>
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

        /// <summary>
        /// Constructs an async-enumerable sequence that depends on a resource object, whose lifetime is tied to the resulting async-enumerable sequence's lifetime.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the produced sequence.</typeparam>
        /// <typeparam name="TResource">The type of the resource used during the generation of the resulting sequence. Needs to implement <see cref="IDisposable"/>.</typeparam>
        /// <param name="resourceFactory">Asynchronous factory function to obtain a resource object.</param>
        /// <param name="enumerableFactory">Asynchronous factory function to obtain an async-enumerable sequence that depends on the obtained resource.</param>
        /// <returns>An async-enumerable sequence whose lifetime controls the lifetime of the dependent resource object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="resourceFactory"/> or <paramref name="enumerableFactory"/> is null.</exception>
        /// <remarks>This operator is especially useful in conjunction with the asynchronous programming features introduced in C# 5.0 and Visual Basic 11.</remarks>
        /// <remarks>When a subscription to the resulting sequence is disposed, the CancellationToken that was fed to the asynchronous resource factory and async-enumerable factory functions will be signaled.</remarks>
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
        /// <summary>
        /// Constructs an async-enumerable sequence that depends on a resource object, whose lifetime is tied to the resulting async-enumerable sequence's lifetime. The resource is obtained and used through asynchronous methods.
        /// The CancellationToken passed to the asynchronous methods is tied to the returned disposable subscription, allowing best-effort cancellation at any stage of the resource acquisition or usage.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the produced sequence.</typeparam>
        /// <typeparam name="TResource">The type of the resource used during the generation of the resulting sequence. Needs to implement <see cref="IDisposable"/>.</typeparam>
        /// <param name="resourceFactory">Asynchronous factory function to obtain a resource object.</param>
        /// <param name="enumerableFactory">Asynchronous factory function to obtain an async-enumerable sequence that depends on the obtained resource.</param>
        /// <returns>An async-enumerable sequence whose lifetime controls the lifetime of the dependent resource object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="resourceFactory"/> or <paramref name="enumerableFactory"/> is null.</exception>
        /// <remarks>This operator is especially useful in conjunction with the asynchronous programming features introduced in C# 5.0 and Visual Basic 11.</remarks>
        /// <remarks>When a subscription to the resulting sequence is disposed, the CancellationToken that was fed to the asynchronous resource factory and async-enumerable factory functions will be signaled.</remarks>
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
