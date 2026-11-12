// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Catch<TSource, TException>(this IAsyncObservable<TSource> source, Func<TException, IAsyncObservable<TSource>> handler)
            where TException : Exception
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            return Create(
                source,
                handler,
                static async (source, handler, observer) =>
                {
                    var sourceSubscription = new SingleAssignmentAsyncDisposable();

                    var (sink, handlerSubscription) = AsyncObserver.Catch(observer, sourceSubscription, handler);

                    // A source that fails during its own subscribe disposes its holder before it
                    // is assigned, which disposes the subscription the moment it is.
                    await sourceSubscription.AssignAsync(await source.SubscribeSafeAsync(sink).ConfigureAwait(false)).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(sourceSubscription, handlerSubscription);
                });
        }

        public static IAsyncObservable<TSource> Catch<TSource, TException>(this IAsyncObservable<TSource> source, Func<TException, ValueTask<IAsyncObservable<TSource>>> handler)
            where TException : Exception
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            return Create(
                source,
                handler,
                static async (source, handler, observer) =>
                {
                    var sourceSubscription = new SingleAssignmentAsyncDisposable();

                    var (sink, handlerSubscription) = AsyncObserver.Catch(observer, sourceSubscription, handler);

                    // A source that fails during its own subscribe disposes its holder before it
                    // is assigned, which disposes the subscription the moment it is.
                    await sourceSubscription.AssignAsync(await source.SubscribeSafeAsync(sink).ConfigureAwait(false)).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(sourceSubscription, handlerSubscription);
                });
        }

        public static IAsyncObservable<TSource> Catch<TSource>(this IAsyncObservable<TSource> first, IAsyncObservable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return new CatchAsyncObservable<TSource>(new[] { first, second });
        }

        public static IAsyncObservable<TSource> Catch<TSource>(params IAsyncObservable<TSource>[] sources) => Catch((IEnumerable<IAsyncObservable<TSource>>)sources);

        public static IAsyncObservable<TSource> Catch<TSource>(this IEnumerable<IAsyncObservable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return new CatchAsyncObservable<TSource>(sources);
        }
    }

    // Rx.NET's Catch over its TailRecursiveSink (TailRecursiveAsyncSink here): a source that
    // fails is left and the next is subscribed, the last error is the result's if the sources
    // run dry, and a chain of catches, however built, is one subscription deep. Retry is a
    // Catch over the same source repeated.
    internal sealed class CatchAsyncObservable<TSource> : AsyncObservableBase<TSource>
    {
        private readonly IEnumerable<IAsyncObservable<TSource>> _sources;

        public CatchAsyncObservable(IEnumerable<IAsyncObservable<TSource>> sources)
        {
            _sources = sources;
        }

        public IEnumerable<IAsyncObservable<TSource>> GetSources() => _sources;

        protected override async ValueTask<IAsyncDisposable> SubscribeAsyncCore(IAsyncObserver<TSource> observer)
        {
            var sink = new CatchSink<TSource>(observer);

            await sink.RunAsync(_sources).ConfigureAwait(false);

            return sink;
        }
    }

    internal sealed class CatchSink<TSource> : TailRecursiveAsyncSink<TSource>
    {
        private Exception _lastError;

        public CatchSink(IAsyncObserver<TSource> observer)
            : base(observer)
        {
        }

        // A failed source is left for the next; its error is the result's only if no source
        // follows.
        public override ValueTask OnErrorAsync(Exception error)
        {
            _lastError = error;
            return RecurseAsync();
        }

        public override ValueTask OnCompletedAsync() => CompleteAsync();

        // A type test, as in Rx.NET: nothing but a Catch is a catch chain, where Concat's
        // marker interface has several implementers.
        protected override IEnumerable<IAsyncObservable<TSource>> Extract(IAsyncObservable<TSource> source) =>
            source is CatchAsyncObservable<TSource> @catch ? @catch.GetSources() : null;

        protected override ValueTask DoneAsync() =>
            _lastError == null ? Observer.OnCompletedAsync() : Observer.OnErrorAsync(_lastError);

        // A Defer that throws is a source that failed, as in Rx.NET.
        protected override ValueTask EvaluationFailedAsync(Exception error)
        {
            _lastError = error;
            return default;
        }
    }

    public partial class AsyncObserver
    {
        public static (IAsyncObserver<TSource>, IAsyncDisposable) Catch<TSource, TException>(IAsyncObserver<TSource> observer, IAsyncDisposable sourceSubscription, Func<TException, IAsyncObservable<TSource>> handler)
            where TException : Exception
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (sourceSubscription == null)
                throw new ArgumentNullException(nameof(sourceSubscription));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            return Catch<TSource, TException>(observer, sourceSubscription, ex => new ValueTask<IAsyncObservable<TSource>>(handler(ex)));
        }

        // The sink for the handler forms. It observes the source; sourceSubscription is the
        // source's subscription (or a holder for it), released once the handler's sequence has
        // been subscribed, as in Rx.NET; the returned disposable holds that subscription.
        public static (IAsyncObserver<TSource>, IAsyncDisposable) Catch<TSource, TException>(IAsyncObserver<TSource> observer, IAsyncDisposable sourceSubscription, Func<TException, ValueTask<IAsyncObservable<TSource>>> handler)
            where TException : Exception
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (sourceSubscription == null)
                throw new ArgumentNullException(nameof(sourceSubscription));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            var handlerSubscription = new SingleAssignmentAsyncDisposable();

            var sink = Create<TSource>(
                observer.OnNextAsync,
                async ex =>
                {
                    if (ex is TException error)
                    {
                        IAsyncObservable<TSource> handlerObservable;

                        try
                        {
                            handlerObservable = await handler(error).ConfigureAwait(false);
                        }
                        catch (Exception err)
                        {
                            await observer.OnErrorAsync(err).ConfigureAwait(false);
                            return;
                        }

                        // The handler's sequence goes straight to the observer: in Rx.NET it is
                        // subscribed with the sink, whose once-only flag then forwards whatever
                        // comes, which is the same thing one subscription nearer.
                        var subscription = await handlerObservable.SubscribeSafeAsync(observer).ConfigureAwait(false);

                        await sourceSubscription.DisposeAsync().ConfigureAwait(false);

                        await handlerSubscription.AssignAsync(subscription).ConfigureAwait(false);
                    }
                    else
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    }
                },
                observer.OnCompletedAsync
            );

            return (sink, handlerSubscription);
        }
    }
}
