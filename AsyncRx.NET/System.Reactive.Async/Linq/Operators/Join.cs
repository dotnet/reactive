// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TResult> Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(this IAsyncObservable<TLeft> left, IAsyncObservable<TRight> right, Func<TLeft, IAsyncObservable<TLeftDuration>> leftDurationSelector, Func<TRight, IAsyncObservable<TRightDuration>> rightDurationSelector, Func<TLeft, TRight, TResult> resultSelector)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));
            if (leftDurationSelector == null)
                throw new ArgumentNullException(nameof(leftDurationSelector));
            if (rightDurationSelector == null)
                throw new ArgumentNullException(nameof(rightDurationSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return CreateAsyncObservable<TResult>.From(
                left,
                (right, leftDurationSelector, rightDurationSelector, resultSelector),
                static async (left, state, observer) =>
                {
                    var subscriptions = new CompositeAsyncDisposable();

                    var (leftObserver, rightObserver, disposable) = AsyncObserver.Join(observer, subscriptions, state.leftDurationSelector, state.rightDurationSelector, state.resultSelector);

                    var leftSubscription = await left.SubscribeSafeAsync(leftObserver).ConfigureAwait(false);
                    await subscriptions.AddAsync(leftSubscription).ConfigureAwait(false);

                    var rightSubscription = await state.right.SubscribeSafeAsync(rightObserver).ConfigureAwait(false);
                    await subscriptions.AddAsync(rightSubscription).ConfigureAwait(false);

                    return disposable;
                });
        }
    }

    public partial class AsyncObserver
    {
        public static (IAsyncObserver<TLeft>, IAsyncObserver<TRight>, IAsyncDisposable) Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(IAsyncObserver<TResult> observer, IAsyncDisposable subscriptions, Func<TLeft, IAsyncObservable<TLeftDuration>> leftDurationSelector, Func<TRight, IAsyncObservable<TRightDuration>> rightDurationSelector, Func<TLeft, TRight, TResult> resultSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscriptions == null)
                throw new ArgumentNullException(nameof(subscriptions));
            if (leftDurationSelector == null)
                throw new ArgumentNullException(nameof(leftDurationSelector));
            if (rightDurationSelector == null)
                throw new ArgumentNullException(nameof(rightDurationSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            var gate = new AsyncGate();

            var group = new CompositeAsyncDisposable(subscriptions);

            var leftMap = new SortedDictionary<int, TLeft>();
            var rightMap = new SortedDictionary<int, TRight>();

            var leftDone = false;
            var rightDone = false;

            var leftId = default(int);
            var rightId = default(int);

            async ValueTask OnErrorAsync(Exception ex)
            {
                using (await gate.LockAsync().ConfigureAwait(false))
                {
                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                }
            }

            var leftObserver =
                Create<TLeft>(
                    async x =>
                    {
                        var theLeftId = default(int);
                        var theRightId = default(int);

                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            theLeftId = leftId++;
                            theRightId = rightId;
                            leftMap.Add(theLeftId, x);
                        }

                        var duration = default(IAsyncObservable<TLeftDuration>);
                        try
                        {
                            duration = leftDurationSelector(x);
                        }
                        catch (Exception ex)
                        {
                            await OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }

                        var sad = new SingleAssignmentAsyncDisposable();

                        await group.AddAsync(sad).ConfigureAwait(false);

                        var durationObserver =
                            Create<TLeftDuration>(
                                d => default,
                                OnErrorAsync,
                                async () =>
                                {
                                    using (await gate.LockAsync().ConfigureAwait(false))
                                    {
                                        if (leftMap.Remove(theLeftId) && leftMap.Count == 0 && leftDone)
                                        {
                                            await observer.OnCompletedAsync().ConfigureAwait(false);
                                        }
                                    }

                                    await group.RemoveAsync(sad).ConfigureAwait(false);
                                }
                            );

                        var durationSubscription = await duration.FirstOrDefault().SubscribeSafeAsync(durationObserver).ConfigureAwait(false);

                        await sad.AssignAsync(durationSubscription).ConfigureAwait(false);

                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            foreach (var rightValue in rightMap)
                            {
                                if (rightValue.Key < theRightId)
                                {
                                    var result = default(TResult);
                                    try
                                    {
                                        result = resultSelector(x, rightValue.Value);
                                    }
                                    catch (Exception ex)
                                    {
                                        await OnErrorAsync(ex).ConfigureAwait(false);
                                        return;
                                    }

                                    await observer.OnNextAsync(result).ConfigureAwait(false);
                                }
                            }
                        }
                    },
                    OnErrorAsync,
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            leftDone = true;

                            if (rightDone || leftMap.Count == 0)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            var rightObserver =
                Create<TRight>(
                    async x =>
                    {
                        var theLeftId = 0;
                        var theRightId = 0;

                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            theRightId = rightId++;
                            theLeftId = leftId;
                            rightMap.Add(theRightId, x);
                        }

                        var duration = default(IAsyncObservable<TRightDuration>);
                        try
                        {
                            duration = rightDurationSelector(x);
                        }
                        catch (Exception ex)
                        {
                            await OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }

                        var sad = new SingleAssignmentAsyncDisposable();

                        await group.AddAsync(sad).ConfigureAwait(false);

                        var durationObserver =
                            Create<TRightDuration>(
                                d => default,
                                OnErrorAsync,
                                async () =>
                                {
                                    using (await gate.LockAsync().ConfigureAwait(false))
                                    {
                                        if (rightMap.Remove(theRightId) && rightMap.Count == 0 && rightDone)
                                        {
                                            await observer.OnCompletedAsync().ConfigureAwait(false);
                                        }
                                    }

                                    await group.RemoveAsync(sad).ConfigureAwait(false);
                                }
                            );

                        var durationSubscription = await duration.FirstOrDefault().SubscribeSafeAsync(durationObserver).ConfigureAwait(false);

                        await sad.AssignAsync(durationSubscription).ConfigureAwait(false);

                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            foreach (var leftValue in leftMap)
                            {
                                if (leftValue.Key < theLeftId)
                                {
                                    var result = default(TResult);
                                    try
                                    {
                                        result = resultSelector(leftValue.Value, x);
                                    }
                                    catch (Exception ex)
                                    {
                                        await OnErrorAsync(ex).ConfigureAwait(false);
                                        return;
                                    }

                                    await observer.OnNextAsync(result).ConfigureAwait(false);
                                }
                            }
                        }
                    },
                    OnErrorAsync,
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            rightDone = true;

                            if (leftDone || rightMap.Count == 0)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            return (leftObserver, rightObserver, group);
        }
    }
}
