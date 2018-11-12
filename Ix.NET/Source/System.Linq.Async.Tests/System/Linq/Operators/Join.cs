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
    public class Join : AsyncEnumerableTests
    {
        [Fact]
        public void Join_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(default, Return42, x => x, x => x, (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(Return42, default, x => x, x => x, (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(Return42, Return42, default, x => x, (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(Return42, Return42, x => x, default, (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(Return42, Return42, x => x, x => x, default));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(default, Return42, x => x, x => x, (x, y) => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(Return42, default, x => x, x => x, (x, y) => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(Return42, Return42, default, x => x, (x, y) => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(Return42, Return42, x => x, default, (x, y) => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(Return42, Return42, x => x, x => x, default, EqualityComparer<int>.Default));
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
            var xs = Throw<int>(ex);
            var ys = new[] { 0, 1, 2 }.ToAsyncEnumerable();

            var res = xs.Join(ys, x => x % 3, y => y % 3, (x, y) => x + y);

            var e = res.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
        }

        [Fact]
        public void Join6()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = Throw<int>(ex);

            var res = xs.Join(ys, x => x % 3, y => y % 3, (x, y) => x + y);

            var e = res.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
        }

        [Fact]
        public void Join7()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 0, 1, 2 }.ToAsyncEnumerable();

            var res = xs.Join(ys, x => { throw ex; }, y => y, (x, y) => x + y);

            var e = res.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
        }

        [Fact]
        public void Join8()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 0, 1, 2 }.ToAsyncEnumerable();

            var res = xs.Join(ys, x => x, y => { throw ex; }, (x, y) => x + y);

            var e = res.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
        }

        [Fact]
        public void Join9()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 0, 1, 2 }.ToAsyncEnumerable();

            var res = xs.Join<int, int, int, int>(ys, x => x, y => y, (x, y) => { throw ex; });

            var e = res.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
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
                new Customer { CustomerId = "ALFKI" },
                new Customer { CustomerId = "ANANT" },
                new Customer { CustomerId = "FISSA" },
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

        public class CustomerOrder : IEquatable<CustomerOrder>
        {
            public int OrderId { get; set; }
            public string CustomerId { get; set; }

            public bool Equals(CustomerOrder other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;
                return OrderId == other.OrderId && string.Equals(CustomerId, other.CustomerId);
            }

            public override bool Equals(object obj)
            {
                if (obj is null) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
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
        }
    }
}
