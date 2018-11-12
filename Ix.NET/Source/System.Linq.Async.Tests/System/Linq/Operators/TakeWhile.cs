// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class TakeWhile : AsyncEnumerableTests
    {
        [Fact]
        public void TakeWhile_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.TakeWhile<int>(default, x => true));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.TakeWhile<int>(default, (x, i) => true));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.TakeWhile(Return42, default(Func<int, bool>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.TakeWhile(Return42, default(Func<int, int, bool>)));
        }

        [Fact]
        public void TakeWhile1()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile(x => x < 3);

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            NoNext(e);
        }

        [Fact]
        public void TakeWhile2()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile(x => false);

            var e = ys.GetAsyncEnumerator();
            NoNext(e);
        }

        [Fact]
        public void TakeWhile3()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile(x => true);

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void TakeWhile4()
        {
            var xs = new[] { 1, 2, 3, 4, 3, 2, 1 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile(x => x < 3);

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            NoNext(e);
        }

        [Fact]
        public void TakeWhile5()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile(new Func<int, bool>(x => { throw ex; }));

            var e = ys.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
        }

        [Fact]
        public void TakeWhile6()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile((x, i) => i < 2);

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            NoNext(e);
        }

        [Fact]
        public void TakeWhile7()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile((x, i) => false);

            var e = ys.GetAsyncEnumerator();
            NoNext(e);
        }

        [Fact]
        public void TakeWhile8()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile((x, i) => true);

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void TakeWhile9()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile(new Func<int, int, bool>((x, i) => { throw ex; }));

            var e = ys.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
        }


        [Fact]
        public async Task TakeWhile10()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile(x => x < 3);

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task TakeWhile11()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile((x, i) => i < 2);

            await SequenceIdentity(ys);
        }
    }
}
