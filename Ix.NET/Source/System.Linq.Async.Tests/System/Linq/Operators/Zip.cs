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
    public class Zip : AsyncEnumerableTests
    {
        [Fact]
        public void Zip_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Zip<int, int, int>(default(IAsyncEnumerable<int>), Return42, (x, y) => x + y));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Zip<int, int, int>(Return42, default(IAsyncEnumerable<int>), (x, y) => x + y));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Zip<int, int, int>(Return42, Return42, default(Func<int, int, int>)));
        }

        [Fact]
        public void Zip1()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();
            var res = xs.Zip(ys, (x, y) => x * y);

            var e = res.GetAsyncEnumerator();
            HasNext(e, 1 * 4);
            HasNext(e, 2 * 5);
            HasNext(e, 3 * 6);
            NoNext(e);
        }

        [Fact]
        public void Zip2()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6, 7 }.ToAsyncEnumerable();
            var res = xs.Zip(ys, (x, y) => x * y);

            var e = res.GetAsyncEnumerator();
            HasNext(e, 1 * 4);
            HasNext(e, 2 * 5);
            HasNext(e, 3 * 6);
            NoNext(e);
        }

        [Fact]
        public void Zip3()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();
            var res = xs.Zip(ys, (x, y) => x * y);

            var e = res.GetAsyncEnumerator();
            HasNext(e, 1 * 4);
            HasNext(e, 2 * 5);
            HasNext(e, 3 * 6);
            NoNext(e);
        }

        [Fact]
        public void Zip4()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = Throw<int>(ex);
            var res = xs.Zip(ys, (x, y) => x * y);

            var e = res.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Zip5()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex);
            var ys = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.Zip(ys, (x, y) => x * y);

            var e = res.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Zip6()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.Zip(ys, (x, y) => { if (x > 0) throw ex; return x * y; });

            var e = res.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public async Task Zip7()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();
            var res = xs.Zip(ys, (x, y) => x * y);

            await SequenceIdentity(res);
        }
    }
}
