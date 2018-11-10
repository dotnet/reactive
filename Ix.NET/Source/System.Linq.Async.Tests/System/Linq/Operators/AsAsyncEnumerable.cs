// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Linq;
using Xunit;

namespace Tests
{
    public class AsAsyncEnumerable : AsyncEnumerableTests
    {
        [Fact]
        public void AsAsyncEnumerable1()
        {
            var xs = Return42;
            var ys = xs.AsAsyncEnumerable();

            Assert.Same(xs, ys); // NB: Consistent with LINQ to Objects behavior.
        }
    }
}
