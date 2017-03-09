// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

#if !NO_PERF
using System;

namespace System.Reactive.Linq.ObservableImpl
{
    class Dematerialize<TSource> : Producer<TSource>
    {
        private readonly IObservable<Notification<TSource>> _source;

        public Dematerialize(IObservable<Notification<TSource>> source)
        {
            _source = source;
        }

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(observer, cancel);
            setSink(sink);
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<TSource>, IObserver<Notification<TSource>>
        {
            public _(IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            public void OnNext(Notification<TSource> value)
            {
                switch (value.Kind)
                {
                    case NotificationKind.OnNext:
                        base._observer.OnNext(value.Value);
                        break;
                    case NotificationKind.OnError:
                        base._observer.OnError(value.Exception);
                        base.Dispose();
                        break;
                    case NotificationKind.OnCompleted:
                        base._observer.OnCompleted();
                        base.Dispose();
                        break;
                }
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }
}
#endif