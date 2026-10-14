// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Diagnostics;
using System.Reactive;

using Microsoft.Reactive.Testing;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Catch</c> scenarios, from Rx.NET's <c>CatchTest.cs</c>.</summary>
/// <remarks>
/// <para>
/// 27 of the file's 28 tests (the <c>Catch_ArgumentChecking</c> test is the code-generated
/// stratum). Twenty-two transcribe as they are: the two-source, <c>params</c>, enumerable and
/// handler forms, with an enumerable of sources as the shared <see cref="MockEnumerable{T}"/>
/// or <see cref="RogueEnumerable{T}"/> and the original's <c>Subscriptions</c> assertions on it
/// as <c>Enumerations</c>, and <c>Catch_TailRecursive1</c>, whose expectation is computed. Five
/// are hand-adjusted. The three <c>*_DefaultScheduler_*</c> tests await the result through
/// <see cref="SharedReactiveTest.ToListAsync{T}"/> where the originals block on an event;
/// <c>Catch_TailRecursive2</c> collects with <c>ToListAsync</c> where the original uses the
/// blocking <c>ForEach</c>, and keeps its check that the stack does not deepen with recursion;
/// <c>Catch_TailRecursive3</c> takes the single element where the original blocks on
/// <c>Wait()</c>.
/// </para>
/// <para>
/// <c>Catch_TailRecursive2</c> composes with <c>StartWith</c>, which joins the model as plumbing
/// in its own folder with that one form.
/// </para>
/// </remarks>
public abstract class CatchTests : SharedReactiveTest
{
    [TestMethod]
    public void Catch_IEofIO_GetEnumeratorThrows()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xss = new RogueEnumerable<Seq<int>>(ex);

        var res = scheduler.Start(() =>
            Seq.Catch(xss)
        );

        res.Messages.AssertEqual(
            OnError<int>(200, ex)
        );
    }

    [TestMethod]
    public void Catch_IEofIO()
    {
        var scheduler = Scheduler;

        var xs1 = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnNext(30, 3),
            OnError<int>(40, new Exception())
        );

        var xs2 = scheduler.CreateColdObservable(
            OnNext(10, 4),
            OnNext(20, 5),
            OnError<int>(30, new Exception())
        );

        var xs3 = scheduler.CreateColdObservable(
            OnNext(10, 6),
            OnNext(20, 7),
            OnNext(30, 8),
            OnNext(40, 9),
            OnCompleted<int>(50)
        );

        var res = scheduler.Start(() =>
            Seq.Catch(new[] { xs1, xs2, xs3 })
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
    public void Catch_NoErrors()
    {
        var scheduler = Scheduler;

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnCompleted<int>(230)
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(240, 4),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            o1.Catch(o2)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnCompleted<int>(230)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );

        o2.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void Catch_Never()
    {
        var scheduler = Scheduler;

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(240, 4),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            o1.Catch(o2)
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
    public void Catch_Empty()
    {
        var scheduler = Scheduler;

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(230)
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(240, 4),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            o1.Catch(o2)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(230)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );

        o2.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void Catch_Return()
    {
        var scheduler = Scheduler;

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(230)
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(240, 4),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            o1.Catch(o2)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnCompleted<int>(230)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );

        o2.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void Catch_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnError<int>(230, ex)
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(240, 4),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            o1.Catch(o2)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(240, 4),
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
    public void Catch_Error_Never()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnError<int>(230, ex)
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = scheduler.Start(() =>
            o1.Catch(o2)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(230, 1000)
        );
    }

    [TestMethod]
    public void Catch_Error_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnError<int>(230, new Exception())
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(240, 4),
            OnError<int>(250, ex)
        );

        var res = scheduler.Start(() =>
            o1.Catch(o2)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(240, 4),
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
    public void Catch_Multiple()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnError<int>(215, ex)
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(220, 3),
            OnError<int>(225, ex)
        );

        var o3 = scheduler.CreateHotObservable(
            OnNext(230, 4),
            OnCompleted<int>(235)
        );

        var res = scheduler.Start(() =>
            Seq.Catch(o1, o2, o3)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnCompleted<int>(235)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 215)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(215, 225)
        );

        o3.Subscriptions.AssertEqual(
            Subscribe(225, 235)
        );
    }

    [TestMethod]
    public void Catch_ErrorSpecific_Caught()
    {
        var scheduler = Scheduler;

        var ex = new ArgumentException("x");

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnError<int>(230, ex)
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(240, 4),
            OnCompleted<int>(250)
        );

        var handlerCalled = default(long?);

        var res = scheduler.Start(() =>
            o1.Catch((ArgumentException ex_) => { handlerCalled = scheduler.Clock; return o2; })
        );

        Assert.AreEqual(230, handlerCalled);

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(240, 4),
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
    public void Catch_ErrorSpecific_Uncaught()
    {
        var scheduler = Scheduler;

        var ex = new InvalidOperationException("x");

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnError<int>(230, ex)
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(240, 4),
            OnCompleted<int>(250)
        );

        var handlerCalled = default(long?);

        var res = scheduler.Start(() =>
            o1.Catch((ArgumentException ex_) => { handlerCalled = scheduler.Clock; return o2; })
        );

        Assert.IsNull(handlerCalled);

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnError<int>(230, ex)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );

        o2.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void Catch_HandlerThrows()
    {
        var scheduler = Scheduler;

        var ex1 = new ArgumentException("x");
        var ex2 = new Exception();

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnError<int>(230, ex1)
        );

        var handlerCalled = default(long?);

        var res = scheduler.Start(() =>
            o1.Catch((ArgumentException ex_) => { handlerCalled = scheduler.Clock; throw ex2; })
        );

        Assert.AreEqual(230, handlerCalled);

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnError<int>(230, ex2)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }

    [TestMethod]
    public void Catch_Nested_OuterCatches()
    {
        var scheduler = Scheduler;

        var ex = new ArgumentException("x");

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnError<int>(215, ex)
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(220, 3),
            OnCompleted<int>(225)
        );

        var o3 = scheduler.CreateHotObservable(
            OnNext(220, 4), //!
            OnCompleted<int>(225)
        );

        var firstHandlerCalled = default(long?);
        var secondHandlerCalled = default(long?);

        var res = scheduler.Start(() =>
            o1
            .Catch((InvalidOperationException ex_) =>
            {
                firstHandlerCalled = scheduler.Clock;
                return o2;
            })
            .Catch((ArgumentException ex_) =>
            {
                secondHandlerCalled = scheduler.Clock;
                return o3;
            })
        );

        Assert.IsNull(firstHandlerCalled);
        Assert.AreEqual(215, secondHandlerCalled);

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 4),
            OnCompleted<int>(225)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 215)
        );

        o2.Subscriptions.AssertEqual(
        );

        o3.Subscriptions.AssertEqual(
            Subscribe(215, 225)
        );
    }

    [TestMethod]
    public void Catch_Nested_InnerCatches()
    {
        var scheduler = Scheduler;

        var ex = new ArgumentException("x");

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnError<int>(215, ex)
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(220, 3), //!
            OnCompleted<int>(225)
        );

        var o3 = scheduler.CreateHotObservable(
            OnNext(220, 4),
            OnCompleted<int>(225)
        );

        var firstHandlerCalled = default(long?);
        var secondHandlerCalled = default(long?);

        var res = scheduler.Start(() =>
            o1
            .Catch((ArgumentException ex_) =>
            {
                firstHandlerCalled = scheduler.Clock;
                return o2;
            })
            .Catch((InvalidOperationException ex_) =>
            {
                secondHandlerCalled = scheduler.Clock;
                return o3;
            })
        );

        Assert.AreEqual(215, firstHandlerCalled);
        Assert.IsNull(secondHandlerCalled);

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnCompleted<int>(225)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 215)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(215, 225)
        );

        o3.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void Catch_ThrowFromNestedCatch()
    {
        var scheduler = Scheduler;

        var ex1 = new ArgumentException("x1");
        var ex2 = new ArgumentException("x2");

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnError<int>(215, ex1)
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(220, 3), //!
            OnError<int>(225, ex2)
        );

        var o3 = scheduler.CreateHotObservable(
            OnNext(230, 4),
            OnCompleted<int>(235)
        );

        var firstHandlerCalled = default(long?);
        var secondHandlerCalled = default(long?);

        var res = scheduler.Start(() =>
            o1
            .Catch((ArgumentException ex_) =>
            {
                firstHandlerCalled = scheduler.Clock;
                Assert.IsTrue(ex1 == ex_, "Expected ex1");
                return o2;
            })
            .Catch((ArgumentException ex_) =>
            {
                secondHandlerCalled = scheduler.Clock;
                Assert.IsTrue(ex2 == ex_, "Expected ex2");
                return o3;
            })
        );

        Assert.AreEqual(215, firstHandlerCalled);
        Assert.AreEqual(225, secondHandlerCalled);

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnCompleted<int>(235)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 215)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(215, 225)
        );

        o3.Subscriptions.AssertEqual(
            Subscribe(225, 235)
        );
    }

    [TestMethod]
    public async Task Catch_DefaultScheduler_Binary()
    {
        var res = await ToListAsync(Seq.Return(1).Catch(Seq.Return(2)));

        Assert.AreEqual(1, res.Single());
    }

    [TestMethod]
    public async Task Catch_DefaultScheduler_Nary()
    {
        var res = await ToListAsync(Seq.Catch(Seq.Return(1), Seq.Return(2), Seq.Return(3)));

        Assert.AreEqual(1, res.Single());
    }

    [TestMethod]
    public async Task Catch_DefaultScheduler_NaryEnumerable()
    {
        IEnumerable<Seq<int>> sources = [Seq.Return(1), Seq.Return(2), Seq.Return(3)];

        var res = await ToListAsync(Seq.Catch(sources));

        Assert.AreEqual(1, res.Single());
    }

    [TestMethod]
    public void Catch_EmptyIterator()
    {
        var scheduler = Scheduler;

        var res = scheduler.Start(() =>
            Seq.Catch((IEnumerable<Seq<int>>)new Seq<int>[0])
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(200)
        );
    }

    [TestMethod]
    public void Catch_IteratorThrows()
    {
        var scheduler = Scheduler;
        var ex = new Exception();

        var res = scheduler.Start(() =>
            Seq.Catch(Catch_IteratorThrows_Source(ex, true))
        );

        res.Messages.AssertEqual(
            OnError<int>(200, ex)
        );
    }

    [TestMethod]
    public void Catch_EnumerableThrows()
    {
        var scheduler = Scheduler;

        var o = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnCompleted<int>(225)
        );

        var ex = new Exception();
        var xss = new MockEnumerable<Seq<int>>(scheduler, GetObservablesForCatchThrow(o, ex));

        var res = scheduler.Start(() =>
            xss.Catch()
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnCompleted<int>(225)
        );

        o.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );

        xss.Enumerations.AssertEqual(
            new Enumeration(200, 225)
        );
    }

    [TestMethod]
    public void Catch_EnumerableTiming()
    {
        var scheduler = Scheduler;

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2), // !
            OnNext(220, 3), // !
            OnError<int>(230, new Exception())
        );

        var o2 = scheduler.CreateColdObservable(
            OnNext(50, 4),  // !
            OnNext(60, 5),  // !
            OnNext(70, 6),  // !
            OnError<int>(80, new Exception())
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
            xss.Select(xs => (Seq<int>)xs).Catch()
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(280, 4),
            OnNext(290, 5),
            OnNext(300, 6),
            OnNext(320, 7),
            OnNext(330, 8),
            OnCompleted<int>(340)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(230, 310)
        );

        o3.Subscriptions.AssertEqual(
            Subscribe(310, 340)
        );

        xss.Enumerations.AssertEqual(
            new Enumeration(200, 340)
        );
    }

    [TestMethod]
    public void Catch_Enumerable_Dispose()
    {
        var scheduler = Scheduler;

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnError<int>(230, new Exception())
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
            xss.Select(xs => (Seq<int>)xs).Catch(),
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
    public void Catch_TailRecursive1()
    {
        var create = 0L;
        var start = 200L;
        var end = 1000L;

        var scheduler = Scheduler;

        var o = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnNext(30, 3),
            OnError<int>(40, new Exception())
        );

        Seq<int> f() => Seq.Defer(() => o.Catch(f()));
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
    public async Task Catch_TailRecursive2()
    {
        Func<int, Seq<int>> f = null!;
        f = x => Seq.Defer(() =>
            Seq.Throw<int>(new Exception(), ThreadPoolScheduler).StartWith(x).Catch(f(x + 1)));

        var lst = await ToListAsync(f(0).Select(x => new StackTrace().FrameCount).Take(10));

        Assert.IsTrue(lst.Last() - lst.First() < 10);
    }

    [TestMethod]
    public async Task Catch_TailRecursive3()
    {
        var ex = new Exception();

        var res =
            Seq.Catch(
                Seq.Defer(() =>
                {
                    if (ex != null)
                    {
                        throw ex;
                    }

                    return Seq.Return(-2);
                }),
                Seq.Defer(() =>
                {
                    if (ex != null)
                    {
                        throw ex;
                    }

                    return Seq.Return(-1);
                }),
                Seq.Return(42)
            );

        Assert.AreEqual(42, (await ToListAsync(res)).Single());
    }

    private IEnumerable<Seq<int>> Catch_IteratorThrows_Source(Exception ex, bool b)
    {
        if (b)
        {
            throw ex;
        }
        else
        {
            yield break;
        }
    }

    private IEnumerable<Seq<int>> GetObservablesForCatchThrow(Seq<int> first, Exception ex)
    {
        yield return first;
        throw ex;
    }
}
