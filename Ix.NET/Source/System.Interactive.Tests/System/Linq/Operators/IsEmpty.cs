// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using Xunit;

namespace Tests
{
    public class IsEmpty : Tests
    {
        [Fact]
        public void IsEmtpy_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.IsEmpty<int>(null));
        }

        [Fact]
        public void IsEmpty_Empty()
        {
            Assert.True(Enumerable.Empty<int>().IsEmpty());
        }

        [Fact]
        public void IsEmpty_NonEmpty()
        {
            Assert.False(new[] { 1 }.IsEmpty());
        }
    }
}
