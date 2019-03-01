// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Generate : Tests
    {
        [Fact]
        public void Generate_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Generate<int, int>(0, null, _ => _, _ => _));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Generate<int, int>(0, _ => true, null, _ => _));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Generate<int, int>(0, _ => true, _ => _, null));
        }

        [Fact]
        public void Generate1()
        {
            var res = EnumerableEx.Generate(0, x => x < 5, x => x + 1, x => x * x).ToList();
            Assert.True(Enumerable.SequenceEqual(res, new[] { 0, 1, 4, 9, 16 }));
        }
    }
}
