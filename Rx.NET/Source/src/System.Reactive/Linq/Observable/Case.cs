// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Case<TValue, TResult> : Producer<TResult, Case<TValue, TResult>._>, IEvaluatableObservable<TResult>
    {
        private readonly Func<TValue> _selector;
        private readonly IDictionary<TValue, IObservable<TResult>> _sources;
        private readonly IObservable<TResult> _defaultSource;

        public Case(Func<TValue> selector, IDictionary<TValue, IObservable<TResult>> sources, IObservable<TResult> defaultSource)
        {
            _selector = selector;
            _sources = sources;
            _defaultSource = defaultSource;
        }

        public IObservable<TResult> Eval()
        {
            if (_sources.TryGetValue(_selector(), out var res))
            {
                return res;
            }

            return _defaultSource;
        }

        protected override _ CreateSink(IObserver<TResult> observer) => new _(observer);

        protected override void Run(_ sink) => sink.Run(this);

        internal sealed class _ : IdentitySink<TResult>
        {
            public _(IObserver<TResult> observer)
                : base(observer)
            {
            }

            public void Run(Case<TValue, TResult> parent)
            {
                IObservable<TResult> result;
                try
                {
                    result = parent.Eval();
                }
                catch (Exception exception)
                {
                    ForwardOnError(exception);

                    return;
                }

                Run(result);
            }
        }
    }
}
