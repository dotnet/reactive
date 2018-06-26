// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace System.Reactive.Linq.ObservableImpl
{
    /// <summary>
    /// Maps the upstream items into observable sequences, 
    /// runs some or all source observables at once but still concatenates them in order,
    /// buffering later observable items until their turn.
    /// </summary>
    internal static class ConcatManyEager
    {

        /// <summary>
        /// Run all sources at once.
        /// </summary>
        /// <typeparam name="TSource">The upstream element type.</typeparam>
        /// <typeparam name="TResult">The result and inner sequence element type.</typeparam>
        internal sealed class All<TSource, TResult> : Producer<TResult, All<TSource, TResult>.MainObserver>
        {
            readonly IObservable<TSource> _source;

            readonly Func<TSource, IObservable<TResult>> _mapper;

            readonly bool _delayErrors;

            public All(IObservable<TSource> source, Func<TSource, IObservable<TResult>> mapper, bool delayErrors)
            {
                _source = source;
                _mapper = mapper;
                _delayErrors = delayErrors;
            }

            protected override MainObserver CreateSink(IObserver<TResult> observer) => new MainObserver(observer, _mapper, _delayErrors);

            protected override void Run(MainObserver sink) => sink.Run(_source);

            internal sealed class MainObserver : MainObserverBase<TSource, TResult>
            {
                internal MainObserver(IObserver<TResult> observer,
                    Func<TSource, IObservable<TResult>> mapper, bool delayErrors) : base(observer, mapper, delayErrors)
                {
                }

                public override void OnNext(TSource value)
                {
                    if (_done)
                    {
                        return;
                    }

                    var innerSource = default(IObservable<TResult>);

                    try
                    {
                        innerSource = _mapper(value) ?? throw new NullReferenceException("The mapper returned a null IObservable");
                    }
                    catch (Exception ex)
                    {
                        DisposeUpstream();
                        OnError(ex);
                        return;
                    }

                    var innerObserver = new ConcatManyEagerInnerObserver<TResult>(this);
                    _observers.Enqueue(innerObserver);

                    innerObserver.SetResource(innerSource.SubscribeSafe(innerObserver));

                    Drain();
                }

                protected override void DrainLoop()
                {
                    var missed = 1;

                    for (; ; )
                    {
                        if (Volatile.Read(ref _disposed))
                        {
                            _current?.Dispose();
                            _current = null;

                            while (_observers.TryDequeue(out var inner))
                            {
                                inner.Dispose();
                            }
                        }
                        else
                        {
                            var mainDone = Volatile.Read(ref _done);

                            if (mainDone && !_delayErrors)
                            {
                                var ex = Volatile.Read(ref _errors);
                                if (ex != null)
                                {
                                    ForwardOnError(ex);
                                    continue;
                                }
                            }

                            var current = _current;
                            if (current == null)
                            {
                                if (_observers.TryDequeue(out current))
                                {
                                    _current = current;
                                }
                                else
                                {
                                    if (mainDone)
                                    {
                                        var ex = Volatile.Read(ref _errors);
                                        if (ex != null)
                                        {
                                            ForwardOnError(ex);
                                        }
                                        else
                                        {
                                            ForwardOnCompleted();
                                        }

                                        continue;
                                    }
                                }
                            }

                            if (current != null)
                            {
                                var innerDone = current.IsDone();

                                var innerQueue = current.GetQueue();

                                var item = default(TResult);

                                var success = innerQueue != null && innerQueue.TryDequeue(out item);

                                if (innerDone && !success)
                                {
                                    _current = null;
                                    continue;
                                }

                                if (success)
                                {
                                    ForwardOnNext(item);
                                    continue;
                                }
                            }
                        }

                        missed = Interlocked.Add(ref _wip, -missed);
                        if (missed == 0)
                        {
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Run some sources at once.
        /// </summary>
        /// <typeparam name="TSource">The upstream element type.</typeparam>
        /// <typeparam name="TResult">The result and inner sequence element type.</typeparam>
        internal sealed class Some<TSource, TResult> : Producer<TResult, Some<TSource, TResult>.MainObserver>
        {
            readonly IObservable<TSource> _source;

            readonly Func<TSource, IObservable<TResult>> _mapper;

            readonly bool _delayErrors;

            readonly int _maxConcurrency;

            public Some(IObservable<TSource> source, Func<TSource, IObservable<TResult>> mapper, bool delayErrors, int maxConcurrency)
            {
                _source = source;
                _mapper = mapper;
                _delayErrors = delayErrors;
                _maxConcurrency = maxConcurrency;
            }

            protected override MainObserver CreateSink(IObserver<TResult> observer) => new MainObserver(observer, _mapper, _delayErrors, _maxConcurrency);

            protected override void Run(MainObserver sink) => sink.Run(_source);

            internal sealed class MainObserver : MainObserverBase<TSource, TResult>
            {
                /// <summary>
                /// The maximum number of concurrent subscriptions to
                /// inner sources.
                /// </summary>
                readonly int _maxConcurrency;

                /// <summary>
                /// Buffers the upstream items to be mapped into IObservables
                /// as soon as there is a subscription "slot" available,
                /// i.e., when _active &lt; _maxConcurrency
                /// </summary>
                readonly ConcurrentQueue<TSource> _sourceItems;

                /// <summary>
                /// Tracks how many inner observers have been created and
                /// subscribed to an inner source.
                /// </summary>
                int _active;

                /// <summary>
                /// Indicates that no further upstream items should be mapped
                /// to inner sources at all due to no more upstream items
                /// or that the last mapping failed.
                /// </summary>
                bool _noMoreSources;

                internal MainObserver(IObserver<TResult> observer,
                    Func<TSource, IObservable<TResult>> mapper, bool delayErrors, int maxConcurrency) : base(observer, mapper, delayErrors)
                {
                    _maxConcurrency = maxConcurrency;
                    _sourceItems = new ConcurrentQueue<TSource>();
                }

                public override void OnNext(TSource value)
                {
                    _sourceItems.Enqueue(value);
                    Drain();
                }

                protected override void DrainLoop()
                {
                    var missed = 1;

                    for (; ; )
                    {
                        if (Volatile.Read(ref _disposed))
                        {
                            _current?.Dispose();
                            _current = null;

                            while (_observers.TryDequeue(out var inner))
                            {
                                inner.Dispose();
                            }

                            while (_sourceItems.TryDequeue(out var _))
                                ;
                        }
                        else
                        {
                            var mainDone = Volatile.Read(ref _done);

                            if (mainDone && !_delayErrors)
                            {
                                var ex = Volatile.Read(ref _errors);
                                if (ex != null)
                                {
                                    ForwardOnError(ex);
                                    continue;
                                }
                            }

                            if (!_noMoreSources && _active < _maxConcurrency)
                            {
                                if (_sourceItems.TryDequeue(out var item))
                                {
                                    var inner = default(IObservable<TResult>);
                                    try
                                    {
                                        inner = _mapper(item) ?? throw new NullReferenceException("The mapper returned a null IObservable");
                                    }
                                    catch (Exception ex)
                                    {
                                        // If the mapping fails, we stop the upstream and
                                        // pretend the upstream failed
                                        // no further inner sources will be mapped or subscribed to
                                        DisposeUpstream();
                                        _noMoreSources = true;
                                        if (_delayErrors)
                                        {
                                            ExceptionHelper.TryAddException(ref _errors, ex);
                                        }
                                        else
                                        {
                                            ExceptionHelper.TrySetException(ref _errors, ex);
                                        }
                                        Volatile.Write(ref _done, true);
                                        continue;
                                    }

                                    _active++;
                                    var innerObserver = new ConcatManyEagerInnerObserver<TResult>(this);
                                    _observers.Enqueue(innerObserver);

                                    innerObserver.SetResource(inner.SubscribeSafe(innerObserver));
                                    continue;
                                }
                                else if (mainDone)
                                {
                                    _noMoreSources = true;
                                }
                            }

                            var current = _current;
                            if (current == null)
                            {
                                if (_observers.TryDequeue(out current))
                                {
                                    _current = current;
                                }
                                else
                                {
                                    if (mainDone)
                                    {
                                        var ex = Volatile.Read(ref _errors);
                                        if (ex != null)
                                        {
                                            ForwardOnError(ex);
                                        }
                                        else
                                        {
                                            ForwardOnCompleted();
                                        }

                                        continue;
                                    }
                                }
                            }

                            if (current != null)
                            {
                                var innerDone = current.IsDone();

                                var innerQueue = current.GetQueue();

                                var item = default(TResult);

                                var success = innerQueue != null && innerQueue.TryDequeue(out item);

                                if (innerDone && !success)
                                {
                                    _current = null;
                                    _active--;
                                    continue;
                                }

                                if (success)
                                {
                                    ForwardOnNext(item);
                                    continue;
                                }
                            }
                        }

                        missed = Interlocked.Add(ref _wip, -missed);
                        if (missed == 0)
                        {
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Base observer for ConcatManyEager with common functionality to
        /// both all and some concurrency modes.
        /// </summary>
        /// <typeparam name="TSource">The source element type.</typeparam>
        /// <typeparam name="TResult">The result element type.</typeparam>
        internal abstract class MainObserverBase<TSource, TResult> : Sink<TSource, TResult>, IConcatManyEagerSupport<TResult>
        {
            protected readonly Func<TSource, IObservable<TResult>> _mapper;

            protected readonly bool _delayErrors;

            /// <summary>
            /// Holds the subscribed inner observers and gets dequeued when the current
            /// one terminates.
            /// </summary>
            protected readonly ConcurrentQueue<ConcatManyEagerInnerObserver<TResult>> _observers;

            /// <summary>
            /// Holds the single or aggregate exception.
            /// </summary>
            protected Exception _errors;

            /// <summary>
            /// Indicates that the Drain() method should clean up the
            /// queues and other references upon disposing the sequence.
            /// </summary>
            protected bool _disposed;

            /// <summary>
            /// Indicates the main source has finished signaling items.
            /// </summary>
            protected bool _done;

            /// <summary>
            /// Indicates the drain loop is active if non-zero.
            /// </summary>
            protected int _wip;

            /// <summary>
            /// The current inner observer whose elements are relayed to the downstream,
            /// used for establishing a fast-path in InnerNext.
            /// </summary>
            protected ConcatManyEagerInnerObserver<TResult> _current;

            internal MainObserverBase(IObserver<TResult> observer,
                Func<TSource, IObservable<TResult>> mapper, bool delayErrors) : base(observer)
            {
                _mapper = mapper;
                _delayErrors = delayErrors;
                _observers = new ConcurrentQueue<ConcatManyEagerInnerObserver<TResult>>();
            }

            protected override void Dispose(bool disposing)
            {
                Volatile.Write(ref _disposed, true);

                base.Dispose(disposing);

                if (disposing)
                {
                    Drain();
                }
            }

            public override void OnCompleted()
            {
                Volatile.Write(ref _done, true);
                Drain();
            }

            public override void OnError(Exception error)
            {
                if (_delayErrors)
                {
                    ExceptionHelper.TryAddException(ref _errors, error);
                }
                else
                {
                    ExceptionHelper.TrySetException(ref _errors, error);
                }
                Volatile.Write(ref _done, true);
                Drain();
            }

            public void InnerNext(ConcatManyEagerInnerObserver<TResult> sender, TResult item)
            {
                // fast path
                if (Interlocked.CompareExchange(ref _wip, 1, 0) == 0)
                {
                    // the sender is the one allowed to emit
                    if (_current == sender)
                    {
                        // if there is no items queued
                        var q = sender.GetQueue();
                        if (q == null || q.IsEmpty)
                        {
                            // emit directly
                            ForwardOnNext(item);
                        }
                        else
                        {
                            // otherwise queue up
                            q.Enqueue(item);
                            // and drain all
                            DrainLoop();
                            return;
                        }
                    }
                    else
                    {
                        // the sender is not the active one
                        // queue up items
                        var q = sender.GetOrCreateQueue();
                        q.Enqueue(item);
                    }

                    // quit if no further work has been signaled
                    if (Interlocked.Decrement(ref _wip) == 0)
                    {
                        return;
                    }
                }
                else
                {
                    // slow path, queue up items
                    var q = sender.GetOrCreateQueue();
                    q.Enqueue(item);

                    // signal there is more work to be performed
                    if (Interlocked.Increment(ref _wip) != 1)
                    {
                        return;
                    }
                }
                // do the extra work
                DrainLoop();
            }

            public void InnerError(ConcatManyEagerInnerObserver<TResult> sender, Exception error)
            {
                if (_delayErrors)
                {
                    ExceptionHelper.TryAddException(ref _errors, error);
                }
                else
                {
                    ExceptionHelper.TrySetException(ref _errors, error);
                }
                sender.SetDone();
                Drain();
            }

            public void InnerCompleted(ConcatManyEagerInnerObserver<TResult> sender)
            {
                sender.SetDone();
                Drain();
            }

            /// <summary>
            /// Provides a serialized way for emitting items and handling
            /// the operator's state, including termination and
            /// possibly handling new subscriptions.
            /// </summary>
            protected void Drain()
            {
                if (Interlocked.Increment(ref _wip) == 1)
                {
                    DrainLoop();
                }
            }

            protected abstract void DrainLoop();
        }

        /// <summary>
        /// Callback support for the inner observers.
        /// </summary>
        /// <typeparam name="TResult">The output value type.</typeparam>
        internal interface IConcatManyEagerSupport<TResult>
        {
            void InnerNext(ConcatManyEagerInnerObserver<TResult> sender, TResult item);

            void InnerError(ConcatManyEagerInnerObserver<TResult> sender, Exception error);

            void InnerCompleted(ConcatManyEagerInnerObserver<TResult> sender);
        }

        /// <summary>
        /// Observer for the inner sources of the eager concatenation operator that
        /// calls the main observer through the support interface.
        /// </summary>
        /// <typeparam name="TResult">The output value type.</typeparam>
        internal sealed class ConcatManyEagerInnerObserver<TResult> : SafeObserver<TResult>
        {
            readonly IConcatManyEagerSupport<TResult> _parent;

            bool _done;

            ConcurrentQueue<TResult> _queue;

            public ConcatManyEagerInnerObserver(IConcatManyEagerSupport<TResult> parent)
            {
                _parent = parent;
            }

            public override void OnCompleted()
            {
                _parent.InnerCompleted(this);
                Dispose();
            }

            public override void OnError(Exception error)
            {
                _parent.InnerError(this, error);
                Dispose();
            }

            public override void OnNext(TResult value)
            {
                _parent.InnerNext(this, value);
            }

            internal void SetDone()
            {
                Volatile.Write(ref _done, true);
            }

            internal bool IsDone()
            {
                return Volatile.Read(ref _done);
            }

            internal ConcurrentQueue<TResult> GetQueue()
            {
                return Volatile.Read(ref _queue);
            }

            internal ConcurrentQueue<TResult> GetOrCreateQueue()
            {
                var q = GetQueue();
                if (q == null)
                {
                    q = new ConcurrentQueue<TResult>();
                    Volatile.Write(ref _queue, q);
                }
                return q;
            }
        }
    }
}
