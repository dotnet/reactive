// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;

namespace System.Reactive.Linq.InternalObservable
{
    internal sealed class InternalRange : InternalBaseObservable<int>
    {
        readonly int start;

        readonly int end;

        internal InternalRange(int start, int count)
        {
            this.start = start;
            this.end = start + count;
        }

        public override void Subscribe(IInternalObserver<int> observer)
        {
            var d = new BooleanDisposable();

            observer.OnSubscribe(d);

            var end = this.end;

            for (int i = start; i != end; i++)
            {
                if (d.IsDisposed)
                {
                    return;
                }

                observer.OnNext(i);
            }

            if  (d.IsDisposed)
            {
                return;
            }
            observer.OnCompleted();
        }
    }
}
