// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Reactive.Concurrency;
using System.Threading;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReactiveTests.Dummies;

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

#if !NO_SYNCCTX
            ReactiveAssert.Throws<ArgumentNullException>(() => Synchronization.SubscribeOn(default(IObservable<int>), new MySyncCtx()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Synchronization.SubscribeOn(DummyObservable<int>.Instance, default(SynchronizationContext)));
#endif
        }

        [TestMethod]
        public void Synchronization_ObserveOn_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Synchronization.ObserveOn(default(IObservable<int>), Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Synchronization.ObserveOn(DummyObservable<int>.Instance, default(IScheduler)));

#if !NO_SYNCCTX
            ReactiveAssert.Throws<ArgumentNullException>(() => Synchronization.ObserveOn(default(IObservable<int>), new MySyncCtx()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Synchronization.ObserveOn(DummyObservable<int>.Instance, default(SynchronizationContext)));
#endif
        }

        [TestMethod]
        public void Synchronization_Synchronize_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Synchronization.Synchronize(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Synchronization.Synchronize(default(IObservable<int>), new object()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Synchronization.Synchronize(DummyObservable<int>.Instance, null));
        }

        class MySyncCtx : SynchronizationContext
        {
        }
    }
}
