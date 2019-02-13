// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Threading.Tasks
{
    public static class AsyncEnumerableExtensions
    {
#if !BCL_HAS_CONFIGUREAWAIT // https://github.com/dotnet/coreclr/pull/21939

        /// <summary>Configures how awaits on the tasks returned from an async iteration will be performed.</summary>
        /// <typeparam name="T">The type of the objects being iterated.</typeparam>
        /// <param name="source">The source enumerable being iterated.</param>
        /// <param name="continueOnCapturedContext">Whether to capture and marshal back to the current context.</param>
        /// <returns>The configured enumerable.</returns>
        public static ConfiguredCancelableAsyncEnumerable<T> ConfigureAwait<T>(
            this IAsyncEnumerable<T> source, bool continueOnCapturedContext) =>
            new ConfiguredCancelableAsyncEnumerable<T>(source, continueOnCapturedContext, cancellationToken: default);

        /// <summary>Sets the <see cref="CancellationToken"/> to be passed to <see cref="IAsyncEnumerable{T}.GetAsyncEnumerator(CancellationToken)"/> when iterating.</summary>
        /// <typeparam name="T">The type of the objects being iterated.</typeparam>
        /// <param name="source">The source enumerable being iterated.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
        /// <returns>The configured enumerable.</returns>
        public static ConfiguredCancelableAsyncEnumerable<T> WithCancellation<T>(
            this IAsyncEnumerable<T> source, CancellationToken cancellationToken) =>
            new ConfiguredCancelableAsyncEnumerable<T>(source, continueOnCapturedContext: true, cancellationToken);

#endif

#if BCL_HAS_CONFIGUREAWAIT
        public static ConfiguredAsyncEnumerator<T> ConfigureAwait<T>(this IAsyncEnumerator<T> enumerator, bool continueOnCapturedContext)
        {
            if (enumerator == null)
                throw Error.ArgumentNull(nameof(enumerator));

            // NB: We need our own copy of the struct to access the constructor.
            return new ConfiguredAsyncEnumerator<T>(enumerator, continueOnCapturedContext);
        }

        /// <summary>Provides an awaitable async enumerator that enables cancelable iteration and configured awaits.</summary>
        [StructLayout(LayoutKind.Auto)]
        public readonly struct ConfiguredAsyncEnumerator<T>
        {
            private readonly IAsyncEnumerator<T> _enumerator;
            private readonly bool _continueOnCapturedContext;

            internal ConfiguredAsyncEnumerator(IAsyncEnumerator<T> enumerator, bool continueOnCapturedContext)
            {
                _enumerator = enumerator;
                _continueOnCapturedContext = continueOnCapturedContext;
            }

            /// <summary>Advances the enumerator asynchronously to the next element of the collection.</summary>
            /// <returns>
            /// A <see cref="ConfiguredValueTaskAwaitable{Boolean}"/> that will complete with a result of <c>true</c>
            /// if the enumerator was successfully advanced to the next element, or <c>false</c> if the enumerator has
            /// passed the end of the collection.
            /// </returns>
            public ConfiguredValueTaskAwaitable<bool> MoveNextAsync() =>
                _enumerator.MoveNextAsync().ConfigureAwait(_continueOnCapturedContext);

            /// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
            public T Current => _enumerator.Current;

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or
            /// resetting unmanaged resources asynchronously.
            /// </summary>
            public ConfiguredValueTaskAwaitable DisposeAsync() =>
                _enumerator.DisposeAsync().ConfigureAwait(_continueOnCapturedContext);
        }
#else
        public static ConfiguredCancelableAsyncEnumerable<T>.Enumerator ConfigureAwait<T>(this IAsyncEnumerator<T> enumerator, bool continueOnCapturedContext)
        {
            if (enumerator == null)
                throw Error.ArgumentNull(nameof(enumerator));

            return new ConfiguredCancelableAsyncEnumerable<T>.Enumerator(enumerator, continueOnCapturedContext);
        }
#endif
    }
}
