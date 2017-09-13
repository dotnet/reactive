// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IEventSource<Unit> ToEvent<TSource>(this IAsyncObservable<Unit> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new EventSource<Unit>(source, (onNext, _) => onNext(Unit.Default));
        }

        public static IEventSource<TSource> ToEvent<TSource>(this IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new EventSource<TSource>(source, (onNext, x) => onNext(x));
        }
    }
}
