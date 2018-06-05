// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;
using ReactiveTests.Dummies;
using System.Reflection;
using System.Threading;

namespace ReactiveTests.Tests
{
    public class SynchronizeTest : TestBase
    {
        [Fact]
        public void Synchronize_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Synchronize<int>(default(IObservable<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Synchronize<int>(default(IObservable<int>), new object()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Synchronize<int>(someObservable, null));
        }

#if !NO_THREAD
        [Fact]
        public void Synchronize_Range()
        {
            int i = 0;
            bool outsideLock = true;

            var gate = new object();
            lock (gate)
            {
                outsideLock = false;
                Observable.Range(0, 100, NewThreadScheduler.Default).Synchronize(gate).Subscribe(x => i++, () => { Assert.True(outsideLock); });
                Thread.Sleep(100);
                Assert.Equal(0, i);
                outsideLock = true;
            }

            while (i < 100)
            {
                Thread.Sleep(10);
                lock (gate)
                {
                    int start = i;
                    Thread.Sleep(100);
                    Assert.Equal(start, i);
                }
            }
        }

        [Fact]
        public void Synchronize_Throw()
        {
            var ex = new Exception();
            var resLock = new object();
            var e = default(Exception);
            bool outsideLock = true;

            var gate = new object();
            lock (gate)
            {
                outsideLock = false;
                Observable.Throw<int>(ex, NewThreadScheduler.Default).Synchronize(gate).Subscribe(x => { Assert.True(false); }, err => { lock (resLock) { e = err; } }, () => { Assert.True(outsideLock); });
                Thread.Sleep(100);
                Assert.Null(e);
                outsideLock = true;
            }

            while (true)
            {
                lock (resLock)
                {
                    if (e != null)
                        break;
                }
            }

            Assert.Same(ex, e);
        }

        [Fact]
        public void Synchronize_BadObservable()
        {
            var o = Observable.Create<int>(obs =>
            {
                var t1 = new Thread(() =>
                {
                    for (int i = 0; i < 100; i++)
                    {
                        obs.OnNext(i);
                    }
                });

                new Thread(() =>
                {
                    t1.Start();

                    for (int i = 100; i < 200; i++)
                    {
                        obs.OnNext(i);
                    }

                    t1.Join();
                    obs.OnCompleted();
                }).Start();

                return () => { };
            });

            var evt = new ManualResetEvent(false);

            int sum = 0;
            o.Synchronize().Subscribe(x => sum += x, () => { evt.Set(); });

            evt.WaitOne();

            Assert.Equal(Enumerable.Range(0, 200).Sum(), sum);
        }
#endif

    }
}
