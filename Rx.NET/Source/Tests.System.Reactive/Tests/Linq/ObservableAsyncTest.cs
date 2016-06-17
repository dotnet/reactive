// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Microsoft.Reactive.Testing;
using Xunit;

#if !NO_TPL
using System.Threading.Tasks;
#endif

namespace ReactiveTests.Tests
{
    
    public partial class ObservableAsyncTest : ReactiveTest
    {
#if !NO_TPL
        private Task<int> doneTask;

        public ObservableAsyncTest()
        {
            var tcs = new TaskCompletionSource<int>();
            tcs.SetResult(42);
            doneTask = tcs.Task;
        }

#endif
        #region FromAsyncPattern

        [Fact]
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

        [Fact]
        public void FromAsyncPattern0()
        {
            var x = new Result();

            Func<AsyncCallback, object, IAsyncResult> begin = (cb, _) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)().Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext<int>(1), Notification.CreateOnCompleted<int>() }));
        }

        [Fact]
        public void FromAsyncPatternAction0()
        {
            var x = new Result();

            Func<AsyncCallback, object, IAsyncResult> begin = (cb, _) => { cb(x); return x; };
            Action<IAsyncResult> end = iar => { Assert.Same(x, iar); };

            var res = Observable.FromAsyncPattern(begin, end)().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new[] { new Unit() }));
        }

        [Fact]
        public void FromAsyncPattern0_Error()
        {
            var x = new Result();
            var ex = new Exception();

            Func<AsyncCallback, object, IAsyncResult> begin = (cb, _) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); throw ex; };

            var res = Observable.FromAsyncPattern(begin, end)().Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void FromAsyncPattern0_ErrorBegin()
        {
            var x = new Result();
            var ex = new Exception();

            Func<AsyncCallback, object, IAsyncResult> begin = (cb, _) => { cb(x); throw ex; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 0; };

            var res = Observable.FromAsyncPattern(begin, end)().Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void FromAsyncPattern1()
        {
            var x = new Result();

            Func<int, AsyncCallback, object, IAsyncResult> begin = (a, cb, _) =>
            {
                Assert.Equal(a, 2);
                cb(x);
                return x;
            };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)(2).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext<int>(1), Notification.CreateOnCompleted<int>() }));
        }

        [Fact]
        public void FromAsyncPatternAction1()
        {
            var x = new Result();

            Func<int, AsyncCallback, object, IAsyncResult> begin = (a, cb, _) =>
            {
                Assert.Equal(a, 2);
                cb(x);
                return x;
            };
            Action<IAsyncResult> end = iar => { Assert.Same(x, iar); };

            var res = Observable.FromAsyncPattern(begin, end)(2).ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new[] { new Unit() }));
        }

        [Fact]
        public void FromAsyncPattern1_Error()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, AsyncCallback, object, IAsyncResult> begin = (a, cb, o) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); throw ex; };

            var res = Observable.FromAsyncPattern(begin, end)(2).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void FromAsyncPattern1_ErrorBegin()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, AsyncCallback, object, IAsyncResult> begin = (a, cb, o) => { cb(x); throw ex; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 0; };

            var res = Observable.FromAsyncPattern(begin, end)(2).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void FromAsyncPattern2()
        {
            var x = new Result();

            Func<int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, cb, _) =>
            {
                Assert.Equal(a, 2);
                Assert.Equal(b, 3);
                cb(x);
                return x;
            };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext<int>(1), Notification.CreateOnCompleted<int>() }));
        }

        [Fact]
        public void FromAsyncPatternAction2()
        {
            var x = new Result();

            Func<int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, cb, _) =>
            {
                Assert.Equal(a, 2);
                Assert.Equal(b, 3);
                cb(x);
                return x;
            };
            Action<IAsyncResult> end = iar => { Assert.Same(x, iar); };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3).ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new[] { new Unit() }));
        }

        [Fact]
        public void FromAsyncPattern2_Error()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, cb, o) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); throw ex; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void FromAsyncPattern2_ErrorBegin()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, cb, o) => { cb(x); throw ex; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 0; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

#if !NO_LARGEARITY
        [Fact]
        public void FromAsyncPattern3()
        {
            var x = new Result();

            Func<int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, cb, _) =>
            {
                Assert.Equal(a, 2);
                Assert.Equal(b, 3);
                Assert.Equal(c, 4);
                cb(x);
                return x;
            };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext<int>(1), Notification.CreateOnCompleted<int>() }));
        }
        [Fact]
        public void FromAsyncPatternAction3()
        {
            var x = new Result();

            Func<int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, cb, _) =>
            {
                Assert.Equal(a, 2);
                Assert.Equal(b, 3);
                Assert.Equal(c, 4);
                cb(x);
                return x;
            };
            Action<IAsyncResult> end = iar => { Assert.Same(x, iar); };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4).ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new[] { new Unit() }));
        }

        [Fact]
        public void FromAsyncPattern3_Error()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, cb, o) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); throw ex; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void FromAsyncPattern3_ErrorBegin()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, cb, o) => { cb(x); throw ex; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 0; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void FromAsyncPattern4()
        {
            var x = new Result();

            Func<int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, cb, _) =>
            {
                Assert.Equal(a, 2);
                Assert.Equal(b, 3);
                Assert.Equal(c, 4);
                Assert.Equal(d, 5);
                cb(x);
                return x;
            };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext<int>(1), Notification.CreateOnCompleted<int>() }));
        }

        [Fact]
        public void FromAsyncPatternAction4()
        {
            var x = new Result();

            Func<int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, cb, _) =>
            {
                Assert.Equal(a, 2);
                Assert.Equal(b, 3);
                Assert.Equal(c, 4);
                Assert.Equal(d, 5);
                cb(x);
                return x;
            };
            Action<IAsyncResult> end = iar => { Assert.Same(x, iar); };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5).ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new[] { new Unit() }));
        }

        [Fact]
        public void FromAsyncPattern4_Error()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, cb, o) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); throw ex; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void FromAsyncPattern4_ErrorBegin()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, cb, o) => { cb(x); throw ex; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 0; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void FromAsyncPattern5()
        {
            var x = new Result();

            Func<int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, cb, _) =>
            {
                Assert.Equal(a, 2);
                Assert.Equal(b, 3);
                Assert.Equal(c, 4);
                Assert.Equal(d, 5);
                Assert.Equal(e, 6);
                cb(x);
                return x;
            };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext<int>(1), Notification.CreateOnCompleted<int>() }));
        }

        [Fact]
        public void FromAsyncPatternAction5()
        {
            var x = new Result();

            Func<int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, cb, _) =>
            {
                Assert.Equal(a, 2);
                Assert.Equal(b, 3);
                Assert.Equal(c, 4);
                Assert.Equal(d, 5);
                Assert.Equal(e, 6);
                cb(x);
                return x;
            };
            Action<IAsyncResult> end = iar => { Assert.Same(x, iar); };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6).ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new[] { new Unit() }));
        }

        [Fact]
        public void FromAsyncPattern5_Error()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, cb, o) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); throw ex; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void FromAsyncPattern5_ErrorBegin()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, cb, o) => { cb(x); throw ex; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 0; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void FromAsyncPattern6()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, cb, _) =>
            {
                Assert.Equal(a, 2);
                Assert.Equal(b, 3);
                Assert.Equal(c, 4);
                Assert.Equal(d, 5);
                Assert.Equal(e, 6);
                Assert.Equal(f, 7);
                cb(x);
                return x;
            };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext<int>(1), Notification.CreateOnCompleted<int>() }));
        }

        [Fact]
        public void FromAsyncPatternAction6()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, cb, _) =>
            {
                Assert.Equal(a, 2);
                Assert.Equal(b, 3);
                Assert.Equal(c, 4);
                Assert.Equal(d, 5);
                Assert.Equal(e, 6);
                Assert.Equal(f, 7);
                cb(x);
                return x;
            };
            Action<IAsyncResult> end = iar => { Assert.Same(x, iar); };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7).ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new[] { new Unit() }));
        }

        [Fact]
        public void FromAsyncPattern6_Error()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, cb, o) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); throw ex; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void FromAsyncPattern6_ErrorBegin()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, cb, o) => { cb(x); throw ex; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 0; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void FromAsyncPattern7()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, cb, _) =>
            {
                Assert.Equal(a, 2);
                Assert.Equal(b, 3);
                Assert.Equal(c, 4);
                Assert.Equal(d, 5);
                Assert.Equal(e, 6);
                Assert.Equal(f, 7);
                Assert.Equal(g, 8);
                cb(x);
                return x;
            };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext<int>(1), Notification.CreateOnCompleted<int>() }));
        }

        [Fact]
        public void FromAsyncPatternAction7()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, cb, _) =>
            {
                Assert.Equal(a, 2);
                Assert.Equal(b, 3);
                Assert.Equal(c, 4);
                Assert.Equal(d, 5);
                Assert.Equal(e, 6);
                Assert.Equal(f, 7);
                Assert.Equal(g, 8);
                cb(x);
                return x;
            };
            Action<IAsyncResult> end = iar => { Assert.Same(x, iar); };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8).ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new[] { new Unit() }));
        }

        [Fact]
        public void FromAsyncPattern7_Error()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, cb, o) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); throw ex; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void FromAsyncPattern7_ErrorBegin()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, cb, o) => { cb(x); throw ex; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 0; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void FromAsyncPattern8()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, cb, _) =>
            {
                Assert.Equal(a, 2);
                Assert.Equal(b, 3);
                Assert.Equal(c, 4);
                Assert.Equal(d, 5);
                Assert.Equal(e, 6);
                Assert.Equal(f, 7);
                Assert.Equal(g, 8);
                Assert.Equal(h, 9);
                cb(x);
                return x;
            };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext<int>(1), Notification.CreateOnCompleted<int>() }));
        }

        [Fact]
        public void FromAsyncPatternAction8()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, cb, _) =>
            {
                Assert.Equal(a, 2);
                Assert.Equal(b, 3);
                Assert.Equal(c, 4);
                Assert.Equal(d, 5);
                Assert.Equal(e, 6);
                Assert.Equal(f, 7);
                Assert.Equal(g, 8);
                Assert.Equal(h, 9);
                cb(x);
                return x;
            };
            Action<IAsyncResult> end = iar => { Assert.Same(x, iar); };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9).ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new[] { new Unit() }));
        }

        [Fact]
        public void FromAsyncPattern8_Error()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, cb, o) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); throw ex; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void FromAsyncPattern8_ErrorBegin()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, cb, o) => { cb(x); throw ex; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 0; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void FromAsyncPattern9()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, cb, _) =>
            {
                Assert.Equal(a, 2);
                Assert.Equal(b, 3);
                Assert.Equal(c, 4);
                Assert.Equal(d, 5);
                Assert.Equal(e, 6);
                Assert.Equal(f, 7);
                Assert.Equal(g, 8);
                Assert.Equal(h, 9);
                Assert.Equal(i, 10);
                cb(x);
                return x;
            };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext<int>(1), Notification.CreateOnCompleted<int>() }));
        }

        [Fact]
        public void FromAsyncPatternAction9()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, cb, _) =>
            {
                Assert.Equal(a, 2);
                Assert.Equal(b, 3);
                Assert.Equal(c, 4);
                Assert.Equal(d, 5);
                Assert.Equal(e, 6);
                Assert.Equal(f, 7);
                Assert.Equal(g, 8);
                Assert.Equal(h, 9);
                Assert.Equal(i, 10);
                cb(x);
                return x;
            };
            Action<IAsyncResult> end = iar => { Assert.Same(x, iar); };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10).ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new[] { new Unit() }));
        }

        [Fact]
        public void FromAsyncPattern9_Error()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, cb, o) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); throw ex; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void FromAsyncPattern9_ErrorBegin()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, cb, o) => { cb(x); throw ex; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 0; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void FromAsyncPattern10()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, cb, _) =>
            {
                Assert.Equal(a, 2);
                Assert.Equal(b, 3);
                Assert.Equal(c, 4);
                Assert.Equal(d, 5);
                Assert.Equal(e, 6);
                Assert.Equal(f, 7);
                Assert.Equal(g, 8);
                Assert.Equal(h, 9);
                Assert.Equal(i, 10);
                Assert.Equal(j, 11);
                cb(x);
                return x;
            };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext<int>(1), Notification.CreateOnCompleted<int>() }));
        }

        [Fact]
        public void FromAsyncPatternAction10()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, cb, _) =>
            {
                Assert.Equal(a, 2);
                Assert.Equal(b, 3);
                Assert.Equal(c, 4);
                Assert.Equal(d, 5);
                Assert.Equal(e, 6);
                Assert.Equal(f, 7);
                Assert.Equal(g, 8);
                Assert.Equal(h, 9);
                Assert.Equal(i, 10);
                Assert.Equal(j, 11);
                cb(x);
                return x;
            };
            Action<IAsyncResult> end = iar => { Assert.Same(x, iar); };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11).ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new[] { new Unit() }));
        }

        [Fact]
        public void FromAsyncPattern10_Error()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, cb, o) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); throw ex; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void FromAsyncPattern10_ErrorBegin()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, cb, o) => { cb(x); throw ex; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 0; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void FromAsyncPattern11()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, cb, _) =>
            {
                Assert.Equal(a, 2);
                Assert.Equal(b, 3);
                Assert.Equal(c, 4);
                Assert.Equal(d, 5);
                Assert.Equal(e, 6);
                Assert.Equal(f, 7);
                Assert.Equal(g, 8);
                Assert.Equal(h, 9);
                Assert.Equal(i, 10);
                Assert.Equal(j, 11);
                Assert.Equal(k, 12);
                cb(x);
                return x;
            };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext<int>(1), Notification.CreateOnCompleted<int>() }));
        }

        [Fact]
        public void FromAsyncPatternAction11()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, cb, _) =>
            {
                Assert.Equal(a, 2);
                Assert.Equal(b, 3);
                Assert.Equal(c, 4);
                Assert.Equal(d, 5);
                Assert.Equal(e, 6);
                Assert.Equal(f, 7);
                Assert.Equal(g, 8);
                Assert.Equal(h, 9);
                Assert.Equal(i, 10);
                Assert.Equal(j, 11);
                Assert.Equal(k, 12);
                cb(x);
                return x;
            };
            Action<IAsyncResult> end = iar => { Assert.Same(x, iar); };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new[] { new Unit() }));
        }

        [Fact]
        public void FromAsyncPattern11_Error()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, cb, o) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); throw ex; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void FromAsyncPattern11_ErrorBegin()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, cb, o) => { cb(x); throw ex; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 0; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void FromAsyncPattern12()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, cb, _) =>
            {
                Assert.Equal(a, 2);
                Assert.Equal(b, 3);
                Assert.Equal(c, 4);
                Assert.Equal(d, 5);
                Assert.Equal(e, 6);
                Assert.Equal(f, 7);
                Assert.Equal(g, 8);
                Assert.Equal(h, 9);
                Assert.Equal(i, 10);
                Assert.Equal(j, 11);
                Assert.Equal(k, 12);
                Assert.Equal(l, 13);
                cb(x);
                return x;
            };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext<int>(1), Notification.CreateOnCompleted<int>() }));
        }

        [Fact]
        public void FromAsyncPatternAction12()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, cb, _) =>
            {
                Assert.Equal(a, 2);
                Assert.Equal(b, 3);
                Assert.Equal(c, 4);
                Assert.Equal(d, 5);
                Assert.Equal(e, 6);
                Assert.Equal(f, 7);
                Assert.Equal(g, 8);
                Assert.Equal(h, 9);
                Assert.Equal(i, 10);
                Assert.Equal(j, 11);
                Assert.Equal(k, 12);
                Assert.Equal(l, 13);
                cb(x);
                return x;
            };
            Action<IAsyncResult> end = iar => { Assert.Same(x, iar); };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new[] { new Unit() }));
        }

        [Fact]
        public void FromAsyncPattern12_Error()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, cb, o) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); throw ex; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void FromAsyncPattern12_ErrorBegin()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, cb, o) => { cb(x); throw ex; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 0; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void FromAsyncPattern13()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, m, cb, _) =>
            {
                Assert.Equal(a, 2);
                Assert.Equal(b, 3);
                Assert.Equal(c, 4);
                Assert.Equal(d, 5);
                Assert.Equal(e, 6);
                Assert.Equal(f, 7);
                Assert.Equal(g, 8);
                Assert.Equal(h, 9);
                Assert.Equal(i, 10);
                Assert.Equal(j, 11);
                Assert.Equal(k, 12);
                Assert.Equal(l, 13);
                Assert.Equal(m, 14);
                cb(x);
                return x;
            };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext<int>(1), Notification.CreateOnCompleted<int>() }));
        }

        [Fact]
        public void FromAsyncPatternAction13()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, m, cb, _) =>
            {
                Assert.Equal(a, 2);
                Assert.Equal(b, 3);
                Assert.Equal(c, 4);
                Assert.Equal(d, 5);
                Assert.Equal(e, 6);
                Assert.Equal(f, 7);
                Assert.Equal(g, 8);
                Assert.Equal(h, 9);
                Assert.Equal(i, 10);
                Assert.Equal(j, 11);
                Assert.Equal(k, 12);
                Assert.Equal(l, 13);
                Assert.Equal(m, 14);
                cb(x);
                return x;
            };
            Action<IAsyncResult> end = iar => { Assert.Same(x, iar); };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new[] { new Unit() }));
        }

        [Fact]
        public void FromAsyncPattern13_Error()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, m, cb, o) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); throw ex; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void FromAsyncPattern13_ErrorBegin()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, m, cb, o) => { cb(x); throw ex; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 0; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void FromAsyncPattern14()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, m, n, cb, _) =>
            {
                Assert.Equal(a, 2);
                Assert.Equal(b, 3);
                Assert.Equal(c, 4);
                Assert.Equal(d, 5);
                Assert.Equal(e, 6);
                Assert.Equal(f, 7);
                Assert.Equal(g, 8);
                Assert.Equal(h, 9);
                Assert.Equal(i, 10);
                Assert.Equal(j, 11);
                Assert.Equal(k, 12);
                Assert.Equal(l, 13);
                Assert.Equal(m, 14);
                Assert.Equal(n, 15);
                cb(x);
                return x;
            };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext<int>(1), Notification.CreateOnCompleted<int>() }));
        }

        [Fact]
        public void FromAsyncPatternAction14()
        {
            var x = new Result();

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, m, n, cb, _) =>
            {
                Assert.Equal(a, 2);
                Assert.Equal(b, 3);
                Assert.Equal(c, 4);
                Assert.Equal(d, 5);
                Assert.Equal(e, 6);
                Assert.Equal(f, 7);
                Assert.Equal(g, 8);
                Assert.Equal(h, 9);
                Assert.Equal(i, 10);
                Assert.Equal(j, 11);
                Assert.Equal(k, 12);
                Assert.Equal(l, 13);
                Assert.Equal(m, 14);
                Assert.Equal(n, 15);
                cb(x);
                return x;
            };
            Action<IAsyncResult> end = iar => { Assert.Same(x, iar); };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new[] { new Unit() }));
        }

        [Fact]
        public void FromAsyncPattern14_Error()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, m, n, cb, o) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); throw ex; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void FromAsyncPattern14_ErrorBegin()
        {
            var x = new Result();
            var ex = new Exception();

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, m, n, cb, o) => { cb(x); throw ex; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 0; };

            var res = Observable.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
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

        [Fact]
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


        [Fact]
        public void Start_Action()
        {
            bool done = false;
            Assert.True(Observable.Start(() => { done = true; }).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(done, "done");
        }

        [Fact]
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

            Assert.True(done, "done");
        }

        [Fact]
        public void Start_ActionError()
        {
            var ex = new Exception();

            var res = Observable.Start(() => { throw ex; }).Materialize().ToEnumerable();

            Assert.True(res.SequenceEqual(new[] {
                Notification.CreateOnError<Unit>(ex)
            }));
        }

        [Fact]
        public void Start_Func()
        {
            var res = Observable.Start(() => 1).ToEnumerable();

            Assert.True(res.SequenceEqual(new[] {
                1
            }));
        }

        [Fact]
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

        [Fact]
        public void Start_FuncError()
        {
            var ex = new Exception();

            var res = Observable.Start<int>(() => { throw ex; }).Materialize().ToEnumerable();

            Assert.True(res.SequenceEqual(new[] {
                Notification.CreateOnError<int>(ex)
            }));
        }

#endregion

#region StartAsync

#if !NO_TPL

#region Func

        [Fact]
        public void StartAsync_Func_ArgumentChecking()
        {
            var s = Scheduler.Immediate;

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartAsync<int>(default(Func<Task<int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartAsync<int>(default(Func<CancellationToken, Task<int>>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartAsync<int>(default(Func<Task<int>>), s));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartAsync<int>(default(Func<CancellationToken, Task<int>>), s));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartAsync<int>(() => doneTask, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartAsync<int>(ct => doneTask, default(IScheduler)));
        }

        [Fact]
        public void StartAsync_Func_Success()
        {
            var n = 42;

            var i = 0;

            var xs = Observable.StartAsync(() =>
            {
                i++;
                return Task.Factory.StartNew(() => n);
            });

            Assert.Equal(n, xs.Single());
            Assert.Equal(1, i);

            Assert.Equal(n, xs.Single());
            Assert.Equal(1, i);
        }

        [Fact]
        public void StartAsync_Func_Throw_Synchronous()
        {
            var ex = new Exception();

            var xs = Observable.StartAsync<int>(() =>
            {
                throw ex;
            });

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [Fact]
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

        [Fact]
        public void StartAsync_FuncWithCancel_Success()
        {
            var n = 42;

            var i = 0;
            var xs = Observable.StartAsync(ct =>
            {
                i++;
                return Task.Factory.StartNew(() => n);
            });

            Assert.Equal(n, xs.Single());
            Assert.Equal(1, i);

            Assert.Equal(n, xs.Single());
            Assert.Equal(1, i);
        }

        [Fact]
        public void StartAsync_FuncWithCancel_Throw_Synchronous()
        {
            var ex = new Exception();

            var xs = Observable.StartAsync<int>(ct =>
            {
                throw ex;
            });

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [Fact]
        public void StartAsync_FuncWithCancel_Throw_Asynchronous()
        {
            var ex = new Exception();

            var xs = Observable.StartAsync<int>(ct =>
                Task.Factory.StartNew<int>(() => { throw ex; })
            );

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [Fact]
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

#if DESKTOPCLR
        [Fact]
        public void StartAsync_Func_Scheduler1()
        {
            var tcs = new TaskCompletionSource<int>();

            var e = new ManualResetEvent(false);
            var x = default(int);
            var t = default(int);

            var xs = Observable.StartAsync(() => tcs.Task, Scheduler.Immediate);
            xs.Subscribe(res =>
            {
                x = res;
                t = Thread.CurrentThread.ManagedThreadId;
                e.Set();
            });

            tcs.SetResult(42);

            e.WaitOne();

            Assert.Equal(42, x);
            Assert.Equal(Thread.CurrentThread.ManagedThreadId, t);
        }

        [Fact]
        public void StartAsync_Func_Scheduler2()
        {
            var tcs = new TaskCompletionSource<int>();

            var e = new ManualResetEvent(false);
            var x = default(int);
            var t = default(int);

            var xs = Observable.StartAsync(ct => tcs.Task, Scheduler.Immediate);
            xs.Subscribe(res =>
            {
                x = res;
                t = Thread.CurrentThread.ManagedThreadId;
                e.Set();
            });

            tcs.SetResult(42);

            e.WaitOne();

            Assert.Equal(42, x);
            Assert.Equal(Thread.CurrentThread.ManagedThreadId, t);
        }
#endif

#endregion

#region Action

        [Fact]
        public void StartAsync_Action_ArgumentChecking()
        {
            var s = Scheduler.Immediate;

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartAsync(default(Func<Task>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartAsync(default(Func<CancellationToken, Task>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartAsync(default(Func<Task>), s));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartAsync(default(Func<CancellationToken, Task>), s));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartAsync(() => (Task)doneTask, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartAsync(ct => (Task)doneTask, default(IScheduler)));
        }

        [Fact]
        public void StartAsync_Action_Success()
        {
            var i = 0;
            var xs = Observable.StartAsync(() =>
            {
                i++;
                return Task.Factory.StartNew(() => { });
            });

            Assert.Equal(Unit.Default, xs.Single());
            Assert.Equal(1, i);

            Assert.Equal(Unit.Default, xs.Single());
            Assert.Equal(1, i);
        }

        [Fact]
        public void StartAsync_Action_Throw_Synchronous()
        {
            var ex = new Exception();

            var xs = Observable.StartAsync(() =>
            {
                throw ex;
            });

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [Fact]
        public void StartAsync_Action_Throw_Asynchronous()
        {
            var ex = new Exception();

            var xs = Observable.StartAsync(() =>
                Task.Factory.StartNew(() => { throw ex; })
            );

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [Fact]
        public void StartAsync_ActionWithCancel_Success()
        {
            var i = 0;
            var xs = Observable.StartAsync(ct =>
            {
                i++;
                return Task.Factory.StartNew(() => { });
            });

            Assert.Equal(Unit.Default, xs.Single());
            Assert.Equal(1, i);

            Assert.Equal(Unit.Default, xs.Single());
            Assert.Equal(1, i);
        }

        [Fact]
        public void StartAsync_ActionWithCancel_Throw_Synchronous()
        {
            var ex = new Exception();

            var xs = Observable.StartAsync(ct =>
            {
                throw ex;
            });

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [Fact]
        public void StartAsync_ActionWithCancel_Throw_Asynchronous()
        {
            var ex = new Exception();

            var xs = Observable.StartAsync(ct =>
                Task.Factory.StartNew(() => { throw ex; })
            );

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [Fact]
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

#if DESKTOPCLR
        [Fact]
        public void StartAsync_Action_Scheduler1()
        {
            var tcs = new TaskCompletionSource<int>();

            var e = new ManualResetEvent(false);
            var t = default(int);

            var xs = Observable.StartAsync(() => (Task)tcs.Task, Scheduler.Immediate);
            xs.Subscribe(res =>
            {
                t = Thread.CurrentThread.ManagedThreadId;
                e.Set();
            });

            tcs.SetResult(42);

            e.WaitOne();

            Assert.Equal(Thread.CurrentThread.ManagedThreadId, t);
        }

        [Fact]
        public void StartAsync_Action_Scheduler2()
        {
            var tcs = new TaskCompletionSource<int>();

            var e = new ManualResetEvent(false);
            var t = default(int);

            var xs = Observable.StartAsync(ct => (Task)tcs.Task, Scheduler.Immediate);
            xs.Subscribe(res =>
            {
                t = Thread.CurrentThread.ManagedThreadId;
                e.Set();
            });

            tcs.SetResult(42);

            e.WaitOne();

            Assert.Equal(Thread.CurrentThread.ManagedThreadId, t);
        }
#endif

#endregion

#endif

#endregion

#region FromAsync

#if !NO_TPL

#region Func

        [Fact]
        public void FromAsync_Func_ArgumentChecking()
        {
            var s = Scheduler.Immediate;

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync<int>(default(Func<Task<int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync<int>(default(Func<CancellationToken, Task<int>>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync<int>(default(Func<Task<int>>), s));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync<int>(() => doneTask, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync<int>(default(Func<CancellationToken, Task<int>>), s));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync<int>(ct => doneTask, default(IScheduler)));
        }

        [Fact]
        public void FromAsync_Func_Success()
        {
            var n = 42;

            var i = 0;
            var xs = Observable.FromAsync(() =>
            {
                i++;
                return Task.Factory.StartNew(() => n);
            });

            Assert.Equal(n, xs.Single());
            Assert.Equal(1, i);

            Assert.Equal(n, xs.Single());
            Assert.Equal(2, i);
        }

        [Fact]
        public void FromAsync_Func_Throw_Synchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync<int>(() =>
            {
                throw ex;
            });

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [Fact]
        public void FromAsync_Func_Throw_Asynchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync<int>(() =>
                Task.Factory.StartNew<int>(() => { throw ex; })
            );

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [Fact]
        public void FromAsync_FuncWithCancel_Success()
        {
            var n = 42;

            var i = 0;
            var xs = Observable.FromAsync(ct =>
            {
                i++;
                return Task.Factory.StartNew(() => n);
            });

            Assert.Equal(n, xs.Single());
            Assert.Equal(1, i);

            Assert.Equal(n, xs.Single());
            Assert.Equal(2, i);
        }

        [Fact]
        public void FromAsync_FuncWithCancel_Throw_Synchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync<int>(ct =>
            {
                throw ex;
            });

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [Fact]
        public void FromAsync_FuncWithCancel_Throw_Asynchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync<int>(ct =>
                Task.Factory.StartNew<int>(() => { throw ex; })
            );

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [Fact]
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

#if DESKTOPCLR
        [Fact]
        public void FromAsync_Func_Scheduler1()
        {
            var e = new ManualResetEvent(false);
            var x = default(int);
            var t = default(int);

            var tcs = new TaskCompletionSource<int>();

            var xs = Observable.FromAsync(() => tcs.Task, Scheduler.Immediate);
            xs.Subscribe(res =>
            {
                x = res;
                t = Thread.CurrentThread.ManagedThreadId;
                e.Set();
            });

            tcs.SetResult(42);

            e.WaitOne();

            Assert.Equal(42, x);
            Assert.Equal(Thread.CurrentThread.ManagedThreadId, t);
        }

        [Fact]
        public void FromAsync_Func_Scheduler2()
        {
            var e = new ManualResetEvent(false);
            var x = default(int);
            var t = default(int);

            var tcs = new TaskCompletionSource<int>();

            var xs = Observable.FromAsync(ct => tcs.Task, Scheduler.Immediate);
            xs.Subscribe(res =>
            {
                x = res;
                t = Thread.CurrentThread.ManagedThreadId;
                e.Set();
            });

            tcs.SetResult(42);

            e.WaitOne();

            Assert.Equal(42, x);
            Assert.Equal(Thread.CurrentThread.ManagedThreadId, t);
        }
#endif

#endregion

#region Action

        [Fact]
        public void FromAsync_Action_ArgumentChecking()
        {
            var s = Scheduler.Immediate;

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync(default(Func<Task>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync(default(Func<CancellationToken, Task>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync(default(Func<Task>), s));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync(() => (Task)doneTask, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync(default(Func<CancellationToken, Task>), s));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync(ct => (Task)doneTask, default(IScheduler)));
        }

        [Fact]
        public void FromAsync_Action_Success()
        {
            var i = 0;
            var xs = Observable.FromAsync(() =>
            {
                i++;
                return Task.Factory.StartNew(() => { });
            });

            Assert.Equal(Unit.Default, xs.Single());
            Assert.Equal(1, i);

            Assert.Equal(Unit.Default, xs.Single());
            Assert.Equal(2, i);
        }

        [Fact]
        public void FromAsync_Action_Throw_Synchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync(() =>
            {
                throw ex;
            });

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [Fact]
        public void FromAsync_Action_Throw_Asynchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync(() =>
                Task.Factory.StartNew(() => { throw ex; })
            );

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [Fact]
        public void FromAsync_ActionWithCancel_Success()
        {
            var i = 0;
            var xs = Observable.FromAsync(ct =>
            {
                i++;
                return Task.Factory.StartNew(() => { });
            });

            Assert.Equal(Unit.Default, xs.Single());
            Assert.Equal(1, i);

            Assert.Equal(Unit.Default, xs.Single());
            Assert.Equal(2, i);
        }

        [Fact]
        public void FromAsync_ActionWithCancel_Throw_Synchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync(ct =>
            {
                throw ex;
            });

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [Fact]
        public void FromAsync_ActionWithCancel_Throw_Asynchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync(ct =>
                Task.Factory.StartNew(() => { throw ex; })
            );

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [Fact]
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

#if DESKTOPCLR
        [Fact]
        public void FromAsync_Action_Scheduler1()
        {
            var e = new ManualResetEvent(false);
            var t = default(int);

            var tcs = new TaskCompletionSource<int>();

            var xs = Observable.FromAsync(() => (Task)tcs.Task, Scheduler.Immediate);
            xs.Subscribe(res =>
            {
                t = Thread.CurrentThread.ManagedThreadId;
                e.Set();
            });

            tcs.SetResult(42);

            e.WaitOne();

            Assert.Equal(Thread.CurrentThread.ManagedThreadId, t);
        }

        [Fact]
        public void FromAsync_Action_Scheduler2()
        {
            var e = new ManualResetEvent(false);
            var t = default(int);

            var tcs = new TaskCompletionSource<int>();

            var xs = Observable.FromAsync(ct => (Task)tcs.Task, Scheduler.Immediate);
            xs.Subscribe(res =>
            {
                t = Thread.CurrentThread.ManagedThreadId;
                e.Set();
            });

            tcs.SetResult(42);

            e.WaitOne();

            Assert.Equal(Thread.CurrentThread.ManagedThreadId, t);
        }
#endif

#endregion

#endif

#endregion

#region ToAsync

        [Fact]
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

        [Fact]
        public void ToAsync0()
        {
            Assert.True(Observable.ToAsync<int>(() => 0)().ToEnumerable().SequenceEqual(new[] { 0 }));
            Assert.True(Observable.ToAsync<int>(() => 0, Scheduler.Default)().ToEnumerable().SequenceEqual(new[] { 0 }));
        }

        [Fact]
        public void ToAsync1()
        {
            Assert.True(Observable.ToAsync<int, int>(a => a)(1).ToEnumerable().SequenceEqual(new[] { 1 }));
            Assert.True(Observable.ToAsync<int, int>(a => a, Scheduler.Default)(1).ToEnumerable().SequenceEqual(new[] { 1 }));
        }

        [Fact]
        public void ToAsync2()
        {
            Assert.True(Observable.ToAsync<int, int, int>((a, b) => a + b)(1, 2).ToEnumerable().SequenceEqual(new[] { 1 + 2 }));
            Assert.True(Observable.ToAsync<int, int, int>((a, b) => a + b, Scheduler.Default)(1, 2).ToEnumerable().SequenceEqual(new[] { 1 + 2 }));
        }

        [Fact]
        public void ToAsync3()
        {
            Assert.True(Observable.ToAsync<int, int, int, int>((a, b, c) => a + b + c)(1, 2, 3).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 }));
            Assert.True(Observable.ToAsync<int, int, int, int>((a, b, c) => a + b + c, Scheduler.Default)(1, 2, 3).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 }));
        }

        [Fact]
        public void ToAsync4()
        {
            Assert.True(Observable.ToAsync<int, int, int, int, int>((a, b, c, d) => a + b + c + d)(1, 2, 3, 4).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 }));
            Assert.True(Observable.ToAsync<int, int, int, int, int>((a, b, c, d) => a + b + c + d, Scheduler.Default)(1, 2, 3, 4).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 }));
        }

#if !NO_LARGEARITY

        [Fact]
        public void ToAsync5()
        {
            Assert.True(Observable.ToAsync<int, int, int, int, int, int>((a, b, c, d, e) => a + b + c + d + e)(1, 2, 3, 4, 5).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int>((a, b, c, d, e) => a + b + c + d + e, Scheduler.Default)(1, 2, 3, 4, 5).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 }));
        }

        [Fact]
        public void ToAsync6()
        {
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int>((a, b, c, d, e, f) => a + b + c + d + e + f)(1, 2, 3, 4, 5, 6).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int>((a, b, c, d, e, f) => a + b + c + d + e + f, Scheduler.Default)(1, 2, 3, 4, 5, 6).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 }));
        }

        [Fact]
        public void ToAsync7()
        {
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g) => a + b + c + d + e + f + g)(1, 2, 3, 4, 5, 6, 7).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g) => a + b + c + d + e + f + g, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 }));
        }

        [Fact]
        public void ToAsync8()
        {
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h)(1, 2, 3, 4, 5, 6, 7, 8).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 }));
        }

        [Fact]
        public void ToAsync9()
        {
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i) => a + b + c + d + e + f + g + h + i)(1, 2, 3, 4, 5, 6, 7, 8, 9).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i) => a + b + c + d + e + f + g + h + i, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 }));
        }

        [Fact]
        public void ToAsync10()
        {
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j) => a + b + c + d + e + f + g + h + i + j)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j) => a + b + c + d + e + f + g + h + i + j, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 }));
        }

        [Fact]
        public void ToAsync11()
        {
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k) => a + b + c + d + e + f + g + h + i + j + k)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k) => a + b + c + d + e + f + g + h + i + j + k, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 }));
        }

        [Fact]
        public void ToAsync12()
        {
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) => a + b + c + d + e + f + g + h + i + j + k + l)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) => a + b + c + d + e + f + g + h + i + j + k + l, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 }));
        }

        [Fact]
        public void ToAsync13()
        {
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) => a + b + c + d + e + f + g + h + i + j + k + l + m)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) => a + b + c + d + e + f + g + h + i + j + k + l + m, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 }));
        }

        [Fact]
        public void ToAsync14()
        {
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => a + b + c + d + e + f + g + h + i + j + k + l + m + n)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => a + b + c + d + e + f + g + h + i + j + k + l + m + n, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 }));
        }

        [Fact]
        public void ToAsync15()
        {
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => a + b + c + d + e + f + g + h + i + j + k + l + m + n + o)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 + 15 }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => a + b + c + d + e + f + g + h + i + j + k + l + m + n + o, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 + 15 }));
        }

        [Fact]
        public void ToAsync16()
        {
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => a + b + c + d + e + f + g + h + i + j + k + l + m + n + o + p)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 + 15 + 16 }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => a + b + c + d + e + f + g + h + i + j + k + l + m + n + o + p, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 + 15 + 16 }));
        }
#endif

        [Fact]
        public void ToAsync_Error0()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int>(() => { throw ex; })().Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error1()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int>(a => { throw ex; })(1).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error2()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int>((a, b) => { throw ex; })(1, 2).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error3()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int>((a, b, c) => { throw ex; })(1, 2, 3).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error4()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int>((a, b, c, d) => { throw ex; })(1, 2, 3, 4).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

#if !NO_LARGEARITY

        [Fact]
        public void ToAsync_Error5()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int>((a, b, c, d, e) => { throw ex; })(1, 2, 3, 4, 5).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error6()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int>((a, b, c, d, e, f) => { throw ex; })(1, 2, 3, 4, 5, 6).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error7()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g) => { throw ex; })(1, 2, 3, 4, 5, 6, 7).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error8()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h) => { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error9()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i) => { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error10()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j) => { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error11()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k) => { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error12()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) => { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error13()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) => { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error14()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error15()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error16()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }
#endif

        [Fact]
        public void ToAsyncAction0()
        {
            bool hasRun = false;
            Assert.True(Observable.ToAsync(() => { hasRun = true; })().ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync(() => { hasRun = true; }, Scheduler.Default)().ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError0()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync(() => { throw ex; })().Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction1()
        {
            bool hasRun = false;
            Assert.True(Observable.ToAsync<int>(a => { Assert.Equal(1, a); hasRun = true; })(1).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int>(a => { Assert.Equal(1, a); hasRun = true; }, Scheduler.Default)(1).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError1()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int>(a => { Assert.Equal(1, a); throw ex; })(1).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction2()
        {
            bool hasRun = false;
            Assert.True(Observable.ToAsync<int, int>((a, b) => { Assert.Equal(1, a); Assert.Equal(2, b); hasRun = true; })(1, 2).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int, int>((a, b) => { Assert.Equal(1, a); Assert.Equal(2, b); hasRun = true; }, Scheduler.Default)(1, 2).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError2()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int>((a, b) => { Assert.Equal(1, a); Assert.Equal(2, b); throw ex; })(1, 2).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction3()
        {
            bool hasRun = false;
            Assert.True(Observable.ToAsync<int, int, int>((a, b, c) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); hasRun = true; })(1, 2, 3).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int, int, int>((a, b, c) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); hasRun = true; }, Scheduler.Default)(1, 2, 3).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError3()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int>((a, b, c) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); throw ex; })(1, 2, 3).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction4()
        {
            bool hasRun = false;
            Assert.True(Observable.ToAsync<int, int, int, int>((a, b, c, d) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); hasRun = true; })(1, 2, 3, 4).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int, int, int, int>((a, b, c, d) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError4()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int>((a, b, c, d) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); throw ex; })(1, 2, 3, 4).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

#if !NO_LARGEARITY

        [Fact]
        public void ToAsyncAction5()
        {
            bool hasRun = false;
            Assert.True(Observable.ToAsync<int, int, int, int, int>((a, b, c, d, e) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); hasRun = true; })(1, 2, 3, 4, 5).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int, int, int, int, int>((a, b, c, d, e) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError5()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int>((a, b, c, d, e) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); throw ex; })(1, 2, 3, 4, 5).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction6()
        {
            bool hasRun = false;
            Assert.True(Observable.ToAsync<int, int, int, int, int, int>((a, b, c, d, e, f) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); hasRun = true; })(1, 2, 3, 4, 5, 6).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int>((a, b, c, d, e, f) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError6()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int>((a, b, c, d, e, f) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); throw ex; })(1, 2, 3, 4, 5, 6).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction7()
        {
            bool hasRun = false;
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int>((a, b, c, d, e, f, g) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); hasRun = true; })(1, 2, 3, 4, 5, 6, 7).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int>((a, b, c, d, e, f, g) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError7()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int>((a, b, c, d, e, f, g) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); throw ex; })(1, 2, 3, 4, 5, 6, 7).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction8()
        {
            bool hasRun = false;
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError8()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction9()
        {
            bool hasRun = false;
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8, 9).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError9()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction10()
        {
            bool hasRun = false;
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError10()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction11()
        {
            bool hasRun = false;
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError11()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction12()
        {
            bool hasRun = false;
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); Assert.Equal(12, l); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); Assert.Equal(12, l); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError12()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); Assert.Equal(12, l); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction13()
        {
            bool hasRun = false;
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); Assert.Equal(12, l); Assert.Equal(13, m); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); Assert.Equal(12, l); Assert.Equal(13, m); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError13()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); Assert.Equal(12, l); Assert.Equal(13, m); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction14()
        {
            bool hasRun = false;
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); Assert.Equal(12, l); Assert.Equal(13, m); Assert.Equal(14, n); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); Assert.Equal(12, l); Assert.Equal(13, m); Assert.Equal(14, n); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError14()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); Assert.Equal(12, l); Assert.Equal(13, m); Assert.Equal(14, n); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction15()
        {
            bool hasRun = false;
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); Assert.Equal(12, l); Assert.Equal(13, m); Assert.Equal(14, n); Assert.Equal(15, o); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); Assert.Equal(12, l); Assert.Equal(13, m); Assert.Equal(14, n); Assert.Equal(15, o); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError15()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); Assert.Equal(12, l); Assert.Equal(13, m); Assert.Equal(14, n); Assert.Equal(15, o); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction16()
        {
            bool hasRun = false;
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); Assert.Equal(12, l); Assert.Equal(13, m); Assert.Equal(14, n); Assert.Equal(15, o); Assert.Equal(16, p); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); Assert.Equal(12, l); Assert.Equal(13, m); Assert.Equal(14, n); Assert.Equal(15, o); Assert.Equal(16, p); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError16()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); Assert.Equal(12, l); Assert.Equal(13, m); Assert.Equal(14, n); Assert.Equal(15, o); Assert.Equal(16, p); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }
#endif

#endregion
    }
}
