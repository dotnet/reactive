using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Assert = Xunit.Assert;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class SynchronizeTest
    {
            [TestMethod]
            public void Synchronize_WithObject_WhenArgumentsAreNull_ThrowsException()
            {
                ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Synchronize<int>(source: null!));

                ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Synchronize<int>(
                    source: null!,
                    gate:   new object()));

                ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Synchronize(
                    source: Observable.Empty<int>(),
                    gate:   (null as object)!));
            }

            [TestMethod]
            public void Synchronize_WithObject_WhenNotificationsAreSynchronized_PropagatesNotifications()
            {
                using var source = new Subject<int>();

                var observer = new TestObserver<int>();

                using var subscription = source
                    .Synchronize()
                    .Subscribe(observer);

                source.OnNext(1);
                source.OnNext(2);
                source.OnNext(3);
                source.OnCompleted();

                Assert.Equal(observer.ObservedValues, new[] { 1, 2, 3 });
                Assert.Equal(observer.ObservedCompletionCount, 1);
            }

            [TestMethod]
            [Timeout(10000)]
            public async Task Synchronize_WithObject_WhenGateIsLocked_BlocksNotification()
            {
                using var source = new Subject<int>();

                var gate = new object();

                var observer = new TestObserver<int>();

                using var subscription = source
                    .Synchronize(gate: gate)
                    .Subscribe(observer);

                var whenHostReady       = new ManualResetEventSlim();
                var whenProducerRunning = new ManualResetEventSlim();

                var whenProducerFinished = Task.Run(() =>
                {
                    whenHostReady.Wait();

                    // 3) Try to produce a value, which should block on the gate
                    whenProducerRunning.Set();
                    source.OnNext(1);
                });

                // 1) Lock the gate, so the producer gets blocked when producing a value
                lock (gate)
                {
                    whenHostReady.Set();

                    // 2) Wait for the producer to actually run, and get blocked
                    whenProducerRunning.Wait();
                    Thread.Yield();

                    // 4) Check that the value hasn't been produced
                    Assert.Empty(observer.ObservedValues);
                    Assert.Empty(observer.ObservedErrors);
                }

                await whenProducerFinished;

                // 5) Check that the second value has been produced
                Assert.Equal(observer.ObservedValues, new[] { 1 });
                Assert.Empty(observer.ObservedErrors);
                Assert.Equal(observer.ObservedCompletionCount, 0);
            }

            [TestMethod]
            [Timeout(10000)]
            public async Task Synchronize_WithObject_WhenPublishingNotification_LocksGate()
            {
                using var source = new Subject<int>();

                var gate = new object();

                var whenObserverRunning = new ManualResetEventSlim();
                var whenHostFinished    = new ManualResetEventSlim();

                using var subscription = source
                    .Synchronize(gate: gate)
                    .Subscribe(_ =>
                    {
                        whenObserverRunning.Set();

                        // 3) Wait for the host to verify the lock
                        whenHostFinished.Wait();
                    });

                // 1) Check that the gate isn't locked during operator setup.
                var wasGateEntered = Monitor.TryEnter(gate);
                Assert.True(wasGateEntered);
                Monitor.Exit(gate);

                var whenProducerFinished = Task.Run(() =>
                {
                    source.OnNext(1);
                });

                // 2) Wait for the observer to open a lock on the gate.
                whenObserverRunning.Wait();

                // 4) Check that the gate is locked
                wasGateEntered = Monitor.TryEnter(gate);
                try
                {
                    Assert.False(wasGateEntered);
                }
                finally
                {
                    if (wasGateEntered)
                        Monitor.Exit(gate);
                }

                whenHostFinished.Set();

                // 5) Wait for the producer/observer to release the gate
                await whenProducerFinished;

                // 6) Check that the gate is unlocked again
                wasGateEntered = Monitor.TryEnter(gate);
                Assert.True(wasGateEntered);
                Monitor.Exit(gate);
            }

        [TestMethod]
        public void Synchronize_WithLock_WhenArgumentsAreNull_ThrowsException()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Synchronize<int>(
                source: null!,
                gate:   new Lock()));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Synchronize(
                source: Observable.Empty<int>(),
                gate:   (null as Lock)!));
        }

        [TestMethod]
        public void Synchronize_WithLock_WhenNotificationsAreSynchronized_PropagatesNotifications()
        {
            using var source = new Subject<int>();

            var observer = new TestObserver<int>();

            using var subscription = source
                .Synchronize(new Lock())
                .Subscribe(observer);

            source.OnNext(1);
            source.OnNext(2);
            source.OnNext(3);
            source.OnCompleted();

            Assert.Equal(observer.ObservedValues, new[] { 1, 2, 3 });
            Assert.Equal(observer.ObservedCompletionCount, 1);
        }

        [TestMethod]
        [Timeout(10000)]
        public async Task Synchronize_WithLock_WhenGateIsLocked_BlocksNotification()
        {
            using var source = new Subject<int>();

            var gate = new Lock();

            var observer = new TestObserver<int>();

            using var subscription = source
                .Synchronize(gate: gate)
                .Subscribe(observer);

            var whenHostReady       = new ManualResetEventSlim();
            var whenProducerRunning = new ManualResetEventSlim();

            var whenProducerFinished = Task.Run(() =>
            {
                whenHostReady.Wait();

                // 3) Try to produce a value, which should block on the gate
                whenProducerRunning.Set();
                source.OnNext(1);
            });

            // 1) Lock the gate, so the producer gets blocked when producing a value
            lock (gate)
            {
                whenHostReady.Set();

                // 2) Wait for the producer to actually run, and get blocked
                whenProducerRunning.Wait();
                Thread.Yield();

                // 4) Check that the value hasn't been produced
                Assert.Empty(observer.ObservedValues);
                Assert.Empty(observer.ObservedErrors);
            }

            await whenProducerFinished;

            // 5) Check that the second value has been produced
            Assert.Equal(observer.ObservedValues, new[] { 1 });
            Assert.Empty(observer.ObservedErrors);
            Assert.Equal(observer.ObservedCompletionCount, 0);
        }

        [TestMethod]
        [Timeout(10000)]
        public async Task Synchronize_WithLock_WhenPublishingNotification_LocksGate()
        {
            using var source = new Subject<int>();

            var gate = new Lock();

            var whenObserverRunning = new ManualResetEventSlim();
            var whenHostFinished    = new ManualResetEventSlim();

            using var subscription = source
                .Synchronize(gate: gate)
                .Subscribe(_ =>
                {
                    whenObserverRunning.Set();

                    // 3) Wait for the host to verify the lock
                    whenHostFinished.Wait();
                });

            // 1) Check that the gate isn't locked during operator setup.
            var wasGateEntered = gate.TryEnter();
            Assert.True(wasGateEntered);
            gate.Exit();

            var whenProducerFinished = Task.Run(() =>
            {
                source.OnNext(1);
            });

            // 2) Wait for the observer to open a lock on the gate.
            whenObserverRunning.Wait();

            // 4) Check that the gate is locked
            wasGateEntered = gate.TryEnter();
            try
            {
                Assert.False(wasGateEntered);
            }
            finally
            {
                if (wasGateEntered)
                    gate.Exit();
            }

            whenHostFinished.Set();

            // 5) Wait for the producer/observer to release the gate
            await whenProducerFinished;

            // 6) Check that the gate is unlocked again
            wasGateEntered = gate.TryEnter();
            Assert.True(wasGateEntered);
            gate.Exit();
        }

        private class TestObserver<T>
            : IObserver<T>
        {
            public TestObserver()
            {
                _observedErrors = new();
                _observedValues = new();
            }

            public int ObservedCompletionCount
                => _observedCompletionCount;

            public List<Exception> ObservedErrors
                => _observedErrors;

            public List<T> ObservedValues
                => _observedValues;

            void IObserver<T>.OnCompleted()
                => ++_observedCompletionCount;

            void IObserver<T>.OnError(Exception error)
                => _observedErrors.Add(error);

            void IObserver<T>.OnNext(T value)
                => _observedValues.Add(value);

            private             int             _observedCompletionCount;
            private readonly    List<Exception> _observedErrors;
            private readonly    List<T>         _observedValues;
        }
    }
}
