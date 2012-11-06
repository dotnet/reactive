// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if HAS_AWAIT
using System.Threading;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace System.Reactive.Linq
{
    internal partial class QueryLanguage
    {
        public virtual AsyncSubject<TSource> GetAwaiter<TSource>(IObservable<TSource> source)
        {
            var s = new AsyncSubject<TSource>();
            source.SubscribeSafe(s);
            return s;
        }

        public virtual AsyncSubject<TSource> GetAwaiter<TSource>(IConnectableObservable<TSource> source)
        {
            var s = new AsyncSubject<TSource>();
            source.SubscribeSafe(s);
            source.Connect();
            return s;
        }

        public virtual AsyncSubject<TSource> RunAsync<TSource>(IObservable<TSource> source, CancellationToken cancellationToken)
        {
            var s = new AsyncSubject<TSource>();

            var cancel = new Action(() => s.OnError(new OperationCanceledException()));
            if (cancellationToken.IsCancellationRequested)
            {
                cancel();
                return s;
            }

            var d = source.SubscribeSafe(s);
            cancellationToken.Register(d.Dispose);
            cancellationToken.Register(cancel);

            return s;
        }

        public virtual AsyncSubject<TSource> RunAsync<TSource>(IConnectableObservable<TSource> source, CancellationToken cancellationToken)
        {
            var s = new AsyncSubject<TSource>();

            var cancel = new Action(() => s.OnError(new OperationCanceledException()));
            if (cancellationToken.IsCancellationRequested)
            {
                cancel();
                return s;
            }

            var d = new CompositeDisposable(source.SubscribeSafe(s), source.Connect());
            cancellationToken.Register(d.Dispose);
            cancellationToken.Register(cancel);

            return s;
        }
    }
}
#endif