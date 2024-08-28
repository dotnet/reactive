// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

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

#pragma warning disable CA1806 // (Unused new instance.) We expect the constructor to throw.
            ReactiveAssert.Throws<ArgumentNullException>(() => new MyEventPatternSource(null, (a, x) => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => new MyEventPatternSource(xs, null));
#pragma warning restore CA1806

            var e = new MyEventPatternSource(xs, (a, x) => { })
            {
                GetInvoke = h => (_, __) => { }
            };

            ReactiveAssert.Throws<ArgumentNullException>(() => e.OnNext += null);

            e.GetInvoke = h => null;
            ReactiveAssert.Throws<ArgumentNullException>(() => e.OnNext += (_, __) => { });
            e.GetInvoke = null;

            ReactiveAssert.Throws<ArgumentNullException>(() => e.OnNext -= null);
        }
    }

    internal class MyEventPatternSource : EventPatternSourceBase<object, EventArgs>
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
                Add(value, GetInvoke(value));
            }

            remove
            {
                Remove(value);
            }
        }
    }
}
