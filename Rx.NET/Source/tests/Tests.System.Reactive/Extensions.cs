// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;

namespace ReactiveTests
{
    public static class Extensions
    {
        //public static IDisposable ScheduleAbsolute(this TestScheduler scheduler, long time, Action action)
        //{
        //    return scheduler.ScheduleAbsolute(default(object), time, (scheduler1, state1) => { action(); return Disposable.Empty; });
        //}

        //public static IDisposable ScheduleRelative(this TestScheduler scheduler, long time, Action action)
        //{
        //    return scheduler.ScheduleRelative(default(object), time, (scheduler1, state1) => { action(); return Disposable.Empty; });
        //}

        public static void EnsureTrampoline(this CurrentThreadScheduler scheduler, Action action)
        {
            if (scheduler.ScheduleRequired)
            {
                scheduler.Schedule(action);
            }
            else
            {
                action();
            }
        }

        public static IEnumerable<R> Zip<T1, T2, R>(this IEnumerable<T1> source1, IEnumerable<T2> source2, Func<T1, T2, R> f)
        {
            using (var e1 = source1.GetEnumerator())
            using (var e2 = source2.GetEnumerator())
            {
                while (e1.MoveNext() && e2.MoveNext())
                {
                    yield return f(e1.Current, e2.Current);
                }
            }
        }
    }
}
