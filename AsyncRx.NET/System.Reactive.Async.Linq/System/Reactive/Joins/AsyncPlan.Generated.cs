// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Reactive.Joins
{
    internal sealed class AsyncPlan<TSource1, TResult> : AsyncPlanBase<TSource1, TResult>
    {
        private readonly Func<TSource1, TResult> _selector;

        internal AsyncPlan(AsyncPattern<TSource1> expression, Func<TSource1, TResult> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1) => Task.FromResult(_selector(arg1));
    }

    internal sealed class AsyncPlanWithTask<TSource1, TResult> : AsyncPlanBase<TSource1, TResult>
    {
        private readonly Func<TSource1, Task<TResult>> _selector;

        internal AsyncPlanWithTask(AsyncPattern<TSource1> expression, Func<TSource1, Task<TResult>> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1) => _selector(arg1);
    }

    internal abstract class AsyncPlanBase<TSource1, TResult> : AsyncPlan<TResult>
    {
        private readonly AsyncPattern<TSource1> _expression;

        internal AsyncPlanBase(AsyncPattern<TSource1> expression)
        {
            _expression = expression;
        }

        protected abstract Task<TResult> EvalAsync(TSource1 arg1); // REVIEW: Consider the use of ValueTask<TResult>.

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, _expression.Source1, onError);

            var activePlan = default(ActiveAsyncPlan<TSource1>);

            activePlan = new ActiveAsyncPlan<TSource1>(
                joinObserver1,
                async (arg1) =>
                {
                    var res = default(TResult);

                    try
                    {
                        res = await EvalAsync(arg1).ConfigureAwait(false);
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

    internal sealed class AsyncPlan<TSource1, TSource2, TResult> : AsyncPlanBase<TSource1, TSource2, TResult>
    {
        private readonly Func<TSource1, TSource2, TResult> _selector;

        internal AsyncPlan(AsyncPattern<TSource1, TSource2> expression, Func<TSource1, TSource2, TResult> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2) => Task.FromResult(_selector(arg1, arg2));
    }

    internal sealed class AsyncPlanWithTask<TSource1, TSource2, TResult> : AsyncPlanBase<TSource1, TSource2, TResult>
    {
        private readonly Func<TSource1, TSource2, Task<TResult>> _selector;

        internal AsyncPlanWithTask(AsyncPattern<TSource1, TSource2> expression, Func<TSource1, TSource2, Task<TResult>> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2) => _selector(arg1, arg2);
    }

    internal abstract class AsyncPlanBase<TSource1, TSource2, TResult> : AsyncPlan<TResult>
    {
        private readonly AsyncPattern<TSource1, TSource2> _expression;

        internal AsyncPlanBase(AsyncPattern<TSource1, TSource2> expression)
        {
            _expression = expression;
        }

        protected abstract Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2); // REVIEW: Consider the use of ValueTask<TResult>.

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, _expression.Source1, onError);
            var joinObserver2 = AsyncPlan<TResult>.CreateObserver<TSource2>(externalSubscriptions, _expression.Source2, onError);

            var activePlan = default(ActiveAsyncPlan<TSource1, TSource2>);

            activePlan = new ActiveAsyncPlan<TSource1, TSource2>(
                joinObserver1,
                joinObserver2,
                async (arg1, arg2) =>
                {
                    var res = default(TResult);

                    try
                    {
                        res = await EvalAsync(arg1, arg2).ConfigureAwait(false);
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

    internal sealed class AsyncPlan<TSource1, TSource2, TSource3, TResult> : AsyncPlanBase<TSource1, TSource2, TSource3, TResult>
    {
        private readonly Func<TSource1, TSource2, TSource3, TResult> _selector;

        internal AsyncPlan(AsyncPattern<TSource1, TSource2, TSource3> expression, Func<TSource1, TSource2, TSource3, TResult> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3) => Task.FromResult(_selector(arg1, arg2, arg3));
    }

    internal sealed class AsyncPlanWithTask<TSource1, TSource2, TSource3, TResult> : AsyncPlanBase<TSource1, TSource2, TSource3, TResult>
    {
        private readonly Func<TSource1, TSource2, TSource3, Task<TResult>> _selector;

        internal AsyncPlanWithTask(AsyncPattern<TSource1, TSource2, TSource3> expression, Func<TSource1, TSource2, TSource3, Task<TResult>> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3) => _selector(arg1, arg2, arg3);
    }

    internal abstract class AsyncPlanBase<TSource1, TSource2, TSource3, TResult> : AsyncPlan<TResult>
    {
        private readonly AsyncPattern<TSource1, TSource2, TSource3> _expression;

        internal AsyncPlanBase(AsyncPattern<TSource1, TSource2, TSource3> expression)
        {
            _expression = expression;
        }

        protected abstract Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3); // REVIEW: Consider the use of ValueTask<TResult>.

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, _expression.Source1, onError);
            var joinObserver2 = AsyncPlan<TResult>.CreateObserver<TSource2>(externalSubscriptions, _expression.Source2, onError);
            var joinObserver3 = AsyncPlan<TResult>.CreateObserver<TSource3>(externalSubscriptions, _expression.Source3, onError);

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
                        res = await EvalAsync(arg1, arg2, arg3).ConfigureAwait(false);
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

    internal sealed class AsyncPlan<TSource1, TSource2, TSource3, TSource4, TResult> : AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TResult>
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TResult> _selector;

        internal AsyncPlan(AsyncPattern<TSource1, TSource2, TSource3, TSource4> expression, Func<TSource1, TSource2, TSource3, TSource4, TResult> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4) => Task.FromResult(_selector(arg1, arg2, arg3, arg4));
    }

    internal sealed class AsyncPlanWithTask<TSource1, TSource2, TSource3, TSource4, TResult> : AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TResult>
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, Task<TResult>> _selector;

        internal AsyncPlanWithTask(AsyncPattern<TSource1, TSource2, TSource3, TSource4> expression, Func<TSource1, TSource2, TSource3, TSource4, Task<TResult>> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4) => _selector(arg1, arg2, arg3, arg4);
    }

    internal abstract class AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TResult> : AsyncPlan<TResult>
    {
        private readonly AsyncPattern<TSource1, TSource2, TSource3, TSource4> _expression;

        internal AsyncPlanBase(AsyncPattern<TSource1, TSource2, TSource3, TSource4> expression)
        {
            _expression = expression;
        }

        protected abstract Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4); // REVIEW: Consider the use of ValueTask<TResult>.

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, _expression.Source1, onError);
            var joinObserver2 = AsyncPlan<TResult>.CreateObserver<TSource2>(externalSubscriptions, _expression.Source2, onError);
            var joinObserver3 = AsyncPlan<TResult>.CreateObserver<TSource3>(externalSubscriptions, _expression.Source3, onError);
            var joinObserver4 = AsyncPlan<TResult>.CreateObserver<TSource4>(externalSubscriptions, _expression.Source4, onError);

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
                        res = await EvalAsync(arg1, arg2, arg3, arg4).ConfigureAwait(false);
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

    internal sealed class AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TResult> : AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TResult> _selector;

        internal AsyncPlan(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TResult> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5) => Task.FromResult(_selector(arg1, arg2, arg3, arg4, arg5));
    }

    internal sealed class AsyncPlanWithTask<TSource1, TSource2, TSource3, TSource4, TSource5, TResult> : AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, Task<TResult>> _selector;

        internal AsyncPlanWithTask(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, Task<TResult>> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5) => _selector(arg1, arg2, arg3, arg4, arg5);
    }

    internal abstract class AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TResult> : AsyncPlan<TResult>
    {
        private readonly AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5> _expression;

        internal AsyncPlanBase(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5> expression)
        {
            _expression = expression;
        }

        protected abstract Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5); // REVIEW: Consider the use of ValueTask<TResult>.

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, _expression.Source1, onError);
            var joinObserver2 = AsyncPlan<TResult>.CreateObserver<TSource2>(externalSubscriptions, _expression.Source2, onError);
            var joinObserver3 = AsyncPlan<TResult>.CreateObserver<TSource3>(externalSubscriptions, _expression.Source3, onError);
            var joinObserver4 = AsyncPlan<TResult>.CreateObserver<TSource4>(externalSubscriptions, _expression.Source4, onError);
            var joinObserver5 = AsyncPlan<TResult>.CreateObserver<TSource5>(externalSubscriptions, _expression.Source5, onError);

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
                        res = await EvalAsync(arg1, arg2, arg3, arg4, arg5).ConfigureAwait(false);
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

    internal sealed class AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult> : AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult> _selector;

        internal AsyncPlan(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6) => Task.FromResult(_selector(arg1, arg2, arg3, arg4, arg5, arg6));
    }

    internal sealed class AsyncPlanWithTask<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult> : AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, Task<TResult>> _selector;

        internal AsyncPlanWithTask(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, Task<TResult>> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6) => _selector(arg1, arg2, arg3, arg4, arg5, arg6);
    }

    internal abstract class AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult> : AsyncPlan<TResult>
    {
        private readonly AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6> _expression;

        internal AsyncPlanBase(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6> expression)
        {
            _expression = expression;
        }

        protected abstract Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6); // REVIEW: Consider the use of ValueTask<TResult>.

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, _expression.Source1, onError);
            var joinObserver2 = AsyncPlan<TResult>.CreateObserver<TSource2>(externalSubscriptions, _expression.Source2, onError);
            var joinObserver3 = AsyncPlan<TResult>.CreateObserver<TSource3>(externalSubscriptions, _expression.Source3, onError);
            var joinObserver4 = AsyncPlan<TResult>.CreateObserver<TSource4>(externalSubscriptions, _expression.Source4, onError);
            var joinObserver5 = AsyncPlan<TResult>.CreateObserver<TSource5>(externalSubscriptions, _expression.Source5, onError);
            var joinObserver6 = AsyncPlan<TResult>.CreateObserver<TSource6>(externalSubscriptions, _expression.Source6, onError);

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
                        res = await EvalAsync(arg1, arg2, arg3, arg4, arg5, arg6).ConfigureAwait(false);
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

    internal sealed class AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult> : AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult> _selector;

        internal AsyncPlan(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6, TSource7 arg7) => Task.FromResult(_selector(arg1, arg2, arg3, arg4, arg5, arg6, arg7));
    }

    internal sealed class AsyncPlanWithTask<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult> : AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, Task<TResult>> _selector;

        internal AsyncPlanWithTask(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, Task<TResult>> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6, TSource7 arg7) => _selector(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
    }

    internal abstract class AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult> : AsyncPlan<TResult>
    {
        private readonly AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7> _expression;

        internal AsyncPlanBase(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7> expression)
        {
            _expression = expression;
        }

        protected abstract Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6, TSource7 arg7); // REVIEW: Consider the use of ValueTask<TResult>.

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, _expression.Source1, onError);
            var joinObserver2 = AsyncPlan<TResult>.CreateObserver<TSource2>(externalSubscriptions, _expression.Source2, onError);
            var joinObserver3 = AsyncPlan<TResult>.CreateObserver<TSource3>(externalSubscriptions, _expression.Source3, onError);
            var joinObserver4 = AsyncPlan<TResult>.CreateObserver<TSource4>(externalSubscriptions, _expression.Source4, onError);
            var joinObserver5 = AsyncPlan<TResult>.CreateObserver<TSource5>(externalSubscriptions, _expression.Source5, onError);
            var joinObserver6 = AsyncPlan<TResult>.CreateObserver<TSource6>(externalSubscriptions, _expression.Source6, onError);
            var joinObserver7 = AsyncPlan<TResult>.CreateObserver<TSource7>(externalSubscriptions, _expression.Source7, onError);

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
                        res = await EvalAsync(arg1, arg2, arg3, arg4, arg5, arg6, arg7).ConfigureAwait(false);
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

    internal sealed class AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult> : AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult> _selector;

        internal AsyncPlan(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6, TSource7 arg7, TSource8 arg8) => Task.FromResult(_selector(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));
    }

    internal sealed class AsyncPlanWithTask<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult> : AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, Task<TResult>> _selector;

        internal AsyncPlanWithTask(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, Task<TResult>> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6, TSource7 arg7, TSource8 arg8) => _selector(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
    }

    internal abstract class AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult> : AsyncPlan<TResult>
    {
        private readonly AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8> _expression;

        internal AsyncPlanBase(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8> expression)
        {
            _expression = expression;
        }

        protected abstract Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6, TSource7 arg7, TSource8 arg8); // REVIEW: Consider the use of ValueTask<TResult>.

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, _expression.Source1, onError);
            var joinObserver2 = AsyncPlan<TResult>.CreateObserver<TSource2>(externalSubscriptions, _expression.Source2, onError);
            var joinObserver3 = AsyncPlan<TResult>.CreateObserver<TSource3>(externalSubscriptions, _expression.Source3, onError);
            var joinObserver4 = AsyncPlan<TResult>.CreateObserver<TSource4>(externalSubscriptions, _expression.Source4, onError);
            var joinObserver5 = AsyncPlan<TResult>.CreateObserver<TSource5>(externalSubscriptions, _expression.Source5, onError);
            var joinObserver6 = AsyncPlan<TResult>.CreateObserver<TSource6>(externalSubscriptions, _expression.Source6, onError);
            var joinObserver7 = AsyncPlan<TResult>.CreateObserver<TSource7>(externalSubscriptions, _expression.Source7, onError);
            var joinObserver8 = AsyncPlan<TResult>.CreateObserver<TSource8>(externalSubscriptions, _expression.Source8, onError);

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
                        res = await EvalAsync(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8).ConfigureAwait(false);
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

    internal sealed class AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult> : AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult> _selector;

        internal AsyncPlan(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6, TSource7 arg7, TSource8 arg8, TSource9 arg9) => Task.FromResult(_selector(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));
    }

    internal sealed class AsyncPlanWithTask<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult> : AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, Task<TResult>> _selector;

        internal AsyncPlanWithTask(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, Task<TResult>> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6, TSource7 arg7, TSource8 arg8, TSource9 arg9) => _selector(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
    }

    internal abstract class AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult> : AsyncPlan<TResult>
    {
        private readonly AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9> _expression;

        internal AsyncPlanBase(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9> expression)
        {
            _expression = expression;
        }

        protected abstract Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6, TSource7 arg7, TSource8 arg8, TSource9 arg9); // REVIEW: Consider the use of ValueTask<TResult>.

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, _expression.Source1, onError);
            var joinObserver2 = AsyncPlan<TResult>.CreateObserver<TSource2>(externalSubscriptions, _expression.Source2, onError);
            var joinObserver3 = AsyncPlan<TResult>.CreateObserver<TSource3>(externalSubscriptions, _expression.Source3, onError);
            var joinObserver4 = AsyncPlan<TResult>.CreateObserver<TSource4>(externalSubscriptions, _expression.Source4, onError);
            var joinObserver5 = AsyncPlan<TResult>.CreateObserver<TSource5>(externalSubscriptions, _expression.Source5, onError);
            var joinObserver6 = AsyncPlan<TResult>.CreateObserver<TSource6>(externalSubscriptions, _expression.Source6, onError);
            var joinObserver7 = AsyncPlan<TResult>.CreateObserver<TSource7>(externalSubscriptions, _expression.Source7, onError);
            var joinObserver8 = AsyncPlan<TResult>.CreateObserver<TSource8>(externalSubscriptions, _expression.Source8, onError);
            var joinObserver9 = AsyncPlan<TResult>.CreateObserver<TSource9>(externalSubscriptions, _expression.Source9, onError);

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
                        res = await EvalAsync(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9).ConfigureAwait(false);
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

    internal sealed class AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult> : AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult> _selector;

        internal AsyncPlan(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6, TSource7 arg7, TSource8 arg8, TSource9 arg9, TSource10 arg10) => Task.FromResult(_selector(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10));
    }

    internal sealed class AsyncPlanWithTask<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult> : AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, Task<TResult>> _selector;

        internal AsyncPlanWithTask(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, Task<TResult>> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6, TSource7 arg7, TSource8 arg8, TSource9 arg9, TSource10 arg10) => _selector(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    internal abstract class AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult> : AsyncPlan<TResult>
    {
        private readonly AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10> _expression;

        internal AsyncPlanBase(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10> expression)
        {
            _expression = expression;
        }

        protected abstract Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6, TSource7 arg7, TSource8 arg8, TSource9 arg9, TSource10 arg10); // REVIEW: Consider the use of ValueTask<TResult>.

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, _expression.Source1, onError);
            var joinObserver2 = AsyncPlan<TResult>.CreateObserver<TSource2>(externalSubscriptions, _expression.Source2, onError);
            var joinObserver3 = AsyncPlan<TResult>.CreateObserver<TSource3>(externalSubscriptions, _expression.Source3, onError);
            var joinObserver4 = AsyncPlan<TResult>.CreateObserver<TSource4>(externalSubscriptions, _expression.Source4, onError);
            var joinObserver5 = AsyncPlan<TResult>.CreateObserver<TSource5>(externalSubscriptions, _expression.Source5, onError);
            var joinObserver6 = AsyncPlan<TResult>.CreateObserver<TSource6>(externalSubscriptions, _expression.Source6, onError);
            var joinObserver7 = AsyncPlan<TResult>.CreateObserver<TSource7>(externalSubscriptions, _expression.Source7, onError);
            var joinObserver8 = AsyncPlan<TResult>.CreateObserver<TSource8>(externalSubscriptions, _expression.Source8, onError);
            var joinObserver9 = AsyncPlan<TResult>.CreateObserver<TSource9>(externalSubscriptions, _expression.Source9, onError);
            var joinObserver10 = AsyncPlan<TResult>.CreateObserver<TSource10>(externalSubscriptions, _expression.Source10, onError);

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
                        res = await EvalAsync(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10).ConfigureAwait(false);
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

    internal sealed class AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult> : AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult> _selector;

        internal AsyncPlan(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6, TSource7 arg7, TSource8 arg8, TSource9 arg9, TSource10 arg10, TSource11 arg11) => Task.FromResult(_selector(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11));
    }

    internal sealed class AsyncPlanWithTask<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult> : AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, Task<TResult>> _selector;

        internal AsyncPlanWithTask(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, Task<TResult>> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6, TSource7 arg7, TSource8 arg8, TSource9 arg9, TSource10 arg10, TSource11 arg11) => _selector(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    internal abstract class AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult> : AsyncPlan<TResult>
    {
        private readonly AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11> _expression;

        internal AsyncPlanBase(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11> expression)
        {
            _expression = expression;
        }

        protected abstract Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6, TSource7 arg7, TSource8 arg8, TSource9 arg9, TSource10 arg10, TSource11 arg11); // REVIEW: Consider the use of ValueTask<TResult>.

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, _expression.Source1, onError);
            var joinObserver2 = AsyncPlan<TResult>.CreateObserver<TSource2>(externalSubscriptions, _expression.Source2, onError);
            var joinObserver3 = AsyncPlan<TResult>.CreateObserver<TSource3>(externalSubscriptions, _expression.Source3, onError);
            var joinObserver4 = AsyncPlan<TResult>.CreateObserver<TSource4>(externalSubscriptions, _expression.Source4, onError);
            var joinObserver5 = AsyncPlan<TResult>.CreateObserver<TSource5>(externalSubscriptions, _expression.Source5, onError);
            var joinObserver6 = AsyncPlan<TResult>.CreateObserver<TSource6>(externalSubscriptions, _expression.Source6, onError);
            var joinObserver7 = AsyncPlan<TResult>.CreateObserver<TSource7>(externalSubscriptions, _expression.Source7, onError);
            var joinObserver8 = AsyncPlan<TResult>.CreateObserver<TSource8>(externalSubscriptions, _expression.Source8, onError);
            var joinObserver9 = AsyncPlan<TResult>.CreateObserver<TSource9>(externalSubscriptions, _expression.Source9, onError);
            var joinObserver10 = AsyncPlan<TResult>.CreateObserver<TSource10>(externalSubscriptions, _expression.Source10, onError);
            var joinObserver11 = AsyncPlan<TResult>.CreateObserver<TSource11>(externalSubscriptions, _expression.Source11, onError);

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
                        res = await EvalAsync(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11).ConfigureAwait(false);
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

    internal sealed class AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult> : AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult> _selector;

        internal AsyncPlan(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6, TSource7 arg7, TSource8 arg8, TSource9 arg9, TSource10 arg10, TSource11 arg11, TSource12 arg12) => Task.FromResult(_selector(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12));
    }

    internal sealed class AsyncPlanWithTask<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult> : AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, Task<TResult>> _selector;

        internal AsyncPlanWithTask(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, Task<TResult>> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6, TSource7 arg7, TSource8 arg8, TSource9 arg9, TSource10 arg10, TSource11 arg11, TSource12 arg12) => _selector(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    internal abstract class AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult> : AsyncPlan<TResult>
    {
        private readonly AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12> _expression;

        internal AsyncPlanBase(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12> expression)
        {
            _expression = expression;
        }

        protected abstract Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6, TSource7 arg7, TSource8 arg8, TSource9 arg9, TSource10 arg10, TSource11 arg11, TSource12 arg12); // REVIEW: Consider the use of ValueTask<TResult>.

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, _expression.Source1, onError);
            var joinObserver2 = AsyncPlan<TResult>.CreateObserver<TSource2>(externalSubscriptions, _expression.Source2, onError);
            var joinObserver3 = AsyncPlan<TResult>.CreateObserver<TSource3>(externalSubscriptions, _expression.Source3, onError);
            var joinObserver4 = AsyncPlan<TResult>.CreateObserver<TSource4>(externalSubscriptions, _expression.Source4, onError);
            var joinObserver5 = AsyncPlan<TResult>.CreateObserver<TSource5>(externalSubscriptions, _expression.Source5, onError);
            var joinObserver6 = AsyncPlan<TResult>.CreateObserver<TSource6>(externalSubscriptions, _expression.Source6, onError);
            var joinObserver7 = AsyncPlan<TResult>.CreateObserver<TSource7>(externalSubscriptions, _expression.Source7, onError);
            var joinObserver8 = AsyncPlan<TResult>.CreateObserver<TSource8>(externalSubscriptions, _expression.Source8, onError);
            var joinObserver9 = AsyncPlan<TResult>.CreateObserver<TSource9>(externalSubscriptions, _expression.Source9, onError);
            var joinObserver10 = AsyncPlan<TResult>.CreateObserver<TSource10>(externalSubscriptions, _expression.Source10, onError);
            var joinObserver11 = AsyncPlan<TResult>.CreateObserver<TSource11>(externalSubscriptions, _expression.Source11, onError);
            var joinObserver12 = AsyncPlan<TResult>.CreateObserver<TSource12>(externalSubscriptions, _expression.Source12, onError);

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
                        res = await EvalAsync(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12).ConfigureAwait(false);
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

    internal sealed class AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult> : AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult> _selector;

        internal AsyncPlan(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6, TSource7 arg7, TSource8 arg8, TSource9 arg9, TSource10 arg10, TSource11 arg11, TSource12 arg12, TSource13 arg13) => Task.FromResult(_selector(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13));
    }

    internal sealed class AsyncPlanWithTask<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult> : AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, Task<TResult>> _selector;

        internal AsyncPlanWithTask(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, Task<TResult>> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6, TSource7 arg7, TSource8 arg8, TSource9 arg9, TSource10 arg10, TSource11 arg11, TSource12 arg12, TSource13 arg13) => _selector(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    internal abstract class AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult> : AsyncPlan<TResult>
    {
        private readonly AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13> _expression;

        internal AsyncPlanBase(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13> expression)
        {
            _expression = expression;
        }

        protected abstract Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6, TSource7 arg7, TSource8 arg8, TSource9 arg9, TSource10 arg10, TSource11 arg11, TSource12 arg12, TSource13 arg13); // REVIEW: Consider the use of ValueTask<TResult>.

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, _expression.Source1, onError);
            var joinObserver2 = AsyncPlan<TResult>.CreateObserver<TSource2>(externalSubscriptions, _expression.Source2, onError);
            var joinObserver3 = AsyncPlan<TResult>.CreateObserver<TSource3>(externalSubscriptions, _expression.Source3, onError);
            var joinObserver4 = AsyncPlan<TResult>.CreateObserver<TSource4>(externalSubscriptions, _expression.Source4, onError);
            var joinObserver5 = AsyncPlan<TResult>.CreateObserver<TSource5>(externalSubscriptions, _expression.Source5, onError);
            var joinObserver6 = AsyncPlan<TResult>.CreateObserver<TSource6>(externalSubscriptions, _expression.Source6, onError);
            var joinObserver7 = AsyncPlan<TResult>.CreateObserver<TSource7>(externalSubscriptions, _expression.Source7, onError);
            var joinObserver8 = AsyncPlan<TResult>.CreateObserver<TSource8>(externalSubscriptions, _expression.Source8, onError);
            var joinObserver9 = AsyncPlan<TResult>.CreateObserver<TSource9>(externalSubscriptions, _expression.Source9, onError);
            var joinObserver10 = AsyncPlan<TResult>.CreateObserver<TSource10>(externalSubscriptions, _expression.Source10, onError);
            var joinObserver11 = AsyncPlan<TResult>.CreateObserver<TSource11>(externalSubscriptions, _expression.Source11, onError);
            var joinObserver12 = AsyncPlan<TResult>.CreateObserver<TSource12>(externalSubscriptions, _expression.Source12, onError);
            var joinObserver13 = AsyncPlan<TResult>.CreateObserver<TSource13>(externalSubscriptions, _expression.Source13, onError);

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
                        res = await EvalAsync(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13).ConfigureAwait(false);
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

    internal sealed class AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult> : AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult> _selector;

        internal AsyncPlan(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6, TSource7 arg7, TSource8 arg8, TSource9 arg9, TSource10 arg10, TSource11 arg11, TSource12 arg12, TSource13 arg13, TSource14 arg14) => Task.FromResult(_selector(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14));
    }

    internal sealed class AsyncPlanWithTask<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult> : AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, Task<TResult>> _selector;

        internal AsyncPlanWithTask(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, Task<TResult>> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6, TSource7 arg7, TSource8 arg8, TSource9 arg9, TSource10 arg10, TSource11 arg11, TSource12 arg12, TSource13 arg13, TSource14 arg14) => _selector(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    internal abstract class AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult> : AsyncPlan<TResult>
    {
        private readonly AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14> _expression;

        internal AsyncPlanBase(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14> expression)
        {
            _expression = expression;
        }

        protected abstract Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6, TSource7 arg7, TSource8 arg8, TSource9 arg9, TSource10 arg10, TSource11 arg11, TSource12 arg12, TSource13 arg13, TSource14 arg14); // REVIEW: Consider the use of ValueTask<TResult>.

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, _expression.Source1, onError);
            var joinObserver2 = AsyncPlan<TResult>.CreateObserver<TSource2>(externalSubscriptions, _expression.Source2, onError);
            var joinObserver3 = AsyncPlan<TResult>.CreateObserver<TSource3>(externalSubscriptions, _expression.Source3, onError);
            var joinObserver4 = AsyncPlan<TResult>.CreateObserver<TSource4>(externalSubscriptions, _expression.Source4, onError);
            var joinObserver5 = AsyncPlan<TResult>.CreateObserver<TSource5>(externalSubscriptions, _expression.Source5, onError);
            var joinObserver6 = AsyncPlan<TResult>.CreateObserver<TSource6>(externalSubscriptions, _expression.Source6, onError);
            var joinObserver7 = AsyncPlan<TResult>.CreateObserver<TSource7>(externalSubscriptions, _expression.Source7, onError);
            var joinObserver8 = AsyncPlan<TResult>.CreateObserver<TSource8>(externalSubscriptions, _expression.Source8, onError);
            var joinObserver9 = AsyncPlan<TResult>.CreateObserver<TSource9>(externalSubscriptions, _expression.Source9, onError);
            var joinObserver10 = AsyncPlan<TResult>.CreateObserver<TSource10>(externalSubscriptions, _expression.Source10, onError);
            var joinObserver11 = AsyncPlan<TResult>.CreateObserver<TSource11>(externalSubscriptions, _expression.Source11, onError);
            var joinObserver12 = AsyncPlan<TResult>.CreateObserver<TSource12>(externalSubscriptions, _expression.Source12, onError);
            var joinObserver13 = AsyncPlan<TResult>.CreateObserver<TSource13>(externalSubscriptions, _expression.Source13, onError);
            var joinObserver14 = AsyncPlan<TResult>.CreateObserver<TSource14>(externalSubscriptions, _expression.Source14, onError);

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
                        res = await EvalAsync(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14).ConfigureAwait(false);
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

    internal sealed class AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult> : AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult> _selector;

        internal AsyncPlan(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6, TSource7 arg7, TSource8 arg8, TSource9 arg9, TSource10 arg10, TSource11 arg11, TSource12 arg12, TSource13 arg13, TSource14 arg14, TSource15 arg15) => Task.FromResult(_selector(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15));
    }

    internal sealed class AsyncPlanWithTask<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult> : AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, Task<TResult>> _selector;

        internal AsyncPlanWithTask(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, Task<TResult>> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6, TSource7 arg7, TSource8 arg8, TSource9 arg9, TSource10 arg10, TSource11 arg11, TSource12 arg12, TSource13 arg13, TSource14 arg14, TSource15 arg15) => _selector(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    internal abstract class AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult> : AsyncPlan<TResult>
    {
        private readonly AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15> _expression;

        internal AsyncPlanBase(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15> expression)
        {
            _expression = expression;
        }

        protected abstract Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6, TSource7 arg7, TSource8 arg8, TSource9 arg9, TSource10 arg10, TSource11 arg11, TSource12 arg12, TSource13 arg13, TSource14 arg14, TSource15 arg15); // REVIEW: Consider the use of ValueTask<TResult>.

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, _expression.Source1, onError);
            var joinObserver2 = AsyncPlan<TResult>.CreateObserver<TSource2>(externalSubscriptions, _expression.Source2, onError);
            var joinObserver3 = AsyncPlan<TResult>.CreateObserver<TSource3>(externalSubscriptions, _expression.Source3, onError);
            var joinObserver4 = AsyncPlan<TResult>.CreateObserver<TSource4>(externalSubscriptions, _expression.Source4, onError);
            var joinObserver5 = AsyncPlan<TResult>.CreateObserver<TSource5>(externalSubscriptions, _expression.Source5, onError);
            var joinObserver6 = AsyncPlan<TResult>.CreateObserver<TSource6>(externalSubscriptions, _expression.Source6, onError);
            var joinObserver7 = AsyncPlan<TResult>.CreateObserver<TSource7>(externalSubscriptions, _expression.Source7, onError);
            var joinObserver8 = AsyncPlan<TResult>.CreateObserver<TSource8>(externalSubscriptions, _expression.Source8, onError);
            var joinObserver9 = AsyncPlan<TResult>.CreateObserver<TSource9>(externalSubscriptions, _expression.Source9, onError);
            var joinObserver10 = AsyncPlan<TResult>.CreateObserver<TSource10>(externalSubscriptions, _expression.Source10, onError);
            var joinObserver11 = AsyncPlan<TResult>.CreateObserver<TSource11>(externalSubscriptions, _expression.Source11, onError);
            var joinObserver12 = AsyncPlan<TResult>.CreateObserver<TSource12>(externalSubscriptions, _expression.Source12, onError);
            var joinObserver13 = AsyncPlan<TResult>.CreateObserver<TSource13>(externalSubscriptions, _expression.Source13, onError);
            var joinObserver14 = AsyncPlan<TResult>.CreateObserver<TSource14>(externalSubscriptions, _expression.Source14, onError);
            var joinObserver15 = AsyncPlan<TResult>.CreateObserver<TSource15>(externalSubscriptions, _expression.Source15, onError);

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
                        res = await EvalAsync(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15).ConfigureAwait(false);
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

    internal sealed class AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult> : AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult> _selector;

        internal AsyncPlan(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6, TSource7 arg7, TSource8 arg8, TSource9 arg9, TSource10 arg10, TSource11 arg11, TSource12 arg12, TSource13 arg13, TSource14 arg14, TSource15 arg15, TSource16 arg16) => Task.FromResult(_selector(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16));
    }

    internal sealed class AsyncPlanWithTask<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult> : AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, Task<TResult>> _selector;

        internal AsyncPlanWithTask(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16> expression, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, Task<TResult>> selector)
            : base(expression)
        {
            _selector = selector;
        }

        protected override Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6, TSource7 arg7, TSource8 arg8, TSource9 arg9, TSource10 arg10, TSource11 arg11, TSource12 arg12, TSource13 arg13, TSource14 arg14, TSource15 arg15, TSource16 arg16) => _selector(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    internal abstract class AsyncPlanBase<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult> : AsyncPlan<TResult>
    {
        private readonly AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16> _expression;

        internal AsyncPlanBase(AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16> expression)
        {
            _expression = expression;
        }

        protected abstract Task<TResult> EvalAsync(TSource1 arg1, TSource2 arg2, TSource3 arg3, TSource4 arg4, TSource5 arg5, TSource6 arg6, TSource7 arg7, TSource8 arg8, TSource9 arg9, TSource10 arg10, TSource11 arg11, TSource12 arg12, TSource13 arg13, TSource14 arg14, TSource15 arg15, TSource16 arg16); // REVIEW: Consider the use of ValueTask<TResult>.

        internal override ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, Task> deactivate)
        {
            var onError = new Func<Exception, Task>(observer.OnErrorAsync);

            var joinObserver1 = AsyncPlan<TResult>.CreateObserver<TSource1>(externalSubscriptions, _expression.Source1, onError);
            var joinObserver2 = AsyncPlan<TResult>.CreateObserver<TSource2>(externalSubscriptions, _expression.Source2, onError);
            var joinObserver3 = AsyncPlan<TResult>.CreateObserver<TSource3>(externalSubscriptions, _expression.Source3, onError);
            var joinObserver4 = AsyncPlan<TResult>.CreateObserver<TSource4>(externalSubscriptions, _expression.Source4, onError);
            var joinObserver5 = AsyncPlan<TResult>.CreateObserver<TSource5>(externalSubscriptions, _expression.Source5, onError);
            var joinObserver6 = AsyncPlan<TResult>.CreateObserver<TSource6>(externalSubscriptions, _expression.Source6, onError);
            var joinObserver7 = AsyncPlan<TResult>.CreateObserver<TSource7>(externalSubscriptions, _expression.Source7, onError);
            var joinObserver8 = AsyncPlan<TResult>.CreateObserver<TSource8>(externalSubscriptions, _expression.Source8, onError);
            var joinObserver9 = AsyncPlan<TResult>.CreateObserver<TSource9>(externalSubscriptions, _expression.Source9, onError);
            var joinObserver10 = AsyncPlan<TResult>.CreateObserver<TSource10>(externalSubscriptions, _expression.Source10, onError);
            var joinObserver11 = AsyncPlan<TResult>.CreateObserver<TSource11>(externalSubscriptions, _expression.Source11, onError);
            var joinObserver12 = AsyncPlan<TResult>.CreateObserver<TSource12>(externalSubscriptions, _expression.Source12, onError);
            var joinObserver13 = AsyncPlan<TResult>.CreateObserver<TSource13>(externalSubscriptions, _expression.Source13, onError);
            var joinObserver14 = AsyncPlan<TResult>.CreateObserver<TSource14>(externalSubscriptions, _expression.Source14, onError);
            var joinObserver15 = AsyncPlan<TResult>.CreateObserver<TSource15>(externalSubscriptions, _expression.Source15, onError);
            var joinObserver16 = AsyncPlan<TResult>.CreateObserver<TSource16>(externalSubscriptions, _expression.Source16, onError);

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
                        res = await EvalAsync(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16).ConfigureAwait(false);
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
