// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using Xunit;

namespace Tests
{
    public class ForEach : Tests
    {
        [Fact]
        public void ForEach_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.ForEach<int>(null, x => { }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.ForEach<int>(new[] { 1 }, default(Action<int>)));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.ForEach<int>(null, (x, i) => { }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.ForEach<int>(new[] { 1 }, default(Action<int, int>)));
        }

        [Fact]
        public void ForEach1()
        {
            var n = 0;
            Enumerable.Range(5, 3).ForEach(x => n += x);
            Assert.Equal(5 + 6 + 7, n);
        }

        [Fact]
        public void ForEach2()
        {
            var n = 0;
            Enumerable.Range(5, 3).ForEach((x, i) => n += x * i);
            Assert.Equal(5 * 0 + 6 * 1 + 7 * 2, n);
        }
    }
}
