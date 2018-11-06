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
    public class SelectMany : AsyncEnumerableTests
    {
        [Fact]
        public void SelectMany_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SelectMany<int, int>(default, default(Func<int, IAsyncEnumerable<int>>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SelectMany<int, int>(default, default(Func<int, int, IAsyncEnumerable<int>>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SelectMany<int, int>(Return42, default(Func<int, IAsyncEnumerable<int>>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SelectMany<int, int>(Return42, default(Func<int, int, IAsyncEnumerable<int>>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SelectMany<int, int, int>(default, default(Func<int, IAsyncEnumerable<int>>), (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SelectMany<int, int, int>(default, default(Func<int, int, IAsyncEnumerable<int>>), (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SelectMany<int, int, int>(Return42, default(Func<int, IAsyncEnumerable<int>>), (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SelectMany<int, int, int>(Return42, default(Func<int, int, IAsyncEnumerable<int>>), (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SelectMany<int, int, int>(Return42, x => default, default(Func<int, int, int>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SelectMany<int, int, int>(Return42, (x, i) => default, default(Func<int, int, int>)));
        }

        [Fact]
        public void SelectMany1()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany(x => Enumerable.Range(0, x).ToAsyncEnumerable());

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 0);
            HasNext(e, 0);
            HasNext(e, 1);
            HasNext(e, 0);
            HasNext(e, 1);
            HasNext(e, 2);
            NoNext(e);
        }

        [Fact]
        public void SelectMany2()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany(x =>
            {
                if (x < 3)
                    return Enumerable.Range(0, x).ToAsyncEnumerable();
                else
                    return Throw<int>(ex);
            });

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 0);
            HasNext(e, 0);
            HasNext(e, 1);
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
        }

        [Fact]
        public void SelectMany3()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex);
            var ys = xs.SelectMany(x => Enumerable.Range(0, x).ToAsyncEnumerable());

            var e = ys.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
        }

        [Fact]
        public void SelectMany4()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany(x =>
            {
                if (x < 3)
                    return Enumerable.Range(0, x).ToAsyncEnumerable();
                else
                    throw ex;
            });

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 0);
            HasNext(e, 0);
            HasNext(e, 1);
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
        }

        [Fact]
        public void SelectMany5()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany((x, i) => Enumerable.Range(i + 5, x).ToAsyncEnumerable());

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 5);
            HasNext(e, 6);
            HasNext(e, 7);
            HasNext(e, 7);
            HasNext(e, 8);
            HasNext(e, 9);
            NoNext(e);
        }

        [Fact]
        public void SelectMany6()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany((x, i) =>
            {
                if (i < 2)
                    return Enumerable.Range(0, x).ToAsyncEnumerable();
                else
                    return Throw<int>(ex);
            });

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 0);
            HasNext(e, 0);
            HasNext(e, 1);
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
        }

        [Fact]
        public void SelectMany7()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex);
            var ys = xs.SelectMany((x, i) => Enumerable.Range(0, x).ToAsyncEnumerable());

            var e = ys.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
        }

        [Fact]
        public void SelectMany8()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany((x, i) =>
            {
                if (i < 2)
                    return Enumerable.Range(0, x).ToAsyncEnumerable();
                else
                    throw ex;
            });

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 0);
            HasNext(e, 0);
            HasNext(e, 1);
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
        }

        [Fact]
        public void SelectMany9()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany(x => Enumerable.Range(3, x).ToAsyncEnumerable(), (x, y) => x * y);

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 3);
            HasNext(e, 6);
            HasNext(e, 8);
            HasNext(e, 9);
            HasNext(e, 12);
            HasNext(e, 15);
            NoNext(e);
        }

        [Fact]
        public void SelectMany10()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany((x, i) => Enumerable.Range(i + 3, x).ToAsyncEnumerable(), (x, y) => x * y);

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 3);
            HasNext(e, 8);
            HasNext(e, 10);
            HasNext(e, 15);
            HasNext(e, 18);
            HasNext(e, 21);
            NoNext(e);
        }

        [Fact]
        public void SelectMany11()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany(x => Enumerable.Range(3, x).ToAsyncEnumerable(), (x, y) =>
            {
                if (x * y > 10)
                    throw ex;
                return x * y;
            });

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 3);
            HasNext(e, 6);
            HasNext(e, 8);
            HasNext(e, 9);
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
        }

        [Fact]
        public void SelectMany12()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany((x, i) => Enumerable.Range(i + 3, x).ToAsyncEnumerable(), (x, y) =>
            {
                if (x * y > 10)
                    throw ex;
                return x * y;
            });

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 3);
            HasNext(e, 8);
            HasNext(e, 10);
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
        }

        [Fact]
        public async Task SelectMany13()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany(x => Enumerable.Range(0, x).ToAsyncEnumerable());

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task SelectMany14()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany((x, i) => Enumerable.Range(i + 5, x).ToAsyncEnumerable());

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task SelectMany15()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany(x => Enumerable.Range(3, x).ToAsyncEnumerable(), (x, y) => x * y);

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task SelectMany16()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany((x, i) => Enumerable.Range(i + 3, x).ToAsyncEnumerable(), (x, y) => x * y);

            await SequenceIdentity(ys);
        }
    }
}
