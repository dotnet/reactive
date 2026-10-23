// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> OnErrorResumeNext<TSource>(this IAsyncObservable<TSource> first, IAsyncObservable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return new OnErrorResumeNextAsyncObservable<TSource>(new[] { first, second });
        }

        public static IAsyncObservable<TSource> OnErrorResumeNext<TSource>(params IAsyncObservable<TSource>[] sources) => OnErrorResumeNext((IEnumerable<IAsyncObservable<TSource>>)sources);

        public static IAsyncObservable<TSource> OnErrorResumeNext<TSource>(this IEnumerable<IAsyncObservable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return new OnErrorResumeNextAsyncObservable<TSource>(sources);
        }
    }

    // Rx.NET's OnErrorResumeNext over its TailRecursiveSink (TailRecursiveAsyncSink here), as
    // Catch is: each source is left for the next whether it completes or fails, its
    // subscription released as it goes, the result completes when the sources run dry, and a
    // chain, however built, is one subscription deep.
    internal sealed class OnErrorResumeNextAsyncObservable<TSource> : AsyncObservableBase<TSource>
    {
        private readonly IEnumerable<IAsyncObservable<TSource>> _sources;

        public OnErrorResumeNextAsyncObservable(IEnumerable<IAsyncObservable<TSource>> sources)
        {
            _sources = sources;
        }

        public IEnumerable<IAsyncObservable<TSource>> GetSources() => _sources;

        protected override async ValueTask<IAsyncDisposable> SubscribeAsyncCore(IAsyncObserver<TSource> observer)
        {
            var sink = new OnErrorResumeNextSink<TSource>(observer);

            await sink.RunAsync(_sources).ConfigureAwait(false);

            return sink;
        }
    }

    internal sealed class OnErrorResumeNextSink<TSource> : TailRecursiveAsyncSink<TSource>
    {
        public OnErrorResumeNextSink(IAsyncObserver<TSource> observer)
            : base(observer)
        {
        }

        // A source's error and completion alike mean the next source.
        public override ValueTask OnErrorAsync(Exception error) => RecurseAsync();

        public override ValueTask OnCompletedAsync() => RecurseAsync();

        protected override IEnumerable<IAsyncObservable<TSource>> Extract(IAsyncObservable<TSource> source) =>
            source is OnErrorResumeNextAsyncObservable<TSource> chain ? chain.GetSources() : null;

        protected override ValueTask DoneAsync() => CompleteAsync();

        // A Defer that throws is a source that failed, which is left for the next, as in Rx.NET.
        protected override ValueTask EvaluationFailedAsync(Exception error) => default;
    }
}
