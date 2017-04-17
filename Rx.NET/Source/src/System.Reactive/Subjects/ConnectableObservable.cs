// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace System.Reactive.Subjects
{
    /// <summary>
    /// Represents an observable wrapper that can be connected and disconnected from its underlying observable sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TResult">The type of the elements in the resulting sequence, after transformation through the subject.</typeparam>
    internal class ConnectableObservable<TSource, TResult> : IConnectableObservable<TResult>
    {
        private readonly ISubject<TSource, TResult> _subject;
        private readonly IObservable<TSource> _source;
        private readonly object _gate;

        private Connection _connection;

        /// <summary>
        /// Creates an observable that can be connected and disconnected from its source.
        /// </summary>
        /// <param name="source">Underlying observable source sequence that can be connected and disconnected from the wrapper.</param>
        /// <param name="subject">Subject exposed by the connectable observable, receiving data from the underlying source sequence upon connection.</param>
        public ConnectableObservable(IObservable<TSource> source, ISubject<TSource, TResult> subject)
        {
            _subject = subject;
            _source = source.AsObservable(); // This gets us auto-detach behavior; otherwise, we'd have to roll our own, including trampoline installation.
            _gate = new object();
        }

        /// <summary>
        /// Connects the observable wrapper to its source. All subscribed observers will receive values from the underlying observable sequence as long as the connection is established.
        /// </summary>
        /// <returns>Disposable object used to disconnect the observable wrapper from its source, causing subscribed observer to stop receiving values from the underlying observable sequence.</returns>
        public IDisposable Connect()
        {
            lock (_gate)
            {
                if (_connection == null)
                {
                    var subscription = _source.SubscribeSafe(_subject);
                    _connection = new Connection(this, subscription);
                }

                return _connection;
            }
        }

        private sealed class Connection : IDisposable
        {
            private readonly ConnectableObservable<TSource, TResult> _parent;
            private IDisposable _subscription;

            public Connection(ConnectableObservable<TSource, TResult> parent, IDisposable subscription)
            {
                _parent = parent;
                _subscription = subscription;
            }

            public void Dispose()
            {
                lock (_parent._gate)
                {
                    if (_subscription != null)
                    {
                        _subscription.Dispose();
                        _subscription = null;

                        _parent._connection = null;
                    }
                }
            }
        }

        /// <summary>
        /// Subscribes an observer to the observable sequence. No values from the underlying observable source will be received unless a connection was established through the Connect method.
        /// </summary>
        /// <param name="observer">Observer that will receive values from the underlying observable source when the current ConnectableObservable instance is connected through a call to Connect.</param>
        /// <returns>Disposable used to unsubscribe from the observable sequence.</returns>
        public IDisposable Subscribe(IObserver<TResult> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return _subject.SubscribeSafe(observer);
        }
    }
}
