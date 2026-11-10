// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Average</c> scenarios, from Rx.NET's <c>AverageTest.cs</c>.</summary>
/// <remarks>
/// 60 of the file's 71 tests, all transcribed as they are: the typed forms over each numeric
/// type and its nullable form, and the selector forms. Not here: the <c>*_ArgumentChecking</c>
/// test, the code-generated stratum; and the ten <c>Average_InjectOverflow_*</c> tests, which
/// set the running count inside Rx.NET's sink to <c>long.MaxValue</c> by reflection on a private
/// field, to show that the count is incremented in a checked context. AsyncRx.NET's sinks keep
/// the count in a local of a closure, where nothing can reach it, so those ten are written
/// against Rx.NET in its runner. The four <c>*_Overflow</c> tests at the end are not from the
/// original file: they ask the same question of both targets through the running sum, which
/// shares the checked block and the error path with the count and can be overflowed by two
/// elements, where the count cannot be reached. Only the <c>long</c> and <c>decimal</c>
/// accumulators can overflow that way (the <c>int</c> forms sum into a <c>long</c>, the
/// floating-point forms do not overflow), so there are four.
/// </remarks>
public abstract class AverageTests : SharedReactiveTest
{
    [TestMethod]
    public void Average_Int32_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnError<double>(250, e => e is InvalidOperationException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Average_Int32_Return()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnNext(250, 2.0),
            OnCompleted<double>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Average_Int32_Some()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 3),
            OnNext(220, 4),
            OnNext(230, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnNext(250, 3.0),
            OnCompleted<double>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Average_Int32_Throw()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(210, ex)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnError<double>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Average_Int32_Never()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void Average_Int64_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1L),
            OnCompleted<long>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnError<double>(250, e => e is InvalidOperationException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Average_Int64_Return()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1L),
            OnNext(210, 2L),
            OnCompleted<long>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnNext(250, 2.0),
            OnCompleted<double>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Average_Int64_Some()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1L),
            OnNext(210, 3L),
            OnNext(220, 4L),
            OnNext(230, 2L),
            OnCompleted<long>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnNext(250, 3.0),
            OnCompleted<double>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Average_Int64_Throw()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1L),
            OnError<long>(210, ex)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnError<double>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Average_Int64_Never()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1L)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void Average_Double_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1.0),
            OnCompleted<double>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnError<double>(250, e => e is InvalidOperationException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Average_Double_Return()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1.0),
            OnNext(210, 2.0),
            OnCompleted<double>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnNext(250, 2.0),
            OnCompleted<double>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Average_Double_Some()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1.0),
            OnNext(210, 3.0),
            OnNext(220, 4.0),
            OnNext(230, 2.0),
            OnCompleted<double>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnNext(250, 3.0),
            OnCompleted<double>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Average_Double_Throw()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1.0),
            OnError<double>(210, ex)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnError<double>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Average_Double_Never()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1.0)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void Average_Float_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1f),
            OnCompleted<float>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnError<float>(250, e => e is InvalidOperationException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Average_Float_Return()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1f),
            OnNext(210, 2f),
            OnCompleted<float>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnNext(250, 2f),
            OnCompleted<float>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Average_Float_Some()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1f),
            OnNext(210, 3f),
            OnNext(220, 4f),
            OnNext(230, 2f),
            OnCompleted<float>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnNext(250, 3f),
            OnCompleted<float>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Average_Float_Throw()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1f),
            OnError<float>(210, ex)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnError<float>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Average_Float_Never()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1f)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void Average_Decimal_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1m),
            OnCompleted<decimal>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnError<decimal>(250, e => e is InvalidOperationException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Average_Decimal_Return()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1m),
            OnNext(210, 2m),
            OnCompleted<decimal>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnNext(250, 2m),
            OnCompleted<decimal>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Average_Decimal_Some()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1m),
            OnNext(210, 3m),
            OnNext(220, 4m),
            OnNext(230, 2m),
            OnCompleted<decimal>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnNext(250, 3m),
            OnCompleted<decimal>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Average_Decimal_Throw()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1m),
            OnError<decimal>(210, ex)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnError<decimal>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Average_Decimal_Never()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1m)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void Average_Nullable_Int32_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, (int?)1),
            OnCompleted<int?>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnNext(250, (double?)null),
            OnCompleted<double?>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Average_Nullable_Int32_Return()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, (int?)1),
            OnNext(210, (int?)2),
            OnCompleted<int?>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnNext(250, (double?)2.0),
            OnCompleted<double?>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Average_Nullable_Int32_Some()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, (int?)1),
            OnNext(210, (int?)3),
            OnNext(220, (int?)null),
            OnNext(230, (int?)2),
            OnCompleted<int?>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnNext(250, (double?)2.5),
            OnCompleted<double?>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Average_Nullable_Int32_Throw()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, (int?)1),
            OnError<int?>(210, ex)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnError<double?>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Average_Nullable_Int32_Never()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, (int?)1)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void Average_Nullable_Int64_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, (long?)1L),
            OnCompleted<long?>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnNext(250, (double?)null),
            OnCompleted<double?>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Average_Nullable_Int64_Return()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, (long?)1L),
            OnNext(210, (long?)2L),
            OnCompleted<long?>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnNext(250, (double?)2.0),
            OnCompleted<double?>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Average_Nullable_Int64_Some()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, (long?)1L),
            OnNext(210, (long?)3L),
            OnNext(220, (long?)null),
            OnNext(230, (long?)2L),
            OnCompleted<long?>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnNext(250, (double?)2.5),
            OnCompleted<double?>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Average_Nullable_Int64_Throw()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, (long?)1L),
            OnError<long?>(210, ex)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnError<double?>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Average_Nullable_Int64_Never()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, (long?)1L)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void Average_Nullable_Double_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, (double?)1.0),
            OnCompleted<double?>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnNext(250, (double?)null),
            OnCompleted<double?>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Average_Nullable_Double_Return()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, (double?)1.0),
            OnNext(210, (double?)2.0),
            OnCompleted<double?>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnNext(250, (double?)2.0),
            OnCompleted<double?>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Average_Nullable_Double_Some()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, (double?)1.0),
            OnNext(210, (double?)3.0),
            OnNext(220, (double?)null),
            OnNext(230, (double?)2.0),
            OnCompleted<double?>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnNext(250, (double?)2.5),
            OnCompleted<double?>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Average_Nullable_Double_Throw()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, (double?)1.0),
            OnError<double?>(210, ex)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnError<double?>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Average_Nullable_Double_Never()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, (double?)1.0)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void Average_Nullable_Float_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, (float?)1f),
            OnCompleted<float?>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnNext(250, (float?)null),
            OnCompleted<float?>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Average_Nullable_Float_Return()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, (float?)1f),
            OnNext(210, (float?)2f),
            OnCompleted<float?>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnNext(250, (float?)2f),
            OnCompleted<float?>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Average_Nullable_Float_Some()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, (float?)1f),
            OnNext(210, (float?)3f),
            OnNext(220, (float?)null),
            OnNext(230, (float?)2f),
            OnCompleted<float?>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnNext(250, (float?)2.5f),
            OnCompleted<float?>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Average_Nullable_Float_Throw()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, (float?)1f),
            OnError<float?>(210, ex)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnError<float?>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Average_Nullable_Float_Never()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, (float?)1f)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void Average_Nullable_Decimal_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, (decimal?)1m),
            OnCompleted<decimal?>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnNext(250, (decimal?)null),
            OnCompleted<decimal?>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Average_Nullable_Decimal_Return()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, (decimal?)1m),
            OnNext(210, (decimal?)2m),
            OnCompleted<decimal?>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnNext(250, (decimal?)2m),
            OnCompleted<decimal?>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Average_Nullable_Decimal_Some()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, (decimal?)1m),
            OnNext(210, (decimal?)3m),
            OnNext(220, (decimal?)null),
            OnNext(230, (decimal?)2m),
            OnCompleted<decimal?>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnNext(250, (decimal?)2.5m),
            OnCompleted<decimal?>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Average_Nullable_Decimal_Throw()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, (decimal?)1m),
            OnError<decimal?>(210, ex)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnError<decimal?>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Average_Nullable_Decimal_Never()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, (decimal?)1m)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void Average_Selector_Regular_Int32()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, "b"),
            OnNext(220, "fo"),
            OnNext(230, "qux"),
            OnCompleted<string>(240)
        );

        var res = scheduler.Start(() => xs.Average(x => x.Length));

        res.Messages.AssertEqual(
            OnNext(240, 2.0),
            OnCompleted<double>(240)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 240)
        );
    }

    [TestMethod]
    public void Average_Selector_Regular_Int64()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, "b"),
            OnNext(220, "fo"),
            OnNext(230, "qux"),
            OnCompleted<string>(240)
        );

        var res = scheduler.Start(() => xs.Average(x => (long)x.Length));

        res.Messages.AssertEqual(
            OnNext(240, 2.0),
            OnCompleted<double>(240)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 240)
        );
    }

    [TestMethod]
    public void Average_Selector_Regular_Single()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, "b"),
            OnNext(220, "fo"),
            OnNext(230, "qux"),
            OnCompleted<string>(240)
        );

        var res = scheduler.Start(() => xs.Average(x => (float)x.Length));

        res.Messages.AssertEqual(
            OnNext(240, 2.0f),
            OnCompleted<float>(240)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 240)
        );
    }

    [TestMethod]
    public void Average_Selector_Regular_Double()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, "b"),
            OnNext(220, "fo"),
            OnNext(230, "qux"),
            OnCompleted<string>(240)
        );

        var res = scheduler.Start(() => xs.Average(x => (double)x.Length));

        res.Messages.AssertEqual(
            OnNext(240, 2.0),
            OnCompleted<double>(240)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 240)
        );
    }

    [TestMethod]
    public void Average_Selector_Regular_Decimal()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, "b"),
            OnNext(220, "fo"),
            OnNext(230, "qux"),
            OnCompleted<string>(240)
        );

        var res = scheduler.Start(() => xs.Average(x => (decimal)x.Length));

        res.Messages.AssertEqual(
            OnNext(240, 2.0m),
            OnCompleted<decimal>(240)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 240)
        );
    }

    [TestMethod]
    public void Average_Selector_Regular_Int32_Nullable()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, "b"),
            OnNext(220, "fo"),
            OnNext(230, "qux"),
            OnCompleted<string>(240)
        );

        var res = scheduler.Start(() => xs.Average(x => x == "fo" ? default(int?) : x.Length));

        res.Messages.AssertEqual(
            OnNext(240, (double?)2.0),
            OnCompleted<double?>(240)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 240)
        );
    }

    [TestMethod]
    public void Average_Selector_Regular_Int64_Nullable()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, "b"),
            OnNext(220, "fo"),
            OnNext(230, "qux"),
            OnCompleted<string>(240)
        );

        var res = scheduler.Start(() => xs.Average(x => x == "fo" ? default(long?) : x.Length));

        res.Messages.AssertEqual(
            OnNext(240, (double?)2.0),
            OnCompleted<double?>(240)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 240)
        );
    }

    [TestMethod]
    public void Average_Selector_Regular_Single_Nullable()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, "b"),
            OnNext(220, "fo"),
            OnNext(230, "qux"),
            OnCompleted<string>(240)
        );

        var res = scheduler.Start(() => xs.Average(x => x == "fo" ? default(float?) : x.Length));

        res.Messages.AssertEqual(
            OnNext(240, (float?)2.0),
            OnCompleted<float?>(240)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 240)
        );
    }

    [TestMethod]
    public void Average_Selector_Regular_Double_Nullable()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, "b"),
            OnNext(220, "fo"),
            OnNext(230, "qux"),
            OnCompleted<string>(240)
        );

        var res = scheduler.Start(() => xs.Average(x => x == "fo" ? default(double?) : x.Length));

        res.Messages.AssertEqual(
            OnNext(240, (double?)2.0),
            OnCompleted<double?>(240)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 240)
        );
    }

    [TestMethod]
    public void Average_Selector_Regular_Decimal_Nullable()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, "b"),
            OnNext(220, "fo"),
            OnNext(230, "qux"),
            OnCompleted<string>(240)
        );

        var res = scheduler.Start(() => xs.Average(x => x == "fo" ? default(decimal?) : x.Length));

        res.Messages.AssertEqual(
            OnNext(240, (decimal?)2.0),
            OnCompleted<decimal?>(240)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 240)
        );
    }

    [TestMethod]
    public void Average_Int64_Overflow()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1L),
            OnNext(210, long.MaxValue),
            OnNext(220, long.MaxValue),
            OnCompleted<long>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnError<double>(220, e => e is OverflowException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }

    [TestMethod]
    public void Average_Decimal_Overflow()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1m),
            OnNext(210, decimal.MaxValue),
            OnNext(220, decimal.MaxValue),
            OnCompleted<decimal>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnError<decimal>(220, e => e is OverflowException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }

    [TestMethod]
    public void Average_Nullable_Int64_Overflow()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, (long?)1),
            OnNext(210, (long?)long.MaxValue),
            OnNext(220, (long?)long.MaxValue),
            OnCompleted<long?>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnError<double?>(220, e => e is OverflowException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }

    [TestMethod]
    public void Average_Nullable_Decimal_Overflow()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, (decimal?)1),
            OnNext(210, (decimal?)decimal.MaxValue),
            OnNext(220, (decimal?)decimal.MaxValue),
            OnCompleted<decimal?>(250)
        );

        var res = scheduler.Start(() =>
            xs.Average()
        );

        res.Messages.AssertEqual(
            OnError<decimal?>(220, e => e is OverflowException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }
}
