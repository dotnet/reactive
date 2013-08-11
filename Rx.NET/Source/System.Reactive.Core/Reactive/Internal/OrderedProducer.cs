// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace System.Reactive
{
    abstract class OrderedProducer<TSource> : Producer<TSource>
    {
        internal readonly IObservable<TSource> _source;
        private readonly OrderedProducer<TSource> _previous;

        protected OrderedProducer(IObservable<TSource> source, OrderedProducer<TSource> previous)
        {
            _source = source;
            _previous = previous;
        }

        protected abstract SortSink Sort(IObserver<TSource> observer, IDisposable cancel);

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = Sort(observer, cancel);
            setSink(sink);

            var disposables = new CompositeDisposable();

            var p = _previous;
            while (p != null)
            {
                // p.Sort may return the same sink reference that was passed to it, so we need to ensure that initialization only occurs once
                if (!sink._initialized)
                    disposables.Add(sink.InitializeAndSet());

                sink = p.Sort(sink, Disposable.Empty);
                p = p._previous;
            }

            if (disposables.Count == 0)
            {
                Debug.Assert(!sink._initialized);

                var d = sink.InitializeAndSet();
                sink.Run(_source);
                return d;
            }
            else
            {
                if (!sink._initialized)
                    disposables.Add(sink.InitializeAndSet());

                sink.Run(_source);
                return new CompositeDisposable(disposables.Reverse());
            }
        }

        protected abstract class SortSink : Sink<TSource>, IObserver<TSource>
        {
            internal bool _initialized;

            protected SortSink(IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            internal IDisposable InitializeAndSet()
            {
                _initialized = true;
                return Initialize();
            }

            public abstract IDisposable Initialize();

            public abstract void Run(IObservable<TSource> source);

            public abstract void OnNext(TSource value);

            public abstract void OnError(Exception error);

            public abstract void OnCompleted();
        }
    }
}