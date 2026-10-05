// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reflection;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>The argument-validation stratum, one test per operator, over the API surface.</summary>
/// <remarks>
/// <para>
/// Where Rx.NET has a hand-written <c>*_ArgumentChecking</c> test per operator, each test here
/// is one line: <see cref="ArgumentChecks"/> walks the operator's overloads on the running
/// target's surface and applies the rules.
/// </para>
/// <para>
/// This class declares the 122 operator names both surfaces have, and the 17 that only one
/// has. The latter are inconclusive on the target that lacks them, on purpose: each is an
/// unresolved difference between the libraries, to be looked at one by one as the operators'
/// behavioural tests are reached, and the skip is the reminder. The only one-sided names not
/// here are the pairs that differ only in name (<c>AsObservable</c>, <c>ToObservable</c> and
/// <c>SubscribeSafe</c> against <c>AsAsyncObservable</c>, <c>ToAsyncObservable</c> and
/// <c>SubscribeSafeAsync</c>; the handler-based <c>SubscribeAsync</c> family against the same
/// family under Rx.NET's <c>Subscribe</c>; <c>UsingAsync</c> against Rx.NET's asynchronous
/// <c>Using</c> overload), which each target's derived class declares as its own, since nothing
/// is unresolved about them. <see cref="EveryOperatorHasATest"/> keeps the lists
/// complete: it fails on a target whose surface has a public static method with no test, so a
/// new operator gets its argument checks the moment it is added.
/// </para>
/// </remarks>
public abstract class ArgumentCheckingTests
{
    /// <summary>The surface of the target under test.</summary>
    protected abstract IApiSurface Surface { get; }

    [TestMethod]
    public void EveryOperatorHasATest()
    {
        // A method on either class is covered by a test named for it, or by one scoped to its
        // class: Subscribe_Operators_ArgumentChecking covers Subscribe on the operator class.
        var tests = GetType().GetMethods().Select(m => m.Name).ToHashSet(StringComparer.Ordinal);
        var missing = new List<string>();
        foreach (var scope in new[] { SurfaceClass.Operators, SurfaceClass.Extensions })
        {
            var type = ArgumentChecks.Classes(Surface, scope)[0];
            missing.AddRange(type
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Select(m => m.Name)
                .Distinct()
                .Where(n => !tests.Contains(n + "_ArgumentChecking") && !tests.Contains($"{n}_{scope}_ArgumentChecking"))
                .Select(n => $"{type.Name}.{n}"));
        }

        Assert.IsEmpty(missing, "Operators with no argument-checking test: " + string.Join(", ", missing.Order()));
    }

    // ---- Operators both targets have ----

    [TestMethod]
    public void Aggregate_ArgumentChecking() => Check("Aggregate");

    [TestMethod]
    public void All_ArgumentChecking() => Check("All");

    [TestMethod]
    public void Amb_ArgumentChecking() => Check("Amb");

    [TestMethod]
    public void And_ArgumentChecking() => Check("And");

    [TestMethod]
    public void Any_ArgumentChecking() => Check("Any");

    [TestMethod]
    public void Append_ArgumentChecking() => Check("Append");

    [TestMethod]
    public void Average_ArgumentChecking() => Check("Average");

    [TestMethod]
    public void Buffer_ArgumentChecking() => Check("Buffer");

    [TestMethod]
    public void Case_ArgumentChecking() => Check("Case");

    [TestMethod]
    public void Cast_ArgumentChecking() => Check("Cast");

    [TestMethod]
    public void Catch_ArgumentChecking() => Check("Catch");

    [TestMethod]
    public void CombineLatest_ArgumentChecking() => Check("CombineLatest");

    [TestMethod]
    public void Concat_ArgumentChecking() => Check("Concat");

    [TestMethod]
    public void Contains_ArgumentChecking() => Check("Contains");

    [TestMethod]
    public void Count_ArgumentChecking() => Check("Count");

    [TestMethod]
    public void Create_ArgumentChecking() => Check("Create");

    [TestMethod]
    public void DefaultIfEmpty_ArgumentChecking() => Check("DefaultIfEmpty");

    [TestMethod]
    public void Defer_ArgumentChecking() => Check("Defer");

    [TestMethod]
    public void DeferAsync_ArgumentChecking() => Check("DeferAsync");

    [TestMethod]
    public void Delay_ArgumentChecking() => Check("Delay");

    [TestMethod]
    public void DelaySubscription_ArgumentChecking() => Check("DelaySubscription");

    [TestMethod]
    public void Dematerialize_ArgumentChecking() => Check("Dematerialize");

    [TestMethod]
    public void Distinct_ArgumentChecking() => Check("Distinct");

    [TestMethod]
    public void DistinctUntilChanged_ArgumentChecking() => Check("DistinctUntilChanged");

    [TestMethod]
    public void Do_ArgumentChecking() => Check("Do");

    [TestMethod]
    public void DoWhile_ArgumentChecking() => Check("DoWhile");

    [TestMethod]
    public void ElementAt_ArgumentChecking() => Check("ElementAt");

    [TestMethod]
    public void ElementAtOrDefault_ArgumentChecking() => Check("ElementAtOrDefault");

    [TestMethod]
    public void Empty_ArgumentChecking() => Check("Empty");

    [TestMethod]
    public void Finally_ArgumentChecking() => Check("Finally");

    [TestMethod]
    public void First_ArgumentChecking() => Check("First");

    [TestMethod]
    public void FirstAsync_ArgumentChecking() => Check("FirstAsync");

    [TestMethod]
    public void FirstOrDefault_ArgumentChecking() => Check("FirstOrDefault");

    [TestMethod]
    public void FirstOrDefaultAsync_ArgumentChecking() => Check("FirstOrDefaultAsync");

    [TestMethod]
    public void For_ArgumentChecking() => Check("For");

    [TestMethod]
    public void ForEachAsync_ArgumentChecking() => Check("ForEachAsync");

    [TestMethod]
    public void FromAsync_ArgumentChecking() => Check("FromAsync");

    [TestMethod]
    public void FromAsyncPattern_ArgumentChecking() => Check("FromAsyncPattern");

    [TestMethod]
    public void FromEvent_ArgumentChecking() => Check("FromEvent");

    [TestMethod]
    public void FromEventPattern_ArgumentChecking() => Check("FromEventPattern");

    [TestMethod]
    public void Generate_ArgumentChecking() => Check("Generate");

    [TestMethod]
    public void GetAwaiter_ArgumentChecking() => Check("GetAwaiter");

    [TestMethod]
    public void GroupBy_ArgumentChecking() => Check("GroupBy");

    [TestMethod]
    public void GroupByUntil_ArgumentChecking() => Check("GroupByUntil");

    [TestMethod]
    public void GroupJoin_ArgumentChecking() => Check("GroupJoin");

    [TestMethod]
    public void If_ArgumentChecking() => Check("If");

    [TestMethod]
    public void IgnoreElements_ArgumentChecking() => Check("IgnoreElements");

    [TestMethod]
    public void Interval_ArgumentChecking() => Check("Interval");

    [TestMethod]
    public void IsEmpty_ArgumentChecking() => Check("IsEmpty");

    [TestMethod]
    public void Join_ArgumentChecking() => Check("Join");

    [TestMethod]
    public void Last_ArgumentChecking() => Check("Last");

    [TestMethod]
    public void LastAsync_ArgumentChecking() => Check("LastAsync");

    [TestMethod]
    public void LastOrDefault_ArgumentChecking() => Check("LastOrDefault");

    [TestMethod]
    public void LastOrDefaultAsync_ArgumentChecking() => Check("LastOrDefaultAsync");

    [TestMethod]
    public void LongCount_ArgumentChecking() => Check("LongCount");

    [TestMethod]
    public void Materialize_ArgumentChecking() => Check("Materialize");

    [TestMethod]
    public void Max_ArgumentChecking() => Check("Max");

    [TestMethod]
    public void MaxBy_ArgumentChecking() => Check("MaxBy");

    [TestMethod]
    public void Merge_ArgumentChecking() => Check("Merge");

    [TestMethod]
    public void Min_ArgumentChecking() => Check("Min");

    [TestMethod]
    public void MinBy_ArgumentChecking() => Check("MinBy");

    [TestMethod]
    public void Multicast_ArgumentChecking() => Check("Multicast");

    [TestMethod]
    public void Never_ArgumentChecking() => Check("Never");

    [TestMethod]
    public void ObserveOn_ArgumentChecking() => Check("ObserveOn");

    [TestMethod]
    public void OfType_ArgumentChecking() => Check("OfType");

    [TestMethod]
    public void OnErrorResumeNext_ArgumentChecking() => Check("OnErrorResumeNext");

    [TestMethod]
    public void Prepend_ArgumentChecking() => Check("Prepend");

    [TestMethod]
    public void Publish_ArgumentChecking() => Check("Publish");

    [TestMethod]
    public void PublishLast_ArgumentChecking() => Check("PublishLast");

    [TestMethod]
    public void Range_ArgumentChecking() => Check("Range");

    [TestMethod]
    public void RefCount_ArgumentChecking() => Check("RefCount");

    [TestMethod]
    public void Repeat_ArgumentChecking() => Check("Repeat");

    [TestMethod]
    public void Replay_ArgumentChecking() => Check("Replay");

    [TestMethod]
    public void Retry_ArgumentChecking() => Check("Retry");

    [TestMethod]
    public void Return_ArgumentChecking() => Check("Return");

    [TestMethod]
    public void RunAsync_ArgumentChecking() => Check("RunAsync");

    [TestMethod]
    public void Sample_ArgumentChecking() => Check("Sample");

    [TestMethod]
    public void Scan_ArgumentChecking() => Check("Scan");

    [TestMethod]
    public void Select_ArgumentChecking() => Check("Select");

    [TestMethod]
    public void SelectMany_ArgumentChecking() => Check("SelectMany");

    [TestMethod]
    public void SequenceEqual_ArgumentChecking() => Check("SequenceEqual");

    [TestMethod]
    public void Single_ArgumentChecking() => Check("Single");

    [TestMethod]
    public void SingleAsync_ArgumentChecking() => Check("SingleAsync");

    [TestMethod]
    public void SingleOrDefault_ArgumentChecking() => Check("SingleOrDefault");

    [TestMethod]
    public void SingleOrDefaultAsync_ArgumentChecking() => Check("SingleOrDefaultAsync");

    [TestMethod]
    public void Skip_ArgumentChecking() => Check("Skip");

    [TestMethod]
    public void SkipLast_ArgumentChecking() => Check("SkipLast");

    [TestMethod]
    public void SkipUntil_ArgumentChecking() => Check("SkipUntil");

    [TestMethod]
    public void SkipWhile_ArgumentChecking() => Check("SkipWhile");

    [TestMethod]
    public void Start_ArgumentChecking() => Check("Start");

    [TestMethod]
    public void StartAsync_ArgumentChecking() => Check("StartAsync");

    [TestMethod]
    public void StartWith_ArgumentChecking() => Check("StartWith");

    [TestMethod]
    public void SubscribeOn_ArgumentChecking() => Check("SubscribeOn");

    [TestMethod]
    public void Sum_ArgumentChecking() => Check("Sum");

    [TestMethod]
    public void Switch_ArgumentChecking() => Check("Switch");

    [TestMethod]
    public void Synchronize_ArgumentChecking() => Check("Synchronize");

    [TestMethod]
    public void Take_ArgumentChecking() => Check("Take");

    [TestMethod]
    public void TakeLast_ArgumentChecking() => Check("TakeLast");

    [TestMethod]
    public void TakeLastBuffer_ArgumentChecking() => Check("TakeLastBuffer");

    [TestMethod]
    public void TakeUntil_ArgumentChecking() => Check("TakeUntil");

    [TestMethod]
    public void TakeWhile_ArgumentChecking() => Check("TakeWhile");

    [TestMethod]
    public void Then_ArgumentChecking() => Check("Then");

    [TestMethod]
    public void Throttle_ArgumentChecking() => Check("Throttle");

    [TestMethod]
    public void Throw_ArgumentChecking() => Check("Throw");

    [TestMethod]
    public void TimeInterval_ArgumentChecking() => Check("TimeInterval");

    [TestMethod]
    public void Timeout_ArgumentChecking() => Check("Timeout");

    [TestMethod]
    public void Timer_ArgumentChecking() => Check("Timer");

    [TestMethod]
    public void Timestamp_ArgumentChecking() => Check("Timestamp");

    [TestMethod]
    public void ToArray_ArgumentChecking() => Check("ToArray");

    [TestMethod]
    public void ToAsync_ArgumentChecking() => Check("ToAsync");

    [TestMethod]
    public void ToDictionary_ArgumentChecking() => Check("ToDictionary");

    [TestMethod]
    public void ToEvent_ArgumentChecking() => Check("ToEvent");

    [TestMethod]
    public void ToEventPattern_ArgumentChecking() => Check("ToEventPattern");

    [TestMethod]
    public void ToList_ArgumentChecking() => Check("ToList");

    [TestMethod]
    public void ToLookup_ArgumentChecking() => Check("ToLookup");

    [TestMethod]
    public void Using_ArgumentChecking() => Check("Using");

    [TestMethod]
    public void When_ArgumentChecking() => Check("When");

    [TestMethod]
    public void Where_ArgumentChecking() => Check("Where");

    [TestMethod]
    public void While_ArgumentChecking() => Check("While");

    [TestMethod]
    public void Window_ArgumentChecking() => Check("Window");

    [TestMethod]
    public void WithLatestFrom_ArgumentChecking() => Check("WithLatestFrom");

    [TestMethod]
    public void Zip_ArgumentChecking() => Check("Zip");

    // ---- Operators only Rx.NET has: unresolved differences, inconclusive on AsyncRx.NET ----

    [TestMethod]
    public void AutoConnect_ArgumentChecking() => CheckIfPresent("AutoConnect");

    [TestMethod]
    public void Chunkify_ArgumentChecking() => CheckIfPresent("Chunkify");

    [TestMethod]
    public void Collect_ArgumentChecking() => CheckIfPresent("Collect");

    [TestMethod]
    public void ForEach_ArgumentChecking() => CheckIfPresent("ForEach");

    [TestMethod]
    public void GetEnumerator_ArgumentChecking() => CheckIfPresent("GetEnumerator");

    [TestMethod]
    public void Latest_ArgumentChecking() => CheckIfPresent("Latest");

    [TestMethod]
    public void MostRecent_ArgumentChecking() => CheckIfPresent("MostRecent");

    [TestMethod]
    public void Next_ArgumentChecking() => CheckIfPresent("Next");

    [TestMethod]
    public void RepeatWhen_ArgumentChecking() => CheckIfPresent("RepeatWhen");

    // New in Rx.NET 7.0: resets the exception dispatch state captured by Rx's rethrow helpers.
    [TestMethod]
    public void ResetExceptionDispatchState_ArgumentChecking() => CheckIfPresent("ResetExceptionDispatchState");

    [TestMethod]
    public void RetryWhen_ArgumentChecking() => CheckIfPresent("RetryWhen");

    // Rx.NET's enumerable Subscribe, a shortcut for ToObservable(scheduler).Subscribe(observer);
    // its handler-based Subscribe family lives on the extensions class and is paired with
    // AsyncRx.NET's SubscribeAsync in the derived classes.
    [TestMethod]
    public void Subscribe_Operators_ArgumentChecking() => CheckIfPresent("Subscribe", SurfaceClass.Operators);

    [TestMethod]
    public void ToEnumerable_ArgumentChecking() => CheckIfPresent("ToEnumerable");

    [TestMethod]
    public void Wait_ArgumentChecking() => CheckIfPresent("Wait");

    // ---- Operators only AsyncRx.NET has: unresolved differences, inconclusive on Rx.NET ----

    [TestMethod]
    public void ToHashSet_ArgumentChecking() => CheckIfPresent("ToHashSet");

    [TestMethod]
    public void UsingAwait_ArgumentChecking() => CheckIfPresent("UsingAwait");

    [TestMethod]
    public void UsingAwaitAsync_ArgumentChecking() => CheckIfPresent("UsingAwaitAsync");

    /// <summary>Runs the argument checks over every overload of an operator.</summary>
    /// <param name="operatorName">The operator's method name.</param>
    /// <param name="scope">Which of the surface's classes to look at.</param>
    /// <remarks>
    /// An operator with no overloads on the surface is a failure: the declaring class claims it
    /// is there. For the operators both targets have, and for a derived class to declare the
    /// ones its target has under its own name.
    /// </remarks>
    protected void Check(string operatorName, SurfaceClass scope = SurfaceClass.Any)
    {
        if (ArgumentChecks.Overloads(Surface, operatorName, scope).Count == 0)
        {
            Assert.Fail($"{operatorName} has no overloads on this target's surface.");
        }

        var problems = ArgumentChecks.Check(Surface, operatorName, scope);

        Assert.IsEmpty(problems, string.Join(Environment.NewLine, problems));
    }

    /// <summary>
    /// As <see cref="Check"/>, but inconclusive on a target that does not have the operator.
    /// </summary>
    /// <param name="operatorName">The operator's method name.</param>
    /// <param name="scope">Which of the surface's classes to look at.</param>
    /// <remarks>For the operators only one library has, while that is unresolved.</remarks>
    protected void CheckIfPresent(string operatorName, SurfaceClass scope = SurfaceClass.Any)
    {
        if (ArgumentChecks.Overloads(Surface, operatorName, scope).Count == 0)
        {
            Assert.Inconclusive($"{operatorName} is not on this target's surface: an unresolved difference between the libraries.");
        }

        Check(operatorName, scope);
    }
}
