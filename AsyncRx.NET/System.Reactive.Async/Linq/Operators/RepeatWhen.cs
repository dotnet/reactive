// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> RepeatWhen<TSource, TSignal>(this IAsyncObservable<TSource> source, Func<IAsyncObservable<object>, IAsyncObservable<TSignal>> handler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            return new RepeatWhenAsyncObservable<TSource, TSignal>(source, handler);
        }
    }
}
