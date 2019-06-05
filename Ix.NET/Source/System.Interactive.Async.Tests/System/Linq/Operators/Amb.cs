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
    public class Amb : AsyncEnumerableExTests
    {
        [Fact]
        public void Amb_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Amb(default, Return42));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Amb(Return42, default));
        }

        [Fact]
        public async Task Amb_First_Wins()
        {
            var source = AsyncEnumerable.Range(1, 5).Amb(AsyncEnumerableEx.Never<int>());

            var xs = source.GetAsyncEnumerator();

            try
            {
                for (var i = 1; i <= 5; i++)
                {
                    Assert.True(await xs.MoveNextAsync());
                    Assert.Equal(i, xs.Current);
                }

                Assert.False(await xs.MoveNextAsync());
            }
            finally
            {
                await xs.DisposeAsync();
            }
        }

        [Fact]
        public async Task Amb_First_Wins_Alt()
        {
            var source = AsyncEnumerable.Range(1, 5).Amb(AsyncEnumerable.Range(1, 5).SelectAwait(async v =>
            {
                await Task.Delay(500);
                return v;
            }));

            var xs = source.GetAsyncEnumerator();

            try
            {
                for (var i = 1; i <= 5; i++)
                {
                    Assert.True(await xs.MoveNextAsync());
                    Assert.Equal(i, xs.Current);
                }

                Assert.False(await xs.MoveNextAsync());
            }
            finally
            {
                await xs.DisposeAsync();
            }
        }

        [Fact]
        public async Task Amb_Second_Wins()
        {
            var source = AsyncEnumerableEx.Never<int>().Amb(AsyncEnumerable.Range(1, 5));

            var xs = source.GetAsyncEnumerator();

            try
            {
                for (var i = 1; i <= 5; i++)
                {
                    Assert.True(await xs.MoveNextAsync());
                    Assert.Equal(i, xs.Current);
                }

                Assert.False(await xs.MoveNextAsync());
            }
            finally
            {
                await xs.DisposeAsync();
            }
        }

        [Fact]
        public async Task Amb_Second_Wins_Alt()
        {
            var source = AsyncEnumerable.Range(1, 5).SelectAwait(async v =>
            {
                await Task.Delay(500);
                return v;
            }).Amb(AsyncEnumerable.Range(6, 5));

            var xs = source.GetAsyncEnumerator();

            try
            {
                for (var i = 1; i <= 5; i++)
                {
                    Assert.True(await xs.MoveNextAsync());
                    Assert.Equal(i + 5, xs.Current);
                }

                Assert.False(await xs.MoveNextAsync());
            }
            finally
            {
                await xs.DisposeAsync();
            }
        }

        [Fact]
        public async Task Amb_Many_First_Wins()
        {
            var source = AsyncEnumerableEx.Amb(
                AsyncEnumerable.Range(1, 5),
                AsyncEnumerableEx.Never<int>(),
                AsyncEnumerableEx.Never<int>()
            );

            var xs = source.GetAsyncEnumerator();

            try
            {
                for (var i = 1; i <= 5; i++)
                {
                    Assert.True(await xs.MoveNextAsync());
                    Assert.Equal(i, xs.Current);
                }

                Assert.False(await xs.MoveNextAsync());
            }
            finally
            {
                await xs.DisposeAsync();
            }
        }

        [Fact]
        public async Task Amb_Many_Last_Wins()
        {
            var source = AsyncEnumerableEx.Amb(
                AsyncEnumerableEx.Never<int>(),
                AsyncEnumerableEx.Never<int>(),
                AsyncEnumerable.Range(1, 5)
            );

            var xs = source.GetAsyncEnumerator();

            try
            {
                for (var i = 1; i <= 5; i++)
                {
                    Assert.True(await xs.MoveNextAsync());
                    Assert.Equal(i, xs.Current);
                }

                Assert.False(await xs.MoveNextAsync());
            }
            finally
            {
                await xs.DisposeAsync();
            }
        }

        [Fact]
        public async Task Amb_Many_Enum_First_Wins()
        {
            var source = AsyncEnumerableEx.Amb(new[] {
                    AsyncEnumerable.Range(1, 5),
                    AsyncEnumerableEx.Never<int>(),
                    AsyncEnumerableEx.Never<int>()
                }.AsEnumerable()
            );

            var xs = source.GetAsyncEnumerator();

            try
            {
                for (var i = 1; i <= 5; i++)
                {
                    Assert.True(await xs.MoveNextAsync());
                    Assert.Equal(i, xs.Current);
                }

                Assert.False(await xs.MoveNextAsync());
            }
            finally
            {
                await xs.DisposeAsync();
            }
        }

        [Fact]
        public async Task Amb_Many_Enum_Last_Wins()
        {
            var source = AsyncEnumerableEx.Amb(new[] {
                    AsyncEnumerableEx.Never<int>(),
                    AsyncEnumerableEx.Never<int>(),
                    AsyncEnumerable.Range(1, 5)
                }.AsEnumerable()
            );

            var xs = source.GetAsyncEnumerator();

            try
            {
                for (var i = 1; i <= 5; i++)
                {
                    Assert.True(await xs.MoveNextAsync());
                    Assert.Equal(i, xs.Current);
                }

                Assert.False(await xs.MoveNextAsync());
            }
            finally
            {
                await xs.DisposeAsync();
            }
        }
    }
}
