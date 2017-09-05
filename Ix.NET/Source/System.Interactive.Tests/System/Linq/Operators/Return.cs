// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Linq;
using Xunit;

namespace Tests
{
    public class Return : Tests
    {
        [Fact]
        public void Return1()
        {
            Assert.Equal(42, EnumerableEx.Return(42).Single());
        }
    }
}
