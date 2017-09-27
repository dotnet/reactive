// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Reactive.Joins
{
    internal sealed class AsyncPlan<TSource1, TResult> : AsyncPlan<TResult>
    {
        public AsyncPattern<TSource1> Expression { get; }

        public Func<TSource1, TResult> Selector { get; }

        internal AsyncPlan(AsyncPattern<TSource1> expression, Func<TSource1, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, this.Expression.Source1, onError);

            var activePlan = default(ActiveAsyncPlan<TSource1>);

            activePlan = new ActiveAsyncPlan<TSource1>(
                joinObserver1,
                async (arg1) =>
                {
                    var res = default(TResult);

                    try
                    {
                        res = Selector(arg1);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnNextAsync(res).ConfigureAwait(false);
                },
                async () =>
                {
                    await joinObserver1.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await deactivate(activePlan).ConfigureAwait(false);
                }
            );

            joinObserver1.AddActivePlan(activePlan);

            return activePlan;
        }
    }

    internal sealed class AsyncPlan<TSource1, TSource2, TResult> : AsyncPlan<TResult>
    {
        public AsyncPattern<TSource1, TSource2> Expression { get; }

        public Func<TSource1, TSource2, TResult> Selector { get; }

        internal AsyncPlan(AsyncPattern<TSource1, TSource2> expression, Func<TSource1, TSource2, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, this.Expression.Source1, onError);
            var joinObserver2 = AsyncPlan<TResult>.CreateObserver<TSource2>(externalSubscriptions, this.Expression.Source2, onError);

            var activePlan = default(ActiveAsyncPlan<TSource1, TSource2>);

            activePlan = new ActiveAsyncPlan<TSource1, TSource2>(
                joinObserver1,
                joinObserver2,
                async (arg1, arg2) =>
                {
                    var res = default(TResult);

                    try
                    {
                        res = Selector(arg1, arg2);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnNextAsync(res).ConfigureAwait(false);
                },
                async () =>
                {
                    await joinObserver1.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver2.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await deactivate(activePlan).ConfigureAwait(false);
                }
            );

            joinObserver1.AddActivePlan(activePlan);
            joinObserver2.AddActivePlan(activePlan);

            return activePlan;
        }
    }

    internal sealed class AsyncPlan<TSource1, TSource2, TSource3, TResult> : AsyncPlan<TResult>
    {
        public AsyncPattern<TSource1, TSource2, TSource3> Expression { get; }

        public Func<TSource1, TSource2, TSource3, TResult> Selector { get; }

        internal AsyncPlan(AsyncPattern<TSource1, TSource2, TSource3> expression, Func<TSource1, TSource2, TSource3, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, this.Expression.Source1, onError);
            var joinObserver2 = AsyncPlan<TResult>.CreateObserver<TSource2>(externalSubscriptions, this.Expression.Source2, onError);
            var joinObserver3 = AsyncPlan<TResult>.CreateObserver<TSource3>(externalSubscriptions, this.Expression.Source3, onError);

            var activePlan = default(ActiveAsyncPlan<TSource1, TSource2, TSource3>);

            activePlan = new ActiveAsyncPlan<TSource1, TSource2, TSource3>(
                joinObserver1,
                joinObserver2,
                joinObserver3,
                async (arg1, arg2, arg3) =>
                {
                    var res = default(TResult);

                    try
                    {
                        res = Selector(arg1, arg2, arg3);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnNextAsync(res).ConfigureAwait(false);
                },
                async () =>
                {
                    await joinObserver1.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver2.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver3.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await deactivate(activePlan).ConfigureAwait(false);
                }
            );

            joinObserver1.AddActivePlan(activePlan);
            joinObserver2.AddActivePlan(activePlan);
            joinObserver3.AddActivePlan(activePlan);

            return activePlan;
        }
    }

    internal sealed class AsyncPlan<TSource1, TSource2, TSource3, TSource4, TResult> : AsyncPlan<TResult>
    {
        public AsyncPattern<TSource1, TSource2, TSource3, TSource4> Expression { get; }

        public Func<TSource1, TSource2, TSource3, TSource4, TResult> Selector { get; }

        internal AsyncPlan(AsyncPattern<TSource1, TSource2, TSource3, TSource4> expression, Func<TSource1, TSource2, TSource3, TSource4, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, this.Expression.Source1, onError);
            var joinObserver2 = AsyncPlan<TResult>.CreateObserver<TSource2>(externalSubscriptions, this.Expression.Source2, onError);
            var joinObserver3 = AsyncPlan<TResult>.CreateObserver<TSource3>(externalSubscriptions, this.Expression.Source3, onError);
            var joinObserver4 = AsyncPlan<TResult>.CreateObserver<TSource4>(externalSubscriptions, this.Expression.Source4, onError);

            var activePlan = default(ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4>);

            activePlan = new ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4>(
                joinObserver1,
                joinObserver2,
                joinObserver3,
                joinObserver4,
                async (arg1, arg2, arg3, arg4) =>
                {
                    var res = default(TResult);

                    try
                    {
                        res = Selector(arg1, arg2, arg3, arg4);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnNextAsync(res).ConfigureAwait(false);
                },
                async () =>
                {
                    await joinObserver1.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver2.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver3.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver4.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await deactivate(activePlan).ConfigureAwait(false);
                }
            );

            joinObserver1.AddActivePlan(activePlan);
            joinObserver2.AddActivePlan(activePlan);
            joinObserver3.AddActivePlan(activePlan);
            joinObserver4.AddActivePlan(activePlan);

            return activePlan;
        }
    }

    internal sealed class AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TResult> : AsyncPlan<TResult>
    {
        public AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5> Expression { get; }

        public Func<TSource1, TSource2, TSource3, TSource4, TSource5, TResult> Selector { get; }

        internal AsyncPlan(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, this.Expression.Source1, onError);
            var joinObserver2 = AsyncPlan<TResult>.CreateObserver<TSource2>(externalSubscriptions, this.Expression.Source2, onError);
            var joinObserver3 = AsyncPlan<TResult>.CreateObserver<TSource3>(externalSubscriptions, this.Expression.Source3, onError);
            var joinObserver4 = AsyncPlan<TResult>.CreateObserver<TSource4>(externalSubscriptions, this.Expression.Source4, onError);
            var joinObserver5 = AsyncPlan<TResult>.CreateObserver<TSource5>(externalSubscriptions, this.Expression.Source5, onError);

            var activePlan = default(ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5>);

            activePlan = new ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5>(
                joinObserver1,
                joinObserver2,
                joinObserver3,
                joinObserver4,
                joinObserver5,
                async (arg1, arg2, arg3, arg4, arg5) =>
                {
                    var res = default(TResult);

                    try
                    {
                        res = Selector(arg1, arg2, arg3, arg4, arg5);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnNextAsync(res).ConfigureAwait(false);
                },
                async () =>
                {
                    await joinObserver1.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver2.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver3.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver4.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver5.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await deactivate(activePlan).ConfigureAwait(false);
                }
            );

            joinObserver1.AddActivePlan(activePlan);
            joinObserver2.AddActivePlan(activePlan);
            joinObserver3.AddActivePlan(activePlan);
            joinObserver4.AddActivePlan(activePlan);
            joinObserver5.AddActivePlan(activePlan);

            return activePlan;
        }
    }

    internal sealed class AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult> : AsyncPlan<TResult>
    {
        public AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6> Expression { get; }

        public Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult> Selector { get; }

        internal AsyncPlan(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, this.Expression.Source1, onError);
            var joinObserver2 = AsyncPlan<TResult>.CreateObserver<TSource2>(externalSubscriptions, this.Expression.Source2, onError);
            var joinObserver3 = AsyncPlan<TResult>.CreateObserver<TSource3>(externalSubscriptions, this.Expression.Source3, onError);
            var joinObserver4 = AsyncPlan<TResult>.CreateObserver<TSource4>(externalSubscriptions, this.Expression.Source4, onError);
            var joinObserver5 = AsyncPlan<TResult>.CreateObserver<TSource5>(externalSubscriptions, this.Expression.Source5, onError);
            var joinObserver6 = AsyncPlan<TResult>.CreateObserver<TSource6>(externalSubscriptions, this.Expression.Source6, onError);

            var activePlan = default(ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6>);

            activePlan = new ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6>(
                joinObserver1,
                joinObserver2,
                joinObserver3,
                joinObserver4,
                joinObserver5,
                joinObserver6,
                async (arg1, arg2, arg3, arg4, arg5, arg6) =>
                {
                    var res = default(TResult);

                    try
                    {
                        res = Selector(arg1, arg2, arg3, arg4, arg5, arg6);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnNextAsync(res).ConfigureAwait(false);
                },
                async () =>
                {
                    await joinObserver1.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver2.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver3.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver4.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver5.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver6.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await deactivate(activePlan).ConfigureAwait(false);
                }
            );

            joinObserver1.AddActivePlan(activePlan);
            joinObserver2.AddActivePlan(activePlan);
            joinObserver3.AddActivePlan(activePlan);
            joinObserver4.AddActivePlan(activePlan);
            joinObserver5.AddActivePlan(activePlan);
            joinObserver6.AddActivePlan(activePlan);

            return activePlan;
        }
    }

    internal sealed class AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult> : AsyncPlan<TResult>
    {
        public AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7> Expression { get; }

        public Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult> Selector { get; }

        internal AsyncPlan(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, this.Expression.Source1, onError);
            var joinObserver2 = AsyncPlan<TResult>.CreateObserver<TSource2>(externalSubscriptions, this.Expression.Source2, onError);
            var joinObserver3 = AsyncPlan<TResult>.CreateObserver<TSource3>(externalSubscriptions, this.Expression.Source3, onError);
            var joinObserver4 = AsyncPlan<TResult>.CreateObserver<TSource4>(externalSubscriptions, this.Expression.Source4, onError);
            var joinObserver5 = AsyncPlan<TResult>.CreateObserver<TSource5>(externalSubscriptions, this.Expression.Source5, onError);
            var joinObserver6 = AsyncPlan<TResult>.CreateObserver<TSource6>(externalSubscriptions, this.Expression.Source6, onError);
            var joinObserver7 = AsyncPlan<TResult>.CreateObserver<TSource7>(externalSubscriptions, this.Expression.Source7, onError);

            var activePlan = default(ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7>);

            activePlan = new ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7>(
                joinObserver1,
                joinObserver2,
                joinObserver3,
                joinObserver4,
                joinObserver5,
                joinObserver6,
                joinObserver7,
                async (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
                {
                    var res = default(TResult);

                    try
                    {
                        res = Selector(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnNextAsync(res).ConfigureAwait(false);
                },
                async () =>
                {
                    await joinObserver1.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver2.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver3.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver4.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver5.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver6.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver7.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await deactivate(activePlan).ConfigureAwait(false);
                }
            );

            joinObserver1.AddActivePlan(activePlan);
            joinObserver2.AddActivePlan(activePlan);
            joinObserver3.AddActivePlan(activePlan);
            joinObserver4.AddActivePlan(activePlan);
            joinObserver5.AddActivePlan(activePlan);
            joinObserver6.AddActivePlan(activePlan);
            joinObserver7.AddActivePlan(activePlan);

            return activePlan;
        }
    }

    internal sealed class AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult> : AsyncPlan<TResult>
    {
        public AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8> Expression { get; }

        public Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult> Selector { get; }

        internal AsyncPlan(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, this.Expression.Source1, onError);
            var joinObserver2 = AsyncPlan<TResult>.CreateObserver<TSource2>(externalSubscriptions, this.Expression.Source2, onError);
            var joinObserver3 = AsyncPlan<TResult>.CreateObserver<TSource3>(externalSubscriptions, this.Expression.Source3, onError);
            var joinObserver4 = AsyncPlan<TResult>.CreateObserver<TSource4>(externalSubscriptions, this.Expression.Source4, onError);
            var joinObserver5 = AsyncPlan<TResult>.CreateObserver<TSource5>(externalSubscriptions, this.Expression.Source5, onError);
            var joinObserver6 = AsyncPlan<TResult>.CreateObserver<TSource6>(externalSubscriptions, this.Expression.Source6, onError);
            var joinObserver7 = AsyncPlan<TResult>.CreateObserver<TSource7>(externalSubscriptions, this.Expression.Source7, onError);
            var joinObserver8 = AsyncPlan<TResult>.CreateObserver<TSource8>(externalSubscriptions, this.Expression.Source8, onError);

            var activePlan = default(ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8>);

            activePlan = new ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8>(
                joinObserver1,
                joinObserver2,
                joinObserver3,
                joinObserver4,
                joinObserver5,
                joinObserver6,
                joinObserver7,
                joinObserver8,
                async (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
                {
                    var res = default(TResult);

                    try
                    {
                        res = Selector(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnNextAsync(res).ConfigureAwait(false);
                },
                async () =>
                {
                    await joinObserver1.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver2.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver3.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver4.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver5.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver6.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver7.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver8.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await deactivate(activePlan).ConfigureAwait(false);
                }
            );

            joinObserver1.AddActivePlan(activePlan);
            joinObserver2.AddActivePlan(activePlan);
            joinObserver3.AddActivePlan(activePlan);
            joinObserver4.AddActivePlan(activePlan);
            joinObserver5.AddActivePlan(activePlan);
            joinObserver6.AddActivePlan(activePlan);
            joinObserver7.AddActivePlan(activePlan);
            joinObserver8.AddActivePlan(activePlan);

            return activePlan;
        }
    }

    internal sealed class AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult> : AsyncPlan<TResult>
    {
        public AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9> Expression { get; }

        public Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult> Selector { get; }

        internal AsyncPlan(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, this.Expression.Source1, onError);
            var joinObserver2 = AsyncPlan<TResult>.CreateObserver<TSource2>(externalSubscriptions, this.Expression.Source2, onError);
            var joinObserver3 = AsyncPlan<TResult>.CreateObserver<TSource3>(externalSubscriptions, this.Expression.Source3, onError);
            var joinObserver4 = AsyncPlan<TResult>.CreateObserver<TSource4>(externalSubscriptions, this.Expression.Source4, onError);
            var joinObserver5 = AsyncPlan<TResult>.CreateObserver<TSource5>(externalSubscriptions, this.Expression.Source5, onError);
            var joinObserver6 = AsyncPlan<TResult>.CreateObserver<TSource6>(externalSubscriptions, this.Expression.Source6, onError);
            var joinObserver7 = AsyncPlan<TResult>.CreateObserver<TSource7>(externalSubscriptions, this.Expression.Source7, onError);
            var joinObserver8 = AsyncPlan<TResult>.CreateObserver<TSource8>(externalSubscriptions, this.Expression.Source8, onError);
            var joinObserver9 = AsyncPlan<TResult>.CreateObserver<TSource9>(externalSubscriptions, this.Expression.Source9, onError);

            var activePlan = default(ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9>);

            activePlan = new ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9>(
                joinObserver1,
                joinObserver2,
                joinObserver3,
                joinObserver4,
                joinObserver5,
                joinObserver6,
                joinObserver7,
                joinObserver8,
                joinObserver9,
                async (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
                {
                    var res = default(TResult);

                    try
                    {
                        res = Selector(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnNextAsync(res).ConfigureAwait(false);
                },
                async () =>
                {
                    await joinObserver1.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver2.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver3.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver4.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver5.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver6.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver7.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver8.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver9.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await deactivate(activePlan).ConfigureAwait(false);
                }
            );

            joinObserver1.AddActivePlan(activePlan);
            joinObserver2.AddActivePlan(activePlan);
            joinObserver3.AddActivePlan(activePlan);
            joinObserver4.AddActivePlan(activePlan);
            joinObserver5.AddActivePlan(activePlan);
            joinObserver6.AddActivePlan(activePlan);
            joinObserver7.AddActivePlan(activePlan);
            joinObserver8.AddActivePlan(activePlan);
            joinObserver9.AddActivePlan(activePlan);

            return activePlan;
        }
    }

    internal sealed class AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult> : AsyncPlan<TResult>
    {
        public AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10> Expression { get; }

        public Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult> Selector { get; }

        internal AsyncPlan(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, this.Expression.Source1, onError);
            var joinObserver2 = AsyncPlan<TResult>.CreateObserver<TSource2>(externalSubscriptions, this.Expression.Source2, onError);
            var joinObserver3 = AsyncPlan<TResult>.CreateObserver<TSource3>(externalSubscriptions, this.Expression.Source3, onError);
            var joinObserver4 = AsyncPlan<TResult>.CreateObserver<TSource4>(externalSubscriptions, this.Expression.Source4, onError);
            var joinObserver5 = AsyncPlan<TResult>.CreateObserver<TSource5>(externalSubscriptions, this.Expression.Source5, onError);
            var joinObserver6 = AsyncPlan<TResult>.CreateObserver<TSource6>(externalSubscriptions, this.Expression.Source6, onError);
            var joinObserver7 = AsyncPlan<TResult>.CreateObserver<TSource7>(externalSubscriptions, this.Expression.Source7, onError);
            var joinObserver8 = AsyncPlan<TResult>.CreateObserver<TSource8>(externalSubscriptions, this.Expression.Source8, onError);
            var joinObserver9 = AsyncPlan<TResult>.CreateObserver<TSource9>(externalSubscriptions, this.Expression.Source9, onError);
            var joinObserver10 = AsyncPlan<TResult>.CreateObserver<TSource10>(externalSubscriptions, this.Expression.Source10, onError);

            var activePlan = default(ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10>);

            activePlan = new ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10>(
                joinObserver1,
                joinObserver2,
                joinObserver3,
                joinObserver4,
                joinObserver5,
                joinObserver6,
                joinObserver7,
                joinObserver8,
                joinObserver9,
                joinObserver10,
                async (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
                {
                    var res = default(TResult);

                    try
                    {
                        res = Selector(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnNextAsync(res).ConfigureAwait(false);
                },
                async () =>
                {
                    await joinObserver1.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver2.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver3.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver4.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver5.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver6.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver7.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver8.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver9.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver10.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await deactivate(activePlan).ConfigureAwait(false);
                }
            );

            joinObserver1.AddActivePlan(activePlan);
            joinObserver2.AddActivePlan(activePlan);
            joinObserver3.AddActivePlan(activePlan);
            joinObserver4.AddActivePlan(activePlan);
            joinObserver5.AddActivePlan(activePlan);
            joinObserver6.AddActivePlan(activePlan);
            joinObserver7.AddActivePlan(activePlan);
            joinObserver8.AddActivePlan(activePlan);
            joinObserver9.AddActivePlan(activePlan);
            joinObserver10.AddActivePlan(activePlan);

            return activePlan;
        }
    }

    internal sealed class AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult> : AsyncPlan<TResult>
    {
        public AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11> Expression { get; }

        public Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult> Selector { get; }

        internal AsyncPlan(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, this.Expression.Source1, onError);
            var joinObserver2 = AsyncPlan<TResult>.CreateObserver<TSource2>(externalSubscriptions, this.Expression.Source2, onError);
            var joinObserver3 = AsyncPlan<TResult>.CreateObserver<TSource3>(externalSubscriptions, this.Expression.Source3, onError);
            var joinObserver4 = AsyncPlan<TResult>.CreateObserver<TSource4>(externalSubscriptions, this.Expression.Source4, onError);
            var joinObserver5 = AsyncPlan<TResult>.CreateObserver<TSource5>(externalSubscriptions, this.Expression.Source5, onError);
            var joinObserver6 = AsyncPlan<TResult>.CreateObserver<TSource6>(externalSubscriptions, this.Expression.Source6, onError);
            var joinObserver7 = AsyncPlan<TResult>.CreateObserver<TSource7>(externalSubscriptions, this.Expression.Source7, onError);
            var joinObserver8 = AsyncPlan<TResult>.CreateObserver<TSource8>(externalSubscriptions, this.Expression.Source8, onError);
            var joinObserver9 = AsyncPlan<TResult>.CreateObserver<TSource9>(externalSubscriptions, this.Expression.Source9, onError);
            var joinObserver10 = AsyncPlan<TResult>.CreateObserver<TSource10>(externalSubscriptions, this.Expression.Source10, onError);
            var joinObserver11 = AsyncPlan<TResult>.CreateObserver<TSource11>(externalSubscriptions, this.Expression.Source11, onError);

            var activePlan = default(ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11>);

            activePlan = new ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11>(
                joinObserver1,
                joinObserver2,
                joinObserver3,
                joinObserver4,
                joinObserver5,
                joinObserver6,
                joinObserver7,
                joinObserver8,
                joinObserver9,
                joinObserver10,
                joinObserver11,
                async (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
                {
                    var res = default(TResult);

                    try
                    {
                        res = Selector(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnNextAsync(res).ConfigureAwait(false);
                },
                async () =>
                {
                    await joinObserver1.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver2.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver3.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver4.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver5.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver6.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver7.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver8.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver9.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver10.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver11.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await deactivate(activePlan).ConfigureAwait(false);
                }
            );

            joinObserver1.AddActivePlan(activePlan);
            joinObserver2.AddActivePlan(activePlan);
            joinObserver3.AddActivePlan(activePlan);
            joinObserver4.AddActivePlan(activePlan);
            joinObserver5.AddActivePlan(activePlan);
            joinObserver6.AddActivePlan(activePlan);
            joinObserver7.AddActivePlan(activePlan);
            joinObserver8.AddActivePlan(activePlan);
            joinObserver9.AddActivePlan(activePlan);
            joinObserver10.AddActivePlan(activePlan);
            joinObserver11.AddActivePlan(activePlan);

            return activePlan;
        }
    }

    internal sealed class AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult> : AsyncPlan<TResult>
    {
        public AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12> Expression { get; }

        public Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult> Selector { get; }

        internal AsyncPlan(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, this.Expression.Source1, onError);
            var joinObserver2 = AsyncPlan<TResult>.CreateObserver<TSource2>(externalSubscriptions, this.Expression.Source2, onError);
            var joinObserver3 = AsyncPlan<TResult>.CreateObserver<TSource3>(externalSubscriptions, this.Expression.Source3, onError);
            var joinObserver4 = AsyncPlan<TResult>.CreateObserver<TSource4>(externalSubscriptions, this.Expression.Source4, onError);
            var joinObserver5 = AsyncPlan<TResult>.CreateObserver<TSource5>(externalSubscriptions, this.Expression.Source5, onError);
            var joinObserver6 = AsyncPlan<TResult>.CreateObserver<TSource6>(externalSubscriptions, this.Expression.Source6, onError);
            var joinObserver7 = AsyncPlan<TResult>.CreateObserver<TSource7>(externalSubscriptions, this.Expression.Source7, onError);
            var joinObserver8 = AsyncPlan<TResult>.CreateObserver<TSource8>(externalSubscriptions, this.Expression.Source8, onError);
            var joinObserver9 = AsyncPlan<TResult>.CreateObserver<TSource9>(externalSubscriptions, this.Expression.Source9, onError);
            var joinObserver10 = AsyncPlan<TResult>.CreateObserver<TSource10>(externalSubscriptions, this.Expression.Source10, onError);
            var joinObserver11 = AsyncPlan<TResult>.CreateObserver<TSource11>(externalSubscriptions, this.Expression.Source11, onError);
            var joinObserver12 = AsyncPlan<TResult>.CreateObserver<TSource12>(externalSubscriptions, this.Expression.Source12, onError);

            var activePlan = default(ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12>);

            activePlan = new ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12>(
                joinObserver1,
                joinObserver2,
                joinObserver3,
                joinObserver4,
                joinObserver5,
                joinObserver6,
                joinObserver7,
                joinObserver8,
                joinObserver9,
                joinObserver10,
                joinObserver11,
                joinObserver12,
                async (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
                {
                    var res = default(TResult);

                    try
                    {
                        res = Selector(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnNextAsync(res).ConfigureAwait(false);
                },
                async () =>
                {
                    await joinObserver1.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver2.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver3.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver4.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver5.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver6.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver7.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver8.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver9.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver10.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver11.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver12.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await deactivate(activePlan).ConfigureAwait(false);
                }
            );

            joinObserver1.AddActivePlan(activePlan);
            joinObserver2.AddActivePlan(activePlan);
            joinObserver3.AddActivePlan(activePlan);
            joinObserver4.AddActivePlan(activePlan);
            joinObserver5.AddActivePlan(activePlan);
            joinObserver6.AddActivePlan(activePlan);
            joinObserver7.AddActivePlan(activePlan);
            joinObserver8.AddActivePlan(activePlan);
            joinObserver9.AddActivePlan(activePlan);
            joinObserver10.AddActivePlan(activePlan);
            joinObserver11.AddActivePlan(activePlan);
            joinObserver12.AddActivePlan(activePlan);

            return activePlan;
        }
    }

    internal sealed class AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult> : AsyncPlan<TResult>
    {
        public AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13> Expression { get; }

        public Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult> Selector { get; }

        internal AsyncPlan(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, this.Expression.Source1, onError);
            var joinObserver2 = AsyncPlan<TResult>.CreateObserver<TSource2>(externalSubscriptions, this.Expression.Source2, onError);
            var joinObserver3 = AsyncPlan<TResult>.CreateObserver<TSource3>(externalSubscriptions, this.Expression.Source3, onError);
            var joinObserver4 = AsyncPlan<TResult>.CreateObserver<TSource4>(externalSubscriptions, this.Expression.Source4, onError);
            var joinObserver5 = AsyncPlan<TResult>.CreateObserver<TSource5>(externalSubscriptions, this.Expression.Source5, onError);
            var joinObserver6 = AsyncPlan<TResult>.CreateObserver<TSource6>(externalSubscriptions, this.Expression.Source6, onError);
            var joinObserver7 = AsyncPlan<TResult>.CreateObserver<TSource7>(externalSubscriptions, this.Expression.Source7, onError);
            var joinObserver8 = AsyncPlan<TResult>.CreateObserver<TSource8>(externalSubscriptions, this.Expression.Source8, onError);
            var joinObserver9 = AsyncPlan<TResult>.CreateObserver<TSource9>(externalSubscriptions, this.Expression.Source9, onError);
            var joinObserver10 = AsyncPlan<TResult>.CreateObserver<TSource10>(externalSubscriptions, this.Expression.Source10, onError);
            var joinObserver11 = AsyncPlan<TResult>.CreateObserver<TSource11>(externalSubscriptions, this.Expression.Source11, onError);
            var joinObserver12 = AsyncPlan<TResult>.CreateObserver<TSource12>(externalSubscriptions, this.Expression.Source12, onError);
            var joinObserver13 = AsyncPlan<TResult>.CreateObserver<TSource13>(externalSubscriptions, this.Expression.Source13, onError);

            var activePlan = default(ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13>);

            activePlan = new ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13>(
                joinObserver1,
                joinObserver2,
                joinObserver3,
                joinObserver4,
                joinObserver5,
                joinObserver6,
                joinObserver7,
                joinObserver8,
                joinObserver9,
                joinObserver10,
                joinObserver11,
                joinObserver12,
                joinObserver13,
                async (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
                {
                    var res = default(TResult);

                    try
                    {
                        res = Selector(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnNextAsync(res).ConfigureAwait(false);
                },
                async () =>
                {
                    await joinObserver1.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver2.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver3.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver4.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver5.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver6.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver7.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver8.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver9.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver10.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver11.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver12.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver13.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await deactivate(activePlan).ConfigureAwait(false);
                }
            );

            joinObserver1.AddActivePlan(activePlan);
            joinObserver2.AddActivePlan(activePlan);
            joinObserver3.AddActivePlan(activePlan);
            joinObserver4.AddActivePlan(activePlan);
            joinObserver5.AddActivePlan(activePlan);
            joinObserver6.AddActivePlan(activePlan);
            joinObserver7.AddActivePlan(activePlan);
            joinObserver8.AddActivePlan(activePlan);
            joinObserver9.AddActivePlan(activePlan);
            joinObserver10.AddActivePlan(activePlan);
            joinObserver11.AddActivePlan(activePlan);
            joinObserver12.AddActivePlan(activePlan);
            joinObserver13.AddActivePlan(activePlan);

            return activePlan;
        }
    }

    internal sealed class AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult> : AsyncPlan<TResult>
    {
        public AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14> Expression { get; }

        public Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult> Selector { get; }

        internal AsyncPlan(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, this.Expression.Source1, onError);
            var joinObserver2 = AsyncPlan<TResult>.CreateObserver<TSource2>(externalSubscriptions, this.Expression.Source2, onError);
            var joinObserver3 = AsyncPlan<TResult>.CreateObserver<TSource3>(externalSubscriptions, this.Expression.Source3, onError);
            var joinObserver4 = AsyncPlan<TResult>.CreateObserver<TSource4>(externalSubscriptions, this.Expression.Source4, onError);
            var joinObserver5 = AsyncPlan<TResult>.CreateObserver<TSource5>(externalSubscriptions, this.Expression.Source5, onError);
            var joinObserver6 = AsyncPlan<TResult>.CreateObserver<TSource6>(externalSubscriptions, this.Expression.Source6, onError);
            var joinObserver7 = AsyncPlan<TResult>.CreateObserver<TSource7>(externalSubscriptions, this.Expression.Source7, onError);
            var joinObserver8 = AsyncPlan<TResult>.CreateObserver<TSource8>(externalSubscriptions, this.Expression.Source8, onError);
            var joinObserver9 = AsyncPlan<TResult>.CreateObserver<TSource9>(externalSubscriptions, this.Expression.Source9, onError);
            var joinObserver10 = AsyncPlan<TResult>.CreateObserver<TSource10>(externalSubscriptions, this.Expression.Source10, onError);
            var joinObserver11 = AsyncPlan<TResult>.CreateObserver<TSource11>(externalSubscriptions, this.Expression.Source11, onError);
            var joinObserver12 = AsyncPlan<TResult>.CreateObserver<TSource12>(externalSubscriptions, this.Expression.Source12, onError);
            var joinObserver13 = AsyncPlan<TResult>.CreateObserver<TSource13>(externalSubscriptions, this.Expression.Source13, onError);
            var joinObserver14 = AsyncPlan<TResult>.CreateObserver<TSource14>(externalSubscriptions, this.Expression.Source14, onError);

            var activePlan = default(ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14>);

            activePlan = new ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14>(
                joinObserver1,
                joinObserver2,
                joinObserver3,
                joinObserver4,
                joinObserver5,
                joinObserver6,
                joinObserver7,
                joinObserver8,
                joinObserver9,
                joinObserver10,
                joinObserver11,
                joinObserver12,
                joinObserver13,
                joinObserver14,
                async (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
                {
                    var res = default(TResult);

                    try
                    {
                        res = Selector(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnNextAsync(res).ConfigureAwait(false);
                },
                async () =>
                {
                    await joinObserver1.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver2.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver3.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver4.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver5.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver6.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver7.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver8.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver9.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver10.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver11.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver12.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver13.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver14.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await deactivate(activePlan).ConfigureAwait(false);
                }
            );

            joinObserver1.AddActivePlan(activePlan);
            joinObserver2.AddActivePlan(activePlan);
            joinObserver3.AddActivePlan(activePlan);
            joinObserver4.AddActivePlan(activePlan);
            joinObserver5.AddActivePlan(activePlan);
            joinObserver6.AddActivePlan(activePlan);
            joinObserver7.AddActivePlan(activePlan);
            joinObserver8.AddActivePlan(activePlan);
            joinObserver9.AddActivePlan(activePlan);
            joinObserver10.AddActivePlan(activePlan);
            joinObserver11.AddActivePlan(activePlan);
            joinObserver12.AddActivePlan(activePlan);
            joinObserver13.AddActivePlan(activePlan);
            joinObserver14.AddActivePlan(activePlan);

            return activePlan;
        }
    }

    internal sealed class AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult> : AsyncPlan<TResult>
    {
        public AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15> Expression { get; }

        public Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult> Selector { get; }

        internal AsyncPlan(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, this.Expression.Source1, onError);
            var joinObserver2 = AsyncPlan<TResult>.CreateObserver<TSource2>(externalSubscriptions, this.Expression.Source2, onError);
            var joinObserver3 = AsyncPlan<TResult>.CreateObserver<TSource3>(externalSubscriptions, this.Expression.Source3, onError);
            var joinObserver4 = AsyncPlan<TResult>.CreateObserver<TSource4>(externalSubscriptions, this.Expression.Source4, onError);
            var joinObserver5 = AsyncPlan<TResult>.CreateObserver<TSource5>(externalSubscriptions, this.Expression.Source5, onError);
            var joinObserver6 = AsyncPlan<TResult>.CreateObserver<TSource6>(externalSubscriptions, this.Expression.Source6, onError);
            var joinObserver7 = AsyncPlan<TResult>.CreateObserver<TSource7>(externalSubscriptions, this.Expression.Source7, onError);
            var joinObserver8 = AsyncPlan<TResult>.CreateObserver<TSource8>(externalSubscriptions, this.Expression.Source8, onError);
            var joinObserver9 = AsyncPlan<TResult>.CreateObserver<TSource9>(externalSubscriptions, this.Expression.Source9, onError);
            var joinObserver10 = AsyncPlan<TResult>.CreateObserver<TSource10>(externalSubscriptions, this.Expression.Source10, onError);
            var joinObserver11 = AsyncPlan<TResult>.CreateObserver<TSource11>(externalSubscriptions, this.Expression.Source11, onError);
            var joinObserver12 = AsyncPlan<TResult>.CreateObserver<TSource12>(externalSubscriptions, this.Expression.Source12, onError);
            var joinObserver13 = AsyncPlan<TResult>.CreateObserver<TSource13>(externalSubscriptions, this.Expression.Source13, onError);
            var joinObserver14 = AsyncPlan<TResult>.CreateObserver<TSource14>(externalSubscriptions, this.Expression.Source14, onError);
            var joinObserver15 = AsyncPlan<TResult>.CreateObserver<TSource15>(externalSubscriptions, this.Expression.Source15, onError);

            var activePlan = default(ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15>);

            activePlan = new ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15>(
                joinObserver1,
                joinObserver2,
                joinObserver3,
                joinObserver4,
                joinObserver5,
                joinObserver6,
                joinObserver7,
                joinObserver8,
                joinObserver9,
                joinObserver10,
                joinObserver11,
                joinObserver12,
                joinObserver13,
                joinObserver14,
                joinObserver15,
                async (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
                {
                    var res = default(TResult);

                    try
                    {
                        res = Selector(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnNextAsync(res).ConfigureAwait(false);
                },
                async () =>
                {
                    await joinObserver1.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver2.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver3.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver4.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver5.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver6.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver7.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver8.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver9.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver10.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver11.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver12.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver13.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver14.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver15.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await deactivate(activePlan).ConfigureAwait(false);
                }
            );

            joinObserver1.AddActivePlan(activePlan);
            joinObserver2.AddActivePlan(activePlan);
            joinObserver3.AddActivePlan(activePlan);
            joinObserver4.AddActivePlan(activePlan);
            joinObserver5.AddActivePlan(activePlan);
            joinObserver6.AddActivePlan(activePlan);
            joinObserver7.AddActivePlan(activePlan);
            joinObserver8.AddActivePlan(activePlan);
            joinObserver9.AddActivePlan(activePlan);
            joinObserver10.AddActivePlan(activePlan);
            joinObserver11.AddActivePlan(activePlan);
            joinObserver12.AddActivePlan(activePlan);
            joinObserver13.AddActivePlan(activePlan);
            joinObserver14.AddActivePlan(activePlan);
            joinObserver15.AddActivePlan(activePlan);

            return activePlan;
        }
    }

    internal sealed class AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult> : AsyncPlan<TResult>
    {
        public AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16> Expression { get; }

        public Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult> Selector { get; }

        internal AsyncPlan(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, this.Expression.Source1, onError);
            var joinObserver2 = AsyncPlan<TResult>.CreateObserver<TSource2>(externalSubscriptions, this.Expression.Source2, onError);
            var joinObserver3 = AsyncPlan<TResult>.CreateObserver<TSource3>(externalSubscriptions, this.Expression.Source3, onError);
            var joinObserver4 = AsyncPlan<TResult>.CreateObserver<TSource4>(externalSubscriptions, this.Expression.Source4, onError);
            var joinObserver5 = AsyncPlan<TResult>.CreateObserver<TSource5>(externalSubscriptions, this.Expression.Source5, onError);
            var joinObserver6 = AsyncPlan<TResult>.CreateObserver<TSource6>(externalSubscriptions, this.Expression.Source6, onError);
            var joinObserver7 = AsyncPlan<TResult>.CreateObserver<TSource7>(externalSubscriptions, this.Expression.Source7, onError);
            var joinObserver8 = AsyncPlan<TResult>.CreateObserver<TSource8>(externalSubscriptions, this.Expression.Source8, onError);
            var joinObserver9 = AsyncPlan<TResult>.CreateObserver<TSource9>(externalSubscriptions, this.Expression.Source9, onError);
            var joinObserver10 = AsyncPlan<TResult>.CreateObserver<TSource10>(externalSubscriptions, this.Expression.Source10, onError);
            var joinObserver11 = AsyncPlan<TResult>.CreateObserver<TSource11>(externalSubscriptions, this.Expression.Source11, onError);
            var joinObserver12 = AsyncPlan<TResult>.CreateObserver<TSource12>(externalSubscriptions, this.Expression.Source12, onError);
            var joinObserver13 = AsyncPlan<TResult>.CreateObserver<TSource13>(externalSubscriptions, this.Expression.Source13, onError);
            var joinObserver14 = AsyncPlan<TResult>.CreateObserver<TSource14>(externalSubscriptions, this.Expression.Source14, onError);
            var joinObserver15 = AsyncPlan<TResult>.CreateObserver<TSource15>(externalSubscriptions, this.Expression.Source15, onError);
            var joinObserver16 = AsyncPlan<TResult>.CreateObserver<TSource16>(externalSubscriptions, this.Expression.Source16, onError);

            var activePlan = default(ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16>);

            activePlan = new ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16>(
                joinObserver1,
                joinObserver2,
                joinObserver3,
                joinObserver4,
                joinObserver5,
                joinObserver6,
                joinObserver7,
                joinObserver8,
                joinObserver9,
                joinObserver10,
                joinObserver11,
                joinObserver12,
                joinObserver13,
                joinObserver14,
                joinObserver15,
                joinObserver16,
                async (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
                {
                    var res = default(TResult);

                    try
                    {
                        res = Selector(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnNextAsync(res).ConfigureAwait(false);
                },
                async () =>
                {
                    await joinObserver1.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver2.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver3.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver4.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver5.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver6.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver7.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver8.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver9.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver10.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver11.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver12.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver13.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver14.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver15.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await joinObserver16.RemoveActivePlan(activePlan).ConfigureAwait(false);
                    await deactivate(activePlan).ConfigureAwait(false);
                }
            );

            joinObserver1.AddActivePlan(activePlan);
            joinObserver2.AddActivePlan(activePlan);
            joinObserver3.AddActivePlan(activePlan);
            joinObserver4.AddActivePlan(activePlan);
            joinObserver5.AddActivePlan(activePlan);
            joinObserver6.AddActivePlan(activePlan);
            joinObserver7.AddActivePlan(activePlan);
            joinObserver8.AddActivePlan(activePlan);
            joinObserver9.AddActivePlan(activePlan);
            joinObserver10.AddActivePlan(activePlan);
            joinObserver11.AddActivePlan(activePlan);
            joinObserver12.AddActivePlan(activePlan);
            joinObserver13.AddActivePlan(activePlan);
            joinObserver14.AddActivePlan(activePlan);
            joinObserver15.AddActivePlan(activePlan);
            joinObserver16.AddActivePlan(activePlan);

            return activePlan;
        }
    }

}
