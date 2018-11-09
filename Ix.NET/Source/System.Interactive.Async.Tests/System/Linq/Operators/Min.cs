﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class Min : AsyncEnumerableExTests
    {
        [Fact]
        public async Task Min_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.Min(default, Comparer<DateTime>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.Min(AsyncEnumerable.Empty<DateTime>(), default));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.Min(default, Comparer<DateTime>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.Min(AsyncEnumerable.Empty<DateTime>(), default, CancellationToken.None));
        }
    }
}