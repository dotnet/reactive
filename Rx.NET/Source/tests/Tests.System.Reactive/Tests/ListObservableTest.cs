// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{

    public partial class ListObservableTest : ReactiveTest
    {
        [Fact]
        public void Ctor_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new ListObservable<int>(null));
        }

        [Fact]
        public void Subscribe_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new ListObservable<int>(Observable.Never<int>()).Subscribe(null));
        }

        [Fact]
        public void Value_None()
        {
            var o = new ListObservable<int>(Observable.Empty<int>());
            ReactiveAssert.Throws<InvalidOperationException>(() => { var t = o.Value; });
        }

        [Fact]
        public void Value_Some()
        {
            var o = new ListObservable<int>(Observable.Range(0, 10));
            Assert.Equal(9, o.Value);
        }

        [Fact]
        public void IndexOf_None()
        {
            var o = new ListObservable<int>(Observable.Empty<int>());
            Assert.Equal(-1, o.IndexOf(0));
        }

        [Fact]
        public void IndexOf_Some_NotFound()
        {
            var o = new ListObservable<int>(Observable.Range(0, 10));
            Assert.Equal(-1, o.IndexOf(100));
        }

        [Fact]
        public void IndexOf_Some_Found()
        {
            var o = new ListObservable<int>(Observable.Range(0, 10));
            Assert.Equal(3, o.IndexOf(3));
        }

        [Fact]
        public void RemoveAt_Some_NotFound()
        {
            var o = new ListObservable<int>(Observable.Range(0, 10));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => o.RemoveAt(100));
            o.AssertEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
        }

        [Fact]
        public void RemoveAt_Some_Found()
        {
            var o = new ListObservable<int>(Observable.Range(0, 10));
            o.RemoveAt(3);
            o.AssertEqual(0, 1, 2, 4, 5, 6, 7, 8, 9);
        }

        [Fact]
        public void Insert_Invalid()
        {
            var o = new ListObservable<int>(Observable.Range(0, 10));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => o.Insert(100, 100));
            o.AssertEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
        }

        [Fact]
        public void Insert_Invalid_2()
        {
            var o = new ListObservable<int>(Observable.Range(0, 10));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => o.Insert(-1, 100));
            o.AssertEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
        }

        [Fact]
        public void Insert_Beginning()
        {
            var o = new ListObservable<int>(Observable.Range(0, 10));
            o.Insert(0, -1);
            o.AssertEqual(-1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
        }

        [Fact]
        public void Insert_Middle()
        {
            var o = new ListObservable<int>(Observable.Range(0, 10));
            o.Insert(3, -1);
            o.AssertEqual(0, 1, 2, -1, 3, 4, 5, 6, 7, 8, 9);
        }

        [Fact]
        public void Change_Beginning()
        {
            var o = new ListObservable<int>(Observable.Range(0, 10))
            {
                [0] = -1
            };
            o.AssertEqual(-1, 1, 2, 3, 4, 5, 6, 7, 8, 9);
        }

        [Fact]
        public void Change_Middle()
        {
            var o = new ListObservable<int>(Observable.Range(0, 10))
            {
                [5] = -1
            };
            o.AssertEqual(0, 1, 2, 3, 4, -1, 6, 7, 8, 9);
        }

        [Fact]
        public void Change_End()
        {
            var o = new ListObservable<int>(Observable.Range(0, 10))
            {
                [9] = -1
            };
            o.AssertEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, -1);
        }

        [Fact]
        public void Change_Error()
        {
            var o = new ListObservable<int>(Observable.Range(0, 10));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => o[100] = -1);
        }

        [Fact]
        public void Insert_End()
        {
            var o = new ListObservable<int>(Observable.Range(0, 10));
            o.Insert(10, -1);
            o.AssertEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, -1);
        }

        [Fact]
        public void Contains_None()
        {
            var o = new ListObservable<int>(Observable.Empty<int>());
            Assert.Equal(false, o.Contains(0));
        }

        [Fact]
        public void Contains_Some_NotFound()
        {
            var o = new ListObservable<int>(Observable.Range(0, 10));
            Assert.Equal(false, o.Contains(100));
        }

        [Fact]
        public void Contains_Some_Found()
        {
            var o = new ListObservable<int>(Observable.Range(0, 10));
            Assert.Equal(true, o.Contains(3));
        }

        [Fact]
        public void Clear()
        {
            var o = new ListObservable<int>(Observable.Range(0, 10));
            o.Clear();
            o.AssertEqual();
        }

        [Fact]
        public void IsReadOnly()
        {
            var o = new ListObservable<int>(Observable.Never<int>());
            Assert.Equal(false, o.IsReadOnly);
        }

        [Fact]
        public void This_None()
        {
            var o = new ListObservable<int>(Observable.Empty<int>());
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => { var t = o[0]; });
        }

        [Fact]
        public void This_Some_NotFound()
        {
            var o = new ListObservable<int>(Observable.Range(0, 10));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => { var t = o[100]; });
        }

        [Fact]
        public void This_Some_Found()
        {
            var o = new ListObservable<int>(Observable.Range(0, 10));
            Assert.Equal(5, o[5]);
        }

        [Fact]
        public void CopyTo_RightSize()
        {
            var o = new ListObservable<int>(Observable.Range(0, 10));
            var array = new int[10];
            o.CopyTo(array, 0);
            array.AssertEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
        }

        [Fact]
        public void CopyTo_RightSize_Offset()
        {
            var o = new ListObservable<int>(Observable.Range(0, 10));
            var array = new int[10];
            ReactiveAssert.Throws<ArgumentException>(() => o.CopyTo(array, 3));
        }

        [Fact]
        public void CopyTo_Bigger()
        {
            var o = new ListObservable<int>(Observable.Range(0, 10));
            var array = new int[15];
            o.CopyTo(array, 0);
            array.AssertEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 0, 0, 0, 0);
        }

        [Fact]
        public void CopyTo_Bigger_Offset()
        {
            var o = new ListObservable<int>(Observable.Range(0, 10));
            var array = new int[15];
            o.CopyTo(array, 3);
            array.AssertEqual(0, 0, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 0);
        }

        [Fact]
        public void CopyTo_Smaller()
        {
            var o = new ListObservable<int>(Observable.Range(0, 10));
            var array = new int[5];
            ReactiveAssert.Throws<ArgumentException>(() => o.CopyTo(array, 0));
        }

        [Fact]
        public void CopyTo_Smaller_Offset()
        {
            var o = new ListObservable<int>(Observable.Range(0, 10));
            var array = new int[5];
            ReactiveAssert.Throws<ArgumentException>(() => o.CopyTo(array, 3));
        }

        [Fact]
        public void Add_Empty()
        {
            var o = new ListObservable<int>(Observable.Empty<int>())
            {
                100
            };
            o.AssertEqual(100);
        }

        [Fact]
        public void Add_Some()
        {
            var o = new ListObservable<int>(Observable.Return(200))
            {
                100
            };
            o.AssertEqual(200, 100);
        }

        [Fact]
        public void Remove_None()
        {
            var o = new ListObservable<int>(Observable.Empty<int>());
            Assert.Equal(false, o.Remove(0));
            o.AssertEqual();
        }

        [Fact]
        public void Remove_Some_NotFound()
        {
            var o = new ListObservable<int>(Observable.Range(0, 10));
            Assert.Equal(false, o.Remove(100));
            o.AssertEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
        }

        [Fact]
        public void Remove_Some_Found()
        {
            var o = new ListObservable<int>(Observable.Range(0, 10));
            Assert.Equal(true, o.Remove(3));
            o.AssertEqual(0, 1, 2, 4, 5, 6, 7, 8, 9);
        }

        [Fact]
        public void ForEach()
        {
            var o = new ListObservable<int>(Observable.Range(0, 10));
            var l = new List<int>();

            foreach (var x in o)
            {
                l.Add(x);
            }

            l.AssertEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
        }

        [Fact]
        public void ForEach_Old()
        {
            var o = new ListObservable<int>(Observable.Range(0, 10));
            var l = new List<int>();

            foreach (int x in (IEnumerable)o)
            {
                l.Add(x);
            }

            l.AssertEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
        }

        [Fact]
        public void Subscribe_Never()
        {
            var s = new TestScheduler();

            var xs = s.CreateHotObservable<int>(
            );

            var results = s.Start(() => new ListObservable<int>(xs));

            results.Messages.AssertEqual(
            );
        }

        [Fact]
        public void Subscribe_Infinite()
        {
            var s = new TestScheduler();

            var xs = s.CreateHotObservable(
                OnNext(300, 1)
            );

            var results = s.Start(() => new ListObservable<int>(xs));

            results.Messages.AssertEqual(
            );
        }

        [Fact]
        public void Subscribe_Error()
        {
            var s = new TestScheduler();

            var ex = new Exception();

            var xs = s.CreateHotObservable(
                OnNext(300, 1),
                OnError<int>(400, ex)
            );

            var results = s.Start(() => new ListObservable<int>(xs));

            results.Messages.AssertEqual(
                OnError<object>(400, ex)
            );
        }

        [Fact]
        public void Subscribe_Completed()
        {
            var s = new TestScheduler();

            var xs = s.CreateHotObservable(
                OnNext(300, 1),
                OnCompleted<int>(400)
            );

            var results = s.Start(() => new ListObservable<int>(xs));

            results.Messages.AssertEqual(
                OnCompleted<object>(400)
            );
        }

        [Fact]
        public void Subscribe_Disposed()
        {
            var s = new TestScheduler();

            var xs = s.CreateHotObservable(
                OnNext(300, 1),
                OnCompleted<int>(1100)
            );

            var results = s.Start(() => new ListObservable<int>(xs));

            results.Messages.AssertEqual(
            );
        }

        [Fact]
        public void Subscribe_Disposed_Multi()
        {
            var s = new TestScheduler();

            var xs = s.CreateHotObservable(
                OnNext(300, 1),
                OnCompleted<int>(400)
            );

            var o = new ListObservable<int>(xs);

            var results1 = s.CreateObserver<object>();
            var results2 = s.CreateObserver<object>();

            var d1 = o.Subscribe(results1);
            var d2 = o.Subscribe(results2);

            s.ScheduleAbsolute(350, () => d1.Dispose());
            s.ScheduleAbsolute(500, () => d2.Dispose());

            s.Start();

            results1.Messages.AssertEqual(
            );

            results2.Messages.AssertEqual(
            );
        }
    }
}
