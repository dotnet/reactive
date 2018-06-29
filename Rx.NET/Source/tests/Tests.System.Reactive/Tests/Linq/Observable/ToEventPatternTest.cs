// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class ToEventPatternTest : ReactiveTest
    {

        [Fact]
        public void ToEventPattern_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToEventPattern<EventArgs>(null));
        }

        [Fact]
        public void ToEventPattern_IEvent()
        {
            var src = new Subject<EventPattern<EventArgs<int>>>();
            var evt = src.ToEventPattern();

            var snd = new object();

            var lst = new List<int>();
            var hnd = new EventHandler<EventArgs<int>>((s, e) =>
            {
                Assert.Same(snd, s);
                lst.Add(e.Value);
            });

            evt.OnNext += hnd;

            src.OnNext(new EventPattern<EventArgs<int>>(snd, new EventArgs<int>(42)));
            src.OnNext(new EventPattern<EventArgs<int>>(snd, new EventArgs<int>(43)));

            evt.OnNext -= hnd;

            src.OnNext(new EventPattern<EventArgs<int>>(snd, new EventArgs<int>(44)));

            Assert.True(lst.SequenceEqual(new[] { 42, 43 }));
        }

        [Fact]
        public void ToEventPattern_IEvent_Fails()
        {
            var src = new Subject<EventPattern<EventArgs<int>>>();
            var evt = src.ToEventPattern();

            var snd = new object();

            var lst = new List<int>();
            var hnd = new EventHandler<EventArgs<int>>((s, e) =>
            {
                Assert.Same(snd, s);
                lst.Add(e.Value);
            });

            evt.OnNext += hnd;

            src.OnNext(new EventPattern<EventArgs<int>>(snd, new EventArgs<int>(42)));
            src.OnNext(new EventPattern<EventArgs<int>>(snd, new EventArgs<int>(43)));

            var ex = new Exception();

            ReactiveAssert.Throws(ex, () => src.OnError(ex));

            Assert.True(lst.SequenceEqual(new[] { 42, 43 }));
        }

        [Fact]
        public void ToEventPattern_IEvent_Completes()
        {
            var src = new Subject<EventPattern<EventArgs<int>>>();
            var evt = src.ToEventPattern();

            var snd = new object();

            var lst = new List<int>();
            var hnd = new EventHandler<EventArgs<int>>((s, e) =>
            {
                Assert.Same(snd, s);
                lst.Add(e.Value);
            });

            evt.OnNext += hnd;

            src.OnNext(new EventPattern<EventArgs<int>>(snd, new EventArgs<int>(42)));
            src.OnNext(new EventPattern<EventArgs<int>>(snd, new EventArgs<int>(43)));

            src.OnCompleted();

            Assert.True(lst.SequenceEqual(new[] { 42, 43 }));
        }

        private class EventSrc
        {
            public event EventHandler<EventArgs<string>> E;

            public void On(string s)
            {
                E?.Invoke(this, new EventArgs<string>(s));
            }
        }

        private class EventArgs<T> : EventArgs
        {
            public T Value { get; private set; }

            public EventArgs(T value)
            {
                Value = value;
            }
        }

        [Fact]
        public void FromEventPattern_ToEventPattern()
        {
            var src = new EventSrc();
            var evt = Observable.FromEventPattern<EventHandler<EventArgs<string>>, EventArgs<string>>(h => new EventHandler<EventArgs<string>>(h), h => src.E += h, h => src.E -= h);

            var res = evt.ToEventPattern();

            var lst = new List<string>();
            var hnd = new EventHandler<EventArgs<string>>((s, e) =>
            {
                Assert.Same(src, s);
                lst.Add(e.Value);
            });

            src.On("bar");

            res.OnNext += hnd;

            src.On("foo");
            src.On("baz");

            res.OnNext -= hnd;

            src.On("qux");

            Assert.True(lst.SequenceEqual(new[] { "foo", "baz" }));
        }

        [Fact]
        public void ToEvent_DuplicateHandlers()
        {
            var src = new Subject<Unit>();
            var evt = src.ToEvent();

            var num = 0;
            var hnd = new Action<Unit>(e => num++);

            evt.OnNext += hnd;

            Assert.Equal(0, num);

            src.OnNext(new Unit());
            Assert.Equal(1, num);

            evt.OnNext += hnd;

            src.OnNext(new Unit());
            Assert.Equal(3, num);

            evt.OnNext -= hnd;

            src.OnNext(new Unit());
            Assert.Equal(4, num);

            evt.OnNext -= hnd;

            src.OnNext(new Unit());
            Assert.Equal(4, num);
        }

        [Fact]
        public void ToEvent_SourceCompletes()
        {
            var src = new Subject<Unit>();
            var evt = src.ToEvent();

            var num = 0;
            var hnd = new Action<Unit>(e => num++);

            evt.OnNext += hnd;

            Assert.Equal(0, num);

            src.OnNext(new Unit());
            Assert.Equal(1, num);

            src.OnNext(new Unit());
            Assert.Equal(2, num);

            src.OnCompleted();
            Assert.Equal(2, num);

            var tbl = GetSubscriptionTable(evt);
            Assert.True(tbl.Count == 0);
        }

        [Fact]
        public void ToEvent_SourceFails()
        {
            var src = new Subject<Unit>();
            var evt = src.ToEvent();

            var num = 0;
            var hnd = new Action<Unit>(e => num++);

            evt.OnNext += hnd;

            Assert.Equal(0, num);

            src.OnNext(new Unit());
            Assert.Equal(1, num);

            src.OnNext(new Unit());
            Assert.Equal(2, num);

            var ex = new Exception();

            ReactiveAssert.Throws(ex, () => src.OnError(ex));

            var tbl = GetSubscriptionTable(evt);
            Assert.True(tbl.Count == 0);
        }

        [Fact]
        public void ToEvent_DoneImmediately()
        {
            var src = Observable.Empty<Unit>();
            var evt = src.ToEvent();

            var num = 0;
            var hnd = new Action<Unit>(e => num++);

            for (var i = 0; i < 2; i++)
            {
                evt.OnNext += hnd;

                Assert.Equal(0, num);

                var tbl = GetSubscriptionTable(evt);
                Assert.True(tbl.Count == 0);
            }
        }

        [Fact]
        public void ToEvent_UnbalancedHandlers()
        {
            var src = new Subject<Unit>();
            var evt = src.ToEvent();

            var num = 0;
            var hnd = new Action<Unit>(e => num++);

            evt.OnNext += hnd;
            Assert.Equal(0, num);

            evt.OnNext -= hnd;
            Assert.Equal(0, num);

            evt.OnNext -= hnd;
            Assert.Equal(0, num);

            evt.OnNext += hnd;
            Assert.Equal(0, num);

            src.OnNext(new Unit());
            Assert.Equal(1, num);

            src.OnNext(new Unit());
            Assert.Equal(2, num);

            evt.OnNext -= hnd;
            Assert.Equal(2, num);

            src.OnNext(new Unit());
            Assert.Equal(2, num);
        }

        private static Dictionary<Delegate, Stack<IDisposable>> GetSubscriptionTable(object evt)
        {
            return (Dictionary<Delegate, Stack<IDisposable>>)evt.GetType().GetField("_subscriptions", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(evt);
        }

        [Fact]
        public void EventPattern_Equality()
        {
            var e1 = new EventPattern<string, EventArgs>("Bart", EventArgs.Empty);
            var e2 = new EventPattern<string, EventArgs>("Bart", EventArgs.Empty);

            Assert.True(e1.Equals(e1));
            Assert.True(e1.Equals(e2));
            Assert.True(e2.Equals(e1));
            Assert.True(e1 == e2);
            Assert.True(!(e1 != e2));
            Assert.True(e1.GetHashCode() == e2.GetHashCode());

            Assert.False(e1.Equals(null));
            Assert.False(e1.Equals("xy"));
            Assert.False(e1 == null);
        }

        [Fact]
        public void EventPattern_Inequality()
        {
            var a1 = new MyEventArgs();
            var a2 = new MyEventArgs();

            var e1 = new EventPattern<string, MyEventArgs>("Bart", a1);
            var e2 = new EventPattern<string, MyEventArgs>("John", a1);
            var e3 = new EventPattern<string, MyEventArgs>("Bart", a2);

            Assert.True(!e1.Equals(e2));
            Assert.True(!e2.Equals(e1));
            Assert.True(!(e1 == e2));
            Assert.True(e1 != e2);
            Assert.True(e1.GetHashCode() != e2.GetHashCode());

            Assert.True(!e1.Equals(e3));
            Assert.True(!e3.Equals(e1));
            Assert.True(!(e1 == e3));
            Assert.True(e1 != e3);
            Assert.True(e1.GetHashCode() != e3.GetHashCode());
        }

        private class MyEventArgs : EventArgs
        {
        }

    }
}
