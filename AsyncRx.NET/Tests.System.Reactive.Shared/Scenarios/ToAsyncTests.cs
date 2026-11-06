// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>ToAsync</c> scenarios, from Rx.NET's <c>ToAsyncTest.cs</c>.</summary>
/// <remarks>
/// 68 of the file's 69 tests (<c>ToAsync_ArgumentChecking</c> is the code-generated stratum),
/// all transcribed as they are: the function and action forms at every arity from zero to
/// sixteen, with and without a scheduler, their results and their errors. <c>Seq.ToAsync(f)</c>
/// returns a function, as <c>Observable.ToAsync(f)</c> does, so the call <c>(1, 2)</c> that
/// follows reads as the original; where the original blocks on <c>ToEnumerable</c>, the scenario
/// awaits <see cref="SharedReactiveTest.ToListAsync{T}"/>. The longer lines are wrapped for width
/// and nothing else.
/// </remarks>
public abstract class ToAsyncTests : SharedReactiveTest
{
    [TestMethod]
    public async Task ToAsync0()
    {
        Assert.IsTrue((await ToListAsync(Seq.ToAsync(() => 0)())).SequenceEqual([0]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync(() => 0, DefaultScheduler)())).SequenceEqual(
            [0]));
    }

    [TestMethod]
    public async Task ToAsync1()
    {
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int>(a => a)(1))).SequenceEqual([1]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int>(a => a, DefaultScheduler)(
            1))).SequenceEqual([1]));
    }

    [TestMethod]
    public async Task ToAsync2()
    {
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int>((a, b) => a + b)(1,
            2))).SequenceEqual([1 + 2]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int>((a, b) => a + b,
            DefaultScheduler)(1, 2))).SequenceEqual([1 + 2]));
    }

    [TestMethod]
    public async Task ToAsync3()
    {
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int>((a, b, c) => a + b + c)(1,
            2, 3))).SequenceEqual([1 + 2 + 3]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int>((a, b, c) => a + b + c,
            DefaultScheduler)(1, 2, 3))).SequenceEqual([1 + 2 + 3]));
    }

    [TestMethod]
    public async Task ToAsync4()
    {
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int>((a, b, c, d) =>
            a + b + c + d)(1, 2, 3, 4))).SequenceEqual([1 + 2 + 3 + 4]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int>((a, b, c, d) =>
            a + b + c + d, DefaultScheduler)(1, 2, 3, 4))).SequenceEqual([1 + 2 + 3 + 4]));
    }

    [TestMethod]
    public async Task ToAsync5()
    {
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int>((a, b, c, d,
            e) => a + b + c + d + e)(1, 2, 3, 4, 5))).SequenceEqual([1 + 2 + 3 + 4 + 5]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int>((a, b, c, d,
            e) => a + b + c + d + e, DefaultScheduler)(1, 2, 3, 4, 5))).SequenceEqual(
            [1 + 2 + 3 + 4 + 5]));
    }

    [TestMethod]
    public async Task ToAsync6()
    {
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int>((a, b, c, d,
            e, f) => a + b + c + d + e + f)(1, 2, 3, 4, 5, 6))).SequenceEqual(
            [1 + 2 + 3 + 4 + 5 + 6]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int>((a, b, c, d,
            e, f) => a + b + c + d + e + f, DefaultScheduler)(1, 2, 3, 4, 5, 6))).SequenceEqual(
            [1 + 2 + 3 + 4 + 5 + 6]));
    }

    [TestMethod]
    public async Task ToAsync7()
    {
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int>((a, b,
            c, d, e, f, g) => a + b + c + d + e + f + g)(1, 2, 3, 4, 5, 6, 7))).SequenceEqual(
            [1 + 2 + 3 + 4 + 5 + 6 + 7]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int>((a, b,
            c, d, e, f, g) => a + b + c + d + e + f + g, DefaultScheduler)(1, 2, 3, 4, 5, 6,
            7))).SequenceEqual([1 + 2 + 3 + 4 + 5 + 6 + 7]));
    }

    [TestMethod]
    public async Task ToAsync8()
    {
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int>((
            a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h)(1, 2, 3, 4, 5, 6, 7,
            8))).SequenceEqual([1 + 2 + 3 + 4 + 5 + 6 + 7 + 8]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int>((
            a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h, DefaultScheduler)(1, 2, 3, 4,
            5, 6, 7, 8))).SequenceEqual([1 + 2 + 3 + 4 + 5 + 6 + 7 + 8]));
    }

    [TestMethod]
    public async Task ToAsync9()
    {
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int>((a, b, c, d, e, f, g, h, i) => a + b + c + d + e + f + g + h + i)(1, 2, 3, 4, 5, 6,
            7, 8, 9))).SequenceEqual([1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int>((a, b, c, d, e, f, g, h, i) => a + b + c + d + e + f + g + h + i,
            DefaultScheduler)(1, 2, 3, 4, 5, 6, 7, 8, 9))).SequenceEqual(
            [1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9]));
    }

    [TestMethod]
    public async Task ToAsync10()
    {
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int>((a, b, c, d, e, f, g, h, i, j) => a + b + c + d + e + f + g + h + i + j)(1, 2,
            3, 4, 5, 6, 7, 8, 9, 10))).SequenceEqual([1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int>((a, b, c, d, e, f, g, h, i, j) => a + b + c + d + e + f + g + h + i + j,
            DefaultScheduler)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10))).SequenceEqual(
            [1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10]));
    }

    [TestMethod]
    public async Task ToAsync11()
    {
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int>((a, b, c, d, e, f, g, h, i, j, k) =>
            a + b + c + d + e + f + g + h + i + j + k)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
            11))).SequenceEqual([1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int>((a, b, c, d, e, f, g, h, i, j, k) =>
            a + b + c + d + e + f + g + h + i + j + k, DefaultScheduler)(1, 2, 3, 4, 5, 6, 7, 8, 9,
            10, 11))).SequenceEqual([1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11]));
    }

    [TestMethod]
    public async Task ToAsync12()
    {
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) =>
            a + b + c + d + e + f + g + h + i + j + k + l)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11,
            12))).SequenceEqual([1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) =>
            a + b + c + d + e + f + g + h + i + j + k + l, DefaultScheduler)(1, 2, 3, 4, 5, 6, 7, 8,
            9, 10, 11, 12))).SequenceEqual([1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12]));
    }

    [TestMethod]
    public async Task ToAsync13()
    {
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) =>
            a + b + c + d + e + f + g + h + i + j + k + l + m)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11,
            12, 13))).SequenceEqual([1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) =>
            a + b + c + d + e + f + g + h + i + j + k + l + m, DefaultScheduler)(1, 2, 3, 4, 5, 6,
            7, 8, 9, 10, 11, 12, 13))).SequenceEqual(
            [1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13]));
    }

    [TestMethod]
    public async Task ToAsync14()
    {
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) =>
            a + b + c + d + e + f + g + h + i + j + k + l + m + n)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
            11, 12, 13, 14))).SequenceEqual(
            [1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) =>
            a + b + c + d + e + f + g + h + i + j + k + l + m + n, DefaultScheduler)(1, 2, 3, 4, 5,
            6, 7, 8, 9, 10, 11, 12, 13, 14))).SequenceEqual(
            [1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14]));
    }

    [TestMethod]
    public async Task ToAsync15()
    {
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) =>
            a + b + c + d + e + f + g + h + i + j + k + l + m + n + o)(1, 2, 3, 4, 5, 6, 7, 8, 9,
            10, 11, 12, 13, 14, 15))).SequenceEqual(
            [1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 + 15]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) =>
            a + b + c + d + e + f + g + h + i + j + k + l + m + n + o, DefaultScheduler)(1, 2, 3, 4,
            5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15))).SequenceEqual(
            [1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 + 15]));
    }

    [TestMethod]
    public async Task ToAsync16()
    {
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o,
            p) => a + b + c + d + e + f + g + h + i + j + k + l + m + n + o + p)(1, 2, 3, 4, 5, 6,
            7, 8, 9, 10, 11, 12, 13, 14, 15, 16))).SequenceEqual(
            [1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 + 15 + 16]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o,
            p) => a + b + c + d + e + f + g + h + i + j + k + l + m + n + o + p, DefaultScheduler)(
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16))).SequenceEqual(
            [1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 + 15 + 16]));
    }

    [TestMethod]
    public async Task ToAsync_Error0()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int>(() =>
            { throw ex; })().Materialize())).SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task ToAsync_Error1()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int>(a => { throw ex; })(
            1).Materialize())).SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task ToAsync_Error2()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int>((a, b) => { throw ex; })(1,
            2).Materialize())).SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task ToAsync_Error3()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int>((a, b, c) =>
            { throw ex; })(1, 2, 3).Materialize())).SequenceEqual([Notification.CreateOnError<int>(
            ex)]));
    }

    [TestMethod]
    public async Task ToAsync_Error4()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int>((a, b, c, d) =>
            { throw ex; })(1, 2, 3, 4).Materialize())).SequenceEqual([Notification.CreateOnError<
            int>(ex)]));
    }

    [TestMethod]
    public async Task ToAsync_Error5()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int>((a, b, c, d,
            e) => { throw ex; })(1, 2, 3, 4, 5).Materialize())).SequenceEqual(
            [Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task ToAsync_Error6()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int>((a, b, c, d,
            e, f) => { throw ex; })(1, 2, 3, 4, 5, 6).Materialize())).SequenceEqual(
            [Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task ToAsync_Error7()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int>((a, b,
            c, d, e, f, g) => { throw ex; })(1, 2, 3, 4, 5, 6, 7).Materialize())).SequenceEqual(
            [Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task ToAsync_Error8()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int>((
            a, b, c, d, e, f, g, h) => { throw ex; })(1, 2, 3, 4, 5, 6, 7,
            8).Materialize())).SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task ToAsync_Error9()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int>((a, b, c, d, e, f, g, h, i) => { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8,
            9).Materialize())).SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task ToAsync_Error10()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int>((a, b, c, d, e, f, g, h, i, j) => { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9,
            10).Materialize())).SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task ToAsync_Error11()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int>((a, b, c, d, e, f, g, h, i, j, k) => { throw ex; })(1, 2, 3, 4, 5, 6, 7,
            8, 9, 10, 11).Materialize())).SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task ToAsync_Error12()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) => { throw ex; })(1, 2, 3, 4,
            5, 6, 7, 8, 9, 10, 11, 12).Materialize())).SequenceEqual([Notification.CreateOnError<
            int>(ex)]));
    }

    [TestMethod]
    public async Task ToAsync_Error13()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) => { throw ex; })(1, 2,
            3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).Materialize())).SequenceEqual(
            [Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task ToAsync_Error14()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) =>
            { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13,
            14).Materialize())).SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task ToAsync_Error15()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) =>
            { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14,
            15).Materialize())).SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task ToAsync_Error16()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o,
            p) => { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
            16).Materialize())).SequenceEqual([Notification.CreateOnError<int>(ex)]));
    }

    [TestMethod]
    public async Task ToAsyncAction0()
    {
        var hasRun = false;
        Assert.IsTrue((await ToListAsync(Seq.ToAsync(() => { hasRun = true; })())).SequenceEqual(
            [new Unit()]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync(() => { hasRun = true; },
            DefaultScheduler)())).SequenceEqual([new Unit()]));
        Assert.IsTrue(hasRun, "has run");
    }

    [TestMethod]
    public async Task ToAsyncActionError0()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync(() =>
            { throw ex; })().Materialize())).SequenceEqual([Notification.CreateOnError<Unit>(ex)]));
    }

    [TestMethod]
    public async Task ToAsyncAction1()
    {
        var hasRun = false;
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int>(a => { Assert.AreEqual(1,
            a); hasRun = true; })(1))).SequenceEqual([new Unit()]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int>(a => { Assert.AreEqual(1,
            a); hasRun = true; }, DefaultScheduler)(1))).SequenceEqual([new Unit()]));
        Assert.IsTrue(hasRun, "has run");
    }

    [TestMethod]
    public async Task ToAsyncActionError1()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int>(a => { Assert.AreEqual(1,
            a); throw ex; })(1).Materialize())).SequenceEqual([Notification.CreateOnError<Unit>(
            ex)]));
    }

    [TestMethod]
    public async Task ToAsyncAction2()
    {
        var hasRun = false;
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int>((a, b) => { Assert.AreEqual(1,
            a); Assert.AreEqual(2, b); hasRun = true; })(1, 2))).SequenceEqual([new Unit()]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int>((a, b) => { Assert.AreEqual(1,
            a); Assert.AreEqual(2, b); hasRun = true; }, DefaultScheduler)(1, 2))).SequenceEqual(
            [new Unit()]));
        Assert.IsTrue(hasRun, "has run");
    }

    [TestMethod]
    public async Task ToAsyncActionError2()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int>((a, b) => { Assert.AreEqual(1,
            a); Assert.AreEqual(2, b); throw ex; })(1, 2).Materialize())).SequenceEqual(
            [Notification.CreateOnError<Unit>(ex)]));
    }

    [TestMethod]
    public async Task ToAsyncAction3()
    {
        var hasRun = false;
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int>((a, b, c) => { Assert.AreEqual(
            1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); hasRun = true; })(1, 2,
            3))).SequenceEqual([new Unit()]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int>((a, b, c) => { Assert.AreEqual(
            1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); hasRun = true; },
            DefaultScheduler)(1, 2, 3))).SequenceEqual([new Unit()]));
        Assert.IsTrue(hasRun, "has run");
    }

    [TestMethod]
    public async Task ToAsyncActionError3()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int>((a, b, c) => { Assert.AreEqual(
            1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); throw ex; })(1, 2,
            3).Materialize())).SequenceEqual([Notification.CreateOnError<Unit>(ex)]));
    }

    [TestMethod]
    public async Task ToAsyncAction4()
    {
        var hasRun = false;
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int>((a, b, c, d) =>
            { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(
            4, d); hasRun = true; })(1, 2, 3, 4))).SequenceEqual([new Unit()]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int>((a, b, c, d) =>
            { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(
            4, d); hasRun = true; }, DefaultScheduler)(1, 2, 3, 4))).SequenceEqual([new Unit()]));
        Assert.IsTrue(hasRun, "has run");
    }

    [TestMethod]
    public async Task ToAsyncActionError4()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int>((a, b, c, d) =>
            { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(
            4, d); throw ex; })(1, 2, 3, 4).Materialize())).SequenceEqual(
            [Notification.CreateOnError<Unit>(ex)]));
    }

    [TestMethod]
    public async Task ToAsyncAction5()
    {
        var hasRun = false;
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int>((a, b, c, d, e) =>
            { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(
            4, d); Assert.AreEqual(5, e); hasRun = true; })(1, 2, 3, 4, 5))).SequenceEqual(
            [new Unit()]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int>((a, b, c, d, e) =>
            { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(
            4, d); Assert.AreEqual(5, e); hasRun = true; }, DefaultScheduler)(1, 2, 3, 4,
            5))).SequenceEqual([new Unit()]));
        Assert.IsTrue(hasRun, "has run");
    }

    [TestMethod]
    public async Task ToAsyncActionError5()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int>((a, b, c, d, e) =>
            { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(
            4, d); Assert.AreEqual(5, e); throw ex; })(1, 2, 3, 4, 5).Materialize())).SequenceEqual(
            [Notification.CreateOnError<Unit>(ex)]));
    }

    [TestMethod]
    public async Task ToAsyncAction6()
    {
        var hasRun = false;
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int>((a, b, c, d, e,
            f) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3,
            c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6,
            f); hasRun = true; })(1, 2, 3, 4, 5, 6))).SequenceEqual([new Unit()]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int>((a, b, c, d, e,
            f) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3,
            c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6,
            f); hasRun = true; }, DefaultScheduler)(1, 2, 3, 4, 5, 6))).SequenceEqual(
            [new Unit()]));
        Assert.IsTrue(hasRun, "has run");
    }

    [TestMethod]
    public async Task ToAsyncActionError6()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int>((a, b, c, d, e,
            f) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3,
            c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); throw ex; })(1,
            2, 3, 4, 5, 6).Materialize())).SequenceEqual([Notification.CreateOnError<Unit>(ex)]));
    }

    [TestMethod]
    public async Task ToAsyncAction7()
    {
        var hasRun = false;
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int>((a, b, c, d,
            e, f, g) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3,
            c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6,
            f); Assert.AreEqual(7, g); hasRun = true; })(1, 2, 3, 4, 5, 6, 7))).SequenceEqual(
            [new Unit()]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int>((a, b, c, d,
            e, f, g) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3,
            c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6,
            f); Assert.AreEqual(7, g); hasRun = true; }, DefaultScheduler)(1, 2, 3, 4, 5, 6,
            7))).SequenceEqual([new Unit()]));
        Assert.IsTrue(hasRun, "has run");
    }

    [TestMethod]
    public async Task ToAsyncActionError7()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int>((a, b, c, d,
            e, f, g) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3,
            c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6,
            f); Assert.AreEqual(7, g); throw ex; })(1, 2, 3, 4, 5, 6,
            7).Materialize())).SequenceEqual([Notification.CreateOnError<Unit>(ex)]));
    }

    [TestMethod]
    public async Task ToAsyncAction8()
    {
        var hasRun = false;
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int>((a, b,
            c, d, e, f, g, h) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3,
            c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6,
            f); Assert.AreEqual(7, g); Assert.AreEqual(8, h); hasRun = true; })(1, 2, 3, 4, 5, 6, 7,
            8))).SequenceEqual([new Unit()]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int>((a, b,
            c, d, e, f, g, h) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3,
            c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6,
            f); Assert.AreEqual(7, g); Assert.AreEqual(8, h); hasRun = true; }, DefaultScheduler)(1,
            2, 3, 4, 5, 6, 7, 8))).SequenceEqual([new Unit()]));
        Assert.IsTrue(hasRun, "has run");
    }

    [TestMethod]
    public async Task ToAsyncActionError8()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int>((a, b,
            c, d, e, f, g, h) => { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3,
            c); Assert.AreEqual(4, d); Assert.AreEqual(5, e); Assert.AreEqual(6,
            f); Assert.AreEqual(7, g); Assert.AreEqual(8, h); throw ex; })(1, 2, 3, 4, 5, 6, 7,
            8).Materialize())).SequenceEqual([Notification.CreateOnError<Unit>(ex)]));
    }

    [TestMethod]
    public async Task ToAsyncAction9()
    {
        var hasRun = false;
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int>((
            a, b, c, d, e, f, g, h, i) => { Assert.AreEqual(1, a); Assert.AreEqual(2,
            b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5,
            e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8,
            h); Assert.AreEqual(9, i); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8, 9))).SequenceEqual(
            [new Unit()]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int>((
            a, b, c, d, e, f, g, h, i) => { Assert.AreEqual(1, a); Assert.AreEqual(2,
            b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5,
            e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8,
            h); Assert.AreEqual(9, i); hasRun = true; }, DefaultScheduler)(1, 2, 3, 4, 5, 6, 7, 8,
            9))).SequenceEqual([new Unit()]));
        Assert.IsTrue(hasRun, "has run");
    }

    [TestMethod]
    public async Task ToAsyncActionError9()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int>((
            a, b, c, d, e, f, g, h, i) => { Assert.AreEqual(1, a); Assert.AreEqual(2,
            b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5,
            e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8,
            h); Assert.AreEqual(9, i); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8,
            9).Materialize())).SequenceEqual([Notification.CreateOnError<Unit>(ex)]));
    }

    [TestMethod]
    public async Task ToAsyncAction10()
    {
        var hasRun = false;
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int>((a, b, c, d, e, f, g, h, i, j) => { Assert.AreEqual(1, a); Assert.AreEqual(2,
            b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5,
            e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8,
            h); Assert.AreEqual(9, i); Assert.AreEqual(10, j); hasRun = true; })(1, 2, 3, 4, 5, 6,
            7, 8, 9, 10))).SequenceEqual([new Unit()]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int>((a, b, c, d, e, f, g, h, i, j) => { Assert.AreEqual(1, a); Assert.AreEqual(2,
            b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5,
            e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8,
            h); Assert.AreEqual(9, i); Assert.AreEqual(10, j); hasRun = true; }, DefaultScheduler)(
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10))).SequenceEqual([new Unit()]));
        Assert.IsTrue(hasRun, "has run");
    }

    [TestMethod]
    public async Task ToAsyncActionError10()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int>((a, b, c, d, e, f, g, h, i, j) => { Assert.AreEqual(1, a); Assert.AreEqual(2,
            b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5,
            e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8,
            h); Assert.AreEqual(9, i); Assert.AreEqual(10, j); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8,
            9, 10).Materialize())).SequenceEqual([Notification.CreateOnError<Unit>(ex)]));
    }

    [TestMethod]
    public async Task ToAsyncAction11()
    {
        var hasRun = false;
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int>((a, b, c, d, e, f, g, h, i, j, k) => { Assert.AreEqual(1, a); Assert.AreEqual(
            2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5,
            e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8,
            h); Assert.AreEqual(9, i); Assert.AreEqual(10, j); Assert.AreEqual(11,
            k); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11))).SequenceEqual([new Unit()]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int>((a, b, c, d, e, f, g, h, i, j, k) => { Assert.AreEqual(1, a); Assert.AreEqual(
            2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5,
            e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8,
            h); Assert.AreEqual(9, i); Assert.AreEqual(10, j); Assert.AreEqual(11,
            k); hasRun = true; }, DefaultScheduler)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
            11))).SequenceEqual([new Unit()]));
        Assert.IsTrue(hasRun, "has run");
    }

    [TestMethod]
    public async Task ToAsyncActionError11()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int>((a, b, c, d, e, f, g, h, i, j, k) => { Assert.AreEqual(1, a); Assert.AreEqual(
            2, b); Assert.AreEqual(3, c); Assert.AreEqual(4, d); Assert.AreEqual(5,
            e); Assert.AreEqual(6, f); Assert.AreEqual(7, g); Assert.AreEqual(8,
            h); Assert.AreEqual(9, i); Assert.AreEqual(10, j); Assert.AreEqual(11, k); throw ex; })(
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11).Materialize())).SequenceEqual(
            [Notification.CreateOnError<Unit>(ex)]));
    }

    [TestMethod]
    public async Task ToAsyncAction12()
    {
        var hasRun = false;
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) => { Assert.AreEqual(1,
            a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4,
            d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7,
            g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10,
            j); Assert.AreEqual(11, k); Assert.AreEqual(12, l); hasRun = true; })(1, 2, 3, 4, 5, 6,
            7, 8, 9, 10, 11, 12))).SequenceEqual([new Unit()]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) => { Assert.AreEqual(1,
            a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4,
            d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7,
            g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10,
            j); Assert.AreEqual(11, k); Assert.AreEqual(12, l); hasRun = true; }, DefaultScheduler)(
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12))).SequenceEqual([new Unit()]));
        Assert.IsTrue(hasRun, "has run");
    }

    [TestMethod]
    public async Task ToAsyncActionError12()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) => { Assert.AreEqual(1,
            a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4,
            d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7,
            g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10,
            j); Assert.AreEqual(11, k); Assert.AreEqual(12, l); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8,
            9, 10, 11, 12).Materialize())).SequenceEqual([Notification.CreateOnError<Unit>(ex)]));
    }

    [TestMethod]
    public async Task ToAsyncAction13()
    {
        var hasRun = false;
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) => { Assert.AreEqual(1,
            a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4,
            d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7,
            g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10,
            j); Assert.AreEqual(11, k); Assert.AreEqual(12, l); Assert.AreEqual(13,
            m); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13))).SequenceEqual(
            [new Unit()]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) => { Assert.AreEqual(1,
            a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4,
            d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7,
            g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10,
            j); Assert.AreEqual(11, k); Assert.AreEqual(12, l); Assert.AreEqual(13,
            m); hasRun = true; }, DefaultScheduler)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
            13))).SequenceEqual([new Unit()]));
        Assert.IsTrue(hasRun, "has run");
    }

    [TestMethod]
    public async Task ToAsyncActionError13()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) => { Assert.AreEqual(1,
            a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(4,
            d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7,
            g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10,
            j); Assert.AreEqual(11, k); Assert.AreEqual(12, l); Assert.AreEqual(13,
            m); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
            13).Materialize())).SequenceEqual([Notification.CreateOnError<Unit>(ex)]));
    }

    [TestMethod]
    public async Task ToAsyncAction14()
    {
        var hasRun = false;
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) =>
            { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(
            4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7,
            g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10,
            j); Assert.AreEqual(11, k); Assert.AreEqual(12, l); Assert.AreEqual(13,
            m); Assert.AreEqual(14, n); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13,
            14))).SequenceEqual([new Unit()]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) =>
            { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(
            4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7,
            g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10,
            j); Assert.AreEqual(11, k); Assert.AreEqual(12, l); Assert.AreEqual(13,
            m); Assert.AreEqual(14, n); hasRun = true; }, DefaultScheduler)(1, 2, 3, 4, 5, 6, 7, 8,
            9, 10, 11, 12, 13, 14))).SequenceEqual([new Unit()]));
        Assert.IsTrue(hasRun, "has run");
    }

    [TestMethod]
    public async Task ToAsyncActionError14()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) =>
            { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(
            4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7,
            g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10,
            j); Assert.AreEqual(11, k); Assert.AreEqual(12, l); Assert.AreEqual(13,
            m); Assert.AreEqual(14, n); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13,
            14).Materialize())).SequenceEqual([Notification.CreateOnError<Unit>(ex)]));
    }

    [TestMethod]
    public async Task ToAsyncAction15()
    {
        var hasRun = false;
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) =>
            { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(
            4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7,
            g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10,
            j); Assert.AreEqual(11, k); Assert.AreEqual(12, l); Assert.AreEqual(13,
            m); Assert.AreEqual(14, n); Assert.AreEqual(15, o); hasRun = true; })(1, 2, 3, 4, 5, 6,
            7, 8, 9, 10, 11, 12, 13, 14, 15))).SequenceEqual([new Unit()]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) =>
            { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(
            4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7,
            g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10,
            j); Assert.AreEqual(11, k); Assert.AreEqual(12, l); Assert.AreEqual(13,
            m); Assert.AreEqual(14, n); Assert.AreEqual(15, o); hasRun = true; }, DefaultScheduler)(
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15))).SequenceEqual([new Unit()]));
        Assert.IsTrue(hasRun, "has run");
    }

    [TestMethod]
    public async Task ToAsyncActionError15()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) =>
            { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(
            4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7,
            g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10,
            j); Assert.AreEqual(11, k); Assert.AreEqual(12, l); Assert.AreEqual(13,
            m); Assert.AreEqual(14, n); Assert.AreEqual(15, o); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8,
            9, 10, 11, 12, 13, 14, 15).Materialize())).SequenceEqual(
            [Notification.CreateOnError<Unit>(ex)]));
    }

    [TestMethod]
    public async Task ToAsyncAction16()
    {
        var hasRun = false;
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) =>
            { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(
            4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7,
            g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10,
            j); Assert.AreEqual(11, k); Assert.AreEqual(12, l); Assert.AreEqual(13,
            m); Assert.AreEqual(14, n); Assert.AreEqual(15, o); Assert.AreEqual(16,
            p); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
            16))).SequenceEqual([new Unit()]));
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) =>
            { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(
            4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7,
            g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10,
            j); Assert.AreEqual(11, k); Assert.AreEqual(12, l); Assert.AreEqual(13,
            m); Assert.AreEqual(14, n); Assert.AreEqual(15, o); Assert.AreEqual(16,
            p); hasRun = true; }, DefaultScheduler)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14,
            15, 16))).SequenceEqual([new Unit()]));
        Assert.IsTrue(hasRun, "has run");
    }

    [TestMethod]
    public async Task ToAsyncActionError16()
    {
        var ex = new Exception();
        Assert.IsTrue((await ToListAsync(Seq.ToAsync<int, int, int, int, int, int, int, int, int,
            int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) =>
            { Assert.AreEqual(1, a); Assert.AreEqual(2, b); Assert.AreEqual(3, c); Assert.AreEqual(
            4, d); Assert.AreEqual(5, e); Assert.AreEqual(6, f); Assert.AreEqual(7,
            g); Assert.AreEqual(8, h); Assert.AreEqual(9, i); Assert.AreEqual(10,
            j); Assert.AreEqual(11, k); Assert.AreEqual(12, l); Assert.AreEqual(13,
            m); Assert.AreEqual(14, n); Assert.AreEqual(15, o); Assert.AreEqual(16,
            p); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
            16).Materialize())).SequenceEqual([Notification.CreateOnError<Unit>(ex)]));
    }
}
