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
        /// <summary>
        /// Returns an async-enumerable sequence that invokes the specified factory function whenever a new observer subscribes.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the sequence returned by the factory function, and in the resulting sequence.</typeparam>
        /// <param name="factory">The async-enumerable factory function to invoke for each consumer that starts enumerating the resulting asynchronous sequence.</param>
        /// <returns>An async-enumerable sequence whose observers trigger an invocation of the given async-enumerable factory function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="factory"/> is null.</exception>
        public static IAsyncEnumerable<TSource> Defer<TSource>(Func<IAsyncEnumerable<TSource>> factory)
        {
            if (factory == null)
                throw Error.ArgumentNull(nameof(factory));

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                await foreach (var item in factory().WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Returns an async-enumerable sequence that starts the specified asynchronous factory function whenever a new observer subscribes.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the sequence returned by the factory function, and in the resulting sequence.</typeparam>
        /// <param name="factory">Asynchronous factory function to start for each consumer that starts enumerating the resulting asynchronous sequence.</param>
        /// <returns>An async-enumerable sequence whose observers trigger the given asynchronous async-enumerable factory function to be started.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="factory"/> is null.</exception>
        /// <remarks>This operator is especially useful in conjunction with the asynchronous programming features introduced in C# 5.0 and Visual Basic 11.</remarks>
        public static IAsyncEnumerable<TSource> Defer<TSource>(Func<Task<IAsyncEnumerable<TSource>>> factory)
        {
            if (factory == null)
                throw Error.ArgumentNull(nameof(factory));

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                await foreach (var item in (await factory().ConfigureAwait(false)).WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    yield return item;
                }
            }
        }

#if !NO_DEEP_CANCELLATION
        /// <summary>
        /// Returns an async-enumerable sequence that starts the specified cancellable asynchronous factory function whenever a new observer subscribes.
        /// The CancellationToken passed to the asynchronous factory function is tied to the returned disposable subscription, allowing best-effort cancellation.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the sequence returned by the factory function, and in the resulting sequence.</typeparam>
        /// <param name="factory">Asynchronous factory function, supporting cancellation, to start for each consumer that starts enumerating the resulting asynchronous sequence.</param>
        /// <returns>An async-enumerable sequence whose observers trigger the given asynchronous async-enumerable factory function to be started.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="factory"/> is null.</exception>
        /// <remarks>This operator is especially useful in conjunction with the asynchronous programming features introduced in C# 5.0 and Visual Basic 11.</remarks>
        /// <remarks>When a subscription to the resulting sequence is disposed, the CancellationToken that was fed to the asynchronous async-enumerable factory function will be signaled.</remarks>
        public static IAsyncEnumerable<TSource> Defer<TSource>(Func<CancellationToken, Task<IAsyncEnumerable<TSource>>> factory)
        {
            if (factory == null)
                throw Error.ArgumentNull(nameof(factory));

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                await foreach (var item in (await factory(cancellationToken).ConfigureAwait(false)).WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    yield return item;
                }
            }
        }
#endif
    }
}
