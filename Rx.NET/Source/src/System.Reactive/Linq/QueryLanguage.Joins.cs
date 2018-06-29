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
            return new WhenObservable<TResult>(plans);
        }

        private sealed class WhenObservable<TResult> : ObservableBase<TResult>
        {
            private readonly IEnumerable<Plan<TResult>> _plans;

            public WhenObservable(IEnumerable<Plan<TResult>> plans)
            {
                _plans = plans;
            }

            protected override IDisposable SubscribeCore(IObserver<TResult> observer)
            {
                var externalSubscriptions = new Dictionary<object, IJoinObserver>();
                var gate = new object();
                var activePlans = new List<ActivePlan>();
                var outObserver = new OutObserver(observer, externalSubscriptions);
                try
                {
                    void onDeactivate(ActivePlan activePlan)
                    {
                        activePlans.Remove(activePlan);
                        if (activePlans.Count == 0)
                        {
                            outObserver.OnCompleted();
                        }
                    }

                    foreach (var plan in _plans)
                    {
                        activePlans.Add(plan.Activate(externalSubscriptions, outObserver,
                                                      onDeactivate));
                    }
                }
                catch (Exception e)
                {
                    //
                    // [OK] Use of unsafe Subscribe: we're calling into a known producer implementation.
                    //
                    observer.OnError(e);
                    return Disposable.Empty;
                }

                var group = new CompositeDisposable(externalSubscriptions.Values.Count);
                foreach (var joinObserver in externalSubscriptions.Values)
                {
                    joinObserver.Subscribe(gate);
                    group.Add(joinObserver);
                }
                return group;
            }

            private sealed class OutObserver : IObserver<TResult>
            {
                private readonly IObserver<TResult> _observer;
                private readonly Dictionary<object, IJoinObserver> _externalSubscriptions;

                public OutObserver(IObserver<TResult> observer, Dictionary<object, IJoinObserver> externalSubscriptions)
                {
                    _observer = observer;
                    _externalSubscriptions = externalSubscriptions;
                }

                public void OnCompleted()
                {
                    _observer.OnCompleted();
                }

                public void OnError(Exception exception)
                {
                    foreach (var po in _externalSubscriptions.Values)
                    {
                        po.Dispose();
                    }
                    _observer.OnError(exception);
                }

                public void OnNext(TResult value)
                {
                    _observer.OnNext(value);
                }
            }
        }

        #endregion
    }
}
