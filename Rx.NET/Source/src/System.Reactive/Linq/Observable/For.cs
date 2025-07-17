﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class For<TSource, TResult> : Producer<TResult, For<TSource, TResult>._>, IConcatenatable<TResult>
    {
        private readonly IEnumerable<TSource> _source;
        private readonly Func<TSource, IObservable<TResult>> _resultSelector;

        public For(IEnumerable<TSource> source, Func<TSource, IObservable<TResult>> resultSelector)
        {
            _source = source;
            _resultSelector = resultSelector;
        }

        protected override _ CreateSink(IObserver<TResult> observer) => new(observer);

        protected override void Run(_ sink) => sink.Run(GetSources());

        public IEnumerable<IObservable<TResult>> GetSources()
        {
            foreach (var item in _source)
            {
                yield return _resultSelector(item);
            }
        }

        internal sealed class _ : ConcatSink<TResult>
        {
            public _(IObserver<TResult> observer)
                : base(observer)
            {
            }
        }
    }
}
