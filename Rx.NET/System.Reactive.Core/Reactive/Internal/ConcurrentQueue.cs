// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

/*
 * WARNING: Auto-generated file (7/18/2012 4:47:38 PM)
 *
 * Stripped down code based on ndp\clr\src\BCL\System\Collections\Concurrent\ConcurrentQueue.cs
 */

#if NO_CDS_COLLECTIONS

#pragma warning disable 0420

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;

namespace System.Collections.Concurrent
{
    internal class ConcurrentQueue<T>
    {
        private volatile Segment m_head;
        private volatile Segment m_tail;

        private const int SEGMENT_SIZE = 32;

        public ConcurrentQueue()
        {
            m_head = m_tail = new Segment(0, this);
        }

        public bool IsEmpty
        {
            get
            {
                Segment head = m_head;
                if (!head.IsEmpty)
                    //fast route 1:
                    //if current head is not empty, then queue is not empty
                    return false;
                else if (head.Next == null)
                    //fast route 2:
                    //if current head is empty and it's the last segment
                    //then queue is empty
                    return true;
                else
                //slow route:
                //current head is empty and it is NOT the last segment,
                //it means another thread is growing new segment 
                {
                    SpinWait spin = new SpinWait();
                    while (head.IsEmpty)
                    {
                        if (head.Next == null)
                            return true;

                        spin.SpinOnce();
                        head = m_head;
                    }
                    return false;
                }
            }
        }

        public void Enqueue(T item)
        {
            SpinWait spin = new SpinWait();
            while (true)
            {
                Segment tail = m_tail;
                if (tail.TryAppend(item))
                    return;
                spin.SpinOnce();
            }
        }

        public bool TryDequeue(out T result)
        {
            while (!IsEmpty)
            {
                Segment head = m_head;
                if (head.TryRemove(out result))
                    return true;
                //since method IsEmpty spins, we don't need to spin in the while loop
            }
            result = default(T);
            return false;
        }

        private class Segment
        {
            //we define two volatile arrays: m_array and m_state. Note that the accesses to the array items 
            //do not get volatile treatment. But we don't need to worry about loading adjacent elements or 
            //store/load on adjacent elements would suffer reordering. 
            // - Two stores:  these are at risk, but CLRv2 memory model guarantees store-release hence we are safe.
            // - Two loads: because one item from two volatile arrays are accessed, the loads of the array references
            //          are sufficient to prevent reordering of the loads of the elements.
            internal volatile T[] m_array;

            // For each entry in m_array, the corresponding entry in m_state indicates whether this position contains 
            // a valid value. m_state is initially all false. 
            internal volatile VolatileBool[] m_state;

            //pointer to the next segment. null if the current segment is the last segment
            private volatile Segment m_next;

            //We use this zero based index to track how many segments have been created for the queue, and
            //to compute how many active segments are there currently. 
            // * The number of currently active segments is : m_tail.m_index - m_head.m_index + 1;
            // * m_index is incremented with every Segment.Grow operation. We use Int64 type, and we can safely 
            //   assume that it never overflows. To overflow, we need to do 2^63 increments, even at a rate of 4 
            //   billion (2^32) increments per second, it takes 2^31 seconds, which is about 64 years.
            internal readonly long m_index;

            //indices of where the first and last valid values
            // - m_low points to the position of the next element to pop from this segment, range [0, infinity)
            //      m_low >= SEGMENT_SIZE implies the segment is disposable
            // - m_high points to the position of the latest pushed element, range [-1, infinity)
            //      m_high == -1 implies the segment is new and empty
            //      m_high >= SEGMENT_SIZE-1 means this segment is ready to grow. 
            //        and the thread who sets m_high to SEGMENT_SIZE-1 is responsible to grow the segment
            // - Math.Min(m_low, SEGMENT_SIZE) > Math.Min(m_high, SEGMENT_SIZE-1) implies segment is empty
            // - initially m_low =0 and m_high=-1;
            private volatile int m_low;
            private volatile int m_high;

            private volatile ConcurrentQueue<T> m_source;

            internal Segment(long index, ConcurrentQueue<T> source)
            {
                m_array = new T[SEGMENT_SIZE];
                m_state = new VolatileBool[SEGMENT_SIZE]; //all initialized to false
                m_high = -1;
                Contract.Assert(index >= 0);
                m_index = index;
                m_source = source;
            }

            internal Segment Next
            {
                get { return m_next; }
            }

            internal bool IsEmpty
            {
                get { return (Low > High); }
            }

            internal void UnsafeAdd(T value)
            {
                Contract.Assert(m_high < SEGMENT_SIZE - 1);
                m_high++;
                m_array[m_high] = value;
                m_state[m_high].m_value = true;
            }

            internal Segment UnsafeGrow()
            {
                Contract.Assert(m_high >= SEGMENT_SIZE - 1);
                Segment newSegment = new Segment(m_index + 1, m_source); //m_index is Int64, we don't need to worry about overflow
                m_next = newSegment;
                return newSegment;
            }

            internal void Grow()
            {
                //no CAS is needed, since there is no contention (other threads are blocked, busy waiting)
                Segment newSegment = new Segment(m_index + 1, m_source);  //m_index is Int64, we don't need to worry about overflow
                m_next = newSegment;
                Contract.Assert(m_source.m_tail == this);
                m_source.m_tail = m_next;
            }

            internal bool TryAppend(T value)
            {
                //quickly check if m_high is already over the boundary, if so, bail out
                if (m_high >= SEGMENT_SIZE - 1)
                {
                    return false;
                }

                //Now we will use a CAS to increment m_high, and store the result in newhigh.
                //Depending on how many free spots left in this segment and how many threads are doing this Increment
                //at this time, the returning "newhigh" can be 
                // 1) < SEGMENT_SIZE - 1 : we took a spot in this segment, and not the last one, just insert the value
                // 2) == SEGMENT_SIZE - 1 : we took the last spot, insert the value AND grow the segment
                // 3) > SEGMENT_SIZE - 1 : we failed to reserve a spot in this segment, we return false to 
                //    Queue.Enqueue method, telling it to try again in the next segment.

                int newhigh = SEGMENT_SIZE; //initial value set to be over the boundary

                //We need do Interlocked.Increment and value/state update in a finally block to ensure that they run
                //without interuption. This is to prevent anything from happening between them, and another dequeue
                //thread maybe spinning forever to wait for m_state[] to be true;
                try
                { }
                finally
                {
                    newhigh = Interlocked.Increment(ref m_high);
                    if (newhigh <= SEGMENT_SIZE - 1)
                    {
                        m_array[newhigh] = value;
                        m_state[newhigh].m_value = true;
                    }

                    //if this thread takes up the last slot in the segment, then this thread is responsible
                    //to grow a new segment. Calling Grow must be in the finally block too for reliability reason:
                    //if thread abort during Grow, other threads will be left busy spinning forever.
                    if (newhigh == SEGMENT_SIZE - 1)
                    {
                        Grow();
                    }
                }

                //if newhigh <= SEGMENT_SIZE-1, it means the current thread successfully takes up a spot
                return newhigh <= SEGMENT_SIZE - 1;
            }

            internal bool TryRemove(out T result)
            {
                SpinWait spin = new SpinWait();
                int lowLocal = Low, highLocal = High;
                while (lowLocal <= highLocal)
                {
                    //try to update m_low
                    if (Interlocked.CompareExchange(ref m_low, lowLocal + 1, lowLocal) == lowLocal)
                    {
                        //if the specified value is not available (this spot is taken by a push operation,
                        // but the value is not written into yet), then spin
                        SpinWait spinLocal = new SpinWait();
                        while (!m_state[lowLocal].m_value)
                        {
                            spinLocal.SpinOnce();
                        }
                        result = m_array[lowLocal];
                        m_array[lowLocal] = default(T); //release the reference to the object. 

                        //if the current thread sets m_low to SEGMENT_SIZE, which means the current segment becomes
                        //disposable, then this thread is responsible to dispose this segment, and reset m_head 
                        if (lowLocal + 1 >= SEGMENT_SIZE)
                        {
                            //  Invariant: we only dispose the current m_head, not any other segment
                            //  In usual situation, disposing a segment is simply seting m_head to m_head.m_next
                            //  But there is one special case, where m_head and m_tail points to the same and ONLY
                            //segment of the queue: Another thread A is doing Enqueue and finds that it needs to grow,
                            //while the *current* thread is doing *this* Dequeue operation, and finds that it needs to 
                            //dispose the current (and ONLY) segment. Then we need to wait till thread A finishes its 
                            //Grow operation, this is the reason of having the following while loop
                            spinLocal = new SpinWait();
                            while (m_next == null)
                            {
                                spinLocal.SpinOnce();
                            }
                            Contract.Assert(m_source.m_head == this);
                            m_source.m_head = m_next;
                        }
                        return true;
                    }
                    else
                    {
                        //CAS failed due to contention: spin briefly and retry
                        spin.SpinOnce();
                        lowLocal = Low; highLocal = High;
                    }
                }//end of while
                result = default(T);
                return false;
            }

            internal bool TryPeek(out T result)
            {
                result = default(T);
                int lowLocal = Low;
                if (lowLocal > High)
                    return false;
                SpinWait spin = new SpinWait();
                while (!m_state[lowLocal].m_value)
                {
                    spin.SpinOnce();
                }
                result = m_array[lowLocal];
                return true;
            }

            internal int Low
            {
                get
                {
                    return Math.Min(m_low, SEGMENT_SIZE);
                }
            }

            internal int High
            {
                get
                {
                    //if m_high > SEGMENT_SIZE, it means it's out of range, we should return
                    //SEGMENT_SIZE-1 as the logical position
                    return Math.Min(m_high, SEGMENT_SIZE - 1);
                }
            }

        }
    }//end of class Segment

    struct VolatileBool
    {
        public VolatileBool(bool value)
        {
            m_value = value;
        }
        public volatile bool m_value;
    }
}

#endif