// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

// The Begin and End delegates are locals of delegate type, as the originals write them.
#pragma warning disable IDE0039 // Use local function

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared <c>FromAsyncPattern</c> scenarios, from Rx.NET's <c>FromAsyncPatternTest.cs</c>.
/// </summary>
/// <remarks>
/// 60 of the file's 61 tests (<c>FromAsyncPattern_ArgumentChecking</c> is the code-generated
/// stratum), all transcribed as they are: at every arity from zero to fourteen, the Begin/End
/// pair over a result and over nothing, an End that throws, and a Begin that throws after
/// calling back. <c>Seq.FromAsyncPattern(begin, end)</c> returns a function, as the original
/// does, so the call that follows reads as the original; where the original blocks on
/// <c>ToEnumerable</c>, the scenario awaits <see cref="SharedReactiveTest.ToListAsync{T}"/>. The
/// Begin delegates are declared with <c>object?</c> for the state parameter, as Rx.NET declares
/// them (the original has nullable annotations off), and the Begin delegates' assertions on
/// their arguments put the expected value first, as MSTest's analyzer asks, where the originals
/// put it second. The <c>Result</c> double is the original's.
/// </remarks>
public abstract class FromAsyncPatternTests : SharedReactiveTest
{
    [TestMethod]
    public async Task FromAsyncPattern0()
    {
        var x = new Result();

        Func<AsyncCallback, object?, IAsyncResult> begin = (cb, _) => { cb(x); return x; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 1; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)().Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnNext(1),
            Notification.CreateOnCompleted<int>()]));
    }

    [TestMethod]
    public async Task FromAsyncPatternAction0()
    {
        var x = new Result();

        Func<AsyncCallback, object?, IAsyncResult> begin = (cb, _) => { cb(x); return x; };
        Action<IAsyncResult> end = iar => { Assert.AreSame(x, iar); };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)())).ToArray();
        Assert.IsTrue(res.SequenceEqual([new Unit()]));
    }

    [TestMethod]
    public async Task FromAsyncPattern0_Error()
    {
        var x = new Result();
        var ex = new Exception();

        Func<AsyncCallback, object?, IAsyncResult> begin = (cb, _) => { cb(x); return x; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); throw ex; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)().Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task FromAsyncPattern0_ErrorBegin()
    {
        var x = new Result();
        var ex = new Exception();

        Func<AsyncCallback, object?, IAsyncResult> begin = (cb, _) => { cb(x); throw ex; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 0; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)().Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task FromAsyncPattern1()
    {
        var x = new Result();

        Func<int, AsyncCallback, object?, IAsyncResult> begin = (a, cb, _) =>
        {
            Assert.AreEqual(2, a);
            cb(x);
            return x;
        };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 1; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnNext(1),
            Notification.CreateOnCompleted<int>()]));
    }

    [TestMethod]
    public async Task FromAsyncPatternAction1()
    {
        var x = new Result();

        Func<int, AsyncCallback, object?, IAsyncResult> begin = (a, cb, _) =>
        {
            Assert.AreEqual(2, a);
            cb(x);
            return x;
        };
        Action<IAsyncResult> end = iar => { Assert.AreSame(x, iar); };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2))).ToArray();
        Assert.IsTrue(res.SequenceEqual([new Unit()]));
    }

    [TestMethod]
    public async Task FromAsyncPattern1_Error()
    {
        var x = new Result();
        var ex = new Exception();

        Func<int, AsyncCallback, object?, IAsyncResult> begin = (a, cb, o) => { cb(x); return x; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); throw ex; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task FromAsyncPattern1_ErrorBegin()
    {
        var x = new Result();
        var ex = new Exception();

        Func<int, AsyncCallback, object?, IAsyncResult> begin = (a, cb, o) => { cb(x); throw ex; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 0; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task FromAsyncPattern2()
    {
        var x = new Result();

        Func<int, int, AsyncCallback, object?, IAsyncResult> begin = (a, b, cb, _) =>
        {
            Assert.AreEqual(2, a);
            Assert.AreEqual(3, b);
            cb(x);
            return x;
        };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 1; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2,
            3).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnNext(1),
            Notification.CreateOnCompleted<int>()]));
    }

    [TestMethod]
    public async Task FromAsyncPatternAction2()
    {
        var x = new Result();

        Func<int, int, AsyncCallback, object?, IAsyncResult> begin = (a, b, cb, _) =>
        {
            Assert.AreEqual(2, a);
            Assert.AreEqual(3, b);
            cb(x);
            return x;
        };
        Action<IAsyncResult> end = iar => { Assert.AreSame(x, iar); };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3))).ToArray();
        Assert.IsTrue(res.SequenceEqual([new Unit()]));
    }

    [TestMethod]
    public async Task FromAsyncPattern2_Error()
    {
        var x = new Result();
        var ex = new Exception();

        Func<int, int, AsyncCallback, object?, IAsyncResult> begin = (a, b, cb, o) => { cb(
            x); return x; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); throw ex; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2,
            3).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task FromAsyncPattern2_ErrorBegin()
    {
        var x = new Result();
        var ex = new Exception();

        Func<int, int, AsyncCallback, object?, IAsyncResult> begin = (a, b, cb, o) => { cb(
            x); throw ex; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 0; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2,
            3).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task FromAsyncPattern3()
    {
        var x = new Result();

        Func<int, int, int, AsyncCallback, object?, IAsyncResult> begin = (a, b, c, cb, _) =>
        {
            Assert.AreEqual(2, a);
            Assert.AreEqual(3, b);
            Assert.AreEqual(4, c);
            cb(x);
            return x;
        };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 1; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3,
            4).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnNext(1),
            Notification.CreateOnCompleted<int>()]));
    }

    [TestMethod]
    public async Task FromAsyncPatternAction3()
    {
        var x = new Result();

        Func<int, int, int, AsyncCallback, object?, IAsyncResult> begin = (a, b, c, cb, _) =>
        {
            Assert.AreEqual(2, a);
            Assert.AreEqual(3, b);
            Assert.AreEqual(4, c);
            cb(x);
            return x;
        };
        Action<IAsyncResult> end = iar => { Assert.AreSame(x, iar); };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4))).ToArray();
        Assert.IsTrue(res.SequenceEqual([new Unit()]));
    }

    [TestMethod]
    public async Task FromAsyncPattern3_Error()
    {
        var x = new Result();
        var ex = new Exception();

        Func<int, int, int, AsyncCallback, object?, IAsyncResult> begin = (a, b, c, cb, o) => { cb(
            x); return x; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); throw ex; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3,
            4).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task FromAsyncPattern3_ErrorBegin()
    {
        var x = new Result();
        var ex = new Exception();

        Func<int, int, int, AsyncCallback, object?, IAsyncResult> begin = (a, b, c, cb, o) => { cb(
            x); throw ex; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 0; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3,
            4).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task FromAsyncPattern4()
    {
        var x = new Result();

        Func<int, int, int, int, AsyncCallback, object?, IAsyncResult> begin = (a, b, c, d, cb,
            _) =>
        {
            Assert.AreEqual(2, a);
            Assert.AreEqual(3, b);
            Assert.AreEqual(4, c);
            Assert.AreEqual(5, d);
            cb(x);
            return x;
        };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 1; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4,
            5).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnNext(1),
            Notification.CreateOnCompleted<int>()]));
    }

    [TestMethod]
    public async Task FromAsyncPatternAction4()
    {
        var x = new Result();

        Func<int, int, int, int, AsyncCallback, object?, IAsyncResult> begin = (a, b, c, d, cb,
            _) =>
        {
            Assert.AreEqual(2, a);
            Assert.AreEqual(3, b);
            Assert.AreEqual(4, c);
            Assert.AreEqual(5, d);
            cb(x);
            return x;
        };
        Action<IAsyncResult> end = iar => { Assert.AreSame(x, iar); };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5))).ToArray();
        Assert.IsTrue(res.SequenceEqual([new Unit()]));
    }

    [TestMethod]
    public async Task FromAsyncPattern4_Error()
    {
        var x = new Result();
        var ex = new Exception();

        Func<int, int, int, int, AsyncCallback, object?, IAsyncResult> begin = (a, b, c, d, cb,
            o) => { cb(x); return x; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); throw ex; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4,
            5).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task FromAsyncPattern4_ErrorBegin()
    {
        var x = new Result();
        var ex = new Exception();

        Func<int, int, int, int, AsyncCallback, object?, IAsyncResult> begin = (a, b, c, d, cb,
            o) => { cb(x); throw ex; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 0; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4,
            5).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task FromAsyncPattern5()
    {
        var x = new Result();

        Func<int, int, int, int, int, AsyncCallback, object?, IAsyncResult> begin = (a, b, c, d, e,
            cb, _) =>
        {
            Assert.AreEqual(2, a);
            Assert.AreEqual(3, b);
            Assert.AreEqual(4, c);
            Assert.AreEqual(5, d);
            Assert.AreEqual(6, e);
            cb(x);
            return x;
        };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 1; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5,
            6).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnNext(1),
            Notification.CreateOnCompleted<int>()]));
    }

    [TestMethod]
    public async Task FromAsyncPatternAction5()
    {
        var x = new Result();

        Func<int, int, int, int, int, AsyncCallback, object?, IAsyncResult> begin = (a, b, c, d, e,
            cb, _) =>
        {
            Assert.AreEqual(2, a);
            Assert.AreEqual(3, b);
            Assert.AreEqual(4, c);
            Assert.AreEqual(5, d);
            Assert.AreEqual(6, e);
            cb(x);
            return x;
        };
        Action<IAsyncResult> end = iar => { Assert.AreSame(x, iar); };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6))).ToArray();
        Assert.IsTrue(res.SequenceEqual([new Unit()]));
    }

    [TestMethod]
    public async Task FromAsyncPattern5_Error()
    {
        var x = new Result();
        var ex = new Exception();

        Func<int, int, int, int, int, AsyncCallback, object?, IAsyncResult> begin = (a, b, c, d, e,
            cb, o) => { cb(x); return x; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); throw ex; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5,
            6).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task FromAsyncPattern5_ErrorBegin()
    {
        var x = new Result();
        var ex = new Exception();

        Func<int, int, int, int, int, AsyncCallback, object?, IAsyncResult> begin = (a, b, c, d, e,
            cb, o) => { cb(x); throw ex; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 0; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5,
            6).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task FromAsyncPattern6()
    {
        var x = new Result();

        Func<int, int, int, int, int, int, AsyncCallback, object?, IAsyncResult> begin = (a, b, c,
            d, e, f, cb, _) =>
        {
            Assert.AreEqual(2, a);
            Assert.AreEqual(3, b);
            Assert.AreEqual(4, c);
            Assert.AreEqual(5, d);
            Assert.AreEqual(6, e);
            Assert.AreEqual(7, f);
            cb(x);
            return x;
        };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 1; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6,
            7).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnNext(1),
            Notification.CreateOnCompleted<int>()]));
    }

    [TestMethod]
    public async Task FromAsyncPatternAction6()
    {
        var x = new Result();

        Func<int, int, int, int, int, int, AsyncCallback, object?, IAsyncResult> begin = (a, b, c,
            d, e, f, cb, _) =>
        {
            Assert.AreEqual(2, a);
            Assert.AreEqual(3, b);
            Assert.AreEqual(4, c);
            Assert.AreEqual(5, d);
            Assert.AreEqual(6, e);
            Assert.AreEqual(7, f);
            cb(x);
            return x;
        };
        Action<IAsyncResult> end = iar => { Assert.AreSame(x, iar); };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7))).ToArray();
        Assert.IsTrue(res.SequenceEqual([new Unit()]));
    }

    [TestMethod]
    public async Task FromAsyncPattern6_Error()
    {
        var x = new Result();
        var ex = new Exception();

        Func<int, int, int, int, int, int, AsyncCallback, object?, IAsyncResult> begin = (a, b, c,
            d, e, f, cb, o) => { cb(x); return x; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); throw ex; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6,
            7).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task FromAsyncPattern6_ErrorBegin()
    {
        var x = new Result();
        var ex = new Exception();

        Func<int, int, int, int, int, int, AsyncCallback, object?, IAsyncResult> begin = (a, b, c,
            d, e, f, cb, o) => { cb(x); throw ex; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 0; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6,
            7).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task FromAsyncPattern7()
    {
        var x = new Result();

        Func<int, int, int, int, int, int, int, AsyncCallback, object?, IAsyncResult> begin = (a, b,
            c, d, e, f, g, cb, _) =>
        {
            Assert.AreEqual(2, a);
            Assert.AreEqual(3, b);
            Assert.AreEqual(4, c);
            Assert.AreEqual(5, d);
            Assert.AreEqual(6, e);
            Assert.AreEqual(7, f);
            Assert.AreEqual(8, g);
            cb(x);
            return x;
        };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 1; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7,
            8).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnNext(1),
            Notification.CreateOnCompleted<int>()]));
    }

    [TestMethod]
    public async Task FromAsyncPatternAction7()
    {
        var x = new Result();

        Func<int, int, int, int, int, int, int, AsyncCallback, object?, IAsyncResult> begin = (a, b,
            c, d, e, f, g, cb, _) =>
        {
            Assert.AreEqual(2, a);
            Assert.AreEqual(3, b);
            Assert.AreEqual(4, c);
            Assert.AreEqual(5, d);
            Assert.AreEqual(6, e);
            Assert.AreEqual(7, f);
            Assert.AreEqual(8, g);
            cb(x);
            return x;
        };
        Action<IAsyncResult> end = iar => { Assert.AreSame(x, iar); };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7,
            8))).ToArray();
        Assert.IsTrue(res.SequenceEqual([new Unit()]));
    }

    [TestMethod]
    public async Task FromAsyncPattern7_Error()
    {
        var x = new Result();
        var ex = new Exception();

        Func<int, int, int, int, int, int, int, AsyncCallback, object?, IAsyncResult> begin = (a, b,
            c, d, e, f, g, cb, o) => { cb(x); return x; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); throw ex; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7,
            8).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task FromAsyncPattern7_ErrorBegin()
    {
        var x = new Result();
        var ex = new Exception();

        Func<int, int, int, int, int, int, int, AsyncCallback, object?, IAsyncResult> begin = (a, b,
            c, d, e, f, g, cb, o) => { cb(x); throw ex; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 0; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7,
            8).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task FromAsyncPattern8()
    {
        var x = new Result();

        Func<int, int, int, int, int, int, int, int, AsyncCallback, object?, IAsyncResult> begin = (
            a, b, c, d, e, f, g, h, cb, _) =>
        {
            Assert.AreEqual(2, a);
            Assert.AreEqual(3, b);
            Assert.AreEqual(4, c);
            Assert.AreEqual(5, d);
            Assert.AreEqual(6, e);
            Assert.AreEqual(7, f);
            Assert.AreEqual(8, g);
            Assert.AreEqual(9, h);
            cb(x);
            return x;
        };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 1; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8,
            9).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnNext(1),
            Notification.CreateOnCompleted<int>()]));
    }

    [TestMethod]
    public async Task FromAsyncPatternAction8()
    {
        var x = new Result();

        Func<int, int, int, int, int, int, int, int, AsyncCallback, object?, IAsyncResult> begin = (
            a, b, c, d, e, f, g, h, cb, _) =>
        {
            Assert.AreEqual(2, a);
            Assert.AreEqual(3, b);
            Assert.AreEqual(4, c);
            Assert.AreEqual(5, d);
            Assert.AreEqual(6, e);
            Assert.AreEqual(7, f);
            Assert.AreEqual(8, g);
            Assert.AreEqual(9, h);
            cb(x);
            return x;
        };
        Action<IAsyncResult> end = iar => { Assert.AreSame(x, iar); };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8,
            9))).ToArray();
        Assert.IsTrue(res.SequenceEqual([new Unit()]));
    }

    [TestMethod]
    public async Task FromAsyncPattern8_Error()
    {
        var x = new Result();
        var ex = new Exception();

        Func<int, int, int, int, int, int, int, int, AsyncCallback, object?, IAsyncResult> begin = (
            a, b, c, d, e, f, g, h, cb, o) => { cb(x); return x; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); throw ex; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8,
            9).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task FromAsyncPattern8_ErrorBegin()
    {
        var x = new Result();
        var ex = new Exception();

        Func<int, int, int, int, int, int, int, int, AsyncCallback, object?, IAsyncResult> begin = (
            a, b, c, d, e, f, g, h, cb, o) => { cb(x); throw ex; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 0; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8,
            9).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task FromAsyncPattern9()
    {
        var x = new Result();

        Func<int, int, int, int, int, int, int, int, int, AsyncCallback, object?,
            IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, cb, _) =>
        {
            Assert.AreEqual(2, a);
            Assert.AreEqual(3, b);
            Assert.AreEqual(4, c);
            Assert.AreEqual(5, d);
            Assert.AreEqual(6, e);
            Assert.AreEqual(7, f);
            Assert.AreEqual(8, g);
            Assert.AreEqual(9, h);
            Assert.AreEqual(10, i);
            cb(x);
            return x;
        };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 1; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9,
            10).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnNext(1),
            Notification.CreateOnCompleted<int>()]));
    }

    [TestMethod]
    public async Task FromAsyncPatternAction9()
    {
        var x = new Result();

        Func<int, int, int, int, int, int, int, int, int, AsyncCallback, object?,
            IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, cb, _) =>
        {
            Assert.AreEqual(2, a);
            Assert.AreEqual(3, b);
            Assert.AreEqual(4, c);
            Assert.AreEqual(5, d);
            Assert.AreEqual(6, e);
            Assert.AreEqual(7, f);
            Assert.AreEqual(8, g);
            Assert.AreEqual(9, h);
            Assert.AreEqual(10, i);
            cb(x);
            return x;
        };
        Action<IAsyncResult> end = iar => { Assert.AreSame(x, iar); };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9,
            10))).ToArray();
        Assert.IsTrue(res.SequenceEqual([new Unit()]));
    }

    [TestMethod]
    public async Task FromAsyncPattern9_Error()
    {
        var x = new Result();
        var ex = new Exception();

        Func<int, int, int, int, int, int, int, int, int, AsyncCallback, object?,
            IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, cb, o) => { cb(x); return x; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); throw ex; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9,
            10).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task FromAsyncPattern9_ErrorBegin()
    {
        var x = new Result();
        var ex = new Exception();

        Func<int, int, int, int, int, int, int, int, int, AsyncCallback, object?,
            IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, cb, o) => { cb(x); throw ex; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 0; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9,
            10).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task FromAsyncPattern10()
    {
        var x = new Result();

        Func<int, int, int, int, int, int, int, int, int, int, AsyncCallback, object?,
            IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, cb, _) =>
        {
            Assert.AreEqual(2, a);
            Assert.AreEqual(3, b);
            Assert.AreEqual(4, c);
            Assert.AreEqual(5, d);
            Assert.AreEqual(6, e);
            Assert.AreEqual(7, f);
            Assert.AreEqual(8, g);
            Assert.AreEqual(9, h);
            Assert.AreEqual(10, i);
            Assert.AreEqual(11, j);
            cb(x);
            return x;
        };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 1; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10,
            11).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnNext(1),
            Notification.CreateOnCompleted<int>()]));
    }

    [TestMethod]
    public async Task FromAsyncPatternAction10()
    {
        var x = new Result();

        Func<int, int, int, int, int, int, int, int, int, int, AsyncCallback, object?,
            IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, cb, _) =>
        {
            Assert.AreEqual(2, a);
            Assert.AreEqual(3, b);
            Assert.AreEqual(4, c);
            Assert.AreEqual(5, d);
            Assert.AreEqual(6, e);
            Assert.AreEqual(7, f);
            Assert.AreEqual(8, g);
            Assert.AreEqual(9, h);
            Assert.AreEqual(10, i);
            Assert.AreEqual(11, j);
            cb(x);
            return x;
        };
        Action<IAsyncResult> end = iar => { Assert.AreSame(x, iar); };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10,
            11))).ToArray();
        Assert.IsTrue(res.SequenceEqual([new Unit()]));
    }

    [TestMethod]
    public async Task FromAsyncPattern10_Error()
    {
        var x = new Result();
        var ex = new Exception();

        Func<int, int, int, int, int, int, int, int, int, int, AsyncCallback, object?,
            IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, cb, o) => { cb(x); return x; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); throw ex; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10,
            11).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task FromAsyncPattern10_ErrorBegin()
    {
        var x = new Result();
        var ex = new Exception();

        Func<int, int, int, int, int, int, int, int, int, int, AsyncCallback, object?,
            IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, cb, o) => { cb(x); throw ex; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 0; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10,
            11).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task FromAsyncPattern11()
    {
        var x = new Result();

        Func<int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object?,
            IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, cb, _) =>
        {
            Assert.AreEqual(2, a);
            Assert.AreEqual(3, b);
            Assert.AreEqual(4, c);
            Assert.AreEqual(5, d);
            Assert.AreEqual(6, e);
            Assert.AreEqual(7, f);
            Assert.AreEqual(8, g);
            Assert.AreEqual(9, h);
            Assert.AreEqual(10, i);
            Assert.AreEqual(11, j);
            Assert.AreEqual(12, k);
            cb(x);
            return x;
        };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 1; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10,
            11, 12).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnNext(1),
            Notification.CreateOnCompleted<int>()]));
    }

    [TestMethod]
    public async Task FromAsyncPatternAction11()
    {
        var x = new Result();

        Func<int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object?,
            IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, cb, _) =>
        {
            Assert.AreEqual(2, a);
            Assert.AreEqual(3, b);
            Assert.AreEqual(4, c);
            Assert.AreEqual(5, d);
            Assert.AreEqual(6, e);
            Assert.AreEqual(7, f);
            Assert.AreEqual(8, g);
            Assert.AreEqual(9, h);
            Assert.AreEqual(10, i);
            Assert.AreEqual(11, j);
            Assert.AreEqual(12, k);
            cb(x);
            return x;
        };
        Action<IAsyncResult> end = iar => { Assert.AreSame(x, iar); };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10,
            11, 12))).ToArray();
        Assert.IsTrue(res.SequenceEqual([new Unit()]));
    }

    [TestMethod]
    public async Task FromAsyncPattern11_Error()
    {
        var x = new Result();
        var ex = new Exception();

        Func<int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object?,
            IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, cb, o) => { cb(x); return x; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); throw ex; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10,
            11, 12).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task FromAsyncPattern11_ErrorBegin()
    {
        var x = new Result();
        var ex = new Exception();

        Func<int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object?,
            IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, cb, o) => { cb(x); throw ex; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 0; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10,
            11, 12).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task FromAsyncPattern12()
    {
        var x = new Result();

        Func<int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object?,
            IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, cb, _) =>
        {
            Assert.AreEqual(2, a);
            Assert.AreEqual(3, b);
            Assert.AreEqual(4, c);
            Assert.AreEqual(5, d);
            Assert.AreEqual(6, e);
            Assert.AreEqual(7, f);
            Assert.AreEqual(8, g);
            Assert.AreEqual(9, h);
            Assert.AreEqual(10, i);
            Assert.AreEqual(11, j);
            Assert.AreEqual(12, k);
            Assert.AreEqual(13, l);
            cb(x);
            return x;
        };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 1; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10,
            11, 12, 13).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnNext(1),
            Notification.CreateOnCompleted<int>()]));
    }

    [TestMethod]
    public async Task FromAsyncPatternAction12()
    {
        var x = new Result();

        Func<int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object?,
            IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, cb, _) =>
        {
            Assert.AreEqual(2, a);
            Assert.AreEqual(3, b);
            Assert.AreEqual(4, c);
            Assert.AreEqual(5, d);
            Assert.AreEqual(6, e);
            Assert.AreEqual(7, f);
            Assert.AreEqual(8, g);
            Assert.AreEqual(9, h);
            Assert.AreEqual(10, i);
            Assert.AreEqual(11, j);
            Assert.AreEqual(12, k);
            Assert.AreEqual(13, l);
            cb(x);
            return x;
        };
        Action<IAsyncResult> end = iar => { Assert.AreSame(x, iar); };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10,
            11, 12, 13))).ToArray();
        Assert.IsTrue(res.SequenceEqual([new Unit()]));
    }

    [TestMethod]
    public async Task FromAsyncPattern12_Error()
    {
        var x = new Result();
        var ex = new Exception();

        Func<int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object?,
            IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, cb, o) => { cb(
            x); return x; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); throw ex; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10,
            11, 12, 13).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task FromAsyncPattern12_ErrorBegin()
    {
        var x = new Result();
        var ex = new Exception();

        Func<int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback, object?,
            IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, cb, o) => { cb(
            x); throw ex; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 0; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10,
            11, 12, 13).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task FromAsyncPattern13()
    {
        var x = new Result();

        Func<int, int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback,
            object?, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, m, cb, _) =>
        {
            Assert.AreEqual(2, a);
            Assert.AreEqual(3, b);
            Assert.AreEqual(4, c);
            Assert.AreEqual(5, d);
            Assert.AreEqual(6, e);
            Assert.AreEqual(7, f);
            Assert.AreEqual(8, g);
            Assert.AreEqual(9, h);
            Assert.AreEqual(10, i);
            Assert.AreEqual(11, j);
            Assert.AreEqual(12, k);
            Assert.AreEqual(13, l);
            Assert.AreEqual(14, m);
            cb(x);
            return x;
        };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 1; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10,
            11, 12, 13, 14).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnNext(1),
            Notification.CreateOnCompleted<int>()]));
    }

    [TestMethod]
    public async Task FromAsyncPatternAction13()
    {
        var x = new Result();

        Func<int, int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback,
            object?, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, m, cb, _) =>
        {
            Assert.AreEqual(2, a);
            Assert.AreEqual(3, b);
            Assert.AreEqual(4, c);
            Assert.AreEqual(5, d);
            Assert.AreEqual(6, e);
            Assert.AreEqual(7, f);
            Assert.AreEqual(8, g);
            Assert.AreEqual(9, h);
            Assert.AreEqual(10, i);
            Assert.AreEqual(11, j);
            Assert.AreEqual(12, k);
            Assert.AreEqual(13, l);
            Assert.AreEqual(14, m);
            cb(x);
            return x;
        };
        Action<IAsyncResult> end = iar => { Assert.AreSame(x, iar); };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10,
            11, 12, 13, 14))).ToArray();
        Assert.IsTrue(res.SequenceEqual([new Unit()]));
    }

    [TestMethod]
    public async Task FromAsyncPattern13_Error()
    {
        var x = new Result();
        var ex = new Exception();

        Func<int, int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback,
            object?, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, m, cb, o) => { cb(
            x); return x; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); throw ex; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10,
            11, 12, 13, 14).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task FromAsyncPattern13_ErrorBegin()
    {
        var x = new Result();
        var ex = new Exception();

        Func<int, int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback,
            object?, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, m, cb, o) => { cb(
            x); throw ex; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 0; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10,
            11, 12, 13, 14).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task FromAsyncPattern14()
    {
        var x = new Result();

        Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback,
            object?, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, m, n, cb, _) =>
        {
            Assert.AreEqual(2, a);
            Assert.AreEqual(3, b);
            Assert.AreEqual(4, c);
            Assert.AreEqual(5, d);
            Assert.AreEqual(6, e);
            Assert.AreEqual(7, f);
            Assert.AreEqual(8, g);
            Assert.AreEqual(9, h);
            Assert.AreEqual(10, i);
            Assert.AreEqual(11, j);
            Assert.AreEqual(12, k);
            Assert.AreEqual(13, l);
            Assert.AreEqual(14, m);
            Assert.AreEqual(15, n);
            cb(x);
            return x;
        };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 1; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10,
            11, 12, 13, 14, 15).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnNext(1),
            Notification.CreateOnCompleted<int>()]));
    }

    [TestMethod]
    public async Task FromAsyncPatternAction14()
    {
        var x = new Result();

        Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback,
            object?, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, m, n, cb, _) =>
        {
            Assert.AreEqual(2, a);
            Assert.AreEqual(3, b);
            Assert.AreEqual(4, c);
            Assert.AreEqual(5, d);
            Assert.AreEqual(6, e);
            Assert.AreEqual(7, f);
            Assert.AreEqual(8, g);
            Assert.AreEqual(9, h);
            Assert.AreEqual(10, i);
            Assert.AreEqual(11, j);
            Assert.AreEqual(12, k);
            Assert.AreEqual(13, l);
            Assert.AreEqual(14, m);
            Assert.AreEqual(15, n);
            cb(x);
            return x;
        };
        Action<IAsyncResult> end = iar => { Assert.AreSame(x, iar); };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10,
            11, 12, 13, 14, 15))).ToArray();
        Assert.IsTrue(res.SequenceEqual([new Unit()]));
    }

    [TestMethod]
    public async Task FromAsyncPattern14_Error()
    {
        var x = new Result();
        var ex = new Exception();

        Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback,
            object?, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, m, n, cb, o) =>
            { cb(x); return x; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); throw ex; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10,
            11, 12, 13, 14, 15).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task FromAsyncPattern14_ErrorBegin()
    {
        var x = new Result();
        var ex = new Exception();

        Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, AsyncCallback,
            object?, IAsyncResult> begin = (a, b, c, d, e, f, g, h, i, j, k, l, m, n, cb, o) =>
            { cb(x); throw ex; };
        Func<IAsyncResult, int> end = iar => { Assert.AreSame(x, iar); return 0; };

        var res = (await ToListAsync(Seq.FromAsyncPattern(begin, end)(2, 3, 4, 5, 6, 7, 8, 9, 10,
            11, 12, 13, 14, 15).Materialize())).ToArray();
        Assert.IsTrue(res.SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    private class Result : IAsyncResult
    {
        public object? AsyncState
        {
            get { throw new NotImplementedException(); }
        }

        public WaitHandle AsyncWaitHandle
        {
            get { throw new NotImplementedException(); }
        }

        public bool CompletedSynchronously
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsCompleted
        {
            get { throw new NotImplementedException(); }
        }
    }
}
