// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using System.Reactive.Linq;
using System.Reactive.Linq.InternalObservable;

namespace Tests.System.Reactive.Tests.Linq.InternalObservable
{
    public class InternalTakeTest : ReactiveTest
    {
        [Fact]
        public void InternalTake_Basic()
        {
            var actual = new InternalTake<int>(new InternalRange(1, 5), 3)
                .ToList().First();

            Assert.Equal(new List<int>() { 1, 2, 3 }, actual);
        }

        [Fact]
        public void InternalTake_Take_Twice()
        {
            var actual = new InternalTake<int>(new InternalTake<int>(new InternalRange(1, 5), 4), 2)
                .ToList().First();

            Assert.Equal(new List<int>() { 1, 2 }, actual);
        }
    }
}
