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
    public class Take : AsyncEnumerableTests
    {
        [Fact]
        public void Take_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Take<int>(default, 5));
        }

        [Fact]
        public void Take0()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Take(-2);

            var e = ys.GetAsyncEnumerator();
            NoNext(e);
        }

        [Fact]
        public void Take1()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Take(2);

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            NoNext(e);
        }

        [Fact]
        public void Take2()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Take(10);

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void Take3()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Take(0);

            var e = ys.GetAsyncEnumerator();
            NoNext(e);
        }

        [Fact]
        public void Take4()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex);
            var ys = xs.Take(2);

            var e = ys.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
        }

        [Fact]
        public async Task Take5()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Take(2);

            await SequenceIdentity(ys);
        }
    }
}
