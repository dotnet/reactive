// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Subjects;

namespace System.Reactive.Linq
{
    // REVIEW: Consider if these are worth retaining in the async space.

    partial class AsyncObservable
    {
        public static Func<IAsyncObservable<TResult>> FromAsyncPattern<TResult>(Func<AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return () =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                try
                {
                    AsyncObserver.FromAsyncPattern(subject, begin, end);
                }
                catch (Exception ex)
                {
                    return Throw<TResult>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, IAsyncObservable<TResult>> FromAsyncPattern<T1, TResult>(Func<T1, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return (arg1) =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                try
                {
                    AsyncObserver.FromAsyncPattern(subject, begin, end, arg1);
                }
                catch (Exception ex)
                {
                    return Throw<TResult>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, IAsyncObservable<TResult>> FromAsyncPattern<T1, T2, TResult>(Func<T1, T2, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return (arg1, arg2) =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                try
                {
                    AsyncObserver.FromAsyncPattern(subject, begin, end, arg1, arg2);
                }
                catch (Exception ex)
                {
                    return Throw<TResult>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, IAsyncObservable<TResult>> FromAsyncPattern<T1, T2, T3, TResult>(Func<T1, T2, T3, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return (arg1, arg2, arg3) =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                try
                {
                    AsyncObserver.FromAsyncPattern(subject, begin, end, arg1, arg2, arg3);
                }
                catch (Exception ex)
                {
                    return Throw<TResult>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, IAsyncObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return (arg1, arg2, arg3, arg4) =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                try
                {
                    AsyncObserver.FromAsyncPattern(subject, begin, end, arg1, arg2, arg3, arg4);
                }
                catch (Exception ex)
                {
                    return Throw<TResult>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, IAsyncObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return (arg1, arg2, arg3, arg4, arg5) =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                try
                {
                    AsyncObserver.FromAsyncPattern(subject, begin, end, arg1, arg2, arg3, arg4, arg5);
                }
                catch (Exception ex)
                {
                    return Throw<TResult>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, IAsyncObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return (arg1, arg2, arg3, arg4, arg5, arg6) =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                try
                {
                    AsyncObserver.FromAsyncPattern(subject, begin, end, arg1, arg2, arg3, arg4, arg5, arg6);
                }
                catch (Exception ex)
                {
                    return Throw<TResult>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, IAsyncObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                try
                {
                    AsyncObserver.FromAsyncPattern(subject, begin, end, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                }
                catch (Exception ex)
                {
                    return Throw<TResult>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, IAsyncObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                try
                {
                    AsyncObserver.FromAsyncPattern(subject, begin, end, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
                }
                catch (Exception ex)
                {
                    return Throw<TResult>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IAsyncObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                try
                {
                    AsyncObserver.FromAsyncPattern(subject, begin, end, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
                }
                catch (Exception ex)
                {
                    return Throw<TResult>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IAsyncObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                try
                {
                    AsyncObserver.FromAsyncPattern(subject, begin, end, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
                }
                catch (Exception ex)
                {
                    return Throw<TResult>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IAsyncObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                try
                {
                    AsyncObserver.FromAsyncPattern(subject, begin, end, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
                }
                catch (Exception ex)
                {
                    return Throw<TResult>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IAsyncObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                try
                {
                    AsyncObserver.FromAsyncPattern(subject, begin, end, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
                }
                catch (Exception ex)
                {
                    return Throw<TResult>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IAsyncObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                try
                {
                    AsyncObserver.FromAsyncPattern(subject, begin, end, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
                }
                catch (Exception ex)
                {
                    return Throw<TResult>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IAsyncObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            {
                var subject = new SequentialAsyncAsyncSubject<TResult>();

                try
                {
                    AsyncObserver.FromAsyncPattern(subject, begin, end, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
                }
                catch (Exception ex)
                {
                    return Throw<TResult>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

        public static Func<IAsyncObservable<Unit>> FromAsyncPattern(Func<AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, Unit> end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return () =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                try
                {
                    AsyncObserver.FromAsyncPattern(subject, begin, end);
                }
                catch (Exception ex)
                {
                    return Throw<Unit>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, IAsyncObservable<Unit>> FromAsyncPattern<T1>(Func<T1, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, Unit> end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return (arg1) =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                try
                {
                    AsyncObserver.FromAsyncPattern(subject, begin, end, arg1);
                }
                catch (Exception ex)
                {
                    return Throw<Unit>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, IAsyncObservable<Unit>> FromAsyncPattern<T1, T2>(Func<T1, T2, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, Unit> end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return (arg1, arg2) =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                try
                {
                    AsyncObserver.FromAsyncPattern(subject, begin, end, arg1, arg2);
                }
                catch (Exception ex)
                {
                    return Throw<Unit>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, IAsyncObservable<Unit>> FromAsyncPattern<T1, T2, T3>(Func<T1, T2, T3, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, Unit> end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return (arg1, arg2, arg3) =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                try
                {
                    AsyncObserver.FromAsyncPattern(subject, begin, end, arg1, arg2, arg3);
                }
                catch (Exception ex)
                {
                    return Throw<Unit>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, IAsyncObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4>(Func<T1, T2, T3, T4, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, Unit> end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return (arg1, arg2, arg3, arg4) =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                try
                {
                    AsyncObserver.FromAsyncPattern(subject, begin, end, arg1, arg2, arg3, arg4);
                }
                catch (Exception ex)
                {
                    return Throw<Unit>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, IAsyncObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, Unit> end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return (arg1, arg2, arg3, arg4, arg5) =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                try
                {
                    AsyncObserver.FromAsyncPattern(subject, begin, end, arg1, arg2, arg3, arg4, arg5);
                }
                catch (Exception ex)
                {
                    return Throw<Unit>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, IAsyncObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, Unit> end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return (arg1, arg2, arg3, arg4, arg5, arg6) =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                try
                {
                    AsyncObserver.FromAsyncPattern(subject, begin, end, arg1, arg2, arg3, arg4, arg5, arg6);
                }
                catch (Exception ex)
                {
                    return Throw<Unit>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, IAsyncObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, Unit> end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                try
                {
                    AsyncObserver.FromAsyncPattern(subject, begin, end, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                }
                catch (Exception ex)
                {
                    return Throw<Unit>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, IAsyncObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T1, T2, T3, T4, T5, T6, T7, T8, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, Unit> end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                try
                {
                    AsyncObserver.FromAsyncPattern(subject, begin, end, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
                }
                catch (Exception ex)
                {
                    return Throw<Unit>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IAsyncObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, Unit> end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                try
                {
                    AsyncObserver.FromAsyncPattern(subject, begin, end, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
                }
                catch (Exception ex)
                {
                    return Throw<Unit>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IAsyncObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, Unit> end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                try
                {
                    AsyncObserver.FromAsyncPattern(subject, begin, end, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
                }
                catch (Exception ex)
                {
                    return Throw<Unit>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IAsyncObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, Unit> end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                try
                {
                    AsyncObserver.FromAsyncPattern(subject, begin, end, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
                }
                catch (Exception ex)
                {
                    return Throw<Unit>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IAsyncObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, Unit> end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                try
                {
                    AsyncObserver.FromAsyncPattern(subject, begin, end, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
                }
                catch (Exception ex)
                {
                    return Throw<Unit>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IAsyncObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, Unit> end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                try
                {
                    AsyncObserver.FromAsyncPattern(subject, begin, end, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
                }
                catch (Exception ex)
                {
                    return Throw<Unit>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IAsyncObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, Unit> end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            {
                var subject = new SequentialAsyncAsyncSubject<Unit>();

                try
                {
                    AsyncObserver.FromAsyncPattern(subject, begin, end, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
                }
                catch (Exception ex)
                {
                    return Throw<Unit>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

    }

    partial class AsyncObserver
    {
        public static IAsyncResult FromAsyncPattern<TResult>(IAsyncObserver<TResult> observer, Func<AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return begin(async iar =>
            {
                TResult result;

                try
                {
                    result = end(iar);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    return;
                }

                await observer.OnNextAsync(result).ConfigureAwait(false);
                await observer.OnCompletedAsync().ConfigureAwait(false);
            }, null);
        }

        public static IAsyncResult FromAsyncPattern<T1, TResult>(IAsyncObserver<TResult> observer, Func<T1, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end, T1 arg1)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return begin(arg1, async iar =>
            {
                TResult result;

                try
                {
                    result = end(iar);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    return;
                }

                await observer.OnNextAsync(result).ConfigureAwait(false);
                await observer.OnCompletedAsync().ConfigureAwait(false);
            }, null);
        }

        public static IAsyncResult FromAsyncPattern<T1, T2, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end, T1 arg1, T2 arg2)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return begin(arg1, arg2, async iar =>
            {
                TResult result;

                try
                {
                    result = end(iar);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    return;
                }

                await observer.OnNextAsync(result).ConfigureAwait(false);
                await observer.OnCompletedAsync().ConfigureAwait(false);
            }, null);
        }

        public static IAsyncResult FromAsyncPattern<T1, T2, T3, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end, T1 arg1, T2 arg2, T3 arg3)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return begin(arg1, arg2, arg3, async iar =>
            {
                TResult result;

                try
                {
                    result = end(iar);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    return;
                }

                await observer.OnNextAsync(result).ConfigureAwait(false);
                await observer.OnCompletedAsync().ConfigureAwait(false);
            }, null);
        }

        public static IAsyncResult FromAsyncPattern<T1, T2, T3, T4, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return begin(arg1, arg2, arg3, arg4, async iar =>
            {
                TResult result;

                try
                {
                    result = end(iar);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    return;
                }

                await observer.OnNextAsync(result).ConfigureAwait(false);
                await observer.OnCompletedAsync().ConfigureAwait(false);
            }, null);
        }

        public static IAsyncResult FromAsyncPattern<T1, T2, T3, T4, T5, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return begin(arg1, arg2, arg3, arg4, arg5, async iar =>
            {
                TResult result;

                try
                {
                    result = end(iar);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    return;
                }

                await observer.OnNextAsync(result).ConfigureAwait(false);
                await observer.OnCompletedAsync().ConfigureAwait(false);
            }, null);
        }

        public static IAsyncResult FromAsyncPattern<T1, T2, T3, T4, T5, T6, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return begin(arg1, arg2, arg3, arg4, arg5, arg6, async iar =>
            {
                TResult result;

                try
                {
                    result = end(iar);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    return;
                }

                await observer.OnNextAsync(result).ConfigureAwait(false);
                await observer.OnCompletedAsync().ConfigureAwait(false);
            }, null);
        }

        public static IAsyncResult FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return begin(arg1, arg2, arg3, arg4, arg5, arg6, arg7, async iar =>
            {
                TResult result;

                try
                {
                    result = end(iar);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    return;
                }

                await observer.OnNextAsync(result).ConfigureAwait(false);
                await observer.OnCompletedAsync().ConfigureAwait(false);
            }, null);
        }

        public static IAsyncResult FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return begin(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, async iar =>
            {
                TResult result;

                try
                {
                    result = end(iar);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    return;
                }

                await observer.OnNextAsync(result).ConfigureAwait(false);
                await observer.OnCompletedAsync().ConfigureAwait(false);
            }, null);
        }

        public static IAsyncResult FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return begin(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, async iar =>
            {
                TResult result;

                try
                {
                    result = end(iar);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    return;
                }

                await observer.OnNextAsync(result).ConfigureAwait(false);
                await observer.OnCompletedAsync().ConfigureAwait(false);
            }, null);
        }

        public static IAsyncResult FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return begin(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, async iar =>
            {
                TResult result;

                try
                {
                    result = end(iar);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    return;
                }

                await observer.OnNextAsync(result).ConfigureAwait(false);
                await observer.OnCompletedAsync().ConfigureAwait(false);
            }, null);
        }

        public static IAsyncResult FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return begin(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, async iar =>
            {
                TResult result;

                try
                {
                    result = end(iar);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    return;
                }

                await observer.OnNextAsync(result).ConfigureAwait(false);
                await observer.OnCompletedAsync().ConfigureAwait(false);
            }, null);
        }

        public static IAsyncResult FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return begin(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, async iar =>
            {
                TResult result;

                try
                {
                    result = end(iar);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    return;
                }

                await observer.OnNextAsync(result).ConfigureAwait(false);
                await observer.OnCompletedAsync().ConfigureAwait(false);
            }, null);
        }

        public static IAsyncResult FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return begin(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, async iar =>
            {
                TResult result;

                try
                {
                    result = end(iar);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    return;
                }

                await observer.OnNextAsync(result).ConfigureAwait(false);
                await observer.OnCompletedAsync().ConfigureAwait(false);
            }, null);
        }

        public static IAsyncResult FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(IAsyncObserver<TResult> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return begin(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, async iar =>
            {
                TResult result;

                try
                {
                    result = end(iar);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    return;
                }

                await observer.OnNextAsync(result).ConfigureAwait(false);
                await observer.OnCompletedAsync().ConfigureAwait(false);
            }, null);
        }

        public static IAsyncResult FromAsyncPattern(IAsyncObserver<Unit> observer, Func<AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, Unit> end)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return FromAsyncPattern(observer, begin, iar =>
            {
                end(iar);
                return Unit.Default;
            });
        }

        public static IAsyncResult FromAsyncPattern<T1>(IAsyncObserver<Unit> observer, Func<T1, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, Unit> end, T1 arg1)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return FromAsyncPattern(observer, begin, iar =>
            {
                end(iar);
                return Unit.Default;
            }, arg1);
        }

        public static IAsyncResult FromAsyncPattern<T1, T2>(IAsyncObserver<Unit> observer, Func<T1, T2, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, Unit> end, T1 arg1, T2 arg2)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return FromAsyncPattern(observer, begin, iar =>
            {
                end(iar);
                return Unit.Default;
            }, arg1, arg2);
        }

        public static IAsyncResult FromAsyncPattern<T1, T2, T3>(IAsyncObserver<Unit> observer, Func<T1, T2, T3, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, Unit> end, T1 arg1, T2 arg2, T3 arg3)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return FromAsyncPattern(observer, begin, iar =>
            {
                end(iar);
                return Unit.Default;
            }, arg1, arg2, arg3);
        }

        public static IAsyncResult FromAsyncPattern<T1, T2, T3, T4>(IAsyncObserver<Unit> observer, Func<T1, T2, T3, T4, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, Unit> end, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return FromAsyncPattern(observer, begin, iar =>
            {
                end(iar);
                return Unit.Default;
            }, arg1, arg2, arg3, arg4);
        }

        public static IAsyncResult FromAsyncPattern<T1, T2, T3, T4, T5>(IAsyncObserver<Unit> observer, Func<T1, T2, T3, T4, T5, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, Unit> end, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return FromAsyncPattern(observer, begin, iar =>
            {
                end(iar);
                return Unit.Default;
            }, arg1, arg2, arg3, arg4, arg5);
        }

        public static IAsyncResult FromAsyncPattern<T1, T2, T3, T4, T5, T6>(IAsyncObserver<Unit> observer, Func<T1, T2, T3, T4, T5, T6, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, Unit> end, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return FromAsyncPattern(observer, begin, iar =>
            {
                end(iar);
                return Unit.Default;
            }, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        public static IAsyncResult FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7>(IAsyncObserver<Unit> observer, Func<T1, T2, T3, T4, T5, T6, T7, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, Unit> end, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return FromAsyncPattern(observer, begin, iar =>
            {
                end(iar);
                return Unit.Default;
            }, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        public static IAsyncResult FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8>(IAsyncObserver<Unit> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, Unit> end, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return FromAsyncPattern(observer, begin, iar =>
            {
                end(iar);
                return Unit.Default;
            }, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        public static IAsyncResult FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9>(IAsyncObserver<Unit> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, Unit> end, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return FromAsyncPattern(observer, begin, iar =>
            {
                end(iar);
                return Unit.Default;
            }, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        public static IAsyncResult FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(IAsyncObserver<Unit> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, Unit> end, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return FromAsyncPattern(observer, begin, iar =>
            {
                end(iar);
                return Unit.Default;
            }, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        public static IAsyncResult FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(IAsyncObserver<Unit> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, Unit> end, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return FromAsyncPattern(observer, begin, iar =>
            {
                end(iar);
                return Unit.Default;
            }, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
        }

        public static IAsyncResult FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(IAsyncObserver<Unit> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, Unit> end, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return FromAsyncPattern(observer, begin, iar =>
            {
                end(iar);
                return Unit.Default;
            }, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
        }

        public static IAsyncResult FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(IAsyncObserver<Unit> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, Unit> end, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return FromAsyncPattern(observer, begin, iar =>
            {
                end(iar);
                return Unit.Default;
            }, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
        }

        public static IAsyncResult FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(IAsyncObserver<Unit> observer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, Unit> end, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            return FromAsyncPattern(observer, begin, iar =>
            {
                end(iar);
                return Unit.Default;
            }, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
        }

    }
}
