// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ReactiveTests
{
#if SILVERLIGHT && !SILVERLIGHTM7
    public class TestBase : Microsoft.Silverlight.Testing.SilverlightTest
    {
        public void RunAsync(Action<Waiter> a)
        {
            EnqueueCallback(() =>
            {
                var w = new Waiter(TestComplete);
                a(w);
                w.Wait();
            });
        }

        public void CompleteAsync()
        {
            EnqueueTestComplete();
        }
    }

    public class Waiter
    {
        private Action _complete;

        public Waiter(Action complete)
        {
            _complete = complete;
        }

        public void Set()
        {
            _complete();
        }

        public void Wait()
        {
        }
    }
#else
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
#endif
}
