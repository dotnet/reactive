﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class ElementAt<TSource> : Producer<TSource, ElementAt<TSource>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly int _index;

        public ElementAt(IObservable<TSource> source, int index)
        {
            _source = source;
            _index = index;
        }

        protected override _ CreateSink(IObserver<TSource> observer) => new(_index, observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : IdentitySink<TSource>
        {
            private int _i;

            public _(int index, IObserver<TSource> observer)
                : base(observer)
            {
                _i = index;
            }

            public override void OnNext(TSource value)
            {
                if (_i == 0)
                {
                    ForwardOnNext(value);
                    ForwardOnCompleted();
                }

                _i--;
            }

            public override void OnCompleted()
            {
                if (_i >= 0)
                {
                    try
                    {
                        throw new ArgumentOutOfRangeException("index");
                    }
                    catch (Exception e)
                    {
                        ForwardOnError(e);
                    }
                }
            }
        }
    }
}
