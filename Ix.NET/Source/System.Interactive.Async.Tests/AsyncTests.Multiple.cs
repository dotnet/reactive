// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public partial class AsyncTests
    {
        [Fact]
        public void Concat_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Concat<int>(null, AsyncEnumerable.Return(42)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Concat<int>(AsyncEnumerable.Return(42), null));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Concat<int>(default(IAsyncEnumerable<int>[])));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Concat<int>(default(IEnumerable<IAsyncEnumerable<int>>)));
        }

        [Fact]
        public void Concat1()
        {
            var ys = new[] { 1, 2, 3 }.ToAsyncEnumerable().Concat(new[] { 4, 5, 6 }.ToAsyncEnumerable());

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            HasNext(e, 5);
            HasNext(e, 6);
            NoNext(e);
        }

        [Fact]
        public void Concat2()
        {
            var ex = new Exception("Bang");
            var ys = new[] { 1, 2, 3 }.ToAsyncEnumerable().Concat(AsyncEnumerable.Throw<int>(ex));

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Concat3()
        {
            var ex = new Exception("Bang");
            var ys = AsyncEnumerable.Throw<int>(ex).Concat(new[] { 4, 5, 6 }.ToAsyncEnumerable());

            var e = ys.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Concat4()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5 }.ToAsyncEnumerable();
            var zs = new[] { 6, 7, 8 }.ToAsyncEnumerable();

            var res = AsyncEnumerable.Concat(xs, ys, zs);

            var e = res.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            HasNext(e, 5);
            HasNext(e, 6);
            HasNext(e, 7);
            HasNext(e, 8);
            NoNext(e);
        }

        [Fact]
        public void Concat5()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5 }.ToAsyncEnumerable();
            var zs = AsyncEnumerable.Throw<int>(ex);

            var res = AsyncEnumerable.Concat(xs, ys, zs);

            var e = res.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            HasNext(e, 5);
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Concat6()
        {
            var res = AsyncEnumerable.Concat(ConcatXss());

            var e = res.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            HasNext(e, 5);
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single().Message == "Bang!");
        }

        [Fact]
        public void Concat7()
        {
            var ws = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var xs = new[] { 4, 5 }.ToAsyncEnumerable();
            var ys = new[] { 6, 7, 8 }.ToAsyncEnumerable();
            var zs = new[] { 9, 10, 11 }.ToAsyncEnumerable();

            var res = ws.Concat(xs).Concat(ys).Concat(zs);

            var e = res.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            HasNext(e, 5);
            HasNext(e, 6);
            HasNext(e, 7);
            HasNext(e, 8);
            HasNext(e, 9);
            HasNext(e, 10);
            HasNext(e, 11);
            NoNext(e);
        }

        [Fact]
        public async Task Concat8()
        {
            var ws = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var xs = new[] { 4, 5 }.ToAsyncEnumerable();
            var ys = new[] { 6, 7, 8 }.ToAsyncEnumerable();
            var zs = new[] { 9, 10, 11 }.ToAsyncEnumerable();

            var res = ws.Concat(xs).Concat(ys).Concat(zs);

            await SequenceIdentity(res);
        }

        [Fact]
        public async Task Concat9()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5 }.ToAsyncEnumerable();
            var zs = new[] { 6, 7, 8 }.ToAsyncEnumerable();

            var res = AsyncEnumerable.Concat(xs, ys, zs);

            await SequenceIdentity(res);
        }

        [Fact]
        public async Task Concat10()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5 }.ToAsyncEnumerable();
            var zs = new[] { 6, 7, 8 }.ToAsyncEnumerable();

            var c = xs.Concat(ys).Concat(zs);

            var res = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            Assert.True(res.SequenceEqual(await c.ToArray()));
        }

        [Fact]
        public async Task Concat11()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5 }.ToAsyncEnumerable();
            var zs = new[] { 6, 7, 8 }.ToAsyncEnumerable();

            var c = xs.Concat(ys).Concat(zs);

            var res = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
            Assert.True(res.SequenceEqual(await c.ToList()));
        }

        [Fact]
        public async Task Concat12()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5 }.ToAsyncEnumerable();
            var zs = new[] { 6, 7, 8 }.ToAsyncEnumerable();

            var c = xs.Concat(ys).Concat(zs);

            Assert.Equal(8, await c.Count());
        }

        static IEnumerable<IAsyncEnumerable<int>> ConcatXss()
        {
            yield return new[] { 1, 2, 3 }.ToAsyncEnumerable();
            yield return new[] { 4, 5 }.ToAsyncEnumerable();
            throw new Exception("Bang!");
        }

        [Fact]
        public void GroupJoin_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(null, AsyncEnumerable.Return(42), x => x, x => x, (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(AsyncEnumerable.Return(42), null, x => x, x => x, (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), null, x => x, (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), x => x, null, (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), x => x, x => x, null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(null, AsyncEnumerable.Return(42), x => x, x => x, (x, y) => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(AsyncEnumerable.Return(42), null, x => x, x => x, (x, y) => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), null, x => x, (x, y) => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), x => x, null, (x, y) => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), x => x, x => x, null, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), x => x, x => x, (x, y) => x, default(IEqualityComparer<int>)));
        }

        [Fact]
        public void GroupJoin1()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 4, 7, 6, 2, 3, 4, 8, 9 }.ToAsyncEnumerable();

            var res = xs.GroupJoin(ys, x => x % 3, y => y % 3, (x, i) => x + " - " + i.Aggregate("", (s, j) => s + j).Result);

            var e = res.GetAsyncEnumerator();
            HasNext(e, "0 - 639");
            HasNext(e, "1 - 474");
            HasNext(e, "2 - 28");
            NoNext(e);
        }

        [Fact]
        public void GroupJoin2()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 3, 6, 4 }.ToAsyncEnumerable();

            var res = xs.GroupJoin(ys, x => x % 3, y => y % 3, (x, i) => x + " - " + i.Aggregate("", (s, j) => s + j).Result);

            var e = res.GetAsyncEnumerator();
            HasNext(e, "0 - 36");
            HasNext(e, "1 - 4");
            HasNext(e, "2 - ");
            NoNext(e);
        }

        [Fact]
        public void GroupJoin3()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Throw<int>(ex);
            var ys = new[] { 3, 6, 4 }.ToAsyncEnumerable();

            var res = xs.GroupJoin(ys, x => x % 3, y => y % 3, (x, i) => x + " - " + i.Aggregate("", (s, j) => s + j).Result);

            var e = res.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void GroupJoin4()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = AsyncEnumerable.Throw<int>(ex);

            var res = xs.GroupJoin(ys, x => x % 3, y => y % 3, (x, i) => x + " - " + i.Aggregate("", (s, j) => s + j).Result);

            var e = res.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void GroupJoin5()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 3, 6, 4 }.ToAsyncEnumerable();

            var res = xs.GroupJoin(ys, x => { throw ex; }, y => y % 3, (x, i) => x + " - " + i.Aggregate("", (s, j) => s + j).Result);

            var e = res.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void GroupJoin6()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 3, 6, 4 }.ToAsyncEnumerable();

            var res = xs.GroupJoin(ys, x => x % 3, y => { throw ex; }, (x, i) => x + " - " + i.Aggregate("", (s, j) => s + j).Result);

            var e = res.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void GroupJoin7()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 3, 6, 4 }.ToAsyncEnumerable();

            var res = xs.GroupJoin(ys, x => x % 3, y => y % 3, (x, i) =>
            {
                if (x == 1)
                    throw ex;
                return x + " - " + i.Aggregate("", (s, j) => s + j).Result;
            });

            var e = res.GetAsyncEnumerator();
            HasNext(e, "0 - 36");
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Join_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(null, AsyncEnumerable.Return(42), x => x, x => x, (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(AsyncEnumerable.Return(42), null, x => x, x => x, (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), null, x => x, (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), x => x, null, (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), x => x, x => x, null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(null, AsyncEnumerable.Return(42), x => x, x => x, (x, y) => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(AsyncEnumerable.Return(42), null, x => x, x => x, (x, y) => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), null, x => x, (x, y) => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), x => x, null, (x, y) => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), x => x, x => x, null, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), x => x, x => x, (x, y) => x, default(IEqualityComparer<int>)));
        }

        [Fact]
        public void Join1()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 3, 6, 4 }.ToAsyncEnumerable();

            var res = xs.Join(ys, x => x % 3, y => y % 3, (x, y) => x + y);

            var e = res.GetAsyncEnumerator();
            HasNext(e, 0 + 3);
            HasNext(e, 0 + 6);
            HasNext(e, 1 + 4);
            NoNext(e);
        }

        [Fact]
        public void Join2()
        {
            var xs = new[] { 3, 6, 4 }.ToAsyncEnumerable();
            var ys = new[] { 0, 1, 2 }.ToAsyncEnumerable();

            var res = xs.Join(ys, x => x % 3, y => y % 3, (x, y) => x + y);

            var e = res.GetAsyncEnumerator();
            HasNext(e, 3 + 0);
            HasNext(e, 6 + 0);
            HasNext(e, 4 + 1);
            NoNext(e);
        }

        [Fact]
        public void Join3()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 3, 6 }.ToAsyncEnumerable();

            var res = xs.Join(ys, x => x % 3, y => y % 3, (x, y) => x + y);

            var e = res.GetAsyncEnumerator();
            HasNext(e, 0 + 3);
            HasNext(e, 0 + 6);
            NoNext(e);
        }

        [Fact]
        public void Join4()
        {
            var xs = new[] { 3, 6 }.ToAsyncEnumerable();
            var ys = new[] { 0, 1, 2 }.ToAsyncEnumerable();

            var res = xs.Join(ys, x => x % 3, y => y % 3, (x, y) => x + y);

            var e = res.GetAsyncEnumerator();
            HasNext(e, 3 + 0);
            HasNext(e, 6 + 0);
            NoNext(e);
        }

        [Fact]
        public void Join5()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Throw<int>(ex);
            var ys = new[] { 0, 1, 2 }.ToAsyncEnumerable();

            var res = xs.Join(ys, x => x % 3, y => y % 3, (x, y) => x + y);

            var e = res.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Join6()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = AsyncEnumerable.Throw<int>(ex);

            var res = xs.Join(ys, x => x % 3, y => y % 3, (x, y) => x + y);

            var e = res.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Join7()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 0, 1, 2 }.ToAsyncEnumerable();

            var res = xs.Join(ys, x => { throw ex; }, y => y, (x, y) => x + y);

            var e = res.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Join8()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 0, 1, 2 }.ToAsyncEnumerable();

            var res = xs.Join(ys, x => x, y => { throw ex; }, (x, y) => x + y);

            var e = res.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Join9()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 0, 1, 2 }.ToAsyncEnumerable();

            var res = xs.Join<int, int, int, int>(ys, x => x, y => y, (x, y) => { throw ex; });

            var e = res.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public async Task Join10()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 3, 6, 4 }.ToAsyncEnumerable();

            var res = xs.Join(ys, x => x % 3, y => y % 3, (x, y) => x + y);

            await SequenceIdentity(res);
        }


        [Fact]
        public void Join11()
        {
            var customers = new List<Customer>
            {
                new Customer {CustomerId = "ALFKI"},
                new Customer {CustomerId = "ANANT"},
                new Customer {CustomerId = "FISSA"}
            };
            var orders = new List<Order>
            {
                new Order { OrderId = 1, CustomerId = "ALFKI"},
                new Order { OrderId = 2, CustomerId = "ALFKI"},
                new Order { OrderId = 3, CustomerId = "ALFKI"},
                new Order { OrderId = 4, CustomerId = "FISSA"},
                new Order { OrderId = 5, CustomerId = "FISSA"},
                new Order { OrderId = 6, CustomerId = "FISSA"},
            };

            var asyncResult = customers.ToAsyncEnumerable()
                                       .Join(orders.ToAsyncEnumerable(), c => c.CustomerId, o => o.CustomerId,
                                            (c, o) => new CustomerOrder { CustomerId = c.CustomerId, OrderId = o.OrderId });

            var e = asyncResult.GetAsyncEnumerator();
            HasNext(e, new CustomerOrder { CustomerId = "ALFKI", OrderId = 1 });
            HasNext(e, new CustomerOrder { CustomerId = "ALFKI", OrderId = 2 });
            HasNext(e, new CustomerOrder { CustomerId = "ALFKI", OrderId = 3 });
            HasNext(e, new CustomerOrder { CustomerId = "FISSA", OrderId = 4 });
            HasNext(e, new CustomerOrder { CustomerId = "FISSA", OrderId = 5 });
            HasNext(e, new CustomerOrder { CustomerId = "FISSA", OrderId = 6 });
            NoNext(e);
        }

        [Fact]
        public void Join12()
        {
            var customers = new List<Customer>
            {
                new Customer {CustomerId = "ANANT"},
                new Customer {CustomerId = "ALFKI"},
                new Customer {CustomerId = "FISSA"}
            };
            var orders = new List<Order>
            {
                new Order { OrderId = 1, CustomerId = "ALFKI"},
                new Order { OrderId = 2, CustomerId = "ALFKI"},
                new Order { OrderId = 3, CustomerId = "ALFKI"},
                new Order { OrderId = 4, CustomerId = "FISSA"},
                new Order { OrderId = 5, CustomerId = "FISSA"},
                new Order { OrderId = 6, CustomerId = "FISSA"},
            };

            var asyncResult = customers.ToAsyncEnumerable()
                                       .Join(orders.ToAsyncEnumerable(), c => c.CustomerId, o => o.CustomerId,
                                            (c, o) => new CustomerOrder { CustomerId = c.CustomerId, OrderId = o.OrderId });

            var e = asyncResult.GetAsyncEnumerator();
            HasNext(e, new CustomerOrder { CustomerId = "ALFKI", OrderId = 1 });
            HasNext(e, new CustomerOrder { CustomerId = "ALFKI", OrderId = 2 });
            HasNext(e, new CustomerOrder { CustomerId = "ALFKI", OrderId = 3 });
            HasNext(e, new CustomerOrder { CustomerId = "FISSA", OrderId = 4 });
            HasNext(e, new CustomerOrder { CustomerId = "FISSA", OrderId = 5 });
            HasNext(e, new CustomerOrder { CustomerId = "FISSA", OrderId = 6 });
            NoNext(e);
        }

        public class Customer
        {
            public string CustomerId { get; set; }
        }

        public class Order
        {
            public int OrderId { get; set; }
            public string CustomerId { get; set; }
        }

        [DebuggerDisplay("CustomerId = {CustomerId}, OrderId = {OrderId}")]
        public class CustomerOrder : IEquatable<CustomerOrder>
        {
            public bool Equals(CustomerOrder other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return OrderId == other.OrderId && string.Equals(CustomerId, other.CustomerId);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((CustomerOrder)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (OrderId * 397) ^ (CustomerId != null ? CustomerId.GetHashCode() : 0);
                }
            }

            public static bool operator ==(CustomerOrder left, CustomerOrder right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(CustomerOrder left, CustomerOrder right)
            {
                return !Equals(left, right);
            }

            public int OrderId { get; set; }
            public string CustomerId { get; set; }
        }
    }
}
