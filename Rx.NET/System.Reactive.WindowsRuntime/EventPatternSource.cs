// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if HAS_WINRT
using Windows.Foundation;

namespace System.Reactive
{
    class EventPatternSource<TSender, TEventArgs> : EventPatternSourceBase<TSender, TEventArgs>, IEventPatternSource<TSender, TEventArgs>
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