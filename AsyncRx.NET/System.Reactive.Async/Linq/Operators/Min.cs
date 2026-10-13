// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Min<TSource>(this IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create(source, static (source, observer) => source.SubscribeSafeAsync(AsyncObserver.Min(observer)));
        }

        public static IAsyncObservable<TSource> Min<TSource>(this IAsyncObservable<TSource> source, IComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return CreateAsyncObservable<TSource>.From(
                source,
                comparer,
                static (source, comparer, observer) => source.SubscribeSafeAsync(AsyncObserver.Min(observer, comparer)));
        }
        // As in Rx.NET: the selector forms are the generic forms over a Select.
        public static IAsyncObservable<TResult> Min<TSource, TResult>(this IAsyncObservable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Min();
        }

        public static IAsyncObservable<TResult> Min<TSource, TResult>(this IAsyncObservable<TSource> source, Func<TSource, TResult> selector, IComparer<TResult> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return source.Select(selector).Min(comparer);
        }

        public static IAsyncObservable<TResult> Min<TSource, TResult>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Min();
        }

        public static IAsyncObservable<TResult> Min<TSource, TResult>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask<TResult>> selector, IComparer<TResult> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return source.Select(selector).Min(comparer);
        }
    }

    public partial class AsyncObserver
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
                    if (x == null)
                    {
                        // As in Rx.NET: for a reference type (or a nullable one), null elements
                        // take no part in the comparison.
                        return;
                    }

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
                    // As in Rx.NET since v2: an empty source is an error for a value type, and
                    // yields null for a reference type (or a nullable one).
                    if (!found && default(TSource) != null)
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

                    return default;
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

                    return default;
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

                    return default;
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

                    return default;
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

                    return default;
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

                    return default;
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

                    return default;
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

                    return default;
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

                    return default;
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

                    return default;
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
