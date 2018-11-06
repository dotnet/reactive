// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Throw : AsyncEnumerableTests
    {
        [Fact]
        public void Throw_Null()
        {
            AssertThrows<ArgumentNullException>(() => Throw<int>(default));
        }

        [Fact]
        public void Throw1()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex);

            var e = xs.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).InnerExceptions.Single() == ex);
            AssertThrows<InvalidOperationException>(() => Nop(e.Current));
        }

        private void Nop(object o)
        {
        }
    }
}
