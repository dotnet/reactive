// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace Tests
{
    public class CreateEnumerable : AsyncEnumerableTests
    {
        [Fact]
        public void CreateEnumerable_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.CreateEnumerable<int>(default(Func<CancellationToken, IAsyncEnumerator<int>>)));
        }
    }
}
