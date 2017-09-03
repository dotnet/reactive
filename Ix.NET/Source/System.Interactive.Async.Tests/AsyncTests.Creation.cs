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

        [Fact]
        public void Return()
        {
            var xs = AsyncEnumerable.Return(42);
            HasNext(xs.GetAsyncEnumerator(), 42);
        }

        [Fact]
        public void Throw_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Throw<int>(null));
        }

        [Fact]
        public void Throw()
        {
            var ex = new Exception("Bang");
            var xs = AsyncEnumerable.Throw<int>(ex);

            var e = xs.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).InnerExceptions.Single() == ex);
            AssertThrows<InvalidOperationException>(() => Nop(e.Current));
        }

        private void Nop(object o)
        {
        }
    }
}
