// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 
using System.Collections.Generic;

namespace System.Threading.Tasks
{
    static class TaskExt
    {
        public static readonly Task<bool> True = Task.FromResult(true);
        public static readonly Task<bool> False = Task.FromResult(false);
        
        public static Task<T> Throw<T>(Exception exception)
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.TrySetException(exception);
            return tcs.Task;
        }

        public static Task<V> Zip<T, U, V>(this Task<T> t1, Task<U> t2, Func<T, U, V> f)
        {
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
