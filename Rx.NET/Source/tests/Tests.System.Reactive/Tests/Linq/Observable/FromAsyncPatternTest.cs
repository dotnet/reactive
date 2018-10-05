// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
#pragma warning disable IDE0039 // Use local function
    public class FromAsyncPatternTest : ReactiveTest
    {
        private readonly Task<int> _doneTask;

        public FromAsyncPatternTest()
        {
            var tcs = new TaskCompletionSource<int>();
            tcs.SetResult(42);
            _doneTask = tcs.Task;
        }

        [Fact]
        public void FromAsyncPattern_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern(null, iar => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern(null, iar => 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int>(null, iar => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int>(null, iar => 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int>(null, iar => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int>(null, iar => 0));
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

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern((cb, o) => null, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int>((cb, o) => null, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int>((a, cb, o) => null, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int>((a, cb, o) => null, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int>((a, b, cb, o) => null, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int>((a, b, cb, o) => null, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int>((a, b, c, cb, o) => null, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int>((a, b, c, cb, o) => null, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int>((a, b, c, d, cb, o) => null, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int>((a, b, c, d, cb, o) => null, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int>((a, b, c, d, e, cb, o) => null, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int>((a, b, c, d, e, cb, o) => null, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int>((a, b, c, d, e, f, cb, o) => null, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int>((a, b, c, d, e, f, cb, o) => null, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int>((a, b, c, d, e, f, g, cb, o) => null, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, cb, o) => null, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, cb, o) => null, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, cb, o) => null, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, cb, o) => null, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, cb, o) => null, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, cb, o) => null, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, cb, o) => null, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, cb, o) => null, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, cb, o) => null, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, cb, o) => null, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, cb, o) => null, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, cb, o) => null, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, cb, o) => null, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, cb, o) => null, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsyncPattern<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, cb, o) => null, default));
        }

        [Fact]
        public void FromAsyncPattern0()
        {
            var x = new Result();

            Func<AsyncCallback, object, IAsyncResult> begin = (cb, _) => { cb(x); return x; };
            Func<IAsyncResult, int> end = iar => { Assert.Same(x, iar); return 1; };

            var res = Observable.FromAsyncPattern(begin, end)().Materialize().ToEnumerable().ToArray();
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext(1), Notification.CreateOnCompleted<int>() }));
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
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext(1), Notification.CreateOnCompleted<int>() }));
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
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext(1), Notification.CreateOnCompleted<int>() }));
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
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext(1), Notification.CreateOnCompleted<int>() }));
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
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext(1), Notification.CreateOnCompleted<int>() }));
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
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext(1), Notification.CreateOnCompleted<int>() }));
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
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext(1), Notification.CreateOnCompleted<int>() }));
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
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext(1), Notification.CreateOnCompleted<int>() }));
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
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext(1), Notification.CreateOnCompleted<int>() }));
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
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext(1), Notification.CreateOnCompleted<int>() }));
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
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext(1), Notification.CreateOnCompleted<int>() }));
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
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext(1), Notification.CreateOnCompleted<int>() }));
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
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext(1), Notification.CreateOnCompleted<int>() }));
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
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext(1), Notification.CreateOnCompleted<int>() }));
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
            Assert.True(res.SequenceEqual(new Notification<int>[] { Notification.CreateOnNext(1), Notification.CreateOnCompleted<int>() }));
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

        private class Result : IAsyncResult
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

    }
#pragma warning restore IDE0039 // Use local function
}
