// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantTypeArgumentsOfMethod

namespace Tests
{
    public partial class AsyncTests
    {
        private const int WaitTimeoutMs = 5000;

        [Fact]
        public async Task Aggregate_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int>(null, (x, y) => x + y));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int>(AsyncEnumerable.Return(42), null));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int, int>(null, 0, (x, y) => x + y));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int, int>(AsyncEnumerable.Return(42), 0, null));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int, int, int>(null, 0, (x, y) => x + y, z => z));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int, int, int>(AsyncEnumerable.Return(42), 0, null, z => z));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int, int, int>(AsyncEnumerable.Return(42), 0, (x, y) => x + y, null));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int>(null, (x, y) => x + y, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int>(AsyncEnumerable.Return(42), null, CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int, int>(null, 0, (x, y) => x + y, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int, int>(AsyncEnumerable.Return(42), 0, null, CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int, int, int>(null, 0, (x, y) => x + y, z => z, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int, int, int>(AsyncEnumerable.Return(42), 0, null, z => z, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int, int, int>(AsyncEnumerable.Return(42), 0, (x, y) => x + y, null, CancellationToken.None));
        }

        [Fact]
        public void Aggregate1()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Aggregate((x, y) => x * y);
            Assert.Equal(24, ys.Result);
        }

        [Fact]
        public void Aggregate2()
        {
            var xs = new int[0].ToAsyncEnumerable();
            var ys = xs.Aggregate((x, y) => x * y);
            AssertThrows<Exception>(() => ys.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void Aggregate3()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Throw<int>(ex);
            var ys = xs.Aggregate((x, y) => x * y);
            AssertThrows<Exception>(() => ys.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Aggregate4()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Aggregate((x, y) => { throw ex; });
            AssertThrows<Exception>(() => ys.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Aggregate5()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Aggregate(1, (x, y) => x * y);
            Assert.Equal(24, ys.Result);
        }

        [Fact]
        public void Aggregate6()
        {
            var xs = new int[0].ToAsyncEnumerable();
            var ys = xs.Aggregate(1, (x, y) => x * y);
            Assert.Equal(1, ys.Result);
        }

        [Fact]
        public void Aggregate7()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Throw<int>(ex);
            var ys = xs.Aggregate(1, (x, y) => x * y);
            AssertThrows<Exception>(() => ys.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Aggregate8()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Aggregate(1, (x, y) => { throw ex; });
            AssertThrows<Exception>(() => ys.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Aggregate9()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Aggregate(1, (x, y) => x * y, x => x + 1);
            Assert.Equal(25, ys.Result);
        }

        [Fact]
        public void Aggregate10()
        {
            var xs = new int[0].ToAsyncEnumerable();
            var ys = xs.Aggregate(1, (x, y) => x * y, x => x + 1);
            Assert.Equal(2, ys.Result);
        }

        [Fact]
        public void Aggregate11()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Throw<int>(ex);
            var ys = xs.Aggregate(1, (x, y) => x * y, x => x + 1);
            AssertThrows<Exception>(() => ys.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Aggregate12()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Aggregate(1, (x, y) => { throw ex; }, x => x + 1);
            AssertThrows<Exception>(() => ys.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Aggregate13()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Aggregate<int, int, int>(1, (x, y) => x * y, x => { throw ex; });
            AssertThrows<Exception>(() => ys.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public async Task Count_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Count<int>(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Count<int>(null, x => true));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Count<int>(AsyncEnumerable.Return(42), null));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Count<int>(null, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Count<int>(null, x => true, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Count<int>(AsyncEnumerable.Return(42), null, CancellationToken.None));
        }

        [Fact]
        public void Count1()
        {
            Assert.Equal(0, new int[0].ToAsyncEnumerable().Count().Result);
            Assert.Equal(3, new[] { 1, 2, 3 }.ToAsyncEnumerable().Count().Result);
            AssertThrows<AggregateException>(() => AsyncEnumerable.Throw<int>(new Exception("Bang!")).Count().Wait(WaitTimeoutMs));
        }

        [Fact]
        public void Count2()
        {
            Assert.Equal(0, new int[0].ToAsyncEnumerable().Count(x => x < 3).Result);
            Assert.Equal(2, new[] { 1, 2, 3 }.ToAsyncEnumerable().Count(x => x < 3).Result);
            AssertThrows<AggregateException>(() => AsyncEnumerable.Throw<int>(new Exception("Bang!")).Count(x => x < 3).Wait(WaitTimeoutMs));
        }

        [Fact]
        public void Count3()
        {
            var ex = new Exception("Bang!");
            var ys = new[] { 1, 2, 3 }.ToAsyncEnumerable().Count(x => { throw ex; });
            AssertThrows<Exception>(() => ys.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public async Task LongCount_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCount<int>(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCount<int>(null, x => true));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCount<int>(AsyncEnumerable.Return(42), null));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCount<int>(null, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCount<int>(null, x => true, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCount<int>(AsyncEnumerable.Return(42), null, CancellationToken.None));
        }

        [Fact]
        public void LongCount1()
        {
            Assert.Equal(0, new int[0].ToAsyncEnumerable().LongCount().Result);
            Assert.Equal(3, new[] { 1, 2, 3 }.ToAsyncEnumerable().LongCount().Result);
            AssertThrows<AggregateException>(() => AsyncEnumerable.Throw<int>(new Exception("Bang!")).LongCount().Wait(WaitTimeoutMs));
        }

        [Fact]
        public void LongCount2()
        {
            Assert.Equal(0, new int[0].ToAsyncEnumerable().LongCount(x => x < 3).Result);
            Assert.Equal(2, new[] { 1, 2, 3 }.ToAsyncEnumerable().LongCount(x => x < 3).Result);
            AssertThrows<AggregateException>(() => AsyncEnumerable.Throw<int>(new Exception("Bang!")).LongCount(x => x < 3).Wait(WaitTimeoutMs));
        }

        [Fact]
        public void LongCount3()
        {
            var ex = new Exception("Bang!");
            var ys = new[] { 1, 2, 3 }.ToAsyncEnumerable().LongCount(x => { throw ex; });
            AssertThrows<Exception>(() => ys.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public async Task All_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.All<int>(null, x => true));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.All<int>(AsyncEnumerable.Return(42), null));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.All<int>(null, x => true, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.All<int>(AsyncEnumerable.Return(42), null, CancellationToken.None));
        }

        [Fact]
        public void All1()
        {
            var res = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().All(x => x % 2 == 0);
            Assert.False(res.Result);
        }

        [Fact]
        public void All2()
        {
            var res = new[] { 2, 8, 4 }.ToAsyncEnumerable().All(x => x % 2 == 0);
            Assert.True(res.Result);
        }

        [Fact]
        public void All3()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).All(x => x % 2 == 0);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void All4()
        {
            var ex = new Exception("Bang!");
            var res = new[] { 2, 8, 4 }.ToAsyncEnumerable().All(x => { throw ex; });
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public async Task Any_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Any<int>(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Any<int>(null, x => true));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Any<int>(AsyncEnumerable.Return(42), null));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Any<int>(null, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Any<int>(null, x => true, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Any<int>(AsyncEnumerable.Return(42), null, CancellationToken.None));
        }

        [Fact]
        public void Any1()
        {
            var res = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().Any(x => x % 2 == 0);
            Assert.True(res.Result);
        }

        [Fact]
        public void Any2()
        {
            var res = new[] { 2, 8, 4 }.ToAsyncEnumerable().Any(x => x % 2 != 0);
            Assert.False(res.Result);
        }

        [Fact]
        public void Any3()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).Any(x => x % 2 == 0);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Any4()
        {
            var ex = new Exception("Bang!");
            var res = new[] { 2, 8, 4 }.ToAsyncEnumerable().Any(x => { throw ex; });
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Any5()
        {
            var res = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().Any();
            Assert.True(res.Result);
        }

        [Fact]
        public void Any6()
        {
            var res = new int[0].ToAsyncEnumerable().Any();
            Assert.False(res.Result);
        }

        [Fact]
        public async Task Contains_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Contains<int>(null, 42));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Contains<int>(null, 42, EqualityComparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Contains<int>(AsyncEnumerable.Return(42), 42, null));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Contains<int>(null, 42, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Contains<int>(null, 42, EqualityComparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Contains<int>(AsyncEnumerable.Return(42), 42, null, CancellationToken.None));
        }

        [Fact]
        public void Contains1()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable();
            var ys = xs.Contains(3);
            Assert.True(ys.Result);
        }

        [Fact]
        public void Contains2()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable();
            var ys = xs.Contains(6);
            Assert.False(ys.Result);
        }

        [Fact]
        public void Contains3()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable();
            var ys = xs.Contains(-3, new Eq());
            Assert.True(ys.Result);
        }

        [Fact]
        public void Contains4()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable();
            var ys = xs.Contains(-6, new Eq());
            Assert.False(ys.Result);
        }

        class Eq : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return EqualityComparer<int>.Default.Equals(Math.Abs(x), Math.Abs(y));
            }

            public int GetHashCode(int obj)
            {
                return EqualityComparer<int>.Default.GetHashCode(Math.Abs(obj));
            }
        }

        [Fact]
        public async Task First_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.First<int>(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.First<int>(null, x => true));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.First<int>(AsyncEnumerable.Return(42), null));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.First<int>(null, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.First<int>(null, x => true, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.First<int>(AsyncEnumerable.Return(42), null, CancellationToken.None));
        }

        [Fact]
        public void First1()
        {
            var res = AsyncEnumerable.Empty<int>().First();
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void First2()
        {
            var res = AsyncEnumerable.Empty<int>().First(x => true);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void First3()
        {
            var res = AsyncEnumerable.Return(42).First(x => x % 2 != 0);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void First4()
        {
            var res = AsyncEnumerable.Return(42).First();
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void First5()
        {
            var res = AsyncEnumerable.Return(42).First(x => x % 2 == 0);
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void First6()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).First();
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void First7()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).First(x => true);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void First8()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().First();
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void First9()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().First(x => x % 2 != 0);
            Assert.Equal(45, res.Result);
        }

        [Fact]
        public async Task FirstOrDefault_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.FirstOrDefault<int>(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.FirstOrDefault<int>(null, x => true));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.FirstOrDefault<int>(AsyncEnumerable.Return(42), null));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.FirstOrDefault<int>(null, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.FirstOrDefault<int>(null, x => true, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.FirstOrDefault<int>(AsyncEnumerable.Return(42), null, CancellationToken.None));
        }

        [Fact]
        public void FirstOrDefault1()
        {
            var res = AsyncEnumerable.Empty<int>().FirstOrDefault();
            Assert.Equal(0, res.Result);
        }

        [Fact]
        public void FirstOrDefault2()
        {
            var res = AsyncEnumerable.Empty<int>().FirstOrDefault(x => true);
            Assert.Equal(0, res.Result);
        }

        [Fact]
        public void FirstOrDefault3()
        {
            var res = AsyncEnumerable.Return(42).FirstOrDefault(x => x % 2 != 0);
            Assert.Equal(0, res.Result);
        }

        [Fact]
        public void FirstOrDefault4()
        {
            var res = AsyncEnumerable.Return(42).FirstOrDefault();
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void FirstOrDefault5()
        {
            var res = AsyncEnumerable.Return(42).FirstOrDefault(x => x % 2 == 0);
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void FirstOrDefault6()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).FirstOrDefault();
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void FirstOrDefault7()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).FirstOrDefault(x => true);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void FirstOrDefault8()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().FirstOrDefault();
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void FirstOrDefault9()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().FirstOrDefault(x => x % 2 != 0);
            Assert.Equal(45, res.Result);
        }

        [Fact]
        public void FirstOrDefault10()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().FirstOrDefault(x => x < 10);
            Assert.Equal(0, res.Result);
        }

        [Fact]
        public async Task Last_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Last<int>(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Last<int>(null, x => true));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Last<int>(AsyncEnumerable.Return(42), null));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Last<int>(null, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Last<int>(null, x => true, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Last<int>(AsyncEnumerable.Return(42), null, CancellationToken.None));
        }

        [Fact]
        public void Last1()
        {
            var res = AsyncEnumerable.Empty<int>().Last();
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void Last2()
        {
            var res = AsyncEnumerable.Empty<int>().Last(x => true);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void Last3()
        {
            var res = AsyncEnumerable.Return(42).Last(x => x % 2 != 0);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void Last4()
        {
            var res = AsyncEnumerable.Return(42).Last();
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void Last5()
        {
            var res = AsyncEnumerable.Return(42).Last(x => x % 2 == 0);
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void Last6()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).Last();
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Last7()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).Last(x => true);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Last8()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().Last();
            Assert.Equal(90, res.Result);
        }

        [Fact]
        public void Last9()
        {
            var res = new[] { 42, 23, 45, 90 }.ToAsyncEnumerable().Last(x => x % 2 != 0);
            Assert.Equal(45, res.Result);
        }

        [Fact]
        public async Task LastOrDefault_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastOrDefault<int>(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastOrDefault<int>(null, x => true));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastOrDefault<int>(AsyncEnumerable.Return(42), null));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastOrDefault<int>(null, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastOrDefault<int>(null, x => true, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastOrDefault<int>(AsyncEnumerable.Return(42), null, CancellationToken.None));
        }

        [Fact]
        public void LastOrDefault1()
        {
            var res = AsyncEnumerable.Empty<int>().LastOrDefault();
            Assert.Equal(0, res.Result);
        }

        [Fact]
        public void LastOrDefault2()
        {
            var res = AsyncEnumerable.Empty<int>().LastOrDefault(x => true);
            Assert.Equal(0, res.Result);
        }

        [Fact]
        public void LastOrDefault3()
        {
            var res = AsyncEnumerable.Return(42).LastOrDefault(x => x % 2 != 0);
            Assert.Equal(0, res.Result);
        }

        [Fact]
        public void LastOrDefault4()
        {
            var res = AsyncEnumerable.Return(42).LastOrDefault();
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void LastOrDefault5()
        {
            var res = AsyncEnumerable.Return(42).LastOrDefault(x => x % 2 == 0);
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void LastOrDefault6()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).LastOrDefault();
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void LastOrDefault7()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).LastOrDefault(x => true);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void LastOrDefault8()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().LastOrDefault();
            Assert.Equal(90, res.Result);
        }

        [Fact]
        public void LastOrDefault9()
        {
            var res = new[] { 42, 23, 45, 90 }.ToAsyncEnumerable().LastOrDefault(x => x % 2 != 0);
            Assert.Equal(45, res.Result);
        }

        [Fact]
        public void LastOrDefault10()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().LastOrDefault(x => x < 10);
            Assert.Equal(0, res.Result);
        }

        [Fact]
        public async Task Single_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Single<int>(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Single<int>(null, x => true));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Single<int>(AsyncEnumerable.Return(42), null));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Single<int>(null, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Single<int>(null, x => true, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Single<int>(AsyncEnumerable.Return(42), null, CancellationToken.None));
        }

        [Fact]
        public void Single1()
        {
            var res = AsyncEnumerable.Empty<int>().Single();
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void Single2()
        {
            var res = AsyncEnumerable.Empty<int>().Single(x => true);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void Single3()
        {
            var res = AsyncEnumerable.Return(42).Single(x => x % 2 != 0);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void Single4()
        {
            var res = AsyncEnumerable.Return(42).Single();
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void Single5()
        {
            var res = AsyncEnumerable.Return(42).Single(x => x % 2 == 0);
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void Single6()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).Single();
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Single7()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).Single(x => true);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Single8()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().Single();
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void Single9()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().Single(x => x % 2 != 0);
            Assert.Equal(45, res.Result);
        }

        [Fact]
        public void Single10()
        {
            var res = new[] { 42, 23, 45, 90 }.ToAsyncEnumerable().Single(x => x % 2 != 0);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void Single11()
        {
            var res = new int[0].ToAsyncEnumerable().Single();
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public async Task SingleOrDefault_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefault<int>(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefault<int>(null, x => true));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefault<int>(AsyncEnumerable.Return(42), null));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefault<int>(null, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefault<int>(null, x => true, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefault<int>(AsyncEnumerable.Return(42), null, CancellationToken.None));
        }

        [Fact]
        public void SingleOrDefault1()
        {
            var res = AsyncEnumerable.Empty<int>().SingleOrDefault();
            Assert.Equal(0, res.Result);
        }

        [Fact]
        public void SingleOrDefault2()
        {
            var res = AsyncEnumerable.Empty<int>().SingleOrDefault(x => true);
            Assert.Equal(0, res.Result);
        }

        [Fact]
        public void SingleOrDefault3()
        {
            var res = AsyncEnumerable.Return(42).SingleOrDefault(x => x % 2 != 0);
            Assert.Equal(0, res.Result);
        }

        [Fact]
        public void SingleOrDefault4()
        {
            var res = AsyncEnumerable.Return(42).SingleOrDefault();
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void SingleOrDefault5()
        {
            var res = AsyncEnumerable.Return(42).SingleOrDefault(x => x % 2 == 0);
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void SingleOrDefault6()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).SingleOrDefault();
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void SingleOrDefault7()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).SingleOrDefault(x => true);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void SingleOrDefault8()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleOrDefault();
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void SingleOrDefault9()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleOrDefault(x => x % 2 != 0);
            Assert.Equal(45, res.Result);
        }

        [Fact]
        public void SingleOrDefault10()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleOrDefault(x => x < 10);
            Assert.Equal(0, res.Result);
        }

        [Fact]
        public void SingleOrDefault11()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleOrDefault(x => true);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void SingleOrDefault12()
        {
            var res = new int[0].ToAsyncEnumerable().SingleOrDefault();
            Assert.Equal(0, res.Result);
        }

        [Fact]
        public async Task ElementAt_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ElementAt<int>(null, 0));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => AsyncEnumerable.ElementAt<int>(AsyncEnumerable.Return(42), -1));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ElementAt<int>(null, 0, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => AsyncEnumerable.ElementAt<int>(AsyncEnumerable.Return(42), -1, CancellationToken.None));
        }

        [Fact]
        public void ElementAt1()
        {
            var res = AsyncEnumerable.Empty<int>().ElementAt(0);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is ArgumentOutOfRangeException);
        }

        [Fact]
        public void ElementAt2()
        {
            var res = AsyncEnumerable.Return<int>(42).ElementAt(0);
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void ElementAt3()
        {
            var res = AsyncEnumerable.Return<int>(42).ElementAt(1);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is ArgumentOutOfRangeException);
        }

        [Fact]
        public void ElementAt4()
        {
            var res = new[] { 1, 42, 3 }.ToAsyncEnumerable().ElementAt(1);
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void ElementAt5()
        {
            var res = new[] { 1, 42, 3 }.ToAsyncEnumerable().ElementAt(7);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is ArgumentOutOfRangeException);
        }

        [Fact]
        public void ElementAt6()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).ElementAt(15);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public async Task ElementAt7()
        {
            var en = new CancellationTestAsyncEnumerable(10);

            var res = en.ElementAt(1);
            Assert.Equal(1, await res);
        }

        [Fact]
        public async Task ElementAtOrDefault_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ElementAtOrDefault<int>(null, 0));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => AsyncEnumerable.ElementAtOrDefault<int>(AsyncEnumerable.Return(42), -1));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ElementAtOrDefault<int>(null, 0, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => AsyncEnumerable.ElementAtOrDefault<int>(AsyncEnumerable.Return(42), -1, CancellationToken.None));
        }

        [Fact]
        public void ElementAtOrDefault1()
        {
            var res = AsyncEnumerable.Empty<int>().ElementAtOrDefault(0);
            Assert.Equal(0, res.Result);
        }

        [Fact]
        public void ElementAtOrDefault2()
        {
            var res = AsyncEnumerable.Return<int>(42).ElementAtOrDefault(0);
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void ElementAtOrDefault3()
        {
            var res = AsyncEnumerable.Return<int>(42).ElementAtOrDefault(1);
            Assert.Equal(0, res.Result);
        }

        [Fact]
        public void ElementAtOrDefault4()
        {
            var res = new[] { 1, 42, 3 }.ToAsyncEnumerable().ElementAtOrDefault(1);
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void ElementAtOrDefault5()
        {
            var res = new[] { 1, 42, 3 }.ToAsyncEnumerable().ElementAtOrDefault(7);
            Assert.Equal(0, res.Result);
        }

        [Fact]
        public void ElementAtOrDefault6()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).ElementAtOrDefault(15);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public async Task ToList_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToList<int>(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToList<int>(null, CancellationToken.None));
        }

        [Fact]
        public void ToList1()
        {
            var xs = new[] { 42, 25, 39 };
            var res = xs.ToAsyncEnumerable().ToList();
            Assert.True(res.Result.SequenceEqual(xs));
        }

        [Fact]
        public void ToList2()
        {
            var xs = AsyncEnumerable.Empty<int>();
            var res = xs.ToList();
            Assert.True(res.Result.Count == 0);
        }

        [Fact]
        public void ToList3()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).ToList();
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public async Task ToArray_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToArray<int>(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToArray<int>(null, CancellationToken.None));
        }

        [Fact]
        public void ToArray1()
        {
            var xs = new[] { 42, 25, 39 };
            var res = xs.ToAsyncEnumerable().ToArray();
            Assert.True(res.Result.SequenceEqual(xs));
        }

        [Fact]
        public void ToArray2()
        {
            var xs = AsyncEnumerable.Empty<int>();
            var res = xs.ToArray();
            Assert.True(res.Result.Length == 0);
        }

        [Fact]
        public void ToArray3()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).ToArray();
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public async Task ToArray4()
        {
            var xs = await AsyncEnumerable.Range(5,50).Take(10).ToArray();
            var ex = new[] { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };

            Assert.True(ex.SequenceEqual(xs));
        }

        [Fact]
        public async Task ToArray5()
        {
            var res = new[] { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
            var xs = new HashSet<int>(res);

            var arr = await xs.ToAsyncEnumerable().ToArray();
            

            Assert.True(res.SequenceEqual(arr));
        }

        [Fact]
        public async Task ToDictionary_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int>(null, x => 0));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int>(AsyncEnumerable.Return(42), null));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int>(null, x => 0, EqualityComparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int>(AsyncEnumerable.Return(42), null, EqualityComparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int>(AsyncEnumerable.Return(42), x => 0, null));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(null, x => 0, x => 0));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(AsyncEnumerable.Return(42), null, x => 0));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(AsyncEnumerable.Return(42), x => 0, null));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(null, x => 0, x => 0, EqualityComparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(AsyncEnumerable.Return(42), null, x => 0, EqualityComparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(AsyncEnumerable.Return(42), x => 0, null, EqualityComparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(AsyncEnumerable.Return(42), x => 0, x => 0, null));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int>(null, x => 0, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int>(AsyncEnumerable.Return(42), null, CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int>(null, x => 0, EqualityComparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int>(AsyncEnumerable.Return(42), null, EqualityComparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int>(AsyncEnumerable.Return(42), x => 0, null, CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(null, x => 0, x => 0, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(AsyncEnumerable.Return(42), null, x => 0, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(AsyncEnumerable.Return(42), x => 0, null, CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(null, x => 0, x => 0, EqualityComparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(AsyncEnumerable.Return(42), null, x => 0, EqualityComparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(AsyncEnumerable.Return(42), x => 0, null, EqualityComparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(AsyncEnumerable.Return(42), x => 0, x => 0, null, CancellationToken.None));
        }

        [Fact]
        public void ToDictionary1()
        {
            var xs = new[] { 1, 4 }.ToAsyncEnumerable();
            var res = xs.ToDictionary(x => x % 2).Result;
            Assert.True(res[0] == 4);
            Assert.True(res[1] == 1);
        }

        [Fact]
        public void ToDictionary2()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            AssertThrows<Exception>(() => xs.ToDictionary(x => x % 2).Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is ArgumentException);
        }

        [Fact]
        public void ToDictionary3()
        {
            var xs = new[] { 1, 4 }.ToAsyncEnumerable();
            var res = xs.ToDictionary(x => x % 2, x => x + 1).Result;
            Assert.True(res[0] == 5);
            Assert.True(res[1] == 2);
        }

        [Fact]
        public void ToDictionary4()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            AssertThrows<Exception>(() => xs.ToDictionary(x => x % 2, x => x + 1).Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is ArgumentException);
        }

        [Fact]
        public void ToDictionary5()
        {
            var xs = new[] { 1, 4 }.ToAsyncEnumerable();
            var res = xs.ToDictionary(x => x % 2, new Eq()).Result;
            Assert.True(res[0] == 4);
            Assert.True(res[1] == 1);
        }

        [Fact]
        public void ToDictionary6()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            AssertThrows<Exception>(() => xs.ToDictionary(x => x % 2, new Eq()).Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is ArgumentException);
        }

        [Fact]
        public void ToDictionary7()
        {
            var xs = new[] { 1, 4 }.ToAsyncEnumerable();
            var res = xs.ToDictionary(x => x % 2, x => x, new Eq()).Result;
            Assert.True(res[0] == 4);
            Assert.True(res[1] == 1);
        }

        [Fact]
        public async Task ToLookup_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int>(null, x => 0));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int>(AsyncEnumerable.Return(42), null));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int>(null, x => 0, EqualityComparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int>(AsyncEnumerable.Return(42), null, EqualityComparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int>(AsyncEnumerable.Return(42), x => 0, null));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(null, x => 0, x => 0));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(AsyncEnumerable.Return(42), null, x => 0));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(AsyncEnumerable.Return(42), x => 0, null));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(null, x => 0, x => 0, EqualityComparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(AsyncEnumerable.Return(42), null, x => 0, EqualityComparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(AsyncEnumerable.Return(42), x => 0, null, EqualityComparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(AsyncEnumerable.Return(42), x => 0, x => 0, null));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int>(null, x => 0, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int>(AsyncEnumerable.Return(42), null, CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int>(null, x => 0, EqualityComparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int>(AsyncEnumerable.Return(42), null, EqualityComparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int>(AsyncEnumerable.Return(42), x => 0, null, CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(null, x => 0, x => 0, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(AsyncEnumerable.Return(42), null, x => 0, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(AsyncEnumerable.Return(42), x => 0, null, CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(null, x => 0, x => 0, EqualityComparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(AsyncEnumerable.Return(42), null, x => 0, EqualityComparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(AsyncEnumerable.Return(42), x => 0, null, EqualityComparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(AsyncEnumerable.Return(42), x => 0, x => 0, null, CancellationToken.None));
        }

        [Fact]
        public void ToLookup1()
        {
            var xs = new[] { 1, 4 }.ToAsyncEnumerable();
            var res = xs.ToLookup(x => x % 2).Result;
            Assert.True(res.Contains(0));
            Assert.True(res.Contains(1));
            Assert.True(res[0].Contains(4));
            Assert.True(res[1].Contains(1));
            Assert.True(res.Count == 2);
        }

        [Fact]
        public void ToLookup2()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            var res = xs.ToLookup(x => x % 2).Result;
            Assert.True(res.Contains(0));
            Assert.True(res.Contains(1));
            Assert.True(res[0].Contains(4));
            Assert.True(res[0].Contains(2));
            Assert.True(res[1].Contains(1));
            Assert.True(res.Count == 2);
        }

        [Fact]
        public void ToLookup3()
        {
            var xs = new[] { 1, 4 }.ToAsyncEnumerable();
            var res = xs.ToLookup(x => x % 2, x => x + 1).Result;
            Assert.True(res.Contains(0));
            Assert.True(res.Contains(1));
            Assert.True(res[0].Contains(5));
            Assert.True(res[1].Contains(2));
            Assert.True(res.Count == 2);
        }

        [Fact]
        public void ToLookup4()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            var res = xs.ToLookup(x => x % 2, x => x + 1).Result;
            Assert.True(res.Contains(0));
            Assert.True(res.Contains(1));
            Assert.True(res[0].Contains(5));
            Assert.True(res[0].Contains(3));
            Assert.True(res[1].Contains(2));
            Assert.True(res.Count == 2);
        }

        [Fact]
        public void ToLookup5()
        {
            var xs = new[] { 1, 4 }.ToAsyncEnumerable();
            var res = xs.ToLookup(x => x % 2, new Eq()).Result;
            Assert.True(res.Contains(0));
            Assert.True(res.Contains(1));
            Assert.True(res[0].Contains(4));
            Assert.True(res[1].Contains(1));
            Assert.True(res.Count == 2);
        }

        [Fact]
        public void ToLookup6()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            var res = xs.ToLookup(x => x % 2, new Eq()).Result;
            Assert.True(res.Contains(0));
            Assert.True(res.Contains(1));
            Assert.True(res[0].Contains(4));
            Assert.True(res[0].Contains(2));
            Assert.True(res[1].Contains(1));
            Assert.True(res.Count == 2);
        }

        [Fact]
        public void ToLookup7()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            var res = xs.ToLookup(x => x % 2).Result;
            foreach (var g in res)
                Assert.True(g.Key == 0 || g.Key == 1);
        }

        [Fact]
        public void ToLookup8()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            var res = xs.ToLookup(x => x % 2).Result;
#pragma warning disable IDE0007 // Use implicit type
            foreach (IGrouping<int, int> g in (IEnumerable)res)
                Assert.True(g.Key == 0 || g.Key == 1);
#pragma warning restore IDE0007 // Use implicit type
        }

        [Fact]
        public void ToLookup9()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            var res = xs.ToLookup(x => x % 2, x => x, new Eq()).Result;
            Assert.True(res.Contains(0));
            Assert.True(res.Contains(1));
            Assert.True(res[0].Contains(4));
            Assert.True(res[0].Contains(2));
            Assert.True(res[1].Contains(1));
            Assert.True(res.Count == 2);
        }

        [Fact]
        public async Task Average_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<int>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<int?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<long>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<long?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<double>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<double?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<float>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<float?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<decimal>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<decimal?>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<int>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<int?>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<long>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<long?>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<double>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<double?>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<float>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<float?>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<decimal>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<decimal?>), x => x));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, int>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, int?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, long>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, long?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, double>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, double?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, float>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, float?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, decimal>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, decimal?>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<int>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<int?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<long>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<long?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<double>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<double?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<float>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<float?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<decimal>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<decimal?>), CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<int>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<int?>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<long>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<long?>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<double>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<double?>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<float>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<float?>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<decimal>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<decimal?>), x => x, CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, int>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, int?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, long>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, long?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, double>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, double?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, float>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, float?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, decimal>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, decimal?>), CancellationToken.None));
        }

        [Fact]
        public void Average1()
        {
            var xs = new[] { 1, 2, 3 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), ys.Average().Result);
            Assert.Equal(xs.Average(), ys.Average(x => x).Result);
        }

        [Fact]
        public void Average2()
        {
            var xs = new[] { 1, default(int?), 3 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), ys.Average().Result);
            Assert.Equal(xs.Average(), ys.Average(x => x).Result);
        }

        [Fact]
        public void Average3()
        {
            var xs = new[] { 1L, 2L, 3L };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), ys.Average().Result);
            Assert.Equal(xs.Average(), ys.Average(x => x).Result);
        }

        [Fact]
        public void Average4()
        {
            var xs = new[] { 1L, default(long?), 3L };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), ys.Average().Result);
            Assert.Equal(xs.Average(), ys.Average(x => x).Result);
        }

        [Fact]
        public void Average5()
        {
            var xs = new[] { 1.0, 2.0, 3.0 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), ys.Average().Result);
            Assert.Equal(xs.Average(), ys.Average(x => x).Result);
        }

        [Fact]
        public void Average6()
        {
            var xs = new[] { 1.0, default(double?), 3.0 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), ys.Average().Result);
            Assert.Equal(xs.Average(), ys.Average(x => x).Result);
        }

        [Fact]
        public void Average7()
        {
            var xs = new[] { 1.0f, 2.0f, 3.0f };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), ys.Average().Result);
            Assert.Equal(xs.Average(), ys.Average(x => x).Result);
        }

        [Fact]
        public void Average8()
        {
            var xs = new[] { 1.0f, default(float?), 3.0f };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), ys.Average().Result);
            Assert.Equal(xs.Average(), ys.Average(x => x).Result);
        }

        [Fact]
        public void Average9()
        {
            var xs = new[] { 1.0m, 2.0m, 3.0m };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), ys.Average().Result);
            Assert.Equal(xs.Average(), ys.Average(x => x).Result);
        }

        [Fact]
        public void Average10()
        {
            var xs = new[] { 1.0m, default(decimal?), 3.0m };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), ys.Average().Result);
            Assert.Equal(xs.Average(), ys.Average(x => x).Result);
        }

        [Fact]
        public void Average11()
        {
            var xs = new int[0];
            var ys = xs.ToAsyncEnumerable();
            AssertThrows<Exception>(() => ys.Average().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void Average12()
        {
            var xs = new int?[0];
            var ys = xs.ToAsyncEnumerable();
            Assert.Null(ys.Average().Result);
        }

        [Fact]
        public void Average13()
        {
            var xs = new long[0];
            var ys = xs.ToAsyncEnumerable();
            AssertThrows<Exception>(() => ys.Average().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void Average14()
        {
            var xs = new long?[0];
            var ys = xs.ToAsyncEnumerable();
            Assert.Null(ys.Average().Result);
        }

        [Fact]
        public void Average15()
        {
            var xs = new double[0];
            var ys = xs.ToAsyncEnumerable();
            AssertThrows<Exception>(() => ys.Average().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void Average16()
        {
            var xs = new double?[0];
            var ys = xs.ToAsyncEnumerable();
            Assert.Null(ys.Average().Result);
        }

        [Fact]
        public void Average17()
        {
            var xs = new float[0];
            var ys = xs.ToAsyncEnumerable();
            AssertThrows<Exception>(() => ys.Average().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void Average18()
        {
            var xs = new float?[0];
            var ys = xs.ToAsyncEnumerable();
            Assert.Null(ys.Average().Result);
        }

        [Fact]
        public void Average19()
        {
            var xs = new decimal[0];
            var ys = xs.ToAsyncEnumerable();
            AssertThrows<Exception>(() => ys.Average().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void Average20()
        {
            var xs = new decimal?[0];
            var ys = xs.ToAsyncEnumerable();
            Assert.Null(ys.Average().Result);
        }

        [Fact]
        public async Task Min_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<int>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<int?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<long>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<long?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<double>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<double?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<float>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<float?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<decimal>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<decimal?>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<int>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<int?>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<long>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<long?>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<double>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<double?>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<float>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<float?>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<decimal>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<decimal?>), x => x));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, int>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, int?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, long>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, long?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, double>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, double?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, float>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, float?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, decimal>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, decimal?>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<DateTime>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<DateTime>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<DateTime>(), default(Func<DateTime, bool>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<int>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<int?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<long>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<long?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<double>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<double?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<float>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<float?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<decimal>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<decimal?>), CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<int>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<int?>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<long>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<long?>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<double>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<double?>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<float>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<float?>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<decimal>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<decimal?>), x => x, CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, int>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, int?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, long>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, long?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, double>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, double?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, float>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, float?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, decimal>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, decimal?>), CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<DateTime>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<DateTime>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<DateTime>(), default(Func<DateTime, bool>), CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<DateTime>), Comparer<DateTime>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<DateTime>(), default(IComparer<DateTime>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<DateTime>), Comparer<DateTime>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<DateTime>(), default(IComparer<DateTime>), CancellationToken.None));
        }

        [Fact]
        public void Min1()
        {
            var xs = new[] { 2, 1, 3 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), ys.Min().Result);
            Assert.Equal(xs.Min(), ys.Min(x => x).Result);
        }

        [Fact]
        public void Min2()
        {
            var xs = new[] { 2, default(int?), 3 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), ys.Min().Result);
            Assert.Equal(xs.Min(), ys.Min(x => x).Result);
        }

        [Fact]
        public void Min3()
        {
            var xs = new[] { 2L, 1L, 3L };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), ys.Min().Result);
            Assert.Equal(xs.Min(), ys.Min(x => x).Result);
        }

        [Fact]
        public void Min4()
        {
            var xs = new[] { 2L, default(long?), 3L };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), ys.Min().Result);
            Assert.Equal(xs.Min(), ys.Min(x => x).Result);
        }

        [Fact]
        public void Min5()
        {
            var xs = new[] { 2.0, 1.0, 3.0 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), ys.Min().Result);
            Assert.Equal(xs.Min(), ys.Min(x => x).Result);
        }

        [Fact]
        public void Min6()
        {
            var xs = new[] { 2.0, default(double?), 3.0 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), ys.Min().Result);
            Assert.Equal(xs.Min(), ys.Min(x => x).Result);
        }

        [Fact]
        public void Min7()
        {
            var xs = new[] { 2.0f, 1.0f, 3.0f };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), ys.Min().Result);
            Assert.Equal(xs.Min(), ys.Min(x => x).Result);
        }

        [Fact]
        public void Min8()
        {
            var xs = new[] { 2.0f, default(float?), 3.0f };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), ys.Min().Result);
            Assert.Equal(xs.Min(), ys.Min(x => x).Result);
        }

        [Fact]
        public void Min9()
        {
            var xs = new[] { 2.0m, 1.0m, 3.0m };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), ys.Min().Result);
            Assert.Equal(xs.Min(), ys.Min(x => x).Result);
        }

        [Fact]
        public void Min10()
        {
            var xs = new[] { 2.0m, default(decimal?), 3.0m };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), ys.Min().Result);
            Assert.Equal(xs.Min(), ys.Min(x => x).Result);
        }

        [Fact]
        public void Min11()
        {
            var xs = new[] { DateTime.Now.AddDays(1), DateTime.Now.Subtract(TimeSpan.FromDays(1)), DateTime.Now.AddDays(2), DateTime.Now };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), ys.Min().Result);
            Assert.Equal(xs.Min(), ys.Min(x => x).Result);
        }

        [Fact]
        public async Task Max_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<int>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<int?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<long>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<long?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<double>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<double?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<float>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<float?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<decimal>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<decimal?>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<int>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<int?>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<long>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<long?>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<double>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<double?>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<float>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<float?>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<decimal>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<decimal?>), x => x));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, int>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, int?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, long>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, long?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, double>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, double?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, float>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, float?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, decimal>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, decimal?>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<DateTime>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<DateTime>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<DateTime>(), default(Func<DateTime, bool>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<int>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<int?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<long>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<long?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<double>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<double?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<float>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<float?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<decimal>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<decimal?>), CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<int>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<int?>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<long>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<long?>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<double>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<double?>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<float>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<float?>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<decimal>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<decimal?>), x => x, CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, int>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, int?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, long>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, long?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, double>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, double?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, float>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, float?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, decimal>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, decimal?>), CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<DateTime>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<DateTime>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<DateTime>(), default(Func<DateTime, bool>), CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<DateTime>), Comparer<DateTime>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<DateTime>(), default(IComparer<DateTime>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<DateTime>), Comparer<DateTime>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<DateTime>(), default(IComparer<DateTime>), CancellationToken.None));
        }

        [Fact]
        public void Max1()
        {
            var xs = new[] { 2, 7, 3 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Max(), ys.Max().Result);
            Assert.Equal(xs.Max(), ys.Max(x => x).Result);
        }

        [Fact]
        public void Max2()
        {
            var xs = new[] { 2, default(int?), 3, 1 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Max(), ys.Max().Result);
            Assert.Equal(xs.Max(), ys.Max(x => x).Result);
        }

        [Fact]
        public void Max3()
        {
            var xs = new[] { 2L, 7L, 3L };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Max(), ys.Max().Result);
            Assert.Equal(xs.Max(), ys.Max(x => x).Result);
        }

        [Fact]
        public void Max4()
        {
            var xs = new[] { 2L, default(long?), 3L, 1L };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Max(), ys.Max().Result);
            Assert.Equal(xs.Max(), ys.Max(x => x).Result);
        }

        [Fact]
        public void Max5()
        {
            var xs = new[] { 2.0, 7.0, 3.0 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Max(), ys.Max().Result);
            Assert.Equal(xs.Max(), ys.Max(x => x).Result);
        }

        [Fact]
        public void Max6()
        {
            var xs = new[] { 2.0, default(double?), 3.0, 1.0 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Max(), ys.Max().Result);
            Assert.Equal(xs.Max(), ys.Max(x => x).Result);
        }

        [Fact]
        public void Max7()
        {
            var xs = new[] { 2.0f, 7.0f, 3.0f };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Max(), ys.Max().Result);
            Assert.Equal(xs.Max(), ys.Max(x => x).Result);
        }

        [Fact]
        public void Max8()
        {
            var xs = new[] { 2.0f, default(float?), 3.0f, 1.0f };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Max(), ys.Max().Result);
            Assert.Equal(xs.Max(), ys.Max(x => x).Result);
        }

        [Fact]
        public void Max9()
        {
            var xs = new[] { 2.0m, 7.0m, 3.0m };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Max(), ys.Max().Result);
            Assert.Equal(xs.Max(), ys.Max(x => x).Result);
        }

        [Fact]
        public void Max10()
        {
            var xs = new[] { 2.0m, default(decimal?), 3.0m, 1.0m };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Max(), ys.Max().Result);
            Assert.Equal(xs.Max(), ys.Max(x => x).Result);
        }

        [Fact]
        public void Max11()
        {
            var xs = new[] { DateTime.Now.AddDays(1), DateTime.Now.Subtract(TimeSpan.FromDays(1)), DateTime.Now.AddDays(2), DateTime.Now };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Max(), ys.Max().Result);
            Assert.Equal(xs.Max(), ys.Max(x => x).Result);
        }

        [Fact]
        public async Task Sum_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<int>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<int?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<long>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<long?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<double>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<double?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<float>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<float?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<decimal>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<decimal?>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<int>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<int?>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<long>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<long?>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<double>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<double?>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<float>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<float?>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<decimal>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<decimal?>), x => x));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, int>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, int?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, long>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, long?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, double>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, double?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, float>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, float?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, decimal>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, decimal?>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<int>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<int?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<long>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<long?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<double>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<double?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<float>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<float?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<decimal>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<decimal?>), CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<int>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<int?>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<long>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<long?>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<double>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<double?>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<float>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<float?>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<decimal>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<decimal?>), x => x, CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, int>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, int?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, long>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, long?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, double>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, double?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, float>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, float?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, decimal>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, decimal?>), CancellationToken.None));
        }

        [Fact]
        public void Sum1()
        {
            var xs = new[] { 1, 2, 3 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Sum(), ys.Sum().Result);
            Assert.Equal(xs.Sum(), ys.Sum(x => x).Result);
        }

        [Fact]
        public void Sum2()
        {
            var xs = new[] { 1, default(int?), 3 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Sum(), ys.Sum().Result);
            Assert.Equal(xs.Sum(), ys.Sum(x => x).Result);
        }

        [Fact]
        public void Sum3()
        {
            var xs = new[] { 1L, 2L, 3L };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Sum(), ys.Sum().Result);
            Assert.Equal(xs.Sum(), ys.Sum(x => x).Result);
        }

        [Fact]
        public void Sum4()
        {
            var xs = new[] { 1L, default(long?), 3L };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Sum(), ys.Sum().Result);
            Assert.Equal(xs.Sum(), ys.Sum(x => x).Result);
        }

        [Fact]
        public void Sum5()
        {
            var xs = new[] { 1.0, 2.0, 3.0 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Sum(), ys.Sum().Result);
            Assert.Equal(xs.Sum(), ys.Sum(x => x).Result);
        }

        [Fact]
        public void Sum6()
        {
            var xs = new[] { 1.0, default(double?), 3.0 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Sum(), ys.Sum().Result);
            Assert.Equal(xs.Sum(), ys.Sum(x => x).Result);
        }

        [Fact]
        public void Sum7()
        {
            var xs = new[] { 1.0f, 2.0f, 3.0f };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Sum(), ys.Sum().Result);
            Assert.Equal(xs.Sum(), ys.Sum(x => x).Result);
        }

        [Fact]
        public void Sum8()
        {
            var xs = new[] { 1.0f, default(float?), 3.0f };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Sum(), ys.Sum().Result);
            Assert.Equal(xs.Sum(), ys.Sum(x => x).Result);
        }

        [Fact]
        public void Sum9()
        {
            var xs = new[] { 1.0m, 2.0m, 3.0m };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Sum(), ys.Sum().Result);
            Assert.Equal(xs.Sum(), ys.Sum(x => x).Result);
        }

        [Fact]
        public void Sum10()
        {
            var xs = new[] { 1.0m, default(decimal?), 3.0m };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Sum(), ys.Sum().Result);
            Assert.Equal(xs.Sum(), ys.Sum(x => x).Result);
        }

        [Fact]
        public async Task MinBy_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinBy(default(IAsyncEnumerable<int>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinBy(AsyncEnumerable.Return(42), default(Func<int, int>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinBy(default(IAsyncEnumerable<int>), x => x, Comparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinBy(AsyncEnumerable.Return(42), default(Func<int, int>), Comparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinBy(AsyncEnumerable.Return(42), x => x, default(IComparer<int>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinBy(default(IAsyncEnumerable<int>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinBy(AsyncEnumerable.Return(42), default(Func<int, int>), CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinBy(default(IAsyncEnumerable<int>), x => x, Comparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinBy(AsyncEnumerable.Return(42), default(Func<int, int>), Comparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinBy(AsyncEnumerable.Return(42), x => x, default(IComparer<int>), CancellationToken.None));
        }

        [Fact]
        public void MinBy1()
        {
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().MinBy(x => x / 2);
            var res = xs.Result;

            Assert.True(res.SequenceEqual(new[] { 3, 2 }));
        }

        [Fact]
        public void MinBy2()
        {
            var xs = new int[0].ToAsyncEnumerable().MinBy(x => x / 2);

            AssertThrows<Exception>(() => xs.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void MinBy3()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().MinBy(x => { if (x == 3) throw ex; return x; });

            AssertThrows<Exception>(() => xs.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void MinBy4()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().MinBy(x => { if (x == 4) throw ex; return x; });

            AssertThrows<Exception>(() => xs.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void MinBy5()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().Concat(AsyncEnumerable.Throw<int>(ex)).MinBy(x => x, Comparer<int>.Default);

            AssertThrows<Exception>(() => xs.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public async Task MaxBy_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MaxBy(default(IAsyncEnumerable<int>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MaxBy(AsyncEnumerable.Return(42), default(Func<int, int>)));
                                                                      
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MaxBy(default(IAsyncEnumerable<int>), x => x, Comparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MaxBy(AsyncEnumerable.Return(42), default(Func<int, int>), Comparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MaxBy(AsyncEnumerable.Return(42), x => x, default(IComparer<int>)));
                                                                      
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MaxBy(default(IAsyncEnumerable<int>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MaxBy(AsyncEnumerable.Return(42), default(Func<int, int>), CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MaxBy(default(IAsyncEnumerable<int>), x => x, Comparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MaxBy(AsyncEnumerable.Return(42), default(Func<int, int>), Comparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MaxBy(AsyncEnumerable.Return(42), x => x, default(IComparer<int>), CancellationToken.None));
        }

        [Fact]
        public void MaxBy1()
        {
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().MaxBy(x => x / 2);
            var res = xs.Result;

            Assert.True(res.SequenceEqual(new[] { 7, 6 }));
        }

        [Fact]
        public void MaxBy2()
        {
            var xs = new int[0].ToAsyncEnumerable().MaxBy(x => x / 2);

            AssertThrows<Exception>(() => xs.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void MaxBy3()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().MaxBy(x => { if (x == 3) throw ex; return x; });

            AssertThrows<Exception>(() => xs.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void MaxBy4()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().MaxBy(x => { if (x == 4) throw ex; return x; });

            AssertThrows<Exception>(() => xs.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void MaxBy5()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().Concat(AsyncEnumerable.Throw<int>(ex)).MaxBy(x => x, Comparer<int>.Default);

            AssertThrows<Exception>(() => xs.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }
    }
}