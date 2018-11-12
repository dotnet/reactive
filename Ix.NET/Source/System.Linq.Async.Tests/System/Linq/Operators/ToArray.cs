// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class ToArray : AsyncEnumerableTests
    {
        [Fact]
        public async Task ToArray_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToArray<int>(default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToArray<int>(default, CancellationToken.None));
        }

        [Fact]
        public void ToArray1()
        {
            var xs = new[] { 42, 25, 39 };
            var res = xs.ToAsyncEnumerable().ToArray();
            Assert.True(res.Result.SequenceEqual(xs));
        }

        [Fact]
        public void ToArray2()
        {
            var xs = AsyncEnumerable.Empty<int>();
            var res = xs.ToArray();
            Assert.True(res.Result.Length == 0);
        }

        [Fact]
        public void ToArray3()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).ToArray();
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
        }

        [Fact]
        public async Task ToArray4()
        {
            var xs = await AsyncEnumerable.Range(5, 50).Take(10).ToArray();
            var ex = new[] { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };

            Assert.True(ex.SequenceEqual(xs));
        }

        [Fact]
        public async Task ToArray5()
        {
            var res = new[] { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
            var xs = new HashSet<int>(res);

            var arr = await xs.ToAsyncEnumerable().ToArray();

            Assert.True(res.SequenceEqual(arr));
        }
    }
}
