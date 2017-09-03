// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class Repeat : AsyncEnumerableTests
    {
        [Fact]
        public void Repeat_Null()
        {
            AssertThrows<ArgumentOutOfRangeException>(() => AsyncEnumerable.Repeat(0, -1));
        }

        [Fact]
        public void Repeat1()
        {
            var xs = AsyncEnumerable.Repeat(2, 5);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 2);
            HasNext(e, 2);
            HasNext(e, 2);
            HasNext(e, 2);
            HasNext(e, 2);
            NoNext(e);
        }

        [Fact]
        public void Repeat2()
        {
            var xs = AsyncEnumerable.Repeat(2, 0);

            var e = xs.GetAsyncEnumerator();
            NoNext(e);
        }
    }
}
