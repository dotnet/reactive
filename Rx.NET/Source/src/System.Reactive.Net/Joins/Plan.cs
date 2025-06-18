﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Reactive.Joins
{
    /// <summary>
    /// Represents an execution plan for join patterns.
    /// </summary>
    /// <typeparam name="TResult">The type of the results produced by the plan.</typeparam>
    public abstract class Plan<TResult>
    {
        internal Plan()
        {
        }

        internal abstract ActivePlan Activate(Dictionary<object, IJoinObserver> externalSubscriptions,
                                              IObserver<TResult> observer, Action<ActivePlan> deactivate);

        internal static JoinObserver<TSource> CreateObserver<TSource>(
            Dictionary<object, IJoinObserver> externalSubscriptions, IObservable<TSource> observable, Action<Exception> onError)
        {
            JoinObserver<TSource> observer;

            if (!externalSubscriptions.TryGetValue(observable, out var nonGeneric))
            {
                observer = new JoinObserver<TSource>(observable, onError);
                externalSubscriptions.Add(observable, observer);
            }
            else
            {
                observer = (JoinObserver<TSource>)nonGeneric;
            }
            
            return observer;
        }
    }

    internal sealed class Plan<T1, TResult> : Plan<TResult>
    {
        internal Pattern<T1> Expression { get; }

        internal Func<T1, TResult> Selector { get; }

        internal Plan(Pattern<T1> expression, Func<T1, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActivePlan Activate(Dictionary<object, IJoinObserver> externalSubscriptions,
                                              IObserver<TResult> observer, Action<ActivePlan> deactivate)
        {
            var onError = new Action<Exception>(observer.OnError);
            var firstJoinObserver = CreateObserver(externalSubscriptions, Expression.First, onError);
            var activePlan = default(ActivePlan<T1>)!;

            activePlan = new ActivePlan<T1>(firstJoinObserver,
                first =>
                {
                    TResult result;
                    try
                    {
                        result = Selector(first);
                    }
                    catch (Exception exception)
                    {
                        observer.OnError(exception);
                        return;
                    }
                    observer.OnNext(result);
                },
                () =>
                {
                    firstJoinObserver.RemoveActivePlan(activePlan);
                    deactivate(activePlan);
                });

            firstJoinObserver.AddActivePlan(activePlan);
            return activePlan;
        }
    }

    internal sealed class Plan<T1, T2, TResult> : Plan<TResult>
    {
        internal Pattern<T1, T2> Expression { get; }

        internal Func<T1, T2, TResult> Selector { get; }

        internal Plan(Pattern<T1, T2> expression, Func<T1, T2, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActivePlan Activate(Dictionary<object, IJoinObserver> externalSubscriptions,
                                              IObserver<TResult> observer, Action<ActivePlan> deactivate)
        {
            var onError = new Action<Exception>(observer.OnError);
            var firstJoinObserver = CreateObserver(externalSubscriptions, Expression.First, onError);
            var secondJoinObserver = CreateObserver(externalSubscriptions, Expression.Second, onError);
            var activePlan = default(ActivePlan<T1, T2>)!;

            activePlan = new ActivePlan<T1, T2>(firstJoinObserver, secondJoinObserver,
                (first, second) =>
                {
                    TResult result;
                    try
                    {
                        result = Selector(first, second);
                    }
                    catch (Exception exception)
                    {
                        observer.OnError(exception);
                        return;
                    }
                    observer.OnNext(result);
                },
                () =>
                {
                    firstJoinObserver.RemoveActivePlan(activePlan);
                    secondJoinObserver.RemoveActivePlan(activePlan);
                    deactivate(activePlan);
                });

            firstJoinObserver.AddActivePlan(activePlan);
            secondJoinObserver.AddActivePlan(activePlan);
            return activePlan;
        }
    }

    internal sealed class Plan<T1, T2, T3, TResult> : Plan<TResult>
    {
        internal Pattern<T1, T2, T3> Expression { get; }

        internal Func<T1, T2, T3, TResult> Selector { get; }

        internal Plan(Pattern<T1, T2, T3> expression, Func<T1, T2, T3, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }


        internal override ActivePlan Activate(Dictionary<object, IJoinObserver> externalSubscriptions,
                                              IObserver<TResult> observer, Action<ActivePlan> deactivate)
        {
            var onError = new Action<Exception>(observer.OnError);
            var firstJoinObserver = CreateObserver(externalSubscriptions, Expression.First, onError);
            var secondJoinObserver = CreateObserver(externalSubscriptions, Expression.Second, onError);
            var thirdJoinObserver = CreateObserver(externalSubscriptions, Expression.Third, onError);
            var activePlan = default(ActivePlan<T1, T2, T3>)!;

            activePlan = new ActivePlan<T1, T2, T3>(firstJoinObserver, secondJoinObserver, thirdJoinObserver,
                (first, second, third) =>
                {
                    TResult result;
                    try
                    {
                        result = Selector(first, second, third);
                    }
                    catch (Exception exception)
                    {
                        observer.OnError(exception);
                        return;
                    }
                    observer.OnNext(result);
                },
                    () =>
                    {
                        firstJoinObserver.RemoveActivePlan(activePlan);
                        secondJoinObserver.RemoveActivePlan(activePlan);
                        thirdJoinObserver.RemoveActivePlan(activePlan);
                        deactivate(activePlan);
                    });

            firstJoinObserver.AddActivePlan(activePlan);
            secondJoinObserver.AddActivePlan(activePlan);
            thirdJoinObserver.AddActivePlan(activePlan);
            return activePlan;
        }
    }

    internal sealed class Plan<T1, T2, T3, T4, TResult> : Plan<TResult>
    {
        internal Pattern<T1, T2, T3, T4> Expression { get; }

        internal Func<T1, T2, T3, T4, TResult> Selector { get; }

        internal Plan(Pattern<T1, T2, T3, T4> expression,
                      Func<T1, T2, T3, T4, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActivePlan Activate(Dictionary<object, IJoinObserver> externalSubscriptions,
                                              IObserver<TResult> observer, Action<ActivePlan> deactivate)
        {
            var onError = new Action<Exception>(observer.OnError);
            var firstJoinObserver = CreateObserver(externalSubscriptions, Expression.First, onError);
            var secondJoinObserver = CreateObserver(externalSubscriptions, Expression.Second, onError);
            var thirdJoinObserver = CreateObserver(externalSubscriptions, Expression.Third, onError);
            var fourthJoinObserver = CreateObserver(externalSubscriptions, Expression.Fourth, onError);
            var activePlan = default(ActivePlan<T1, T2, T3, T4>)!;

            activePlan = new ActivePlan<T1, T2, T3, T4>(firstJoinObserver, secondJoinObserver, thirdJoinObserver, fourthJoinObserver,
                (first, second, third, fourth) =>
                {
                    TResult result;
                    try
                    {
                        result = Selector(first, second, third, fourth);
                    }
                    catch (Exception exception)
                    {
                        observer.OnError(exception);
                        return;
                    }
                    observer.OnNext(result);
                },
                () =>
                {
                    firstJoinObserver.RemoveActivePlan(activePlan);
                    secondJoinObserver.RemoveActivePlan(activePlan);
                    thirdJoinObserver.RemoveActivePlan(activePlan);
                    fourthJoinObserver.RemoveActivePlan(activePlan);
                    deactivate(activePlan);
                });

            firstJoinObserver.AddActivePlan(activePlan);
            secondJoinObserver.AddActivePlan(activePlan);
            thirdJoinObserver.AddActivePlan(activePlan);
            fourthJoinObserver.AddActivePlan(activePlan);
            return activePlan;
        }
    }

    internal sealed class Plan<T1, T2, T3, T4, T5, TResult> : Plan<TResult>
    {
        internal Pattern<T1, T2, T3, T4, T5> Expression { get; }

        internal Func<T1, T2, T3, T4, T5, TResult> Selector { get; }

        internal Plan(Pattern<T1, T2, T3, T4, T5> expression,
                      Func<T1, T2, T3, T4, T5, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActivePlan Activate(Dictionary<object, IJoinObserver> externalSubscriptions,
                                              IObserver<TResult> observer, Action<ActivePlan> deactivate)
        {
            var onError = new Action<Exception>(observer.OnError);
            var firstJoinObserver = CreateObserver(externalSubscriptions, Expression.First, onError);
            var secondJoinObserver = CreateObserver(externalSubscriptions, Expression.Second, onError);
            var thirdJoinObserver = CreateObserver(externalSubscriptions, Expression.Third, onError);
            var fourthJoinObserver = CreateObserver(externalSubscriptions, Expression.Fourth, onError);
            var fifthJoinObserver = CreateObserver(externalSubscriptions, Expression.Fifth, onError);
            var activePlan = default(ActivePlan<T1, T2, T3, T4, T5>)!;

            activePlan = new ActivePlan<T1, T2, T3, T4, T5>(firstJoinObserver, secondJoinObserver, thirdJoinObserver, fourthJoinObserver, fifthJoinObserver,
                (first, second, third, fourth, fifth) =>
                {
                    TResult result;
                    try
                    {
                        result = Selector(first, second, third, fourth, fifth);
                    }
                    catch (Exception exception)
                    {
                        observer.OnError(exception);
                        return;
                    }
                    observer.OnNext(result);
                },
                () =>
                {
                    firstJoinObserver.RemoveActivePlan(activePlan);
                    secondJoinObserver.RemoveActivePlan(activePlan);
                    thirdJoinObserver.RemoveActivePlan(activePlan);
                    fourthJoinObserver.RemoveActivePlan(activePlan);
                    fifthJoinObserver.RemoveActivePlan(activePlan);
                    deactivate(activePlan);
                });

            firstJoinObserver.AddActivePlan(activePlan);
            secondJoinObserver.AddActivePlan(activePlan);
            thirdJoinObserver.AddActivePlan(activePlan);
            fourthJoinObserver.AddActivePlan(activePlan);
            fifthJoinObserver.AddActivePlan(activePlan);
            return activePlan;
        }
    }

    internal sealed class Plan<T1, T2, T3, T4, T5, T6, TResult> : Plan<TResult>
    {
        internal Pattern<T1, T2, T3, T4, T5, T6> Expression { get; }

        internal Func<T1, T2, T3, T4, T5, T6, TResult> Selector { get; }

        internal Plan(Pattern<T1, T2, T3, T4, T5, T6> expression,
                      Func<T1, T2, T3, T4, T5, T6, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActivePlan Activate(Dictionary<object, IJoinObserver> externalSubscriptions,
                                              IObserver<TResult> observer, Action<ActivePlan> deactivate)
        {
            var onError = new Action<Exception>(observer.OnError);
            var firstJoinObserver = CreateObserver(externalSubscriptions, Expression.First, onError);
            var secondJoinObserver = CreateObserver(externalSubscriptions, Expression.Second, onError);
            var thirdJoinObserver = CreateObserver(externalSubscriptions, Expression.Third, onError);
            var fourthJoinObserver = CreateObserver(externalSubscriptions, Expression.Fourth, onError);
            var fifthJoinObserver = CreateObserver(externalSubscriptions, Expression.Fifth, onError);
            var sixthJoinObserver = CreateObserver(externalSubscriptions, Expression.Sixth, onError);
            var activePlan = default(ActivePlan<T1, T2, T3, T4, T5, T6>)!;

            activePlan = new ActivePlan<T1, T2, T3, T4, T5, T6>(firstJoinObserver, secondJoinObserver, thirdJoinObserver,
                fourthJoinObserver, fifthJoinObserver, sixthJoinObserver,
                (first, second, third, fourth, fifth, sixth) =>
                {
                    TResult result;
                    try
                    {
                        result = Selector(first, second, third, fourth, fifth, sixth);
                    }
                    catch (Exception exception)
                    {
                        observer.OnError(exception);
                        return;
                    }
                    observer.OnNext(result);
                },
                () =>
                {
                    firstJoinObserver.RemoveActivePlan(activePlan);
                    secondJoinObserver.RemoveActivePlan(activePlan);
                    thirdJoinObserver.RemoveActivePlan(activePlan);
                    fourthJoinObserver.RemoveActivePlan(activePlan);
                    fifthJoinObserver.RemoveActivePlan(activePlan);
                    sixthJoinObserver.RemoveActivePlan(activePlan);
                    deactivate(activePlan);
                });

            firstJoinObserver.AddActivePlan(activePlan);
            secondJoinObserver.AddActivePlan(activePlan);
            thirdJoinObserver.AddActivePlan(activePlan);
            fourthJoinObserver.AddActivePlan(activePlan);
            fifthJoinObserver.AddActivePlan(activePlan);
            sixthJoinObserver.AddActivePlan(activePlan);
            return activePlan;
        }
    }

    internal sealed class Plan<T1, T2, T3, T4, T5, T6, T7, TResult> : Plan<TResult>
    {
        internal Pattern<T1, T2, T3, T4, T5, T6, T7> Expression { get; }

        internal Func<T1, T2, T3, T4, T5, T6, T7, TResult> Selector { get; }

        internal Plan(Pattern<T1, T2, T3, T4, T5, T6, T7> expression,
                      Func<T1, T2, T3, T4, T5, T6, T7, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActivePlan Activate(Dictionary<object, IJoinObserver> externalSubscriptions,
                                              IObserver<TResult> observer, Action<ActivePlan> deactivate)
        {
            var onError = new Action<Exception>(observer.OnError);
            var firstJoinObserver = CreateObserver(externalSubscriptions, Expression.First, onError);
            var secondJoinObserver = CreateObserver(externalSubscriptions, Expression.Second, onError);
            var thirdJoinObserver = CreateObserver(externalSubscriptions, Expression.Third, onError);
            var fourthJoinObserver = CreateObserver(externalSubscriptions, Expression.Fourth, onError);
            var fifthJoinObserver = CreateObserver(externalSubscriptions, Expression.Fifth, onError);
            var sixthJoinObserver = CreateObserver(externalSubscriptions, Expression.Sixth, onError);
            var seventhJoinObserver = CreateObserver(externalSubscriptions, Expression.Seventh, onError);
            var activePlan = default(ActivePlan<T1, T2, T3, T4, T5, T6, T7>)!;

            activePlan = new ActivePlan<T1, T2, T3, T4, T5, T6, T7>(firstJoinObserver, secondJoinObserver, thirdJoinObserver,
                fourthJoinObserver, fifthJoinObserver, sixthJoinObserver, seventhJoinObserver,
                (first, second, third, fourth, fifth, sixth, seventh) =>
                {
                    TResult result;
                    try
                    {
                        result = Selector(first, second, third, fourth, fifth, sixth, seventh);
                    }
                    catch (Exception exception)
                    {
                        observer.OnError(exception);
                        return;
                    }
                    observer.OnNext(result);
                },
                () =>
                {
                    firstJoinObserver.RemoveActivePlan(activePlan);
                    secondJoinObserver.RemoveActivePlan(activePlan);
                    thirdJoinObserver.RemoveActivePlan(activePlan);
                    fourthJoinObserver.RemoveActivePlan(activePlan);
                    fifthJoinObserver.RemoveActivePlan(activePlan);
                    sixthJoinObserver.RemoveActivePlan(activePlan);
                    seventhJoinObserver.RemoveActivePlan(activePlan);
                    deactivate(activePlan);
                });

            firstJoinObserver.AddActivePlan(activePlan);
            secondJoinObserver.AddActivePlan(activePlan);
            thirdJoinObserver.AddActivePlan(activePlan);
            fourthJoinObserver.AddActivePlan(activePlan);
            fifthJoinObserver.AddActivePlan(activePlan);
            sixthJoinObserver.AddActivePlan(activePlan);
            seventhJoinObserver.AddActivePlan(activePlan);
            return activePlan;
        }
    }

    internal sealed class Plan<T1, T2, T3, T4, T5, T6, T7, T8, TResult> : Plan<TResult>
    {
        internal Pattern<T1, T2, T3, T4, T5, T6, T7, T8> Expression { get; }

        internal Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> Selector { get; }

        internal Plan(Pattern<T1, T2, T3, T4, T5, T6, T7, T8> expression,
                      Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActivePlan Activate(Dictionary<object, IJoinObserver> externalSubscriptions,
                                              IObserver<TResult> observer, Action<ActivePlan> deactivate)
        {
            var onError = new Action<Exception>(observer.OnError);
            var firstJoinObserver = CreateObserver(externalSubscriptions, Expression.First, onError);
            var secondJoinObserver = CreateObserver(externalSubscriptions, Expression.Second, onError);
            var thirdJoinObserver = CreateObserver(externalSubscriptions, Expression.Third, onError);
            var fourthJoinObserver = CreateObserver(externalSubscriptions, Expression.Fourth, onError);
            var fifthJoinObserver = CreateObserver(externalSubscriptions, Expression.Fifth, onError);
            var sixthJoinObserver = CreateObserver(externalSubscriptions, Expression.Sixth, onError);
            var seventhJoinObserver = CreateObserver(externalSubscriptions, Expression.Seventh, onError);
            var eighthJoinObserver = CreateObserver(externalSubscriptions, Expression.Eighth, onError);
            var activePlan = default(ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8>)!;

            activePlan = new ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8>(firstJoinObserver, secondJoinObserver, thirdJoinObserver,
                fourthJoinObserver, fifthJoinObserver, sixthJoinObserver, seventhJoinObserver, eighthJoinObserver,
                (first, second, third, fourth, fifth, sixth, seventh, eighth) =>
                {
                    TResult result;
                    try
                    {
                        result = Selector(first, second, third, fourth, fifth, sixth, seventh, eighth);
                    }
                    catch (Exception exception)
                    {
                        observer.OnError(exception);
                        return;
                    }
                    observer.OnNext(result);
                },
                () =>
                {
                    firstJoinObserver.RemoveActivePlan(activePlan);
                    secondJoinObserver.RemoveActivePlan(activePlan);
                    thirdJoinObserver.RemoveActivePlan(activePlan);
                    fourthJoinObserver.RemoveActivePlan(activePlan);
                    fifthJoinObserver.RemoveActivePlan(activePlan);
                    sixthJoinObserver.RemoveActivePlan(activePlan);
                    seventhJoinObserver.RemoveActivePlan(activePlan);
                    eighthJoinObserver.RemoveActivePlan(activePlan);
                    deactivate(activePlan);
                });

            firstJoinObserver.AddActivePlan(activePlan);
            secondJoinObserver.AddActivePlan(activePlan);
            thirdJoinObserver.AddActivePlan(activePlan);
            fourthJoinObserver.AddActivePlan(activePlan);
            fifthJoinObserver.AddActivePlan(activePlan);
            sixthJoinObserver.AddActivePlan(activePlan);
            seventhJoinObserver.AddActivePlan(activePlan);
            eighthJoinObserver.AddActivePlan(activePlan);
            return activePlan;
        }
    }

    internal sealed class Plan<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> : Plan<TResult>
    {
        internal Pattern<T1, T2, T3, T4, T5, T6, T7, T8, T9> Expression { get; }

        internal Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> Selector { get; }

        internal Plan(Pattern<T1, T2, T3, T4, T5, T6, T7, T8, T9> expression,
                      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActivePlan Activate(Dictionary<object, IJoinObserver> externalSubscriptions,
                                              IObserver<TResult> observer, Action<ActivePlan> deactivate)
        {
            var onError = new Action<Exception>(observer.OnError);
            var firstJoinObserver = CreateObserver(externalSubscriptions, Expression.First, onError);
            var secondJoinObserver = CreateObserver(externalSubscriptions, Expression.Second, onError);
            var thirdJoinObserver = CreateObserver(externalSubscriptions, Expression.Third, onError);
            var fourthJoinObserver = CreateObserver(externalSubscriptions, Expression.Fourth, onError);
            var fifthJoinObserver = CreateObserver(externalSubscriptions, Expression.Fifth, onError);
            var sixthJoinObserver = CreateObserver(externalSubscriptions, Expression.Sixth, onError);
            var seventhJoinObserver = CreateObserver(externalSubscriptions, Expression.Seventh, onError);
            var eighthJoinObserver = CreateObserver(externalSubscriptions, Expression.Eighth, onError);
            var ninthJoinObserver = CreateObserver(externalSubscriptions, Expression.Ninth, onError);
            var activePlan = default(ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8, T9>)!;

            activePlan = new ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8, T9>(firstJoinObserver, secondJoinObserver, thirdJoinObserver,
                fourthJoinObserver, fifthJoinObserver, sixthJoinObserver, seventhJoinObserver, eighthJoinObserver, ninthJoinObserver,
                (first, second, third, fourth, fifth, sixth, seventh, eighth, ninth) =>
                {
                    TResult result;
                    try
                    {
                        result = Selector(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth);
                    }
                    catch (Exception exception)
                    {
                        observer.OnError(exception);
                        return;
                    }
                    observer.OnNext(result);
                },
                () =>
                {
                    firstJoinObserver.RemoveActivePlan(activePlan);
                    secondJoinObserver.RemoveActivePlan(activePlan);
                    thirdJoinObserver.RemoveActivePlan(activePlan);
                    fourthJoinObserver.RemoveActivePlan(activePlan);
                    fifthJoinObserver.RemoveActivePlan(activePlan);
                    sixthJoinObserver.RemoveActivePlan(activePlan);
                    seventhJoinObserver.RemoveActivePlan(activePlan);
                    eighthJoinObserver.RemoveActivePlan(activePlan);
                    ninthJoinObserver.RemoveActivePlan(activePlan);
                    deactivate(activePlan);
                });

            firstJoinObserver.AddActivePlan(activePlan);
            secondJoinObserver.AddActivePlan(activePlan);
            thirdJoinObserver.AddActivePlan(activePlan);
            fourthJoinObserver.AddActivePlan(activePlan);
            fifthJoinObserver.AddActivePlan(activePlan);
            sixthJoinObserver.AddActivePlan(activePlan);
            seventhJoinObserver.AddActivePlan(activePlan);
            eighthJoinObserver.AddActivePlan(activePlan);
            ninthJoinObserver.AddActivePlan(activePlan);
            return activePlan;
        }
    }

    internal sealed class Plan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> : Plan<TResult>
    {
        internal Pattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Expression { get; }

        internal Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> Selector { get; }

        internal Plan(Pattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> expression,
                      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActivePlan Activate(Dictionary<object, IJoinObserver> externalSubscriptions,
                                              IObserver<TResult> observer, Action<ActivePlan> deactivate)
        {
            var onError = new Action<Exception>(observer.OnError);
            var firstJoinObserver = CreateObserver(externalSubscriptions, Expression.First, onError);
            var secondJoinObserver = CreateObserver(externalSubscriptions, Expression.Second, onError);
            var thirdJoinObserver = CreateObserver(externalSubscriptions, Expression.Third, onError);
            var fourthJoinObserver = CreateObserver(externalSubscriptions, Expression.Fourth, onError);
            var fifthJoinObserver = CreateObserver(externalSubscriptions, Expression.Fifth, onError);
            var sixthJoinObserver = CreateObserver(externalSubscriptions, Expression.Sixth, onError);
            var seventhJoinObserver = CreateObserver(externalSubscriptions, Expression.Seventh, onError);
            var eighthJoinObserver = CreateObserver(externalSubscriptions, Expression.Eighth, onError);
            var ninthJoinObserver = CreateObserver(externalSubscriptions, Expression.Ninth, onError);
            var tenthJoinObserver = CreateObserver(externalSubscriptions, Expression.Tenth, onError);
            var activePlan = default(ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>)!;

            activePlan = new ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(firstJoinObserver, secondJoinObserver, thirdJoinObserver,
                fourthJoinObserver, fifthJoinObserver, sixthJoinObserver, seventhJoinObserver, eighthJoinObserver, ninthJoinObserver, tenthJoinObserver,
                (first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth) =>
                {
                    TResult result;
                    try
                    {
                        result = Selector(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth);
                    }
                    catch (Exception exception)
                    {
                        observer.OnError(exception);
                        return;
                    }
                    observer.OnNext(result);
                },
                () =>
                {
                    firstJoinObserver.RemoveActivePlan(activePlan);
                    secondJoinObserver.RemoveActivePlan(activePlan);
                    thirdJoinObserver.RemoveActivePlan(activePlan);
                    fourthJoinObserver.RemoveActivePlan(activePlan);
                    fifthJoinObserver.RemoveActivePlan(activePlan);
                    sixthJoinObserver.RemoveActivePlan(activePlan);
                    seventhJoinObserver.RemoveActivePlan(activePlan);
                    eighthJoinObserver.RemoveActivePlan(activePlan);
                    ninthJoinObserver.RemoveActivePlan(activePlan);
                    tenthJoinObserver.RemoveActivePlan(activePlan);
                    deactivate(activePlan);
                });

            firstJoinObserver.AddActivePlan(activePlan);
            secondJoinObserver.AddActivePlan(activePlan);
            thirdJoinObserver.AddActivePlan(activePlan);
            fourthJoinObserver.AddActivePlan(activePlan);
            fifthJoinObserver.AddActivePlan(activePlan);
            sixthJoinObserver.AddActivePlan(activePlan);
            seventhJoinObserver.AddActivePlan(activePlan);
            eighthJoinObserver.AddActivePlan(activePlan);
            ninthJoinObserver.AddActivePlan(activePlan);
            tenthJoinObserver.AddActivePlan(activePlan);
            return activePlan;
        }
    }

    internal sealed class Plan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> : Plan<TResult>
    {
        internal Pattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Expression { get; }

        internal Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> Selector { get; }

        internal Plan(Pattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> expression,
                      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActivePlan Activate(Dictionary<object, IJoinObserver> externalSubscriptions,
                                              IObserver<TResult> observer, Action<ActivePlan> deactivate)
        {
            var onError = new Action<Exception>(observer.OnError);
            var firstJoinObserver = CreateObserver(externalSubscriptions, Expression.First, onError);
            var secondJoinObserver = CreateObserver(externalSubscriptions, Expression.Second, onError);
            var thirdJoinObserver = CreateObserver(externalSubscriptions, Expression.Third, onError);
            var fourthJoinObserver = CreateObserver(externalSubscriptions, Expression.Fourth, onError);
            var fifthJoinObserver = CreateObserver(externalSubscriptions, Expression.Fifth, onError);
            var sixthJoinObserver = CreateObserver(externalSubscriptions, Expression.Sixth, onError);
            var seventhJoinObserver = CreateObserver(externalSubscriptions, Expression.Seventh, onError);
            var eighthJoinObserver = CreateObserver(externalSubscriptions, Expression.Eighth, onError);
            var ninthJoinObserver = CreateObserver(externalSubscriptions, Expression.Ninth, onError);
            var tenthJoinObserver = CreateObserver(externalSubscriptions, Expression.Tenth, onError);
            var eleventhJoinObserver = CreateObserver(externalSubscriptions, Expression.Eleventh, onError);

            var activePlan = default(ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>)!;

            activePlan = new ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(firstJoinObserver, secondJoinObserver, thirdJoinObserver,
                fourthJoinObserver, fifthJoinObserver, sixthJoinObserver, seventhJoinObserver, eighthJoinObserver, ninthJoinObserver, tenthJoinObserver, eleventhJoinObserver,
                (first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh) =>
                {
                    TResult result;
                    try
                    {
                        result = Selector(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh);
                    }
                    catch (Exception exception)
                    {
                        observer.OnError(exception);
                        return;
                    }
                    observer.OnNext(result);
                },
                () =>
                {
                    firstJoinObserver.RemoveActivePlan(activePlan);
                    secondJoinObserver.RemoveActivePlan(activePlan);
                    thirdJoinObserver.RemoveActivePlan(activePlan);
                    fourthJoinObserver.RemoveActivePlan(activePlan);
                    fifthJoinObserver.RemoveActivePlan(activePlan);
                    sixthJoinObserver.RemoveActivePlan(activePlan);
                    seventhJoinObserver.RemoveActivePlan(activePlan);
                    eighthJoinObserver.RemoveActivePlan(activePlan);
                    ninthJoinObserver.RemoveActivePlan(activePlan);
                    tenthJoinObserver.RemoveActivePlan(activePlan);
                    eleventhJoinObserver.RemoveActivePlan(activePlan);
                    deactivate(activePlan);
                });

            firstJoinObserver.AddActivePlan(activePlan);
            secondJoinObserver.AddActivePlan(activePlan);
            thirdJoinObserver.AddActivePlan(activePlan);
            fourthJoinObserver.AddActivePlan(activePlan);
            fifthJoinObserver.AddActivePlan(activePlan);
            sixthJoinObserver.AddActivePlan(activePlan);
            seventhJoinObserver.AddActivePlan(activePlan);
            eighthJoinObserver.AddActivePlan(activePlan);
            ninthJoinObserver.AddActivePlan(activePlan);
            tenthJoinObserver.AddActivePlan(activePlan);
            eleventhJoinObserver.AddActivePlan(activePlan);
            return activePlan;
        }
    }

    internal sealed class Plan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> : Plan<TResult>
    {
        internal Pattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Expression { get; }

        internal Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> Selector { get; }

        internal Plan(Pattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> expression,
                      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActivePlan Activate(Dictionary<object, IJoinObserver> externalSubscriptions,
                                              IObserver<TResult> observer, Action<ActivePlan> deactivate)
        {
            var onError = new Action<Exception>(observer.OnError);
            var firstJoinObserver = CreateObserver(externalSubscriptions, Expression.First, onError);
            var secondJoinObserver = CreateObserver(externalSubscriptions, Expression.Second, onError);
            var thirdJoinObserver = CreateObserver(externalSubscriptions, Expression.Third, onError);
            var fourthJoinObserver = CreateObserver(externalSubscriptions, Expression.Fourth, onError);
            var fifthJoinObserver = CreateObserver(externalSubscriptions, Expression.Fifth, onError);
            var sixthJoinObserver = CreateObserver(externalSubscriptions, Expression.Sixth, onError);
            var seventhJoinObserver = CreateObserver(externalSubscriptions, Expression.Seventh, onError);
            var eighthJoinObserver = CreateObserver(externalSubscriptions, Expression.Eighth, onError);
            var ninthJoinObserver = CreateObserver(externalSubscriptions, Expression.Ninth, onError);
            var tenthJoinObserver = CreateObserver(externalSubscriptions, Expression.Tenth, onError);
            var eleventhJoinObserver = CreateObserver(externalSubscriptions, Expression.Eleventh, onError);
            var twelfthJoinObserver = CreateObserver(externalSubscriptions, Expression.Twelfth, onError);

            var activePlan = default(ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>)!;

            activePlan = new ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(firstJoinObserver, secondJoinObserver, thirdJoinObserver,
                fourthJoinObserver, fifthJoinObserver, sixthJoinObserver, seventhJoinObserver, eighthJoinObserver, ninthJoinObserver, tenthJoinObserver, eleventhJoinObserver,
                twelfthJoinObserver,
                (first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth) =>
                {
                    TResult result;
                    try
                    {
                        result = Selector(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth);
                    }
                    catch (Exception exception)
                    {
                        observer.OnError(exception);
                        return;
                    }
                    observer.OnNext(result);
                },
                () =>
                {
                    firstJoinObserver.RemoveActivePlan(activePlan);
                    secondJoinObserver.RemoveActivePlan(activePlan);
                    thirdJoinObserver.RemoveActivePlan(activePlan);
                    fourthJoinObserver.RemoveActivePlan(activePlan);
                    fifthJoinObserver.RemoveActivePlan(activePlan);
                    sixthJoinObserver.RemoveActivePlan(activePlan);
                    seventhJoinObserver.RemoveActivePlan(activePlan);
                    eighthJoinObserver.RemoveActivePlan(activePlan);
                    ninthJoinObserver.RemoveActivePlan(activePlan);
                    tenthJoinObserver.RemoveActivePlan(activePlan);
                    eleventhJoinObserver.RemoveActivePlan(activePlan);
                    twelfthJoinObserver.RemoveActivePlan(activePlan);
                    deactivate(activePlan);
                });

            firstJoinObserver.AddActivePlan(activePlan);
            secondJoinObserver.AddActivePlan(activePlan);
            thirdJoinObserver.AddActivePlan(activePlan);
            fourthJoinObserver.AddActivePlan(activePlan);
            fifthJoinObserver.AddActivePlan(activePlan);
            sixthJoinObserver.AddActivePlan(activePlan);
            seventhJoinObserver.AddActivePlan(activePlan);
            eighthJoinObserver.AddActivePlan(activePlan);
            ninthJoinObserver.AddActivePlan(activePlan);
            tenthJoinObserver.AddActivePlan(activePlan);
            eleventhJoinObserver.AddActivePlan(activePlan);
            twelfthJoinObserver.AddActivePlan(activePlan);
            return activePlan;
        }
    }


    internal sealed class Plan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> : Plan<TResult>
    {
        internal Pattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Expression { get; }

        internal Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> Selector { get; }

        internal Plan(Pattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> expression,
                      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActivePlan Activate(Dictionary<object, IJoinObserver> externalSubscriptions,
                                              IObserver<TResult> observer, Action<ActivePlan> deactivate)
        {
            var onError = new Action<Exception>(observer.OnError);
            var firstJoinObserver = CreateObserver(externalSubscriptions, Expression.First, onError);
            var secondJoinObserver = CreateObserver(externalSubscriptions, Expression.Second, onError);
            var thirdJoinObserver = CreateObserver(externalSubscriptions, Expression.Third, onError);
            var fourthJoinObserver = CreateObserver(externalSubscriptions, Expression.Fourth, onError);
            var fifthJoinObserver = CreateObserver(externalSubscriptions, Expression.Fifth, onError);
            var sixthJoinObserver = CreateObserver(externalSubscriptions, Expression.Sixth, onError);
            var seventhJoinObserver = CreateObserver(externalSubscriptions, Expression.Seventh, onError);
            var eighthJoinObserver = CreateObserver(externalSubscriptions, Expression.Eighth, onError);
            var ninthJoinObserver = CreateObserver(externalSubscriptions, Expression.Ninth, onError);
            var tenthJoinObserver = CreateObserver(externalSubscriptions, Expression.Tenth, onError);
            var eleventhJoinObserver = CreateObserver(externalSubscriptions, Expression.Eleventh, onError);
            var twelfthJoinObserver = CreateObserver(externalSubscriptions, Expression.Twelfth, onError);
            var thirteenthJoinObserver = CreateObserver(externalSubscriptions, Expression.Thirteenth, onError);

            var activePlan = default(ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>)!;

            activePlan = new ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(firstJoinObserver, secondJoinObserver, thirdJoinObserver,
                fourthJoinObserver, fifthJoinObserver, sixthJoinObserver, seventhJoinObserver, eighthJoinObserver, ninthJoinObserver, tenthJoinObserver, eleventhJoinObserver,
                twelfthJoinObserver, thirteenthJoinObserver,
                (first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth) =>
                {
                    TResult result;
                    try
                    {
                        result = Selector(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth);
                    }
                    catch (Exception exception)
                    {
                        observer.OnError(exception);
                        return;
                    }
                    observer.OnNext(result);
                },
                () =>
                {
                    firstJoinObserver.RemoveActivePlan(activePlan);
                    secondJoinObserver.RemoveActivePlan(activePlan);
                    thirdJoinObserver.RemoveActivePlan(activePlan);
                    fourthJoinObserver.RemoveActivePlan(activePlan);
                    fifthJoinObserver.RemoveActivePlan(activePlan);
                    sixthJoinObserver.RemoveActivePlan(activePlan);
                    seventhJoinObserver.RemoveActivePlan(activePlan);
                    eighthJoinObserver.RemoveActivePlan(activePlan);
                    ninthJoinObserver.RemoveActivePlan(activePlan);
                    tenthJoinObserver.RemoveActivePlan(activePlan);
                    eleventhJoinObserver.RemoveActivePlan(activePlan);
                    twelfthJoinObserver.RemoveActivePlan(activePlan);
                    thirteenthJoinObserver.RemoveActivePlan(activePlan);
                    deactivate(activePlan);
                });

            firstJoinObserver.AddActivePlan(activePlan);
            secondJoinObserver.AddActivePlan(activePlan);
            thirdJoinObserver.AddActivePlan(activePlan);
            fourthJoinObserver.AddActivePlan(activePlan);
            fifthJoinObserver.AddActivePlan(activePlan);
            sixthJoinObserver.AddActivePlan(activePlan);
            seventhJoinObserver.AddActivePlan(activePlan);
            eighthJoinObserver.AddActivePlan(activePlan);
            ninthJoinObserver.AddActivePlan(activePlan);
            tenthJoinObserver.AddActivePlan(activePlan);
            eleventhJoinObserver.AddActivePlan(activePlan);
            twelfthJoinObserver.AddActivePlan(activePlan);
            thirteenthJoinObserver.AddActivePlan(activePlan);
            return activePlan;
        }
    }


    internal sealed class Plan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> : Plan<TResult>
    {
        internal Pattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Expression { get; }

        internal Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> Selector { get; }

        internal Plan(Pattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> expression,
                      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActivePlan Activate(Dictionary<object, IJoinObserver> externalSubscriptions,
                                              IObserver<TResult> observer, Action<ActivePlan> deactivate)
        {
            var onError = new Action<Exception>(observer.OnError);
            var firstJoinObserver = CreateObserver(externalSubscriptions, Expression.First, onError);
            var secondJoinObserver = CreateObserver(externalSubscriptions, Expression.Second, onError);
            var thirdJoinObserver = CreateObserver(externalSubscriptions, Expression.Third, onError);
            var fourthJoinObserver = CreateObserver(externalSubscriptions, Expression.Fourth, onError);
            var fifthJoinObserver = CreateObserver(externalSubscriptions, Expression.Fifth, onError);
            var sixthJoinObserver = CreateObserver(externalSubscriptions, Expression.Sixth, onError);
            var seventhJoinObserver = CreateObserver(externalSubscriptions, Expression.Seventh, onError);
            var eighthJoinObserver = CreateObserver(externalSubscriptions, Expression.Eighth, onError);
            var ninthJoinObserver = CreateObserver(externalSubscriptions, Expression.Ninth, onError);
            var tenthJoinObserver = CreateObserver(externalSubscriptions, Expression.Tenth, onError);
            var eleventhJoinObserver = CreateObserver(externalSubscriptions, Expression.Eleventh, onError);
            var twelfthJoinObserver = CreateObserver(externalSubscriptions, Expression.Twelfth, onError);
            var thirteenthJoinObserver = CreateObserver(externalSubscriptions, Expression.Thirteenth, onError);
            var fourteenthJoinObserver = CreateObserver(externalSubscriptions, Expression.Fourteenth, onError);

            var activePlan = default(ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>)!;

            activePlan = new ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
                firstJoinObserver, secondJoinObserver, thirdJoinObserver,
                fourthJoinObserver, fifthJoinObserver, sixthJoinObserver,
                seventhJoinObserver, eighthJoinObserver, ninthJoinObserver,
                tenthJoinObserver, eleventhJoinObserver,
                twelfthJoinObserver, thirteenthJoinObserver,
                fourteenthJoinObserver,
                (first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth) =>
                {
                    TResult result;
                    try
                    {
                        result = Selector(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth);
                    }
                    catch (Exception exception)
                    {
                        observer.OnError(exception);
                        return;
                    }
                    observer.OnNext(result);
                },
                () =>
                {
                    firstJoinObserver.RemoveActivePlan(activePlan);
                    secondJoinObserver.RemoveActivePlan(activePlan);
                    thirdJoinObserver.RemoveActivePlan(activePlan);
                    fourthJoinObserver.RemoveActivePlan(activePlan);
                    fifthJoinObserver.RemoveActivePlan(activePlan);
                    sixthJoinObserver.RemoveActivePlan(activePlan);
                    seventhJoinObserver.RemoveActivePlan(activePlan);
                    eighthJoinObserver.RemoveActivePlan(activePlan);
                    ninthJoinObserver.RemoveActivePlan(activePlan);
                    tenthJoinObserver.RemoveActivePlan(activePlan);
                    eleventhJoinObserver.RemoveActivePlan(activePlan);
                    twelfthJoinObserver.RemoveActivePlan(activePlan);
                    thirteenthJoinObserver.RemoveActivePlan(activePlan);
                    fourteenthJoinObserver.RemoveActivePlan(activePlan);
                    deactivate(activePlan);
                });

            firstJoinObserver.AddActivePlan(activePlan);
            secondJoinObserver.AddActivePlan(activePlan);
            thirdJoinObserver.AddActivePlan(activePlan);
            fourthJoinObserver.AddActivePlan(activePlan);
            fifthJoinObserver.AddActivePlan(activePlan);
            sixthJoinObserver.AddActivePlan(activePlan);
            seventhJoinObserver.AddActivePlan(activePlan);
            eighthJoinObserver.AddActivePlan(activePlan);
            ninthJoinObserver.AddActivePlan(activePlan);
            tenthJoinObserver.AddActivePlan(activePlan);
            eleventhJoinObserver.AddActivePlan(activePlan);
            twelfthJoinObserver.AddActivePlan(activePlan);
            thirteenthJoinObserver.AddActivePlan(activePlan);
            fourteenthJoinObserver.AddActivePlan(activePlan);
            return activePlan;
        }
    }

    internal sealed class Plan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> : Plan<TResult>
    {
        internal Pattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Expression { get; }

        internal Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> Selector { get; }

        internal Plan(Pattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> expression,
                      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActivePlan Activate(Dictionary<object, IJoinObserver> externalSubscriptions,
                                              IObserver<TResult> observer, Action<ActivePlan> deactivate)
        {
            var onError = new Action<Exception>(observer.OnError);
            var firstJoinObserver = CreateObserver(externalSubscriptions, Expression.First, onError);
            var secondJoinObserver = CreateObserver(externalSubscriptions, Expression.Second, onError);
            var thirdJoinObserver = CreateObserver(externalSubscriptions, Expression.Third, onError);
            var fourthJoinObserver = CreateObserver(externalSubscriptions, Expression.Fourth, onError);
            var fifthJoinObserver = CreateObserver(externalSubscriptions, Expression.Fifth, onError);
            var sixthJoinObserver = CreateObserver(externalSubscriptions, Expression.Sixth, onError);
            var seventhJoinObserver = CreateObserver(externalSubscriptions, Expression.Seventh, onError);
            var eighthJoinObserver = CreateObserver(externalSubscriptions, Expression.Eighth, onError);
            var ninthJoinObserver = CreateObserver(externalSubscriptions, Expression.Ninth, onError);
            var tenthJoinObserver = CreateObserver(externalSubscriptions, Expression.Tenth, onError);
            var eleventhJoinObserver = CreateObserver(externalSubscriptions, Expression.Eleventh, onError);
            var twelfthJoinObserver = CreateObserver(externalSubscriptions, Expression.Twelfth, onError);
            var thirteenthJoinObserver = CreateObserver(externalSubscriptions, Expression.Thirteenth, onError);
            var fourteenthJoinObserver = CreateObserver(externalSubscriptions, Expression.Fourteenth, onError);
            var fifteenthJoinObserver = CreateObserver(externalSubscriptions, Expression.Fifteenth, onError);

            var activePlan = default(ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>)!;

            activePlan = new ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
                firstJoinObserver, secondJoinObserver, thirdJoinObserver,
                fourthJoinObserver, fifthJoinObserver, sixthJoinObserver,
                seventhJoinObserver, eighthJoinObserver, ninthJoinObserver,
                tenthJoinObserver, eleventhJoinObserver,
                twelfthJoinObserver, thirteenthJoinObserver,
                fourteenthJoinObserver, fifteenthJoinObserver,
                (first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth) =>
                {
                    TResult result;
                    try
                    {
                        result = Selector(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth);
                    }
                    catch (Exception exception)
                    {
                        observer.OnError(exception);
                        return;
                    }
                    observer.OnNext(result);
                },
                () =>
                {
                    firstJoinObserver.RemoveActivePlan(activePlan);
                    secondJoinObserver.RemoveActivePlan(activePlan);
                    thirdJoinObserver.RemoveActivePlan(activePlan);
                    fourthJoinObserver.RemoveActivePlan(activePlan);
                    fifthJoinObserver.RemoveActivePlan(activePlan);
                    sixthJoinObserver.RemoveActivePlan(activePlan);
                    seventhJoinObserver.RemoveActivePlan(activePlan);
                    eighthJoinObserver.RemoveActivePlan(activePlan);
                    ninthJoinObserver.RemoveActivePlan(activePlan);
                    tenthJoinObserver.RemoveActivePlan(activePlan);
                    eleventhJoinObserver.RemoveActivePlan(activePlan);
                    twelfthJoinObserver.RemoveActivePlan(activePlan);
                    thirteenthJoinObserver.RemoveActivePlan(activePlan);
                    fourteenthJoinObserver.RemoveActivePlan(activePlan);
                    fifteenthJoinObserver.RemoveActivePlan(activePlan);
                    deactivate(activePlan);
                });

            firstJoinObserver.AddActivePlan(activePlan);
            secondJoinObserver.AddActivePlan(activePlan);
            thirdJoinObserver.AddActivePlan(activePlan);
            fourthJoinObserver.AddActivePlan(activePlan);
            fifthJoinObserver.AddActivePlan(activePlan);
            sixthJoinObserver.AddActivePlan(activePlan);
            seventhJoinObserver.AddActivePlan(activePlan);
            eighthJoinObserver.AddActivePlan(activePlan);
            ninthJoinObserver.AddActivePlan(activePlan);
            tenthJoinObserver.AddActivePlan(activePlan);
            eleventhJoinObserver.AddActivePlan(activePlan);
            twelfthJoinObserver.AddActivePlan(activePlan);
            thirteenthJoinObserver.AddActivePlan(activePlan);
            fourteenthJoinObserver.AddActivePlan(activePlan);
            fifteenthJoinObserver.AddActivePlan(activePlan);
            return activePlan;
        }
    }
    internal sealed class Plan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> : Plan<TResult>
    {
        internal Pattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> Expression { get; }

        internal Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> Selector { get; }

        internal Plan(Pattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> expression,
                      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> selector)
        {
            Expression = expression;
            Selector = selector;
        }

        internal override ActivePlan Activate(Dictionary<object, IJoinObserver> externalSubscriptions,
                                              IObserver<TResult> observer, Action<ActivePlan> deactivate)
        {
            var onError = new Action<Exception>(observer.OnError);
            var firstJoinObserver = CreateObserver(externalSubscriptions, Expression.First, onError);
            var secondJoinObserver = CreateObserver(externalSubscriptions, Expression.Second, onError);
            var thirdJoinObserver = CreateObserver(externalSubscriptions, Expression.Third, onError);
            var fourthJoinObserver = CreateObserver(externalSubscriptions, Expression.Fourth, onError);
            var fifthJoinObserver = CreateObserver(externalSubscriptions, Expression.Fifth, onError);
            var sixthJoinObserver = CreateObserver(externalSubscriptions, Expression.Sixth, onError);
            var seventhJoinObserver = CreateObserver(externalSubscriptions, Expression.Seventh, onError);
            var eighthJoinObserver = CreateObserver(externalSubscriptions, Expression.Eighth, onError);
            var ninthJoinObserver = CreateObserver(externalSubscriptions, Expression.Ninth, onError);
            var tenthJoinObserver = CreateObserver(externalSubscriptions, Expression.Tenth, onError);
            var eleventhJoinObserver = CreateObserver(externalSubscriptions, Expression.Eleventh, onError);
            var twelfthJoinObserver = CreateObserver(externalSubscriptions, Expression.Twelfth, onError);
            var thirteenthJoinObserver = CreateObserver(externalSubscriptions, Expression.Thirteenth, onError);
            var fourteenthJoinObserver = CreateObserver(externalSubscriptions, Expression.Fourteenth, onError);
            var fifteenthJoinObserver = CreateObserver(externalSubscriptions, Expression.Fifteenth, onError);
            var sixteenthJoinObserver = CreateObserver(externalSubscriptions, Expression.Sixteenth, onError);

            var activePlan = default(ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>)!;

            activePlan = new ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
                firstJoinObserver, secondJoinObserver, thirdJoinObserver,
                fourthJoinObserver, fifthJoinObserver, sixthJoinObserver,
                seventhJoinObserver, eighthJoinObserver, ninthJoinObserver,
                tenthJoinObserver, eleventhJoinObserver,
                twelfthJoinObserver, thirteenthJoinObserver,
                fourteenthJoinObserver, fifteenthJoinObserver,
                sixteenthJoinObserver,
                (first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth, sixteenth) =>
                {
                    TResult result;
                    try
                    {
                        result = Selector(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth, sixteenth);
                    }
                    catch (Exception exception)
                    {
                        observer.OnError(exception);
                        return;
                    }
                    observer.OnNext(result);
                },
                () =>
                {
                    firstJoinObserver.RemoveActivePlan(activePlan);
                    secondJoinObserver.RemoveActivePlan(activePlan);
                    thirdJoinObserver.RemoveActivePlan(activePlan);
                    fourthJoinObserver.RemoveActivePlan(activePlan);
                    fifthJoinObserver.RemoveActivePlan(activePlan);
                    sixthJoinObserver.RemoveActivePlan(activePlan);
                    seventhJoinObserver.RemoveActivePlan(activePlan);
                    eighthJoinObserver.RemoveActivePlan(activePlan);
                    ninthJoinObserver.RemoveActivePlan(activePlan);
                    tenthJoinObserver.RemoveActivePlan(activePlan);
                    eleventhJoinObserver.RemoveActivePlan(activePlan);
                    twelfthJoinObserver.RemoveActivePlan(activePlan);
                    thirteenthJoinObserver.RemoveActivePlan(activePlan);
                    fourteenthJoinObserver.RemoveActivePlan(activePlan);
                    fifteenthJoinObserver.RemoveActivePlan(activePlan);
                    sixteenthJoinObserver.RemoveActivePlan(activePlan);
                    deactivate(activePlan);
                });

            firstJoinObserver.AddActivePlan(activePlan);
            secondJoinObserver.AddActivePlan(activePlan);
            thirdJoinObserver.AddActivePlan(activePlan);
            fourthJoinObserver.AddActivePlan(activePlan);
            fifthJoinObserver.AddActivePlan(activePlan);
            sixthJoinObserver.AddActivePlan(activePlan);
            seventhJoinObserver.AddActivePlan(activePlan);
            eighthJoinObserver.AddActivePlan(activePlan);
            ninthJoinObserver.AddActivePlan(activePlan);
            tenthJoinObserver.AddActivePlan(activePlan);
            eleventhJoinObserver.AddActivePlan(activePlan);
            twelfthJoinObserver.AddActivePlan(activePlan);
            thirteenthJoinObserver.AddActivePlan(activePlan);
            fourteenthJoinObserver.AddActivePlan(activePlan);
            fifteenthJoinObserver.AddActivePlan(activePlan);
            sixteenthJoinObserver.AddActivePlan(activePlan);
            return activePlan;
        }
    }
}
