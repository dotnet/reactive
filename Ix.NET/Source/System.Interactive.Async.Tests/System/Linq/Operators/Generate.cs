// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class Generate : AsyncEnumerableExTests
    {
        [Fact]
        public void Generate_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Generate<int, int>(0, default, x => x, x => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Generate<int, int>(0, x => true, default, x => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Generate<int, int>(0, x => true, x => x, default));
        }

        [Fact]
        public async Task Generate1()
        {
            var xs = AsyncEnumerableEx.Generate(0, x => x < 5, x => x + 1, x => x * x);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 0);
            HasNext(e, 1);
            HasNext(e, 4);
            HasNext(e, 9);
            HasNext(e, 16);
            NoNext(e);
            await e.DisposeAsync();
        }

        [Fact]
        public void Generate2()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerableEx.Generate(0, x => { throw ex; }, x => x + 1, x => x * x);

            var e = xs.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Generate3()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerableEx.Generate(0, x => true, x => x + 1, x => { if (x == 1) throw ex; return x * x; });

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 0);
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Generate4()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerableEx.Generate(0, x => true, x => { throw ex; }, x => x * x);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 0);
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).InnerExceptions.Single() == ex);
        }

        [Fact]
        public async Task Generate5()
        {
            var xs = AsyncEnumerableEx.Generate(0, x => x < 5, x => x + 1, x => x * x);

            await SequenceIdentity(xs);
        }
    }
}
