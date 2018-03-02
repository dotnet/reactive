// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IConnectableAsyncObservable<TResult> Multicast<TSource, TResult>(this IAsyncObservable<TSource> source, IAsyncSubject<TSource, TResult> subject)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (subject == null)
                throw new ArgumentNullException(nameof(subject));

            return new ConnectableAsyncObservable<TSource, TResult>(source, subject);
        }

        public static IAsyncObservable<TSource> Multicast<TSource>(this IAsyncObservable<TSource> source, Func<IAsyncSubject<TSource>> subjectFactory)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (subjectFactory == null)
                throw new ArgumentNullException(nameof(subjectFactory));

            return Multicast(source, () => Task.FromResult<IAsyncSubject<TSource, TSource>>(subjectFactory()), x => Task.FromResult(x));
        }

        public static IAsyncObservable<TSource> Multicast<TSource>(this IAsyncObservable<TSource> source, Func<Task<IAsyncSubject<TSource>>> subjectFactory)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (subjectFactory == null)
                throw new ArgumentNullException(nameof(subjectFactory));

            return Multicast<TSource, TSource, TSource>(source, async () => await subjectFactory().ConfigureAwait(false), x => Task.FromResult(x));
        }

        public static IAsyncObservable<TResult> Multicast<TSource, TIntermediate, TResult>(this IAsyncObservable<TSource> source, Func<IAsyncSubject<TSource, TIntermediate>> subjectFactory, Func<IAsyncObservable<TIntermediate>, IAsyncObservable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (subjectFactory == null)
                throw new ArgumentNullException(nameof(subjectFactory));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Multicast(source, () => Task.FromResult(subjectFactory()), x => Task.FromResult(selector(x)));
        }

        public static IAsyncObservable<TResult> Multicast<TSource, TIntermediate, TResult>(this IAsyncObservable<TSource> source, Func<Task<IAsyncSubject<TSource, TIntermediate>>> subjectFactory, Func<IAsyncObservable<TIntermediate>, Task<IAsyncObservable<TResult>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (subjectFactory == null)
                throw new ArgumentNullException(nameof(subjectFactory));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            // REVIEW: Use a lifted observer operator.

            return Create<TResult>(async observer =>
            {
                var observable = default(IAsyncObservable<TResult>);
                var connectable = default(IConnectableAsyncObservable<TIntermediate>);

                try
                {
                    var subject = await subjectFactory().ConfigureAwait(false);
                    connectable = new ConnectableAsyncObservable<TSource, TIntermediate>(source, subject);
                    observable = await selector(connectable).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    return AsyncDisposable.Nop;
                }

                var d = new CompositeAsyncDisposable();

                var subscription = await observable.SubscribeAsync(observer).ConfigureAwait(false);
                await d.AddAsync(subscription).ConfigureAwait(false);

                var connection = await connectable.ConnectAsync().ConfigureAwait(false);
                await d.AddAsync(connection).ConfigureAwait(false);

                return d;
            });
        }
    }
}
