// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class FirstOrDefaultTest : ReactiveTest
    {

        [Fact]
        public void FirstOrDefault_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FirstOrDefault(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FirstOrDefault(default(IObservable<int>), _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FirstOrDefault(DummyObservable<int>.Instance, default));
        }

        [Fact]
        public void FirstOrDefault_Empty()
        {
            Assert.Equal(default, Observable.Empty<int>().FirstOrDefault());
        }

        [Fact]
        public void FirstOrDefaultPredicate_Empty()
        {
            Assert.Equal(default, Observable.Empty<int>().FirstOrDefault(_ => true));
        }

        [Fact]
        public void FirstOrDefault_Return()
        {
            var value = 42;
            Assert.Equal(value, Observable.Return(value).FirstOrDefault());
        }

        [Fact]
        public void FirstOrDefault_Throw()
        {
            var ex = new Exception();

            var xs = Observable.Throw<int>(ex);

            ReactiveAssert.Throws(ex, () => xs.FirstOrDefault());
        }

        [Fact]
        public void FirstOrDefault_Range()
        {
            var value = 42;
            Assert.Equal(value, Observable.Range(value, 10).FirstOrDefault());
        }

#if !NO_THREAD
        [Fact]
        public void FirstOrDefault_NoDoubleSet()
        {
            //
            // Regression test for a possible race condition caused by Return style operators
            // that could trigger two Set calls on a ManualResetEvent, causing it to get
            // disposed in between those two calls (cf. FirstOrDefaultInternal). This led
            // to an exception will the following stack trace:
            //
            //    System.ObjectDisposedException: Safe handle has been closed
            //       at System.Runtime.InteropServices.SafeHandle.DangerousAddRef(Boolean& success)
            //       at System.StubHelpers.StubHelpers.SafeHandleAddRef(SafeHandle pHandle, Boolean& success)
            //       at Microsoft.Win32.Win32Native.SetEvent(SafeWaitHandle handle)
            //       at System.Threading.EventWaitHandle.Set()
            //       at System.Reactive.Linq.QueryLanguage.<>c__DisplayClass458_1`1.<FirstOrDefaultInternal>b__2()
            //

            var o = new O();

            Scheduler.Default.Schedule(() =>
            {
                var x = o.FirstOrDefault();
            });

            o.Wait();

            o.Next();

            Thread.Sleep(100); // enough time to let the ManualResetEvent dispose

            o.Done();
        }
#endif

        private class O : IObservable<int>
        {
            private readonly ManualResetEvent _event = new ManualResetEvent(false);
            private IObserver<int> _observer;

            public void Wait()
            {
                _event.WaitOne();
            }

            public void Next()
            {
                _observer.OnNext(42);
            }

            public void Done()
            {
                _observer.OnCompleted();
            }

            public IDisposable Subscribe(IObserver<int> observer)
            {
                _observer = observer;
                _event.Set();
                return Disposable.Empty;
            }
        }

    }
}
