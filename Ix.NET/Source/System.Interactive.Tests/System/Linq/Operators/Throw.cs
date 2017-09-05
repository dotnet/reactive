// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Throw : Tests
    {
        [Fact]
        public void Throw_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Throw<int>(null));
        }

        [Fact]
        public void Throw1()
        {
            var ex = new MyException();
            var xs = EnumerableEx.Throw<int>(ex);

            var e = xs.GetEnumerator();
            AssertThrows<MyException>(() => e.MoveNext());
        }

        private sealed class MyException : Exception
        {
        }
    }
}
