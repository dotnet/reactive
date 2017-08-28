// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Joins;

namespace System.Reactive.Linq
{
    internal partial class QueryLanguage
    {
        #region And

        public virtual Pattern<TLeft, TRight> And<TLeft, TRight>(IObservable<TLeft> left, IObservable<TRight> right)
        {
            return new Pattern<TLeft, TRight>(left, right);
        }

        #endregion

        #region Then

        public virtual Plan<TResult> Then<TSource, TResult>(IObservable<TSource> source, Func<TSource, TResult> selector)
        {
            return new Pattern<TSource>(source).Then(selector);
        }

        #endregion

        #region When

        public virtual IObservable<TResult> When<TResult>(params Plan<TResult>[] plans)
        {
            return When((IEnumerable<Plan<TResult>>)plans);
        }

        public virtual IObservable<TResult> When<TResult>(IEnumerable<Plan<TResult>> plans)
        {
            return new AnonymousObservable<TResult>(observer =>
            {
                var externalSubscriptions = new Dictionary<object, IJoinObserver>();
                var gate = new object();
                var activePlans = new List<ActivePlan>();
                var outObserver = Observer.Create<TResult>(observer.OnNext,
                    exception =>
                    {
                        foreach (var po in externalSubscriptions.Values)
                        {
                            po.Dispose();
                        }
                        observer.OnError(exception);
                    },
                    observer.OnCompleted);
                try
                {
                    foreach (var plan in plans)
                        activePlans.Add(plan.Activate(externalSubscriptions, outObserver,
                                                      activePlan =>
                                                      {
                                                          activePlans.Remove(activePlan);
                                                          if (activePlans.Count == 0)
                                                              outObserver.OnCompleted();
                                                      }));
                }
                catch (Exception e)
                {
                    //
                    // [OK] Use of unsafe Subscribe: we're calling into a known producer implementation.
                    //
                    return Throw<TResult>(e).Subscribe/*Unsafe*/(observer);
                }

                var group = new CompositeDisposable(externalSubscriptions.Values.Count);
                foreach (var joinObserver in externalSubscriptions.Values)
                {
                    joinObserver.Subscribe(gate);
                    group.Add(joinObserver);
                }
                return group;
            });
        }

        #endregion
    }
}
