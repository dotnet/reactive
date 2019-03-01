// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Defer: Tests
    {
        [Fact]
        public void Defer_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Defer<int>(null));
        }

        [Fact]
        public void Defer1()
        {
            var i = 0;
            var n = 5;
            var xs = EnumerableEx.Defer(() =>
            {
                i++;
                return Enumerable.Range(0, n);
            });

            Assert.Equal(0, i);

            Assert.True(Enumerable.SequenceEqual(xs, Enumerable.Range(0, n)));
            Assert.Equal(1, i);

            n = 3;
            Assert.True(Enumerable.SequenceEqual(xs, Enumerable.Range(0, n)));
            Assert.Equal(2, i);
        }

        [Fact]
        public void Defer2()
        {
            var xs = EnumerableEx.Defer<int>(() =>
            {
                throw new MyException();
            });

            AssertThrows<MyException>(() => xs.GetEnumerator().MoveNext());
        }

        private sealed class MyException : Exception
        {
        }
    }
}
