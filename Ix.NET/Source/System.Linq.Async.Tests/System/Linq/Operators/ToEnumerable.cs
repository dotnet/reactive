// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using Xunit;

namespace Tests
{
    public class ToEnumerable : AsyncEnumerableTests
    {
        [Fact]
        public void ToEnumerable_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.ToEnumerable<int>(null));
        }

        [Fact]
        public void ToEnumerable1()
        {
            var xs = Return42.ToEnumerable();
            Assert.True(xs.SequenceEqual(new[] { 42 }));
        }

        [Fact]
        public void ToEnumerable2()
        {
            var xs = AsyncEnumerable.Empty<int>().ToEnumerable();
            Assert.True(xs.SequenceEqual(new int[0]));
        }

        [Fact]
        public void ToEnumerable3()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex).ToEnumerable();
            Assert.Throws<Exception>(() => xs.GetEnumerator().MoveNext());
        }
    }
}
