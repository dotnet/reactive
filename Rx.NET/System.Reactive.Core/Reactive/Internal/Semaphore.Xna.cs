// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if NO_SEMAPHORE && (XNA || NETCF)
using System;
using System.Collections.Generic;
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
        private readonly List<ManualResetEvent> m_waiting;

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
            m_waiting = new List<ManualResetEvent>();
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
            var toBeReleased = new List<ManualResetEvent>();
            lock (m_lockObject)
            {
                oldCount = m_currentCount;
                if (releaseCount + m_currentCount > m_maximumCount)
                {
                    throw new ArgumentOutOfRangeException("releaseCount", "Amount of releases would overflow maximum");
                }

                var waiting = m_waiting.ToArray();
                var left = Math.Max(0, releaseCount - waiting.Length);
                for (var i = 0; i < releaseCount && i < m_waiting.Count; i++)
                {
                    toBeReleased.Add(waiting[i]);
                    m_waiting.RemoveAt(0);
                }
                m_currentCount += left;
            }
            foreach(var release in toBeReleased)
            {
                release.Set();
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

            var manualResetEvent = default(ManualResetEvent);

            lock (m_lockObject)
            {
                if (m_currentCount == 0)
                {
                    manualResetEvent = new ManualResetEvent(false);
                    m_waiting.Add(manualResetEvent);
                }
                else
                {
                    m_currentCount--;
                    return true;
                }
            }
#if XNA_31_ZUNE || NETCF35
            if (!manualResetEvent.WaitOne(millisecondsTimeout, false))
#else
            if (!manualResetEvent.WaitOne(millisecondsTimeout))
#endif
            {
                lock(m_lockObject)
                {
                    m_waiting.Remove(manualResetEvent);
                }
                return false;
            }
            return true;
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