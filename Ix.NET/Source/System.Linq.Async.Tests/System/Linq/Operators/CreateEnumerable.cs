// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using Xunit;

namespace Tests
{
    public class CreateEnumerable : AsyncEnumerableTests
    {
        [Fact]
        public void CreateEnumerable_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Create<int>(default));
        }
    }
}
