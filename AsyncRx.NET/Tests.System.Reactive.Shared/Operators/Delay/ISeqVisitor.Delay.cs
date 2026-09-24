// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    object DelayTime<T>(DelayTimeSeq<T> seq);

    object DelayAbsolute<T>(DelayAbsoluteSeq<T> seq);

    object DelaySelector<T, TDelay>(DelaySelectorSeq<T, TDelay> seq);

    object DelaySubscription<T, TDelay>(DelaySubscriptionSeq<T, TDelay> seq);
}
