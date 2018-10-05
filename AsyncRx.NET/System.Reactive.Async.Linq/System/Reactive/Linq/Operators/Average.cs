// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq
{
    partial class AsyncObserver
    {
        public static IAsyncObserver<int> AverageInt32(IAsyncObserver<double> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var sum = 0L;
            var count = 0L;

            return Create<int>(
                async x =>
                {
                    try
                    {
                        checked
                        {
                            sum += x;
                            count++;
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
                    if (count > 0)
                    {
                        var res = default(double);

                        try
                        {
                            checked
                            {
                                res = (double)sum / count;
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
                    else
                    {
                        await observer.OnErrorAsync(new InvalidOperationException("The sequence is empty.")).ConfigureAwait(false);
                    }
                }
            );
        }

        public static IAsyncObserver<long> AverageInt64(IAsyncObserver<double> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var sum = 0L;
            var count = 0L;

            return Create<long>(
                async x =>
                {
                    try
                    {
                        checked
                        {
                            sum += x;
                            count++;
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
                    if (count > 0)
                    {
                        var res = default(double);

                        try
                        {
                            checked
                            {
                                res = (double)sum / count;
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
                    else
                    {
                        await observer.OnErrorAsync(new InvalidOperationException("The sequence is empty.")).ConfigureAwait(false);
                    }
                }
            );
        }

        public static IAsyncObserver<float> AverageSingle(IAsyncObserver<float> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var sum = 0.0;
            var count = 0L;

            return Create<float>(
                async x =>
                {
                    try
                    {
                        checked
                        {
                            sum += x;
                            count++;
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
                    if (count > 0)
                    {
                        var res = default(float);

                        try
                        {
                            checked
                            {
                                res = (float)(sum / count);
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
                    else
                    {
                        await observer.OnErrorAsync(new InvalidOperationException("The sequence is empty.")).ConfigureAwait(false);
                    }
                }
            );
        }

        public static IAsyncObserver<double> AverageDouble(IAsyncObserver<double> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var sum = 0.0;
            var count = 0L;

            return Create<double>(
                async x =>
                {
                    try
                    {
                        checked
                        {
                            sum += x;
                            count++;
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
                    if (count > 0)
                    {
                        var res = default(double);

                        try
                        {
                            checked
                            {
                                res = sum / count;
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
                    else
                    {
                        await observer.OnErrorAsync(new InvalidOperationException("The sequence is empty.")).ConfigureAwait(false);
                    }
                }
            );
        }

        public static IAsyncObserver<decimal> AverageDecimal(IAsyncObserver<decimal> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var sum = 0m;
            var count = 0L;

            return Create<decimal>(
                async x =>
                {
                    try
                    {
                        checked
                        {
                            sum += x;
                            count++;
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
                    if (count > 0)
                    {
                        var res = default(decimal);

                        try
                        {
                            checked
                            {
                                res = sum / count;
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
                    else
                    {
                        await observer.OnErrorAsync(new InvalidOperationException("The sequence is empty.")).ConfigureAwait(false);
                    }
                }
            );
        }

        public static IAsyncObserver<int?> AverageNullableInt32(IAsyncObserver<double?> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var sum = 0L;
            var count = 0L;

            return Create<int?>(
                async x =>
                {
                    try
                    {
                        if (x.HasValue)
                        {
                            checked
                            {
                                sum += x.GetValueOrDefault();
                                count++;
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
                    if (count > 0)
                    {
                        var res = default(double);

                        try
                        {
                            checked
                            {
                                res = (double)sum / count;
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
                    else
                    {
                        await observer.OnNextAsync(null).ConfigureAwait(false);
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                }
            );
        }

        public static IAsyncObserver<long?> AverageNullableInt64(IAsyncObserver<double?> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var sum = 0L;
            var count = 0L;

            return Create<long?>(
                async x =>
                {
                    try
                    {
                        if (x.HasValue)
                        {
                            checked
                            {
                                sum += x.GetValueOrDefault();
                                count++;
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
                    if (count > 0)
                    {
                        var res = default(double);

                        try
                        {
                            checked
                            {
                                res = (double)sum / count;
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
                    else
                    {
                        await observer.OnNextAsync(null).ConfigureAwait(false);
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                }
            );
        }

        public static IAsyncObserver<float?> AverageNullableSingle(IAsyncObserver<float?> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var sum = 0.0;
            var count = 0L;

            return Create<float?>(
                async x =>
                {
                    try
                    {
                        if (x.HasValue)
                        {
                            checked
                            {
                                sum += x.GetValueOrDefault();
                                count++;
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
                    if (count > 0)
                    {
                        var res = default(float);

                        try
                        {
                            checked
                            {
                                res = (float)(sum / count);
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
                    else
                    {
                        await observer.OnNextAsync(null).ConfigureAwait(false);
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                }
            );
        }

        public static IAsyncObserver<double?> AverageNullableDouble(IAsyncObserver<double?> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var sum = 0.0;
            var count = 0L;

            return Create<double?>(
                async x =>
                {
                    try
                    {
                        if (x.HasValue)
                        {
                            checked
                            {
                                sum += x.GetValueOrDefault();
                                count++;
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
                    if (count > 0)
                    {
                        var res = default(double);

                        try
                        {
                            checked
                            {
                                res = sum / count;
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
                    else
                    {
                        await observer.OnNextAsync(null).ConfigureAwait(false);
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                }
            );
        }

        public static IAsyncObserver<decimal?> AverageNullableDecimal(IAsyncObserver<decimal?> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var sum = 0m;
            var count = 0L;

            return Create<decimal?>(
                async x =>
                {
                    try
                    {
                        if (x.HasValue)
                        {
                            checked
                            {
                                sum += x.GetValueOrDefault();
                                count++;
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
                    if (count > 0)
                    {
                        var res = default(decimal);

                        try
                        {
                            checked
                            {
                                res = sum / count;
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
                    else
                    {
                        await observer.OnNextAsync(null).ConfigureAwait(false);
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                }
            );
        }
    }
}
