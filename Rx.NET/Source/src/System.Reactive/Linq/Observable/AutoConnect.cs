// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Subjects;
using System.Threading;

namespace System.Reactive.Linq.ObservableImpl
{
    /// <summary>
    /// Automatically connect the upstream IConnectableObservable once the
    /// specified number of IObservers have subscribed to this IObservable.
    /// </summary>
    /// <typeparam name="T">The upstream value type.</typeparam>
    internal sealed class AutoConnect<T> : IObservable<T>
    {
        readonly IConnectableObservable<T> source;

        readonly int minObservers;

        readonly Action<IDisposable> onConnect;

        int count;

        internal AutoConnect(IConnectableObservable<T> source, int minObservers, Action<IDisposable> onConnect)
        {
            this.source = source;
            this.minObservers = minObservers;
            this.onConnect = onConnect;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            var d = source.Subscribe(observer);

            if (Volatile.Read(ref count) < minObservers)
            {
                if (Interlocked.Increment(ref count) == minObservers)
                {
                    var c = source.Connect();
                    onConnect?.Invoke(c);
                }
            }
            return d;
        }
    }
}
