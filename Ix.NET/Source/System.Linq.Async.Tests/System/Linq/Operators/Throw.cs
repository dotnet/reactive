// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class Throw : AsyncEnumerableTests
    {
        [Fact]
        public void Throw_Null()
        {
            Assert.Throws<ArgumentNullException>(() => Throw<int>(default));
        }

        [Fact]
        public async Task Throw_Simple()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex);

            var e = xs.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }
    }
}
