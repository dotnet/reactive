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
        public static IAsyncEnumerable<TSource> OnErrorResumeNext<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            return OnErrorResumeNext_(new[] { first, second });
        }

        public static IAsyncEnumerable<TSource> OnErrorResumeNext<TSource>(params IAsyncEnumerable<TSource>[] sources)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            return OnErrorResumeNext_(sources);
        }

        public static IAsyncEnumerable<TSource> OnErrorResumeNext<TSource>(this IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            return OnErrorResumeNext_(sources);
        }

        private static IAsyncEnumerable<TSource> OnErrorResumeNext_<TSource>(IEnumerable<IAsyncEnumerable<TSource>> sources)
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

            public override void Dispose()
            {
                if (sourcesEnumerator != null)
                {
                    sourcesEnumerator.Dispose();
                    sourcesEnumerator = null;
                }

                if (enumerator != null)
                {
                    enumerator.Dispose();
                    enumerator = null;
                }

                base.Dispose();
            }

            protected override async Task<bool> MoveNextCore(CancellationToken cancellationToken)
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

                                enumerator = sourcesEnumerator.Current.GetEnumerator();
                            }

                            try
                            {
                                if (await enumerator.MoveNext(cancellationToken)
                                                    .ConfigureAwait(false))
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
                            enumerator.Dispose();
                            enumerator = null;
                        }

                        break; // case

                }

                Dispose();
                return false;
            }
        }
    }
}