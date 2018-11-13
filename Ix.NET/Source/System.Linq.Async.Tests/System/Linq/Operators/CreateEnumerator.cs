// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class CreateEnumerator : AsyncEnumerableTests
    {
        [Fact]
        public void CreateEnumerator_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.CreateEnumerator(default, () => 3, () => new ValueTask()));
        }

        [Fact]
        public void CreateEnumerator_Throws()
        {
            var iter = AsyncEnumerable.CreateEnumerator(() => new ValueTask<bool>(false), () => 3, () => new ValueTask());

            var enu = (IAsyncEnumerable<int>)iter;

            Assert.Throws<NotSupportedException>(() => enu.GetAsyncEnumerator());
        }
    }
}
