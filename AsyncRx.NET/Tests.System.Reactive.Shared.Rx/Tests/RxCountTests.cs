// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// The shared <c>Count</c> scenarios on Rx.NET, plus its two overflow-injection tests.
/// </summary>
/// <remarks>
/// The <c>Count_*InjectOverflow</c> tests are the originals, over the shared
/// <see cref="OverflowInjection{T}"/>: each sets the running count inside Rx.NET's sink to
/// <c>int.MaxValue</c> and checks that the next element overflows it into the sequence's error.
/// They await the result through <c>ToTask</c> where the original blocks on <c>ForEach</c>,
/// which Rx.NET now marks obsolete.
/// </remarks>
[TestClass]
public sealed class RxCountTests : CountTests
{
    protected override IRxTarget Target => RxTarget.Instance;

    [TestMethod]
    public async Task Count_InjectOverflow()
    {
        var xs = Observable.Return(
            42,
            global::System.Reactive.Concurrency.ThreadPoolScheduler.Instance);

        var res = new OverflowInjection<int>(xs, int.MaxValue).Count();

        await Assert.ThrowsExactlyAsync<OverflowException>(() => res.ToTask());
    }

    [TestMethod]
    public async Task Count_Predicate_InjectOverflow()
    {
        var xs = Observable.Return(
            42,
            global::System.Reactive.Concurrency.ThreadPoolScheduler.Instance);

        var res = new OverflowInjection<int>(xs, int.MaxValue).Count(_ => true);

        await Assert.ThrowsExactlyAsync<OverflowException>(() => res.ToTask());
    }
}
