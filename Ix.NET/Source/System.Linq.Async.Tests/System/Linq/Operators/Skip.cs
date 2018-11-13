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
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Skip<int>(default, 5));
        }

        //[Fact]
        //public void Skip0()
        //{
        //    var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
        //    var ys = xs.Skip(-2);

        //    var e = ys.GetEnumerator();
        //    await NoNextAsync(e);
        //}

        [Fact]
        public async Task Skip1()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Skip(2);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Skip2()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Skip(10);

            var e = ys.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Skip3()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Skip(0);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Skip4Async()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex);
            var ys = xs.Skip(2);

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
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
