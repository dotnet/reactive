// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// The shared <c>Average</c> scenarios on Rx.NET, plus its ten overflow-injection tests.
/// </summary>
/// <remarks>
/// The <c>Average_InjectOverflow_*</c> tests are the originals: each wraps a source so that the
/// subscribe sets the running count inside Rx.NET's sink to <c>long.MaxValue</c>, by reflection
/// on its private <c>_count</c> field, and then checks that the next element overflows it. That
/// reaches into Rx.NET's sink structure, which AsyncRx.NET does not share (its count is a local
/// of a closure), so they are written against Rx.NET directly, with the original's
/// <see cref="OverflowInjection{T}"/>, which <c>CountTest.cs</c> declares for the three files
/// that use it and which is a top-level type here for the same reason. They await the result
/// through <c>ToTask</c> where the original blocks on <c>ForEach</c>, which Rx.NET now marks
/// obsolete.
/// </remarks>
[TestClass]
public sealed class RxAverageTests : AverageTests
{
    protected override IRxTarget Target => RxTarget.Instance;

    [TestMethod]
    public async Task Average_InjectOverflow_Int32()
    {
        var xs = Observable.Return(
            42,
            global::System.Reactive.Concurrency.ThreadPoolScheduler.Instance);

        var res = new OverflowInjection<int>(xs, long.MaxValue).Average();

        await Assert.ThrowsExactlyAsync<OverflowException>(() => res.ToTask());
    }

    [TestMethod]
    public async Task Average_InjectOverflow_Int64()
    {
        var xs = Observable.Return(
            42L,
            global::System.Reactive.Concurrency.ThreadPoolScheduler.Instance);

        var res = new OverflowInjection<long>(xs, long.MaxValue).Average();

        await Assert.ThrowsExactlyAsync<OverflowException>(() => res.ToTask());
    }

    [TestMethod]
    public async Task Average_InjectOverflow_Double()
    {
        var xs = Observable.Return(
            42.0,
            global::System.Reactive.Concurrency.ThreadPoolScheduler.Instance);

        var res = new OverflowInjection<double>(xs, long.MaxValue).Average();

        await Assert.ThrowsExactlyAsync<OverflowException>(() => res.ToTask());
    }

    [TestMethod]
    public async Task Average_InjectOverflow_Single()
    {
        var xs = Observable.Return(
            42.0f,
            global::System.Reactive.Concurrency.ThreadPoolScheduler.Instance);

        var res = new OverflowInjection<float>(xs, long.MaxValue).Average();

        await Assert.ThrowsExactlyAsync<OverflowException>(() => res.ToTask());
    }

    [TestMethod]
    public async Task Average_InjectOverflow_Decimal()
    {
        var xs = Observable.Return(
            42.0m,
            global::System.Reactive.Concurrency.ThreadPoolScheduler.Instance);

        var res = new OverflowInjection<decimal>(xs, long.MaxValue).Average();

        await Assert.ThrowsExactlyAsync<OverflowException>(() => res.ToTask());
    }

    [TestMethod]
    public async Task Average_InjectOverflow_Int32_Nullable()
    {
        var xs = Observable.Return(
            (int?)42,
            global::System.Reactive.Concurrency.ThreadPoolScheduler.Instance);

        var res = new OverflowInjection<int?>(xs, long.MaxValue).Average();

        await Assert.ThrowsExactlyAsync<OverflowException>(() => res.ToTask());
    }

    [TestMethod]
    public async Task Average_InjectOverflow_Int64_Nullable()
    {
        var xs = Observable.Return(
            (long?)42L,
            global::System.Reactive.Concurrency.ThreadPoolScheduler.Instance);

        var res = new OverflowInjection<long?>(xs, long.MaxValue).Average();

        await Assert.ThrowsExactlyAsync<OverflowException>(() => res.ToTask());
    }

    [TestMethod]
    public async Task Average_InjectOverflow_Double_Nullable()
    {
        var xs = Observable.Return(
            (double?)42.0,
            global::System.Reactive.Concurrency.ThreadPoolScheduler.Instance);

        var res = new OverflowInjection<double?>(xs, long.MaxValue).Average();

        await Assert.ThrowsExactlyAsync<OverflowException>(() => res.ToTask());
    }

    [TestMethod]
    public async Task Average_InjectOverflow_Single_Nullable()
    {
        var xs = Observable.Return(
            (float?)42.0f,
            global::System.Reactive.Concurrency.ThreadPoolScheduler.Instance);

        var res = new OverflowInjection<float?>(xs, long.MaxValue).Average();

        await Assert.ThrowsExactlyAsync<OverflowException>(() => res.ToTask());
    }

    [TestMethod]
    public async Task Average_InjectOverflow_Decimal_Nullable()
    {
        var xs = Observable.Return(
            (decimal?)42.0m,
            global::System.Reactive.Concurrency.ThreadPoolScheduler.Instance);

        var res = new OverflowInjection<decimal?>(xs, long.MaxValue).Average();

        await Assert.ThrowsExactlyAsync<OverflowException>(() => res.ToTask());
    }

}
