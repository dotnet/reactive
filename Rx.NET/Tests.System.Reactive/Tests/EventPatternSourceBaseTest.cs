// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Reactive;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class EventPatternSourceBaseTest
    {
        [TestMethod]
        public void ArgumentChecking()
        {
            var xs = Observable.Empty<EventPattern<object, EventArgs>>();

            ReactiveAssert.Throws<ArgumentNullException>(() => new MyEventPatternSource(null, (a, x) => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => new MyEventPatternSource(xs, null));

            var e = new MyEventPatternSource(xs, (a, x) => { });
            e.GetInvoke = h => (_, __) => { };

            ReactiveAssert.Throws<ArgumentNullException>(() => e.OnNext += null);

            e.GetInvoke = h => null;
            ReactiveAssert.Throws<ArgumentNullException>(() => e.OnNext += (_, __) => { });
            e.GetInvoke = null;

            ReactiveAssert.Throws<ArgumentNullException>(() => e.OnNext -= null);
        }
    }

    class MyEventPatternSource : EventPatternSourceBase<object, EventArgs>
    {
        public MyEventPatternSource(IObservable<EventPattern<object, EventArgs>> source, Action<Action<object, EventArgs>, EventPattern<object, EventArgs>> invokeHandler)
            : base(source, invokeHandler)
        {
        }

        public Func<EventHandler<EventArgs>, Action<object, EventArgs>> GetInvoke;

        public event EventHandler<EventArgs> OnNext
        {
            add
            {
                base.Add(value, GetInvoke(value));
            }

            remove
            {
                Remove(value);
            }
        }
    }
}
