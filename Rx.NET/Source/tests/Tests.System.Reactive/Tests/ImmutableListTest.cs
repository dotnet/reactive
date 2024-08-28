// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#if SIGNED
using System.Linq;
using System.Reactive;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

using Assert = Xunit.Assert;

namespace ReactiveTests.Tests
{
#if SIGNED
    [TestClass]
    public class ImmutableListTest
    {
        [TestMethod]
        public void ImmutableList_Basics()
        {
            var list = ImmutableList<int>.Empty;

            Assert.True(list.Data.SequenceEqual([]));

            list = list.Add(42);

            Assert.True(list.Data.SequenceEqual([42]));

            list = list.Remove(42);

            Assert.True(list.Data.SequenceEqual([]));

            list = list.Remove(42);

            Assert.True(list.Data.SequenceEqual([]));

            list = list.Add(43);
            list = list.Add(44);
            list = list.Add(43);

            Assert.True(list.Data.SequenceEqual([43, 44, 43]));

            list = list.Remove(43);

            Assert.True(list.Data.SequenceEqual([44, 43]));

            list = list.Remove(43);

            Assert.True(list.Data.SequenceEqual([44]));

            list = list.Remove(44);

            Assert.True(list.Data.SequenceEqual([]));
        }

        [TestMethod]
        public void ImmutableList_Nulls()
        {
            var list = ImmutableList<string>.Empty;

            Assert.True(list.Data.SequenceEqual([]));

            list = list.Add(null);

            Assert.True(list.Data.SequenceEqual([null]));

            list = list.Remove(null);

            Assert.True(list.Data.SequenceEqual([]));
        }
    }
#endif
}
