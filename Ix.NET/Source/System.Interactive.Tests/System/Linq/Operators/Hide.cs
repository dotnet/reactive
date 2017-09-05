// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Hide : Tests
    {
        [Fact]
        public void Hide_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Hide<int>(null));
        }

        [Fact]
        public void Hide1()
        {
            var xs = new List<int> { 1, 2, 3 };
            var ys = xs.Hide();
            Assert.False(ys is List<int>);
            Assert.True(xs.SequenceEqual(ys));
        }
    }
}
