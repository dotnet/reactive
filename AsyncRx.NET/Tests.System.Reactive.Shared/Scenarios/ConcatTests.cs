// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Diagnostics;
using System.Reactive;

using Microsoft.Reactive.Testing;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Concat</c> scenarios, from Rx.NET's <c>ConcatTest.cs</c>.</summary>
/// <remarks>
/// <para>
/// 27 of the file's 28 tests: the fifteen pairwise virtual-time tests over <c>o1.Concat(o2)</c>;
/// the <c>params</c>, enumerable and <c>Defer</c> tests, where an enumerable of sources is the
/// shared <see cref="MockEnumerable{T}"/> or <see cref="RogueEnumerable{T}"/> and the original's
/// <c>Subscriptions</c> assertions on it become <c>Enumerations</c>; <c>Concat_TailRecursive1</c>,
/// whose expectation is computed; and four real-time tests, hand-adjusted. The two
/// <c>*_DefaultScheduler</c> tests await a <c>TaskCompletionSource</c> through the raw
/// three-handler surface where the original blocks on an event; <c>Concat_TailRecursive2</c>
/// collects with <c>ToListAsync</c> where the original uses the blocking <c>ForEach</c>, and
/// keeps its check that the stack does not deepen with recursion; <c>Concat_Task</c> collects
/// with <c>ToListAsync</c> where the original uses <c>ToArray().Single()</c>.
/// </para>
/// <para>
/// <c>Concat</c> was plumbing before this step, a node other scenarios compose with; it keeps
/// that role from its own folder, and the <c>params</c>, enumerable and task forms are new to
/// the model. Not here: the <c>Concat_ArgumentChecking</c> test, the code-generated stratum.
/// </para>
/// </remarks>
public abstract class ConcatTests : SharedReactiveTest
{
    [TestMethod]
    public async Task Concat_DefaultScheduler()
    {
        var evt = new TaskCompletionSource();

        var sum = 0;
        await Seq.Concat(Seq.Return(1), Seq.Return(2), Seq.Return(3)).SubscribeAsync(
            Scheduler,
            n =>
            {
                sum += n;
            },
            ex => evt.TrySetException(ex),
            () => evt.TrySetResult());

        await evt.Task;

        Assert.AreEqual(6, sum);
    }

    [TestMethod]
    public async Task Concat_IEofIO_DefaultScheduler()
    {
        var evt = new TaskCompletionSource();

        IEnumerable<Seq<int>> sources = [Seq.Return(1), Seq.Return(2), Seq.Return(3)];

        var sum = 0;
        await Seq.Concat(sources).SubscribeAsync(
            Scheduler,
            n =>
            {
                sum += n;
            },
            ex => evt.TrySetException(ex),
            () => evt.TrySetResult());

        await evt.Task;

        Assert.AreEqual(6, sum);
    }

    [TestMethod]
    public void Concat_IEofIO_GetEnumeratorThrows()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xss = new RogueEnumerable<Seq<int>>(ex);

        var res = scheduler.Start(() =>
            Seq.Concat(xss)
        );

        res.Messages.AssertEqual(
            OnError<int>(200, ex)
        );
    }

    [TestMethod]
    public void Concat_IEofIO()
    {
        var scheduler = Scheduler;

        var xs1 = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnNext(30, 3),
            OnCompleted<int>(40)
        );

        var xs2 = scheduler.CreateColdObservable(
            OnNext(10, 4),
            OnNext(20, 5),
            OnCompleted<int>(30)
        );

        var xs3 = scheduler.CreateColdObservable(
            OnNext(10, 6),
            OnNext(20, 7),
            OnNext(30, 8),
            OnNext(40, 9),
            OnCompleted<int>(50)
        );

        var res = scheduler.Start(() =>
            Seq.Concat([xs1, xs2, xs3])
        );

        res.Messages.AssertEqual(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnNext(250, 4),
            OnNext(260, 5),
            OnNext(280, 6),
            OnNext(290, 7),
            OnNext(300, 8),
            OnNext(310, 9),
            OnCompleted<int>(320)
        );

        xs1.Subscriptions.AssertEqual(
            Subscribe(200, 240)
        );

        xs2.Subscriptions.AssertEqual(
            Subscribe(240, 270)
        );

        xs3.Subscriptions.AssertEqual(
            Subscribe(270, 320)
        );
    }

    [TestMethod]
    public void Concat_EmptyEmpty()
    {
        var scheduler = Scheduler;

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(230)
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            o1.Concat(o2)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(250)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(230, 250)
        );
    }

    [TestMethod]
    public void Concat_EmptyNever()
    {
        var scheduler = Scheduler;

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(230)
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = scheduler.Start(() =>
            o1.Concat(o2)
        );

        res.Messages.AssertEqual(
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(230, 1000)
        );
    }

    [TestMethod]
    public void Concat_NeverEmpty()
    {
        var scheduler = Scheduler;

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(230)
        );

        var res = scheduler.Start(() =>
            o1.Concat(o2)
        );

        res.Messages.AssertEqual(
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );

        o2.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void Concat_NeverNever()
    {
        var scheduler = Scheduler;

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = scheduler.Start(() =>
            o1.Concat(o2)
        );

        res.Messages.AssertEqual(
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );

        o2.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void Concat_EmptyThrow()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(230)
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(250, ex)
        );

        var res = scheduler.Start(() =>
            o1.Concat(o2)
        );

        res.Messages.AssertEqual(
            OnError<int>(250, ex)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(230, 250)
        );
    }

    [TestMethod]
    public void Concat_ThrowEmpty()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(230, ex)
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            o1.Concat(o2)
        );

        res.Messages.AssertEqual(
            OnError<int>(230, ex)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );

        o2.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void Concat_ThrowThrow()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(230, ex)
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(250, new Exception())
        );

        var res = scheduler.Start(() =>
            o1.Concat(o2)
        );

        res.Messages.AssertEqual(
            OnError<int>(230, ex)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );

        o2.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void Concat_ReturnEmpty()
    {
        var scheduler = Scheduler;

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(230)
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            o1.Concat(o2)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(230, 250)
        );
    }

    [TestMethod]
    public void Concat_EmptyReturn()
    {
        var scheduler = Scheduler;

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(230)
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(240, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            o1.Concat(o2)
        );

        res.Messages.AssertEqual(
            OnNext(240, 2),
            OnCompleted<int>(250)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(230, 250)
        );
    }

    [TestMethod]
    public void Concat_ReturnNever()
    {
        var scheduler = Scheduler;

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(230)
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = scheduler.Start(() =>
            o1.Concat(o2)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(230, 1000)
        );
    }

    [TestMethod]
    public void Concat_NeverReturn()
    {
        var scheduler = Scheduler;

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(230)
        );

        var res = scheduler.Start(() =>
            o1.Concat(o2)
        );

        res.Messages.AssertEqual(
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );

        o2.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void Concat_ReturnReturn()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnCompleted<int>(230)
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(240, 3),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            o1.Concat(o2)
        );

        res.Messages.AssertEqual(
            OnNext(220, 2),
            OnNext(240, 3),
            OnCompleted<int>(250)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(230, 250)
        );
    }

    [TestMethod]
    public void Concat_ThrowReturn()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(230, ex)
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(240, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            o1.Concat(o2)
        );

        res.Messages.AssertEqual(
            OnError<int>(230, ex)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );

        o2.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void Concat_ReturnThrow()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnCompleted<int>(230)
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(250, ex)
        );

        var res = scheduler.Start(() =>
            o1.Concat(o2)
        );

        res.Messages.AssertEqual(
            OnNext(220, 2),
            OnError<int>(250, ex)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(230, 250)
        );
    }

    [TestMethod]
    public void Concat_SomeDataSomeData()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnCompleted<int>(225)
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            o1.Concat(o2)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(225, 250)
        );
    }

    [TestMethod]
    public void Concat_EnumerableThrows()
    {
        var scheduler = Scheduler;

        var o = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnCompleted<int>(225)
        );

        var ex = new Exception();
        var xss = new MockEnumerable<Seq<int>>(scheduler, GetObservablesForConcatThrow(o, ex));

        var res = scheduler.Start(() =>
            xss.Concat()
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnError<int>(225, ex)
        );

        o.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );

        xss.Enumerations.AssertEqual(
            new Enumeration(200, 225)
        );
    }

    [TestMethod]
    public void Concat_EnumerableTiming()
    {
        var scheduler = Scheduler;

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2), // !
            OnNext(220, 3), // !
            OnCompleted<int>(230)
        );

        var o2 = scheduler.CreateColdObservable(
            OnNext(50, 4),  // !
            OnNext(60, 5),  // !
            OnNext(70, 6),  // !
            OnCompleted<int>(80)
        );

        var o3 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(200, 2),
            OnNext(210, 3),
            OnNext(220, 4),
            OnNext(230, 5),
            OnNext(270, 6),
            OnNext(320, 7), // !
            OnNext(330, 8), // !
            OnCompleted<int>(340)
        );

        var xss = new MockEnumerable<TestableSeq<int>>(scheduler, [o1, o2, o3, o2]);

        var res = scheduler.Start(() =>
            xss.Select(xs => (Seq<int>)xs).Concat()
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(280, 4),
            OnNext(290, 5),
            OnNext(300, 6),
            OnNext(320, 7),
            OnNext(330, 8),
            OnNext(390, 4),
            OnNext(400, 5),
            OnNext(410, 6),
            OnCompleted<int>(420)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(230, 310),
            Subscribe(340, 420)
        );

        o3.Subscriptions.AssertEqual(
            Subscribe(310, 340)
        );

        xss.Enumerations.AssertEqual(
            new Enumeration(200, 420)
        );
    }

    [TestMethod]
    public void Concat_Enumerable_Dispose()
    {
        var scheduler = Scheduler;

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnCompleted<int>(230)
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(200, 2),
            OnNext(210, 3),
            OnNext(240, 4),
            OnNext(270, 5),
            OnNext(320, 6),
            OnNext(330, 7),
            OnCompleted<int>(340)
        );

        var xss = new MockEnumerable<TestableSeq<int>>(scheduler, [o1, o2]);

        var res = scheduler.Start(() =>
            xss.Select(xs => (Seq<int>)xs).Concat(),
            300
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(240, 4),
            OnNext(270, 5)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(230, 300)
        );

        xss.Enumerations.AssertEqual(
            new Enumeration(200, 300)
        );
    }

    [TestMethod]
    public void Concat_Optimization_DeferEvalTiming()
    {
        var scheduler = Scheduler;

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnCompleted<int>(230)
        );

        var o2 = scheduler.CreateColdObservable(
            OnNext(10, 4),
            OnNext(20, 5),
            OnNext(30, 6),
            OnCompleted<int>(40)
        );

        var invoked = default(long);

        var xs = o1;
        var ys = Seq.Defer(() => { invoked = scheduler.Clock; return o2; });

        var res = scheduler.Start(() =>
            xs.Concat(ys)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(240, 4),
            OnNext(250, 5),
            OnNext(260, 6),
            OnCompleted<int>(270)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(230, 270)
        );

        Assert.AreEqual(230, invoked);
    }

    [TestMethod]
    public void Concat_Optimization_DeferExceptionPropagation()
    {
        var scheduler = Scheduler;

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(220)
        );

        var ex = new Exception();
        var invoked = default(long);

        var xs = o1;
        var ys = Seq.Defer(new Func<Seq<int>>(() => { invoked = scheduler.Clock; throw ex; }));

        var res = scheduler.Start(() =>
            xs.Concat(ys)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnError<int>(220, ex)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );

        Assert.AreEqual(220, invoked);
    }

    [TestMethod]
    public void Concat_TailRecursive1()
    {
        var create = 0L;
        var start = 200L;
        var end = 1000L;

        var scheduler = Scheduler;

        var o = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnNext(30, 3),
            OnCompleted<int>(40)
        );

        Seq<int> f() => Seq.Defer(() => o.Concat(f()));
        var res = scheduler.Start(() => f(), create, start, end);

        var expected = new List<Recorded<Notification<int>>>();

        var t = start;
        while (t <= end)
        {
            var n = (t - start) / 10;
            if (n % 4 != 0)
            {
                expected.Add(OnNext(t, (int)(n % 4)));
            }

            t += 10;
        }

        res.Messages.AssertEqual(expected);
    }

    [TestMethod]
    public async Task Concat_TailRecursive2()
    {
        Seq<int> f(int x) => Seq.Defer(() => Seq.Return(x, ThreadPoolScheduler).Concat(f(x + 1)));
        var lst = await ToListAsync(f(0).Select(x => new StackTrace().FrameCount).Take(10));

        Assert.IsTrue(lst.Last() - lst.First() < 10);
    }

    [TestMethod]
    public async Task Concat_Task()
    {
        var tss = Seq.Concat(
            new[]
            {
                Task.Factory.StartNew(() => 1),
                Task.Factory.StartNew(() => 2),
                Task.Factory.StartNew(() => 3),
            }.ToSeq());

        var res = await ToListAsync(tss);

        Assert.IsTrue(res.SequenceEqual([1, 2, 3]));
    }

    private IEnumerable<Seq<int>> GetObservablesForConcatThrow(Seq<int> first, Exception ex)
    {
        yield return first;
        throw ex;
    }
}
