// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Dematerialize<TSource> : Producer<TSource, Dematerialize<TSource>._>
    {
        private readonly IObservable<Notification<TSource>> _source;

        public Dematerialize(IObservable<Notification<TSource>> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<Notification<TSource>, TSource> 
        {
            public _(IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            public override void OnNext(Notification<TSource> value)
            {
                switch (value.Kind)
                {
                    case NotificationKind.OnNext:
                        ForwardOnNext(value.Value);
                        break;
                    case NotificationKind.OnError:
                        ForwardOnError(value.Exception);
                        break;
                    case NotificationKind.OnCompleted:
                        ForwardOnCompleted();
                        break;
                }
            }
        }
    }
}
