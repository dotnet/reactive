// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq;

/// <summary>
/// 17-argument generic function delegate, because the .NET runtime library only goes up to 16.
/// </summary>
/// <remarks>
/// The code generation for <see cref="Qbservable"/> and friends uses delegates to obtain the
/// method info required for expression tree generation, because the trimmer is able to process
/// code that works this way. (We used to use <c>GetCurrentMethod</c>, but the trimmer doesn't
/// support that very well, and we get warnings from the trimmability analyzer if we do that.)
/// The .NET runtime library only provides <c>Func</c> delegates up to 16 arguments. There are
/// various operators Rx also defines for up to 16 type arguments, but a handful of these then
/// take an additional selector function argument, meaning we need a <c>Func</c> delegate with
/// 17 type arguments. Since the .NET runtime doesn't define such a thing, we define it here.
/// </remarks>
internal delegate TResult Func<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15, in T16, in T17, out TResult>(
    T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17);
