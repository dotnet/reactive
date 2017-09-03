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
    public class Scan : AsyncEnumerableExTests
    {
        [Fact]
        public void Scan_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Scan(default(IAsyncEnumerable<int>), 3, (x, y) => x + y));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Scan(AsyncEnumerable.Return(42), 3, default(Func<int, int, int>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Scan(default(IAsyncEnumerable<int>), (x, y) => x + y));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Scan(AsyncEnumerable.Return(42), default(Func<int, int, int>)));
        }

        [Fact]
        public void Scan1()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Scan(8, (x, y) => x + y);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 9);
            HasNext(e, 11);
            HasNext(e, 14);
            NoNext(e);
        }

        [Fact]
        public void Scan2()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Scan((x, y) => x + y);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 3);
            HasNext(e, 6);
            NoNext(e);
        }

        [Fact]
        public void Scan3()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Scan(8, new Func<int, int, int>((x, y) => { throw ex; }));

            var e = xs.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Scan4()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Scan(new Func<int, int, int>((x, y) => { throw ex; }));

            var e = xs.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public async Task Scan5()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Scan(8, (x, y) => x + y);

            await SequenceIdentity(xs);
        }

        [Fact]
        public async Task Scan6()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Scan((x, y) => x + y);

            await SequenceIdentity(xs);
        }
    }
}
