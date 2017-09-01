// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class SkipWhile : AsyncEnumerableTests
    {
        [Fact]
        public void SkipWhile_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SkipWhile<int>(null, x => true));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SkipWhile<int>(null, (x, i) => true));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SkipWhile<int>(AsyncEnumerable.Return(42), default(Func<int, bool>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SkipWhile<int>(AsyncEnumerable.Return(42), default(Func<int, int, bool>)));
        }

        [Fact]
        public void SkipWhile1()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile(x => x < 3);

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void SkipWhile2()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile(x => false);

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void SkipWhile3()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile(x => true);

            var e = ys.GetAsyncEnumerator();
            NoNext(e);
        }

        [Fact]
        public void SkipWhile4()
        {
            var xs = new[] { 1, 2, 3, 4, 3, 2, 1 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile(x => x < 3);

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 3);
            HasNext(e, 4);
            HasNext(e, 3);
            HasNext(e, 2);
            HasNext(e, 1);
            NoNext(e);
        }

        [Fact]
        public void SkipWhile5()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile(new Func<int, bool>(x => { throw ex; }));

            var e = ys.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void SkipWhile6()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile((x, i) => i < 2);

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void SkipWhile7()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile((x, i) => false);

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void SkipWhile8()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile((x, i) => true);

            var e = ys.GetAsyncEnumerator();
            NoNext(e);
        }

        [Fact]
        public void SkipWhile9()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile(new Func<int, int, bool>((x, i) => { throw ex; }));

            var e = ys.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public async Task SkipWhile10()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile(x => x < 3);

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task SkipWhile11()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile((x, i) => false);

            await SequenceIdentity(ys);
        }
    }
}
