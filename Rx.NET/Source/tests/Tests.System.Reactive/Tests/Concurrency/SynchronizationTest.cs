// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive.Concurrency;
using System.Threading;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class SynchronizationTests
    {
        [TestMethod]
        public void Synchronization_SubscribeOn_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Synchronization.SubscribeOn(default(IObservable<int>), Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Synchronization.SubscribeOn(DummyObservable<int>.Instance, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Synchronization.SubscribeOn(default(IObservable<int>), new MySyncCtx()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Synchronization.SubscribeOn(DummyObservable<int>.Instance, default(SynchronizationContext)));
        }

        [TestMethod]
        public void Synchronization_ObserveOn_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Synchronization.ObserveOn(default(IObservable<int>), Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Synchronization.ObserveOn(DummyObservable<int>.Instance, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Synchronization.ObserveOn(default(IObservable<int>), new MySyncCtx()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Synchronization.ObserveOn(DummyObservable<int>.Instance, default(SynchronizationContext)));
        }

        [TestMethod]
        public void Synchronization_Synchronize_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Synchronization.Synchronize(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Synchronization.Synchronize(default(IObservable<int>), new object()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Synchronization.Synchronize(DummyObservable<int>.Instance, null));
        }

        private class MySyncCtx : SynchronizationContext
        {
        }
    }
}
