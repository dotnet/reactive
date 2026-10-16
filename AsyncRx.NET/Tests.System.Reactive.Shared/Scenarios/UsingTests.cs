// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Disposables;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Using</c> scenarios, from Rx.NET's <c>UsingTest.cs</c>.</summary>
/// <remarks>
/// <para>
/// 8 of the file's 9 tests (the <c>Using_ArgumentChecking</c> test is the code-generated
/// stratum). Six transcribe as they are, over the shared <see cref="MockDisposable"/>, which
/// records the virtual time of its creation and disposal: a null resource, a source completing,
/// failing or disposed part-way, and a resource factory or observable factory that throws. The
/// nullable locals the originals leave to a disabled nullable context are written with the
/// null-forgiving operator here (<c>xs!</c>, <c>disposable!</c>, and the null resource the
/// factory returns in <c>Using_Null</c>, whose call also names its type arguments so that the
/// resource type is inferred non-null).
/// </para>
/// <para>
/// The two <c>*_Nested*</c> tests subscribe three nested resources through the harness where
/// the originals use a bare <c>Subscribe()</c>, and expect the resources disposed innermost
/// first and a <c>Finally</c> around them last.
/// </para>
/// </remarks>
public abstract class UsingTests : SharedReactiveTest
{
    [TestMethod]
    public void Using_Null()
    {
        var scheduler = Scheduler;

        var disposeInvoked = 0L;
        var createInvoked = 0L;
        var xs = default(TestableSeq<long>);
        var disposable = default(MockDisposable);
        var _d = default(MockDisposable);

        var res = scheduler.Start(() =>
            Seq.Using<long, MockDisposable>(
                () =>
                {
                    disposeInvoked++;
                    disposable = default;
                    return disposable!;
                },
                d =>
                {
                    _d = d;
                    createInvoked++;
                    xs = scheduler.CreateColdObservable(
                        OnNext(100, scheduler.Clock),
                        OnCompleted<long>(200));
                    return xs;
                }
            )
        );

        Assert.AreSame(disposable, _d);

        res.Messages.AssertEqual(
            OnNext(300, 200L),
            OnCompleted<long>(400)
        );

        Assert.AreEqual(1, createInvoked);
        Assert.AreEqual(1, disposeInvoked);

        xs!.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        Assert.IsNull(disposable);
    }

    [TestMethod]
    public void Using_Complete()
    {
        var scheduler = Scheduler;

        var disposeInvoked = 0;
        var createInvoked = 0;
        var xs = default(TestableSeq<long>);
        var disposable = default(MockDisposable);
        var _d = default(MockDisposable);

        var res = scheduler.Start(() =>
            Seq.Using(
                () =>
                {
                    disposeInvoked++;
                    disposable = new MockDisposable(scheduler);
                    return disposable;
                },
                d =>
                {
                    _d = d;
                    createInvoked++;
                    xs = scheduler.CreateColdObservable(
                        OnNext(100, scheduler.Clock),
                        OnCompleted<long>(200));
                    return xs;
                }
            )
        );

        Assert.AreSame(disposable, _d);

        res.Messages.AssertEqual(
            OnNext(300, 200L),
            OnCompleted<long>(400)
        );

        Assert.AreEqual(1, createInvoked);
        Assert.AreEqual(1, disposeInvoked);

        xs!.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        disposable!.AssertEqual(
            200,
            400
        );
    }

    [TestMethod]
    public void Using_Error()
    {
        var scheduler = Scheduler;

        var disposeInvoked = 0;
        var createInvoked = 0;
        var xs = default(TestableSeq<long>);
        var disposable = default(MockDisposable);
        var _d = default(MockDisposable);
        var ex = new Exception();

        var res = scheduler.Start(() =>
            Seq.Using(
                () =>
                {
                    disposeInvoked++;
                    disposable = new MockDisposable(scheduler);
                    return disposable;
                },
                d =>
                {
                    _d = d;
                    createInvoked++;
                    xs = scheduler.CreateColdObservable(
                        OnNext(100, scheduler.Clock),
                        OnError<long>(200, ex));
                    return xs;
                }
            )
        );

        Assert.AreSame(disposable, _d);

        res.Messages.AssertEqual(
            OnNext(300, 200L),
            OnError<long>(400, ex)
        );

        Assert.AreEqual(1, createInvoked);
        Assert.AreEqual(1, disposeInvoked);

        xs!.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        disposable!.AssertEqual(
            200,
            400
        );
    }

    [TestMethod]
    public void Using_Dispose()
    {
        var scheduler = Scheduler;

        var disposeInvoked = 0;
        var createInvoked = 0;
        var xs = default(TestableSeq<long>);
        var disposable = default(MockDisposable);
        var _d = default(MockDisposable);

        var res = scheduler.Start(() =>
            Seq.Using(
                () =>
                {
                    disposeInvoked++;
                    disposable = new MockDisposable(scheduler);
                    return disposable;
                },
                d =>
                {
                    _d = d;
                    createInvoked++;
                    xs = scheduler.CreateColdObservable(
                        OnNext(100, scheduler.Clock),
                        OnNext(1000, scheduler.Clock + 1));
                    return xs;
                }
            )
        );

        Assert.AreSame(disposable, _d);

        res.Messages.AssertEqual(
            OnNext(300, 200L)
        );

        Assert.AreEqual(1, createInvoked);
        Assert.AreEqual(1, disposeInvoked);

        xs!.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );

        disposable!.AssertEqual(
            200,
            1000
        );
    }

    [TestMethod]
    public void Using_ThrowResourceSelector()
    {
        var scheduler = Scheduler;

        var disposeInvoked = 0;
        var createInvoked = 0;
        var ex = new Exception();

        var res = scheduler.Start(() =>
            Seq.Using<int, IDisposable>(
                () =>
                {
                    disposeInvoked++;
                    throw ex;
                },
                d =>
                {
                    createInvoked++;
                    return Seq.Never<int>();
                }
            )
        );

        res.Messages.AssertEqual(
            OnError<int>(200, ex)
        );

        Assert.AreEqual(0, createInvoked);
        Assert.AreEqual(1, disposeInvoked);
    }

    [TestMethod]
    public void Using_ThrowResourceUsage()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var disposeInvoked = 0;
        var createInvoked = 0;
        var disposable = default(MockDisposable);

        var res = scheduler.Start(() =>
            Seq.Using<int, IDisposable>(
                () =>
                {
                    disposeInvoked++;
                    disposable = new MockDisposable(scheduler);
                    return disposable;
                },
                d =>
                {
                    createInvoked++;
                    throw ex;
                }
            )
        );

        res.Messages.AssertEqual(
            OnError<int>(200, ex)
        );

        Assert.AreEqual(1, createInvoked);
        Assert.AreEqual(1, disposeInvoked);

        disposable!.AssertEqual(
            200,
            200
        );
    }

    [TestMethod]
    public async Task Using_NestedCompleted()
    {
        var order = "";
        await Seq.Using(() => Disposable.Create(() => order += "3"),
            _ => Seq.Using(() => Disposable.Create(() => order += "2"),
                __ => Seq.Using(() => Disposable.Create(() => order += "1"),
                    ___ => Seq.Return(Unit.Default))))
            .Finally(() => order += "4")
            .SubscribeAsync(Scheduler, _ => { });
        Assert.AreEqual("1234", order);
    }

    [TestMethod]
    public async Task Using_NestedDisposed()
    {
        var order = "";
        var d = await Seq.Using(() => Disposable.Create(() => order += "3"),
            _ => Seq.Using(() => Disposable.Create(() => order += "2"),
                __ => Seq.Using(() => Disposable.Create(() => order += "1"),
                    ___ => Seq.Never<Unit>())))
            .Finally(() => order += "4")
            .SubscribeAsync(Scheduler, _ => { });
        await d.DisposeAsync();
        Assert.AreEqual("1234", order);
    }
}
