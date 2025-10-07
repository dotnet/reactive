// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

extern alias SystemReactive;
using SystemReactive::System.Reactive;

using Windows.Foundation;

namespace System.Reactive
{
    internal sealed class EventPatternSource<TSender, TEventArgs> : EventPatternSourceBase<TSender, TEventArgs>, IEventPatternSource<TSender, TEventArgs>
    {
        public EventPatternSource(IObservable<EventPattern<TSender, TEventArgs>> source, Action<Action<TSender, TEventArgs>, /*object,*/ EventPattern<TSender, TEventArgs>> invokeHandler)
            : base(source, invokeHandler)
        {
        }

        event TypedEventHandler<TSender, TEventArgs> IEventPatternSource<TSender, TEventArgs>.OnNext
        {
            add
            {
                Add(value, (o, e) => value(o!, e));
            }

            remove
            {
                Remove(value);
            }
        }
    }
}
