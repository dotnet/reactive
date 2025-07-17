// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
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

        protected override _ CreateSink(IObserver<TSource> observer) => new(observer);

        protected override void Run(_ sink) => sink.Run(this);

        internal sealed class _ : IdentitySink<TSource>
        {
            public _(IObserver<TSource> observer)
                : base(observer)
            {
            }

            private SingleAssignmentDisposableValue _disposable;

            public void Run(Using<TSource, TResource> parent)
            {
                IObservable<TSource> source;
                var disposable = Disposable.Empty;
                try
                {
                    var resource = parent._resourceFactory();
                    if (resource != null)
                    {
                        disposable = resource;
                    }

                    //
                    // NB: We do allow the factory to return `null`, similar to the `using` statement in C#. However, we don't want to bother
                    //     users with a TResource? parameter and cause a breaking change to their code, even if their factory returns non-null.
                    //     Right now, we can't track non-null state across the invocation of resourceFactory into observableFactory. If we'd
                    //     be able to do that, it would make sense to warn users about a possible null. In the absence of this, we'd end up
                    //     with a lot of false positives (in fact, most code would cause a warning), and force users to pollute their code with
                    //     the "damn-it" ! operator.
                    //

                    source = parent._observableFactory(resource!);
                }
                catch (Exception exception)
                {
                    source = Observable.Throw<TSource>(exception);
                }

                // It is important to set the disposable resource after
                // Run(). In the synchronous case this would else dispose
                // the the resource before the source subscription.
                Run(source);
                _disposable.Disposable = disposable;
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                if (disposing)
                {
                    _disposable.Dispose();
                }
            }
        }
    }
}
