// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TResult> GroupJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(this IAsyncObservable<TLeft> left, IAsyncObservable<TRight> right, Func<TLeft, IAsyncObservable<TLeftDuration>> leftDurationSelector, Func<TRight, IAsyncObservable<TRightDuration>> rightDurationSelector, Func<TLeft, IAsyncObservable<TRight>, TResult> resultSelector)
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

            return Create<TResult>(async observer =>
            {
                // Each side owns its own subscription so that it can be released as soon as that
                // side completes (Rx.NET v2+ behaviour), independently of the join as a whole.
                var leftSubscription = new SingleAssignmentAsyncDisposable();
                var rightSubscription = new SingleAssignmentAsyncDisposable();

                var (leftObserver, rightObserver, disposable) = AsyncObserver.GroupJoin(observer, leftSubscription, rightSubscription, leftDurationSelector, rightDurationSelector, resultSelector);

                await leftSubscription.AssignAsync(await left.SubscribeSafeAsync(leftObserver).ConfigureAwait(false)).ConfigureAwait(false);
                await rightSubscription.AssignAsync(await right.SubscribeSafeAsync(rightObserver).ConfigureAwait(false)).ConfigureAwait(false);

                return disposable;
            });
        }
    }

    public partial class AsyncObserver
    {
        // leftSubscription/rightSubscription: the subscriptions to the left and right sources (typically
        // assigned after subscribing). Each is disposed when its side completes, and with the join as a whole.
        public static (IAsyncObserver<TLeft>, IAsyncObserver<TRight>, IAsyncDisposable) GroupJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(IAsyncObserver<TResult> observer, IAsyncDisposable leftSubscription, IAsyncDisposable rightSubscription, Func<TLeft, IAsyncObservable<TLeftDuration>> leftDurationSelector, Func<TRight, IAsyncObservable<TRightDuration>> rightDurationSelector, Func<TLeft, IAsyncObservable<TRight>, TResult> resultSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (leftSubscription == null)
                throw new ArgumentNullException(nameof(leftSubscription));
            if (rightSubscription == null)
                throw new ArgumentNullException(nameof(rightSubscription));
            if (leftDurationSelector == null)
                throw new ArgumentNullException(nameof(leftDurationSelector));
            if (rightDurationSelector == null)
                throw new ArgumentNullException(nameof(rightDurationSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            var gate = new AsyncGate();

            var group = new CompositeAsyncDisposable(leftSubscription, rightSubscription);
            var refCount = new RefCountAsyncDisposable(group);

            var leftMap = new SortedDictionary<int, IAsyncObserver<TRight>>();
            var rightMap = new SortedDictionary<int, TRight>();

            var leftId = default(int);
            var rightId = default(int);

            async ValueTask OnErrorAsync(Exception ex)
            {
                using (await gate.LockAsync().ConfigureAwait(false))
                {
                    foreach (var o in leftMap)
                    {
                        await o.Value.OnErrorAsync(ex).ConfigureAwait(false);
                    }

                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                }
            }

            var leftObserver =
                Create<TLeft>(
                    async x =>
                    {
                        var s = new SequentialSimpleAsyncSubject<TRight>();

                        var theLeftId = default(int);
                        var theRightId = default(int);

                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            theLeftId = leftId++;
                            theRightId = rightId;
                            leftMap.Add(theLeftId, s);
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
                                        if (leftMap.Remove(theLeftId))
                                        {
                                            await s.OnCompletedAsync().ConfigureAwait(false);
                                        }
                                    }

                                    await group.RemoveAsync(sad).ConfigureAwait(false);
                                }
                            );

                        var durationSubscription = await duration.FirstOrDefault().SubscribeSafeAsync(durationObserver).ConfigureAwait(false);

                        await sad.AssignAsync(durationSubscription).ConfigureAwait(false);

                        var window = new WindowAsyncObservable<TRight>(s, refCount);

                        var result = default(TResult);
                        try
                        {
                            result = resultSelector(x, window);
                        }
                        catch (Exception ex)
                        {
                            await OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }

                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnNextAsync(result).ConfigureAwait(false);

                            foreach (var rightValue in rightMap)
                            {
                                if (rightValue.Key < theRightId)
                                {
                                    await s.OnNextAsync(rightValue.Value).ConfigureAwait(false);
                                }
                            }
                        }
                    },
                    OnErrorAsync,
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnCompletedAsync().ConfigureAwait(false);
                        }

                        await leftSubscription.DisposeAsync().ConfigureAwait(false);
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
                                        rightMap.Remove(theRightId);
                                    }

                                    await group.RemoveAsync(sad).ConfigureAwait(false);
                                }
                            );

                        var durationSubscription = await duration.FirstOrDefault().SubscribeSafeAsync(durationObserver).ConfigureAwait(false);

                        await sad.AssignAsync(durationSubscription).ConfigureAwait(false);

                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            foreach (var o in leftMap)
                            {
                                if (o.Key < theLeftId)
                                {
                                    await o.Value.OnNextAsync(x).ConfigureAwait(false);
                                }
                            }
                        }
                    },
                    OnErrorAsync,
                    () => rightSubscription.DisposeAsync()
                );

            return (leftObserver, rightObserver, refCount);
        }
    }
}
