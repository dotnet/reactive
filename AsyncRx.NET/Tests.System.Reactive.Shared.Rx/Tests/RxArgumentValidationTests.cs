// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Joins;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// Rx.NET's own reflective argument-validation scan, from its <c>ArgumentValidationTest.cs</c>.
/// </summary>
/// <remarks>
/// Both of the file's tests, as the originals but for the nullable context (a dictionary of
/// nullable values, a nullable argument array, <c>null!</c> where a null is the point), against
/// Rx.NET only: every public static method of <c>Observable</c> and <c>ObservableEx</c> is invoked
/// with each reference argument null in turn and must throw <c>ArgumentNullException</c>, and
/// every observable it returns must refuse a null subscriber. The code-generated stratum is this
/// suite's form of the same check over both libraries' operator surfaces, name by name; this is
/// Rx.NET's own en-masse form, kept as it is. The original's line comments walking through the
/// matrix are left out; its <c>IDE0300</c> suppression around the table of default values stays.
/// </remarks>
[TestClass]
public sealed class RxArgumentValidationTests
{
    [TestMethod]
    public void Verify_Observable()
    {
        VerifyClass(typeof(Observable));
    }

    [TestMethod]
    public void Verify_ObservableEx()
    {
        VerifyClass(typeof(ObservableEx));
    }

    private static readonly Dictionary<string, object?> _defaultValues;

    static RxArgumentValidationTests()
    {
#pragma warning disable IDE0300 // Simplify collection initialization. We want to be clear about what kinds of collections are in use in these tests.
        _defaultValues = new Dictionary<string, object?>
        {
            { "IObservable`1[Object]", Observable.Return(new object()) },
            { "IObservable`1[Int32]", Observable.Return(1) },
            { "IObservable`1[Task`1[Int32]]", Observable.Return(Task.FromResult(1)) },
            { "IObservable`1[Notification`1[Int32]]", Observable.Return(Notification.CreateOnNext(1)) },
            { "IObservable`1[Int64]", Observable.Return(1L) },
            { "IObservable`1[Double]", Observable.Return(1.0) },
            { "IObservable`1[Single]", Observable.Return(1.0f) },
            { "IObservable`1[Decimal]", Observable.Return(1.0m) },
            { "IObservable`1[Nullable`1[Int32]]", Observable.Return<int?>(1) },
            { "IObservable`1[Nullable`1[Int64]]", Observable.Return<long?>(1L) },
            { "IObservable`1[Nullable`1[Double]]", Observable.Return<double?>(1.0) },
            { "IObservable`1[Nullable`1[Single]]", Observable.Return<float?>(1.0f) },
            { "IObservable`1[Nullable`1[Decimal]]", Observable.Return<decimal?>(1.0m) },
            { "IObservable`1[IObservable`1[Int32]]", Observable.Return(Observable.Return(1)) },
            { "IObservable`1[][Int32]", new[] { Observable.Return(1) } },

            { "IConnectableObservable`1[Int32]", Observable.Return(1).Publish() },
            { "Int32", 1 },
            { "Int64", 1L },
            { "IScheduler", Scheduler.Immediate },
            { "TimeSpan", TimeSpan.FromMilliseconds(1) },
            { "DateTimeOffset", DateTimeOffset.Now },
            { "Object", new object() },
            { "Exception", new Exception() },
            { "String", "String" },
            { "Boolean", false },

            { "IDictionary`2[Int32, IObservable`1[Int32]]", new Dictionary<int, IObservable<int>>() },

            { "Type", typeof(object) },

            { "Int32[]", new[] { 1 } },

            { "ISubject`1[Int32]", new Subject<int>() },

            { "ISubject`2[Int32, Int32]", new Subject<int>() },

            { "IEnumerable`1[Int32]", new[] { 1 } },

            { "IEnumerable`1[IObservable`1[Int32]]", new[] { Observable.Return(1) } },

            { "SynchronizationContext", SynchronizationContext.Current ?? new SynchronizationContext() },

            { "IEqualityComparer`1[Int32]", EqualityComparer<int>.Default },

            { "IComparer`1[Int32]", Comparer<int>.Default },

            { "IObserver`1[Int32]", Observer.Create<int>(v => { }) },

            { "CancellationToken", new CancellationToken() },

            { "TaskObservationOptions", new TaskObservationOptions(null!, false) },

            { "Action", new Action(() => { }) },

            { "Action`1[Int32]", new Action<int>(v => { }) },
            { "Action`1[Exception]", new Action<Exception>(v => { }) },
            { "Action`1[IDisposable]", new Action<IDisposable>(v => { }) },
            { "Action`1[EventHandler]", new Action<EventHandler>(v => { }) },
            { "Action`1[EventHandler`1[Int32]]", new Action<EventHandler<int>>(v => { }) },
            { "Action`1[Action`1[Int32]]", new Action<Action<int>>(v => { }) },
            { "Action`1[Action]", new Action<Action>(v => { }) },
            { "Action`1[IAsyncResult]", new Action<IAsyncResult>(v => { }) },


            { "Action`2[Int32, Int32]", new Action<int, int>((v, u) => { }) },

            { "Func`1[Boolean]", new Func<bool>(() => true) },
            { "Func`1[Int32]", new Func<int>(() => 1) },
            { "Func`1[IObservable`1[Int32]]", new Func<IObservable<int>>(() => Observable.Return(1)) },
            { "Func`1[ISubject`2[Int32, Int32]]", new Func<ISubject<int, int>>(() => new Subject<int>()) },
            { "Func`1[Task`1[IObservable`1[Int32]]]", new Func<Task<IObservable<int>>>(() => Task.FromResult(Observable.Return(1))) },
            { "Func`1[IDisposable]", new Func<IDisposable>(() => Disposable.Empty) },
            { "Func`1[Task]", new Func<Task>(() => Task.FromResult(1)) },
            { "Func`1[Task`1[Int32]]", new Func<Task<int>>(() => Task.FromResult(1)) },
            { "Func`1[IEnumerable`1[IObservable`1[Object]]]", new Func<IEnumerable<IObservable<object>>>(() => new[] { Observable.Return((object)1) }) },

            { "Func`2[Int32, IObservable`1[Int32]]", new Func<int, IObservable<int>>(v => Observable.Return(v)) },
            { "Func`2[Exception, IObservable`1[Int32]]", new Func<Exception, IObservable<int>>(v => Observable.Return(1)) },
            { "Func`2[Int32, Task`1[Int32]]", new Func<int, Task<int>>(v => Task.FromResult(v)) },
            { "Func`2[Int32, Int32]", new Func<int, int>(v => v) },
            { "Func`2[Int32, IEnumerable`1[Int32]]", new Func<int, IEnumerable<int>>(v => new[] { v }) },
            { "Func`2[Int32, Boolean]", new Func<int, bool>(v => true) },
            { "Func`2[Int32, TimeSpan]", new Func<int, TimeSpan>(v => TimeSpan.FromMilliseconds(1)) },
            { "Func`2[Int32, DateTimeOffset]", new Func<int, DateTimeOffset>(v => DateTimeOffset.Now) },
            { "Func`2[IList`1[Int32], Int32]", new Func<IList<int>, int>(v => v.Count) },
            { "Func`2[Int32, Nullable`1[Double]]", new Func<int, double?>(v => v) },
            { "Func`2[Int32, Nullable`1[Single]]", new Func<int, float?>(v => v) },
            { "Func`2[Int32, Nullable`1[Int32]]", new Func<int, int?>(v => v) },
            { "Func`2[Int32, Nullable`1[Decimal]]", new Func<int, decimal?>(v => v) },
            { "Func`2[Int32, Nullable`1[Int64]]", new Func<int, long?>(v => v) },
            { "Func`2[Int32, Double]", new Func<int, double>(v => v) },
            { "Func`2[Int32, Single]", new Func<int, float>(v => v) },
            { "Func`2[Int32, Decimal]", new Func<int, decimal>(v => v) },
            { "Func`2[Int32, Int64]", new Func<int, long>(v => v) },
            { "Func`2[IObservable`1[Object], IObservable`1[Int32]]", new Func<IObservable<object>, IObservable<int>>(v => v.Select(w => 1)) },
            { "Func`2[IObservable`1[Exception], IObservable`1[Int32]]", new Func<IObservable<Exception>, IObservable<int>>(v => v.Select(w => 1)) },
            { "Func`2[IGroupedObservable`2[Int32, Int32], IObservable`1[Int32]]", new Func<IGroupedObservable<int, int>, IObservable<int>>(v => v) },
            { "Func`2[IObservable`1[Int32], IObservable`1[Int32]]", new Func<IObservable<int>, IObservable<int>>(v => v.Select(w => 1)) },
            { "Func`2[CancellationToken, Task`1[IObservable`1[Int32]]]", new Func<CancellationToken, Task<IObservable<int>>>(v => Task.FromResult(Observable.Return(1))) },
            { "Func`2[IDisposable, Task`1[IObservable`1[Int32]]]", new Func<IDisposable, Task<IObservable<int>>>(v => Task.FromResult(Observable.Return(1))) },
            { "Func`2[IDisposable, IObservable`1[Int32]]", new Func<IDisposable, IObservable<int>>(v => Observable.Return(1)) },
            { "Func`2[CancellationToken, Task`1[IDisposable]]", new Func<CancellationToken, Task<IDisposable>>(v => Task.FromResult(Disposable.Empty)) },
            { "Func`2[EventHandler`1[Int32], Int32]", new Func<EventHandler<int>, int>(v => 1) },
            { "Func`2[Action`1[Int32], Int32]", new Func<Action<int>, int>(v => 1) },
            { "Func`2[IObserver`1[Int32], IDisposable]", new Func<IObserver<int>, IDisposable>(v => Disposable.Empty) },
            { "Func`2[IObserver`1[Int32], Action]", new Func<IObserver<int>, Action>(v => () => { }) },
            { "Func`2[IObserver`1[Int32], Task]", new Func<IObserver<int>, Task>(v => Task.FromResult(1)) },
            { "Func`2[IObserver`1[Int32], Task`1[IDisposable]]", new Func<IObserver<int>, Task<IDisposable>>(v => Task.FromResult(Disposable.Empty)) },
            { "Func`2[IObserver`1[Int32], Task`1[Action]]", new Func<IObserver<int>, Task<Action>>(v => Task.FromResult<Action>(() => { })) },
            { "Func`2[CancellationToken, Task]", new Func<CancellationToken, Task>(v => Task.FromResult(1)) },
            { "Func`2[CancellationToken, Task`1[Int32]]", new Func<CancellationToken, Task<int>>(v => Task.FromResult(1)) },
            { "Func`2[IAsyncResult, Int32]", new Func<IAsyncResult, int>(v => 1) },
            { "Func`2[IObserver`1[Int32], IEnumerable`1[IObservable`1[Object]]]", new Func<IObserver<int>, IEnumerable<IObservable<object>>>(v => new[] { Observable.Return((object)1) }) },
            { "Func`2[IObservable`1[Int32], Int32]", new Func<IObservable<int>, int>(v => 1) },

            { "Func`3[Int32, Int32, IObservable`1[Int32]]", new Func<int, int, IObservable<int>>((v, u) => Observable.Return(v + u)) },
            { "Func`3[Int32, Int32, Task`1[Int32]]", new Func<int, int, Task<int>>((v, u) => Task.FromResult(v + u)) },
            { "Func`3[Int32, CancellationToken, Task`1[Int32]]", new Func<int, CancellationToken, Task<int>>((v, u) => Task.FromResult(v)) },
            { "Func`3[Int32, Int32, Int32]", new Func<int, int, int>((v, u) => v + u) },
            { "Func`3[Int32, Int32, IEnumerable`1[Int32]]", new Func<int, int, IEnumerable<int>>((v, u) => new[] { v, u }) },
            { "Func`3[Int32, Int32, Boolean]", new Func<int, int, bool>((v, u) => true) },
            { "Func`3[Int32, IObservable`1[Int32], Int32]", new Func<int, IObservable<int>, int>((v, u) => v) },
            { "Func`3[IDisposable, CancellationToken, Task`1[IObservable`1[Int32]]]", new Func<IDisposable, CancellationToken, Task<IObservable<int>>>((v, u) => Task.FromResult(Observable.Return(1))) },
            { "Func`3[IObserver`1[Int32], CancellationToken, Task]", new Func<IObserver<int>, CancellationToken, Task>((v, w) => Task.FromResult(1)) },
            { "Func`3[IObserver`1[Int32], CancellationToken, Task`1[IDisposable]]", new Func<IObserver<int>, CancellationToken, Task<IDisposable>>((v, w) => Task.FromResult(Disposable.Empty)) },
            { "Func`3[IObserver`1[Int32], CancellationToken, Task`1[Action]]", new Func<IObserver<int>, CancellationToken, Task<Action>>((v, w) => Task.FromResult<Action>(() => { })) },
            { "Func`3[AsyncCallback, Object, IAsyncResult]", new Func<AsyncCallback, object, IAsyncResult>((v, w) => null!) },

            { "Func`4[Int32, Int32, CancellationToken, Task`1[Int32]]", new Func<int, int, CancellationToken, Task<int>>((v, u, w) => Task.FromResult(v)) },
            { "Func`4[Int32, Int32, Int32, Int32]", new Func<int, int, int, int>((v1, v2, v3) => v1 + v2 + v3) },
            { "Func`4[Int32, AsyncCallback, Object, IAsyncResult]", new Func<int, AsyncCallback, object, IAsyncResult>((v, w, x) => null!) },

            { "Func`5[Int32, Int32, Int32, Int32, Int32]", new Func<int, int, int, int, int>((v1, v2, v3, v4) => v1 + v2 + v3 + v4) },

            { "Func`6[Int32, Int32, Int32, Int32, Int32, Int32]", new Func<int, int, int, int, int, int>((v1, v2, v3, v4, v5) => v1 + v2 + v3 + v4 + v5) },

            { "Func`7[Int32, Int32, Int32, Int32, Int32, Int32, Int32]", new Func<int, int, int, int, int, int, int>((v1, v2, v3, v4, v5, v6) => v1 + v2 + v3 + v4 + v5 + v6) },

            { "Func`8[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32]", new Func<int, int, int, int, int, int, int, int>((v1, v2, v3, v4, v5, v6, v7) => v1 + v2 + v3 + v4 + v5 + v6 + v7) },

            { "Func`9[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32]", new Func<int, int, int, int, int, int, int, int, int>((v1, v2, v3, v4, v5, v6, v7, v8) => v1 + v2 + v3 + v4 + v5 + v6 + v7 + v8) },

            { "Func`10[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32]", new Func<int, int, int, int, int, int, int, int, int, int>((v1, v2, v3, v4, v5, v6, v7, v8, v9) => v1 + v2 + v3 + v4 + v5 + v6 + v7 + v8 + v9) },

            { "Func`11[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32]", new Func<int, int, int, int, int, int, int, int, int, int, int>((v1, v2, v3, v4, v5, v6, v7, v8, v9, v10) => v1 + v2 + v3 + v4 + v5 + v6 + v7 + v8 + v9 + v10) },

            { "Func`12[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32]", new Func<int, int, int, int, int, int, int, int, int, int, int, int>((v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11) => v1 + v2 + v3 + v4 + v5 + v6 + v7 + v8 + v9 + v10 + v11) },

            { "Func`13[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32]", new Func<int, int, int, int, int, int, int, int, int, int, int, int, int>((v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12) => v1 + v2 + v3 + v4 + v5 + v6 + v7 + v8 + v9 + v10 + v11 + v12) },

            { "Func`14[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32]", new Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13) => v1 + v2 + v3 + v4 + v5 + v6 + v7 + v8 + v9 + v10 + v11 + v12 + v13) },

            { "Func`15[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32]", new Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13, v14) => v1 + v2 + v3 + v4 + v5 + v6 + v7 + v8 + v9 + v10 + v11 + v12 + v13 + v14) },

            { "Func`16[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32]", new Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13, v14, v15) => v1 + v2 + v3 + v4 + v5 + v6 + v7 + v8 + v9 + v10 + v11 + v12 + v13 + v14 + v15) },

            { "Func`17[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32]", new Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13, v14, v15, v16) => v1 + v2 + v3 + v4 + v5 + v6 + v7 + v8 + v9 + v10 + v11 + v12 + v13 + v14 + v15 + v16) },

            {
                "Plan`1[][Int32]",
                new Plan<int>[] {
                Observable.Return(1).Then(v => v)
        }
            },

            {
                "IEnumerable`1[Plan`1[Int32]]",
                new Plan<int>[] {
                Observable.Return(1).Then(v => v)
        }
            },

            { "Action`3[Int32, Int32, Int32]", new Action<int, int, int>((v1, v2, v3) => { }) },

            { "Action`4[Int32, Int32, Int32, Int32]", new Action<int, int, int, int>((v1, v2, v3, v4) => { }) },

            { "Action`5[Int32, Int32, Int32, Int32, Int32]", new Action<int, int, int, int, int>((v1, v2, v3, v4, v5) => { }) },

            { "Action`6[Int32, Int32, Int32, Int32, Int32, Int32]", new Action<int, int, int, int, int, int>((v1, v2, v3, v4, v5, v6) => { }) },

            { "Action`7[Int32, Int32, Int32, Int32, Int32, Int32, Int32]", new Action<int, int, int, int, int, int, int>((v1, v2, v3, v4, v5, v6, v7) => { }) },

            { "Action`8[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32]", new Action<int, int, int, int, int, int, int, int>((v1, v2, v3, v4, v5, v6, v7, v8) => { }) },

            { "Action`9[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32]", new Action<int, int, int, int, int, int, int, int, int>((v1, v2, v3, v4, v5, v6, v7, v8, v9) => { }) },

            { "Action`10[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32]", new Action<int, int, int, int, int, int, int, int, int, int>((v1, v2, v3, v4, v5, v6, v7, v8, v9, v10) => { }) },

            { "Action`11[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32]", new Action<int, int, int, int, int, int, int, int, int, int, int>((v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11) => { }) },

            { "Action`12[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32]", new Action<int, int, int, int, int, int, int, int, int, int, int, int>((v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12) => { }) },

            { "Action`13[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32]", new Action<int, int, int, int, int, int, int, int, int, int, int, int, int>((v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13) => { }) },

            { "Action`14[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32]", new Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13, v14) => { }) },

            { "Action`15[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32]", new Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13, v14, v15) => { }) },

            { "Action`16[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32]", new Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13, v14, v15, v16) => { }) },

            { "Func`5[Int32, Int32, AsyncCallback, Object, IAsyncResult]", new Func<int, int, AsyncCallback, object, IAsyncResult>((v1, v2, v3, v4) => null!) },

            { "Func`6[Int32, Int32, Int32, AsyncCallback, Object, IAsyncResult]", new Func<int, int, int, AsyncCallback, object, IAsyncResult>((v1, v2, v3, v4, v5) => null!) },

            { "Func`7[Int32, Int32, Int32, Int32, AsyncCallback, Object, IAsyncResult]", new Func<int, int, int, int, AsyncCallback, object, IAsyncResult>((v1, v2, v3, v4, v5, v6) => null!) },

            { "Func`8[Int32, Int32, Int32, Int32, Int32, AsyncCallback, Object, IAsyncResult]", new Func<int, int, int, int, int, AsyncCallback, object, IAsyncResult>((v1, v2, v3, v4, v5, v6, v7) => null!) },

            { "Func`9[Int32, Int32, Int32, Int32, Int32, Int32, AsyncCallback, Object, IAsyncResult]", new Func<int, int, int, int, int, int, AsyncCallback, object, IAsyncResult>((v1, v2, v3, v4, v5, v6, v7, v8) => null!) },

            { "Func`10[Int32, Int32, Int32, Int32, Int32, Int32, Int32, AsyncCallback, Object, IAsyncResult]", new Func<int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult>((v1, v2, v3, v4, v5, v6, v7, v8, v9) => null!) },

            { "Func`11[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, AsyncCallback, Object, IAsyncResult]", new Func<int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult>((v1, v2, v3, v4, v5, v6, v7, v8, v9, v10) => null!) },

            { "Func`12[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, AsyncCallback, Object, IAsyncResult]", new Func<int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult>((v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11) => null!) },

            { "Func`13[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, AsyncCallback, Object, IAsyncResult]", new Func<int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult>((v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12) => null!) },

            { "Func`14[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, AsyncCallback, Object, IAsyncResult]", new Func<int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult>((v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13) => null!) },

            { "Func`15[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, AsyncCallback, Object, IAsyncResult]", new Func<int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult>((v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13, v14) => null!) },

            { "Func`16[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, AsyncCallback, Object, IAsyncResult]", new Func<int, int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult>((v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13, v14, v15) => null!) },

            { "Func`17[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, AsyncCallback, Object, IAsyncResult]", new Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult>((v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13, v14, v15, v16) => null!) }
        };
#pragma warning restore IDE0300 // Simplify collection initialization
    }

    private static void VerifyClass(Type type)
    {
        foreach (var method in type.GetMethods())
        {

            if (!method.IsPublic || !method.IsStatic)
            {
                continue;
            }

            MethodInfo m;

            if (method.IsGenericMethodDefinition)
            {

                var ga = method.GetGenericArguments();

                var targs = new Type[ga.Length];

                for (var k = 0; k < targs.Length; k++)
                {
                    var gac = ga[k].GetGenericParameterConstraints();
                    if (gac.Length == 0)
                    {
                        targs[k] = typeof(int);
                    }
                    else if (gac[0] == typeof(Exception))
                    {
                        targs[k] = typeof(Exception);
                    }
                    else if (gac[0] == typeof(IDisposable))
                    {
                        targs[k] = typeof(IDisposable);
                    }
                    else
                    {
                        throw new Exception("Unknown constraint: " + gac + "\r\n" + method);
                    }
                }

                try
                {
                    m = method.MakeGenericMethod(targs);
                }
                catch (Exception ex)
                {
                    throw new Exception("MakeGenericMethod threw: " + method, ex);
                }
            }
            else
            {
                m = method;
            }

            var args = m.GetParameters();

            for (var i = 0; i < args.Length; i++)
            {
                var margs = new object?[args.Length];

                var argumentCanBeNull = true;

                for (var j = 0; j < args.Length; j++)
                {
                    var pt = args[j].ParameterType;
                    var paramTypeName = TypeNameOf(pt);

                    if (j == i && (pt.IsClass || pt.IsInterface || pt.IsArray || pt.IsAbstract))
                    {
                        margs[j] = null;
                        argumentCanBeNull = args[j].HasDefaultValue && args[j].DefaultValue == null;
                    }
                    else
                    {
                        if (_defaultValues.ContainsKey(paramTypeName))
                        {
                            margs[j] = _defaultValues[paramTypeName];
                        }
                        else
                        {
                            throw new Exception("Default instance not found for: " + paramTypeName + "\r\n\r\n" + m);
                        }
                    }
                }

                var thrown = true;
                var obj = default(object);
                try
                {
                    obj = m.Invoke(null, margs);
                    thrown = false;
                }
                catch (ArgumentNullException)
                {
                }
                catch (Exception ex)
                {
                    if (ex.InnerException is not ArgumentNullException)
                    {
                        throw new Exception("Method threw: " + method + " @ " + i, ex);
                    }
                }
                if (!thrown && !argumentCanBeNull)
                {
                    throw new Exception("Should have thrown: " + method + " @ " + i);
                }
                if (obj == null && !thrown)
                {
                    throw new NullReferenceException("null return: " + method + " @ " + i);
                }
            }

            if (m.ReturnType.Name.Equals("IObservable`1")
                || m.ReturnType.Name.Equals("IConnectableObservable`1"))
            {
                if (m.Name.Equals("FromEventPattern"))
                {
                    continue;
                }

                var margs = new object?[args.Length];

                for (var j = 0; j < args.Length; j++)
                {
                    var pt = args[j].ParameterType;
                    var paramTypeName = TypeNameOf(pt);
                    if (_defaultValues.ContainsKey(paramTypeName))
                    {
                        margs[j] = _defaultValues[paramTypeName];
                    }
                    else
                    {
                        throw new Exception("Default instance not found (Subscribe(null) check): " + paramTypeName + "\r\n\r\n" + m);
                    }
                }

                var thrown = true;
                try
                {

                    if (m.Invoke(null, margs) is IObservable<int> o)
                    {
                        o.Subscribe(null!);

                        thrown = false;
                    }
                }
                catch (ArgumentNullException)
                {
                }
                catch (Exception ex)
                {
                    throw new Exception("Method threw (Subscribe(null) check): " + m, ex);
                }
                if (!thrown)
                {
                    throw new Exception("Should have thrown (Subscribe(null) check): " + m);
                }
            }
        }
    }

    private static string TypeNameOf(Type type)
    {
        var ga = type.GetGenericArguments();
        if (ga.Length == 0)
        {
            return type.Name;
        }

        return type.Name + "[" + string.Join(", ", ga.Select(t => TypeNameOf(t)).ToArray()) + "]";
    }
}
