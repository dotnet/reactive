// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using Xunit;

namespace ReactiveTests.Tests
{

    public class UnitTest
    {
        [Fact]
        public void Unit()
        {
            var u1 = new Unit();
            var u2 = new Unit();
            Assert.True(u1.Equals(u2));
            Assert.False(u1.Equals(""));
            Assert.False(u1.Equals(null));
            Assert.True(u1 == u2);
            Assert.False(u1 != u2);
            Assert.Equal(0, u1.GetHashCode());
            Assert.Equal("()", u1.ToString());
        }
    }
}
