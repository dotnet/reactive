// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reflection;
using System.Threading;

//
// BREAKING CHANGE v2 > v1.x - FromEvent[Pattern] now has an implicit SubscribeOn and Publish operation.
//
// See FromEvent.cs for more information.
//
namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class FromEventPattern
    {
        public sealed class Impl<TDelegate, TEventArgs> : ClassicEventProducer<TDelegate, EventPattern<TEventArgs>>
        {
            private readonly Func<EventHandler<TEventArgs>, TDelegate>? _conversion;

            public Impl(Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler)
                : base(addHandler, removeHandler, scheduler)
            {
            }

            public Impl(Func<EventHandler<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler)
                : base(addHandler, removeHandler, scheduler)
            {
                _conversion = conversion;
            }

            protected override TDelegate GetHandler(Action<EventPattern<TEventArgs>> onNext)
            {
                TDelegate handler;

                if (_conversion == null)
                {
                    Action<object, TEventArgs> h = (sender, eventArgs) => onNext(new EventPattern<TEventArgs>(sender, eventArgs));
                    handler = ReflectionUtils.CreateDelegate<TDelegate>(h, typeof(Action<object, TEventArgs>).GetMethod(nameof(Action<object, TEventArgs>.Invoke))!);
                }
                else
                {
                    handler = _conversion((sender, eventArgs) => onNext(new EventPattern<TEventArgs>(sender, eventArgs)));
                }

                return handler;
            }
        }

        public sealed class Impl<TDelegate, TSender, TEventArgs> : ClassicEventProducer<TDelegate, EventPattern<TSender, TEventArgs>>
        {
            public Impl(Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler)
                : base(addHandler, removeHandler, scheduler)
            {
            }

            protected override TDelegate GetHandler(Action<EventPattern<TSender, TEventArgs>> onNext)
            {
                Action<TSender, TEventArgs> h = (sender, eventArgs) => onNext(new EventPattern<TSender, TEventArgs>(sender, eventArgs));
                return ReflectionUtils.CreateDelegate<TDelegate>(h, typeof(Action<TSender, TEventArgs>).GetMethod(nameof(Action<TSender, TEventArgs>.Invoke))!);
            }
        }

        public sealed class Handler<TSender, TEventArgs, TResult> : EventProducer<Delegate, TResult>
        {
            private readonly object? _target;
            private readonly Type _delegateType;
            private readonly MethodInfo _addMethod;
            private readonly MethodInfo _removeMethod;
            private readonly Func<TSender, TEventArgs, TResult> _getResult;
            private readonly bool _isWinRT;

            public Handler(object? target, Type delegateType, MethodInfo addMethod, MethodInfo removeMethod, Func<TSender, TEventArgs, TResult> getResult, bool isWinRT, IScheduler scheduler)
                : base(scheduler)
            {
                _isWinRT = isWinRT;
                _target = target;
                _delegateType = delegateType;
                _addMethod = addMethod;
                _removeMethod = removeMethod;
                _getResult = getResult;
            }

            protected override Delegate GetHandler(Action<TResult> onNext)
            {
                Action<TSender, TEventArgs> h = (sender, eventArgs) => onNext(_getResult(sender, eventArgs));
                return ReflectionUtils.CreateDelegate(_delegateType, h, typeof(Action<TSender, TEventArgs>).GetMethod(nameof(Action<TSender, TEventArgs>.Invoke))!);
            }

            protected override IDisposable AddHandler(Delegate handler)
            {
                Action removeHandler;

                try
                {
                    if (_isWinRT)
                    {
                        removeHandler = AddHandlerCoreWinRT(handler);
                    }
                    else
                    {
                        removeHandler = AddHandlerCore(handler);
                    }
                }
                catch (TargetInvocationException tie) when (tie.InnerException != null)
                {
                    throw tie.InnerException;
                }

                return new RemoveHandlerDisposable(removeHandler);
            }

            private sealed class RemoveHandlerDisposable : IDisposable
            {
                private Action? _removeHandler;

                public RemoveHandlerDisposable(Action removeHandler)
                {
                    Volatile.Write(ref _removeHandler, removeHandler);
                }

                public void Dispose()
                {
                    try
                    {
                        Interlocked.Exchange(ref _removeHandler, null)?.Invoke();
                    }
                    catch (TargetInvocationException tie) when (tie.InnerException != null)
                    {
                        throw tie.InnerException;
                    }
                }
            }

            private Action AddHandlerCore(Delegate handler)
            {
                _addMethod.Invoke(_target, [handler]);
                return () => _removeMethod.Invoke(_target, [handler]);
            }

            private Action AddHandlerCoreWinRT(Delegate handler)
            {
                var token = _addMethod.Invoke(_target, [handler]);
                return () => _removeMethod.Invoke(_target, [token]);
            }
        }
    }
}
