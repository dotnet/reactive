// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace System.Reactive
{
    class EventSource<T> : IEventSource<T>
    {
        private readonly IObservable<T> _source;
        private readonly Dictionary<Delegate, Stack<IDisposable>> _subscriptions;
        private readonly Action<Action<T>, /*object,*/ T> _invokeHandler;

        public EventSource(IObservable<T> source, Action<Action<T>, /*object,*/ T> invokeHandler)
        {
            _source = source;
            _invokeHandler = invokeHandler;
            _subscriptions = new Dictionary<Delegate, Stack<IDisposable>>();
        }

        public event Action<T> OnNext
        {
            add
            {
                var gate = new object();
                var isAdded = false;
                var isDone = false;

                var remove = new Action(() =>
                {
                    lock (gate)
                    {
                        if (isAdded)
                            Remove(value);
                        else
                            isDone = true;
                    }
                });

                //
                // [OK] Use of unsafe Subscribe: non-pretentious wrapper of an observable in an event; exceptions can occur during +=.
                //
                var d = _source.Subscribe/*Unsafe*/(
                    x => _invokeHandler(value, /*this,*/ x),
                    ex => { remove(); ex.Throw(); },
                    () => remove()
                );

                lock (gate)
                {
                    if (!isDone)
                    {
                        Add(value, d);
                        isAdded = true;
                    }
                }
            }

            remove
            {
                Remove(value);
            }
        }

        private void Add(Delegate handler, IDisposable disposable)
        {
            lock (_subscriptions)
            {
                var l = new Stack<IDisposable>();
                if (!_subscriptions.TryGetValue(handler, out l))
                    _subscriptions[handler] = l = new Stack<IDisposable>();

                l.Push(disposable);
            }
        }

        private void Remove(Delegate handler)
        {
            var d = default(IDisposable);

            lock (_subscriptions)
            {
                var l = new Stack<IDisposable>();
                if (_subscriptions.TryGetValue(handler, out l))
                {
                    d = l.Pop();
                    if (l.Count == 0)
                        _subscriptions.Remove(handler);
                }
            }

            if (d != null)
                d.Dispose();
        }
    }
}
