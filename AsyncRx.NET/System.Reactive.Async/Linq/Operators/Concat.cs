// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{

    public partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Concat<TSource>(this IAsyncObservable<TSource> first, IAsyncObservable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return new ConcatAsyncObservable<TSource>(new[] { first, second });
        }

        public static IAsyncObservable<TSource> Concat<TSource>(params IAsyncObservable<TSource>[] sources) => Concat((IEnumerable<IAsyncObservable<TSource>>)sources);

        public static IAsyncObservable<TSource> Concat<TSource>(this IAsyncObservable<IAsyncObservable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            // As in Rx.NET: a merge that subscribes to one inner sequence at a time.
            return sources.Merge(1);
        }

        public static IAsyncObservable<TSource> Concat<TSource>(this IAsyncObservable<Task<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            // As in Rx.NET: each task becomes a sequence of its result, concatenated in order.
            return sources.Select(static task => task.ToAsyncObservable()).Concat();
        }

        public static IAsyncObservable<TSource> Concat<TSource>(this IEnumerable<IAsyncObservable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return new ConcatAsyncObservable<TSource>(sources);
        }
    }

    // Rx.NET's Concat over its TailRecursiveSink (TailRecursiveAsyncSink here): a source that
    // completes is released and the next is subscribed, and a chain of concatenations, however
    // built, is one subscription deep.
    internal sealed class ConcatAsyncObservable<TSource> : AsyncObservableBase<TSource>, IConcatenatableAsyncObservable<TSource>
    {
        private readonly IEnumerable<IAsyncObservable<TSource>> _sources;

        public ConcatAsyncObservable(IEnumerable<IAsyncObservable<TSource>> sources)
        {
            _sources = sources;
        }

        public IEnumerable<IAsyncObservable<TSource>> GetSources() => _sources;

        protected override async ValueTask<IAsyncDisposable> SubscribeAsyncCore(IAsyncObserver<TSource> observer)
        {
            var sink = new ConcatSink<TSource>(observer);

            await sink.RunAsync(_sources).ConfigureAwait(false);

            return sink;
        }
    }

    internal sealed class ConcatSink<TSource> : TailRecursiveAsyncSink<TSource>
    {
        public ConcatSink(IAsyncObserver<TSource> observer)
            : base(observer)
        {
        }

        public override ValueTask OnErrorAsync(Exception error) => FailAsync(error);

        // The completed source's subscription has been released by the time this is called (see
        // SubscribeAsync below); the drain moves on to the next source.
        public override ValueTask OnCompletedAsync() => RecurseAsync();

        protected override IEnumerable<IAsyncObservable<TSource>> Extract(IAsyncObservable<TSource> source) =>
            source is IConcatenatableAsyncObservable<TSource> concatenatable ? concatenatable.GetSources() : null;

        protected override ValueTask DoneAsync() => CompleteAsync();

        // Each source's subscription is released as soon as it completes, before the next is
        // subscribed, as in Rx.NET.
        protected override ValueTask<IAsyncDisposable> SubscribeAsync(IAsyncObservable<TSource> source) =>
            AsyncObservable.SubscribeReleasingOnCompletedAsync(source, this);
    }
}
