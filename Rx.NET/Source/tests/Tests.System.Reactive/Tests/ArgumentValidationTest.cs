// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Joins;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ReactiveTests.Tests
{
    /// <summary>
    /// Check if the Observable operator methods perform the proper
    /// argument validations en-masse with reflective checks.
    /// </summary>
    public class ArgumentValidationTest
    {
        #region + Default values for the generic types +

        /// <summary>
        /// Contains a map of various types, represented
        /// as strings generated via <see cref="TypeNameOf(Type)"/>,
        /// mapped to a value.
        /// </summary>
        private static Dictionary<string, object> _defaultValues;

        /// <summary>
        /// Prepare the default instances for various types used
        /// throughout Rx.NET.
        /// </summary>
        static ArgumentValidationTest()
        {
            _defaultValues = new Dictionary<string, object>
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

                { "IDictionary`2[Int32, IObservable`1[Int32]]", new Dictionary<int, IObservable<int>>() },

                { "Type", typeof(object) },

                { "Int32[]", new[] { 1 } },

                { "ISubject`1[Int32]", new Subject<int>() },

                { "ISubject`2[Int32, Int32]", new Subject<int>() },

                { "IEnumerable`1[Int32]", new[] { 1 } },

                { "IEnumerable`1[IObservable`1[Int32]]", new[] { Observable.Return(1) } },

                { "SynchronizationContext", SynchronizationContext.Current },

                { "IEqualityComparer`1[Int32]", EqualityComparer<int>.Default },

                { "IComparer`1[Int32]", Comparer<int>.Default },

                { "IObserver`1[Int32]", Observer.Create<int>(v => { }) },

                { "CancellationToken", new CancellationToken() },

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
                { "Func`3[AsyncCallback, Object, IAsyncResult]", new Func<AsyncCallback, object, IAsyncResult>((v, w) => null) },

                { "Func`4[Int32, Int32, CancellationToken, Task`1[Int32]]", new Func<int, int, CancellationToken, Task<int>>((v, u, w) => Task.FromResult(v)) },
                { "Func`4[Int32, Int32, Int32, Int32]", new Func<int, int, int, int>((v1, v2, v3) => v1 + v2 + v3) },
                { "Func`4[Int32, AsyncCallback, Object, IAsyncResult]", new Func<int, AsyncCallback, object, IAsyncResult>((v, w, x) => null) },

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

                { "Func`5[Int32, Int32, AsyncCallback, Object, IAsyncResult]", new Func<int, int, AsyncCallback, object, IAsyncResult>((v1, v2, v3, v4) => null) },

                { "Func`6[Int32, Int32, Int32, AsyncCallback, Object, IAsyncResult]", new Func<int, int, int, AsyncCallback, object, IAsyncResult>((v1, v2, v3, v4, v5) => null) },

                { "Func`7[Int32, Int32, Int32, Int32, AsyncCallback, Object, IAsyncResult]", new Func<int, int, int, int, AsyncCallback, object, IAsyncResult>((v1, v2, v3, v4, v5, v6) => null) },

                { "Func`8[Int32, Int32, Int32, Int32, Int32, AsyncCallback, Object, IAsyncResult]", new Func<int, int, int, int, int, AsyncCallback, object, IAsyncResult>((v1, v2, v3, v4, v5, v6, v7) => null) },

                { "Func`9[Int32, Int32, Int32, Int32, Int32, Int32, AsyncCallback, Object, IAsyncResult]", new Func<int, int, int, int, int, int, AsyncCallback, object, IAsyncResult>((v1, v2, v3, v4, v5, v6, v7, v8) => null) },

                { "Func`10[Int32, Int32, Int32, Int32, Int32, Int32, Int32, AsyncCallback, Object, IAsyncResult]", new Func<int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult>((v1, v2, v3, v4, v5, v6, v7, v8, v9) => null) },

                { "Func`11[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, AsyncCallback, Object, IAsyncResult]", new Func<int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult>((v1, v2, v3, v4, v5, v6, v7, v8, v9, v10) => null) },

                { "Func`12[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, AsyncCallback, Object, IAsyncResult]", new Func<int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult>((v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11) => null) },

                { "Func`13[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, AsyncCallback, Object, IAsyncResult]", new Func<int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult>((v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12) => null) },

                { "Func`14[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, AsyncCallback, Object, IAsyncResult]", new Func<int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult>((v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13) => null) },

                { "Func`15[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, AsyncCallback, Object, IAsyncResult]", new Func<int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult>((v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13, v14) => null) },

                { "Func`16[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, AsyncCallback, Object, IAsyncResult]", new Func<int, int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult>((v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13, v14, v15) => null) },

                { "Func`17[Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, AsyncCallback, Object, IAsyncResult]", new Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult>((v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13, v14, v15, v16) => null) }
            };
        }

        #endregion

        [Fact]
        public void Verify_Observable()
        {
            VerifyClass(typeof(Observable));
        }

        [Fact]
        public void Verify_ObservableEx()
        {
            VerifyClass(typeof(ObservableEx));
        }

        #region + Verification method +

        /// <summary>
        /// Verify that public static members of the class
        /// check for nulls in their arguments as
        /// well as when invoking Subscribe with null.
        /// </summary>
        /// <param name="type">The type to verify.</param>
        private static void VerifyClass(Type type)
        {
            foreach (var method in type.GetMethods())
            {
                // public static only (skip methods like Equals)

                if (!method.IsPublic || !method.IsStatic)
                {
                    continue;
                }

                var m = default(MethodInfo);

                // Is this a generic method?
                if (method.IsGenericMethodDefinition)
                {
                    // we need to specialize it to concrete types
                    // for the reflective call to work

                    // get the type arguments
                    var ga = method.GetGenericArguments();

                    var targs = new Type[ga.Length];

                    // fill in the type arguments
                    for (var k = 0; k < targs.Length; k++)
                    {
                        // watch out for type constrains
                        // the default typeof(int) will not work when
                        // exception or IDisposable is required at minimum
                        var gac = ga[k].GetGenericParameterConstraints();
                        // no type constraints
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
                            // If we get here, a new rule should be added above
                            throw new Exception("Unknown constraint: " + gac + "\r\n" + method);
                        }
                    }

                    // generate a specialized method with the concrete generic arguments
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
                    // non generic method, we can invoke this directly
                    m = method;
                }


                var args = m.GetParameters();

                // for each parameter of the (generic) method
                for (var i = 0; i < args.Length; i++)
                {
                    // prepare a pattern for the method invocation
                    var margs = new object[args.Length];

                    // some arguments can be null, often indicated with a default == null marker
                    // this tracks this case and forgives for not throwing an ArgumentNullException
                    var argumentCanBeNull = true;

                    // for each argument index
                    // with the loop i, this creates an N x N matrix where in each row, one argument is null
                    for (var j = 0; j < args.Length; j++)
                    {
                        // figure out the type of the argument
                        var pt = args[j].ParameterType;
                        // by using some type naming convention as string
                        var paramTypeName = TypeNameOf(pt);

                        // classes, interfaces, arrays and abstract markers can be null
                        // for the diagonal entries of the test matrix
                        if (j == i && (pt.IsClass || pt.IsInterface || pt.IsArray || pt.IsAbstract))
                        {
                            margs[j] = null;
                            // check if the argument can be actually
                            argumentCanBeNull = args[j].HasDefaultValue && args[j].DefaultValue == null;
                        }
                        else
                        {
                            // this argument is not tested for null or is not a null type
                            // find the default instance for it
                            if (_defaultValues.ContainsKey(paramTypeName))
                            {
                                margs[j] = _defaultValues[paramTypeName];
                            }
                            else
                            {
                                // default values have to be instantiated in _defaultValues for
                                // each possible generic type arguments.
                                // this will indicate what concrete instance value is missing.
                                throw new Exception("Default instance not found for: " + paramTypeName + "\r\n\r\n" + m);
                            }
                        }
                    }

                    // assume it threw
                    var thrown = true;
                    var obj = default(object);
                    try
                    {
                        obj = m.Invoke(null, margs);
                        thrown = false;
                    }
                    catch (ArgumentNullException)
                    {
                        // expected exception, just in case
                    }
                    catch (Exception ex)
                    {
                        // reflection wraps the actual exception, let's unwrap it
                        if (!(ex.InnerException is ArgumentNullException))
                        {
                            throw new Exception("Method threw: " + method + " @ " + i, ex);
                        }
                    }
                    // if the call didn't throw and the argument being tested isn't defaulted to null, throw
                    if (!thrown && !argumentCanBeNull)
                    {
                        throw new Exception("Should have thrown: " + method + " @ " + i);
                    }
                    // if the call didn't throw and returned a null object, throw
                    // no operators should return null
                    if (obj == null && !thrown)
                    {
                        throw new NullReferenceException("null return: " + method + " @ " + i);
                    }
                }

                // Now check the same method with valid arguments but
                // Subscribe(null) if it returns an IObservable subclass
                if (m.ReturnType.Name.Equals("IObservable`1")
                    || m.ReturnType.Name.Equals("IConnectableObservable`1"))
                {
                    // these will fail other argument validation with the defaults, skip them
                    if (m.Name.Equals("FromEventPattern"))
                    {
                        continue;
                    }

                    // prepare method arguments
                    var margs = new object[args.Length];

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
                            // default values have to be instantiated in _defaultValues for
                            // each possible generic type arguments.
                            // this will indicate what concrete instance value is missing.
                            //
                            // it may fail independently of the null test above because
                            // the particular type is non-nullable thus skipped above
                            // or was the solo argument and it got never tested with a non-null
                            // value
                            throw new Exception("Default instance not found (Subscribe(null) check): " + paramTypeName + "\r\n\r\n" + m);
                        }
                    }

                    // Assume it throws
                    var thrown = true;
                    try
                    {

                        // Should not return null, but would be mistaken for
                        // throwing because of Subscribe(null)
                        if (m.Invoke(null, margs) is IObservable<int> o)
                        {
                            o.Subscribe(null);

                            thrown = false;
                        }
                    }
                    catch (ArgumentNullException)
                    {
                        // expected
                    }
                    catch (Exception ex)
                    {
                        // Unexpected exception
                        // Maybe some other validation failed inside the method call
                        // Consider skipping this method (set)
                        //
                        // Otherwise, the operator may run with the null IObserver
                        // for a while and crash later.
                        throw new Exception("Method threw (Subscribe(null) check): " + m, ex);
                    }
                    // If it didn't throw, report it
                    if (!thrown)
                    {
                        throw new Exception("Should have thrown (Subscribe(null) check): " + m);
                    }
                }
            }
        }

        /// <summary>
        /// Generate a string representation of a possibly generic type
        /// that is not verbose (i.e, no "System." everywhere).
        /// </summary>
        /// <param name="type">The type to get a string representation</param>
        /// <returns>The string representation of a possibly generic type</returns>
        private static string TypeNameOf(Type type)
        {
            var ga = type.GetGenericArguments();
            if (ga.Length == 0)
            {
                return type.Name;
            }

            return type.Name + "[" + string.Join(", ", ga.Select(t => TypeNameOf(t)).ToArray()) + "]";
        }

        #endregion
    }
}
