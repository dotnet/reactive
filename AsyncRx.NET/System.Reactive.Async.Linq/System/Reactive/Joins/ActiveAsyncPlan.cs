// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Reactive.Joins
{
    internal abstract class ActiveAsyncPlan
    {
        private readonly Dictionary<IAsyncJoinObserver, IAsyncJoinObserver> joinObservers = new Dictionary<IAsyncJoinObserver, IAsyncJoinObserver>();

        internal abstract Task Match();

        protected void AddJoinObserver(IAsyncJoinObserver joinObserver)
        {
            joinObservers.Add(joinObserver, joinObserver);
        }

        protected void Dequeue()
        {
            foreach (var observer in joinObservers.Values)
            {
                observer.Dequeue();
            }
        }
    }
}
