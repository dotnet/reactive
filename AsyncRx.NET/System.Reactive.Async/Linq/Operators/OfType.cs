// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TResult> OfType<TSource, TResult>(this IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<TSource, TResult>(source, static (source, observer) => source.SubscribeSafeAsync(AsyncObserver.OfType<TSource, TResult>(observer)));
        }

        // As in Rx.NET: over a sequence of objects, so that only the result type is named, which
        // is how the operator is called (the two-type-parameter form cannot infer TResult alone).
        public static IAsyncObservable<TResult> OfType<TResult>(this IAsyncObservable<object?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return OfType<object, TResult>(source);
        }
    }

    public partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> OfType<TSource, TResult>(IAsyncObserver<TResult> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return Where(Cast<TSource, TResult>(observer), x => x is TResult);
        }
    }
}
