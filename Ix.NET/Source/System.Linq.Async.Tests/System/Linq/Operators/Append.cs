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
    public class Append : AsyncEnumerableTests
    {
        [Fact]
        public void Append_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Append(null, 42));
        }

        [Fact]
        public void Append1()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Append(4);

            var e = res.GetAsyncEnumerator();

            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public async Task Append2()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Append(4);

            var a = new[] { 1, 2, 3, 4 };

            var arr = await res.ToArray();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task Append3()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var res = xs.Append(4);
            var a = new List<int>
            {
                1,
                2,
                3,
                4
            };
            var arr = await res.ToList();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task Append4()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Append(4);
            Assert.Equal(4, await res.Count());
        }

        [Fact]
        public async Task Append5()
        {
            var xs = AsyncEnumerable.Range(1, 3)
                                    .Where(i => true);

            var res = xs.Append(4);

            var a = new[] { 1, 2, 3, 4 };

            var arr = await res.ToArray();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task Append6()
        {
            var xs = AsyncEnumerable.Range(1, 3)
                                    .Where(i => true);

            var res = xs.Append(4);
            Assert.Equal(4, await res.Count());
        }

        [Fact]
        public async Task Append7()
        {
            var xs = AsyncEnumerable.Range(1, 3)
                                    .Where(i => true);
            var res = xs.Append(4);
            var a = new List<int>
            {
                1,
                2,
                3,
                4
            };
            var arr = await res.ToList();
            Assert.Equal(a, arr);
        }


        [Fact]
        public void AppendN1()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Append(4)
                        .Append(5)
                        .Append(6);

            var e = res.GetAsyncEnumerator();

            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            HasNext(e, 5);
            HasNext(e, 6);
            NoNext(e);
        }

        [Fact]
        public async Task AppendN2()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Append(4)
                        .Append(5)
                        .Append(6);

            var a = new[] { 1, 2, 3, 4, 5, 6 };

            var arr = await res.ToArray();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task AppendN3()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var res = xs.Append(4)
                        .Append(5)
                        .Append(6);

            var a = new List<int>
            {
                1,
                2,
                3,
                4,
                5,
                6
            };

            var arr = await res.ToList();
            Assert.Equal(a, arr);
        }


        [Fact]
        public async Task AppendN5()
        {
            var xs = AsyncEnumerable.Range(1, 3)
                                    .Where(i => true);

            var res = xs.Append(4)
                        .Append(5)
                        .Append(6);

            var a = new[] { 1, 2, 3, 4, 5, 6 };

            var arr = await res.ToArray();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task AppendN6()
        {
            var xs = AsyncEnumerable.Range(1, 3)
                                    .Where(i => true);
            var res = xs.Append(4)
                        .Append(5)
                        .Append(6);

            var a = new List<int>
            {
                1,
                2,
                3,
                4,
                5,
                6
            };

            var arr = await res.ToList();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task AppendN7()
        {
            var xs = AsyncEnumerable.Range(1, 3)
                                    .Where(i => true);

            var res = xs.Append(4)
                        .Append(5)
                        .Append(6);
            Assert.Equal(6, await res.Count());
        }

        [Fact]
        public async Task AppenN4()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Append(4)
                        .Append(5)
                        .Append(6);
            Assert.Equal(6, await res.Count());
        }
    }
}
