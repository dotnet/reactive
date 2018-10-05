// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive
{
    internal abstract class IdentitySink<T> : Sink<T, T>
    {
        protected IdentitySink(IObserver<T> observer) : base(observer)
        {
        }

        public override void OnNext(T value)
        {
            ForwardOnNext(value);
        }
    }
}
