// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive;
using System.Reactive.Linq;

using Microsoft.Reactive.Testing;
using Microsoft.Reactive.Testing.Async;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Microsoft.Reactive.Testing.Async;

[TestClass]
public class AsyncRxDistinctTest : AsyncReactiveTest
{
    [TestMethod]
    [DataRow(ExecutionShape.SynchronousCompletion)]
    [DataRow(ExecutionShape.ForcedYield)]
    public void Distinct_DefaultComparer(ExecutionShape shape)
    {
        Verify(shape, xs => xs.Distinct(),
            OnNext(210, "a1"), OnNext(230, "A1"), OnNext(240, "b1"), OnNext(260, "a2"));
    }

    [TestMethod]
    [DataRow(ExecutionShape.SynchronousCompletion)]
    [DataRow(ExecutionShape.ForcedYield)]
    public void Distinct_CustomComparer(ExecutionShape shape)
    {
        Verify(shape, xs => xs.Distinct(StringComparer.OrdinalIgnoreCase),
            OnNext(210, "a1"), OnNext(240, "b1"), OnNext(260, "a2"));
    }

    [TestMethod]
    [DataRow(ExecutionShape.SynchronousCompletion)]
    [DataRow(ExecutionShape.ForcedYield)]
    public void DistinctUntilChanged_DefaultComparer(ExecutionShape shape)
    {
        Verify(shape, xs => xs.DistinctUntilChanged(),
            OnNext(210, "a1"), OnNext(230, "A1"), OnNext(240, "b1"), OnNext(250, "a1"), OnNext(260, "a2"));
    }

    [TestMethod]
    [DataRow(ExecutionShape.SynchronousCompletion)]
    [DataRow(ExecutionShape.ForcedYield)]
    public void DistinctUntilChanged_CustomComparer(ExecutionShape shape)
    {
        Verify(shape, xs => xs.DistinctUntilChanged(StringComparer.OrdinalIgnoreCase),
            OnNext(210, "a1"), OnNext(240, "b1"), OnNext(250, "a1"), OnNext(260, "a2"));
    }

    [TestMethod]
    [DataRow(ExecutionShape.SynchronousCompletion)]
    [DataRow(ExecutionShape.ForcedYield)]
    public void DistinctUntilChanged_KeySelector(ExecutionShape shape)
    {
        Verify(shape, xs => xs.DistinctUntilChanged(x => x[..1]),
            OnNext(210, "a1"), OnNext(230, "A1"), OnNext(240, "b1"), OnNext(250, "a1"));
    }

    [TestMethod]
    [DataRow(ExecutionShape.SynchronousCompletion)]
    [DataRow(ExecutionShape.ForcedYield)]
    public void DistinctUntilChanged_AsyncKeySelector(ExecutionShape shape)
    {
        Verify(shape, xs => xs.DistinctUntilChanged<string, string>(x => new ValueTask<string>(x[..1])),
            OnNext(210, "a1"), OnNext(230, "A1"), OnNext(240, "b1"), OnNext(250, "a1"));
    }

    [TestMethod]
    [DataRow(ExecutionShape.SynchronousCompletion)]
    [DataRow(ExecutionShape.ForcedYield)]
    public void DistinctUntilChanged_KeySelector_CustomComparer(ExecutionShape shape)
    {
        Verify(shape, xs => xs.DistinctUntilChanged(x => x[..1], StringComparer.OrdinalIgnoreCase),
            OnNext(210, "a1"), OnNext(240, "b1"), OnNext(250, "a1"));
    }

    [TestMethod]
    [DataRow(ExecutionShape.SynchronousCompletion)]
    [DataRow(ExecutionShape.ForcedYield)]
    public void DistinctUntilChanged_AsyncKeySelector_CustomComparer(ExecutionShape shape)
    {
        Verify(shape, xs => xs.DistinctUntilChanged<string, string>(x => new ValueTask<string>(x[..1]), StringComparer.OrdinalIgnoreCase),
            OnNext(210, "a1"), OnNext(240, "b1"), OnNext(250, "a1"));
    }

    private static void Verify(
        ExecutionShape shape,
        Func<IAsyncObservable<string>, IAsyncObservable<string>> create,
        params Recorded<Notification<string>>[] expected)
    {
        var scheduler = new TestAsyncScheduler(shape);
        var xs = scheduler.CreateHotObservable(
            OnNext(210, "a1"),
            OnNext(220, "a1"),
            OnNext(230, "A1"),
            OnNext(240, "b1"),
            OnNext(250, "a1"),
            OnNext(260, "a2"),
            OnNext(270, "a2"),
            OnCompleted<string>(280));

        var res = scheduler.Start(() => create(xs));

        res.Messages.AssertEqual(expected.Append(OnCompleted<string>(280)).ToArray());
        xs.Subscriptions.AssertEqual(Subscribe(200, 280));
    }
}
