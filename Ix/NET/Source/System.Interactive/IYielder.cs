// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#if HAS_AWAIT
namespace System.Linq
{
    public interface IYielder<in T>
    {
        IAwaitable Return(T value);
        IAwaitable Break();
    }

    class Yielder<T> : IYielder<T>, IAwaitable, IAwaiter, ICriticalNotifyCompletion
    {
        private readonly Action<Yielder<T>> _create;
        private bool _running;
        private bool _hasValue;
        private T _value;
        private bool _stopped;
        private Action _continuation;

        public Yielder(Action<Yielder<T>> create)
        {
            _create = create;
        }

        public IAwaitable Return(T value)
        {
            _hasValue = true;
            _value = value;
            return this;
        }

        public IAwaitable Break()
        {
            _stopped = true;
            return this;
        }

        public Yielder<T> GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            if (!_running)
            {
                _running = true;
                _create(this);
            }
            else
            {
                _hasValue = false;
                _continuation();
            }

            return !_stopped && _hasValue;
        }

        public T Current
        {
            get
            {
                return _value;
            }
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public IAwaiter GetAwaiter()
        {
            return this;
        }

        public bool IsCompleted
        {
            get { return false; }
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            _continuation = continuation;
        }

        public void GetResult() { }


        public void OnCompleted(Action continuation)
        {
            _continuation = continuation;
        }
    }
}
#endif
