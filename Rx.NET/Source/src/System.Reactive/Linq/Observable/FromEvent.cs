// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

//
// BREAKING CHANGE v2 > v1.x - FromEvent[Pattern] now has an implicit SubscribeOn and Publish operation.
//
// The free-threaded nature of Rx is key to the performance characteristics of the event processing
// pipeline. However, in places where we bridge with the external world, this sometimes has negative
// effects due to thread-affine operations involved. The FromEvent[Pattern] bridges are one such
// place where we reach out to add and remove operations on events.
//
// Consider the following piece of code, assuming Rx v1.x usage:
//
//   var txt = Observable.FromEventPattern(txtInput, "TextChanged");
//   var res = from term in txt
//             from word in svc.Lookup(term).TakeUntil(txt)
//             select word;
//
// This code is flawed for various reasons. Seasoned Rx developers will immediately suggest usage of
// the Publish operator to share the side-effects of subscribing to the txt sequence, resulting in
// only one subscription to the event:
//
//   var txt = Observable.FromEventPattern(txtInput, "TextChanged");
//   var res = txt.Publish(txt_ => from term in txt_
//                                 from word in svc.Lookup(term).TakeUntil(txt_)
//                                 select word);
//
// Customers are typically confused as to why FromEvent[Pattern] causes multiple handlers to be added
// to the underlying event. This is in contrast with other From* bridges which involve the use of a
// subject (e.g. FromAsyncPattern, FromAsync, and ToObservable on Task<T>).
//
// But there are more issues with the code fragment above. Upon completion of the svc.Lookup(term)
// sequence, TakeUntil will unsubscribe from both sequences, causing the unsubscription to happen in
// the context of the source's OnCompleted, which may be the thread pool. Some thread-affine events
// don't quite like this. In UI frameworks like WPF and Silverlight, this turns out to be not much of
// a problem typically, but it's merely an accident things work out. From an e-mail conversion with
// the WPF/SL/Jupiter experts:
//
//   "Unfortunately, as I expected, it’s confusing, and implementation details are showing through.
//    The bottom line is that event add/remove should always be done on the right thread.
//    
//    Where events are implemented with compiler-generated code, i.e. MultiCastDelegate, the add/remove
//    will be thread safe/agile.  Where events are implemented in custom code, across Wpf/SL/WP/Jupiter,
//    the add/remove are expected to happen on the Dispatcher thread.
//    
//    Jupiter actually has the consistent story here, where all the event add/remove implementations do
//    the thread check.  It should still be a “wrong thread” error, though, not an AV.
//    
//    In SL there’s a mix of core events (which do the thread check) and framework events (which use
//    compiler-generated event implementations).  So you get an exception if you unhook Button.Loaded
//    from off thread, but you don’t get an exception if you unhook Button.Click.
//    
//    In WPF there’s a similar mix (some events are compiler-generated and some use the EventHandlerStore).
//    But I don’t see any thread safety or thread check in the EventHandlerStore.  So while it works, IIUC,
//    it should have race conditions and corruptions."
//
// Starting with "Jupiter" (Windows XAML aka "Metro"), checks are added to ensure the add and remove
// operations for UI events are called from the UI thread. As a result, the dictionary suggest sample
// code shown above starts to fail. A possible fix is to use SubscribeOnDispatcher:
//
//   var txt = Observable.FromEventPattern(txtInput, "TextChanged").SubscribeOnDispatcher();
//   var res = from term in txt
//             from word in svc.Lookup(term).TakeUntil(txt)
//             select word;
//
// This fix has two problems:
//
// 1. Customers often don't quite understand the difference between ObserveOn and SubscribeOn. In fact,
//    we've given guidance that use of the latter is typically indicative of a misunderstanding, and
//    is used rarely. Also, the fragment above would likely be extended with some UI binding code where
//    one needs to use ObserveOnDispatcher, so the combination of both becomes even more confusing.
//
// 2. There's a subtle race condition now. Upon receiving a new term from the txt sequence, SelectMany's
//    invocation of the result selector involves TakeUntil subscribing to txt again. However, the use
//    of SubscribeOnDispatcher means the subscription is now happening asynchronously, leaving a time
//    gap between returning from Subscribe and doing the += on the underlying event:
//
//                    (Subscription of TakeUntil to txt)
//                                     |
//                                     v
//        txt            --------------------------------------------------------------
//                                     |
//                                     +-----...----+  (SubscribeOnDispatcher's post of Subscribe)
//                                                  |
//        TextChanged    ------"re"---------"rea"-------------"reac"-----"react"----...
//                                                  ^
//                                                  |
//                                    (where += on the event happens)
//
//    While this problem is rare and sometimes gets mitigated by accident because code is posting back
//    to e.g. the UI message loop, it's extremely hard to debug when things go wrong.
//
// In order to fix this behavior such that code has the expected behavior, we do two things in Rx v2.0:
//
// - To solve the cross-thread add/remove handler operations and make them single-thread affine, we
//   now do an implicit SubscribeOn with the SynchronizationContext.Current retrieved eagerly upon
//   calling FromEvent[Pattern]. This goes hand-in-hand with a recommendation:
//
//      "Always call FromEvent[Pattern] in a place where you'd normally write += and -= operations
//       yourself. Don't inline the creation of a FromEvent[Pattern] object inside a query."
//
//   This recommendation helps to keep code clean (bridging operations are moved outside queries) and
//   ensures the captured SynchronizationContext is the least surprising one. E.g in the sample code
//   above, the whole query likely lives in a button_Click handler or so.
//
// - To solve the time gap issue, we now add implicit Publish behavior with ref-counted behavior. In
//   other words, the new FromEvent[Pattern] is pretty much the same as:
//
//          Observable_v2.FromEvent[Pattern](<args>)
//      ==
//          Observable_v1.FromEvent[Pattern](<args>).SubscribeOn(SynchronizationContext.Current)
//                                                  .Publish()
//                                                  .RefCount()
//
// Overloads to FromEvent[Pattern] allow to specify the scheduler used for the SubscribeOn operation
// that's taking place internally. When omitted, a SynchronizationContextScheduler will be supplied
// if a current SynchronizationContext is found. If no current SynchronizationContext is found, the
// default scheduler is the immediate scheduler, falling back to the free-threaded behavior we had
// before in v1.x. (See GetSchedulerForCurrentContext in QueryLanguage.Events.cs).
//
// Notice a time gap can still occur at the point of the first subscription to the event sequence,
// or when the ref count fell back to zero. In cases of nested uses of the sequence (such as in the
// running example here), this is fine because the top-level subscription is kept alive for the whole
// duration. In other cases, there's already a race condition between the underlying event and the
// observable wrapper (assuming events are hot). For cold events that have side-effects upon add and
// remove handler operations, use of Observable.Create is recommended. This should be rather rare,
// as most events follow the typical MulticastDelegate implementation pattern:
//
//    public event EventHandler<BarEventArgs> Bar;
//
//    protected void OnBar(int value)
//    {
//        var bar = Bar;
//        if (bar != null)
//            bar(this, new BarEventArgs(value));
//    }
//
// In here, there's already a race between the user hooking up an event handler through the += add
// operation and the event producer (possibly on a different thread) calling OnBar. It's also worth
// pointing out that this race condition is migitated by a check in SynchronizationContextScheduler
// causing synchronous execution in case the caller is already on the target SynchronizationContext.
// This situation is common when using FromEvent[Pattern] immediately after declaring it, e.g. in
// the context of a UI event handler.
//
// Finally, notice we can't simply connect the event to a Subject<T> upon a FromEvent[Pattern] call,
// because this would make it impossible to get rid of this one event handler (unless we expose some
// other means of resource maintenance, e.g. by making the returned object implement IDisposable).
// Also, this would cause the event producer to see the event's delegate in a non-null state all the
// time, causing event argument objects to be newed up, possibly sending those into a zero-observer
// subject (which is opaque to the event producer). Not to mention that the subject would always be
// rooted by the target event (even when the FromEvent[Pattern] observable wrapper is unreachable).
//
namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class FromEvent<TDelegate, TEventArgs> : ClassicEventProducer<TDelegate, TEventArgs>
    {
        private readonly Func<Action<TEventArgs>, TDelegate> _conversion;

        public FromEvent(Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler)
            : base(addHandler, removeHandler, scheduler)
        {
        }

        public FromEvent(Func<Action<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler)
            : base(addHandler, removeHandler, scheduler)
        {
            _conversion = conversion;
        }

        protected override TDelegate GetHandler(Action<TEventArgs> onNext)
        {
            var handler = default(TDelegate);

            if (_conversion == null)
            {
                handler = ReflectionUtils.CreateDelegate<TDelegate>(onNext, typeof(Action<TEventArgs>).GetMethod(nameof(Action<TEventArgs>.Invoke)));
            }
            else
            {
                handler = _conversion(onNext);
            }

            return handler;
        }
    }

    internal abstract class EventProducer<TDelegate, TArgs> : BasicProducer<TArgs>
    {
        private readonly IScheduler _scheduler;
        private readonly object _gate;

        public EventProducer(IScheduler scheduler)
        {
            _scheduler = scheduler;
            _gate = new object();
        }

        protected abstract TDelegate GetHandler(Action<TArgs> onNext);
        protected abstract IDisposable AddHandler(TDelegate handler);

        private Session _session;

        protected override IDisposable Run(IObserver<TArgs> observer)
        {
            var connection = default(IDisposable);

            lock (_gate)
            {
                //
                // A session object holds on to a single handler to the underlying event, feeding
                // into a subject. It also ref counts the number of connections to the subject.
                //
                // When the ref count goes back to zero, the event handler is unregistered, and
                // the session will reach out to reset the _session field to null under the _gate
                // lock. Future subscriptions will cause a new session to be created.
                //
                if (_session == null)
                    _session = new Session(this);

                connection = _session.Connect(observer);
            }

            return connection;
        }

        private sealed class Session
        {
            private readonly EventProducer<TDelegate, TArgs> _parent;
            private readonly Subject<TArgs> _subject;

            private SingleAssignmentDisposable _removeHandler;
            private int _count;

            public Session(EventProducer<TDelegate, TArgs> parent)
            {
                _parent = parent;
                _subject = new Subject<TArgs>();
            }

            public IDisposable Connect(IObserver<TArgs> observer)
            {
                /*
                 * CALLERS - Ensure this is called under the lock!
                 * 
                lock (_parent._gate) */
                {
                    //
                    // We connect the given observer to the subject first, before performing any kind
                    // of initialization which will register an event handler. This is done to ensure
                    // we don't have a time gap between adding the handler and connecting the user's
                    // subject, e.g. when the ImmediateScheduler is used.
                    //
                    // [OK] Use of unsafe Subscribe: called on a known subject implementation.
                    //
                    var connection = _subject.Subscribe/*Unsafe*/(observer);

                    if (++_count == 1)
                    {
                        try
                        {
                            Initialize();
                        }
                        catch (Exception exception)
                        {
                            --_count;
                            connection.Dispose();

                            observer.OnError(exception);
                            return Disposable.Empty;
                        }
                    }

                    return Disposable.Create(() =>
                    {
                        connection.Dispose();

                        lock (_parent._gate)
                        {
                            if (--_count == 0)
                            {
                                _parent._scheduler.Schedule(_removeHandler.Dispose);
                                _parent._session = null;
                            }
                        }
                    });
                }
            }

            private void Initialize()
            {
                /*
                 * CALLERS - Ensure this is called under the lock!
                 * 
                lock (_parent._gate) */
                {
                    //
                    // When the ref count goes to zero, no-one should be able to perform operations on
                    // the session object anymore, because it gets nulled out.
                    //
                    Debug.Assert(_removeHandler == null);
                    _removeHandler = new SingleAssignmentDisposable();

                    //
                    // Conversion code is supposed to be a pure function and shouldn't be run on the
                    // scheduler, but the add handler call should. Notice the scheduler can be the
                    // ImmediateScheduler, causing synchronous invocation. This is the default when
                    // no SynchronizationContext is found (see QueryLanguage.Events.cs and search for
                    // the GetSchedulerForCurrentContext method).
                    //
                    var onNext = _parent.GetHandler(_subject.OnNext);
                    _parent._scheduler.Schedule(onNext, AddHandler);
                }
            }

            private IDisposable AddHandler(IScheduler self, TDelegate onNext)
            {
                var removeHandler = default(IDisposable);
                try
                {
                    removeHandler = _parent.AddHandler(onNext);
                }
                catch (Exception exception)
                {
                    _subject.OnError(exception);
                    return Disposable.Empty;
                }

                //
                // We don't propagate the exception to the OnError channel upon Dispose. This is
                // not possible at this stage, because we've already auto-detached in the base
                // class Producer implementation. Even if we would switch the OnError and auto-
                // detach calls, it wouldn't work because the remove handler logic is scheduled
                // on the given scheduler, causing asynchrony. We can't block waiting for the
                // remove handler to run on the scheduler.
                //
                _removeHandler.Disposable = removeHandler;

                return Disposable.Empty;
            }
        }
    }

    internal abstract class ClassicEventProducer<TDelegate, TArgs> : EventProducer<TDelegate, TArgs>
    {
        private readonly Action<TDelegate> _addHandler;
        private readonly Action<TDelegate> _removeHandler;

        public ClassicEventProducer(Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler)
            : base(scheduler)
        {
            _addHandler = addHandler;
            _removeHandler = removeHandler;
        }

        protected override IDisposable AddHandler(TDelegate handler)
        {
            _addHandler(handler);
            return Disposable.Create(() => _removeHandler(handler));
        }
    }
}
