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
    public class Where : AsyncEnumerableTests
    {
        [Fact]
        public void Where_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Where<int>(default(IAsyncEnumerable<int>), x => true));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Where<int>(default(IAsyncEnumerable<int>), (x, i) => true));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Where<int>(Return42, default(Func<int, bool>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Where<int>(Return42, default(Func<int, int, bool>)));
        }

        [Fact]
        public void Where1()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.Where(x => x % 2 == 0);
            var e = ys.GetAsyncEnumerator();
            HasNext(e, 8);
            HasNext(e, 4);
            HasNext(e, 6);
            HasNext(e, 2);
            HasNext(e, 0);
            NoNext(e);
        }

        [Fact]
        public void Where2()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.Where((x, i) => i % 2 == 0);

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 8);
            HasNext(e, 7);
            HasNext(e, 6);
            HasNext(e, 2);
            HasNext(e, 0);
            NoNext(e);
        }

        [Fact]
        public void Where3()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ex = new Exception("Bang");
            var ys = xs.Where(x => { if (x == 4) throw ex; return true; });

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 8);
            HasNext(e, 5);
            HasNext(e, 7);
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Where4()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ex = new Exception("Bang");
            var ys = xs.Where((x, i) => { if (i == 3) throw ex; return true; });

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 8);
            HasNext(e, 5);
            HasNext(e, 7);
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Where5()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex);
            var ys = xs.Where(x => true);

            var e = ys.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Where6()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex);

            var ys = xs.Where((x, i) => true);
            var e = ys.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }


        [Fact]
        public void Where7()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.Where(x => x % 2 == 0).Where(x => x > 5);
            var e = ys.GetAsyncEnumerator();
            HasNext(e, 8);
            HasNext(e, 6);
            NoNext(e);
        }

        [Fact]
        public async Task Where8()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.Where(x => x % 2 == 0);

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task Where9()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.Where((x, i) => i % 2 == 0);

            await SequenceIdentity(ys);
        }
    }
}
