// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive
{
    internal abstract class TailRecursiveSink<TSource> : Sink<TSource>, IObserver<TSource>
    {
        public TailRecursiveSink(IObserver<TSource> observer, IDisposable cancel)
            : base(observer, cancel)
        {
        }

        private bool _isDisposed;
        private SerialDisposable _subscription;
        private AsyncLock _gate;
        private Stack<IEnumerator<IObservable<TSource>>> _stack;
        private Stack<int?> _length;
        protected Action _recurse;

        public IDisposable Run(IEnumerable<IObservable<TSource>> sources)
        {
            _isDisposed = false;
            _subscription = new SerialDisposable();
            _gate = new AsyncLock();
            _stack = new Stack<IEnumerator<IObservable<TSource>>>();
            _length = new Stack<int?>();

            if (!TryGetEnumerator(sources, out var e))
                return Disposable.Empty;

            _stack.Push(e);
            _length.Push(Helpers.GetLength(sources));

            var cancelable = SchedulerDefaults.TailRecursion.Schedule(self =>
            {
                _recurse = self;
                _gate.Wait(MoveNext);
            });

            return StableCompositeDisposable.Create(_subscription, cancelable, Disposable.Create(() => _gate.Wait(Dispose)));
        }

        protected abstract IEnumerable<IObservable<TSource>> Extract(IObservable<TSource> source);

        private void MoveNext()
        {
            var hasNext = false;
            var next = default(IObservable<TSource>);

            do
            {
                if (_stack.Count == 0)
                    break;

                if (_isDisposed)
                    return;

                var e = _stack.Peek();
                var l = _length.Peek();

                var current = default(IObservable<TSource>);
                try
                {
                    hasNext = e.MoveNext();
                    if (hasNext)
                    {
                        current = e.Current;
                    }
                }
                catch (Exception ex)
                {
                    e.Dispose();

                    //
                    // Failure to enumerate the sequence cannot be handled, even by
                    // operators like Catch, because it'd lead to another attempt at
                    // enumerating to find the next observable sequence. Therefore,
                    // we feed those errors directly to the observer.
                    //
                    _observer.OnError(ex);
                    base.Dispose();
                    return;
                }

                if (!hasNext)
                {
                    e.Dispose();

                    _stack.Pop();
                    _length.Pop();
                }
                else
                {
                    var r = l - 1;
                    _length.Pop();
                    _length.Push(r);

                    try
                    {
                        next = Helpers.Unpack(current);
                    }
                    catch (Exception exception)
                    {
                        //
                        // Errors from unpacking may produce side-effects that normally
                        // would occur during a SubscribeSafe operation. Those would feed
                        // back into the observer and be subject to the operator's error
                        // handling behavior. For example, Catch would allow to handle
                        // the error using a handler function.
                        //
                        if (!Fail(exception))
                        {
                            e.Dispose();
                        }

                        return;
                    }

                    //
                    // Tail recursive case; drop the current frame.
                    //
                    if (r == 0)
                    {
                        e.Dispose();

                        _stack.Pop();
                        _length.Pop();
                    }

                    //
                    // Flattening of nested sequences. Prevents stack overflow in observers.
                    //
                    var nextSeq = Extract(next);
                    if (nextSeq != null)
                    {
                        if (!TryGetEnumerator(nextSeq, out var nextEnumerator))
                            return;

                        _stack.Push(nextEnumerator);
                        _length.Push(Helpers.GetLength(nextSeq));

                        hasNext = false;
                    }
                }
            } while (!hasNext);

            if (!hasNext)
            {
                Done();
                return;
            }

            var d = new SingleAssignmentDisposable();
            _subscription.Disposable = d;
            d.Disposable = next.SubscribeSafe(this);
        }

        private new void Dispose()
        {
            while (_stack.Count > 0)
            {
                var e = _stack.Pop();
                _length.Pop();

                e.Dispose();
            }

            _isDisposed = true;
        }

        private bool TryGetEnumerator(IEnumerable<IObservable<TSource>> sources, out IEnumerator<IObservable<TSource>> result)
        {
            try
            {
                result = sources.GetEnumerator();
                return true;
            }
            catch (Exception exception)
            {
                //
                // Failure to enumerate the sequence cannot be handled, even by
                // operators like Catch, because it'd lead to another attempt at
                // enumerating to find the next observable sequence. Therefore,
                // we feed those errors directly to the observer.
                //
                _observer.OnError(exception);
                base.Dispose();

                result = null;
                return false;
            }
        }

        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(TSource value);

        protected virtual void Done()
        {
            _observer.OnCompleted();
            base.Dispose();
        }

        protected virtual bool Fail(Exception error)
        {
            _observer.OnError(error);
            base.Dispose();

            return false;
        }
    }
}
