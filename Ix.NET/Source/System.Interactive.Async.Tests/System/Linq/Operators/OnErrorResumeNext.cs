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
    public class OnErrorResumeNext : AsyncEnumerableExTests
    {
        [Fact]
        public void OnErrorResumeNext_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.OnErrorResumeNext<int>(default, Return42));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.OnErrorResumeNext<int>(Return42, default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.OnErrorResumeNext<int>(default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.OnErrorResumeNext<int>(default(IEnumerable<IAsyncEnumerable<int>>)));
        }

        [Fact]
        public void OnErrorResumeNext7()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();

            var res = AsyncEnumerableEx.OnErrorResumeNext(xs, ys);

            var e = res.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            HasNext(e, 5);
            HasNext(e, 6);
            NoNext(e);
        }

        [Fact]
        public void OnErrorResumeNext8()
        {
            var ex = new InvalidOperationException("Bang!");

            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Concat(Throw<int>(ex));
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();

            var res = AsyncEnumerableEx.OnErrorResumeNext(xs, ys);

            var e = res.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            HasNext(e, 5);
            HasNext(e, 6);
            NoNext(e);
        }

        [Fact]
        public void OnErrorResumeNext9()
        {
            var ex = new InvalidOperationException("Bang!");

            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Concat(Throw<int>(ex));
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();

            var res = AsyncEnumerableEx.OnErrorResumeNext(new[] { xs, xs, ys, ys });

            var e = res.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            HasNext(e, 5);
            HasNext(e, 6);
            HasNext(e, 4);
            HasNext(e, 5);
            HasNext(e, 6);
            NoNext(e);
        }

        [Fact]
        public void OnErrorResumeNext10()
        {
            var res = OnErrorResumeNextXss().OnErrorResumeNext();

            var e = res.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);

            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single().Message == "Bang!");
        }

        private IEnumerable<IAsyncEnumerable<int>> OnErrorResumeNextXss()
        {
            yield return new[] { 1, 2, 3 }.ToAsyncEnumerable().Concat(Throw<int>(new Exception("!!!")));
            throw new Exception("Bang!");
        }

        [Fact]
        public void OnErrorResumeNext11()
        {
            var ex = new InvalidOperationException("Bang!");

            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Concat(Throw<int>(ex));

            var res = AsyncEnumerableEx.OnErrorResumeNext(new[] { xs, xs });

            var e = res.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            NoNext(e);
        }

        [Fact]
        public void OnErrorResumeNext12()
        {
            var res = AsyncEnumerableEx.OnErrorResumeNext(Enumerable.Empty<IAsyncEnumerable<int>>());

            var e = res.GetAsyncEnumerator();
            NoNext(e);
        }

        [Fact]
        public async Task OnErrorResumeNext13()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();

            var res = AsyncEnumerableEx.OnErrorResumeNext(xs, ys);

            await SequenceIdentity(res);
        }
    }
}
