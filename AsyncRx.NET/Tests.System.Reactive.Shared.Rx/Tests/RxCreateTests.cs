// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// The shared <c>Create</c> scenarios on Rx.NET, plus its two <c>*_ObserverThrows</c> tests.
/// </summary>
/// <remarks>
/// The two are the originals, over Rx.NET's synchronous <c>Create</c>: a handler's exception
/// thrown through the observer leaves the callback synchronously, and <c>ObservableBase</c>
/// rethrows it from <c>Subscribe</c> because the observer has already stopped. The task-based
/// <c>Create</c>, which the shared asynchronous callbacks realize as on both targets, has no
/// such path: the callback's task faults, and the fault goes to an observer that has already
/// stopped. So the tests run here against the synchronous form only, as the originals but for
/// MSTest's assertions.
/// </remarks>
[TestClass]
public sealed class RxCreateTests : CreateTests
{
    protected override IRxTarget Target => RxTarget.Instance;

    [TestMethod]
    public void Create_ObserverThrows()
    {
        Assert.ThrowsExactly<InvalidOperationException>(() =>
            Observable.Create<int>(o =>
            {
                o.OnNext(1);
                return () => { };
            }).Subscribe(x => { throw new InvalidOperationException(); }));
        Assert.ThrowsExactly<InvalidOperationException>(() =>
            Observable.Create<int>(o =>
            {
                o.OnError(new Exception());
                return () => { };
            }).Subscribe(x => { }, ex => { throw new InvalidOperationException(); }));
        Assert.ThrowsExactly<InvalidOperationException>(() =>
            Observable.Create<int>(o =>
            {
                o.OnCompleted();
                return () => { };
            }).Subscribe(x => { }, ex => { }, () => { throw new InvalidOperationException(); }));
    }

    [TestMethod]
    public void CreateWithDisposable_ObserverThrows()
    {
        Assert.ThrowsExactly<InvalidOperationException>(() =>
            Observable.Create<int>(o =>
            {
                o.OnNext(1);
                return Disposable.Empty;
            }).Subscribe(x => { throw new InvalidOperationException(); }));
        Assert.ThrowsExactly<InvalidOperationException>(() =>
            Observable.Create<int>(o =>
            {
                o.OnError(new Exception());
                return Disposable.Empty;
            }).Subscribe(x => { }, ex => { throw new InvalidOperationException(); }));
        Assert.ThrowsExactly<InvalidOperationException>(() =>
            Observable.Create<int>(o =>
            {
                o.OnCompleted();
                return Disposable.Empty;
            }).Subscribe(x => { }, ex => { }, () => { throw new InvalidOperationException(); }));
    }
}
