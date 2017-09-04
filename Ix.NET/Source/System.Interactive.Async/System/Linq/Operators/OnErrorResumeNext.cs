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
        public static IAsyncEnumerable<TSource> OnErrorResumeNext<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return OnErrorResumeNextCore(new[] { first, second });
        }

        public static IAsyncEnumerable<TSource> OnErrorResumeNext<TSource>(params IAsyncEnumerable<TSource>[] sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return OnErrorResumeNextCore(sources);
        }

        public static IAsyncEnumerable<TSource> OnErrorResumeNext<TSource>(this IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return OnErrorResumeNextCore(sources);
        }

        private static IAsyncEnumerable<TSource> OnErrorResumeNextCore<TSource>(IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            return new OnErrorResumeNextAsyncIterator<TSource>(sources);
        }

        private sealed class OnErrorResumeNextAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly IEnumerable<IAsyncEnumerable<TSource>> sources;

            private IAsyncEnumerator<TSource> enumerator;
            private IEnumerator<IAsyncEnumerable<TSource>> sourcesEnumerator;

            public OnErrorResumeNextAsyncIterator(IEnumerable<IAsyncEnumerable<TSource>> sources)
            {
                Debug.Assert(sources != null);

                this.sources = sources;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new OnErrorResumeNextAsyncIterator<TSource>(sources);
            }

            public override async Task DisposeAsync()
            {
                if (sourcesEnumerator != null)
                {
                    sourcesEnumerator.Dispose();
                    sourcesEnumerator = null;
                }

                if (enumerator != null)
                {
                    await enumerator.DisposeAsync().ConfigureAwait(false);
                    enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async Task<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        sourcesEnumerator = sources.GetEnumerator();

                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (true)
                        {
                            if (enumerator == null)
                            {
                                if (!sourcesEnumerator.MoveNext())
                                {
                                    break; // while -- done, nothing else to do
                                }

                                enumerator = sourcesEnumerator.Current.GetAsyncEnumerator();
                            }

                            try
                            {
                                if (await enumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    current = enumerator.Current;
                                    return true;
                                }
                            }
                            catch
                            {
                                // Ignore
                            }

                            // Done with the current one, go to the next
                            await enumerator.DisposeAsync().ConfigureAwait(false);
                            enumerator = null;
                        }

                        break; // case
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }
    }
}
