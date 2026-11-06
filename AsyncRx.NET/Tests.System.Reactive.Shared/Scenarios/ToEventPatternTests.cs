// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared <c>ToEventPattern</c> scenarios, from Rx.NET's <c>ToEventPatternTest.cs</c>.
/// </summary>
/// <remarks>
/// 6 of the file's 12 tests. The event source comes from
/// <see cref="SharedReactiveTest.ToEventPattern{TEventArgs}"/> or
/// <see cref="SharedReactiveTest.ToEvent{T}"/>, the target's own operator over a subject the
/// scenario drives; the handler attachment and the assertions are the originals', since the
/// event-source types are System.Reactive's on both targets. Where the original asserts that
/// <c>OnError</c> on the subject throws the error to the caller, the scenario awaits the
/// subject's asynchronous call. Not here: <c>ToEventPattern_ArgumentChecking</c> (the
/// code-generated stratum); <c>ToEvent_SourceCompletes</c>, <c>ToEvent_SourceFails</c> and
/// <c>ToEvent_DoneImmediately</c>, which reflect into Rx.NET's private subscription table; and
/// the two <c>EventPattern_*</c> tests of the shared <c>EventPattern</c> type itself; those five
/// are in <c>Tests.System.Reactive.Shared.Rx</c> as the originals.
/// </remarks>
public abstract class ToEventPatternTests : SharedReactiveTest
{
    [TestMethod]
    public async Task ToEventPattern_IEvent()
    {
        var src = CreateSubject<EventPattern<EventArgs<int>>>();
        var evt = ToEventPattern(src);

        var snd = new object();

        var lst = new List<int>();
        var hnd = new EventHandler<EventArgs<int>>((s, e) =>
        {
            Assert.AreSame(snd, s);
            lst.Add(e.Value);
        });

        evt.OnNext += hnd;

        await src.OnNextAsync(new EventPattern<EventArgs<int>>(snd, new EventArgs<int>(42)));
        await src.OnNextAsync(new EventPattern<EventArgs<int>>(snd, new EventArgs<int>(43)));

        evt.OnNext -= hnd;

        await src.OnNextAsync(new EventPattern<EventArgs<int>>(snd, new EventArgs<int>(44)));

        Assert.IsTrue(lst.SequenceEqual([42, 43]));
    }

    [TestMethod]
    public async Task ToEventPattern_IEvent_Fails()
    {
        var src = CreateSubject<EventPattern<EventArgs<int>>>();
        var evt = ToEventPattern(src);

        var snd = new object();

        var lst = new List<int>();
        var hnd = new EventHandler<EventArgs<int>>((s, e) =>
        {
            Assert.AreSame(snd, s);
            lst.Add(e.Value);
        });

        evt.OnNext += hnd;

        await src.OnNextAsync(new EventPattern<EventArgs<int>>(snd, new EventArgs<int>(42)));
        await src.OnNextAsync(new EventPattern<EventArgs<int>>(snd, new EventArgs<int>(43)));

        var ex = new Exception();

        var thrown = await Assert.ThrowsExactlyAsync<Exception>(
            async () => await src.OnErrorAsync(ex));
        Assert.AreSame(ex, thrown);

        Assert.IsTrue(lst.SequenceEqual([42, 43]));
    }

    [TestMethod]
    public async Task ToEventPattern_IEvent_Completes()
    {
        var src = CreateSubject<EventPattern<EventArgs<int>>>();
        var evt = ToEventPattern(src);

        var snd = new object();

        var lst = new List<int>();
        var hnd = new EventHandler<EventArgs<int>>((s, e) =>
        {
            Assert.AreSame(snd, s);
            lst.Add(e.Value);
        });

        evt.OnNext += hnd;

        await src.OnNextAsync(new EventPattern<EventArgs<int>>(snd, new EventArgs<int>(42)));
        await src.OnNextAsync(new EventPattern<EventArgs<int>>(snd, new EventArgs<int>(43)));

        await src.OnCompletedAsync();

        Assert.IsTrue(lst.SequenceEqual([42, 43]));
    }

    [TestMethod]
    public void FromEventPattern_ToEventPattern()
    {
        var src = new EventSrc();
        var evt = Seq.FromEventPattern<EventHandler<EventArgs<string>>, EventArgs<string>>(
            h => new EventHandler<EventArgs<string>>(h),
            h => src.E += h,
            h => src.E -= h);

        var res = ToEventPattern(evt);

        var lst = new List<string>();
        var hnd = new EventHandler<EventArgs<string>>((s, e) =>
        {
            Assert.AreSame(src, s);
            lst.Add(e.Value);
        });

        src.On("bar");

        res.OnNext += hnd;

        src.On("foo");
        src.On("baz");

        res.OnNext -= hnd;

        src.On("qux");

        Assert.IsTrue(lst.SequenceEqual(["foo", "baz"]));
    }

    [TestMethod]
    public async Task ToEvent_DuplicateHandlers()
    {
        var src = CreateSubject<Unit>();
        var evt = ToEvent(src);

        var num = 0;
        var hnd = new Action<Unit>(e => num++);

        evt.OnNext += hnd;

        Assert.AreEqual(0, num);

        await src.OnNextAsync(new Unit());
        Assert.AreEqual(1, num);

        evt.OnNext += hnd;

        await src.OnNextAsync(new Unit());
        Assert.AreEqual(3, num);

        evt.OnNext -= hnd;

        await src.OnNextAsync(new Unit());
        Assert.AreEqual(4, num);

        evt.OnNext -= hnd;

        await src.OnNextAsync(new Unit());
        Assert.AreEqual(4, num);
    }

    [TestMethod]
    public async Task ToEvent_UnbalancedHandlers()
    {
        var src = CreateSubject<Unit>();
        var evt = ToEvent(src);

        var num = 0;
        var hnd = new Action<Unit>(e => num++);

        evt.OnNext += hnd;
        Assert.AreEqual(0, num);

        evt.OnNext -= hnd;
        Assert.AreEqual(0, num);

        evt.OnNext -= hnd;
        Assert.AreEqual(0, num);

        evt.OnNext += hnd;
        Assert.AreEqual(0, num);

        await src.OnNextAsync(new Unit());
        Assert.AreEqual(1, num);

        await src.OnNextAsync(new Unit());
        Assert.AreEqual(2, num);

        evt.OnNext -= hnd;
        Assert.AreEqual(2, num);

        await src.OnNextAsync(new Unit());
        Assert.AreEqual(2, num);
    }

    private class EventSrc
    {
        public event EventHandler<EventArgs<string>>? E;

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
}
