// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#if !NO_TPL
using System.Threading.Tasks;
#endif

namespace ReactiveTests.Tests
{
    [TestClass]
    public partial class ObservableAsyncTest : ReactiveTest
    {
        #region FromAsyncPattern

        [TestMethod]
        public void FromAsyncPattern_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern(null, iar => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int>(null, iar => 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int>(null, iar => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int>(null, iar => 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int>(null, iar => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int>(null, iar => 0));
#if !NO_LARGEARITY
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int>(null, iar => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int>(null, iar => 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int>(null, iar => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int>(null, iar => 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int>(null, iar => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int>(null, iar => 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int>(null, iar => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int>(null, iar => 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int>(null, iar => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int>(null, iar => 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int>(null, iar => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int>(null, iar => 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int>(null, iar => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int>(null, iar => 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int>(null, iar => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int, int>(null, iar => 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int, int>(null, iar => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int, int, int>(null, iar => 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int, int, int>(null, iar => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int, int, int, int>(null, iar => 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int, int, int, int>(null, iar => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(null, iar => 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(null, iar => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(null, iar => 0));
#endif

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern((cb, o) => null, default(Action<IAsyncResult>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int>((cb, o) => null, default(Func<IAsyncResult, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int>((a, cb, o) => null, default(Action<IAsyncResult>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int>((a, cb, o) => null, default(Func<IAsyncResult, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int>((a, b, cb, o) => null, default(Action<IAsyncResult>)));
#if !NO_LARGEARITY
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int>((a, b, cb, o) => null, default(Func<IAsyncResult, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int>((a, b, c, cb, o) => null, default(Action<IAsyncResult>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int>((a, b, c, cb, o) => null, default(Func<IAsyncResult, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int>((a, b, c, d, cb, o) => null, default(Action<IAsyncResult>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int>((a, b, c, d, cb, o) => null, default(Func<IAsyncResult, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int>((a, b, c, d, e, cb, o) => null, default(Action<IAsyncResult>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int>((a, b, c, d, e, cb, o) => null, default(Func<IAsyncResult, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int>((a, b, c, d, e, f, cb, o) => null, default(Action<IAsyncResult>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int>((a, b, c, d, e, f, cb, o) => null, default(Func<IAsyncResult, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int>((a, b, c, d, e, f, g, cb, o) => null, default(Action<IAsyncResult>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, cb, o) => null, default(Func<IAsyncResult, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, cb, o) => null, default(Action<IAsyncResult>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, cb, o) => null, default(Func<IAsyncResult, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, cb, o) => null, default(Action<IAsyncResult>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, cb, o) => null, default(Func<IAsyncResult, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, cb, o) => null, default(Action<IAsyncResult>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, cb, o) => null, default(Func<IAsyncResult, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, cb, o) => null, default(Action<IAsyncResult>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, cb, o) => null, default(Func<IAsyncResult, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, cb, o) => null, default(Action<IAsyncResult>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, cb, o) => null, default(Func<IAsyncResult, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, cb, o) => null, default(Action<IAsyncResult>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, cb, o) => null, default(Func<IAsyncResult, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, cb, o) => null, default(Action<IAsyncResult>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, cb, o) => null, default(Func<IAsyncResult, int>)));
#endif
        }

        [TestMethod]
        public void FromAsyncPattern0()
        {
            var x = new Result();

            Func<AsyncCallback, object, IAsyncResult> begin = (cb, _) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)().Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext<int>(1), Notification.CreateOnCompleted<int>() }));
        }

        [TestMethod]
        public void FromAsyncPatternAction0()
        {
            var x = new Result();

            Func<AsyncCallback, object, IAsyncResult> begin = (cb, _) => { cb(x); return x; };
            Action<IAsyncResult> end = iar => { Assert.AreSame(x, iar); };

            var res = Observable.FromAsyncPattern(begin, end)().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new[] { new Unit() }));
        }

        [TestMethod]
        public void FromAsyncPattern0_Error()
        {
            var x = new Result();
            var ex = new Exception();

            Func<AsyncCallback, object, IAsyncResult> begin = (cb, _) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); throw ex; };

            var res = Observable.FromAsyncPattern(begin, end)().Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void FromAsyncPattern0_ErrorBegin()
        {
            var x = new Result();
            var ex = new Exception();

            Func<AsyncCallback, object, IAsyncResult> begin = (cb, _) => { cb(x); throw ex; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 0; };

            var res = Observable.FromAsyncPattern(begin, end)().Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void FromAsyncPattern1()
        {
            var x = new Result();

            Func<int, AsyncCallback, object, IAsyncResult> begin = (a, cb, _) =>
            {
                Assert.AreEqual(a, 2);
                cb(x);
                return x;
            };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)(2).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext<int>(1), Notification.CreateOnCompleted<int>() }));
        }

        [TestMethod]
        public void FromAsyncPatternAction1()
        {
            var x = new Result();

            Func<int, AsyncCallback, object, IAsyncResult> begin = (a, cb, _) =>
            {
                Assert.AreEqual(a, 2);
                cb(x);
                return x;
            };
            Action<IAsyncResult> end = iar => { Assert.AreSame(x, iar); };

            var res = Observable.FromAsyncPattern(begin, end)(2).ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new[] { new Unit() }));
        }

        [TestMethod]
        public void FromAsyncPattern1_Error()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, AsyncCallback, object, IAsyncResult> begin = (a, cb, o) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); throw ex; };

            var res = Observable.FromAsyncPattern(begin, end)(2).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void FromAsyncPattern1_ErrorBegin()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, AsyncCallback, object, IAsyncResult> begin = (a, cb, o) => { cb(x); throw ex; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 0; };

            var res = Observable.FromAsyncPattern(begin, end)(2).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void FromAsyncPattern2()
        {
            var x = new Result();

            Func<int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, cb, _) =>
            {
                Assert.AreEqual(a, 2);
                Assert.AreEqual(b, 3);
                cb(x);
                return x;
            };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext<int>(1), Notification.CreateOnCompleted<int>() }));
        }

        [TestMethod]
        public void FromAsyncPatternAction2()
        {
            var x = new Result();

            Func<int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, cb, _) =>
            {
                Assert.AreEqual(a, 2);
                Assert.AreEqual(b, 3);
                cb(x);
                return x;
            };
            Action<IAsyncResult> end = iar => { Assert.AreSame(x, iar); };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3).ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new[] { new Unit() }));
        }

        [TestMethod]
        public void FromAsyncPattern2_Error()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, cb, o) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); throw ex; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void FromAsyncPattern2_ErrorBegin()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, cb, o) => { cb(x); throw ex; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 0; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

#if !NO_LARGEARITY
        [TestMethod]
        public void FromAsyncPattern3()
        {
            var x = new Result();

            Func<int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, cb, _) =>
            {
                Assert.AreEqual(a, 2);
                Assert.AreEqual(b, 3);
                Assert.AreEqual(c, 4);
                cb(x);
                return x;
            };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext<int>(1), Notification.CreateOnCompleted<int>() }));
        }
        [TestMethod]
        public void FromAsyncPatternAction3()
        {
            var x = new Result();

            Func<int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, cb, _) =>
            {
                Assert.AreEqual(a, 2);
                Assert.AreEqual(b, 3);
                Assert.AreEqual(c, 4);
                cb(x);
                return x;
            };
            Action<IAsyncResult> end = iar => { Assert.AreSame(x, iar); };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4).ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new[] { new Unit() }));
        }

        [TestMethod]
        public void FromAsyncPattern3_Error()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, cb, o) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); throw ex; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void FromAsyncPattern3_ErrorBegin()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, cb, o) => { cb(x); throw ex; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 0; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void FromAsyncPattern4()
        {
            var x = new Result();

            Func<int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, cb, _) =>
            {
                Assert.AreEqual(a, 2);
                Assert.AreEqual(b, 3);
                Assert.AreEqual(c, 4);
                Assert.AreEqual(d, 5);
                cb(x);
                return x;
            };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext<int>(1), Notification.CreateOnCompleted<int>() }));
        }

        [TestMethod]
        public void FromAsyncPatternAction4()
        {
            var x = new Result();

            Func<int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, cb, _) =>
            {
                Assert.AreEqual(a, 2);
                Assert.AreEqual(b, 3);
                Assert.AreEqual(c, 4);
                Assert.AreEqual(d, 5);
                cb(x);
                return x;
            };
            Action<IAsyncResult> end = iar => { Assert.AreSame(x, iar); };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5).ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new[] { new Unit() }));
        }

        [TestMethod]
        public void FromAsyncPattern4_Error()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, cb, o) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); throw ex; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void FromAsyncPattern4_ErrorBegin()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, cb, o) => { cb(x); throw ex; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 0; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void FromAsyncPattern5()
        {
            var x = new Result();

            Func<int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, cb, _) =>
            {
                Assert.AreEqual(a, 2);
                Assert.AreEqual(b, 3);
                Assert.AreEqual(c, 4);
                Assert.AreEqual(d, 5);
                Assert.AreEqual(e, 6);
                cb(x);
                return x;
            };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext<int>(1), Notification.CreateOnCompleted<int>() }));
        }

        [TestMethod]
        public void FromAsyncPatternAction5()
        {
            var x = new Result();

            Func<int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, cb, _) =>
            {
                Assert.AreEqual(a, 2);
                Assert.AreEqual(b, 3);
                Assert.AreEqual(c, 4);
                Assert.AreEqual(d, 5);
                Assert.AreEqual(e, 6);
                cb(x);
                return x;
            };
            Action<IAsyncResult> end = iar => { Assert.AreSame(x, iar); };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6).ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new[] { new Unit() }));
        }

        [TestMethod]
        public void FromAsyncPattern5_Error()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, cb, o) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); throw ex; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void FromAsyncPattern5_ErrorBegin()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, cb, o) => { cb(x); throw ex; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 0; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void FromAsyncPattern6()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, cb, _) =>
            {
                Assert.AreEqual(a, 2);
                Assert.AreEqual(b, 3);
                Assert.AreEqual(c, 4);
                Assert.AreEqual(d, 5);
                Assert.AreEqual(e, 6);
                Assert.AreEqual(f, 7);
                cb(x);
                return x;
            };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext<int>(1), Notification.CreateOnCompleted<int>() }));
        }

        [TestMethod]
        public void FromAsyncPatternAction6()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, cb, _) =>
            {
                Assert.AreEqual(a, 2);
                Assert.AreEqual(b, 3);
                Assert.AreEqual(c, 4);
                Assert.AreEqual(d, 5);
                Assert.AreEqual(e, 6);
                Assert.AreEqual(f, 7);
                cb(x);
                return x;
            };
            Action<IAsyncResult> end = iar => { Assert.AreSame(x, iar); };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7).ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new[] { new Unit() }));
        }

        [TestMethod]
        public void FromAsyncPattern6_Error()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, cb, o) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); throw ex; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void FromAsyncPattern6_ErrorBegin()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, cb, o) => { cb(x); throw ex; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 0; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void FromAsyncPattern7()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, cb, _) =>
            {
                Assert.AreEqual(a, 2);
                Assert.AreEqual(b, 3);
                Assert.AreEqual(c, 4);
                Assert.AreEqual(d, 5);
                Assert.AreEqual(e, 6);
                Assert.AreEqual(f, 7);
                Assert.AreEqual(g, 8);
                cb(x);
                return x;
            };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext<int>(1), Notification.CreateOnCompleted<int>() }));
        }

        [TestMethod]
        public void FromAsyncPatternAction7()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, cb, _) =>
            {
                Assert.AreEqual(a, 2);
                Assert.AreEqual(b, 3);
                Assert.AreEqual(c, 4);
                Assert.AreEqual(d, 5);
                Assert.AreEqual(e, 6);
                Assert.AreEqual(f, 7);
                Assert.AreEqual(g, 8);
                cb(x);
                return x;
            };
            Action<IAsyncResult> end = iar => { Assert.AreSame(x, iar); };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8).ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new[] { new Unit() }));
        }

        [TestMethod]
        public void FromAsyncPattern7_Error()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, cb, o) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); throw ex; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void FromAsyncPattern7_ErrorBegin()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, cb, o) => { cb(x); throw ex; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 0; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void FromAsyncPattern8()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, cb, _) =>
            {
                Assert.AreEqual(a, 2);
                Assert.AreEqual(b, 3);
                Assert.AreEqual(c, 4);
                Assert.AreEqual(d, 5);
                Assert.AreEqual(e, 6);
                Assert.AreEqual(f, 7);
                Assert.AreEqual(g, 8);
                Assert.AreEqual(h, 9);
                cb(x);
                return x;
            };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext<int>(1), Notification.CreateOnCompleted<int>() }));
        }

        [TestMethod]
        public void FromAsyncPatternAction8()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, cb, _) =>
            {
                Assert.AreEqual(a, 2);
                Assert.AreEqual(b, 3);
                Assert.AreEqual(c, 4);
                Assert.AreEqual(d, 5);
                Assert.AreEqual(e, 6);
                Assert.AreEqual(f, 7);
                Assert.AreEqual(g, 8);
                Assert.AreEqual(h, 9);
                cb(x);
                return x;
            };
            Action<IAsyncResult> end = iar => { Assert.AreSame(x, iar); };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9).ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new[] { new Unit() }));
        }

        [TestMethod]
        public void FromAsyncPattern8_Error()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, cb, o) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); throw ex; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void FromAsyncPattern8_ErrorBegin()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, cb, o) => { cb(x); throw ex; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 0; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void FromAsyncPattern9()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, cb, _) =>
            {
                Assert.AreEqual(a, 2);
                Assert.AreEqual(b, 3);
                Assert.AreEqual(c, 4);
                Assert.AreEqual(d, 5);
                Assert.AreEqual(e, 6);
                Assert.AreEqual(f, 7);
                Assert.AreEqual(g, 8);
                Assert.AreEqual(h, 9);
                Assert.AreEqual(i, 10);
                cb(x);
                return x;
            };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext<int>(1), Notification.CreateOnCompleted<int>() }));
        }

        [TestMethod]
        public void FromAsyncPatternAction9()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, cb, _) =>
            {
                Assert.AreEqual(a, 2);
                Assert.AreEqual(b, 3);
                Assert.AreEqual(c, 4);
                Assert.AreEqual(d, 5);
                Assert.AreEqual(e, 6);
                Assert.AreEqual(f, 7);
                Assert.AreEqual(g, 8);
                Assert.AreEqual(h, 9);
                Assert.AreEqual(i, 10);
                cb(x);
                return x;
            };
            Action<IAsyncResult> end = iar => { Assert.AreSame(x, iar); };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10).ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new[] { new Unit() }));
        }

        [TestMethod]
        public void FromAsyncPattern9_Error()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, cb, o) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); throw ex; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void FromAsyncPattern9_ErrorBegin()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, cb, o) => { cb(x); throw ex; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 0; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void FromAsyncPattern10()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, cb, _) =>
            {
                Assert.AreEqual(a, 2);
                Assert.AreEqual(b, 3);
                Assert.AreEqual(c, 4);
                Assert.AreEqual(d, 5);
                Assert.AreEqual(e, 6);
                Assert.AreEqual(f, 7);
                Assert.AreEqual(g, 8);
                Assert.AreEqual(h, 9);
                Assert.AreEqual(i, 10);
                Assert.AreEqual(j, 11);
                cb(x);
                return x;
            };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext<int>(1), Notification.CreateOnCompleted<int>() }));
        }

        [TestMethod]
        public void FromAsyncPatternAction10()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, cb, _) =>
            {
                Assert.AreEqual(a, 2);
                Assert.AreEqual(b, 3);
                Assert.AreEqual(c, 4);
                Assert.AreEqual(d, 5);
                Assert.AreEqual(e, 6);
                Assert.AreEqual(f, 7);
                Assert.AreEqual(g, 8);
                Assert.AreEqual(h, 9);
                Assert.AreEqual(i, 10);
                Assert.AreEqual(j, 11);
                cb(x);
                return x;
            };
            Action<IAsyncResult> end = iar => { Assert.AreSame(x, iar); };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11).ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new[] { new Unit() }));
        }

        [TestMethod]
        public void FromAsyncPattern10_Error()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, cb, o) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); throw ex; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void FromAsyncPattern10_ErrorBegin()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, cb, o) => { cb(x); throw ex; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 0; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void FromAsyncPattern11()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, cb, _) =>
            {
                Assert.AreEqual(a, 2);
                Assert.AreEqual(b, 3);
                Assert.AreEqual(c, 4);
                Assert.AreEqual(d, 5);
                Assert.AreEqual(e, 6);
                Assert.AreEqual(f, 7);
                Assert.AreEqual(g, 8);
                Assert.AreEqual(h, 9);
                Assert.AreEqual(i, 10);
                Assert.AreEqual(j, 11);
                Assert.AreEqual(k, 12);
                cb(x);
                return x;
            };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext<int>(1), Notification.CreateOnCompleted<int>() }));
        }

        [TestMethod]
        public void FromAsyncPatternAction11()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, cb, _) =>
            {
                Assert.AreEqual(a, 2);
                Assert.AreEqual(b, 3);
                Assert.AreEqual(c, 4);
                Assert.AreEqual(d, 5);
                Assert.AreEqual(e, 6);
                Assert.AreEqual(f, 7);
                Assert.AreEqual(g, 8);
                Assert.AreEqual(h, 9);
                Assert.AreEqual(i, 10);
                Assert.AreEqual(j, 11);
                Assert.AreEqual(k, 12);
                cb(x);
                return x;
            };
            Action<IAsyncResult> end = iar => { Assert.AreSame(x, iar); };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new[] { new Unit() }));
        }

        [TestMethod]
        public void FromAsyncPattern11_Error()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, cb, o) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); throw ex; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void FromAsyncPattern11_ErrorBegin()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, cb, o) => { cb(x); throw ex; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 0; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void FromAsyncPattern12()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, cb, _) =>
            {
                Assert.AreEqual(a, 2);
                Assert.AreEqual(b, 3);
                Assert.AreEqual(c, 4);
                Assert.AreEqual(d, 5);
                Assert.AreEqual(e, 6);
                Assert.AreEqual(f, 7);
                Assert.AreEqual(g, 8);
                Assert.AreEqual(h, 9);
                Assert.AreEqual(i, 10);
                Assert.AreEqual(j, 11);
                Assert.AreEqual(k, 12);
                Assert.AreEqual(l, 13);
                cb(x);
                return x;
            };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext<int>(1), Notification.CreateOnCompleted<int>() }));
        }

        [TestMethod]
        public void FromAsyncPatternAction12()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, cb, _) =>
            {
                Assert.AreEqual(a, 2);
                Assert.AreEqual(b, 3);
                Assert.AreEqual(c, 4);
                Assert.AreEqual(d, 5);
                Assert.AreEqual(e, 6);
                Assert.AreEqual(f, 7);
                Assert.AreEqual(g, 8);
                Assert.AreEqual(h, 9);
                Assert.AreEqual(i, 10);
                Assert.AreEqual(j, 11);
                Assert.AreEqual(k, 12);
                Assert.AreEqual(l, 13);
                cb(x);
                return x;
            };
            Action<IAsyncResult> end = iar => { Assert.AreSame(x, iar); };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new[] { new Unit() }));
        }

        [TestMethod]
        public void FromAsyncPattern12_Error()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, cb, o) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); throw ex; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void FromAsyncPattern12_ErrorBegin()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, cb, o) => { cb(x); throw ex; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 0; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void FromAsyncPattern13()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, m, cb, _) =>
            {
                Assert.AreEqual(a, 2);
                Assert.AreEqual(b, 3);
                Assert.AreEqual(c, 4);
                Assert.AreEqual(d, 5);
                Assert.AreEqual(e, 6);
                Assert.AreEqual(f, 7);
                Assert.AreEqual(g, 8);
                Assert.AreEqual(h, 9);
                Assert.AreEqual(i, 10);
                Assert.AreEqual(j, 11);
                Assert.AreEqual(k, 12);
                Assert.AreEqual(l, 13);
                Assert.AreEqual(m, 14);
                cb(x);
                return x;
            };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext<int>(1), Notification.CreateOnCompleted<int>() }));
        }

        [TestMethod]
        public void FromAsyncPatternAction13()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, m, cb, _) =>
            {
                Assert.AreEqual(a, 2);
                Assert.AreEqual(b, 3);
                Assert.AreEqual(c, 4);
                Assert.AreEqual(d, 5);
                Assert.AreEqual(e, 6);
                Assert.AreEqual(f, 7);
                Assert.AreEqual(g, 8);
                Assert.AreEqual(h, 9);
                Assert.AreEqual(i, 10);
                Assert.AreEqual(j, 11);
                Assert.AreEqual(k, 12);
                Assert.AreEqual(l, 13);
                Assert.AreEqual(m, 14);
                cb(x);
                return x;
            };
            Action<IAsyncResult> end = iar => { Assert.AreSame(x, iar); };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new[] { new Unit() }));
        }

        [TestMethod]
        public void FromAsyncPattern13_Error()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, m, cb, o) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); throw ex; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void FromAsyncPattern13_ErrorBegin()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, m, cb, o) => { cb(x); throw ex; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 0; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void FromAsyncPattern14()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, m, n, cb, _) =>
            {
                Assert.AreEqual(a, 2);
                Assert.AreEqual(b, 3);
                Assert.AreEqual(c, 4);
                Assert.AreEqual(d, 5);
                Assert.AreEqual(e, 6);
                Assert.AreEqual(f, 7);
                Assert.AreEqual(g, 8);
                Assert.AreEqual(h, 9);
                Assert.AreEqual(i, 10);
                Assert.AreEqual(j, 11);
                Assert.AreEqual(k, 12);
                Assert.AreEqual(l, 13);
                Assert.AreEqual(m, 14);
                Assert.AreEqual(n, 15);
                cb(x);
                return x;
            };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext<int>(1), Notification.CreateOnCompleted<int>() }));
        }

        [TestMethod]
        public void FromAsyncPatternAction14()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, m, n, cb, _) =>
            {
                Assert.AreEqual(a, 2);
                Assert.AreEqual(b, 3);
                Assert.AreEqual(c, 4);
                Assert.AreEqual(d, 5);
                Assert.AreEqual(e, 6);
                Assert.AreEqual(f, 7);
                Assert.AreEqual(g, 8);
                Assert.AreEqual(h, 9);
                Assert.AreEqual(i, 10);
                Assert.AreEqual(j, 11);
                Assert.AreEqual(k, 12);
                Assert.AreEqual(l, 13);
                Assert.AreEqual(m, 14);
                Assert.AreEqual(n, 15);
                cb(x);
                return x;
            };
            Action<IAsyncResult> end = iar => { Assert.AreSame(x, iar); };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new[] { new Unit() }));
        }

        [TestMethod]
        public void FromAsyncPattern14_Error()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, m, n, cb, o) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); throw ex; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void FromAsyncPattern14_ErrorBegin()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, m, n, cb, o) => { cb(x); throw ex; };
            Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 0; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).Materialize().ToEnumerable().ToArray();
            Assert.IsTrue(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

#endif
        class Result : IAsyncResult
        {
            public object AsyncState
            {
                get { throw new NotImplementedException(); }
            }

            public System.Threading.WaitHandle AsyncWaitHandle
            {
                get { throw new NotImplementedException(); }
            }

            public bool CompletedSynchronously
            {
                get { throw new NotImplementedException(); }
            }

            public bool IsCompleted
            {
                get { throw new NotImplementedException(); }
            }
        }

        #endregion

        #region Start

        [TestMethod]
        public void Start_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Start(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Start<int>((Func<int>)null));

            var someScheduler = new TestScheduler();
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Start(null, someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Start<int>(null, someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Start(() => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Start<int>(() => 1, null));
        }


        [TestMethod]
        public void Start_Action()
        {
            bool done = false;
            Assert.IsTrue(Observable.Start(() => { done = true; }).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(done, "done");
        }

        [TestMethod]
        public void Start_Action2()
        {
            var scheduler = new TestScheduler();

            bool done = false;

            var res = scheduler.Start(() =>
                Observable.Start(() => { done = true; }, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(200, new Unit()),
                OnCompleted<Unit>(200)
            );

            Assert.IsTrue(done, "done");
        }

        [TestMethod]
        public void Start_ActionError()
        {
            var ex = new Exception();

            var res = Observable.Start(() => { throw ex; }).Materialize().ToEnumerable();

            Assert.IsTrue(res.SequenceEqual(new[] {
                Notification.CreateOnError<Unit>(ex)
            }));
        }

        [TestMethod]
        public void Start_Func()
        {
            var res = Observable.Start(() => 1).ToEnumerable();

            Assert.IsTrue(res.SequenceEqual(new[] {
                1
            }));
        }

        [TestMethod]
        public void Start_Func2()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Start(() => 1, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(200, 1),
                OnCompleted<int>(200)
            );
        }

        [TestMethod]
        public void Start_FuncError()
        {
            var ex = new Exception();

            var res = Observable.Start<int>(() => { throw ex; }).Materialize().ToEnumerable();

            Assert.IsTrue(res.SequenceEqual(new[] {
                Notification.CreateOnError<int>(ex)
            }));
        }

        #endregion

        #region StartAsync

#if !NO_TPL

        #region Func

        [TestMethod]
        public void StartAsync_Func_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartAsync<int>(default(Func<Task<int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartAsync<int>(default(Func<CancellationToken, Task<int>>)));
        }

        [TestMethod]
        public void StartAsync_Func_Success()
        {
            var n = 42;

            var i = 0;

            var xs = Observable.StartAsync(() =>
            {
                i++;
                return Task.Factory.StartNew(() => n);
            });

            Assert.AreEqual(n, xs.Single());
            Assert.AreEqual(1, i);

            Assert.AreEqual(n, xs.Single());
            Assert.AreEqual(1, i);
        }

        [TestMethod]
        public void StartAsync_Func_Throw_Synchronous()
        {
            var ex = new Exception();

            var xs = Observable.StartAsync<int>(() =>
            {
                throw ex;
            });

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void StartAsync_Func_Throw_Asynchronous()
        {
            var ex = new Exception();

            var xs = Observable.StartAsync<int>(() =>
                Task.Factory.StartNew<int>(() =>
                {
                    throw ex;
                })
            );

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void StartAsync_FuncWithCancel_Success()
        {
            var n = 42;

            var i = 0;
            var xs = Observable.StartAsync(ct =>
            {
                i++;
                return Task.Factory.StartNew(() => n);
            });

            Assert.AreEqual(n, xs.Single());
            Assert.AreEqual(1, i);

            Assert.AreEqual(n, xs.Single());
            Assert.AreEqual(1, i);
        }

        [TestMethod]
        public void StartAsync_FuncWithCancel_Throw_Synchronous()
        {
            var ex = new Exception();

            var xs = Observable.StartAsync<int>(ct =>
            {
                throw ex;
            });

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void StartAsync_FuncWithCancel_Throw_Asynchronous()
        {
            var ex = new Exception();

            var xs = Observable.StartAsync<int>(ct =>
                Task.Factory.StartNew<int>(() => { throw ex; })
            );

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void StartAsync_FuncWithCancel_Cancel()
        {
            var N = 10;

            for (int n = 0; n < N; n++)
            {
                var e = new ManualResetEvent(false);
                var f = new ManualResetEvent(false);

                var t = default(Task<int>);
                var xs = Observable.StartAsync(ct =>
                    t = Task.Factory.StartNew<int>(() =>
                    {
                        try
                        {
                            e.Set();
                            while (true)
                                ct.ThrowIfCancellationRequested();
                        }
                        finally
                        {
                            f.Set();
                        }
                    })
                );

                e.WaitOne();

                var d = xs.Subscribe(_ => { });
                d.Dispose();

                f.WaitOne();
                while (!t.IsCompleted)
                    ;

                ReactiveAssert.Throws<OperationCanceledException>(() => xs.Single());
            }
        }

        #endregion

        #region Action

        [TestMethod]
        public void StartAsync_Action_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartAsync(default(Func<Task>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartAsync(default(Func<CancellationToken, Task>)));
        }

        [TestMethod]
        public void StartAsync_Action_Success()
        {
            var i = 0;
            var xs = Observable.StartAsync(() =>
            {
                i++;
                return Task.Factory.StartNew(() => { });
            });

            Assert.AreEqual(Unit.Default, xs.Single());
            Assert.AreEqual(1, i);

            Assert.AreEqual(Unit.Default, xs.Single());
            Assert.AreEqual(1, i);
        }

        [TestMethod]
        public void StartAsync_Action_Throw_Synchronous()
        {
            var ex = new Exception();

            var xs = Observable.StartAsync(() =>
            {
                throw ex;
            });

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void StartAsync_Action_Throw_Asynchronous()
        {
            var ex = new Exception();

            var xs = Observable.StartAsync(() =>
                Task.Factory.StartNew(() => { throw ex; })
            );

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void StartAsync_ActionWithCancel_Success()
        {
            var i = 0;
            var xs = Observable.StartAsync(ct =>
            {
                i++;
                return Task.Factory.StartNew(() => { });
            });

            Assert.AreEqual(Unit.Default, xs.Single());
            Assert.AreEqual(1, i);

            Assert.AreEqual(Unit.Default, xs.Single());
            Assert.AreEqual(1, i);
        }

        [TestMethod]
        public void StartAsync_ActionWithCancel_Throw_Synchronous()
        {
            var ex = new Exception();

            var xs = Observable.StartAsync(ct =>
            {
                throw ex;
            });

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void StartAsync_ActionWithCancel_Throw_Asynchronous()
        {
            var ex = new Exception();

            var xs = Observable.StartAsync(ct =>
                Task.Factory.StartNew(() => { throw ex; })
            );

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void StartAsync_ActionWithCancel_Cancel()
        {
            var N = 10;

            for (int n = 0; n < N; n++)
            {
                var e = new ManualResetEvent(false);
                var f = new ManualResetEvent(false);

                var t = default(Task);
                var xs = Observable.StartAsync(ct =>
                    t = Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            e.Set();
                            while (true)
                                ct.ThrowIfCancellationRequested();
                        }
                        finally
                        {
                            f.Set();
                        }
                    })
                );

                e.WaitOne();

                var d = xs.Subscribe(_ => { });
                d.Dispose();

                f.WaitOne();
                while (!t.IsCompleted)
                    ;

                ReactiveAssert.Throws<OperationCanceledException>(() => xs.Single());
            }
        }

        #endregion

#endif

        #endregion

        #region FromAsync

#if !NO_TPL

        #region Func

        [TestMethod]
        public void FromAsync_Func_Success()
        {
            var n = 42;

            var i = 0;
            var xs = Observable.FromAsync(() =>
            {
                i++;
                return Task.Factory.StartNew(() => n);
            });

            Assert.AreEqual(n, xs.Single());
            Assert.AreEqual(1, i);

            Assert.AreEqual(n, xs.Single());
            Assert.AreEqual(2, i);
        }

        [TestMethod]
        public void FromAsync_Func_Throw_Synchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync<int>(() =>
            {
                throw ex;
            });

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void FromAsync_Func_Throw_Asynchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync<int>(() =>
                Task.Factory.StartNew<int>(() => { throw ex; })
            );

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void FromAsync_FuncWithCancel_Success()
        {
            var n = 42;

            var i = 0;
            var xs = Observable.FromAsync(ct =>
            {
                i++;
                return Task.Factory.StartNew(() => n);
            });

            Assert.AreEqual(n, xs.Single());
            Assert.AreEqual(1, i);

            Assert.AreEqual(n, xs.Single());
            Assert.AreEqual(2, i);
        }

        [TestMethod]
        public void FromAsync_FuncWithCancel_Throw_Synchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync<int>(ct =>
            {
                throw ex;
            });

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void FromAsync_FuncWithCancel_Throw_Asynchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync<int>(ct =>
                Task.Factory.StartNew<int>(() => { throw ex; })
            );

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void FromAsync_FuncWithCancel_Cancel()
        {
            var e = new ManualResetEvent(false);
            var f = new ManualResetEvent(false);

            var t = default(Task<int>);
            var xs = Observable.FromAsync(ct =>
                t = Task.Factory.StartNew<int>(() =>
                {
                    try
                    {
                        e.Set();
                        while (true)
                            ct.ThrowIfCancellationRequested();
                    }
                    finally
                    {
                        f.Set();
                    }
                })
            );

            var d = xs.Subscribe(_ => { });
            e.WaitOne();
            d.Dispose();

            f.WaitOne();
            while (!t.IsCompleted)
                ;
        }

        #endregion

        #region Action

        [TestMethod]
        public void FromAsync_Action_Success()
        {
            var i = 0;
            var xs = Observable.FromAsync(() =>
            {
                i++;
                return Task.Factory.StartNew(() => { });
            });

            Assert.AreEqual(Unit.Default, xs.Single());
            Assert.AreEqual(1, i);

            Assert.AreEqual(Unit.Default, xs.Single());
            Assert.AreEqual(2, i);
        }

        [TestMethod]
        public void FromAsync_Action_Throw_Synchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync(() =>
            {
                throw ex;
            });

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void FromAsync_Action_Throw_Asynchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync(() =>
                Task.Factory.StartNew(() => { throw ex; })
            );

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void FromAsync_ActionWithCancel_Success()
        {
            var i = 0;
            var xs = Observable.FromAsync(ct =>
            {
                i++;
                return Task.Factory.StartNew(() => { });
            });

            Assert.AreEqual(Unit.Default, xs.Single());
            Assert.AreEqual(1, i);

            Assert.AreEqual(Unit.Default, xs.Single());
            Assert.AreEqual(2, i);
        }

        [TestMethod]
        public void FromAsync_ActionWithCancel_Throw_Synchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync(ct =>
            {
                throw ex;
            });

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void FromAsync_ActionWithCancel_Throw_Asynchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync(ct =>
                Task.Factory.StartNew(() => { throw ex; })
            );

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void FromAsync_ActionWithCancel_Cancel()
        {
            var e = new ManualResetEvent(false);
            var f = new ManualResetEvent(false);

            var t = default(Task);
            var xs = Observable.FromAsync(ct =>
                t = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        e.Set();
                        while (true)
                            ct.ThrowIfCancellationRequested();
                    }
                    finally
                    {
                        f.Set();
                    }
                })
            );

            var d = xs.Subscribe(_ => { });
            e.WaitOne();
            d.Dispose();

            f.WaitOne();
            while (!t.IsCompleted)
                ;
        }

        #endregion

#endif

        #endregion

        #region ToAsync

        [TestMethod]
        public void ToAsync_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int>(default(Action<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int>(default(Func<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int>(default(Action<int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int>(default(Func<int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int>(default(Action<int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int>(default(Func<int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int>(default(Action<int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int>(default(Func<int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int>(default(Func<int, int, int, int, int>)));
#if !NO_LARGEARITY
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int>(default(Action<int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int>(default(Action<int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int>(default(Func<int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int>(default(Action<int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int>(default(Func<int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int>(default(Action<int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int>(default(Func<int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int>(default(Action<int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int>(default(Func<int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int>(default(Action<int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int>(default(Func<int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int>(default(Action<int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int>(default(Func<int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int>(default(Action<int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int>(default(Func<int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int>(default(Action<int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int>(default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(default(Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(default(Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(default(Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
#endif
            var someScheduler = new TestScheduler();
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int>(default(Action<int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int>(default(Func<int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int>(default(Action<int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int>(default(Func<int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int>(default(Action<int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int>(default(Func<int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int>(default(Action<int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int>(default(Func<int, int, int, int>), someScheduler));
#if !NO_LARGEARITY
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int>(default(Func<int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int>(default(Action<int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int>(default(Action<int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int>(default(Func<int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int>(default(Action<int, int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int>(default(Func<int, int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int>(default(Action<int, int, int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int>(default(Func<int, int, int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int>(default(Action<int, int, int, int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int>(default(Func<int, int, int, int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int>(default(Action<int, int, int, int, int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int>(default(Func<int, int, int, int, int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int>(default(Action<int, int, int, int, int, int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int>(default(Func<int, int, int, int, int, int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int>(default(Action<int, int, int, int, int, int, int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int>(default(Func<int, int, int, int, int, int, int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int>(default(Action<int, int, int, int, int, int, int, int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int>(default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(default(Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(default(Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(default(Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>), someScheduler));
#endif
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(() => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int>(a => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int>(() => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int>((a, b) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int>(a => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int>((a, b, c) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int>((a, b) => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int>((a, b, c, d) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int>((a, b, c) => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int>((a, b, c, d) => 1, null));
#if !NO_LARGEARITY
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int>((a, b, c, d, e) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int>((a, b, c, d, e, f) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int>((a, b, c, d, e) => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int>((a, b, c, d, e, f, g) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int>((a, b, c, d, e, f) => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g) => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h) => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i) => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j) => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k) => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => 1, null));
#endif
        }

        [TestMethod]
        public void ToAsync0()
        {
            Assert.IsTrue(Observable.ToAsync<int>(() => 0)().ToEnumerable().SequenceEqual(new[] { 0 }));
            Assert.IsTrue(Observable.ToAsync<int>(() => 0, Scheduler.Default)().ToEnumerable().SequenceEqual(new[] { 0 }));
        }

        [TestMethod]
        public void ToAsync1()
        {
            Assert.IsTrue(Observable.ToAsync<int, int>(a => a)(1).ToEnumerable().SequenceEqual(new[] { 1 }));
            Assert.IsTrue(Observable.ToAsync<int, int>(a => a, Scheduler.Default)(1).ToEnumerable().SequenceEqual(new[] { 1 }));
        }

        [TestMethod]
        public void ToAsync2()
        {
            Assert.IsTrue(Observable.ToAsync<int, int, int>((a, b) => a + b)(1, 2).ToEnumerable().SequenceEqual(new[] { 1 + 2 }));
            Assert.IsTrue(Observable.ToAsync<int, int, int>((a, b) => a + b, Scheduler.Default)(1, 2).ToEnumerable().SequenceEqual(new[] { 1 + 2 }));
        }

        [TestMethod]
        public void ToAsync3()
        {
            Assert.IsTrue(Observable.ToAsync<int, int, int, int>((a, b, c) => a + b + c)(1, 2, 3).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 }));
            Assert.IsTrue(Observable.ToAsync<int, int, int, int>((a, b, c) => a + b + c, Scheduler.Default)(1, 2, 3).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 }));
        }

        [TestMethod]
        public void ToAsync4()
        {
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int>((a, b, c, d) => a + b + c + d)(1, 2, 3, 4).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 }));
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int>((a, b, c, d) => a + b + c + d, Scheduler.Default)(1, 2, 3, 4).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 }));
        }

#if !NO_LARGEARITY

        [TestMethod]
        public void ToAsync5()
        {
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int>((a, b, c, d, e) => a + b + c + d + e)(1, 2, 3, 4, 5).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 }));
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int>((a, b, c, d, e) => a + b + c + d + e, Scheduler.Default)(1, 2, 3, 4, 5).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 }));
        }

        [TestMethod]
        public void ToAsync6()
        {
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int>((a, b, c, d, e, f) => a + b + c + d + e + f)(1, 2, 3, 4, 5, 6).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 }));
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int>((a, b, c, d, e, f) => a + b + c + d + e + f, Scheduler.Default)(1, 2, 3, 4, 5, 6).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 }));
        }

        [TestMethod]
        public void ToAsync7()
        {
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g) => a + b + c + d + e + f + g)(1, 2, 3, 4, 5, 6, 7).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 }));
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g) => a + b + c + d + e + f + g, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 }));
        }

        [TestMethod]
        public void ToAsync8()
        {
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h)(1, 2, 3, 4, 5, 6, 7, 8).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 }));
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 }));
        }

        [TestMethod]
        public void ToAsync9()
        {
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i) => a + b + c + d + e + f + g + h + i)(1, 2, 3, 4, 5, 6, 7, 8, 9).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 }));
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i) => a + b + c + d + e + f + g + h + i, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 }));
        }

        [TestMethod]
        public void ToAsync10()
        {
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j) => a + b + c + d + e + f + g + h + i + j)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 }));
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j) => a + b + c + d + e + f + g + h + i + j, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 }));
        }

        [TestMethod]
        public void ToAsync11()
        {
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k) => a + b + c + d + e + f + g + h + i + j + k)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 }));
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k) => a + b + c + d + e + f + g + h + i + j + k, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 }));
        }

        [TestMethod]
        public void ToAsync12()
        {
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) => a + b + c + d + e + f + g + h + i + j + k + l)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 }));
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) => a + b + c + d + e + f + g + h + i + j + k + l, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 }));
        }

        [TestMethod]
        public void ToAsync13()
        {
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) => a + b + c + d + e + f + g + h + i + j + k + l + m)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 }));
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) => a + b + c + d + e + f + g + h + i + j + k + l + m, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 }));
        }

        [TestMethod]
        public void ToAsync14()
        {
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => a + b + c + d + e + f + g + h + i + j + k + l + m + n)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 }));
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => a + b + c + d + e + f + g + h + i + j + k + l + m + n, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 }));
        }

        [TestMethod]
        public void ToAsync15()
        {
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => a + b + c + d + e + f + g + h + i + j + k + l + m + n + o)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 + 15 }));
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => a + b + c + d + e + f + g + h + i + j + k + l + m + n + o, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 + 15 }));
        }

        [TestMethod]
        public void ToAsync16()
        {
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => a + b + c + d + e + f + g + h + i + j + k + l + m + n + o + p)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 + 15 + 16 }));
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => a + b + c + d + e + f + g + h + i + j + k + l + m + n + o + p, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 + 15 + 16 }));
        }
#endif

        [TestMethod]
        public void ToAsync_Error0()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int>(() => { throw ex; })().Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void ToAsync_Error1()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int>(a => { throw ex; })(1).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void ToAsync_Error2()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int, int>((a, b) => { throw ex; })(1, 2).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void ToAsync_Error3()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int, int, int>((a, b, c) => { throw ex; })(1, 2, 3).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void ToAsync_Error4()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int>((a, b, c, d) => { throw ex; })(1, 2, 3, 4).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

#if !NO_LARGEARITY

        [TestMethod]
        public void ToAsync_Error5()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int>((a, b, c, d, e) => { throw ex; })(1, 2, 3, 4, 5).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void ToAsync_Error6()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int>((a, b, c, d, e, f) => { throw ex; })(1, 2, 3, 4, 5, 6).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void ToAsync_Error7()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g) => { throw ex; })(1, 2, 3, 4, 5, 6, 7).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void ToAsync_Error8()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h) => { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void ToAsync_Error9()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i) => { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void ToAsync_Error10()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j) => { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void ToAsync_Error11()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k) => { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void ToAsync_Error12()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) => { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void ToAsync_Error13()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) => { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void ToAsync_Error14()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void ToAsync_Error15()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [TestMethod]
        public void ToAsync_Error16()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }
#endif

        [TestMethod]
        public void ToAsyncAction0()
        {
            bool hasRun = false;
            Assert.IsTrue(Observable.ToAsync(() => { hasRun = true; })().ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(Observable.ToAsync(() => { hasRun = true; }, Scheduler.Default)().ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(hasRun, "has run");
        }

        [TestMethod]
        public void ToAsyncActionError0()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync(() => { throw ex; })().Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [TestMethod]
        public void ToAsyncAction1()
        {
            bool hasRun = false;
            Assert.IsTrue(Observable.ToAsync<int>(a => { Assert.AreEqual(1, a); hasRun = true; })(1).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(Observable.ToAsync<int>(a => { Assert.AreEqual(1, a); hasRun = true; }, Scheduler.Default)(1).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(hasRun, "has run");
        }

        [TestMethod]
        public void ToAsyncActionError1()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int>(a => { Assert.AreEqual(1, a); throw ex; })(1).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [TestMethod]
        public void ToAsyncAction2()
        {
            bool hasRun = false;
            Assert.IsTrue(Observable.ToAsync<int, int>((a, b) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); hasRun = true; })(1, 2).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(Observable.ToAsync<int, int>((a, b) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); hasRun = true; }, Scheduler.Default)(1, 2).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(hasRun, "has run");
        }

        [TestMethod]
        public void ToAsyncActionError2()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int>((a, b) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); throw ex; })(1, 2).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [TestMethod]
        public void ToAsyncAction3()
        {
            bool hasRun = false;
            Assert.IsTrue(Observable.ToAsync<int, int, int>((a, b, c) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); hasRun = true; })(1, 2, 3).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(Observable.ToAsync<int, int, int>((a, b, c) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); hasRun = true; }, Scheduler.Default)(1, 2, 3).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(hasRun, "has run");
        }

        [TestMethod]
        public void ToAsyncActionError3()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int, int>((a, b, c) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); throw ex; })(1, 2, 3).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [TestMethod]
        public void ToAsyncAction4()
        {
            bool hasRun = false;
            Assert.IsTrue(Observable.ToAsync<int, int, int, int>((a, b, c, d) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); hasRun = true; })(1, 2, 3, 4).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(Observable.ToAsync<int, int, int, int>((a, b, c, d) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(hasRun, "has run");
        }

        [TestMethod]
        public void ToAsyncActionError4()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int, int, int>((a, b, c, d) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); throw ex; })(1, 2, 3, 4).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

#if !NO_LARGEARITY

        [TestMethod]
        public void ToAsyncAction5()
        {
            bool hasRun = false;
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int>((a, b, c, d, e) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); hasRun = true; })(1, 2, 3, 4, 5).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int>((a, b, c, d, e) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(hasRun, "has run");
        }

        [TestMethod]
        public void ToAsyncActionError5()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int>((a, b, c, d, e) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); throw ex; })(1, 2, 3, 4, 5).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [TestMethod]
        public void ToAsyncAction6()
        {
            bool hasRun = false;
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int>((a, b, c, d, e, f) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); hasRun = true; })(1, 2, 3, 4, 5, 6).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int>((a, b, c, d, e, f) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(hasRun, "has run");
        }

        [TestMethod]
        public void ToAsyncActionError6()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int>((a, b, c, d, e, f) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); throw ex; })(1, 2, 3, 4, 5, 6).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [TestMethod]
        public void ToAsyncAction7()
        {
            bool hasRun = false;
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int>((a, b, c, d, e, f, g) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); hasRun = true; })(1, 2, 3, 4, 5, 6, 7).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int>((a, b, c, d, e, f, g) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(hasRun, "has run");
        }

        [TestMethod]
        public void ToAsyncActionError7()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int>((a, b, c, d, e, f, g) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); throw ex; })(1, 2, 3, 4, 5, 6, 7).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [TestMethod]
        public void ToAsyncAction8()
        {
            bool hasRun = false;
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8, h); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8, h); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(hasRun, "has run");
        }

        [TestMethod]
        public void ToAsyncActionError8()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8, h); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [TestMethod]
        public void ToAsyncAction9()
        {
            bool hasRun = false;
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8, 9).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(hasRun, "has run");
        }

        [TestMethod]
        public void ToAsyncActionError9()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [TestMethod]
        public void ToAsyncAction10()
        {
            bool hasRun = false;
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10, j); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10, j); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(hasRun, "has run");
        }

        [TestMethod]
        public void ToAsyncActionError10()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10, j); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [TestMethod]
        public void ToAsyncAction11()
        {
            bool hasRun = false;
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10, j); Assert.AreEqual(11, k); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10, j); Assert.AreEqual(11, k); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(hasRun, "has run");
        }

        [TestMethod]
        public void ToAsyncActionError11()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10, j); Assert.AreEqual(11, k); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [TestMethod]
        public void ToAsyncAction12()
        {
            bool hasRun = false;
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10, j); Assert.AreEqual(11, k); Assert.AreEqual(12, l); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10, j); Assert.AreEqual(11, k); Assert.AreEqual(12, l); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(hasRun, "has run");
        }

        [TestMethod]
        public void ToAsyncActionError12()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10, j); Assert.AreEqual(11, k); Assert.AreEqual(12, l); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [TestMethod]
        public void ToAsyncAction13()
        {
            bool hasRun = false;
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10, j); Assert.AreEqual(11, k); Assert.AreEqual(12, l); Assert.AreEqual(13, m); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10, j); Assert.AreEqual(11, k); Assert.AreEqual(12, l); Assert.AreEqual(13, m); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(hasRun, "has run");
        }

        [TestMethod]
        public void ToAsyncActionError13()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10, j); Assert.AreEqual(11, k); Assert.AreEqual(12, l); Assert.AreEqual(13, m); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [TestMethod]
        public void ToAsyncAction14()
        {
            bool hasRun = false;
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10, j); Assert.AreEqual(11, k); Assert.AreEqual(12, l); Assert.AreEqual(13, m); Assert.AreEqual(14, n); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10, j); Assert.AreEqual(11, k); Assert.AreEqual(12, l); Assert.AreEqual(13, m); Assert.AreEqual(14, n); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(hasRun, "has run");
        }

        [TestMethod]
        public void ToAsyncActionError14()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10, j); Assert.AreEqual(11, k); Assert.AreEqual(12, l); Assert.AreEqual(13, m); Assert.AreEqual(14, n); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [TestMethod]
        public void ToAsyncAction15()
        {
            bool hasRun = false;
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10, j); Assert.AreEqual(11, k); Assert.AreEqual(12, l); Assert.AreEqual(13, m); Assert.AreEqual(14, n); Assert.AreEqual(15, o); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10, j); Assert.AreEqual(11, k); Assert.AreEqual(12, l); Assert.AreEqual(13, m); Assert.AreEqual(14, n); Assert.AreEqual(15, o); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(hasRun, "has run");
        }

        [TestMethod]
        public void ToAsyncActionError15()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10, j); Assert.AreEqual(11, k); Assert.AreEqual(12, l); Assert.AreEqual(13, m); Assert.AreEqual(14, n); Assert.AreEqual(15, o); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [TestMethod]
        public void ToAsyncAction16()
        {
            bool hasRun = false;
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10, j); Assert.AreEqual(11, k); Assert.AreEqual(12, l); Assert.AreEqual(13, m); Assert.AreEqual(14, n); Assert.AreEqual(15, o); Assert.AreEqual(16, p); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10, j); Assert.AreEqual(11, k); Assert.AreEqual(12, l); Assert.AreEqual(13, m); Assert.AreEqual(14, n); Assert.AreEqual(15, o); Assert.AreEqual(16, p); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.IsTrue(hasRun, "has run");
        }

        [TestMethod]
        public void ToAsyncActionError16()
        {
            var ex = new Exception();
            Assert.IsTrue(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10, j); Assert.AreEqual(11, k); Assert.AreEqual(12, l); Assert.AreEqual(13, m); Assert.AreEqual(14, n); Assert.AreEqual(15, o); Assert.AreEqual(16, p); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }
#endif

        #endregion
    }
}
