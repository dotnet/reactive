// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Using<TSource, TResource> : Producer<TSource, Using<TSource, TResource>._>
        where TResource : IDisposable
    {
        private readonly Func<TResource> _resourceFactory;
        private readonly Func<TResource, IObservable<TSource>> _observableFactory;

        public Using(Func<TResource> resourceFactory, Func<TResource, IObservable<TSource>> observableFactory)
        {
            _resourceFactory = resourceFactory;
            _observableFactory = observableFactory;
        }

        protected override _ CreateSink(IObserver<TSource> observer) => new _(observer);

        protected override void Run(_ sink) => sink.Run(this);

        internal sealed class _ : IdentitySink<TSource>
        {
            public _(IObserver<TSource> observer)
                : base(observer)
            {
            }

            private IDisposable _disposable;

            public void Run(Using<TSource, TResource> parent)
            {
                var source = default(IObservable<TSource>);
                var disposable = Disposable.Empty;
                try
                {
                    var resource = parent._resourceFactory();
                    if (resource != null)
                    {
                        disposable = resource;
                    }

                    source = parent._observableFactory(resource);
                }
                catch (Exception exception)
                {
                    source = Observable.Throw<TSource>(exception);
                }

                // It is important to set the disposable resource after
                // Run(). In the synchronous case this would else dispose
                // the the resource before the source subscription.
                Run(source);
                Disposable.SetSingle(ref _disposable, disposable);
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                if (disposing)
                {
                    Disposable.TryDispose(ref _disposable);
                }
            }
        }
    }
}
