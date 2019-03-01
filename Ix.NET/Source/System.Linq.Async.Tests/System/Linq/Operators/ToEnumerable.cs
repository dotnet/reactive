// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class ToEnumerable : AsyncEnumerableTests
    {
        [Fact]
        public void ToEnumerable_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.ToEnumerable<int>(null));
        }

        [Fact]
        public void ToEnumerable_Single()
        {
            var xs = Return42.ToEnumerable();
            Assert.True(xs.SequenceEqual(new[] { 42 }));
        }

        [Fact]
        public void ToEnumerable_Empty()
        {
            var xs = AsyncEnumerable.Empty<int>().ToEnumerable();
            Assert.True(xs.SequenceEqual(new int[0]));
        }

        [Fact]
        public void ToEnumerable_Throws_Source()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex).ToEnumerable();
            Assert.Throws<Exception>(() => xs.GetEnumerator().MoveNext());
        }

        [Fact]
        public void ToEnumerable_Many()
        {
            var xs = new[] { 1, 2, 3 };

            Assert.Equal(xs, xs.ToAsyncEnumerable().ToEnumerable());
        }

        [Fact]
        public void ToEnumerable_Many_Yield()
        {
            var xs = new[] { 1, 2, 3 };

            Assert.Equal(xs, Yielded(xs).ToEnumerable());
        }

        private static async IAsyncEnumerable<int> Yielded(IEnumerable<int> xs)
        {
            try
            {
                foreach (var x in xs)
                {
                    await Task.Yield();
                    yield return x;
                }
            }
            finally
            {
                await Task.Yield();
            }
        }
    }
}
