// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Cast : AsyncEnumerableTests
    {
        [Fact]
        public void Cast_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Cast<int>(default(IAsyncEnumerable<object>)));
        }

        [Fact]
        public void Cast1()
        {
            var xs = new object[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Cast<int>();

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void Cast2()
        {
            var xs = new[] { new EventArgs(), new EventArgs(), new EventArgs() }.ToAsyncEnumerable();
            var ys = xs.Cast<EventArgs>();

            Assert.Same(xs, ys);
        }
    }
}
