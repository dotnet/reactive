// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

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
