// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Reactive.Joins
{
    public abstract class AsyncPlan<TResult>
    {
        internal AsyncPlan()
        {
        }

        internal abstract ActiveAsyncPlan Activate(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObserver<TResult> observer, Func<ActiveAsyncPlan, ValueTask> deactivate);

        internal static AsyncJoinObserver<TSource> CreateObserver<TSource>(Dictionary<object, IAsyncJoinObserver> externalSubscriptions, IAsyncObservable<TSource> observable, Func<Exception, ValueTask> onError)
        {
            var res = default(AsyncJoinObserver<TSource>);

            if (externalSubscriptions.TryGetValue(observable, out var joinObserver))
            {
                res = (AsyncJoinObserver<TSource>)joinObserver;
            }
            else
            {
                res = new AsyncJoinObserver<TSource>(observable, onError);
                externalSubscriptions.Add(observable, res);
            }

            return res;
        }
    }
}
