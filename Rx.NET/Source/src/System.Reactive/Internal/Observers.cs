// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive
{
    internal sealed class NopObserver<T> : IObserver<T>
    {
        public static readonly IObserver<T> Instance = new NopObserver<T>();

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(T value) { }
    }

    internal sealed class DoneObserver<T> : IObserver<T>
    {
        public static readonly IObserver<T> Completed = new DoneObserver<T>();

        public Exception Exception { get; set; }

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(T value) { }
    }

    internal sealed class DisposedObserver<T> : IObserver<T>
    {
        public static readonly IObserver<T> Instance = new DisposedObserver<T>();

        public void OnCompleted()
        {
            throw new ObjectDisposedException("");
        }

        public void OnError(Exception error)
        {
            throw new ObjectDisposedException("");
        }

        public void OnNext(T value)
        {
            throw new ObjectDisposedException("");
        }
    }

    internal sealed class Observer<T> : IObserver<T>
    {
        private readonly ImmutableList<IObserver<T>> _observers;

        public Observer(ImmutableList<IObserver<T>> observers)
        {
            _observers = observers;
        }

        public void OnCompleted()
        {
            foreach (var observer in _observers.Data)
            {
                observer.OnCompleted();
            }
        }

        public void OnError(Exception error)
        {
            foreach (var observer in _observers.Data)
            {
                observer.OnError(error);
            }
        }

        public void OnNext(T value)
        {
            foreach (var observer in _observers.Data)
            {
                observer.OnNext(value);
            }
        }

        internal IObserver<T> Add(IObserver<T> observer) => new Observer<T>(_observers.Add(observer));

        internal IObserver<T> Remove(IObserver<T> observer)
        {
            var i = Array.IndexOf(_observers.Data, observer);
            if (i < 0)
            {
                return this;
            }

            if (_observers.Data.Length == 2)
            {
                return _observers.Data[1 - i];
            }
            else
            {
                return new Observer<T>(_observers.Remove(observer));
            }
        }
    }
}
