// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Linq;

namespace System.Threading.Tasks
{
    static class TaskExt
    {
        public static Task<T> Return<T>(T value, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.TrySetResult(value);
            return tcs.Task;
        }

        public static Task<T> Throw<T>(Exception exception, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.TrySetException(exception);
            return tcs.Task;
        }

        public static void Handle<T, R>(this Task<T> task, TaskCompletionSource<R> tcs, Action<T> success)
        {
            Handle(task, tcs, success, ex => tcs.TrySetException(ex), () => tcs.TrySetCanceled());
        }

        public static void Handle<T, R>(this Task<T> task, TaskCompletionSource<R> tcs, Action<T> success, Action<AggregateException> error)
        {
            Handle(task, tcs, success, error, () => tcs.TrySetCanceled());
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

        public static Task<bool> UsingEnumerator(this Task<bool> task, IDisposable disposable)
        {
            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    var ignored = t.Exception; // don't remove!
                }

                if (t.IsFaulted || t.IsCanceled || !t.Result)
                    disposable.Dispose();
            }, TaskContinuationOptions.ExecuteSynchronously);

            return task;
        }

        public static Task<bool> UsingEnumeratorSync(this Task<bool> task, IDisposable disposable)
        {
            var tcs = new TaskCompletionSource<bool>();

            task.ContinueWith(t =>
            {
                if (t.IsFaulted || t.IsCanceled || !t.Result)
                    disposable.Dispose(); // TODO: Check whether we need exception handling here!

                t.Handle(tcs, res => tcs.TrySetResult(res));
            }, TaskContinuationOptions.ExecuteSynchronously);

            return tcs.Task;
        }

        public static Task<R> Finally<R>(this Task<R> task, Action action)
        {
            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    var ignored = t.Exception; // don't remove!
                }

                action();
            }, TaskContinuationOptions.ExecuteSynchronously);

            return task;
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
