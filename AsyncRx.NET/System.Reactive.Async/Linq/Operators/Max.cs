// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Max<TSource>(this IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create(source, static (source, observer) => source.SubscribeSafeAsync(AsyncObserver.Max(observer)));
        }

        public static IAsyncObservable<TSource> Max<TSource>(this IAsyncObservable<TSource> source, IComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return CreateAsyncObservable<TSource>.From(
                source,
                comparer,
                static (source, comparer, observer) => source.SubscribeSafeAsync(AsyncObserver.Max(observer, comparer)));
        }
        // As in Rx.NET: the selector forms are the generic forms over a Select.
        public static IAsyncObservable<TResult> Max<TSource, TResult>(this IAsyncObservable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Max();
        }

        public static IAsyncObservable<TResult> Max<TSource, TResult>(this IAsyncObservable<TSource> source, Func<TSource, TResult> selector, IComparer<TResult> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return source.Select(selector).Max(comparer);
        }

        public static IAsyncObservable<TResult> Max<TSource, TResult>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Max();
        }

        public static IAsyncObservable<TResult> Max<TSource, TResult>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask<TResult>> selector, IComparer<TResult> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return source.Select(selector).Max(comparer);
        }
    }

    public partial class AsyncObserver
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
                    // As in Rx.NET since v2: an empty source is an error for a value type, and
                    // yields null for a reference type (or a nullable one).
                    if (!found && default(TSource) != null)
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

                    return default;
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

                    return default;
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

                    return default;
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

                    return default;
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

                    return default;
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
