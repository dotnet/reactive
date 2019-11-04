// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class GenerateTest : ReactiveTest
    {
        #region + Non-timed +

        [Fact]
        public void Generate_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, (IScheduler)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, null, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, (Func<int, int>)null, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, null, DummyFunc<int, int>.Instance, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyScheduler.Instance).Subscribe(null));
        }

        [Fact]
        public void Generate_Finite()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Generate(0, x => x <= 3, x => x + 1, x => x, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, 0),
                OnNext(202, 1),
                OnNext(203, 2),
                OnNext(204, 3),
                OnCompleted<int>(205)
            );
        }

        [Fact]
        public void Generate_Throw_Condition()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Generate(0, new Func<int, bool>(x => { throw ex; }), x => x + 1, x => x, scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(201, ex)
            );
        }

        [Fact]
        public void Generate_Throw_ResultSelector()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Generate(0, x => true, x => x + 1, new Func<int, int>(x => { throw ex; }), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(201, ex)
            );
        }

        [Fact]
        public void Generate_Throw_Iterate()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Generate(0, x => true, new Func<int, int>(x => { throw ex; }), x => x, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, 0),
                OnError<int>(202, ex)
            );
        }

        [Fact]
        public void Generate_Dispose()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Generate(0, x => true, x => x + 1, x => x, scheduler),
                203
            );

            res.Messages.AssertEqual(
                OnNext(201, 0),
                OnNext(202, 1)
            );
        }

        [Fact]
        public void Generate_DefaultScheduler_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, null, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, (Func<int, int>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, null, DummyFunc<int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance).Subscribe(null));
        }

        [Fact]
        public void Generate_DefaultScheduler()
        {
            Observable.Generate(0, x => x < 10, x => x + 1, x => x).AssertEqual(Observable.Generate(0, x => x < 10, x => x + 1, x => x, DefaultScheduler.Instance));
        }

#if !NO_PERF
        [Fact]
        public void Generate_LongRunning1()
        {
            var start = default(ManualResetEvent);
            var end = default(ManualResetEvent);
            var s = new TestLongRunningScheduler(x => start = x, x => end = x);

            var xs = Observable.Generate(0, x => x < 100, x => x + 1, x => x, s);

            var lst = new List<int>();
            var done = false;
            xs.Subscribe(x => { lst.Add(x); }, () => done = true);

            end.WaitOne();

            Assert.True(lst.SequenceEqual(Enumerable.Range(0, 100)));
            Assert.True(done);
        }

        [Fact]
        [MethodImpl(MethodImplOptions.NoOptimization)]
        public void Generate_LongRunning2()
        {
            var start = default(ManualResetEvent);
            var end = default(ManualResetEvent);
            var s = new TestLongRunningScheduler(x => start = x, x => end = x);

            var xs = Observable.Generate(0, _ => true, x => x + 1, x => x, s);

            var lst = new List<int>();
            var d = xs.Subscribe(x => { lst.Add(x); });

            start.WaitOne();

            while (lst.Count < 100)
            {
                ;
            }

            d.Dispose();
            end.WaitOne();

            Assert.True(lst.Take(100).SequenceEqual(Enumerable.Range(0, 100)));
        }

        [Fact]
        public void Generate_LongRunning_Throw()
        {
            var start = default(ManualResetEvent);
            var end = default(ManualResetEvent);
            var s = new TestLongRunningScheduler(x => start = x, x => end = x);

            var ex = new Exception();
            var xs = Observable.Generate(0, x => { if (x < 100) { return true; } throw ex; }, x => x + 1, x => x, s);

            var lst = new List<int>();
            var e = default(Exception);
            var done = false;
            xs.Subscribe(x => { lst.Add(x); }, e_ => e = e_, () => done = true);

            end.WaitOne();

            Assert.True(lst.SequenceEqual(Enumerable.Range(0, 100)));
            Assert.Same(ex, e);
            Assert.False(done);
        }
#endif

        #endregion

        #region + Timed +

        [Fact]
        public void Generate_TimeSpan_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, TimeSpan>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, null, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, TimeSpan>.Instance, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, (Func<int, int>)null, DummyFunc<int, TimeSpan>.Instance, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, null, DummyFunc<int, int>.Instance, DummyFunc<int, TimeSpan>.Instance, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, (Func<int, TimeSpan>)null, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, TimeSpan>.Instance, DummyScheduler.Instance).Subscribe(null));
        }

        [Fact]
        public void Generate_TimeSpan_Finite()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Generate(0, x => x <= 3, x => x + 1, x => x, x => TimeSpan.FromTicks(x + 1), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(202, 0),
                OnNext(204, 1),
                OnNext(207, 2),
                OnNext(211, 3),
                OnCompleted<int>(211)
            );
        }

        [Fact]
        public void Generate_TimeSpan_Throw_Condition()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Generate(0, new Func<int, bool>(x => { throw ex; }), x => x + 1, x => x, x => TimeSpan.FromTicks(x + 1), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(201, ex)
            );
        }

        [Fact]
        public void Generate_TimeSpan_Throw_ResultSelector()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Generate(0, x => true, x => x + 1, new Func<int, int>(x => { throw ex; }), x => TimeSpan.FromTicks(x + 1), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(201, ex)
            );
        }

        [Fact]
        public void Generate_TimeSpan_Throw_Iterate()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Generate(0, x => true, new Func<int, int>(x => { throw ex; }), x => x, x => TimeSpan.FromTicks(x + 1), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(202, 0),
                OnError<int>(202, ex)
            );
        }

        [Fact]
        public void Generate_TimeSpan_Throw_TimeSelector()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Generate(0, x => true, x => x + 1, x => x, new Func<int, TimeSpan>(x => { throw ex; }), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(201, ex)
            );
        }

        [Fact]
        public void Generate_TimeSpan_Dispose()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Generate(0, x => true, x => x + 1, x => x, x => TimeSpan.FromTicks(x + 1), scheduler),
                210
            );

            res.Messages.AssertEqual(
                OnNext(202, 0),
                OnNext(204, 1),
                OnNext(207, 2)
            );
        }

        [Fact]
        public void Generate_TimeSpan_DefaultScheduler_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, null, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, TimeSpan>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, (Func<int, int>)null, DummyFunc<int, TimeSpan>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, null, DummyFunc<int, int>.Instance, DummyFunc<int, TimeSpan>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, (Func<int, TimeSpan>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, TimeSpan>.Instance).Subscribe(null));
        }

        [Fact]
        public void Generate_TimeSpan_DefaultScheduler()
        {
            Observable.Generate(0, x => x < 10, x => x + 1, x => x, x => TimeSpan.FromMilliseconds(x)).AssertEqual(Observable.Generate(0, x => x < 10, x => x + 1, x => x, x => TimeSpan.FromMilliseconds(x), DefaultScheduler.Instance));
        }

        [Fact]
        public void Generate_DateTimeOffset_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, DateTimeOffset>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, null, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, DateTimeOffset>.Instance, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, (Func<int, int>)null, DummyFunc<int, DateTimeOffset>.Instance, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, null, DummyFunc<int, int>.Instance, DummyFunc<int, DateTimeOffset>.Instance, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, (Func<int, DateTimeOffset>)null, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, DateTimeOffset>.Instance, DummyScheduler.Instance).Subscribe(null));
        }

        [Fact]
        public void Generate_DateTimeOffset_Finite()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Generate(0, x => x <= 3, x => x + 1, x => x, x => scheduler.Now.AddTicks(x + 1), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(202, 0),
                OnNext(204, 1),
                OnNext(207, 2),
                OnNext(211, 3),
                OnCompleted<int>(211)
            );
        }

        [Fact]
        public void Generate_DateTimeOffset_Throw_Condition()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Generate(0, new Func<int, bool>(x => { throw ex; }), x => x + 1, x => x, x => scheduler.Now.AddTicks(x + 1), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(201, ex)
            );
        }

        [Fact]
        public void Generate_DateTimeOffset_Throw_ResultSelector()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Generate(0, x => true, x => x + 1, new Func<int, int>(x => { throw ex; }), x => scheduler.Now.AddTicks(x + 1), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(201, ex)
            );
        }

        [Fact]
        public void Generate_DateTimeOffset_Throw_Iterate()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Generate(0, x => true, new Func<int, int>(x => { throw ex; }), x => x, x => scheduler.Now.AddTicks(x + 1), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(202, 0),
                OnError<int>(202, ex)
            );
        }

        [Fact]
        public void Generate_DateTimeOffset_Throw_TimeSelector()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Generate(0, x => true, x => x + 1, x => x, new Func<int, DateTimeOffset>(x => { throw ex; }), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(201, ex)
            );
        }

        [Fact]
        public void Generate_DateTimeOffset_Dispose()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Generate(0, x => true, x => x + 1, x => x, x => scheduler.Now.AddTicks(x + 1), scheduler),
                210
            );

            res.Messages.AssertEqual(
                OnNext(202, 0),
                OnNext(204, 1),
                OnNext(207, 2)
            );
        }

        [Fact]
        public void Generate_DateTimeOffset_DefaultScheduler_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, null, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, DateTimeOffset>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, (Func<int, int>)null, DummyFunc<int, DateTimeOffset>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, null, DummyFunc<int, int>.Instance, DummyFunc<int, DateTimeOffset>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, (Func<int, DateTimeOffset>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, DateTimeOffset>.Instance).Subscribe(null));
        }

        [Fact]
        public void Generate_DateTimeOffset_DefaultScheduler()
        {
            Observable.Generate(0, x => x < 10, x => x + 1, x => x, x => DateTimeOffset.Now.AddMilliseconds(x)).AssertEqual(Observable.Generate(0, x => x < 10, x => x + 1, x => x, x => DateTimeOffset.Now.AddMilliseconds(x), DefaultScheduler.Instance));
        }

        [Fact]
        public void Generate_TimeSpan_DisposeLater()
        {
            var count = 0;
            var d = Observable.Generate(0, i => true, i => i + 1, i => i, _ => TimeSpan.Zero)
                .WithLatestFrom(Observable.Return(1).Concat(Observable.Never<int>()), (a, b) => a)
                .Subscribe(v => Volatile.Write(ref count, v));

            while (Volatile.Read(ref count) < 10000)
            {
                Thread.Sleep(10);
            }

            d.Dispose();
        }

        [Fact]
        public void Generate_DateTimeOffset_DisposeLater()
        {
            var count = 0;

            var d = Observable.Generate(0, i => true, i => i + 1, i => i, _ => DateTimeOffset.Now)
                .WithLatestFrom(Observable.Return(1).Concat(Observable.Never<int>()), (a, b) => a)
                .Subscribe(v => Volatile.Write(ref count, v));

            while (Volatile.Read(ref count) < 10000)
            {
                Thread.Sleep(10);
            }

            d.Dispose();
        }


        #endregion
    }
}
