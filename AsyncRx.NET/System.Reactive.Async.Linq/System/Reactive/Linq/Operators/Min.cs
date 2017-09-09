// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Min<TSource>(IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.Min(observer)));
        }

        public static IAsyncObservable<TSource> Min<TSource>(IAsyncObservable<TSource> source, IComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.Min(observer, comparer)));
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> Min<TSource>(IAsyncObserver<TSource> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return Min(observer, Comparer<TSource>.Default);
        }

        public static IAsyncObserver<TSource> Min<TSource>(IAsyncObserver<TSource> observer, IComparer<TSource> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            var min = default(TSource);
            var found = false;

            return Create<TSource>(
                async x =>
                {
                    if (found)
                    {
                        bool isGreater;

                        try
                        {
                            isGreater = comparer.Compare(x, min) < 0;
                        }
                        catch (Exception ex)
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }

                        if (isGreater)
                        {
                            min = x;
                        }
                    }
                    else
                    {
                        min = x;
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
                        await observer.OnNextAsync(min).ConfigureAwait(false);
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                }
            );
        }

        public static IAsyncObserver<int> MinInt32(IAsyncObserver<int> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var min = 0;
            var found = false;

            return Create<int>(
                x =>
                {
                    if (found)
                    {
                        if (x < min)
                        {
                            min = x;
                        }
                    }
                    else
                    {
                        min = x;
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
                        await observer.OnNextAsync(min).ConfigureAwait(false);
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                }
            );
        }

        public static IAsyncObserver<long> MinInt64(IAsyncObserver<long> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var min = 0L;
            var found = false;

            return Create<long>(
                x =>
                {
                    if (found)
                    {
                        if (x < min)
                        {
                            min = x;
                        }
                    }
                    else
                    {
                        min = x;
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
                        await observer.OnNextAsync(min).ConfigureAwait(false);
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                }
            );
        }

        public static IAsyncObserver<float> MinSingle(IAsyncObserver<float> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var min = 0.0f;
            var found = false;

            return Create<float>(
                x =>
                {
                    if (found)
                    {
                        if (x < min || double.IsNaN(x))
                        {
                            min = x;
                        }
                    }
                    else
                    {
                        min = x;
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
                        await observer.OnNextAsync(min).ConfigureAwait(false);
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                }
            );
        }

        public static IAsyncObserver<double> MinDouble(IAsyncObserver<double> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var min = 0.0;
            var found = false;

            return Create<double>(
                x =>
                {
                    if (found)
                    {
                        if (x < min || double.IsNaN(x))
                        {
                            min = x;
                        }
                    }
                    else
                    {
                        min = x;
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
                        await observer.OnNextAsync(min).ConfigureAwait(false);
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                }
            );
        }

        public static IAsyncObserver<decimal> MinDecimal(IAsyncObserver<decimal> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var min = 0m;
            var found = false;

            return Create<decimal>(
                x =>
                {
                    if (found)
                    {
                        if (x < min)
                        {
                            min = x;
                        }
                    }
                    else
                    {
                        min = x;
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
                        await observer.OnNextAsync(min).ConfigureAwait(false);
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                }
            );
        }

        public static IAsyncObserver<int?> MinNullableInt32(IAsyncObserver<int?> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var min = default(int?);

            return Create<int?>(
                x =>
                {
                    if (min == null || x < min)
                    {
                        min = x;
                    }

                    return Task.CompletedTask;
                },
                observer.OnErrorAsync,
                async () =>
                {
                    await observer.OnNextAsync(min).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }

        public static IAsyncObserver<long?> MinNullableInt64(IAsyncObserver<long?> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var min = default(long?);

            return Create<long?>(
                x =>
                {
                    if (min == null || x < min)
                    {
                        min = x;
                    }

                    return Task.CompletedTask;
                },
                observer.OnErrorAsync,
                async () =>
                {
                    await observer.OnNextAsync(min).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }

        public static IAsyncObserver<float?> MinNullableSingle(IAsyncObserver<float?> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var min = default(float?);

            return Create<float?>(
                x =>
                {
                    if (x != null && (min == null || x < min || double.IsNaN(x.Value)))
                    {
                        min = x;
                    }

                    return Task.CompletedTask;
                },
                observer.OnErrorAsync,
                async () =>
                {
                    await observer.OnNextAsync(min).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }

        public static IAsyncObserver<double?> MinNullableDouble(IAsyncObserver<double?> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var min = default(double?);

            return Create<double?>(
                x =>
                {
                    if (x != null && (min == null || x < min || double.IsNaN(x.Value)))
                    {
                        min = x;
                    }

                    return Task.CompletedTask;
                },
                observer.OnErrorAsync,
                async () =>
                {
                    await observer.OnNextAsync(min).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }

        public static IAsyncObserver<decimal?> MinNullableDecimal(IAsyncObserver<decimal?> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var min = default(decimal?);

            return Create<decimal?>(
                x =>
                {
                    if (min == null || x < min)
                    {
                        min = x;
                    }

                    return Task.CompletedTask;
                },
                observer.OnErrorAsync,
                async () =>
                {
                    await observer.OnNextAsync(min).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }
    }
}
