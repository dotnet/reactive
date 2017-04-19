// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace System.Reactive.Linq
{
    internal partial class QueryLanguage
    {
        public virtual AsyncSubject<TSource> GetAwaiter<TSource>(IObservable<TSource> source)
        {
            return RunAsync<TSource>(source, CancellationToken.None);
        }

        public virtual AsyncSubject<TSource> GetAwaiter<TSource>(IConnectableObservable<TSource> source)
        {
            return RunAsync<TSource>(source, CancellationToken.None);
        }

        public virtual AsyncSubject<TSource> RunAsync<TSource>(IObservable<TSource> source, CancellationToken cancellationToken)
        {
            var s = new AsyncSubject<TSource>();

            if (cancellationToken.IsCancellationRequested)
            {
                return Cancel(s, cancellationToken);
            }

            var d = source.SubscribeSafe(s);

            if (cancellationToken.CanBeCanceled)
            {
                RegisterCancelation(s, d, cancellationToken);
            }

            return s;
        }

        public virtual AsyncSubject<TSource> RunAsync<TSource>(IConnectableObservable<TSource> source, CancellationToken cancellationToken)
        {
            var s = new AsyncSubject<TSource>();

            if (cancellationToken.IsCancellationRequested)
            {
                return Cancel(s, cancellationToken);
            }

            var d = source.SubscribeSafe(s);
            var c = source.Connect();

            if (cancellationToken.CanBeCanceled)
            {
                RegisterCancelation(s, StableCompositeDisposable.Create(d, c), cancellationToken);
            }

            return s;
        }

        private static AsyncSubject<T> Cancel<T>(AsyncSubject<T> subject, CancellationToken cancellationToken)
        {
            subject.OnError(new OperationCanceledException(cancellationToken));
            return subject;
        }

        private static void RegisterCancelation<T>(AsyncSubject<T> subject, IDisposable subscription, CancellationToken token)
        {
            //
            // Separate method used to avoid heap allocation of closure when no cancellation is needed,
            // e.g. when CancellationToken.None is provided to the RunAsync overloads.
            //

            var ctr = token.Register(() =>
            {
                subscription.Dispose();
                Cancel(subject, token);
            });

            //
            // No null-check for ctr is needed:
            //
            // - CancellationTokenRegistration is a struct
            // - Registration will succeed 99% of the time, no warranting an attempt to avoid spurious Subscribe calls
            //
            subject.Subscribe(Stubs<T>.Ignore, _ => ctr.Dispose(), ctr.Dispose);
        }
    }
}