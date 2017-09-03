// Licensed to the .NET Foundation under one or more agreements.
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
    public partial class AsyncTests
    {
        [Fact]
        public void MoveNextExtension_Null()
        {
            var en = default(IAsyncEnumerator<int>);

            Assert.ThrowsAsync<ArgumentNullException>(() => en.MoveNextAsync());
        }

        [Fact]
        public void SelectWhere2()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = xs.Select(i => i + 2).Where(i => i % 2 == 0);

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 2);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void WhereSelect2()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = xs.Where(i => i % 2 == 0).Select(i => i + 2);

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 2);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void WhereSelect3()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = xs.Where(i => i % 2 == 0).Select(i => i + 2).Select(i => i + 2);

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 4);
            HasNext(e, 6);
            NoNext(e);
        }

        [Fact]
        public void AsAsyncEnumerable_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.AsAsyncEnumerable<int>((IAsyncEnumerable<int>)null));
        }

        [Fact]
        public void AsAsyncEnumerable1()
        {
            var xs = AsyncEnumerable.Return(42);
            var ys = xs.AsAsyncEnumerable();

            Assert.NotSame(xs, ys);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 42);
            NoNext(e);
        }

        [Fact]
        public void RepeatSeq_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Repeat(default(IAsyncEnumerable<int>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Repeat(default(IAsyncEnumerable<int>), 3));
            AssertThrows<ArgumentOutOfRangeException>(() => AsyncEnumerableEx.Repeat(AsyncEnumerable.Return(42), -1));
        }

        [Fact]
        public void RepeatSeq1()
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
        public void RepeatSeq2()
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
        public void RepeatSeq3()
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
        public void RepeatSeq0()
        {
            var i = 0;
            var xs = RepeatXs(() => i++).ToAsyncEnumerable().Repeat(0);

            var e = xs.GetAsyncEnumerator();

            NoNext(e);
        }

        [Fact]
        public async Task RepeatSeq6()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Repeat(3);

            await SequenceIdentity(xs);
        }

        static IEnumerable<int> RepeatXs(Action started)
        {
            started();

            yield return 1;
            yield return 2;
        }

        [Fact]
        public void RepeatSeq4()
        {
            var xs = new FailRepeat().ToAsyncEnumerable().Repeat();

            var e = xs.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is NotImplementedException);
        }

        [Fact]
        public void RepeatSeq5()
        {
            var xs = new FailRepeat().ToAsyncEnumerable().Repeat(3);

            var e = xs.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is NotImplementedException);
        }

        class FailRepeat : IEnumerable<int>
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
