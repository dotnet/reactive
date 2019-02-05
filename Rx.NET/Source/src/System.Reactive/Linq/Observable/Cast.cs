// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Cast<TSource, TResult> : Pipe<TSource, TResult> /* Could optimize further by deriving from Select<TResult> and providing Combine<TResult2>. We're not doing this (yet) for debuggability. */
    {
        public Cast(IObservable<TSource> source) : base(source)
        {
        }

        protected override Pipe<TSource, TResult> Clone() => new Cast<TSource, TResult>(_source);
        
        public override void OnNext(TSource value)
        {
            var result = default(TResult);
            try
            {
                result = (TResult)(object)value;
            }
            catch (Exception exception)
            {
                ForwardOnError(exception);
                return;
            }

            ForwardOnNext(result);
        }
    }
}
