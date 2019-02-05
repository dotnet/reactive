// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class All<TSource> : Pipe<TSource, bool>
    {
        private readonly Func<TSource, bool> _predicate;

        public All(IObservable<TSource> source, Func<TSource, bool> predicate) : base(source)
        {
            _predicate = predicate;
        }

        protected override Pipe<TSource, bool> Clone() => new All<TSource>(_source, _predicate);

        public override void OnNext(TSource value)
        {
            var res = false;
            try
            {
                res = _predicate(value);
            }
            catch (Exception ex)
            {
                ForwardOnError(ex);
                return;
            }

            if (!res)
            {
                ForwardOnNext(false);
                ForwardOnCompleted();
            }
        }

        public override void OnCompleted()
        {
            ForwardOnNext(true);
            ForwardOnCompleted();
        }
    }
}
