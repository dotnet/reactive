// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class OnErrorResumeNext : Tests
    {
        [Fact]
        public void OnErrorResumeNext_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.OnErrorResumeNext<int>(null, [1]));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.OnErrorResumeNext<int>([1], null));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.OnErrorResumeNext<int>(default));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.OnErrorResumeNext<int>(default(IEnumerable<IEnumerable<int>>)));
        }

        [Fact]
        public void OnErrorResumeNext1()
        {
            var xs = new[] { 1, 2 };
            var ys = new[] { 3, 4 };

            var res = xs.OnErrorResumeNext(ys);
            Assert.True(Enumerable.SequenceEqual(res, [1, 2, 3, 4]));
        }

        [Fact]
        public void OnErrorResumeNext2()
        {
            var xs = new[] { 1, 2 }.Concat(EnumerableEx.Throw<int>(new MyException()));
            var ys = new[] { 3, 4 };

            var res = xs.OnErrorResumeNext(ys);
            Assert.True(Enumerable.SequenceEqual(res, [1, 2, 3, 4]));
        }

        [Fact]
        public void OnErrorResumeNext3()
        {
            var xs = new[] { 1, 2 };
            var ys = new[] { 3, 4 };
            var zs = new[] { 5, 6 };

            var res = EnumerableEx.OnErrorResumeNext(xs, ys, zs);
            Assert.True(Enumerable.SequenceEqual(res, [1, 2, 3, 4, 5, 6]));
        }

        [Fact]
        public void OnErrorResumeNext4()
        {
            var xs = new[] { 1, 2 }.Concat(EnumerableEx.Throw<int>(new MyException()));
            var ys = new[] { 3, 4 };
            var zs = new[] { 5, 6 };

            var res = EnumerableEx.OnErrorResumeNext(xs, ys, zs);
            Assert.True(Enumerable.SequenceEqual(res, [1, 2, 3, 4, 5, 6]));
        }

        [Fact]
        public void OnErrorResumeNext5()
        {
            var xs = new[] { 1, 2 };
            var ys = new[] { 3, 4 };

            var res = new[] { xs, ys }.OnErrorResumeNext();
            Assert.True(Enumerable.SequenceEqual(res, [1, 2, 3, 4]));
        }

        [Fact]
        public void OnErrorResumeNext6()
        {
            var xs = new[] { 1, 2 }.Concat(EnumerableEx.Throw<int>(new MyException()));
            var ys = new[] { 3, 4 };

            var res = new[] { xs, ys }.OnErrorResumeNext();
            Assert.True(Enumerable.SequenceEqual(res, [1, 2, 3, 4]));
        }

        private sealed class MyException : Exception
        {
        }
    }
}
