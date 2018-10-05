// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

#if HAS_WINRT
using Windows.Foundation;

namespace System.Reactive
{
    internal class EventPatternSource<TSender, TEventArgs> : EventPatternSourceBase<TSender, TEventArgs>, IEventPatternSource<TSender, TEventArgs>
    {
        public EventPatternSource(IObservable<EventPattern<TSender, TEventArgs>> source, Action<Action<TSender, TEventArgs>, /*object,*/ EventPattern<TSender, TEventArgs>> invokeHandler)
            : base(source, invokeHandler)
        {
        }

        event TypedEventHandler<TSender, TEventArgs> IEventPatternSource<TSender, TEventArgs>.OnNext
        {
            add
            {
                Add(value, (o, e) => value(o, e));
            }

            remove
            {
                Remove(value);
            }
        }
    }
}
#endif