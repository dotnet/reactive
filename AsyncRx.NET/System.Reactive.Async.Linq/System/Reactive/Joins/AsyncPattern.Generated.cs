// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Joins
{
    public class AsyncPattern<TSource1> : AsyncPattern
    {
        internal IAsyncObservable<TSource1> Source1 { get; }

        internal AsyncPattern(IAsyncObservable<TSource1> source1)
        {
            Source1 = source1;
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TResult> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlan<TSource1, TResult>(this, selector);
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, Task<TResult>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlanWithTask<TSource1, TResult>(this, selector);
        }
    }

    public class AsyncPattern<TSource1, TSource2> : AsyncPattern
    {
        internal IAsyncObservable<TSource1> Source1 { get; }
        internal IAsyncObservable<TSource2> Source2 { get; }

        internal AsyncPattern(IAsyncObservable<TSource1> source1, IAsyncObservable<TSource2> source2)
        {
            Source1 = source1;
            Source2 = source2;
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TSource2, TResult> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlan<TSource1, TSource2, TResult>(this, selector);
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TSource2, Task<TResult>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlanWithTask<TSource1, TSource2, TResult>(this, selector);
        }
    }

    public class AsyncPattern<TSource1, TSource2, TSource3> : AsyncPattern
    {
        internal IAsyncObservable<TSource1> Source1 { get; }
        internal IAsyncObservable<TSource2> Source2 { get; }
        internal IAsyncObservable<TSource3> Source3 { get; }

        internal AsyncPattern(IAsyncObservable<TSource1> source1, IAsyncObservable<TSource2> source2, IAsyncObservable<TSource3> source3)
        {
            Source1 = source1;
            Source2 = source2;
            Source3 = source3;
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TSource2, TSource3, TResult> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlan<TSource1, TSource2, TSource3, TResult>(this, selector);
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TSource2, TSource3, Task<TResult>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlanWithTask<TSource1, TSource2, TSource3, TResult>(this, selector);
        }
    }

    public class AsyncPattern<TSource1, TSource2, TSource3, TSource4> : AsyncPattern
    {
        internal IAsyncObservable<TSource1> Source1 { get; }
        internal IAsyncObservable<TSource2> Source2 { get; }
        internal IAsyncObservable<TSource3> Source3 { get; }
        internal IAsyncObservable<TSource4> Source4 { get; }

        internal AsyncPattern(IAsyncObservable<TSource1> source1, IAsyncObservable<TSource2> source2, IAsyncObservable<TSource3> source3, IAsyncObservable<TSource4> source4)
        {
            Source1 = source1;
            Source2 = source2;
            Source3 = source3;
            Source4 = source4;
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TSource2, TSource3, TSource4, TResult> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlan<TSource1, TSource2, TSource3, TSource4, TResult>(this, selector);
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TSource2, TSource3, TSource4, Task<TResult>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlanWithTask<TSource1, TSource2, TSource3, TSource4, TResult>(this, selector);
        }
    }

    public class AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5> : AsyncPattern
    {
        internal IAsyncObservable<TSource1> Source1 { get; }
        internal IAsyncObservable<TSource2> Source2 { get; }
        internal IAsyncObservable<TSource3> Source3 { get; }
        internal IAsyncObservable<TSource4> Source4 { get; }
        internal IAsyncObservable<TSource5> Source5 { get; }

        internal AsyncPattern(IAsyncObservable<TSource1> source1, IAsyncObservable<TSource2> source2, IAsyncObservable<TSource3> source3, IAsyncObservable<TSource4> source4, IAsyncObservable<TSource5> source5)
        {
            Source1 = source1;
            Source2 = source2;
            Source3 = source3;
            Source4 = source4;
            Source5 = source5;
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TSource2, TSource3, TSource4, TSource5, TResult> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>(this, selector);
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TSource2, TSource3, TSource4, TSource5, Task<TResult>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlanWithTask<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>(this, selector);
        }
    }

    public class AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6> : AsyncPattern
    {
        internal IAsyncObservable<TSource1> Source1 { get; }
        internal IAsyncObservable<TSource2> Source2 { get; }
        internal IAsyncObservable<TSource3> Source3 { get; }
        internal IAsyncObservable<TSource4> Source4 { get; }
        internal IAsyncObservable<TSource5> Source5 { get; }
        internal IAsyncObservable<TSource6> Source6 { get; }

        internal AsyncPattern(IAsyncObservable<TSource1> source1, IAsyncObservable<TSource2> source2, IAsyncObservable<TSource3> source3, IAsyncObservable<TSource4> source4, IAsyncObservable<TSource5> source5, IAsyncObservable<TSource6> source6)
        {
            Source1 = source1;
            Source2 = source2;
            Source3 = source3;
            Source4 = source4;
            Source5 = source5;
            Source6 = source6;
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>(this, selector);
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, Task<TResult>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlanWithTask<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>(this, selector);
        }
    }

    public class AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7> : AsyncPattern
    {
        internal IAsyncObservable<TSource1> Source1 { get; }
        internal IAsyncObservable<TSource2> Source2 { get; }
        internal IAsyncObservable<TSource3> Source3 { get; }
        internal IAsyncObservable<TSource4> Source4 { get; }
        internal IAsyncObservable<TSource5> Source5 { get; }
        internal IAsyncObservable<TSource6> Source6 { get; }
        internal IAsyncObservable<TSource7> Source7 { get; }

        internal AsyncPattern(IAsyncObservable<TSource1> source1, IAsyncObservable<TSource2> source2, IAsyncObservable<TSource3> source3, IAsyncObservable<TSource4> source4, IAsyncObservable<TSource5> source5, IAsyncObservable<TSource6> source6, IAsyncObservable<TSource7> source7)
        {
            Source1 = source1;
            Source2 = source2;
            Source3 = source3;
            Source4 = source4;
            Source5 = source5;
            Source6 = source6;
            Source7 = source7;
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>(this, selector);
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, Task<TResult>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlanWithTask<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>(this, selector);
        }
    }

    public class AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8> : AsyncPattern
    {
        internal IAsyncObservable<TSource1> Source1 { get; }
        internal IAsyncObservable<TSource2> Source2 { get; }
        internal IAsyncObservable<TSource3> Source3 { get; }
        internal IAsyncObservable<TSource4> Source4 { get; }
        internal IAsyncObservable<TSource5> Source5 { get; }
        internal IAsyncObservable<TSource6> Source6 { get; }
        internal IAsyncObservable<TSource7> Source7 { get; }
        internal IAsyncObservable<TSource8> Source8 { get; }

        internal AsyncPattern(IAsyncObservable<TSource1> source1, IAsyncObservable<TSource2> source2, IAsyncObservable<TSource3> source3, IAsyncObservable<TSource4> source4, IAsyncObservable<TSource5> source5, IAsyncObservable<TSource6> source6, IAsyncObservable<TSource7> source7, IAsyncObservable<TSource8> source8)
        {
            Source1 = source1;
            Source2 = source2;
            Source3 = source3;
            Source4 = source4;
            Source5 = source5;
            Source6 = source6;
            Source7 = source7;
            Source8 = source8;
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>(this, selector);
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, Task<TResult>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlanWithTask<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>(this, selector);
        }
    }

    public class AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9> : AsyncPattern
    {
        internal IAsyncObservable<TSource1> Source1 { get; }
        internal IAsyncObservable<TSource2> Source2 { get; }
        internal IAsyncObservable<TSource3> Source3 { get; }
        internal IAsyncObservable<TSource4> Source4 { get; }
        internal IAsyncObservable<TSource5> Source5 { get; }
        internal IAsyncObservable<TSource6> Source6 { get; }
        internal IAsyncObservable<TSource7> Source7 { get; }
        internal IAsyncObservable<TSource8> Source8 { get; }
        internal IAsyncObservable<TSource9> Source9 { get; }

        internal AsyncPattern(IAsyncObservable<TSource1> source1, IAsyncObservable<TSource2> source2, IAsyncObservable<TSource3> source3, IAsyncObservable<TSource4> source4, IAsyncObservable<TSource5> source5, IAsyncObservable<TSource6> source6, IAsyncObservable<TSource7> source7, IAsyncObservable<TSource8> source8, IAsyncObservable<TSource9> source9)
        {
            Source1 = source1;
            Source2 = source2;
            Source3 = source3;
            Source4 = source4;
            Source5 = source5;
            Source6 = source6;
            Source7 = source7;
            Source8 = source8;
            Source9 = source9;
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>(this, selector);
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, Task<TResult>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlanWithTask<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>(this, selector);
        }
    }

    public class AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10> : AsyncPattern
    {
        internal IAsyncObservable<TSource1> Source1 { get; }
        internal IAsyncObservable<TSource2> Source2 { get; }
        internal IAsyncObservable<TSource3> Source3 { get; }
        internal IAsyncObservable<TSource4> Source4 { get; }
        internal IAsyncObservable<TSource5> Source5 { get; }
        internal IAsyncObservable<TSource6> Source6 { get; }
        internal IAsyncObservable<TSource7> Source7 { get; }
        internal IAsyncObservable<TSource8> Source8 { get; }
        internal IAsyncObservable<TSource9> Source9 { get; }
        internal IAsyncObservable<TSource10> Source10 { get; }

        internal AsyncPattern(IAsyncObservable<TSource1> source1, IAsyncObservable<TSource2> source2, IAsyncObservable<TSource3> source3, IAsyncObservable<TSource4> source4, IAsyncObservable<TSource5> source5, IAsyncObservable<TSource6> source6, IAsyncObservable<TSource7> source7, IAsyncObservable<TSource8> source8, IAsyncObservable<TSource9> source9, IAsyncObservable<TSource10> source10)
        {
            Source1 = source1;
            Source2 = source2;
            Source3 = source3;
            Source4 = source4;
            Source5 = source5;
            Source6 = source6;
            Source7 = source7;
            Source8 = source8;
            Source9 = source9;
            Source10 = source10;
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>(this, selector);
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, Task<TResult>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlanWithTask<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>(this, selector);
        }
    }

    public class AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11> : AsyncPattern
    {
        internal IAsyncObservable<TSource1> Source1 { get; }
        internal IAsyncObservable<TSource2> Source2 { get; }
        internal IAsyncObservable<TSource3> Source3 { get; }
        internal IAsyncObservable<TSource4> Source4 { get; }
        internal IAsyncObservable<TSource5> Source5 { get; }
        internal IAsyncObservable<TSource6> Source6 { get; }
        internal IAsyncObservable<TSource7> Source7 { get; }
        internal IAsyncObservable<TSource8> Source8 { get; }
        internal IAsyncObservable<TSource9> Source9 { get; }
        internal IAsyncObservable<TSource10> Source10 { get; }
        internal IAsyncObservable<TSource11> Source11 { get; }

        internal AsyncPattern(IAsyncObservable<TSource1> source1, IAsyncObservable<TSource2> source2, IAsyncObservable<TSource3> source3, IAsyncObservable<TSource4> source4, IAsyncObservable<TSource5> source5, IAsyncObservable<TSource6> source6, IAsyncObservable<TSource7> source7, IAsyncObservable<TSource8> source8, IAsyncObservable<TSource9> source9, IAsyncObservable<TSource10> source10, IAsyncObservable<TSource11> source11)
        {
            Source1 = source1;
            Source2 = source2;
            Source3 = source3;
            Source4 = source4;
            Source5 = source5;
            Source6 = source6;
            Source7 = source7;
            Source8 = source8;
            Source9 = source9;
            Source10 = source10;
            Source11 = source11;
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>(this, selector);
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, Task<TResult>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlanWithTask<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>(this, selector);
        }
    }

    public class AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12> : AsyncPattern
    {
        internal IAsyncObservable<TSource1> Source1 { get; }
        internal IAsyncObservable<TSource2> Source2 { get; }
        internal IAsyncObservable<TSource3> Source3 { get; }
        internal IAsyncObservable<TSource4> Source4 { get; }
        internal IAsyncObservable<TSource5> Source5 { get; }
        internal IAsyncObservable<TSource6> Source6 { get; }
        internal IAsyncObservable<TSource7> Source7 { get; }
        internal IAsyncObservable<TSource8> Source8 { get; }
        internal IAsyncObservable<TSource9> Source9 { get; }
        internal IAsyncObservable<TSource10> Source10 { get; }
        internal IAsyncObservable<TSource11> Source11 { get; }
        internal IAsyncObservable<TSource12> Source12 { get; }

        internal AsyncPattern(IAsyncObservable<TSource1> source1, IAsyncObservable<TSource2> source2, IAsyncObservable<TSource3> source3, IAsyncObservable<TSource4> source4, IAsyncObservable<TSource5> source5, IAsyncObservable<TSource6> source6, IAsyncObservable<TSource7> source7, IAsyncObservable<TSource8> source8, IAsyncObservable<TSource9> source9, IAsyncObservable<TSource10> source10, IAsyncObservable<TSource11> source11, IAsyncObservable<TSource12> source12)
        {
            Source1 = source1;
            Source2 = source2;
            Source3 = source3;
            Source4 = source4;
            Source5 = source5;
            Source6 = source6;
            Source7 = source7;
            Source8 = source8;
            Source9 = source9;
            Source10 = source10;
            Source11 = source11;
            Source12 = source12;
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>(this, selector);
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, Task<TResult>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlanWithTask<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>(this, selector);
        }
    }

    public class AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13> : AsyncPattern
    {
        internal IAsyncObservable<TSource1> Source1 { get; }
        internal IAsyncObservable<TSource2> Source2 { get; }
        internal IAsyncObservable<TSource3> Source3 { get; }
        internal IAsyncObservable<TSource4> Source4 { get; }
        internal IAsyncObservable<TSource5> Source5 { get; }
        internal IAsyncObservable<TSource6> Source6 { get; }
        internal IAsyncObservable<TSource7> Source7 { get; }
        internal IAsyncObservable<TSource8> Source8 { get; }
        internal IAsyncObservable<TSource9> Source9 { get; }
        internal IAsyncObservable<TSource10> Source10 { get; }
        internal IAsyncObservable<TSource11> Source11 { get; }
        internal IAsyncObservable<TSource12> Source12 { get; }
        internal IAsyncObservable<TSource13> Source13 { get; }

        internal AsyncPattern(IAsyncObservable<TSource1> source1, IAsyncObservable<TSource2> source2, IAsyncObservable<TSource3> source3, IAsyncObservable<TSource4> source4, IAsyncObservable<TSource5> source5, IAsyncObservable<TSource6> source6, IAsyncObservable<TSource7> source7, IAsyncObservable<TSource8> source8, IAsyncObservable<TSource9> source9, IAsyncObservable<TSource10> source10, IAsyncObservable<TSource11> source11, IAsyncObservable<TSource12> source12, IAsyncObservable<TSource13> source13)
        {
            Source1 = source1;
            Source2 = source2;
            Source3 = source3;
            Source4 = source4;
            Source5 = source5;
            Source6 = source6;
            Source7 = source7;
            Source8 = source8;
            Source9 = source9;
            Source10 = source10;
            Source11 = source11;
            Source12 = source12;
            Source13 = source13;
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>(this, selector);
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, Task<TResult>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlanWithTask<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>(this, selector);
        }
    }

    public class AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14> : AsyncPattern
    {
        internal IAsyncObservable<TSource1> Source1 { get; }
        internal IAsyncObservable<TSource2> Source2 { get; }
        internal IAsyncObservable<TSource3> Source3 { get; }
        internal IAsyncObservable<TSource4> Source4 { get; }
        internal IAsyncObservable<TSource5> Source5 { get; }
        internal IAsyncObservable<TSource6> Source6 { get; }
        internal IAsyncObservable<TSource7> Source7 { get; }
        internal IAsyncObservable<TSource8> Source8 { get; }
        internal IAsyncObservable<TSource9> Source9 { get; }
        internal IAsyncObservable<TSource10> Source10 { get; }
        internal IAsyncObservable<TSource11> Source11 { get; }
        internal IAsyncObservable<TSource12> Source12 { get; }
        internal IAsyncObservable<TSource13> Source13 { get; }
        internal IAsyncObservable<TSource14> Source14 { get; }

        internal AsyncPattern(IAsyncObservable<TSource1> source1, IAsyncObservable<TSource2> source2, IAsyncObservable<TSource3> source3, IAsyncObservable<TSource4> source4, IAsyncObservable<TSource5> source5, IAsyncObservable<TSource6> source6, IAsyncObservable<TSource7> source7, IAsyncObservable<TSource8> source8, IAsyncObservable<TSource9> source9, IAsyncObservable<TSource10> source10, IAsyncObservable<TSource11> source11, IAsyncObservable<TSource12> source12, IAsyncObservable<TSource13> source13, IAsyncObservable<TSource14> source14)
        {
            Source1 = source1;
            Source2 = source2;
            Source3 = source3;
            Source4 = source4;
            Source5 = source5;
            Source6 = source6;
            Source7 = source7;
            Source8 = source8;
            Source9 = source9;
            Source10 = source10;
            Source11 = source11;
            Source12 = source12;
            Source13 = source13;
            Source14 = source14;
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>(this, selector);
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, Task<TResult>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlanWithTask<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>(this, selector);
        }
    }

    public class AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15> : AsyncPattern
    {
        internal IAsyncObservable<TSource1> Source1 { get; }
        internal IAsyncObservable<TSource2> Source2 { get; }
        internal IAsyncObservable<TSource3> Source3 { get; }
        internal IAsyncObservable<TSource4> Source4 { get; }
        internal IAsyncObservable<TSource5> Source5 { get; }
        internal IAsyncObservable<TSource6> Source6 { get; }
        internal IAsyncObservable<TSource7> Source7 { get; }
        internal IAsyncObservable<TSource8> Source8 { get; }
        internal IAsyncObservable<TSource9> Source9 { get; }
        internal IAsyncObservable<TSource10> Source10 { get; }
        internal IAsyncObservable<TSource11> Source11 { get; }
        internal IAsyncObservable<TSource12> Source12 { get; }
        internal IAsyncObservable<TSource13> Source13 { get; }
        internal IAsyncObservable<TSource14> Source14 { get; }
        internal IAsyncObservable<TSource15> Source15 { get; }

        internal AsyncPattern(IAsyncObservable<TSource1> source1, IAsyncObservable<TSource2> source2, IAsyncObservable<TSource3> source3, IAsyncObservable<TSource4> source4, IAsyncObservable<TSource5> source5, IAsyncObservable<TSource6> source6, IAsyncObservable<TSource7> source7, IAsyncObservable<TSource8> source8, IAsyncObservable<TSource9> source9, IAsyncObservable<TSource10> source10, IAsyncObservable<TSource11> source11, IAsyncObservable<TSource12> source12, IAsyncObservable<TSource13> source13, IAsyncObservable<TSource14> source14, IAsyncObservable<TSource15> source15)
        {
            Source1 = source1;
            Source2 = source2;
            Source3 = source3;
            Source4 = source4;
            Source5 = source5;
            Source6 = source6;
            Source7 = source7;
            Source8 = source8;
            Source9 = source9;
            Source10 = source10;
            Source11 = source11;
            Source12 = source12;
            Source13 = source13;
            Source14 = source14;
            Source15 = source15;
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>(this, selector);
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, Task<TResult>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlanWithTask<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>(this, selector);
        }
    }

    public class AsyncPattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16> : AsyncPattern
    {
        internal IAsyncObservable<TSource1> Source1 { get; }
        internal IAsyncObservable<TSource2> Source2 { get; }
        internal IAsyncObservable<TSource3> Source3 { get; }
        internal IAsyncObservable<TSource4> Source4 { get; }
        internal IAsyncObservable<TSource5> Source5 { get; }
        internal IAsyncObservable<TSource6> Source6 { get; }
        internal IAsyncObservable<TSource7> Source7 { get; }
        internal IAsyncObservable<TSource8> Source8 { get; }
        internal IAsyncObservable<TSource9> Source9 { get; }
        internal IAsyncObservable<TSource10> Source10 { get; }
        internal IAsyncObservable<TSource11> Source11 { get; }
        internal IAsyncObservable<TSource12> Source12 { get; }
        internal IAsyncObservable<TSource13> Source13 { get; }
        internal IAsyncObservable<TSource14> Source14 { get; }
        internal IAsyncObservable<TSource15> Source15 { get; }
        internal IAsyncObservable<TSource16> Source16 { get; }

        internal AsyncPattern(IAsyncObservable<TSource1> source1, IAsyncObservable<TSource2> source2, IAsyncObservable<TSource3> source3, IAsyncObservable<TSource4> source4, IAsyncObservable<TSource5> source5, IAsyncObservable<TSource6> source6, IAsyncObservable<TSource7> source7, IAsyncObservable<TSource8> source8, IAsyncObservable<TSource9> source9, IAsyncObservable<TSource10> source10, IAsyncObservable<TSource11> source11, IAsyncObservable<TSource12> source12, IAsyncObservable<TSource13> source13, IAsyncObservable<TSource14> source14, IAsyncObservable<TSource15> source15, IAsyncObservable<TSource16> source16)
        {
            Source1 = source1;
            Source2 = source2;
            Source3 = source3;
            Source4 = source4;
            Source5 = source5;
            Source6 = source6;
            Source7 = source7;
            Source8 = source8;
            Source9 = source9;
            Source10 = source10;
            Source11 = source11;
            Source12 = source12;
            Source13 = source13;
            Source14 = source14;
            Source15 = source15;
            Source16 = source16;
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>(this, selector);
        }

        public AsyncPlan<TResult> Then<TResult>(Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, Task<TResult>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPlanWithTask<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>(this, selector);
        }
    }

}
