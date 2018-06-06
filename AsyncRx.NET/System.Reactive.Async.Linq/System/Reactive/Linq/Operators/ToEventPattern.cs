// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IEventPatternSource<TEventArgs> ToEventPattern<TEventArgs>(this IAsyncObservable<EventPattern<TEventArgs>> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new EventPatternSource<TEventArgs>(source, (onNext, e) => onNext(e.Sender, e.EventArgs));
        }
    }
}
