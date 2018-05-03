// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    // REVIEW: Consider if these are worth retaining in the async space.

    partial class AsyncObservable
    {
        public static Func<IAsyncObservable<TResult>> ToAsync<TResult>(Func<TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(function, TaskPoolAsyncScheduler.Default);
        }

        public static Func<IAsyncObservable<TResult>> ToAsync<TResult>(Func<TResult> function, IAsyncScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return () =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                AsyncObserver.ToAsync(subject, function); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, IAsyncObservable<TResult>> ToAsync<T1, TResult>(Func<T1, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(function, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, IAsyncObservable<TResult>> ToAsync<T1, TResult>(Func<T1, TResult> function, IAsyncScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1) =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                AsyncObserver.ToAsync(subject, function, arg1); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, IAsyncObservable<TResult>> ToAsync<T1, T2, TResult>(Func<T1, T2, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(function, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, T2, IAsyncObservable<TResult>> ToAsync<T1, T2, TResult>(Func<T1, T2, TResult> function, IAsyncScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1, arg2) =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                AsyncObserver.ToAsync(subject, function, arg1, arg2); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, IAsyncObservable<TResult>> ToAsync<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(function, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, T2, T3, IAsyncObservable<TResult>> ToAsync<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> function, IAsyncScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1, arg2, arg3) =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                AsyncObserver.ToAsync(subject, function, arg1, arg2, arg3); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, IAsyncObservable<TResult>> ToAsync<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(function, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, T2, T3, T4, IAsyncObservable<TResult>> ToAsync<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> function, IAsyncScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1, arg2, arg3, arg4) =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                AsyncObserver.ToAsync(subject, function, arg1, arg2, arg3, arg4); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, IAsyncObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(function, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, T2, T3, T4, T5, IAsyncObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> function, IAsyncScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1, arg2, arg3, arg4, arg5) =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                AsyncObserver.ToAsync(subject, function, arg1, arg2, arg3, arg4, arg5); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, IAsyncObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(function, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, T2, T3, T4, T5, T6, IAsyncObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> function, IAsyncScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1, arg2, arg3, arg4, arg5, arg6) =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                AsyncObserver.ToAsync(subject, function, arg1, arg2, arg3, arg4, arg5, arg6); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, IAsyncObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(function, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, IAsyncObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> function, IAsyncScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                AsyncObserver.ToAsync(subject, function, arg1, arg2, arg3, arg4, arg5, arg6, arg7); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, IAsyncObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(function, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, IAsyncObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> function, IAsyncScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                AsyncObserver.ToAsync(subject, function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IAsyncObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(function, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IAsyncObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> function, IAsyncScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                AsyncObserver.ToAsync(subject, function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IAsyncObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(function, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IAsyncObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> function, IAsyncScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                AsyncObserver.ToAsync(subject, function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IAsyncObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(function, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IAsyncObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> function, IAsyncScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                AsyncObserver.ToAsync(subject, function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IAsyncObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(function, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IAsyncObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> function, IAsyncScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                AsyncObserver.ToAsync(subject, function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IAsyncObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(function, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IAsyncObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> function, IAsyncScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                AsyncObserver.ToAsync(subject, function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IAsyncObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(function, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IAsyncObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> function, IAsyncScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                AsyncObserver.ToAsync(subject, function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, IAsyncObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(function, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, IAsyncObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> function, IAsyncScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                AsyncObserver.ToAsync(subject, function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, IAsyncObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(function, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, IAsyncObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> function, IAsyncScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                AsyncObserver.ToAsync(subject, function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<IAsyncObservable<Unit>> ToAsync(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(action, TaskPoolAsyncScheduler.Default);
        }

        public static Func<IAsyncObservable<Unit>> ToAsync(Action action, IAsyncScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return () =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                AsyncObserver.ToAsync(subject, action); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, IAsyncObservable<Unit>> ToAsync<T1>(Action<T1> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(action, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, IAsyncObservable<Unit>> ToAsync<T1>(Action<T1> action, IAsyncScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1) =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                AsyncObserver.ToAsync(subject, action, arg1); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, IAsyncObservable<Unit>> ToAsync<T1, T2>(Action<T1, T2> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(action, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, T2, IAsyncObservable<Unit>> ToAsync<T1, T2>(Action<T1, T2> action, IAsyncScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1, arg2) =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                AsyncObserver.ToAsync(subject, action, arg1, arg2); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, IAsyncObservable<Unit>> ToAsync<T1, T2, T3>(Action<T1, T2, T3> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(action, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, T2, T3, IAsyncObservable<Unit>> ToAsync<T1, T2, T3>(Action<T1, T2, T3> action, IAsyncScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1, arg2, arg3) =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                AsyncObserver.ToAsync(subject, action, arg1, arg2, arg3); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, IAsyncObservable<Unit>> ToAsync<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(action, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, T2, T3, T4, IAsyncObservable<Unit>> ToAsync<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, IAsyncScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1, arg2, arg3, arg4) =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                AsyncObserver.ToAsync(subject, action, arg1, arg2, arg3, arg4); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, IAsyncObservable<Unit>> ToAsync<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(action, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, T2, T3, T4, T5, IAsyncObservable<Unit>> ToAsync<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action, IAsyncScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1, arg2, arg3, arg4, arg5) =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                AsyncObserver.ToAsync(subject, action, arg1, arg2, arg3, arg4, arg5); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, IAsyncObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(action, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, T2, T3, T4, T5, T6, IAsyncObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action, IAsyncScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1, arg2, arg3, arg4, arg5, arg6) =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                AsyncObserver.ToAsync(subject, action, arg1, arg2, arg3, arg4, arg5, arg6); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, IAsyncObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(action, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, IAsyncObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action, IAsyncScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                AsyncObserver.ToAsync(subject, action, arg1, arg2, arg3, arg4, arg5, arg6, arg7); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, IAsyncObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(action, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, IAsyncObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action, IAsyncScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                AsyncObserver.ToAsync(subject, action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IAsyncObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(action, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IAsyncObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, IAsyncScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                AsyncObserver.ToAsync(subject, action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IAsyncObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(action, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IAsyncObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, IAsyncScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                AsyncObserver.ToAsync(subject, action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IAsyncObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(action, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IAsyncObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, IAsyncScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                AsyncObserver.ToAsync(subject, action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IAsyncObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(action, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IAsyncObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, IAsyncScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                AsyncObserver.ToAsync(subject, action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IAsyncObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(action, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IAsyncObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, IAsyncScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                AsyncObserver.ToAsync(subject, action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IAsyncObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(action, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IAsyncObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, IAsyncScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                AsyncObserver.ToAsync(subject, action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, IAsyncObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(action, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, IAsyncObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, IAsyncScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                AsyncObserver.ToAsync(subject, action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, IAsyncObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(action, TaskPoolAsyncScheduler.Default);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, IAsyncObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, IAsyncScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                AsyncObserver.ToAsync(subject, action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16); // NB: We don't do anything with the result of scheduling the action; it can't be cancelled.

                return subject.AsAsyncObservable();
            };
        }

    }

    partial class AsyncObserver
    {
        public static Task<IAsyncDisposable> ToAsync<TResult>(IAsyncObserver<TResult> observer, Func<TResult> function)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(observer, function, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<TResult>(IAsyncObserver<TResult> observer, Func<TResult> function, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                TResult res;
                try
                {
                    res = function();
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(res).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, TResult>(IAsyncObserver<TResult> observer, Func<T1, TResult> function, T1 arg1)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(observer, function, arg1, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, TResult>(IAsyncObserver<TResult> observer, Func<T1, TResult> function, T1 arg1, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                TResult res;
                try
                {
                    res = function(arg1);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(res).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, TResult> function, T1 arg1, T2 arg2)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(observer, function, arg1, arg2, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, TResult> function, T1 arg1, T2 arg2, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                TResult res;
                try
                {
                    res = function(arg1, arg2);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(res).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, TResult> function, T1 arg1, T2 arg2, T3 arg3)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(observer, function, arg1, arg2, arg3, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, TResult> function, T1 arg1, T2 arg2, T3 arg3, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                TResult res;
                try
                {
                    res = function(arg1, arg2, arg3);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(res).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(observer, function, arg1, arg2, arg3, arg4, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                TResult res;
                try
                {
                    res = function(arg1, arg2, arg3, arg4);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(res).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(observer, function, arg1, arg2, arg3, arg4, arg5, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                TResult res;
                try
                {
                    res = function(arg1, arg2, arg3, arg4, arg5);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(res).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(observer, function, arg1, arg2, arg3, arg4, arg5, arg6, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                TResult res;
                try
                {
                    res = function(arg1, arg2, arg3, arg4, arg5, arg6);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(res).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(observer, function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                TResult res;
                try
                {
                    res = function(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(res).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(observer, function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                TResult res;
                try
                {
                    res = function(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(res).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(observer, function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                TResult res;
                try
                {
                    res = function(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(res).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(observer, function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                TResult res;
                try
                {
                    res = function(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(res).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(observer, function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                TResult res;
                try
                {
                    res = function(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(res).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(observer, function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                TResult res;
                try
                {
                    res = function(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(res).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(observer, function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                TResult res;
                try
                {
                    res = function(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(res).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(observer, function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                TResult res;
                try
                {
                    res = function(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(res).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(observer, function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                TResult res;
                try
                {
                    res = function(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(res).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(observer, function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                TResult res;
                try
                {
                    res = function(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(res).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync(IAsyncObserver<Unit> observer, Action action)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(observer, action, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync(IAsyncObserver<Unit> observer, Action action, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(Unit.Default).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1>(IAsyncObserver<Unit> observer, Action<T1> action, T1 arg1)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(observer, action, arg1, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1>(IAsyncObserver<Unit> observer, Action<T1> action, T1 arg1, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                try
                {
                    action(arg1);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(Unit.Default).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2>(IAsyncObserver<Unit> observer, Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(observer, action, arg1, arg2, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2>(IAsyncObserver<Unit> observer, Action<T1, T2> action, T1 arg1, T2 arg2, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                try
                {
                    action(arg1, arg2);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(Unit.Default).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3>(IAsyncObserver<Unit> observer, Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(observer, action, arg1, arg2, arg3, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3>(IAsyncObserver<Unit> observer, Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                try
                {
                    action(arg1, arg2, arg3);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(Unit.Default).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4>(IAsyncObserver<Unit> observer, Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(observer, action, arg1, arg2, arg3, arg4, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4>(IAsyncObserver<Unit> observer, Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                try
                {
                    action(arg1, arg2, arg3, arg4);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(Unit.Default).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5>(IAsyncObserver<Unit> observer, Action<T1, T2, T3, T4, T5> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(observer, action, arg1, arg2, arg3, arg4, arg5, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5>(IAsyncObserver<Unit> observer, Action<T1, T2, T3, T4, T5> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                try
                {
                    action(arg1, arg2, arg3, arg4, arg5);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(Unit.Default).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6>(IAsyncObserver<Unit> observer, Action<T1, T2, T3, T4, T5, T6> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(observer, action, arg1, arg2, arg3, arg4, arg5, arg6, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6>(IAsyncObserver<Unit> observer, Action<T1, T2, T3, T4, T5, T6> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                try
                {
                    action(arg1, arg2, arg3, arg4, arg5, arg6);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(Unit.Default).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7>(IAsyncObserver<Unit> observer, Action<T1, T2, T3, T4, T5, T6, T7> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(observer, action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7>(IAsyncObserver<Unit> observer, Action<T1, T2, T3, T4, T5, T6, T7> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                try
                {
                    action(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(Unit.Default).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8>(IAsyncObserver<Unit> observer, Action<T1, T2, T3, T4, T5, T6, T7, T8> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(observer, action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8>(IAsyncObserver<Unit> observer, Action<T1, T2, T3, T4, T5, T6, T7, T8> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                try
                {
                    action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(Unit.Default).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(IAsyncObserver<Unit> observer, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(observer, action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(IAsyncObserver<Unit> observer, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                try
                {
                    action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(Unit.Default).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(IAsyncObserver<Unit> observer, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(observer, action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(IAsyncObserver<Unit> observer, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                try
                {
                    action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(Unit.Default).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(IAsyncObserver<Unit> observer, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(observer, action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(IAsyncObserver<Unit> observer, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                try
                {
                    action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(Unit.Default).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(IAsyncObserver<Unit> observer, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(observer, action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(IAsyncObserver<Unit> observer, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                try
                {
                    action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(Unit.Default).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(IAsyncObserver<Unit> observer, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(observer, action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(IAsyncObserver<Unit> observer, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                try
                {
                    action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(Unit.Default).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(IAsyncObserver<Unit> observer, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(observer, action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(IAsyncObserver<Unit> observer, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                try
                {
                    action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(Unit.Default).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(IAsyncObserver<Unit> observer, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(observer, action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(IAsyncObserver<Unit> observer, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                try
                {
                    action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(Unit.Default).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(IAsyncObserver<Unit> observer, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(observer, action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(IAsyncObserver<Unit> observer, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                try
                {
                    action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                    return;
                }

                await observer.OnNextAsync(Unit.Default).RendezVous(scheduler, ct);
                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }

    }
}
