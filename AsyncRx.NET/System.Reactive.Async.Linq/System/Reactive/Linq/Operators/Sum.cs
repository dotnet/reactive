// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObserver
    {
        public static IAsyncObserver<int> SumInt32(IAsyncObserver<int> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var sum = 0;

            return Create<int>(
                async x =>
                {
                    try
                    {
                        checked
                        {
                            sum += x;
                        }
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    }
                },
                observer.OnErrorAsync,
                async () =>
                {
                    await observer.OnNextAsync(sum).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }

        public static IAsyncObserver<long> SumInt64(IAsyncObserver<long> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var sum = 0L;

            return Create<long>(
                async x =>
                {
                    try
                    {
                        checked
                        {
                            sum += x;
                        }
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    }
                },
                observer.OnErrorAsync,
                async () =>
                {
                    await observer.OnNextAsync(sum).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }

        public static IAsyncObserver<float> SumSingle(IAsyncObserver<float> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var sum = 0.0;

            return Create<float>(
                x =>
                {
                    sum += x;

                    return Task.CompletedTask;
                },
                observer.OnErrorAsync,
                async () =>
                {
                    var res = default(float);

                    try
                    {
                        checked
                        {
                            res = (float)sum;
                        }
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnNextAsync(res).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }

        public static IAsyncObserver<double> SumDouble(IAsyncObserver<double> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var sum = 0.0;

            return Create<double>(
                x =>
                {
                    sum += x;

                    return Task.CompletedTask;
                },
                observer.OnErrorAsync,
                async () =>
                {
                    await observer.OnNextAsync(sum).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }

        public static IAsyncObserver<decimal> SumDecimal(IAsyncObserver<decimal> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var sum = 0m;

            return Create<decimal>(
                x =>
                {
                    sum += x;

                    return Task.CompletedTask;
                },
                observer.OnErrorAsync,
                async () =>
                {
                    await observer.OnNextAsync(sum).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }

        public static IAsyncObserver<int?> SumNullableInt32(IAsyncObserver<int?> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var sum = 0;

            return Create<int?>(
                async x =>
                {
                    try
                    {
                        checked
                        {
                            if (x != null)
                            {
                                sum += x.GetValueOrDefault();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    }
                },
                observer.OnErrorAsync,
                async () =>
                {
                    await observer.OnNextAsync(sum).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }

        public static IAsyncObserver<long?> SumNullableInt64(IAsyncObserver<long?> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var sum = (long?)0L;

            return Create<long?>(
                async x =>
                {
                    try
                    {
                        checked
                        {
                            if (x != null)
                            {
                                sum += x.GetValueOrDefault();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    }
                },
                observer.OnErrorAsync,
                async () =>
                {
                    await observer.OnNextAsync(sum).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }

        public static IAsyncObserver<float?> SumNullableSingle(IAsyncObserver<float?> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var sum = 0.0;

            return Create<float?>(
                x =>
                {
                    if (x != null)
                    {
                        sum += x.GetValueOrDefault();
                    }

                    return Task.CompletedTask;
                },
                observer.OnErrorAsync,
                async () =>
                {
                    var res = default(float);

                    try
                    {
                        checked
                        {
                            res = (float)sum;
                        }
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnNextAsync(res).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }

        public static IAsyncObserver<double?> SumNullableDouble(IAsyncObserver<double?> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var sum = 0.0;

            return Create<double?>(
                x =>
                {
                    if (x != null)
                    {
                        sum += x.GetValueOrDefault();
                    }

                    return Task.CompletedTask;
                },
                observer.OnErrorAsync,
                async () =>
                {
                    await observer.OnNextAsync(sum).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }

        public static IAsyncObserver<decimal?> SumNullableDecimal(IAsyncObserver<decimal?> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var sum = 0m;

            return Create<decimal?>(
                x =>
                {
                    if (x != null)
                    {
                        sum += x.GetValueOrDefault();
                    }

                    return Task.CompletedTask;
                },
                observer.OnErrorAsync,
                async () =>
                {
                    await observer.OnNextAsync(sum).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }
    }
}
