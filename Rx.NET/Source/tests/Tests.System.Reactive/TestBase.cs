// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Threading;

namespace ReactiveTests
{
    public class TestBase
    {
        public void RunAsync(Action<Waiter> a)
        {
            var w = new Waiter();
            a(w);
            w.Wait();
        }
    }

    public class Waiter
    {
        private ManualResetEvent _evt = new ManualResetEvent(false);

        public void Set()
        {
            _evt.Set();
        }

        public void Wait()
        {
            _evt.WaitOne();
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class AsynchronousAttribute : Attribute
    {
    }
}
