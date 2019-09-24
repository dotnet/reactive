// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Security;

namespace System.Linq
{
    internal sealed class Yielder<T> : IYielder<T>, IAwaitable, IAwaiter
    {
        private readonly Action<Yielder<T>> _create;
        private Action? _continuation;
        private bool _hasValue;
        private bool _running;
        private bool _stopped;

        public Yielder(Action<Yielder<T>> create)
        {
            _create = create;
            Current = default!;
        }

        public T Current { get; private set; }

        public IAwaiter GetAwaiter() => this;

        public bool IsCompleted => false;

        public void GetResult()
        {
        }

        [SecurityCritical]
        public void UnsafeOnCompleted(Action continuation)
        {
            _continuation = continuation;
        }

        public void OnCompleted(Action continuation)
        {
            _continuation = continuation;
        }

        public IAwaitable Return(T value)
        {
            _hasValue = true;
            Current = value;
            return this;
        }

        public IAwaitable Break()
        {
            _stopped = true;
            return this;
        }

        public Yielder<T> GetEnumerator() => this;

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
                _continuation!();
            }

            return !_stopped && _hasValue;
        }

        public void Reset() => throw new NotSupportedException();
    }
}
