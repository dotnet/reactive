// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using Xunit;

namespace Tests
{
    public class StartWith : Tests
    {
        [Fact]
        public void StartWith_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.StartWith<int>(null, 5));
        }

        [Fact]
        public void StartWith1()
        {
            var e = Enumerable.Range(1, 5);
            var r = e.StartWith(0).ToList();
            Assert.True(Enumerable.SequenceEqual(r, Enumerable.Range(0, 6)));
        }

        [Fact]
        public void StartWith2()
        {
            var oops = false;
            var e = Enumerable.Range(1, 5).Do(_ => oops = true);
            var r = e.StartWith(0).Take(1).ToList();
            Assert.False(oops);
        }
    }
}
