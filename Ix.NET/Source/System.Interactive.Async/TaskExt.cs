// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 
using System;
using System.Collections.Generic;
using System.Linq;

namespace System.Threading.Tasks
{
    static class TaskExt
    {
        public static readonly Task<bool> True;
        public static readonly Task<bool> False;

        static TaskExt()
        {
            True = Return(true);
            False = Return(false);
        }

        public static Task<T> Return<T>(T value)
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.TrySetResult(value);
            return tcs.Task;
        }

        public static Task<T> Throw<T>(Exception exception)
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.TrySetException(exception);
            return tcs.Task;
        }

        public static void Handle<T, R>(this Task<T> task, TaskCompletionSource<R> tcs, Action<T> success)
        {
            if (task.IsFaulted)
                tcs.TrySetException(task.Exception.InnerExceptions);
            else if (task.IsCanceled)
                tcs.TrySetCanceled();
            else if (task.IsCompleted)
                success(task.Result);
        }

        public static void Handle<T, R>(this Task<T> task, TaskCompletionSource<R> tcs, Action<T> success, Action<AggregateException> error)
        {
            if (task.IsFaulted)
                error(task.Exception);
            else if (task.IsCanceled)
                tcs.TrySetCanceled();
            else if (task.IsCompleted)
                success(task.Result);
        }

        public static void Handle<T, R>(this Task<T> task, TaskCompletionSource<R> tcs, Action<T> success, Action<AggregateException> error, Action canceled)
        {
            if (task.IsFaulted)
                error(task.Exception);
            else if (task.IsCanceled)
                canceled();
            else if (task.IsCompleted)
                success(task.Result);
        }

        public static Task Then<T>(this Task<T> task, Action<Task<T>> continuation)
        {
            //
            // Central location to deal with continuations; allows for experimentation with flags.
            // Note that right now, we don't go for synchronous execution. Users can block on the
            // task returned from MoveNext, which can cause deadlocks (e.g. typical uses of GroupBy
            // involve some aggregate). We'd need deeper asynchrony to make this work with less
            // spawning of tasks.
            //
            return task.ContinueWith(continuation);
        }

        public static Task<R> Then<T, R>(this Task<T> task, Func<Task<T>, R> continuation)
        {
            //
            // See comment on Then<T> for rationale.
            //
            return task.ContinueWith(continuation);
        }

        public static Task<bool> UsingEnumerator(this Task<bool> task, IDisposable disposable)
        {
            return task.Finally(() =>
            {
                if (task.IsFaulted || task.IsCanceled || !task.Result)
                    disposable.Dispose();
            });
        }

        public static Task<R> Finally<R>(this Task<R> task, Action action)
        {
            var tcs = new TaskCompletionSource<R>();

            task.ContinueWith(t =>
            {
                try
                {
                    action();
                }
                finally
                {
                    switch (t.Status)
                    {
                        case TaskStatus.Canceled:
                            tcs.SetCanceled();
                            break;
                        case TaskStatus.Faulted:
                            tcs.SetException(t.Exception.InnerException);
                            break;
                        case TaskStatus.RanToCompletion:
                            tcs.SetResult(t.Result);
                            break;
                    }
                }
            }, TaskContinuationOptions.ExecuteSynchronously);

            return tcs.Task;
        }

        public static Task<V> Zip<T, U, V>(this Task<T> t1, Task<U> t2, Func<T, U, V> f)
        {
            var gate = new object();
            var tcs = new TaskCompletionSource<V>();

            var i = 2;
            var complete = new Action<Task>(t =>
            {
                if (Interlocked.Decrement(ref i) == 0)
                {
                    var exs = new List<Exception>();
                    if (t1.IsFaulted)
                        exs.Add(t1.Exception);
                    if (t2.IsFaulted)
                        exs.Add(t2.Exception);

                    if (exs.Count > 0)
                        tcs.TrySetException(exs);
                    else if (t1.IsCanceled || t2.IsCanceled)
                        tcs.TrySetCanceled();
                    else
                    {
                        var res = default(V);
                        try
                        {
                            res = f(t1.Result, t2.Result);
                        }
                        catch (Exception ex)
                        {
                            tcs.TrySetException(ex);
                            return;
                        }

                        tcs.TrySetResult(res);
                    }
                }
            });

            t1.ContinueWith(complete);
            t2.ContinueWith(complete);

            return tcs.Task;
        }
    }
}
