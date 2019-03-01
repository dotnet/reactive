// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using Xunit;

namespace Tests
{
    public class IgnoreElements : Tests
    {
        [Fact]
        public void IgnoreElements_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.IgnoreElements<int>(null));
        }

        [Fact]
        public void IgnoreElements1()
        {
            var n = 0;
            Enumerable.Range(0, 10).Do(_ => n++).IgnoreElements().Take(5).ForEach(_ => { });
            Assert.Equal(10, n);
        }
    }
}
