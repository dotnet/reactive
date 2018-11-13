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
    public class Repeat : AsyncEnumerableExTests
    {
        [Fact]
        public async Task RepeatValue1()
        {
            var xs = AsyncEnumerableEx.Repeat(2);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 2);
            HasNext(e, 2);
            HasNext(e, 2);
            HasNext(e, 2);
            HasNext(e, 2);
            await e.DisposeAsync();
        }

        [Fact]
        public async Task RepeatValue2()
        {
            var xs = AsyncEnumerableEx.Repeat(2).Take(5);

            await SequenceIdentity(xs);
        }

        [Fact]
        public void RepeatSequence_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Repeat(default(IAsyncEnumerable<int>)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Repeat(default(IAsyncEnumerable<int>), 3));
            Assert.Throws<ArgumentOutOfRangeException>(() => AsyncEnumerableEx.Repeat(Return42, -1));
        }

        [Fact]
        public void RepeatSequence1()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Repeat();

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
        }

        [Fact]
        public void RepeatSequence2()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Repeat(3);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            NoNext(e);
        }

        [Fact]
        public void RepeatSequence3()
        {
            var i = 0;
            var xs = RepeatXs(() => i++).ToAsyncEnumerable().Repeat(3);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 1);
            HasNext(e, 2);
            NoNext(e);

            Assert.Equal(3, i);
        }

        [Fact]
        public void RepeatSequence4()
        {
            var i = 0;
            var xs = RepeatXs(() => i++).ToAsyncEnumerable().Repeat(0);

            var e = xs.GetAsyncEnumerator();

            NoNext(e);
        }

        [Fact]
        public async Task RepeatSequence5()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Repeat(3);

            await SequenceIdentity(xs);
        }

        [Fact]
        public void RepeatSequence6()
        {
            var xs = new FailRepeat().ToAsyncEnumerable().Repeat();

            var e = xs.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is NotImplementedException);
        }

        [Fact]
        public void RepeatSequence7()
        {
            var xs = new FailRepeat().ToAsyncEnumerable().Repeat(3);

            var e = xs.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is NotImplementedException);
        }

        private static IEnumerable<int> RepeatXs(Action started)
        {
            started();

            yield return 1;
            yield return 2;
        }

        private sealed class FailRepeat : IEnumerable<int>
        {
            public IEnumerator<int> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
    }
}
