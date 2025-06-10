﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using Xunit;

namespace Tests
{
    public class If : Tests
    {
        [Fact]
        public void If_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.If<int>(null, [1]));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.If<int>(() => true, null));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.If<int>(null, [1], [1]));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.If<int>(() => true, null, [1]));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.If<int>(() => true, [1], null));
        }

        [Fact]
        public void If1()
        {
            var x = 5;
            var res = EnumerableEx.If(() => x > 0, [+1], [-1]);

            Assert.Equal(+1, res.Single());

            x = -x;
            Assert.Equal(-1, res.Single());
        }

        [Fact]
        public void If2()
        {
            var x = 5;
            var res = EnumerableEx.If(() => x > 0, new[] { +1 });

            Assert.Equal(+1, res.Single());

            x = -x;
            Assert.True(res.IsEmpty());
        }
    }
}
