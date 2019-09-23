// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Using : Tests
    {
        [Fact]
        public void Using_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Using<int, MyDisposable>(null, d => new[] { 1 }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Using<int, MyDisposable>(() => new MyDisposable(), null));
        }

        [Fact]
        public void Using1()
        {
            var d = default(MyDisposable);

            var xs = EnumerableEx.Using(() => d = new MyDisposable(), d_ => new[] { 1 });
            Assert.Null(d);

            var d1 = default(MyDisposable);
            xs.ForEach(_ => { d1 = d; Assert.NotNull(d1); Assert.False(d1!.Done); });
            Assert.True(d1!.Done);

            var d2 = default(MyDisposable);
            xs.ForEach(_ => { d2 = d; Assert.NotNull(d2); Assert.False(d2!.Done); });
            Assert.True(d2!.Done);

            Assert.NotSame(d1, d2);
        }

        [Fact]
        public void Using2()
        {
            var d = default(MyDisposable);

            var xs = EnumerableEx.Using(() => d = new MyDisposable(), d_ => EnumerableEx.Throw<int>(new MyException()));
            Assert.Null(d);

            AssertThrows<MyException>(() => xs.ForEach(_ => { }));
            Assert.True(d!.Done);
        }

        [Fact]
        public void Using3()
        {
            var d = default(MyDisposable);

            var xs = EnumerableEx.Using<int, MyDisposable>(() => d = new MyDisposable(), d_ => { throw new MyException(); });
            Assert.Null(d);

            AssertThrows<MyException>(() => xs.ForEach(_ => { }));
            Assert.True(d!.Done);
        }

        private sealed class MyDisposable : IDisposable
        {
            public bool Done;

            public void Dispose()
            {
                Done = true;
            }
        }

        private sealed class MyException : Exception
        {
        }
    }
}
