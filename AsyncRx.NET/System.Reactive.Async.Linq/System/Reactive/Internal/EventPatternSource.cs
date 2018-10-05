// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive
{
    internal sealed class EventPatternSource<TEventArgs> : EventPatternSourceBase<object, TEventArgs>, IEventPatternSource<TEventArgs>
    {
        public EventPatternSource(IAsyncObservable<EventPattern<object, TEventArgs>> source, Action<Action<object, TEventArgs>, /*object,*/ EventPattern<object, TEventArgs>> invokeHandler)
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
