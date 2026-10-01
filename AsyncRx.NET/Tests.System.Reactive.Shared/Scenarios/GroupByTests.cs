// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Text;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>GroupBy</c> scenarios.</summary>
/// <remarks>
/// So far <c>GroupBy_Outer_Complete</c> from Rx.NET's <c>GroupByTest.cs</c>, with a standard
/// comparer in place of its scheduler-driven throwing one, and one synthetic scenario,
/// <c>GroupBy_Where_OnKey</c>, which applies a generic operator to the sequence of groups. The
/// rest of that file is still to be migrated.
/// </remarks>
public abstract class GroupByTests : SharedReactiveTest
{
    [TestMethod]
    public void GroupBy_Outer_Complete()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var res = Scheduler.Start(() =>
            xs.GroupBy(x => x.Trim(), x => Reverse(x), StringComparer.OrdinalIgnoreCase).Select(g => g.Key)
        );

        res.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "baR"),
            OnNext(350, "Baz"),
            OnNext(360, "qux"),
            OnCompleted<string>(570)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );
    }

    [TestMethod]
    public void GroupBy_Where_OnKey()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnCompleted<string>(570)
        );

        var res = Scheduler.Start(() =>
            xs.GroupBy(x => x.Trim(), x => Reverse(x), StringComparer.OrdinalIgnoreCase).Where(g => g.Key != "qux").Select(g => g.Key)
        );

        res.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "baR"),
            OnNext(350, "Baz"),
            OnCompleted<string>(570)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );
    }

    private static string Reverse(string s)
    {
        var sb = new StringBuilder();

        for (var i = s.Length - 1; i >= 0; i--)
        {
            sb.Append(s[i]);
        }

        return sb.ToString();
    }
}
