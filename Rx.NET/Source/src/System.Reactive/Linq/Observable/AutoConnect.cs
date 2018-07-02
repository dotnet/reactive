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
        private readonly IConnectableObservable<T> _source;
        private readonly int _minObservers;
        private readonly Action<IDisposable> _onConnect;
        private int _count;

        internal AutoConnect(IConnectableObservable<T> source, int minObservers, Action<IDisposable> onConnect)
        {
            _source = source;
            _minObservers = minObservers;
            _onConnect = onConnect;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            var d = _source.Subscribe(observer);

            if (Volatile.Read(ref _count) < _minObservers)
            {
                if (Interlocked.Increment(ref _count) == _minObservers)
                {
                    var c = _source.Connect();
                    _onConnect?.Invoke(c);
                }
            }
            return d;
        }
    }
}
