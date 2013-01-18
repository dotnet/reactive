// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Reactive
{
    class EventPatternSource<TEventArgs> : EventPatternSourceBase<object, TEventArgs>, IEventPatternSource<TEventArgs>
#if !NO_EVENTARGS_CONSTRAINT
        where TEventArgs : EventArgs
#endif
    {
        public EventPatternSource(IObservable<EventPattern<object, TEventArgs>> source, Action<Action<object, TEventArgs>, /*object,*/ EventPattern<object, TEventArgs>> invokeHandler)
            : base(source, invokeHandler)
        {
        }

        event EventHandler<TEventArgs> IEventPatternSource<TEventArgs>.OnNext
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
