// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading;

namespace System.Reactive
{
    /// <summary>
    /// Base class for implementation of query operators, providing a lightweight sink that can be disposed to mute the outgoing observer.
    /// </summary>
    /// <typeparam name="TSource">Type of the resulting sequence's elements.</typeparam>
    /// <remarks>Implementations of sinks are responsible to enforce the message grammar on the associated observer. Upon sending a terminal message, a pairing Dispose call should be made to trigger cancellation of related resources and to mute the outgoing observer.</remarks>
    internal abstract class Sink<TSource> : IDisposable
    {
        protected internal volatile IObserver<TSource> _observer;
        private IDisposable _cancel;

        public Sink(IObserver<TSource> observer, IDisposable cancel)
        {
            _observer = observer;
            _cancel = cancel;
        }

        public virtual void Dispose()
        {
            _observer = NopObserver<TSource>.Instance;

            Interlocked.Exchange(ref _cancel, null)?.Dispose();
        }

        public IObserver<TSource> GetForwarder() => new _(this);

        private sealed class _ : IObserver<TSource>
        {
            private readonly Sink<TSource> _forward;

            public _(Sink<TSource> forward)
            {
                _forward = forward;
            }

            public void OnNext(TSource value)
            {
                _forward._observer.OnNext(value);
            }

            public void OnError(Exception error)
            {
                _forward._observer.OnError(error);
                _forward.Dispose();
            }

            public void OnCompleted()
            {
                _forward._observer.OnCompleted();
                _forward.Dispose();
            }
        }
    }
}
