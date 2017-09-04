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
    public class IgnoreElements : AsyncEnumerableExTests
    {
        [Fact]
        public void IgnoreElements_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.IgnoreElements(default(IAsyncEnumerable<int>)));
        }

        [Fact]
        public void IgnoreElements1()
        {
            var xs = AsyncEnumerable.Empty<int>().IgnoreElements();

            var e = xs.GetAsyncEnumerator();
            NoNext(e);

            AssertThrows<InvalidOperationException>(() => { var ignored = e.Current; });
        }

        [Fact]
        public void IgnoreElements2()
        {
            var xs = Return42.IgnoreElements();

            var e = xs.GetAsyncEnumerator();
            NoNext(e);

            AssertThrows<InvalidOperationException>(() => { var ignored = e.Current; });
        }

        [Fact]
        public void IgnoreElements3()
        {
            var xs = AsyncEnumerable.Range(0, 10).IgnoreElements();

            var e = xs.GetAsyncEnumerator();
            NoNext(e);

            AssertThrows<InvalidOperationException>(() => { var ignored = e.Current; });
        }

        [Fact]
        public void IgnoreElements4()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Throw<int>(ex).IgnoreElements();

            var e = xs.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public async Task IgnoreElements5()
        {
            var xs = AsyncEnumerable.Range(0, 10).IgnoreElements();

            await SequenceIdentity(xs);
        }
    }
}
