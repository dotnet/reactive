// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantTypeArgumentsOfMethod

namespace Tests
{
    public partial class AsyncTests
    {
        private const int WaitTimeoutMs = 5000;

        [Fact]
        public async Task Min_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.Min(default(IAsyncEnumerable<DateTime>), Comparer<DateTime>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.Min(AsyncEnumerable.Empty<DateTime>(), default(IComparer<DateTime>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.Min(default(IAsyncEnumerable<DateTime>), Comparer<DateTime>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.Min(AsyncEnumerable.Empty<DateTime>(), default(IComparer<DateTime>), CancellationToken.None));
        }

        [Fact]
        public async Task Max_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.Max(default(IAsyncEnumerable<DateTime>), Comparer<DateTime>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.Max(AsyncEnumerable.Empty<DateTime>(), default(IComparer<DateTime>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.Max(default(IAsyncEnumerable<DateTime>), Comparer<DateTime>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.Max(AsyncEnumerable.Empty<DateTime>(), default(IComparer<DateTime>), CancellationToken.None));
        }

        private sealed class Eq : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return EqualityComparer<int>.Default.Equals(Math.Abs(x), Math.Abs(y));
            }

            public int GetHashCode(int obj)
            {
                return EqualityComparer<int>.Default.GetHashCode(Math.Abs(obj));
            }
        }
    }
}
