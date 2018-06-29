// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Security;

namespace System.Linq
{
    /// <summary>
    ///     Interface for yielding elements to enumerator.
    /// </summary>
    /// <typeparam name="T">Type of the elements yielded to an enumerator.</typeparam>
    public interface IYielder<in T>
    {
        /// <summary>
        ///     Stops the enumeration.
        /// </summary>
        /// <returns>Awaitable object for use in an asynchronous method.</returns>
        IAwaitable Break();

        /// <summary>
        ///     Yields a value to the enumerator.
        /// </summary>
        /// <param name="value">Value to yield return.</param>
        /// <returns>Awaitable object for use in an asynchronous method.</returns>
        IAwaitable Return(T value);
    }

    internal class Yielder<T> : IYielder<T>, IAwaitable, IAwaiter
    {
        private readonly Action<Yielder<T>> _create;
        private Action _continuation;
        private bool _hasValue;
        private bool _running;
        private bool _stopped;

        public Yielder(Action<Yielder<T>> create)
        {
            _create = create;
        }

        public T Current { get; private set; }

        public IAwaiter GetAwaiter()
        {
            return this;
        }

        public bool IsCompleted
        {
            get { return false; }
        }

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

        public void Reset()
        {
            throw new NotSupportedException();
        }
    }
}