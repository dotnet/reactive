// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Max<TSource>(IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.Max(observer)));
        }

        public static IAsyncObservable<TSource> Max<TSource>(IAsyncObservable<TSource> source, IComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.Max(observer, comparer)));
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> Max<TSource>(IAsyncObserver<TSource> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return Max(observer, Comparer<TSource>.Default);
        }

        public static IAsyncObserver<TSource> Max<TSource>(IAsyncObserver<TSource> observer, IComparer<TSource> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            var max = default(TSource);
            var found = false;

            return Create<TSource>(
                async x =>
                {
                    if (found)
                    {
                        bool isGreater;

                        try
                        {
                            isGreater = comparer.Compare(x, max) > 0;
                        }
                        catch (Exception ex)
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }

                        if (isGreater)
                        {
                            max = x;
                        }
                    }
                    else
                    {
                        max = x;
                        found = true;
                    }
                },
                observer.OnErrorAsync,
                async () =>
                {
                    if (!found)
                    {
                        await observer.OnErrorAsync(new InvalidOperationException("The sequence is empty.")).ConfigureAwait(false);
                    }
                    else
                    {
                        await observer.OnNextAsync(max).ConfigureAwait(false);
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                }
            );
        }

        public static IAsyncObserver<int> MaxInt32(IAsyncObserver<int> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var max = 0;
            var found = false;

            return Create<int>(
                x =>
                {
                    if (found)
                    {
                        if (x > max)
                        {
                            max = x;
                        }
                    }
                    else
                    {
                        max = x;
                        found = true;
                    }

                    return Task.CompletedTask;
                },
                observer.OnErrorAsync,
                async () =>
                {
                    if (!found)
                    {
                        await observer.OnErrorAsync(new InvalidOperationException("The sequence is empty.")).ConfigureAwait(false);
                    }
                    else
                    {
                        await observer.OnNextAsync(max).ConfigureAwait(false);
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                }
            );
        }

        public static IAsyncObserver<long> MaxInt64(IAsyncObserver<long> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var max = 0L;
            var found = false;

            return Create<long>(
                x =>
                {
                    if (found)
                    {
                        if (x > max)
                        {
                            max = x;
                        }
                    }
                    else
                    {
                        max = x;
                        found = true;
                    }

                    return Task.CompletedTask;
                },
                observer.OnErrorAsync,
                async () =>
                {
                    if (!found)
                    {
                        await observer.OnErrorAsync(new InvalidOperationException("The sequence is empty.")).ConfigureAwait(false);
                    }
                    else
                    {
                        await observer.OnNextAsync(max).ConfigureAwait(false);
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                }
            );
        }

        public static IAsyncObserver<float> MaxSingle(IAsyncObserver<float> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var max = 0.0f;
            var found = false;

            return Create<float>(
                x =>
                {
                    if (found)
                    {
                        if (x > max || double.IsNaN(x))
                        {
                            max = x;
                        }
                    }
                    else
                    {
                        max = x;
                        found = true;
                    }

                    return Task.CompletedTask;
                },
                observer.OnErrorAsync,
                async () =>
                {
                    if (!found)
                    {
                        await observer.OnErrorAsync(new InvalidOperationException("The sequence is empty.")).ConfigureAwait(false);
                    }
                    else
                    {
                        await observer.OnNextAsync(max).ConfigureAwait(false);
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                }
            );
        }

        public static IAsyncObserver<double> MaxDouble(IAsyncObserver<double> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var max = 0.0;
            var found = false;

            return Create<double>(
                x =>
                {
                    if (found)
                    {
                        if (x > max || double.IsNaN(x))
                        {
                            max = x;
                        }
                    }
                    else
                    {
                        max = x;
                        found = true;
                    }

                    return Task.CompletedTask;
                },
                observer.OnErrorAsync,
                async () =>
                {
                    if (!found)
                    {
                        await observer.OnErrorAsync(new InvalidOperationException("The sequence is empty.")).ConfigureAwait(false);
                    }
                    else
                    {
                        await observer.OnNextAsync(max).ConfigureAwait(false);
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                }
            );
        }

        public static IAsyncObserver<decimal> MaxDecimal(IAsyncObserver<decimal> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var max = 0m;
            var found = false;

            return Create<decimal>(
                x =>
                {
                    if (found)
                    {
                        if (x > max)
                        {
                            max = x;
                        }
                    }
                    else
                    {
                        max = x;
                        found = true;
                    }

                    return Task.CompletedTask;
                },
                observer.OnErrorAsync,
                async () =>
                {
                    if (!found)
                    {
                        await observer.OnErrorAsync(new InvalidOperationException("The sequence is empty.")).ConfigureAwait(false);
                    }
                    else
                    {
                        await observer.OnNextAsync(max).ConfigureAwait(false);
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                }
            );
        }

        public static IAsyncObserver<int?> MaxNullableInt32(IAsyncObserver<int?> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var max = default(int?);

            return Create<int?>(
                x =>
                {
                    if (max == null || x > max)
                    {
                        max = x;
                    }

                    return Task.CompletedTask;
                },
                observer.OnErrorAsync,
                async () =>
                {
                    await observer.OnNextAsync(max).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }

        public static IAsyncObserver<long?> MaxNullableInt64(IAsyncObserver<long?> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var max = default(long?);

            return Create<long?>(
                x =>
                {
                    if (max == null || x > max)
                    {
                        max = x;
                    }

                    return Task.CompletedTask;
                },
                observer.OnErrorAsync,
                async () =>
                {
                    await observer.OnNextAsync(max).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }

        public static IAsyncObserver<float?> MaxNullableSingle(IAsyncObserver<float?> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var max = default(float?);

            return Create<float?>(
                x =>
                {
                    if (x != null && (max == null || x > max || double.IsNaN(x.Value)))
                    {
                        max = x;
                    }

                    return Task.CompletedTask;
                },
                observer.OnErrorAsync,
                async () =>
                {
                    await observer.OnNextAsync(max).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }

        public static IAsyncObserver<double?> MaxNullableDouble(IAsyncObserver<double?> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var max = default(double?);

            return Create<double?>(
                x =>
                {
                    if (x != null && (max == null || x > max || double.IsNaN(x.Value)))
                    {
                        max = x;
                    }

                    return Task.CompletedTask;
                },
                observer.OnErrorAsync,
                async () =>
                {
                    await observer.OnNextAsync(max).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }

        public static IAsyncObserver<decimal?> MaxNullableDecimal(IAsyncObserver<decimal?> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var max = default(decimal?);

            return Create<decimal?>(
                x =>
                {
                    if (max == null || x > max)
                    {
                        max = x;
                    }

                    return Task.CompletedTask;
                },
                observer.OnErrorAsync,
                async () =>
                {
                    await observer.OnNextAsync(max).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }
    }
}
