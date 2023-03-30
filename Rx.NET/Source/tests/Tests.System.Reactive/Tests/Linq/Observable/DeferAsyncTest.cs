// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Assert = Xunit.Assert;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class DeferAsyncTest : ReactiveTest
    {

        [TestMethod]
        public void DeferAsync_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Defer(default(Func<Task<IObservable<int>>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DeferAsync(default(Func<CancellationToken, Task<IObservable<int>>>)));
        }

        [TestMethod]
        public void DeferAsync_Simple()
        {
            var xs = Observable.Defer(() => Task.Factory.StartNew(() => Observable.Return(42)));

            var res = xs.ToEnumerable().ToList();

            Assert.True(new[] { 42 }.SequenceEqual(res));
        }

        [TestMethod]
        public void DeferAsync_WithCancel_Simple()
        {
            var xs = Observable.DeferAsync(ct => Task.Factory.StartNew(() => Observable.Return(42)));

            var res = xs.ToEnumerable().ToList();

            Assert.True(new[] { 42 }.SequenceEqual(res));
        }

        [TestMethod]
        public void DeferAsync_WithCancel_Cancel()
        {
            var N = 10;// 0000;
            for (var i = 0; i < N; i++)
            {
                var e = new ManualResetEvent(false);
                var called = false;

                var xs = Observable.DeferAsync(ct => Task.Factory.StartNew(() =>
                {
                    e.Set();

                    while (!ct.IsCancellationRequested)
                    {
                        ;
                    }

                    return Observable.Defer(() => { called = true; return Observable.Return(42); });
                }));

                var d = xs.Subscribe(_ => { });

                e.WaitOne();
                d.Dispose();

                Assert.False(called);
            }
        }

    }
}
