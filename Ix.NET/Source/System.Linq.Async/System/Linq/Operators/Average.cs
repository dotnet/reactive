// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        private static async Task<double> AverageCore(this IAsyncEnumerable<int> source, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw new InvalidOperationException(Strings.NO_ELEMENTS);
                }

                long sum = e.Current;
                long count = 1;
                checked
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        sum += e.Current;
                        ++count;
                    }
                }

                return (double)sum / count;
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

        private static async Task<double?> AverageCore(IAsyncEnumerable<int?> source, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var v = e.Current;
                    if (v.HasValue)
                    {
                        long sum = v.GetValueOrDefault();
                        long count = 1;
                        checked
                        {
                            while (await e.MoveNextAsync().ConfigureAwait(false))
                            {
                                v = e.Current;
                                if (v.HasValue)
                                {
                                    sum += v.GetValueOrDefault();
                                    ++count;
                                }
                            }
                        }

                        return (double)sum / count;
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return null;
        }

        private static async Task<double> AverageCore(IAsyncEnumerable<long> source, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw new InvalidOperationException(Strings.NO_ELEMENTS);
                }

                var sum = e.Current;
                long count = 1;
                checked
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        sum += e.Current;
                        ++count;
                    }
                }

                return (double)sum / count;
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

        private static async Task<double?> AverageCore(IAsyncEnumerable<long?> source, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var v = e.Current;
                    if (v.HasValue)
                    {
                        var sum = v.GetValueOrDefault();
                        long count = 1;
                        checked
                        {
                            while (await e.MoveNextAsync().ConfigureAwait(false))
                            {
                                v = e.Current;
                                if (v.HasValue)
                                {
                                    sum += v.GetValueOrDefault();
                                    ++count;
                                }
                            }
                        }

                        return (double)sum / count;
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return null;
        }

        private static async Task<double> AverageCore(IAsyncEnumerable<double> source, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw new InvalidOperationException(Strings.NO_ELEMENTS);
                }

                var sum = e.Current;
                long count = 1;
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    // There is an opportunity to short-circuit here, in that if e.Current is
                    // ever NaN then the result will always be NaN. Assuming that this case is
                    // rare enough that not checking is the better approach generally.
                    sum += e.Current;
                    ++count;
                }

                return sum / count;
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

        private static async Task<double?> AverageCore(IAsyncEnumerable<double?> source, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var v = e.Current;
                    if (v.HasValue)
                    {
                        var sum = v.GetValueOrDefault();
                        long count = 1;
                        checked
                        {
                            while (await e.MoveNextAsync().ConfigureAwait(false))
                            {
                                v = e.Current;
                                if (v.HasValue)
                                {
                                    sum += v.GetValueOrDefault();
                                    ++count;
                                }
                            }
                        }

                        return sum / count;
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return null;
        }

        private static async Task<float> AverageCore(IAsyncEnumerable<float> source, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw new InvalidOperationException(Strings.NO_ELEMENTS);
                }

                double sum = e.Current;
                long count = 1;
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    sum += e.Current;
                    ++count;
                }

                return (float)(sum / count);
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

        private static async Task<float?> AverageCore(IAsyncEnumerable<float?> source, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var v = e.Current;
                    if (v.HasValue)
                    {
                        double sum = v.GetValueOrDefault();
                        long count = 1;
                        checked
                        {
                            while (await e.MoveNextAsync().ConfigureAwait(false))
                            {
                                v = e.Current;
                                if (v.HasValue)
                                {
                                    sum += v.GetValueOrDefault();
                                    ++count;
                                }
                            }
                        }

                        return (float)(sum / count);
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return null;
        }

        private static async Task<decimal> AverageCore(IAsyncEnumerable<decimal> source, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw new InvalidOperationException(Strings.NO_ELEMENTS);
                }

                var sum = e.Current;
                long count = 1;
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    sum += e.Current;
                    ++count;
                }

                return sum / count;
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

        private static async Task<decimal?> AverageCore(IAsyncEnumerable<decimal?> source, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var v = e.Current;
                    if (v.HasValue)
                    {
                        var sum = v.GetValueOrDefault();
                        long count = 1;
                        while (await e.MoveNextAsync().ConfigureAwait(false))
                        {
                            v = e.Current;
                            if (v.HasValue)
                            {
                                sum += v.GetValueOrDefault();
                                ++count;
                            }
                        }

                        return sum / count;
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return null;
        }
    }
}
