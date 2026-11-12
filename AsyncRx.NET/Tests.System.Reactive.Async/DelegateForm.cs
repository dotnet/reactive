// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Async;

/// <summary>Which of AsyncRx.NET's two delegate overloads the target calls.</summary>
/// <remarks>
/// <para>
/// Most AsyncRx.NET operators that take a delegate come in two forms, one whose delegate returns
/// its result directly, as Rx.NET's does, and one whose delegate returns it through a
/// <see cref="ValueTask{TResult}"/>. The shared scenarios are Rx.NET's, so they write the first
/// form; this chooses which form the target reaches with them. Under
/// <see cref="Asynchronous"/> the target wraps each scenario delegate so that it returns its
/// result through a <see cref="ValueTask{TResult}"/> after the test scheduler's yield point, and
/// calls the library's asynchronous overload with it. The scenario's expectations are unchanged,
/// because the library's result must not depend on which overload was used.
/// </para>
/// <para>
/// This is the second axis of an AsyncRx.NET runner, orthogonal to the
/// <see cref="Microsoft.Reactive.Testing.Async.ExecutionShape"/>: the shape says whether the
/// harness's await points suspend, and under <see cref="Asynchronous"/> the wrapped delegate's
/// own await is one of them, so the two shapes exercise both the synchronous and the suspending
/// path through the library's <c>await predicate(x)</c>.
/// </para>
/// </remarks>
public enum DelegateForm
{
    /// <summary>The target calls the overload whose delegate returns its result directly.</summary>
    Synchronous,

    /// <summary>
    /// The target calls the overload whose delegate returns its result through a
    /// <see cref="ValueTask{TResult}"/>.
    /// </summary>
    Asynchronous,
}
