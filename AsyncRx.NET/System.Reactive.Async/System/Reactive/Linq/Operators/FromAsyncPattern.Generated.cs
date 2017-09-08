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
                    begin(async iar =>
                    {
                        TResult result;

                        try
                        {
                            result = end(iar);
                        }
                        catch (Exception ex)
                        {
                            await subject.OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }

                        await subject.OnNextAsync(result).ConfigureAwait(false);
                        await subject.OnCompletedAsync().ConfigureAwait(false);
                    }, null);
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
                    begin(arg1, async iar =>
                    {
                        TResult result;

                        try
                        {
                            result = end(iar);
                        }
                        catch (Exception ex)
                        {
                            await subject.OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }

                        await subject.OnNextAsync(result).ConfigureAwait(false);
                        await subject.OnCompletedAsync().ConfigureAwait(false);
                    }, null);
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
                    begin(arg1, arg2, async iar =>
                    {
                        TResult result;

                        try
                        {
                            result = end(iar);
                        }
                        catch (Exception ex)
                        {
                            await subject.OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }

                        await subject.OnNextAsync(result).ConfigureAwait(false);
                        await subject.OnCompletedAsync().ConfigureAwait(false);
                    }, null);
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
                    begin(arg1, arg2, arg3, async iar =>
                    {
                        TResult result;

                        try
                        {
                            result = end(iar);
                        }
                        catch (Exception ex)
                        {
                            await subject.OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }

                        await subject.OnNextAsync(result).ConfigureAwait(false);
                        await subject.OnCompletedAsync().ConfigureAwait(false);
                    }, null);
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
                    begin(arg1, arg2, arg3, arg4, async iar =>
                    {
                        TResult result;

                        try
                        {
                            result = end(iar);
                        }
                        catch (Exception ex)
                        {
                            await subject.OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }

                        await subject.OnNextAsync(result).ConfigureAwait(false);
                        await subject.OnCompletedAsync().ConfigureAwait(false);
                    }, null);
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
                    begin(arg1, arg2, arg3, arg4, arg5, async iar =>
                    {
                        TResult result;

                        try
                        {
                            result = end(iar);
                        }
                        catch (Exception ex)
                        {
                            await subject.OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }

                        await subject.OnNextAsync(result).ConfigureAwait(false);
                        await subject.OnCompletedAsync().ConfigureAwait(false);
                    }, null);
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
                    begin(arg1, arg2, arg3, arg4, arg5, arg6, async iar =>
                    {
                        TResult result;

                        try
                        {
                            result = end(iar);
                        }
                        catch (Exception ex)
                        {
                            await subject.OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }

                        await subject.OnNextAsync(result).ConfigureAwait(false);
                        await subject.OnCompletedAsync().ConfigureAwait(false);
                    }, null);
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
                    begin(arg1, arg2, arg3, arg4, arg5, arg6, arg7, async iar =>
                    {
                        TResult result;

                        try
                        {
                            result = end(iar);
                        }
                        catch (Exception ex)
                        {
                            await subject.OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }

                        await subject.OnNextAsync(result).ConfigureAwait(false);
                        await subject.OnCompletedAsync().ConfigureAwait(false);
                    }, null);
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
                    begin(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, async iar =>
                    {
                        TResult result;

                        try
                        {
                            result = end(iar);
                        }
                        catch (Exception ex)
                        {
                            await subject.OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }

                        await subject.OnNextAsync(result).ConfigureAwait(false);
                        await subject.OnCompletedAsync().ConfigureAwait(false);
                    }, null);
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
                    begin(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, async iar =>
                    {
                        TResult result;

                        try
                        {
                            result = end(iar);
                        }
                        catch (Exception ex)
                        {
                            await subject.OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }

                        await subject.OnNextAsync(result).ConfigureAwait(false);
                        await subject.OnCompletedAsync().ConfigureAwait(false);
                    }, null);
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
                    begin(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, async iar =>
                    {
                        TResult result;

                        try
                        {
                            result = end(iar);
                        }
                        catch (Exception ex)
                        {
                            await subject.OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }

                        await subject.OnNextAsync(result).ConfigureAwait(false);
                        await subject.OnCompletedAsync().ConfigureAwait(false);
                    }, null);
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
                    begin(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, async iar =>
                    {
                        TResult result;

                        try
                        {
                            result = end(iar);
                        }
                        catch (Exception ex)
                        {
                            await subject.OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }

                        await subject.OnNextAsync(result).ConfigureAwait(false);
                        await subject.OnCompletedAsync().ConfigureAwait(false);
                    }, null);
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
                    begin(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, async iar =>
                    {
                        TResult result;

                        try
                        {
                            result = end(iar);
                        }
                        catch (Exception ex)
                        {
                            await subject.OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }

                        await subject.OnNextAsync(result).ConfigureAwait(false);
                        await subject.OnCompletedAsync().ConfigureAwait(false);
                    }, null);
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
                    begin(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, async iar =>
                    {
                        TResult result;

                        try
                        {
                            result = end(iar);
                        }
                        catch (Exception ex)
                        {
                            await subject.OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }

                        await subject.OnNextAsync(result).ConfigureAwait(false);
                        await subject.OnCompletedAsync().ConfigureAwait(false);
                    }, null);
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
                    begin(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, async iar =>
                    {
                        TResult result;

                        try
                        {
                            result = end(iar);
                        }
                        catch (Exception ex)
                        {
                            await subject.OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }

                        await subject.OnNextAsync(result).ConfigureAwait(false);
                        await subject.OnCompletedAsync().ConfigureAwait(false);
                    }, null);
                }
                catch (Exception ex)
                {
                    return Throw<TResult>(ex);
                }

                return subject.AsAsyncObservable();
            };
        }

    }
}
