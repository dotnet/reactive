// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Reactive.Joins
{
    internal abstract class ActiveAsyncPlan
    {
        private readonly Dictionary<IAsyncJoinObserver, IAsyncJoinObserver> _joinObservers = new();

        internal abstract Task Match();

        protected void AddJoinObserver(IAsyncJoinObserver joinObserver)
        {
            // A plan may name the same source more than once (xs.And(xs)); the source has one
            // join observer, which then holds more than one position in the plan, and a dequeue
            // must take one element from it, not one per position.
            if (!_joinObservers.ContainsKey(joinObserver))
            {
                _joinObservers.Add(joinObserver, joinObserver);
            }
        }

        protected void Dequeue()
        {
            foreach (var observer in _joinObservers.Values)
            {
                observer.Dequeue();
            }
        }
    }
}
