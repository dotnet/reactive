// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public partial class AsyncTests
    {
        [Fact]
        public void AppendPrepend1()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Prepend(4)
                        .Append(5)
                        .Append(6)
                        .Prepend(7)
                        .Append(8)
                        .Prepend(9)
                        .Prepend(10);

            var e = res.GetAsyncEnumerator();

            HasNext(e, 10);
            HasNext(e, 9);
            HasNext(e, 7);
            HasNext(e, 4);
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 5);
            HasNext(e, 6);
            HasNext(e, 8);
            NoNext(e);
        }

        [Fact]
        public async Task AppendPrepend2()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Prepend(4)
                        .Append(5)
                        .Append(6)
                        .Prepend(7)
                        .Append(8)
                        .Prepend(9)
                        .Prepend(10);

            var a = new[] { 10, 9, 7, 4, 1, 2, 3, 5, 6, 8 };

            var arr = await res.ToArray();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task AppendPrepend3()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var res = xs.Prepend(4)
                        .Append(5)
                        .Append(6)
                        .Prepend(7)
                        .Append(8)
                        .Prepend(9)
                        .Prepend(10);

            var a = new List<int> { 10, 9, 7, 4, 1, 2, 3, 5, 6, 8 };

            var arr = await res.ToList();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task AppendPrepend4()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Prepend(4)
                        .Append(5)
                        .Append(6)
                        .Prepend(7)
                        .Append(8)
                        .Prepend(9)
                        .Prepend(10);

            Assert.Equal(10, await res.Count());
        }

        [Fact]
        public async Task AppendPrepend5()
        {
            var xs = AsyncEnumerable.Range(1, 3).Where(i => true);

            var res = xs.Prepend(4)
                        .Append(5)
                        .Append(6)
                        .Prepend(7)
                        .Append(8)
                        .Prepend(9)
                        .Prepend(10);

            var a = new[] { 10, 9, 7, 4, 1, 2, 3, 5, 6, 8 };

            var arr = await res.ToArray();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task AppendPrepend6()
        {
            var xs = AsyncEnumerable.Range(1, 3).Where(i => true);

            var res = xs.Prepend(4)
                        .Append(5)
                        .Append(6)
                        .Prepend(7)
                        .Append(8)
                        .Prepend(9)
                        .Prepend(10);

            var a = new List<int> { 10, 9, 7, 4, 1, 2, 3, 5, 6, 8 };

            var arr = await res.ToList();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task AppendPrepend7()
        {
            var xs = AsyncEnumerable.Range(1, 3).Where(i => true);

            var res = xs.Prepend(4)
                        .Append(5)
                        .Append(6)
                        .Prepend(7)
                        .Append(8)
                        .Prepend(9)
                        .Prepend(10);

            Assert.Equal(10, await res.Count());
        }

        [Fact]
        public void AppendPrepend8()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Append(4)
                        .Prepend(5);

            var e = res.GetAsyncEnumerator();

            HasNext(e, 5);
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public async Task AppendPrepend9()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Append(4)
                        .Prepend(5);

            await SequenceIdentity(res);
        }
    }
}
