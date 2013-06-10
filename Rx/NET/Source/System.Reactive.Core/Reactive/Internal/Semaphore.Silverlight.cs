// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if NO_SEMAPHORE && (SILVERLIGHT || PLIB_LITE)
using System;
using System.Threading;

namespace System.Reactive.Threading
{
    //Monitor based implementation of Semaphore
    //that mimicks the .NET Semaphore class (System.Threading.Semaphore)

    internal sealed class Semaphore : IDisposable
    {
        private int m_currentCount;
        private readonly int m_maximumCount;
        private readonly object m_lockObject;
        private bool m_disposed;

        public Semaphore(int initialCount, int maximumCount)
        {
            if (initialCount < 0)
            {
                throw new ArgumentOutOfRangeException("initialCount", "Non-negative number required.");
            }
            if (maximumCount < 1)
            {
                throw new ArgumentOutOfRangeException("maximumCount", "Positive number required.");
            }
            if (initialCount > maximumCount)
            {
                throw new ArgumentException("Initial count must be smaller than maximum");
            }

            m_currentCount = initialCount;
            m_maximumCount = maximumCount;
            m_lockObject = new object();            
        }

        public int Release()
        {
            return this.Release(1);
        }

        public int Release(int releaseCount)
        {            
            if (releaseCount < 1)
            {
                throw new ArgumentOutOfRangeException("releaseCount", "Positive number required.");
            }
            if (m_disposed)
            {
                throw new ObjectDisposedException("Semaphore");
            }

            var oldCount = default(int);
            lock (m_lockObject)
            {
                oldCount = m_currentCount;
                if (releaseCount + m_currentCount > m_maximumCount)
                {
                    throw new ArgumentOutOfRangeException("releaseCount", "Amount of releases would overflow maximum");
                }
                m_currentCount += releaseCount;
                //PulseAll makes sure all waiting threads get queued for acquiring the lock
                //Pulse would only queue one thread.

                Monitor.PulseAll(m_lockObject);
            }
            return oldCount;
        }

        public bool WaitOne()
        {
            return WaitOne(Timeout.Infinite);
        }

        public bool WaitOne(int millisecondsTimeout)
        {
            if (m_disposed)
            {
                throw new ObjectDisposedException("Semaphore");
            }

            lock (m_lockObject)
            {
                while (m_currentCount == 0)
                {
                    if (!Monitor.Wait(m_lockObject, millisecondsTimeout))
                    {
                        return false;
                    }
                }
                m_currentCount--;
                return true;
            }
        }

        public bool WaitOne(TimeSpan timeout)
        {
            return WaitOne((int)timeout.TotalMilliseconds);
        }

        public void Close()
        {
            Dispose();
        }

        public void Dispose()
        {
            //the .NET CLR semaphore does not release waits upon dispose
            //so we don't do that either.
            m_disposed = true;
        }
    }
}
#endif