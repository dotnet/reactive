// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Windows.Foundation;

namespace System.Reactive.Integration.WindowsRuntime
{
    internal sealed class EventPatternSource<TSender, TEventArgs> : EventPatternSourceBase<TSender, TEventArgs>, ITypedEventPatternSource<TSender, TEventArgs>
    {
        public EventPatternSource(IObservable<EventPattern<TSender, TEventArgs>> source, Action<Action<TSender, TEventArgs>, /*object,*/ EventPattern<TSender, TEventArgs>> invokeHandler)
            : base(source, invokeHandler)
        {
        }

        event TypedEventHandler<TSender, TEventArgs> ITypedEventPatternSource<TSender, TEventArgs>.OnNext
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
