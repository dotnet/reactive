// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if NO_LAZY
#pragma warning disable 0420

//
// Based on ndp\clr\src\BCL\System\Lazy.cs but with LazyThreadSafetyMode.ExecutionAndPublication mode behavior hardcoded.
//

using System.Diagnostics;
using System.Threading;
using System.Reactive;

namespace System
{
    internal class Lazy<T>
    {
        class Boxed
        {
            internal Boxed(T value)
            {
                m_value = value;
            }

            internal T m_value;
        }

        static Func<T> ALREADY_INVOKED_SENTINEL = delegate { return default(T); };

        private object m_boxed;
        private Func<T> m_valueFactory;
        private volatile object m_threadSafeObj;

        public Lazy(Func<T> valueFactory)
        {
            m_threadSafeObj = new object();
            m_valueFactory = valueFactory;
        }

#if !NO_DEBUGGER_ATTRIBUTES
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        public T Value
        {
            get
            {
                Boxed boxed = null;
                if (m_boxed != null)
                {
                    boxed = m_boxed as Boxed;
                    if (boxed != null)
                    {
                        return boxed.m_value;
                    }

                    var exc = m_boxed as Exception;
                    exc.Throw();
                }

                return LazyInitValue();
            }
        }

        private T LazyInitValue()
        {
            Boxed boxed = null;
            object threadSafeObj = m_threadSafeObj;
            bool lockTaken = false;
            try
            {
                if (threadSafeObj != (object)ALREADY_INVOKED_SENTINEL)
                {
                    Monitor.Enter(threadSafeObj);
                    lockTaken = true;
                }

                if (m_boxed == null)
                {
                    boxed = CreateValue();
                    m_boxed = boxed;
                    m_threadSafeObj = ALREADY_INVOKED_SENTINEL;
                }
                else
                {
                    boxed = m_boxed as Boxed;
                    if (boxed == null)
                    {
                        var exc = m_boxed as Exception;
                        exc.Throw();
                    }
                }
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(threadSafeObj);
            }

            return boxed.m_value;
        }

        private Boxed CreateValue()
        {
            Boxed boxed = null;
            try
            {
                if (m_valueFactory == ALREADY_INVOKED_SENTINEL)
                    throw new InvalidOperationException();

                Func<T> factory = m_valueFactory;
                m_valueFactory = ALREADY_INVOKED_SENTINEL;

                boxed = new Boxed(factory());
            }
            catch (Exception ex)
            {
                m_boxed = ex;
                throw;
            }

            return boxed;
        }
    }
}
#pragma warning restore 0420
#endif