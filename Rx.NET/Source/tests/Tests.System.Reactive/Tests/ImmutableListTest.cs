// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

#if SIGNED
using System.Linq;
using System.Reactive;
using Xunit;
#endif

namespace ReactiveTests.Tests
{
#if SIGNED

    public class ImmutableListTest
    {
        [Fact]
        public void ImmutableList_Basics()
        {
            var list = ImmutableList<int>.Empty;

            Assert.True(list.Data.SequenceEqual(new int[] { }));

            list = list.Add(42);

            Assert.True(list.Data.SequenceEqual(new int[] { 42 }));

            list = list.Remove(42);

            Assert.True(list.Data.SequenceEqual(new int[] { }));

            list = list.Remove(42);

            Assert.True(list.Data.SequenceEqual(new int[] { }));

            list = list.Add(43);
            list = list.Add(44);
            list = list.Add(43);

            Assert.True(list.Data.SequenceEqual(new int[] { 43, 44, 43 }));

            list = list.Remove(43);

            Assert.True(list.Data.SequenceEqual(new int[] { 44, 43 }));

            list = list.Remove(43);

            Assert.True(list.Data.SequenceEqual(new int[] { 44 }));

            list = list.Remove(44);

            Assert.True(list.Data.SequenceEqual(new int[] { }));
        }

        [Fact]
        public void ImmutableList_Nulls()
        {
            var list = ImmutableList<string>.Empty;

            Assert.True(list.Data.SequenceEqual(new string[] { }));

            list = list.Add(null);

            Assert.True(list.Data.SequenceEqual(new string[] { null }));

            list = list.Remove(null);

            Assert.True(list.Data.SequenceEqual(new string[] { }));
        }
    }
#endif
}
