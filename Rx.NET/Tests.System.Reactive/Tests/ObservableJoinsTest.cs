// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reactive.Joins;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTests.Tests
{
    [TestClass]
    public partial class ObservableWhensTest : ReactiveTest
    {
        #region And

        [TestMethod]
        public void And_ArgumentChecking()
        {
            var someObservable = Observable.Return(1);
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(null, someObservable));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(someObservable, null));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(someObservable, someObservable).And<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(someObservable, someObservable).And(someObservable).And<int>(null));

#if !NO_LARGEARITY
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(someObservable, someObservable).And(someObservable).And(someObservable).And<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(someObservable, someObservable).And(someObservable).And(someObservable).And(someObservable).And<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(someObservable, someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(someObservable, someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(someObservable, someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(someObservable, someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(someObservable, someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(someObservable, someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(someObservable, someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(someObservable, someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(someObservable, someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(someObservable, someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And<int>(null));
#endif
        }

        [TestMethod]
        public void And2()
        {
            var scheduler = new TestScheduler();

            const int N = 2;

            var obs = new List<IObservable<int>>();
            for (int i = 0; i < N; i++)
            {
                obs.Add(scheduler.CreateHotObservable(
                    OnNext(210, 1),
                    OnCompleted<int>(220)
                ));
            }

            var res = scheduler.Start(() =>
                Observable.When(obs[0].And(obs[1]).Then((a, b) => a + b))
            );

            res.Messages.AssertEqual(
                OnNext(210, N),
                OnCompleted<int>(220)
            );
        }

        [TestMethod]
        public void And2Error()
        {
            var ex = new Exception();

            const int N = 2;

            for (int i = 0; i < N; i++)
            {
                var scheduler = new TestScheduler();
                var obs = new List<IObservable<int>>();

                for (int j = 0; j < N; j++)
                {
                    if (j == i)
                    {
                        obs.Add(scheduler.CreateHotObservable(
                            OnError<int>(210, ex)
                        ));
                    }
                    else
                    {
                        obs.Add(scheduler.CreateHotObservable(
                            OnNext(210, 1),
                            OnCompleted<int>(220)
                        ));
                    }
                }

                var res = scheduler.Start(() =>
                    Observable.When(obs[0].And(obs[1]).Then((a, b) => 0))
                );

                res.Messages.AssertEqual(
                    OnError<int>(210, ex)
                );
            }
        }

        [TestMethod]
        public void And3()
        {
            var scheduler = new TestScheduler();

            const int N = 3;

            var obs = new List<IObservable<int>>();
            for (int i = 0; i < N; i++)
            {
                obs.Add(scheduler.CreateHotObservable(
                    OnNext(210, 1),
                    OnCompleted<int>(220)
                ));
            }

            var res = scheduler.Start(() =>
                Observable.When(obs[0].And(obs[1]).And(obs[2]).Then((a, b, c) => a + b + c))
            );

            res.Messages.AssertEqual(
                OnNext(210, N),
                OnCompleted<int>(220)
            );
        }

        [TestMethod]
        public void And3Error()
        {
            var ex = new Exception();

            const int N = 3;

            for (int i = 0; i < N; i++)
            {
                var scheduler = new TestScheduler();
                var obs = new List<IObservable<int>>();

                for (int j = 0; j < N; j++)
                {
                    if (j == i)
                    {
                        obs.Add(scheduler.CreateHotObservable(
                            OnError<int>(210, ex)
                        ));
                    }
                    else
                    {
                        obs.Add(scheduler.CreateHotObservable(
                            OnNext(210, 1),
                            OnCompleted<int>(220)
                        ));
                    }
                }

                var res = scheduler.Start(() =>
                    Observable.When(obs[0].And(obs[1]).And(obs[2]).Then((a, b, c) => 0))
                );

                res.Messages.AssertEqual(
                    OnError<int>(210, ex)
                );
            }
        }

        [TestMethod]
        public void And4()
        {
            var scheduler = new TestScheduler();

            const int N = 4;

            var obs = new List<IObservable<int>>();
            for (int i = 0; i < N; i++)
            {
                obs.Add(scheduler.CreateHotObservable(
                    OnNext(210, 1),
                    OnCompleted<int>(220)
                ));
            }

            var res = scheduler.Start(() =>
                Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).Then((a, b, c, d) => a + b + c + d))
            );

            res.Messages.AssertEqual(
                OnNext(210, N),
                OnCompleted<int>(220)
            );
        }

        [TestMethod]
        public void And4Error()
        {
            var ex = new Exception();

            const int N = 4;

            for (int i = 0; i < N; i++)
            {
                var scheduler = new TestScheduler();
                var obs = new List<IObservable<int>>();

                for (int j = 0; j < N; j++)
                {
                    if (j == i)
                    {
                        obs.Add(scheduler.CreateHotObservable(
                            OnError<int>(210, ex)
                        ));
                    }
                    else
                    {
                        obs.Add(scheduler.CreateHotObservable(
                            OnNext(210, 1),
                            OnCompleted<int>(220)
                        ));
                    }
                }

                var res = scheduler.Start(() =>
                    Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).Then((a, b, c, d) => 0))
                );

                res.Messages.AssertEqual(
                    OnError<int>(210, ex)
                );
            }
        }

#if !NO_LARGEARITY
        [TestMethod]
        public void And5()
        {
            var scheduler = new TestScheduler();

            const int N = 5;

            var obs = new List<IObservable<int>>();
            for (int i = 0; i < N; i++)
            {
                obs.Add(scheduler.CreateHotObservable(
                    OnNext(210, 1),
                    OnCompleted<int>(220)
                ));
            }

            var res = scheduler.Start(() =>
                Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).Then((a, b, c, d, e) => a + b + c + d + e))
            );

            res.Messages.AssertEqual(
                OnNext(210, N),
                OnCompleted<int>(220)
            );
        }

        [TestMethod]
        public void And5Error()
        {
            var ex = new Exception();

            const int N = 5;

            for (int i = 0; i < N; i++)
            {
                var scheduler = new TestScheduler();
                var obs = new List<IObservable<int>>();

                for (int j = 0; j < N; j++)
                {
                    if (j == i)
                    {
                        obs.Add(scheduler.CreateHotObservable(
                            OnError<int>(210, ex)
                        ));
                    }
                    else
                    {
                        obs.Add(scheduler.CreateHotObservable(
                            OnNext(210, 1),
                            OnCompleted<int>(220)
                        ));
                    }
                }

                var res = scheduler.Start(() =>
                    Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).Then((a, b, c, d, e) => 0))
                );

                res.Messages.AssertEqual(
                    OnError<int>(210, ex)
                );
            }
        }

        [TestMethod]
        public void And6()
        {
            var scheduler = new TestScheduler();

            const int N = 6;

            var obs = new List<IObservable<int>>();
            for (int i = 0; i < N; i++)
            {
                obs.Add(scheduler.CreateHotObservable(
                    OnNext(210, 1),
                    OnCompleted<int>(220)
                ));
            }

            var res = scheduler.Start(() =>
                Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).Then((a, b, c, d, e, f) => a + b + c + d + e + f))
            );

            res.Messages.AssertEqual(
                OnNext(210, N),
                OnCompleted<int>(220)
            );
        }

        [TestMethod]
        public void And6Error()
        {
            var ex = new Exception();

            const int N = 6;

            for (int i = 0; i < N; i++)
            {
                var scheduler = new TestScheduler();
                var obs = new List<IObservable<int>>();

                for (int j = 0; j < N; j++)
                {
                    if (j == i)
                    {
                        obs.Add(scheduler.CreateHotObservable(
                            OnError<int>(210, ex)
                        ));
                    }
                    else
                    {
                        obs.Add(scheduler.CreateHotObservable(
                            OnNext(210, 1),
                            OnCompleted<int>(220)
                        ));
                    }
                }

                var res = scheduler.Start(() =>
                    Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).Then((a, b, c, d, e, f) => 0))
                );

                res.Messages.AssertEqual(
                    OnError<int>(210, ex)
                );
            }
        }

        [TestMethod]
        public void And7()
        {
            var scheduler = new TestScheduler();

            const int N = 7;

            var obs = new List<IObservable<int>>();
            for (int i = 0; i < N; i++)
            {
                obs.Add(scheduler.CreateHotObservable(
                    OnNext(210, 1),
                    OnCompleted<int>(220)
                ));
            }

            var res = scheduler.Start(() =>
                Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6]).Then((a, b, c, d, e, f, g) => a + b + c + d + e + f + g))
            );

            res.Messages.AssertEqual(
                OnNext(210, N),
                OnCompleted<int>(220)
            );
        }

        [TestMethod]
        public void And7Error()
        {
            var ex = new Exception();

            const int N = 7;

            for (int i = 0; i < N; i++)
            {
                var scheduler = new TestScheduler();
                var obs = new List<IObservable<int>>();

                for (int j = 0; j < N; j++)
                {
                    if (j == i)
                    {
                        obs.Add(scheduler.CreateHotObservable(
                            OnError<int>(210, ex)
                        ));
                    }
                    else
                    {
                        obs.Add(scheduler.CreateHotObservable(
                            OnNext(210, 1),
                            OnCompleted<int>(220)
                        ));
                    }
                }

                var res = scheduler.Start(() =>
                    Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6]).Then((a, b, c, d, e, f, g) => 0))
                );

                res.Messages.AssertEqual(
                    OnError<int>(210, ex)
                );
            }
        }

        [TestMethod]
        public void And8()
        {
            var scheduler = new TestScheduler();

            const int N = 8;

            var obs = new List<IObservable<int>>();
            for (int i = 0; i < N; i++)
            {
                obs.Add(scheduler.CreateHotObservable(
                    OnNext(210, 1),
                    OnCompleted<int>(220)
                ));
            }

            var res = scheduler.Start(() =>
                Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6]).And(obs[7]).Then((a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h))
            );

            res.Messages.AssertEqual(
                OnNext(210, N),
                OnCompleted<int>(220)
            );
        }

        [TestMethod]
        public void And8Error()
        {
            var ex = new Exception();

            const int N = 8;

            for (int i = 0; i < N; i++)
            {
                var scheduler = new TestScheduler();
                var obs = new List<IObservable<int>>();

                for (int j = 0; j < N; j++)
                {
                    if (j == i)
                    {
                        obs.Add(scheduler.CreateHotObservable(
                            OnError<int>(210, ex)
                        ));
                    }
                    else
                    {
                        obs.Add(scheduler.CreateHotObservable(
                            OnNext(210, 1),
                            OnCompleted<int>(220)
                        ));
                    }
                }

                var res = scheduler.Start(() =>
                    Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6]).And(obs[7]).Then((a, b, c, d, e, f, g, h) => 0))
                );

                res.Messages.AssertEqual(
                    OnError<int>(210, ex)
                );
            }
        }

        [TestMethod]
        public void And9()
        {
            var scheduler = new TestScheduler();

            const int N = 9;

            var obs = new List<IObservable<int>>();
            for (int i = 0; i < N; i++)
            {
                obs.Add(scheduler.CreateHotObservable(
                    OnNext(210, 1),
                    OnCompleted<int>(220)
                ));
            }

            var res = scheduler.Start(() =>
                Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6]).And(obs[7]).And(obs[8]).Then((a, b, c, d, e, f, g, h, i) => a + b + c + d + e + f + g + h + i))
            );

            res.Messages.AssertEqual(
                OnNext(210, N),
                OnCompleted<int>(220)
            );
        }

        [TestMethod]
        public void And9Error()
        {
            var ex = new Exception();

            const int N = 9;

            for (int i = 0; i < N; i++)
            {
                var scheduler = new TestScheduler();
                var obs = new List<IObservable<int>>();

                for (int j = 0; j < N; j++)
                {
                    if (j == i)
                    {
                        obs.Add(scheduler.CreateHotObservable(
                            OnError<int>(210, ex)
                        ));
                    }
                    else
                    {
                        obs.Add(scheduler.CreateHotObservable(
                            OnNext(210, 1),
                            OnCompleted<int>(220)
                        ));
                    }
                }

                var res = scheduler.Start(() =>
                    Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6]).And(obs[7]).And(obs[8]).Then((a, b, c, d, e, f, g, h, i_) => 0))
                );

                res.Messages.AssertEqual(
                    OnError<int>(210, ex)
                );
            }
        }

        [TestMethod]
        public void And10()
        {
            var scheduler = new TestScheduler();

            const int N = 10;

            var obs = new List<IObservable<int>>();
            for (int i = 0; i < N; i++)
            {
                obs.Add(scheduler.CreateHotObservable(
                    OnNext(210, 1),
                    OnCompleted<int>(220)
                ));
            }

            var res = scheduler.Start(() =>
                Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6]).And(obs[7]).And(obs[8]).And(obs[9]).Then((a, b, c, d, e, f, g, h, i, j) => a + b + c + d + e + f + g + h + i + j))
            );

            res.Messages.AssertEqual(
                OnNext(210, N),
                OnCompleted<int>(220)
            );
        }

        [TestMethod]
        public void And10Error()
        {
            var ex = new Exception();

            const int N = 10;

            for (int i = 0; i < N; i++)
            {
                var scheduler = new TestScheduler();
                var obs = new List<IObservable<int>>();

                for (int j = 0; j < N; j++)
                {
                    if (j == i)
                    {
                        obs.Add(scheduler.CreateHotObservable(
                            OnError<int>(210, ex)
                        ));
                    }
                    else
                    {
                        obs.Add(scheduler.CreateHotObservable(
                            OnNext(210, 1),
                            OnCompleted<int>(220)
                        ));
                    }
                }

                var res = scheduler.Start(() =>
                    Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6]).And(obs[7]).And(obs[8]).And(obs[9]).Then((a, b, c, d, e, f, g, h, i_, j) => 0))
                );

                res.Messages.AssertEqual(
                    OnError<int>(210, ex)
                );
            }
        }

        [TestMethod]
        public void And11()
        {
            var scheduler = new TestScheduler();

            const int N = 11;

            var obs = new List<IObservable<int>>();
            for (int i = 0; i < N; i++)
            {
                obs.Add(scheduler.CreateHotObservable(
                    OnNext(210, 1),
                    OnCompleted<int>(220)
                ));
            }

            var res = scheduler.Start(() =>
                Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6]).And(obs[7]).And(obs[8]).And(obs[9]).And(obs[10]).Then((a, b, c, d, e, f, g, h, i, j, k) => a + b + c + d + e + f + g + h + i + j + k))
            );

            res.Messages.AssertEqual(
                OnNext(210, N),
                OnCompleted<int>(220)
            );
        }

        [TestMethod]
        public void And11Error()
        {
            var ex = new Exception();

            const int N = 11;

            for (int i = 0; i < N; i++)
            {
                var scheduler = new TestScheduler();
                var obs = new List<IObservable<int>>();

                for (int j = 0; j < N; j++)
                {
                    if (j == i)
                    {
                        obs.Add(scheduler.CreateHotObservable(
                            OnError<int>(210, ex)
                        ));
                    }
                    else
                    {
                        obs.Add(scheduler.CreateHotObservable(
                            OnNext(210, 1),
                            OnCompleted<int>(220)
                        ));
                    }
                }

                var res = scheduler.Start(() =>
                    Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6]).And(obs[7]).And(obs[8]).And(obs[9]).And(obs[10]).Then((a, b, c, d, e, f, g, h, i_, j, k) => 0))
                );

                res.Messages.AssertEqual(
                    OnError<int>(210, ex)
                );
            }
        }

        [TestMethod]
        public void And12()
        {
            var scheduler = new TestScheduler();

            const int N = 12;

            var obs = new List<IObservable<int>>();
            for (int i = 0; i < N; i++)
            {
                obs.Add(scheduler.CreateHotObservable(
                    OnNext(210, 1),
                    OnCompleted<int>(220)
                ));
            }

            var res = scheduler.Start(() =>
                Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6]).And(obs[7]).And(obs[8]).And(obs[9]).And(obs[10]).And(obs[11]).Then((a, b, c, d, e, f, g, h, i, j, k, l) => a + b + c + d + e + f + g + h + i + j + k + l))
            );

            res.Messages.AssertEqual(
                OnNext(210, N),
                OnCompleted<int>(220)
            );
        }

        [TestMethod]
        public void And12Error()
        {
            var ex = new Exception();

            const int N = 12;

            for (int i = 0; i < N; i++)
            {
                var scheduler = new TestScheduler();
                var obs = new List<IObservable<int>>();

                for (int j = 0; j < N; j++)
                {
                    if (j == i)
                    {
                        obs.Add(scheduler.CreateHotObservable(
                            OnError<int>(210, ex)
                        ));
                    }
                    else
                    {
                        obs.Add(scheduler.CreateHotObservable(
                            OnNext(210, 1),
                            OnCompleted<int>(220)
                        ));
                    }
                }

                var res = scheduler.Start(() =>
                    Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6]).And(obs[7]).And(obs[8]).And(obs[9]).And(obs[10]).And(obs[11]).Then((a, b, c, d, e, f, g, h, i_, j, k, l) => 0))
                );

                res.Messages.AssertEqual(
                    OnError<int>(210, ex)
                );
            }
        }

        [TestMethod]
        public void And13()
        {
            var scheduler = new TestScheduler();

            const int N = 13;

            var obs = new List<IObservable<int>>();
            for (int i = 0; i < N; i++)
            {
                obs.Add(scheduler.CreateHotObservable(
                    OnNext(210, 1),
                    OnCompleted<int>(220)
                ));
            }

            var res = scheduler.Start(() =>
                Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6]).And(obs[7]).And(obs[8]).And(obs[9]).And(obs[10]).And(obs[11]).And(obs[12]).Then((a, b, c, d, e, f, g, h, i, j, k, l, m) => a + b + c + d + e + f + g + h + i + j + k + l + m))
            );

            res.Messages.AssertEqual(
                OnNext(210, N),
                OnCompleted<int>(220)
            );
        }

        [TestMethod]
        public void And13Error()
        {
            var ex = new Exception();

            const int N = 13;

            for (int i = 0; i < N; i++)
            {
                var scheduler = new TestScheduler();
                var obs = new List<IObservable<int>>();

                for (int j = 0; j < N; j++)
                {
                    if (j == i)
                    {
                        obs.Add(scheduler.CreateHotObservable(
                            OnError<int>(210, ex)
                        ));
                    }
                    else
                    {
                        obs.Add(scheduler.CreateHotObservable(
                            OnNext(210, 1),
                            OnCompleted<int>(220)
                        ));
                    }
                }

                var res = scheduler.Start(() =>
                    Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6]).And(obs[7]).And(obs[8]).And(obs[9]).And(obs[10]).And(obs[11]).And(obs[12]).Then((a, b, c, d, e, f, g, h, i_, j, k, l, m) => 0))
                );

                res.Messages.AssertEqual(
                    OnError<int>(210, ex)
                );
            }
        }

        [TestMethod]
        public void And14()
        {
            var scheduler = new TestScheduler();

            const int N = 14;

            var obs = new List<IObservable<int>>();
            for (int i = 0; i < N; i++)
            {
                obs.Add(scheduler.CreateHotObservable(
                    OnNext(210, 1),
                    OnCompleted<int>(220)
                ));
            }

            var res = scheduler.Start(() =>
                Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6]).And(obs[7]).And(obs[8]).And(obs[9]).And(obs[10]).And(obs[11]).And(obs[12]).And(obs[13]).Then((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => a + b + c + d + e + f + g + h + i + j + k + l + m + n))
            );

            res.Messages.AssertEqual(
                OnNext(210, N),
                OnCompleted<int>(220)
            );
        }

        [TestMethod]
        public void And14Error()
        {
            var ex = new Exception();

            const int N = 14;

            for (int i = 0; i < N; i++)
            {
                var scheduler = new TestScheduler();
                var obs = new List<IObservable<int>>();

                for (int j = 0; j < N; j++)
                {
                    if (j == i)
                    {
                        obs.Add(scheduler.CreateHotObservable(
                            OnError<int>(210, ex)
                        ));
                    }
                    else
                    {
                        obs.Add(scheduler.CreateHotObservable(
                            OnNext(210, 1),
                            OnCompleted<int>(220)
                        ));
                    }
                }

                var res = scheduler.Start(() =>
                    Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6]).And(obs[7]).And(obs[8]).And(obs[9]).And(obs[10]).And(obs[11]).And(obs[12]).And(obs[13]).Then((a, b, c, d, e, f, g, h, i_, j, k, l, m, n) => 0))
                );

                res.Messages.AssertEqual(
                    OnError<int>(210, ex)
                );
            }
        }

        [TestMethod]
        public void And15()
        {
            var scheduler = new TestScheduler();

            const int N = 15;

            var obs = new List<IObservable<int>>();
            for (int i = 0; i < N; i++)
            {
                obs.Add(scheduler.CreateHotObservable(
                    OnNext(210, 1),
                    OnCompleted<int>(220)
                ));
            }

            var res = scheduler.Start(() =>
                Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6]).And(obs[7]).And(obs[8]).And(obs[9]).And(obs[10]).And(obs[11]).And(obs[12]).And(obs[13]).And(obs[14]).Then((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => a + b + c + d + e + f + g + h + i + j + k + l + m + n + o))
            );

            res.Messages.AssertEqual(
                OnNext(210, N),
                OnCompleted<int>(220)
            );
        }

        [TestMethod]
        public void And15Error()
        {
            var ex = new Exception();

            const int N = 15;

            for (int i = 0; i < N; i++)
            {
                var scheduler = new TestScheduler();
                var obs = new List<IObservable<int>>();

                for (int j = 0; j < N; j++)
                {
                    if (j == i)
                    {
                        obs.Add(scheduler.CreateHotObservable(
                            OnError<int>(210, ex)
                        ));
                    }
                    else
                    {
                        obs.Add(scheduler.CreateHotObservable(
                            OnNext(210, 1),
                            OnCompleted<int>(220)
                        ));
                    }
                }

                var res = scheduler.Start(() =>
                    Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6]).And(obs[7]).And(obs[8]).And(obs[9]).And(obs[10]).And(obs[11]).And(obs[12]).And(obs[13]).And(obs[14]).Then((a, b, c, d, e, f, g, h, i_, j, k, l, m, n, o) => 0))
                );

                res.Messages.AssertEqual(
                    OnError<int>(210, ex)
                );
            }
        }

        [TestMethod]
        public void And16()
        {
            var scheduler = new TestScheduler();

            const int N = 16;

            var obs = new List<IObservable<int>>();
            for (int i = 0; i < N; i++)
            {
                obs.Add(scheduler.CreateHotObservable(
                    OnNext(210, 1),
                    OnCompleted<int>(220)
                ));
            }

            var res = scheduler.Start(() =>
                Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6]).And(obs[7]).And(obs[8]).And(obs[9]).And(obs[10]).And(obs[11]).And(obs[12]).And(obs[13]).And(obs[14]).And(obs[15]).Then((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => a + b + c + d + e + f + g + h + i + j + k + l + m + n + o + p))
            );

            res.Messages.AssertEqual(
                OnNext(210, N),
                OnCompleted<int>(220)
            );
        }

        [TestMethod]
        public void And16Error()
        {
            var ex = new Exception();

            const int N = 16;

            for (int i = 0; i < N; i++)
            {
                var scheduler = new TestScheduler();
                var obs = new List<IObservable<int>>();

                for (int j = 0; j < N; j++)
                {
                    if (j == i)
                    {
                        obs.Add(scheduler.CreateHotObservable(
                            OnError<int>(210, ex)
                        ));
                    }
                    else
                    {
                        obs.Add(scheduler.CreateHotObservable(
                            OnNext(210, 1),
                            OnCompleted<int>(220)
                        ));
                    }
                }

                var res = scheduler.Start(() =>
                    Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6]).And(obs[7]).And(obs[8]).And(obs[9]).And(obs[10]).And(obs[11]).And(obs[12]).And(obs[13]).And(obs[14]).And(obs[15]).Then((a, b, c, d, e, f, g, h, i_, j, k, l, m, n, o, p) => 0))
                );

                res.Messages.AssertEqual(
                    OnError<int>(210, ex)
                );
            }
        }
#endif

        #endregion

        #region Then

        [TestMethod]
        public void Then_ArgumentChecking()
        {
            var someObservable = Observable.Return(1);
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Then<int, int>(null, _ => _));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Then<int, int>(someObservable, null));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(someObservable, someObservable).Then<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(someObservable, someObservable).And(someObservable).Then<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(someObservable, someObservable).And(someObservable).And(someObservable).Then<int>(null));

#if !NO_LARGEARITY
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(someObservable, someObservable).And(someObservable).And(someObservable).And(someObservable).Then<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(someObservable, someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).Then<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(someObservable, someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).Then<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(someObservable, someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).Then<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(someObservable, someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).Then<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(someObservable, someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).Then<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(someObservable, someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).Then<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(someObservable, someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).Then<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(someObservable, someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).Then<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(someObservable, someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).Then<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(someObservable, someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).Then<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.And<int, int>(someObservable, someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).And(someObservable).Then<int>(null));
#endif
        }

        [TestMethod]
        public void Then1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnCompleted<int>(220)
            );

            var res = scheduler.Start(() =>
                Observable.When(xs.Then(a => a))
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnCompleted<int>(220)
            );
        }

        [TestMethod]
        public void Then1Error()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                Observable.When(xs.Then(a => a))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }

        [TestMethod]
        public void Then1Throws()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnCompleted<int>(220)
            );

            var res = scheduler.Start(() =>
                Observable.When(xs.Then<int, int>(a => { throw ex; }))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }

        [TestMethod]
        public void Then2Throws()
        {
            var scheduler = new TestScheduler();
            var ex = new Exception();

            const int N = 2;

            var obs = new List<IObservable<int>>();
            for (int i = 0; i < N; i++)
            {
                obs.Add(scheduler.CreateHotObservable(
                    OnNext(210, 1),
                    OnCompleted<int>(220)
                ));
            }

            var res = scheduler.Start(() =>
                Observable.When(obs[0].And(obs[1]).Then<int>((a, b) => { throw ex; }))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }

        [TestMethod]
        public void Then3Throws()
        {
            var scheduler = new TestScheduler();
            var ex = new Exception();

            const int N = 3;

            var obs = new List<IObservable<int>>();
            for (int i = 0; i < N; i++)
            {
                obs.Add(scheduler.CreateHotObservable(
                    OnNext(210, 1),
                    OnCompleted<int>(220)
                ));
            }

            var res = scheduler.Start(() =>
                Observable.When(obs[0].And(obs[1]).And(obs[2]).Then<int>((a, b, c) => { throw ex; }))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }

        [TestMethod]
        public void Then4Throws()
        {
            var scheduler = new TestScheduler();
            var ex = new Exception();

            const int N = 4;

            var obs = new List<IObservable<int>>();
            for (int i = 0; i < N; i++)
            {
                obs.Add(scheduler.CreateHotObservable(
                    OnNext(210, 1),
                    OnCompleted<int>(220)
                ));
            }

            var res = scheduler.Start(() =>
                Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).Then<int>((a, b, c, d) => { throw ex; }))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }

#if !NO_LARGEARITY
        [TestMethod]
        public void Then5Throws()
        {
            var scheduler = new TestScheduler();
            var ex = new Exception();

            const int N = 5;

            var obs = new List<IObservable<int>>();
            for (int i = 0; i < N; i++)
            {
                obs.Add(scheduler.CreateHotObservable(
                    OnNext(210, 1),
                    OnCompleted<int>(220)
                ));
            }

            var res = scheduler.Start(() =>
                Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).Then<int>((a, b, c, d, e) => { throw ex; }))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }

        [TestMethod]
        public void Then6Throws()
        {
            var scheduler = new TestScheduler();
            var ex = new Exception();

            const int N = 6;

            var obs = new List<IObservable<int>>();
            for (int i = 0; i < N; i++)
            {
                obs.Add(scheduler.CreateHotObservable(
                    OnNext(210, 1),
                    OnCompleted<int>(220)
                ));
            }

            var res = scheduler.Start(() =>
                Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).Then<int>((a, b, c, d, e, f) => { throw ex; }))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }

        [TestMethod]
        public void Then7Throws()
        {
            var scheduler = new TestScheduler();
            var ex = new Exception();

            const int N = 7;

            var obs = new List<IObservable<int>>();
            for (int i = 0; i < N; i++)
            {
                obs.Add(scheduler.CreateHotObservable(
                    OnNext(210, 1),
                    OnCompleted<int>(220)
                ));
            }

            var res = scheduler.Start(() =>
                Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6]).Then<int>((a, b, c, d, e, f, g) => { throw ex; }))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }

        [TestMethod]
        public void Then8Throws()
        {
            var scheduler = new TestScheduler();
            var ex = new Exception();

            const int N = 8;

            var obs = new List<IObservable<int>>();
            for (int i = 0; i < N; i++)
            {
                obs.Add(scheduler.CreateHotObservable(
                    OnNext(210, 1),
                    OnCompleted<int>(220)
                ));
            }

            var res = scheduler.Start(() =>
                Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6]).And(obs[7]).Then<int>((a, b, c, d, e, f, g, h) => { throw ex; }))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }

        [TestMethod]
        public void Then9Throws()
        {
            var scheduler = new TestScheduler();
            var ex = new Exception();

            const int N = 9;

            var obs = new List<IObservable<int>>();
            for (int i = 0; i < N; i++)
            {
                obs.Add(scheduler.CreateHotObservable(
                    OnNext(210, 1),
                    OnCompleted<int>(220)
                ));
            }

            var res = scheduler.Start(() =>
                Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6]).And(obs[7]).And(obs[8]).Then<int>((a, b, c, d, e, f, g, h, i_) => { throw ex; }))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }

        [TestMethod]
        public void Then10Throws()
        {
            var scheduler = new TestScheduler();
            var ex = new Exception();

            const int N = 10;

            var obs = new List<IObservable<int>>();
            for (int i = 0; i < N; i++)
            {
                obs.Add(scheduler.CreateHotObservable(
                    OnNext(210, 1),
                    OnCompleted<int>(220)
                ));
            }

            var res = scheduler.Start(() =>
                Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6]).And(obs[7]).And(obs[8]).And(obs[9]).Then<int>((a, b, c, d, e, f, g, h, i_, j) => { throw ex; }))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }

        [TestMethod]
        public void Then11Throws()
        {
            var scheduler = new TestScheduler();
            var ex = new Exception();

            const int N = 11;

            var obs = new List<IObservable<int>>();
            for (int i = 0; i < N; i++)
            {
                obs.Add(scheduler.CreateHotObservable(
                    OnNext(210, 1),
                    OnCompleted<int>(220)
                ));
            }

            var res = scheduler.Start(() =>
                Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6]).And(obs[7]).And(obs[8]).And(obs[9]).And(obs[10]).Then<int>((a, b, c, d, e, f, g, h, i_, j, k) => { throw ex; }))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }

        [TestMethod]
        public void Then12Throws()
        {
            var scheduler = new TestScheduler();
            var ex = new Exception();

            const int N = 12;

            var obs = new List<IObservable<int>>();
            for (int i = 0; i < N; i++)
            {
                obs.Add(scheduler.CreateHotObservable(
                    OnNext(210, 1),
                    OnCompleted<int>(220)
                ));
            }

            var res = scheduler.Start(() =>
                Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6]).And(obs[7]).And(obs[8]).And(obs[9]).And(obs[10]).And(obs[11]).Then<int>((a, b, c, d, e, f, g, h, i_, j, k, l) => { throw ex; }))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }

        [TestMethod]
        public void Then13Throws()
        {
            var scheduler = new TestScheduler();
            var ex = new Exception();

            const int N = 13;

            var obs = new List<IObservable<int>>();
            for (int i = 0; i < N; i++)
            {
                obs.Add(scheduler.CreateHotObservable(
                    OnNext(210, 1),
                    OnCompleted<int>(220)
                ));
            }

            var res = scheduler.Start(() =>
                Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6]).And(obs[7]).And(obs[8]).And(obs[9]).And(obs[10]).And(obs[11]).And(obs[12]).Then<int>((a, b, c, d, e, f, g, h, i_, j, k, l, m) => { throw ex; }))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }

        [TestMethod]
        public void Then14Throws()
        {
            var scheduler = new TestScheduler();
            var ex = new Exception();

            const int N = 14;

            var obs = new List<IObservable<int>>();
            for (int i = 0; i < N; i++)
            {
                obs.Add(scheduler.CreateHotObservable(
                    OnNext(210, 1),
                    OnCompleted<int>(220)
                ));
            }

            var res = scheduler.Start(() =>
                Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6]).And(obs[7]).And(obs[8]).And(obs[9]).And(obs[10]).And(obs[11]).And(obs[12]).And(obs[13]).Then<int>((a, b, c, d, e, f, g, h, i_, j, k, l, m, n) => { throw ex; }))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }

        [TestMethod]
        public void Then15Throws()
        {
            var scheduler = new TestScheduler();
            var ex = new Exception();

            const int N = 15;

            var obs = new List<IObservable<int>>();
            for (int i = 0; i < N; i++)
            {
                obs.Add(scheduler.CreateHotObservable(
                    OnNext(210, 1),
                    OnCompleted<int>(220)
                ));
            }

            var res = scheduler.Start(() =>
                Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6]).And(obs[7]).And(obs[8]).And(obs[9]).And(obs[10]).And(obs[11]).And(obs[12]).And(obs[13]).And(obs[14]).Then<int>((a, b, c, d, e, f, g, h, i_, j, k, l, m, n, o) => { throw ex; }))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }

        [TestMethod]
        public void Then16Throws()
        {
            var scheduler = new TestScheduler();
            var ex = new Exception();

            const int N = 16;

            var obs = new List<IObservable<int>>();
            for (int i = 0; i < N; i++)
            {
                obs.Add(scheduler.CreateHotObservable(
                    OnNext(210, 1),
                    OnCompleted<int>(220)
                ));
            }

            var res = scheduler.Start(() =>
                Observable.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6]).And(obs[7]).And(obs[8]).And(obs[9]).And(obs[10]).And(obs[11]).And(obs[12]).And(obs[13]).And(obs[14]).And(obs[15]).Then<int>((a, b, c, d, e, f, g, h, i_, j, k, l, m, n, o, p) => { throw ex; }))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }
#endif

        #endregion

        #region When

        [TestMethod]
        public void When_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.When<int>((Plan<int>[])null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.When<int>((IEnumerable<Plan<int>>)null));
        }

        [TestMethod]
        public void WhenMultipleDataSymmetric()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnCompleted<int>(270)
            );

            var res = scheduler.Start(() =>
                Observable.When(
                    xs.And(ys).Then((x, y) => x + y)
                )
            );

            res.Messages.AssertEqual(
                OnNext(240, 1 + 4),
                OnNext(250, 2 + 5),
                OnNext(260, 3 + 6),
                OnCompleted<int>(270)
            );
        }

        [TestMethod]
        public void WhenMultipleDataAsymmetric()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(240, 4),
                OnNext(250, 5),
                OnCompleted<int>(270)
            );

            var res = scheduler.Start(() =>
                Observable.When(
                    xs.And(ys).Then((x, y) => x + y)
                )
            );

            res.Messages.AssertEqual(
                OnNext(240, 1 + 4),
                OnNext(250, 2 + 5),
                OnCompleted<int>(270)
            );
        }

        [TestMethod]
        public void WhenEmptyEmpty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnCompleted<int>(240)
            );

            var ys = scheduler.CreateHotObservable(
                OnCompleted<int>(270)
            );

            var res = scheduler.Start(() =>
                Observable.When(
                    xs.And(ys).Then((x, y) => x + y)
                )
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(270)
            );
        }

        [TestMethod]
        public void WhenNeverNever()
        {
            var scheduler = new TestScheduler();

            var xs = Observable.Never<int>();
            var ys = Observable.Never<int>();

            var res = scheduler.Start(() =>
                Observable.When(
                    xs.And(ys).Then((x, y) => x + y)
                )
            );

            res.Messages.AssertEqual(
            );
        }

        [TestMethod]
        public void WhenThrowNonEmpty()
        {
            var ex = new Exception();
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnError<int>(240, ex)
            );

            var ys = scheduler.CreateHotObservable(
                OnCompleted<int>(270)
            );

            var res = scheduler.Start(() =>
                Observable.When(
                    xs.And(ys).Then((x, y) => x + y)
                )
            );

            res.Messages.AssertEqual(
                OnError<int>(240, ex)
            );
        }

        [TestMethod]
        public void ComplicatedWhen()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnCompleted<int>(270)
            );

            var zs = scheduler.CreateHotObservable(
                OnNext(220, 7),
                OnNext(230, 8),
                OnNext(240, 9),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                Observable.When(
                    xs.And(ys).Then((x, y) => x + y),
                    xs.And(zs).Then((x, z) => x * z),
                    ys.And(zs).Then((y, z) => y - z)
                )
            );

            res.Messages.AssertEqual(
                OnNext(220, 1 * 7),
                OnNext(230, 2 * 8),
                OnNext(240, 3 + 4),
                OnNext(250, 5 - 9),
                OnCompleted<int>(300)
            );
        }

        [TestMethod]
        public void When_PlansIteratorThrows()
        {
            var ex = new Exception();
            var _e = default(Exception);

            GetPlans(ex).When().Subscribe(_ => { }, e => { _e = e; });
            Assert.AreSame(_e, ex);
        }

        private IEnumerable<Plan<int>> GetPlans(Exception ex)
        {
            if (ex != null)
                throw ex;
            
            yield break;
        }

        #endregion
    }
}