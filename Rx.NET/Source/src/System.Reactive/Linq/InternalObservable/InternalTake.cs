// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Text;

namespace System.Reactive.Linq.InternalObservable
{
    internal sealed class InternalTake<T> : InternalIntermediateObservable<T, T>
    {
        readonly int count;

        public InternalTake(IObservable<T> source, int count) : base(source)
        {
            this.count = count;
        }

        public override void Subscribe(IInternalObserver<T> observer)
        {
            source.Subscribe(new TakeObserver(observer, count));
        }

        internal sealed class TakeObserver : IInternalObserver<T>
        {
            readonly IInternalObserver<T> downstream;

            int remaining;

            IDisposable upstream;

            internal TakeObserver(IInternalObserver<T> downstream, int count)
            {
                this.downstream = downstream;
                this.remaining = count;
            }

            public void Dispose()
            {
                upstream.Dispose();
            }

            public void OnCompleted()
            {
                if (remaining != 0)
                {
                    downstream.OnCompleted();
                }
            }

            public void OnError(Exception error)
            {
                if (remaining != 0)
                {
                    downstream.OnError(error);
                }
            }

            public void OnNext(T value)
            {
                var r = remaining;
                if (r != 0)
                {
                    remaining = --r;
                    downstream.OnNext(value);
                    if (r == 0)
                    {
                        downstream.OnCompleted();
                    }
                }
            }

            public void OnSubscribe(IDisposable upstream)
            {
                this.upstream = upstream;
                downstream.OnSubscribe(this);

                if (remaining == 0)
                {
                    upstream.Dispose();
                    downstream.OnCompleted();
                }
            }
        }
    }
}
