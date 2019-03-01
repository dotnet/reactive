// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Finally : Tests
    {
        [Fact]
        public void Finally_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Finally<int>(null, () => { }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Finally<int>(new[] { 1 }, null));
        }

        [Fact]
        public void Finally1()
        {
            var done = false;

            var xs = Enumerable.Range(0, 2).Finally(() => done = true);
            Assert.False(done);

            var e = xs.GetEnumerator();
            Assert.False(done);

            HasNext(e, 0);
            Assert.False(done);

            HasNext(e, 1);
            Assert.False(done);

            NoNext(e);
            Assert.True(done);
        }

        [Fact]
        public void Finally2()
        {
            var done = false;

            var xs = Enumerable.Range(0, 2).Finally(() => done = true);
            Assert.False(done);

            var e = xs.GetEnumerator();
            Assert.False(done);

            HasNext(e, 0);
            Assert.False(done);

            e.Dispose();
            Assert.True(done);
        }

        [Fact]
        public void Finally3()
        {
            var done = false;

            var ex = new MyException();
            var xs = EnumerableEx.Throw<int>(ex).Finally(() => done = true);
            Assert.False(done);

            var e = xs.GetEnumerator();
            Assert.False(done);

            try
            {
                HasNext(e, 0);
                Assert.True(false);
            }
            catch (MyException ex_)
            {
                Assert.Same(ex, ex_);
            }

            Assert.True(done);
        }

        private sealed class MyException : Exception
        {
        }
    }
}
