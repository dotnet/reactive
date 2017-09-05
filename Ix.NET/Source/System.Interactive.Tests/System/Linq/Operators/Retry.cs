// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Retry : Tests
    {
        [Fact]
        public void Retry_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Retry<int>(null));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Retry<int>(null, 5));
            AssertThrows<ArgumentOutOfRangeException>(() => EnumerableEx.Retry<int>(new[] { 1 }, -1));
        }

        [Fact]
        public void Retry1()
        {
            var xs = Enumerable.Range(0, 10);

            var res = xs.Retry();
            Assert.True(Enumerable.SequenceEqual(res, xs));
        }

        [Fact]
        public void Retry2()
        {
            var xs = Enumerable.Range(0, 10);

            var res = xs.Retry(2);
            Assert.True(Enumerable.SequenceEqual(res, xs));
        }

        [Fact]
        public void Retry3()
        {
            var ex = new MyException();
            var xs = Enumerable.Range(0, 2).Concat(EnumerableEx.Throw<int>(ex));

            var res = xs.Retry(2);
            var e = res.GetEnumerator();
            HasNext(e, 0);
            HasNext(e, 1);
            HasNext(e, 0);
            HasNext(e, 1);
            AssertThrows<MyException>(() => e.MoveNext(), ex_ => ex == ex_);
        }

        private sealed class MyException : Exception
        {
        }
    }
}
