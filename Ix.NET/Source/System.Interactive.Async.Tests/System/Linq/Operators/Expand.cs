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
    public class Expand : AsyncEnumerableExTests
    {
        [Fact]
        public void Expand_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Expand(default(IAsyncEnumerable<int>), x => default(IAsyncEnumerable<int>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Expand(Return42, default(Func<int, IAsyncEnumerable<int>>)));
        }

        [Fact]
        public void Expand1()
        {
            var xs = new[] { 2, 3 }.ToAsyncEnumerable().Expand(x => AsyncEnumerable.Return(x - 1).Repeat(x - 1));

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 2);
            HasNext(e, 1);
            HasNext(e, 1);
            NoNext(e);
        }

        [Fact]
        public void Expand2()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 2, 3 }.ToAsyncEnumerable().Expand(new Func<int, IAsyncEnumerable<int>>(x => { throw ex; }));

            var e = xs.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Expand3()
        {
            var xs = new[] { 2, 3 }.ToAsyncEnumerable().Expand(x => default(IAsyncEnumerable<int>));

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 2);
            HasNext(e, 3);
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is NullReferenceException);
        }

        [Fact]
        public async Task Expand4()
        {
            var xs = new[] { 2, 3 }.ToAsyncEnumerable().Expand(x => AsyncEnumerable.Return(x - 1).Repeat(x - 1));

            await SequenceIdentity(xs);
        }
    }
}
