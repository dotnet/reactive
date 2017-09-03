// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using Xunit;

namespace Tests
{
    public class AsyncEnumeratorTests
    {
        [Fact]
        public void MoveNextExtension_Null()
        {
            var en = default(IAsyncEnumerator<int>);

            Assert.ThrowsAsync<ArgumentNullException>(() => en.MoveNextAsync());
        }
    }
}
