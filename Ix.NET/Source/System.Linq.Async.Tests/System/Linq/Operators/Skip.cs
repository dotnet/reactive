// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class Skip : AsyncEnumerableTests
    {
        [Fact]
        public void Skip_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Skip<int>(null, 5));
        }

        //[Fact]
        //public void Skip0()
        //{
        //    var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
        //    var ys = xs.Skip(-2);

        //    var e = ys.GetEnumerator();
        //    NoNext(e);
        //}

        [Fact]
        public void Skip1()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Skip(2);

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void Skip2()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Skip(10);

            var e = ys.GetAsyncEnumerator();
            NoNext(e);
        }

        [Fact]
        public void Skip3()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Skip(0);

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void Skip4()
        {
            var ex = new Exception("Bang");
            var xs = AsyncEnumerable.Throw<int>(ex);
            var ys = xs.Skip(2);

            var e = ys.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public async Task Skip5()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Skip(2);

            await SequenceIdentity(ys);
        }
    }
}
