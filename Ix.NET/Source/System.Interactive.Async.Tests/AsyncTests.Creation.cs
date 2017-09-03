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
    public partial class AsyncTests
    {
        [Fact]
        public void Create_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.CreateEnumerable<int>(default(Func<IAsyncEnumerator<int>>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.CreateEnumerator<int>(null, () => 3, () => Task.FromResult(true)));
        }

        [Fact]
        public void Create_Iterator_Throws()
        {
            var iter = AsyncEnumerable.CreateEnumerator<int>(() => Task.FromResult(true), () => 3, () => Task.FromResult(true));

            var enu = (IAsyncEnumerable<int>)iter;

            AssertThrows<NotSupportedException>(() => enu.GetAsyncEnumerator());
        }
    }
}
