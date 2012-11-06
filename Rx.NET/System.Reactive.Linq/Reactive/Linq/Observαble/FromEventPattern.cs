// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reflection;
using System.Threading;

//
// BREAKING CHANGE v2 > v1.x - FromEvent[Pattern] now has an implicit SubscribeOn and Publish operation.
//
// See FromEvent.cs for more information.
//
namespace System.Reactive.Linq.Observαble
{
    class FromEventPattern
    {
        public class τ<TDelegate, TEventArgs> : ClassicEventProducer<TDelegate, EventPattern<TEventArgs>>
#if !NO_EVENTARGS_CONSTRAINT
            where TEventArgs : EventArgs
#endif
        {
            private readonly Func<EventHandler<TEventArgs>, TDelegate> _conversion;

            public τ(Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler)
                : base(addHandler, removeHandler, scheduler)
            {
            }

            public τ(Func<EventHandler<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler)
                : base(addHandler, removeHandler, scheduler)
            {
                _conversion = conversion;
            }

            protected override TDelegate GetHandler(Action<EventPattern<TEventArgs>> onNext)
            {
                var handler = default(TDelegate);

                if (_conversion == null)
                {
                    Action<object, TEventArgs> h = (sender, eventArgs) => onNext(new EventPattern<TEventArgs>(sender, eventArgs));
                    handler = ReflectionUtils.CreateDelegate<TDelegate>(h, typeof(Action<object, TEventArgs>).GetMethod("Invoke"));
                }
                else
                {
                    handler = _conversion((sender, eventArgs) => onNext(new EventPattern<TEventArgs>(sender, eventArgs)));
                }

                return handler;
            }
        }

        public class τ<TDelegate, TSender, TEventArgs> : ClassicEventProducer<TDelegate, EventPattern<TSender, TEventArgs>>
#if !NO_EVENTARGS_CONSTRAINT
            where TEventArgs : EventArgs
#endif
        {
            public τ(Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler)
                : base(addHandler, removeHandler, scheduler)
            {
            }

            protected override TDelegate GetHandler(Action<EventPattern<TSender, TEventArgs>> onNext)
            {
                Action<TSender, TEventArgs> h = (sender, eventArgs) => onNext(new EventPattern<TSender, TEventArgs>(sender, eventArgs));
                return ReflectionUtils.CreateDelegate<TDelegate>(h, typeof(Action<TSender, TEventArgs>).GetMethod("Invoke"));
            }
        }

        public class ρ<TSender, TEventArgs, TResult> : EventProducer<Delegate, TResult>
        {
            private readonly object _target;
            private readonly Type _delegateType;
            private readonly MethodInfo _addMethod;
            private readonly MethodInfo _removeMethod;
            private readonly Func<TSender, TEventArgs, TResult> _getResult;
#if HAS_WINRT
            private readonly bool _isWinRT;
#endif

            public ρ(object target, Type delegateType, MethodInfo addMethod, MethodInfo removeMethod, Func<TSender, TEventArgs, TResult> getResult, bool isWinRT, IScheduler scheduler)
                : base(scheduler)
            {
#if HAS_WINRT
                _isWinRT = isWinRT;
#else
                System.Diagnostics.Debug.Assert(!isWinRT);
#endif
                _target = target;
                _delegateType = delegateType;
                _addMethod = addMethod;
                _removeMethod = removeMethod;
                _getResult = getResult;
            }

            protected override Delegate GetHandler(Action<TResult> onNext)
            {
                Action<TSender, TEventArgs> h = (sender, eventArgs) => onNext(_getResult(sender, eventArgs));
                return ReflectionUtils.CreateDelegate(_delegateType, h, typeof(Action<TSender, TEventArgs>).GetMethod("Invoke"));
            }

            protected override IDisposable AddHandler(Delegate handler)
            {
                var removeHandler = default(Action);

                try
                {
#if HAS_WINRT
                    if (_isWinRT)
                    {
                        var token = _addMethod.Invoke(_target, new object[] { handler });
                        removeHandler = () => _removeMethod.Invoke(_target, new object[] { token });
                    }
                    else
#endif
                    {
                        _addMethod.Invoke(_target, new object[] { handler });
                        removeHandler = () => _removeMethod.Invoke(_target, new object[] { handler });
                    }
                }
                catch (TargetInvocationException tie)
                {
                    throw tie.InnerException;
                }

                return Disposable.Create(() =>
                {
                    try
                    {
                        removeHandler();
                    }
                    catch (TargetInvocationException tie)
                    {
                        throw tie.InnerException;
                    }
                });
            }
        }
    }
}
#endif