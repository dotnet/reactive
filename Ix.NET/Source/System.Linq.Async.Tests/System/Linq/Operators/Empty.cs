// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Empty : AsyncEnumerableTests
    {
        [Fact]
        public void Empty1()
        {
            var xs = AsyncEnumerable.Empty<int>();
            NoNext(xs.GetAsyncEnumerator());
        }

        [Fact]
        public void Empty2()
        {
            var xs = AsyncEnumerable.Empty<int>();

            var e = xs.GetAsyncEnumerator();
            Assert.False(e.MoveNextAsync().Result);
        }

        private void Nop(object o)
        {
        }
    }
}
